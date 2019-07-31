using Unity.Entities;
using Unity.Mathematics;

namespace PlasticApps.Components
{
    public struct TweenRotate : IComponentData
    {
        public quaternion From;
        public quaternion To;
    }
}