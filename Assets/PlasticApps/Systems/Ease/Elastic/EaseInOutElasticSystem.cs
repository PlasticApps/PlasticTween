﻿using PlasticApps.Components;
using PlasticApps.Components.Ease;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using PlasticApps.Systems.Groups;
using Unity.Burst;

namespace PlasticApps.Systems.Ease
{
    [UpdateInGroup(typeof(TweenEasesSystems))]
    public class EaseInOutElasticSystem : JobComponentSystem
    {
        [BurstCompile]
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        [RequireComponentTag(typeof(TweenEaseInOutElastic))]
        struct EaseJob : IJobForEach<TweenBase>
        {
            public void Execute(ref TweenBase tween)
            {
                float value = tween.NormalizedTime;
                float d = 1f;
                float p = d * .3f;
                float a = 0;

                if (value == 0)
                {
                    tween.Value = 0;
                }

                else if ((value /= d * 0.5f) == 2)
                {
                    tween.Value = 1;
                }
                else
                {
                    float s = 0;
                    if (a == 0f || a < 1)
                    {
                        a = 1;
                        s = p / 4;
                    }
                    else
                    {
                        s = p / (2 * (float) math.PI) * math.asin(1 / a);
                    }

                    if (value < 1)
                    {
                        tween.Value = -0.5f * (a * math.pow(2, 10 * (value -= 1)) *
                                               math.sin((value * d - s) * (2 * (float) math.PI) / p));
                    }
                    else
                    {
                        tween.Value = a * math.pow(2, -10 * (value -= 1)) *
                                      math.sin((value * d - s) * (2 * (float) math.PI) / p) *
                                      0.5f +
                                      1;
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