using Assets.Scripts;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public BattleManager bm;

    [Header("=== Auto Generate if Null ===")]
    public ActorController ac;
    public WeaponManager wm;
    public StateManager sm;
    public DirectorManager dm;
    public InteractionManager im;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;
        ac = GetComponent<ActorController>();
        GameObject model = ac.model;
        GameObject sensor = transform.Find("Sensor").gameObject;
        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(gameObject);
        dm = Bind<DirectorManager>(gameObject);
        im = Bind<InteractionManager>(sensor);
        ac.OnAction += DoAction;
    }

    public void SetIsCounterBack(bool isCounterBack)
    {
        sm.isCounterBackEnable = isCounterBack;
    }

    public void DoAction()
    {
        if(im.overlapEcastms.Count != 0)
        {
            if(im.overlapEcastms[0].enventName == "StabTimeline")
            {
                if(BattleManager.CheckAnglePlayer(ac.model, im.overlapEcastms[0].am.ac.model, 15))
                {
                    ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                    transform.position = new Vector3(im.targetTransform.position.x, transform.position.y, im.targetTransform.position.z) + im.targetTransform.forward;
                    dm.PlayFrontStab("StabTimeline", this, im.overlapEcastms[0].am);
                }
            }
        }
    }

    private T Bind<T>(GameObject go) where T: IActorManagerInterface
    {
        T tempInstace = go.GetComponent<T>();
        if(tempInstace == null)
            tempInstace = go.AddComponent<T>();
        tempInstace.am = this;
        return tempInstace;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TryDoDamage(WeaponController attackWc, bool attackValid, bool counterValid)
    {
        if (sm.isDie)
            return;
        if (sm.isExecute)
            return;
        if (sm.isCounterBackEnable)
            return;
        if (sm.isCounterBackFailure && counterValid)
            attackWc.wm.am.Stunned();
        if (sm.isCounterBackFailure)
            HitOrDie(false);
        if (sm.isImmortal)
            return;
        if (sm.isDefense)
            Blocked();
        else
            HitOrDie(true);
    }

    public void HitOrDie(bool doHitAnimation)
    {
        if (sm.hp.healthPostureSystem.healthAmount <= 0)
        {

        }
        else
        {
            sm.HealthDamage(10);
            if (sm.hp.healthPostureSystem.healthAmount > 0)
            {
                if (doHitAnimation)
                    Hit();
            }
            else
                Die();
        }
    }

    public void CounterBack()
    {
        sm.PostureIncrease(10);
        if (sm.hp.healthPostureSystem.postureAmount == sm.hp.healthPostureSystem.postureAmountMax)
        {
            sm.isExecute = true;
            ac.IssueTrigger("stunned");
            ac.pi.inputEnabled = false;
            if (ac.camcon.lockState)
                ac.camcon.LockUnlock();
            ac.camcon.enabled = false;
            ac.isLock = true;
        }
    }

    public void Stunned()
    {
        ac.IssueTrigger("stunned");
    }

    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }

    public void Hit()
    {
        ac.IssueTrigger("hit");
    }

    public void Die()
    {
        ac.IssueTrigger("die");
        ac.pi.inputEnabled = false;
        if (ac.camcon.lockState)
            ac.camcon.LockUnlock();
        ac.camcon.enabled = false;
    }

    public void LockUnlockActorController(bool value)
    {
        ac.SetBool("lock", value);
    }
}
