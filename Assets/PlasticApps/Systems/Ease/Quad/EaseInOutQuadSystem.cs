using PlasticApps.Components;
using PlasticApps.Components.Ease;
using PlasticApps.Systems.Groups;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace PlasticApps.Systems.Ease
{
    [UpdateInGroup(typeof(TweenEasesSystems))]
    public class EaseInOutQuadSystem : JobComponentSystem
    {
        [BurstCompile]
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        [RequireComponentTag(typeof(TweenEaseInOutQuad))]
        public struct EaseJob : IJobForEach<TweenBase>
        {
            public void Execute(ref TweenBase tween)
            {
                unchecked
                {
                    float value = tween.NormalizedTime;
                    value /= .5f;
                    if (value < 1)
                    {
                        tween.Value = 0.5f * value * value;
                    }
                    else
                    {
                        value--;
                        tween.Value = -0.5f * (value * (value - 2) - 1);
                    }
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