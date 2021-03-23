using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureInven
{
    private Equipment[] Inventory = new Equipment[6];

    public bool EmptySlot(int type)
    {
        return null == Inventory[type] ? true : false;
    }

    public Equipment OnEquipSlot(Equipment equipment, Equipment.PartsType type)
    {
        int InvenIndex = (int)type;

        if(EmptySlot(InvenIndex))
        {
            Inventory[InvenIndex] = equipment;
        }

        else
        {
            return ChangeSlot(equipment, InvenIndex);
        }

        return null;
    }

    public Equipment OffEEquipSlot(Equipment.PartsType type)
    {
        int InvenIndex = (int)type;
        return ChangeSlot(null, InvenIndex);
    }

    private Equipment ChangeSlot(Equipment equipment, int type)
    {
        Equipment tmp = Inventory[type];
        Inventory[type] = equipment;

        return tmp;
    }
}

public class Creature : MonoBehaviour
{
    protected enum State { Idle, Walk, Chase, Hit, Attack, Dead };
    protected enum UnitType { Warrior, Magician, Arhcer, Priest };

    // event
    public event Action onDeath;

    // Item
    protected CreatureInven MyInventory;
    public Weapon MyWeapon;

    // enum
    protected State CurrentState { get; set; } = State.Idle;
    protected UnitType CurrentUnitType { get; set; } = UnitType.Warrior;


    // status
    protected int Level = 0;
    public float HitPoint { get; set; } = 1000.0f;
    protected float Mana = 100.0f;
    protected float AttackDamage = 100.0f;
    protected float Armor = 0.0f;
    protected float AttackRange = 1.75f;
    protected float AttackDelay = 0.75f;
    protected float AttackSpeed = 1.0f;
    protected float MoveSpeed = 2.0f;

    // specific status
    protected float HitDelay = 0.0f;
    protected int AttackCount = 0;
    protected int AttackCountMax = 1;
    protected int AttackComboCount = 0;
    protected float AttackTime = 0;

    // bool
    public bool Dead { get; set; } = false;
    protected bool canAttack { get; set; } = true;
    protected bool isHit { get; set; } = false;

    //
    public ParticleSystem bloodEffect;
    public ParticleSystem hitEffect;

    // component
    protected NavMeshAgent pathFinder; // 경로계산 AI 에이전트
    protected Animator animator;
    protected Rigidbody rigidbody;

    // target
    public LayerMask mask;
    protected Creature target = null;
    protected bool hasTarget = false;
    protected float DetectRange = 100.0f;


    protected void TargetLookAt()
    {
        Vector3 tmpDir = target.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(tmpDir), Time.deltaTime * 3.0f);
    }

    protected float CheckDistance(Vector3 position, Vector3 TargetPosition)
    {
        return Vector3.Distance(position, TargetPosition);
    }

    public virtual void OnDie()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        hasTarget = false;
        CurrentState = State.Dead;
        animator.SetTrigger("Dead");
        Dead = true;

        Collider[] MyColliders = GetComponents<Collider>();
        for (int i = 0; i < MyColliders.Length; i++)
        {
            MyColliders[i].enabled = false;
        }

        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.isKinematic = true;

        pathFinder.updatePosition = false;
        pathFinder.updateRotation = false;
        pathFinder.velocity = Vector3.zero;
        pathFinder.isStopped = true;
        pathFinder.enabled = false;
    }

    public virtual void OnDamage(Vector3 hitPoint, Vector3 hitNormal, float damage)
    {
        bloodEffect.transform.position = hitPoint;
        bloodEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        bloodEffect.Play();

        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();

        HitPoint -= damage;

        if (0 >= HitPoint && !Dead)
        {
            OnDie();
        }
    }

    protected void AttackTimer()
    {
        if (Dead || CurrentState == State.Attack)
            return;

        if(!canAttack && !animator.GetBool("isAttack"))
        {
            AttackTime += Time.deltaTime; 

            if(AttackTime >= AttackDelay)
            {
                AttackTime = 0.0f;
                canAttack = true;

                if (MyWeapon != null)
                    MyWeapon.SetAttackEnable(true);
            }
        }
    }

    protected virtual IEnumerator OnAttack()
    {
        if (null == target || target.Dead)
        {
            CurrentState = State.Idle;
            hasTarget = false;
        }

        yield return null;
    }

    protected void OnChase()
    {
        if (hasTarget)
        {
            if(null == target || target.Dead)
            {
                animator.SetBool("isWalk", false);
                animator.SetBool("isAttack", false);
                CurrentState = State.Idle;
                target = null;
                hasTarget = false;
            }

            else
            {
                TargetLookAt();

                if (AttackRange < pathFinder.remainingDistance)
                {
                    ReFindTarget();

                    pathFinder.updatePosition = true;
                    pathFinder.updateRotation = true;
                    pathFinder.isStopped = false;
                    pathFinder.SetDestination(target.transform.position);
                    animator.SetBool("isWalk", true);
                }

                else
                {
                    pathFinder.updatePosition = false;
                    pathFinder.updateRotation = false;
                    pathFinder.isStopped = true;
                    pathFinder.velocity = Vector3.zero;
                    animator.SetBool("isWalk", false);

                    if (canAttack)
                    {
                        CurrentState = State.Attack;
                    }

                    else
                    {
                        CurrentState = State.Idle;
                    }
                }
            }
        }

        else
        {
            CurrentState = State.Idle;
            hasTarget = false;
        }
    }

    protected void ReFindTarget()
    {
        Collider[] colliders =
                    Physics.OverlapSphere(transform.position, DetectRange, mask);

        float distance = 9999.0f;
        Creature tmpTargetSave = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            Creature tmpTarget = colliders[i].GetComponent<Creature>();

            if (null != tmpTarget && !tmpTarget.Dead)
            {
                float tmpDistance = CheckDistance(transform.position, tmpTarget.transform.position);
                if (distance > tmpDistance)
                {
                    tmpTargetSave = tmpTarget;
                    distance = tmpDistance;
                }
            }

            else
            {
                continue;
            }
        }

        if(null == tmpTargetSave)
        {
            target = null;
            hasTarget = false;
            rigidbody.angularVelocity = Vector3.zero;
        }

        else if (null != tmpTargetSave && !tmpTargetSave.Dead)
        {
            target = tmpTargetSave;
            hasTarget = true;
            CurrentState = State.Chase;
            pathFinder.SetDestination(target.transform.position);

            if (MyWeapon != null)
                MyWeapon.SetTargetLayer(target.gameObject.layer);
        }
    }

    protected void FindTarget()
    {
        if(!hasTarget)
        {
            ReFindTarget();
        }

        else
        {
            if (target != null)
            {
                CurrentState = true == target.Dead ? State.Idle : State.Chase;

                if (target.Dead)
                {
                    animator.SetBool("isWalk", false);
                    animator.SetBool("isAttack", false);
                    CurrentState = State.Idle;
                    target = null;
                    hasTarget = false;
                }
            }

            else
            {
                animator.SetBool("isWalk", false);
                animator.SetBool("isAttack", false);
                CurrentState = State.Idle;
                target = null;
                hasTarget = false;
            }
        }
    }

    public void CheckState()
    {
        if (Dead)
            return;

        switch (CurrentState)
        {
            case State.Idle:
                {
                    FindTarget();
                    break;
                }
            case State.Chase:
                {
                    OnChase();
                    break;
                }
            case State.Attack:
                {
                    StartCoroutine(OnAttack());
                    break;
                }
        }
    }

    protected void SetUpStatus(UnitData uData)
    {
        CurrentUnitType = (UnitType)uData.UnitType;
        Level = uData.Level;
        HitPoint = uData.HitPoint;
        Mana = uData.Mana;
        AttackDamage = uData.AttackDamage;
        Armor = uData.Armor;
        AttackRange = uData.AttackRange;
        AttackDelay = uData.AttackDelay;
        AttackSpeed = uData.AttackSpeed;
        pathFinder.speed = MoveSpeed = uData.MoveSpeed;
    }

    protected virtual void Initialize()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        CurrentState = State.Idle;
        CurrentUnitType = UnitType.Warrior;

        MyInventory = new CreatureInven();

        canAttack = true;

        pathFinder.speed = MoveSpeed;
    }

    protected void CheckTarget()
    {
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {

    }
}
