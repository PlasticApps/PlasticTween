using Unity.Entities;

namespace PlasticApps.Systems.Groups
{
    [UpdateAfter(typeof(TweenUpdateSystem))]
    [UpdateInGroup(typeof(TweenSystems))]
    public class TweenEasesSystems: ComponentSystemGroup
    {
    }
}