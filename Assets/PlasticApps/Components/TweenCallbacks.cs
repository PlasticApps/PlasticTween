using Unity.Entities;

namespace PlasticApps.Components
{
    public struct TweenCompleteCallback : IComponentData
    {
        // Callback ID
        public int Value;
    }
}