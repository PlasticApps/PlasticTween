using PlasticApps.Components;
using PlasticApps.Components.Ease;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using PlasticApps.Systems.Groups;
using Unity.Burst;

namespace PlasticApps.Systems.Ease
{
    [UpdateInGroup(typeof(TweenEasesSystems))]
    public class EaseInOutBackSystem : JobComponentSystem
    {
        [BurstCompile]
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        [RequireComponentTag(typeof(TweenEaseInOutBack))]
        struct EaseJob : IJobForEach<TweenBase>
        {
            public void Execute(ref TweenBase tween)
            {
                float value = tween.NormalizedTime;
                float s = 1.70158f;
                value /= .5f;
                if (value < 1)
                {
                    s *= 1.525f;
                    tween.Value = 0.5f * (value * value * ((s + 1) * value - s));
                }
                else
                {
                    value -= 2;
                    s *= (1.525f);
                    tween.Value = 0.5f * (value * value * ((s + 1) * value + s) + 2);
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new EaseJob();
            return job.Schedule(this, inputDeps);
        }
    }
}