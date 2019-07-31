using PlasticApps.Components;
using PlasticApps.Systems.Groups;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace PlasticApps.Systems
{
    [UpdateInGroup(typeof(TweenValuesSystems))]
    public class TweenRotationSystem : JobComponentSystem
    {
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        struct TweenValueJob : IJobForEach<TweenBase, TweenRotate, TweenEntity>
        {
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Rotation> Rotations;

            public void Execute([ReadOnly] ref TweenBase tween, [ReadOnly] ref TweenRotate rotate,
                [ReadOnly] ref TweenEntity entity)
            {
                if (Rotations.Exists(entity.Value))
                {
                    Rotations[entity.Value] = new Rotation
                    {
                        Value = math.slerp(rotate.From, rotate.To, tween.Value)
                    };
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new TweenValueJob
            {
                Rotations = GetComponentDataFromEntity<Rotation>()
            }.Schedule(this, inputDeps);
        }
    }
}