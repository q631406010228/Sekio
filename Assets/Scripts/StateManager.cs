using Assets.Scripts;
using UnityEngine;

public class StateManager : IActorManagerInterface
{
    public float HP = 150;
    public float HPMax = 150;
    public HealthPostureUIVisual hp;

    [Header("=== 1st order state flags ===")]
    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isBlocked;
    public bool isDefense;
    public bool isCounterBack;          //是否在拼刀动画中
    public bool isCounterBackEnable;    //实际上是否能拼刀
    public bool isExecute;
    public bool isExecuteEnable;

    [Header("=== 2nd order state flags ===")]
    public bool isAllowDefense;
    public bool isImmortal;
    public bool isCounterBackSuccess;
    public bool isCounterBackFailure;

    void Start()
    {
        HP = HPMax;
    }

    void Update()
    {
        isGround = am.ac.CheckState("Ground");
        isJump = am.ac.CheckState("Jump");
        isFall = am.ac.CheckState("Fall");
        isRoll = am.ac.CheckState("Roll");
        isJab = am.ac.CheckState("Jab");
        isAttack = am.ac.CheckStateTag("AttackR");
        isHit = am.ac.CheckState("Hit");
        isDie = am.ac.CheckState("Die");
        isBlocked = am.ac.CheckState("Blocked");
        isCounterBack = am.ac.CheckState("CounterBack");
        isDefense = am.ac.CheckStateTag("Defense");

        isAllowDefense = isGround || isDefense;
        //isDefense = isAllowDefense && am.ac.CheckState("ShieldUp", "Defense");
        isImmortal = isRoll || isJab;
        isCounterBackSuccess = isCounterBackEnable;
        isCounterBackFailure = isCounterBack && !isCounterBackEnable;
    }

    public void AddHP(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax);
    }

    public void HealthDamage(float value)
    {
        hp.healthPostureSystem.HealthDamage((int)value);
    }

    public void PostureIncrease(float value)
    {
        hp.healthPostureSystem.PostureIncrease((int)value);
    }

}
