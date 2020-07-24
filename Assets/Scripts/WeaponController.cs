using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Sensor")
        {
            ActorManager am = col.GetComponentInParent<ActorManager>();
            if (am != null && am.sm.isCounterBackEnable)
            {
                wm.am.CounterBack();
                AudioManger.Instance.PlayAudio("Sword1", transform.position);
                GameObject weaponSpark = VFManager.Instance().WeaponSpark;
                weaponSpark.transform.position = this.transform.position;
            }
        }
    }
}
