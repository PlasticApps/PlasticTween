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
    public class EaseInOutExpoSystem : JobComponentSystem
    {
        [BurstCompile]
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        [RequireComponentTag(typeof(TweenEaseInOutExpo))]
        struct EaseJob : IJobForEach<TweenBase>
        {
            public void Execute(ref TweenBase tween)
            {
                float value = tween.NormalizedTime / .5f;
                if (value < 1)
                {
                    tween.Value = 0.5f * math.pow(2, 10 * (value - 1));
                }
                else
                {
                    value--;
                    tween.Value = 0.5f * (-math.pow(2, -10 * value) + 2);
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