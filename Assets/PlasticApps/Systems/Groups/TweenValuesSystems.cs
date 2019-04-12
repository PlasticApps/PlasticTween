using Unity.Entities;

namespace PlasticApps.Systems.Groups
{
    [UpdateInGroup(typeof(TweenSystems))]
    [UpdateAfter(typeof(TweenUpdateSystem))]
    [UpdateAfter(typeof(TweenEasesSystems))]
    public class TweenValuesSystems: ComponentSystemGroup
    {
    }
}