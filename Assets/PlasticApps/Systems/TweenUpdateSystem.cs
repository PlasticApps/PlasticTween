using PlasticApps.Components;
using PlasticApps.Systems.Groups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace PlasticApps.Systems
{
    [UpdateInGroup(typeof(TweenSystems))]
    public class TweenUpdateSystem : JobComponentSystem
    {
        [BurstCompile]
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        struct UpdateJob : IJobForEach<TweenBase>
        {
            public float deltaTime;

            public void Execute(ref TweenBase tween)
            {
                float runningTime = tween.Time + deltaTime;
                tween.Time = runningTime;

                if (runningTime >= 0)
                {
                    float percentage = tween.Duration > 0 ? runningTime / tween.Duration : 1.0f;
                    tween.NormalizedTime = math.clamp(
                        tween.Reversed == 1 ? 1.0f - percentage : percentage,
                        0.0f, 1.0f
                    );
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float dt = Time.deltaTime;
            UpdateJob job = new UpdateJob
            {
                deltaTime = dt
            };
            return job.Schedule(this, inputDeps);
        }

//        same as JobForEach - but more expensive? GC?
//        [BurstCompile]
//        struct UpdateJob : IJobParallelFor
//        {
//            [ReadOnly] public float deltaTime;
//
//            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Entity> Entities;
//
//            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<TweenBase> Tweens;
//            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<TweenBase> OutTweens;
//
//            public void Execute(int index)
//            {
//                var entity = Entities[index];
//                if (OutTweens.Exists(entity))
//                {
//                    var tween = OutTweens[entity];
//                    float runningTime = tween.Time + deltaTime;
//                    float percentage = tween.Duration > 0 ? runningTime / tween.Duration : 1.0f;
//                    tween.Time = runningTime;
//                    tween.NormalizedTime = math.clamp(
//                        tween.Reversed == 1 ? 1.0f - percentage : percentage,
//                        0.0f, 1.0f
//                    );
//                    OutTweens[entity] = tween;
//                }
//            }
//        }
//
//        private EntityQuery m_QueryCompleted;
//
//        protected override void OnCreate()
//        {
//            m_QueryCompleted = GetEntityQuery(
//                ComponentType.ReadOnly<TweenBase>(),
//                ComponentType.Exclude<TweenPaused>(),
//                ComponentType.Exclude<TweenComplete>()
//            );
//        }
//
//        protected override JobHandle OnUpdate(JobHandle inputDeps)
//        {
//            var dt = Time.deltaTime;
//            int total = m_QueryCompleted.CalculateLength();
//
//            return new UpdateJob
//            {
////                Tweens = m_QueryCompleted.ToComponentDataArray<TweenBase>(Allocator.TempJob),
//                Entities = m_QueryCompleted.ToEntityArray(Allocator.TempJob),
//                OutTweens = GetComponentDataFromEntity<TweenBase>(),
//                deltaTime = dt
//            }.Schedule(total, 1, inputDeps);
//        }
    }
}