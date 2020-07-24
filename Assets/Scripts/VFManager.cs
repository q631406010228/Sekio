using UnityEngine;

namespace Assets.Scripts
{
    public class VFManager : MonoBehaviour
    {
        private static readonly VFManager instance = new VFManager();
        public static VFManager Instance()
        {
            return instance;
        }

        private GameObject weaponSpark;
        public GameObject WeaponSpark
        {
            get
            {
                return GameObjectPool.Instance().GetPool(weaponSpark, Vector3.zero, "weaponSpark");
            }
                
            private set
            {
                weaponSpark = value;
            }
        }

        VFManager()
        {
            weaponSpark = Resources.Load<GameObject>("Prefebs/VfxBoomSparks2");
        }
    }
}
