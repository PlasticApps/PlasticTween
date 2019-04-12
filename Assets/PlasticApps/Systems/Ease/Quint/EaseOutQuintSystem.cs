using PlasticApps.Components;
using PlasticApps.Components.Ease;
using PlasticApps.Systems.Groups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace PlasticApps.Systems.Ease
{
    [UpdateInGroup(typeof(TweenEasesSystems))]
    public class EaseOutQuintSystem : JobComponentSystem
    {
        [BurstCompile]
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        [RequireComponentTag(typeof(TweenEaseOutQuint))]
        struct EaseJob : IJobForEach<TweenBase>
        {
            public void Execute(ref TweenBase tween)
            {
                float value = tween.NormalizedTime - 1;
                tween.Value = (value * value * value * value * value + 1);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new EaseJob();
            return job.Schedule(this, inputDeps);
        }
    }
}