using PlasticApps.Components;
using PlasticApps.Components.Ease;
using Unity.Entities;
using Unity.Jobs;
using PlasticApps.Systems.Groups;
using Unity.Burst;
using Unity.Mathematics;

namespace PlasticApps.Systems.Ease
{
    [UpdateInGroup(typeof(TweenEasesSystems))]
    public class EaseOutSineSystem : JobComponentSystem
    {
        [BurstCompile]
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        [RequireComponentTag(typeof(TweenEaseOutSine))]
        struct EaseJob : IJobForEach<TweenBase>
        {
            public void Execute(ref TweenBase tween)
            {
                float value = tween.NormalizedTime;
                tween.Value = math.sin(value * ((float) math.PI * 0.5f));
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new EaseJob();
            return job.Schedule(this, inputDeps);
        }
    }
}