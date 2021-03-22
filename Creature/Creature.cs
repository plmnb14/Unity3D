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
    enum State { Idle, Walk, Chase, Hit, Attack, Dead };
    enum UnitType { Warrior, Magician, Arhcer, Priest };

    // event
    public event Action onDeath;

    private CreatureInven MyInventory;

    // enum
    private State CurrentState { get; set; } = State.Idle;
    private UnitType CurrentUnitType { get; set; } = UnitType.Warrior;


    public int tmpClassNum = 0;
    // status
    private float Level { get; set; } = 0.0f;
    private float HitPoint { get; set; } = 1000.0f;
    private float AttackDamage { get; set; } = 0.0f;
    private float Armor { get; set; } = 0.0f;
    public float AttackRange = 2.5f;
    public float AttackDelay = 2.5f;
    public float AttackSpeed = 1.0f;
    public float MoveSpeed = 2.0f;
    private float HitDelay { get; set; } = 0.0f;
    private int AttackCount { get; set; } = 0;
    private int AttackCountMax { get; set; } = 1;
    private int AttackComboCount { get; set; } = 0;
    private float AttackTime { get; set; } = 0;

    // bool
    public bool Dead { get; set; } = false;
    private bool canAttack { get; set; } = true;
    private bool isHit { get; set; } = false;

    //
    public ParticleSystem bloodEffect;
    public ParticleSystem hitEffect;

    // component
    private NavMeshAgent pathFinder; // 경로계산 AI 에이전트
    private Animator animator;

    // target
    public LayerMask mask;
    private Creature target = null;
    private bool hasTarget = false;
    private float ChaseDelayTime = 0.0f;
    private float DetectRange = 100.0f;

    public Weapon MyWeapon;

    public void Shooting()
    {

    }

    public float CheckDistance(Vector3 position, Vector3 TargetPosition)
    {
        return Vector3.Distance(position, TargetPosition);
    }

    public virtual void OnDie()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        Dead = true;
    }

    public virtual void OnDamage(Vector3 hitPoint, Vector3 hitNormal, float damage)
    {
        HitPoint -= damage;

        if (0 >= HitPoint && !Dead)
        {
            Dead = true;
            animator.SetTrigger("Dead");
            CurrentState = State.Dead;
            OnDie();
        }

        else
        {
            if (!isHit)
            {
                isHit = true;
                //CurrentState = State.Hit;
                //animator.SetTrigger("Hit");
            }
        }

        bloodEffect.transform.position = hitPoint;
        bloodEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        bloodEffect.Play();

        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();
    }

    public void OnHit()
    {

    }

    public void OnWalk()
    {

    }

    public void OnFootR()
    {

    }

    public void OnFootL()
    {

    }

    private void AttackTimer()
    {
        if (CurrentState == State.Attack)
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
        if (canAttack)
        {
            //transform.LookAt(target.transform);

            canAttack = false;
            animator.SetInteger("AttackNum", UnityEngine.Random.Range(0, 2));
            animator.SetBool("isAttack", true);

            if (tmpClassNum == 1)
            {
                Arrow Projectile = ObjectManager.GetObject();
                Vector3 direction = target.transform.position - transform.position;
                Projectile.Initialize(transform.position + new Vector3(0.0f, 1.5f, 0.0f), direction, target.gameObject.layer , 100.0f);
            }
        }

        else
        {
            if (animator.GetBool("isAttack"))
            {
                animator.speed = AttackSpeed;

                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

                animator.speed = 1.0f;
                CurrentState = State.Idle;
                animator.SetBool("isAttack", false);
            }
        }
    }

    protected void OnChase()
    {
        if (hasTarget)
        {
            transform.LookAt(target.transform);

            if (AttackRange < CheckDistance(transform.position, target.transform.position))
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(target.transform.position);
                animator.SetBool("isWalk", true);
            }

            else
            {
                pathFinder.isStopped = true;
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

        else
        {
            CurrentState = State.Idle;
        }
    }

    protected void FindTarget(LayerMask targetMask)
    {
        if(!hasTarget)
        {
            Collider[] colliders =
                    Physics.OverlapSphere(transform.position, DetectRange, targetMask);

            for (int i = 0; i < colliders.Length; i++)
            {
                Creature tmpTarget = colliders[i].GetComponent<Creature>();

                if (null != tmpTarget && !tmpTarget.Dead)
                {
                    target = tmpTarget;
                    hasTarget = true;
                    CurrentState = State.Chase;
                }
            }
        }

        else
        {
            CurrentState = true == target.Dead ? State.Idle : State.Chase;

            if(target.Dead)
            {
                hasTarget = false;
            }
        }
    }

    public void CheckState()
    {
        switch(CurrentState)
        {
            case State.Idle:
                {
                    FindTarget(mask);
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

    protected virtual void Initialize()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        CurrentState = State.Idle;

        MyInventory = new CreatureInven();

        canAttack = true;

        ChaseDelayTime = 0.25f;
        pathFinder.speed = MoveSpeed;
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        CheckState();
        AttackTimer();
    }
}
