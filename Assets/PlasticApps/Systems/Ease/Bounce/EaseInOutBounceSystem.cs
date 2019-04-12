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
    public class EaseInOutBounceSystem : JobComponentSystem
    {
        public static float Ease(float value)
        {
            value /= 1f;
            if (value < 1 / 2.75f)
            {
                return 7.5625f * value * value;
            }
            else if (value < 2 / 2.75f)
            {
                value -= 1.5f / 2.75f;
                return 7.5625f * value * value + .75f;
            }
            else if (value < 2.5f / 2.75f)
            {
                value -= 2.25f / 2.75f;
                return 7.5625f * value * value + .9375f;
            }

            value -= 2.625f / 2.75f;
            return 7.5625f * value * value + .984375f;
        }

        [BurstCompile]
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        [RequireComponentTag(typeof(TweenEaseInOutBounce))]
        struct EaseJob : IJobForEach<TweenBase>
        {
            public void Execute(ref TweenBase tween)
            {
                float value = tween.NormalizedTime;
                float d = 1f;
                if (value < d * 0.5f)
                {
                    tween.Value = Ease(value * 2) * 0.5f;
                }
                else
                {
                    tween.Value = Ease(value * 2 - d) * 0.5f + 0.5f;
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