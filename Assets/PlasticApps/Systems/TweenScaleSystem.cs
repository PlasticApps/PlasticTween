using PlasticApps.Systems.Groups;
using PlasticApps.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace PlasticApps.Systems
{
    [UpdateInGroup(typeof(TweenValuesSystems))]
    public class TweenScaleSystem : JobComponentSystem
    {
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        struct TweenValueJob : IJobForEach<TweenBase, TweenScale, TweenEntity>
        {
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<NonUniformScale> Scales;

            public void Execute([ReadOnly] ref TweenBase tween, [ReadOnly] ref TweenScale scale,
                [ReadOnly] ref TweenEntity entity)
            {
                if (Scales.Exists(entity.Value))
                {
                    Scales[entity.Value] = new NonUniformScale
                    {
                        Value = math.lerp(scale.From, scale.To, tween.Value)
                    };
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new TweenValueJob
            {
                Scales = GetComponentDataFromEntity<NonUniformScale>()
            }.Schedule(this, inputDeps);
        }
    }
}