using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 0.07760613f, 0f)]
[TrackClipType(typeof(MySuperPlayableClip))]
[TrackBindingType(typeof(ActorManager))]
public class MySuperPlayableTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MySuperPlayableMixerBehaviour>.Create (graph, inputCount);
    }
}
