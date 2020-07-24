using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public CameraController camcon;
    public IUserInput pi;
    public GameObject player;
    public float walkSpeed = 8f;
    public float runMultiplier = 2.7f;
    public float jumpVelocity = 3.0f;
    public float rollVelocity = 3.0f;
    public float jabVelocity = 3.0f;
    public MeleeWeaponTrail mw;
    public ActorManager am;
    public bool isLock;

    public PhysicMaterial frictionZero;
    public PhysicMaterial frictionOne;

    [SerializeField]
    public Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private bool lockPlanar = false;
    private bool trackDirection = false;
    private CapsuleCollider col;
    private float lerpTarget;
    private Vector3 deltaPos;

    public delegate void OnActionDelegate();
    public event OnActionDelegate OnAction;

    // Start is called before the first frame update
    void Awake()
    {
        pi = GetComponent<IUserInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        camcon.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLock)
            return;
        if (camcon.isAI)
        {
            if (CheckStateTag("AttackR"))
                pi.targetDmag = 0;
            pi.dmag = Mathf.SmoothDamp(pi.dmag, pi.targetDmag, ref pi.velocityDmag, 0.1f);
        }
        if (pi.lockon)
        {
            camcon.LockChange();
        }
        if(camcon.lockState == false)
        {
            anim.SetFloat("forward", pi.dmag * Mathf.Lerp(anim.GetFloat("forward"), pi.run ? 2.0f : 1f, 0.1f));
            anim.SetFloat("right", 0);
        }
        else
        {
            Vector3 localDVecz = transform.InverseTransformVector(pi.dvec);
            anim.SetFloat("forward", Mathf.Lerp(anim.GetFloat("forward"), localDVecz.z * (pi.run ? 2 : 1), 0.1f));
            anim.SetFloat("right", Mathf.Lerp(anim.GetFloat("right"), localDVecz.x * (pi.run ? 2 : 1), 0.1f));

        }
        if (pi.roll)
        {
            if(rigid.velocity.magnitude < 1f)
            {
                anim.SetTrigger("jump");
                canAttack = false;
            }
            else
            {
                anim.SetTrigger("roll");
            }
        }
        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        if (pi.attack && (CheckState("Ground") || CheckStateTag("AttackR")) && canAttack)
        {
            //mw.enabled = true;
            int x = Random.Range(0, 2);
            anim.SetInteger("attackWay", x);
            anim.SetTrigger("attack");
        }
        if(camcon.lockState == false)
        {
            if (pi.dmag > 0.1f && pi.inputEnabled)
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.dvec, 0.1f);
            if (lockPlanar == false)
            {
                planarVec = walkSpeed * model.transform.forward * pi.dmag * (pi.run ? runMultiplier : 1f);
            }
        }
        else
        {
            if(trackDirection == false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planarVec.normalized;
            }
            if (lockPlanar == false)
                planarVec = pi.dvec * walkSpeed * (pi.run ? runMultiplier : 1f);
        }
        if (pi.defense && (CheckState("Ground") || CheckStateTag("Defense")) && canAttack)
        {
            anim.SetBool("rightCounter", EnemyAttackDirection.isRight);
            anim.SetTrigger("defense");
        }
        if (pi.action && am.sm.isExecuteEnable)
        {
            OnAction.Invoke();
        }
    }

    void FixedUpdate()
    {
        if (isLock)
        {
            rigid.isKinematic = true;
            return;
        }
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }

    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tagName);
        return result;
    }

    public void OnJumpEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
        trackDirection = true;
        canAttack = false;
    }

    public void IsGround()
    {
        anim.SetBool("isGround", true);
    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
        trackDirection = false;
        if(am != null)
            am.SetIsCounterBack(false);
        //if(mw != null)
        //    mw.enabled = false;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        canAttack = false;
    }

    public void OnRollEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, rollVelocity, 0);
        trackDirection = true;
        canAttack = false;
    }

    public void OnJabEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
        //float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.1f);
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"), currentWeight);
    }

    public void OnAttack1hAEnter()
    {
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"), 1);
        pi.inputEnabled = false;
        lerpTarget = 1;
    }

    public void OnAttackIdleUpdate()
    {
        //float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.1f);
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"), currentWeight);
    }

    public void OnAttackExit()
    {
        model.SendMessage("WeaponDisable");
    }

    public void OnHitEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
        if(am != null)
            am.SetIsCounterBack(false);
    }

    public void OnBlockedEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
    }

    public void OnLockEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
    }

    public void OnDieEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }

    public void OnUpdateRM(Vector3 _deltaPos)
    {
        if(CheckStateTag("AttackR"))
        {
            deltaPos += (Vector3)_deltaPos;
            //print(_deltaPos.x);
        }
    }

    public void OnStuundedEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
    }

    public void OnCounterBackEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
    }

    public void SetBool(string boolName, bool value)
    {
        anim.SetBool(boolName, value);
    }

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }

    public void OnRightAttack()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
        EnemyAttackDirection.JudgeDirection(0);
    }

    public void OnLeftAttack()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
        EnemyAttackDirection.JudgeDirection(2);
    }

    public void OnAttackEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
    }

}
