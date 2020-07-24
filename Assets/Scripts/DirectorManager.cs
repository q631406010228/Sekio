using Assets.Scripts;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class DirectorManager : IActorManagerInterface
{
    public PlayableDirector pd;

    [Header("=== Timeline assets ===")]
    public TimelineAsset frontStab;

    [Header("=== Assets Setting ===")]
    public ActorManager attacker;
    public ActorManager victim;

    // Start is called before the first frame update
    void Start()
    {
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            pd.Play();
        }
    }

    public void PlayFrontStab(string timelineName, ActorManager attacker, ActorManager victim)
    {
        if (pd.state == PlayState.Playing)
            return;
        if(timelineName == "StabTimeline")
        {
            pd.playableAsset = frontStab;

            TimelineAsset timeline =(TimelineAsset)pd.playableAsset;
            foreach (var track in timeline.GetOutputTracks())
            {
                if(track.name == "Attacker Track")
                {
                    pd.SetGenericBinding(track, attacker);
                    foreach(var clip in track.GetClips())
                    {
                        MySuperPlayableClip myClip = (MySuperPlayableClip)clip.asset;
                        MySuperPlayableBehaviour myBehav = myClip.template;
                        myClip.myAM.exposedName = System.Guid.NewGuid().ToString();
                        myBehav.myFloat = 777;
                        pd.SetReferenceValue(myClip.myAM.exposedName, attacker);
                    }
                }
                else if(track.name == "Victim Track")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MySuperPlayableClip myClip = (MySuperPlayableClip)clip.asset;
                        MySuperPlayableBehaviour myBehav = myClip.template;
                        myClip.myAM.exposedName = System.Guid.NewGuid().ToString();
                        myBehav.myFloat = 888;
                        pd.SetReferenceValue(myClip.myAM.exposedName, victim);
                    }
                }
                else if (track.name == "Attacker Animation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "Victim Animation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }
            }

            //foreach (var trackBinding in pd.playableAsset.outputs)
            //{
            //    if (trackBinding.streamName == "Attacker Track")
            //    {
            //        pd.SetGenericBinding(trackBinding.sourceObject, attacker);
            //    }
            //    else if (trackBinding.streamName == "Victim Track")
            //    {
            //        pd.SetGenericBinding(trackBinding.sourceObject, victim);
            //    }
            //    else if (trackBinding.streamName == "Attacker Animation")
            //    {
            //        pd.SetGenericBinding(trackBinding.sourceObject, attacker.ac.anim);
            //    }
            //    else if (trackBinding.streamName == "Victim Animation")
            //    {
            //        pd.SetGenericBinding(trackBinding.sourceObject, victim.ac.anim);
            //    }
            //}
            pd.Play();
        }
    }
}
