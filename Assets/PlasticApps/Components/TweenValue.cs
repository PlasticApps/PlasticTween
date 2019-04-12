using Unity.Entities;

namespace PlasticApps.Components
{
    public struct TweenValue: IComponentData
    {
        public float from;
        public float to;
    }
}