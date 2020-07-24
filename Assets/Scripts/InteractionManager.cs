using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class InteractionManager : IActorManagerInterface
    {
        private CapsuleCollider interCol;

        public List<EventCasterManager> overlapEcastms;
        public Transform targetTransform;

        void Start()
        {
            overlapEcastms = new List<EventCasterManager>();
            interCol = GetComponent<CapsuleCollider>();
        }

        /// <summary>
        /// 判断是否能触发timeline
        /// </summary>
        /// <param name="col"></param>
        void OnTriggerStay(Collider col)
        {
            ActorManager targetAm = col.GetComponentInParent<ActorManager>();
            if(targetAm != null && targetAm.sm.isExecute)
            {
                am.sm.isExecuteEnable = true;
                am.ac.camcon.executeDot.enabled = true;
                targetTransform = targetAm.transform;
            }
            EventCasterManager[] ecastms = col.GetComponents<EventCasterManager>();
            foreach (var ecastm in ecastms)
            {
                if (!overlapEcastms.Contains(ecastm))
                {
                    overlapEcastms.Add(ecastm);
                }
            }
        }

        /// <summary>
        /// 退出时清除timeline
        /// </summary>
        /// <param name="col"></param>
        void OnTriggerExit(Collider col)
        {
            am.sm.isExecuteEnable = false;
            am.ac.camcon.executeDot.enabled = false;
            targetTransform = null;
            EventCasterManager[] ecastms = col.GetComponents<EventCasterManager>();
            foreach (var ecastm in ecastms)
            {
                if (overlapEcastms.Contains(ecastm))
                {
                    overlapEcastms.Remove(ecastm);
                }
            }
        }
    }
}
