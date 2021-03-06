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

public delegate void HealthChange(float value);
public delegate void ActiveSkill(int value);
public delegate void ActiveBuffSkill(BuffSkill value, bool add);
public class Creature : MonoBehaviour
{
    public GameObject AimPoint;
    public Vector3 targetWayPoint { get; set; } = new Vector3();

    public Dictionary<string, BuffSkill> buffDictionary { get; set; } = new Dictionary<string, BuffSkill>();
    public SkillData[] skills = new SkillData[3];
    protected bool[] skillCooldown = new bool[3];

    public event HealthChange healthChange;
    public event ActiveSkill activeSkill;
    public event ActiveBuffSkill buffSkill;
    public enum State { Pause, MoveStage, Idle, Walk, Chase, Hit, Attack, Skill, Dead };
    protected enum UnitType { Warrior, Magician, Arhcer, Priest };

    // event
    public event Action onDeath;

    // Item
    protected CreatureInven MyInventory;
    public Weapon MyWeapon;

    // enum
    public State CurrentState { get; set; } = State.Pause;
    protected UnitType CurrentUnitType { get; set; } = UnitType.Warrior;


    public CreatureData OriginStatus { get; set; }
    // status new
    public CreatureData MyStatus { get; set; }

    // specific status
    protected float HitDelay = 0.0f;
    protected int AttackCount = 0;
    protected int AttackCountMax = 1;
    protected int AttackComboCount = 0;
    protected float AttackTime = 0.0f;

    // bool
    public bool Dead { get; set; } = false;
    protected bool canAttack { get; set; } = true;
    protected bool isHit { get; set; } = false;
    protected bool isPlayingSkill = false;


    private WorldHPSlider HpUI;

    // component
    protected NavMeshAgent pathFinder; // ???????? AI ????????
    protected Animator animator;
    protected Rigidbody rigidbody;

    // target
    public LayerMask TargetlayerMask { get; set; }
    protected Creature target = null;
    protected bool hasTarget = false;
    protected float DetectRange = 100.0f;

    public void UpdateNavigation()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.isKinematic = true;

        pathFinder.isStopped = false;
        pathFinder.velocity = Vector3.zero;
        pathFinder.updatePosition = true;
        pathFinder.updateRotation = true;
        pathFinder.ResetPath();
    }

    public void AddBuff(BuffSkill buff)
    {
        buff.gameObject.SetActive(true);
        HpUI.PutBuffOnGrid(buff);
        buffDictionary.Add(buff.skillData.Name, buff);

        if (null != buffSkill)
            buffSkill(buff, true);
    }

    public void RemoveBuff(BuffSkill buff)
    {
        buffDictionary.Remove(buff.skillData.Name);
    }

    public void UpdateHP()
    {
        HpUI.SetUpHealth(MyStatus.HitPoint);
        healthChange += HpUI.ChangeHealth;

        healthChange(MyStatus.HitPoint);
    }

    protected virtual void ActiveSkill(int index)
    {
        if (Dead)
            return;

        skills[index].ActiveSkill();
    }

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
        animator.SetTrigger("OnDead");
        Dead = true;

        Collider[] MyColliders = GetComponents<Collider>();
        for (int i = 0; i < MyColliders.Length; i++)
        {
            MyColliders[i].enabled = false;
        }

        MyWeapon.gameObject.GetComponent<Collider>().enabled = false;

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

    public virtual float CaculateFinalDamage(float damage)
    {
        float calcDamage = damage - (MyStatus.Armor * 0.5f);

        return 0.0f < calcDamage ? calcDamage : 0.0f;
    }

    public virtual void GainHealth(float value)
    {
        if(MyStatus.HitPoint < OriginStatus.HitPoint)
        {
            float gap = OriginStatus.HitPoint - MyStatus.HitPoint;
            float finalValue = 0.0f;

            if(gap > value)
            {
                MyStatus.HitPoint += value;
                finalValue = value;
            }

            else
            {
                MyStatus.HitPoint += gap;
                finalValue = gap;
            }

            var healParticle = ParticleManager.instance.GetParticle("Heal_Once");
            healParticle.transform.position = transform.position;
            healParticle.GetComponent<ParticleObject>().OnPlay("Heal_Once");

            var UnderParticle = ParticleManager.instance.GetParticle("Heal_Under");
            UnderParticle.transform.position = transform.position;
            UnderParticle.GetComponent<ParticleObject>().OnPlay("Heal_Under");

            healthChange(MyStatus.HitPoint);
            StageBattleManager.instance.GetDamageFont(finalValue, transform.position, DamageFont.DamageFontTypes.Heal);
        }
    }

    private void HitParticlePlay(Vector3 hitPoint, Vector3 hitNormal)
    {
        var bloodParticle = ParticleManager.instance.GetParticle("Blood_Small");
        bloodParticle.transform.position = hitPoint;
        bloodParticle.transform.rotation = Quaternion.LookRotation(hitNormal);
        bloodParticle.GetComponent<ParticleObject>().OnPlay("Blood_Small");

        var hitParticle = ParticleManager.instance.GetParticle("Hit_Small");
        hitParticle.transform.position = hitPoint;
        hitParticle.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitParticle.GetComponent<ParticleObject>().OnPlay("Hit_Small");
    }

    private void HitDamageCaculate(float damage, out float finalDamage)
    {
        float FinalDamage = CaculateFinalDamage(damage);
        finalDamage = FinalDamage;
        MyStatus.HitPoint -= FinalDamage;
        healthChange(MyStatus.HitPoint);
    }

    public virtual void OnDamage(Vector3 hitPoint, Vector3 hitNormal, float damage, out float finalDamage)
    {
        HitParticlePlay(hitPoint, hitNormal);
        HitDamageCaculate(damage, out finalDamage);

        StageBattleManager.instance.GetDamageFont(finalDamage, transform.position);

        if (0 >= MyStatus.HitPoint && !Dead)
        {
            OnDie();
        }
    }

    public virtual void OnDamage(Vector3 hitPoint, Vector3 hitNormal, float damage)
    {
        HitParticlePlay(hitPoint, hitNormal);
        HitDamageCaculate(damage, out float finalDamage);

        StageBattleManager.instance.GetDamageFont(finalDamage, transform.position);

        if (0 >= MyStatus.HitPoint && !Dead)
        {
            OnDie();
        }
    }

    protected void AttackTimer()
    {
        if (Dead || CurrentState == State.Attack)
            return;

        if(!canAttack)
        {
            AttackTime += Time.deltaTime; 

            if(AttackTime >= MyStatus.AttackDelay)
            {
                AttackTime = 0.0f;
                canAttack = true;
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

    protected virtual IEnumerator OnSkill()
    {
        while (isPlayingSkill)
            yield return null;

        for (int i = 1; i < 3; i++)
        {
            if (!skills[i].isCooldown)
            {
                skillCooldown[i] = true;
                skills[i].Initialization(this, i);
                isPlayingSkill = true;
                activeSkill(i);
                break;
            }
        }
    }

    public virtual void Shoot()
    {
        if (null != target)
        {
            Projectile newArrow = ObjectManager.instance.GetObject(ObjectManager.ProjType.Arrow);
            Vector3 Dir = target.transform.position - transform.position;
            newArrow.Initialize(AimPoint.transform.position, Dir, target.gameObject.layer, MyStatus.AttackDamage);
            newArrow.Owner = this;
        }
    }

    public virtual void EndSkill()
    {
        CurrentState = State.Idle;
        isPlayingSkill = false;
    }

    public void CooldownEnd(int skillIndex)
    {
        skillCooldown[skillIndex] = false;
    }

    protected void OnChase()
    {
        if (hasTarget)
        {
            if(null == target || target.Dead)
            {
                animator.SetBool("isWalk", false);
                CurrentState = State.Idle;
                target = null;
                hasTarget = false;
            }

            else
            {
                TargetLookAt();

                if (MyStatus.AttackRange < pathFinder.remainingDistance)
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
                    //pathFinder.updatePosition = false;
                    //pathFinder.updateRotation = false;
                    pathFinder.isStopped = true;
                    pathFinder.velocity = Vector3.zero;
                    animator.SetBool("isWalk", false);

                    if((skills[1] != null && !skillCooldown[1]) ||
                        (skills[2] != null && !skillCooldown[2]))
                    {
                        CurrentState = State.Skill;
                    }

                    else if (canAttack)
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
                    Physics.OverlapSphere(transform.position, DetectRange, TargetlayerMask);

        float distance = 9999.0f;
        Creature tmpTargetSave = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            Creature tmpTarget = colliders[i].gameObject.GetComponent<Creature>();

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
                    CurrentState = State.Idle;
                    target = null;
                    hasTarget = false;
                }
            }

            else
            {
                animator.SetBool("isWalk", false);
                CurrentState = State.Idle;
                target = null;
                hasTarget = false;
            }
        }
    }

    private IEnumerator OnPause()
    {
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator MoveStage()
    {
        animator.SetBool("isWalk", true);

        while (Vector3.Distance(transform.position, targetWayPoint) > 0.1f)
        {
            Vector3 tmpDir = targetWayPoint - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(tmpDir), Time.deltaTime * 3.0f);

            yield return new WaitForEndOfFrame();

            float stepSpeed = 5.0f * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, stepSpeed);
        }

        rigidbody.velocity = Vector3.zero;
        transform.position = targetWayPoint;

        animator.SetBool("isWalk", false);
        CurrentState = State.Pause;

        hasTarget = false;
        target = null;

        StageBattleManager.instance.WaveStart();
    }
    
    protected void CheckState()
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
            case State.Skill:
                {
                    StartCoroutine(OnSkill());
                    break;
                }
        }
    }

    protected virtual void Initialize()
    {
        HpUI = Instantiate(Resources.Load<WorldHPSlider>("_Prefabs/UI/Status Slider"), transform);
        HpUI.SetupData();
        HpUI.SetUpHealth(MyStatus.HitPoint);
        UpdateHP();

        pathFinder = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        CurrentState = State.Pause;
        CurrentUnitType = UnitType.Warrior;

        MyInventory = new CreatureInven();

        canAttack = true;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.isKinematic = true;

        var collider = MyWeapon.GetComponent<Collider>();
        if(collider != null)
            MyWeapon.GetComponent<Collider>().enabled = false;
    }
}
