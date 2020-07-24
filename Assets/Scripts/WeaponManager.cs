using Assets.Scripts;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    public Collider weaonColL;
    public Collider weaonColR;

    public GameObject whL;
    public GameObject whR;

    public WeaponController wcL;
    public WeaponController wcR;

    void Start()
    {

        //whL = transform.DeepFind("WeaponHandleL").gameObject;
        whR = transform.DeepFind("WeaponHandleR").gameObject;
        //wcL = BindWeaponController(whL);
        wcR = BindWeaponController(whR);
        //weaonColL = whL.GetComponentInChildren<Collider>();
        weaonColR = whR.GetComponentInChildren<Collider>();
        weaonColR.enabled = false;
    }

    public WeaponController BindWeaponController(GameObject targetObj)
    {
        WeaponController tempWc = targetObj.GetComponent<WeaponController>();
        if(tempWc == null)
        {
            tempWc = targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this;
        return tempWc;
    }

    public void WeaponEnable()
    {
        weaonColR.enabled = true;
    }

    public void WeaponDisable()
    {
        weaonColR.enabled = false;
    }

    public void CounterBackEnable()
    {
        am.SetIsCounterBack(true);
    }

    public void CounterBackDisable()
    {
        am.SetIsCounterBack(false);
    }

}
