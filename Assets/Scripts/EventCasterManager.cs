using Assets.Scripts;
using UnityEngine;

public class EventCasterManager : IActorManagerInterface
{
    public string enventName;
    public bool active;
    public Vector3 offset = new Vector3(0, 0, 0);

    void Start()
    {
        if(am == null)
        {
            am = GetComponentInParent<ActorManager>();
        }
    }
}
