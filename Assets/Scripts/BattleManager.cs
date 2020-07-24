using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface
{
    private CapsuleCollider defCol;

    void Start()
    {
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = new Vector3(0, 1, 0);
        defCol.height = 2;
        defCol.radius = 0.25f;
        defCol.isTrigger = true;
    }
    
    void OnTriggerEnter(Collider col)
    {
        ActorManager attackAm = col.GetComponentInParent<ActorManager>();
        WeaponController attackWc = col.GetComponentInParent<WeaponController>();
        if (attackWc == null)
            return;

        GameObject attacker = attackWc.wm.gameObject;
        GameObject injured = am.wm.gameObject;

        bool attackValid = CheckAnglePlayer(attacker, injured, 45);
        bool injuredValid = CheckAngleTarget(attacker, injured, 30);

        if (col.tag == "Weapon" && !attackAm.sm.isAllowDefense)
        {
            am.TryDoDamage(attackWc, attackValid, injuredValid);
        }
    }

    /// <summary>
    /// attacker的位置与injured朝向的夹角小于attackerAngleLimit
    /// attack朝向与injured朝向大于180 - attackerAngleLimit（双方是正面相对的）
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="injured"></param>
    /// <param name="attackerAngleLimit"></param>
    /// <returns></returns>
    public static bool CheckAnglePlayer(GameObject attacker, GameObject injured, float attackerAngleLimit)
    {
        Vector3 counterDir = attacker.transform.position - injured.transform.position;

        float counterAnglel1 = Vector3.Angle(injured.transform.forward, counterDir);
        float counterAnglel2 = Vector3.Angle(attacker.transform.forward, injured.transform.forward);
                                                                        
        bool counterValid = counterAnglel1 < attackerAngleLimit && counterAnglel2 > 180 - attackerAngleLimit;
        return counterValid;
    }

    /// <summary>
    /// injured的位置与attacker朝向的夹角小于injuredAngleLimit（防止在后背）
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="injured"></param>
    /// <param name="injuredAngleLimit"></param>
    /// <returns></returns>
    public static bool CheckAngleTarget(GameObject attacker, GameObject injured, float injuredAngleLimit)
    {
        Vector3 attackingDir = injured.transform.position - attacker.transform.position;

        float attackingAngle1 = Vector3.Angle(attacker.transform.forward, attackingDir);

        bool attackValid = attackingAngle1 < injuredAngleLimit;
        return attackValid;
    }

}
