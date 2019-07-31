using Unity.Entities;

namespace PlasticApps.Components
{
    /// <summary>
    /// Tween State
    /// </summary>
    public struct TweenBase : IComponentData
    {
        // repeats => -1 => Loop
        public int Loop;

        // bool byte 0 = false, 1 = true
        public byte PingPong;

        // bool byte 0 = false, 1 = true
        public byte Reversed;

        // current ease value 
        public float Value;

        // current time [0-Duration]
        public float Time;

        // current tween progress [0-1] 
        public float NormalizedTime;

        // total duration in seconds
        public float Duration;
     
        // Ease Type id -> retrieved from TypeManager
        public int EaseTypeId;

    }
}