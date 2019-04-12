using Unity.Entities;
using Unity.Mathematics;

namespace PlasticApps.Components
{
    public struct TweenScale: IComponentData
    {
        public float3 From;
        public float3 To;
    }
}