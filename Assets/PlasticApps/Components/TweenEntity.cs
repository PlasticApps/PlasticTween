using Unity.Collections;
using Unity.Entities;

namespace PlasticApps.Components
{
    /// <summary>
    /// Relation - Animation Target [Translation, NonUniformScale, RotationEulerXYZ]
    /// </summary>
    public struct TweenEntity : IComponentData
    {        
        // Tweening Target
        public Entity Value;
    }
}