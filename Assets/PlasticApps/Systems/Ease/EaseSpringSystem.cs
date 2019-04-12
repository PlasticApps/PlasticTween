using PlasticApps.Components;
using PlasticApps.Components.Ease;
using PlasticApps.Systems.Groups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace PlasticApps.Systems.Ease
{
    [UpdateInGroup(typeof(TweenEasesSystems))]
    public class EaseSpringSystem : JobComponentSystem
    {
        [BurstCompile]
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        [RequireComponentTag(typeof(TweenEaseSpring))]
        struct EaseJob : IJobForEach<TweenBase>
        {
            public void Execute(ref TweenBase tween)
            {
                unchecked
                {
                    float value = tween.NormalizedTime;
                    value = math.clamp(value, 0.0f, 1.0f);
                    value =
                    (math.sin(value * (float) math.PI * (0.2f + 2.5f * value * value * value)) *
                     math.pow(1f - value, 2.2f) +
                     value) * (1f + (1f - value) * 1.2f);
                    tween.Value = value;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new EaseJob();
            return job.Schedule(this, inputDeps);
        }
    }

// tests
//    public abstract class EaseSystem<T> : JobComponentSystem where T : IComponentData
//    {
//        private EntityQuery m_Group;
//
//        protected override void OnCreate()
//        {
//            m_Group = GetEntityQuery(ComponentType.ReadWrite<TweenBase>(), ComponentType.ReadOnly<T>());
//        }
//
//        protected abstract JobHandle PrepareJobHandle(JobHandle inputDeps, int totalEntity,
//            NativeArray<TweenBase> tweens);
//
//        protected override JobHandle OnUpdate(JobHandle inputDeps)
//        {
//            return PrepareJobHandle(inputDeps, m_Group.CalculateLength(),
//                m_Group.ToComponentDataArray<TweenBase>(Allocator.TempJob));
//        }
//    }
//
//    [UpdateInGroup(typeof(TweenEasesSystems))]
//    public class EaseSpringSystem : EaseSystem<TweenEaseSpring>
//    {
//        [BurstCompile]
//        struct EaseJob : IJobParallelFor
//        {
//            public NativeArray<TweenBase> Tweens;
//
//            public void Execute(int index)
//            {
//                var tween = Tweens[index];
//                float value = tween.NormalizedTime;
//                value = math.clamp(value, 0.0f, 1.0f);
//                value =
//                (math.sin(value * (float) math.PI * (0.2f + 2.5f * value * value * value)) *
//                 math.pow(1f - value, 2.2f) +
//                 value) * (1f + (1f - value) * 1.2f);
//                tween.Value = value;
//                Tweens[index] = tween;
//            }
//        }
//
//        protected override JobHandle PrepareJobHandle(JobHandle inputDeps, int totalEntity,
//            NativeArray<TweenBase> tweens)
//        {
//            return new EaseJob
//            {
//                Tweens = tweens
//            }.Schedule(totalEntity, 32, inputDeps);
//        }
//    }
//
//
//    [UpdateInGroup(typeof(TweenEasesSystems))]
//    public class EaseSpringSystem : JobComponentSystem
//    {
//        private EntityQuery m_Group;
//
//        protected override void OnCreate()
//        {
//            m_Group = GetEntityQuery(ComponentType.Exclude<TweenPaused>(), ComponentType.Exclude<TweenComplete>(),
//                ComponentType.ReadOnly<TweenEaseSpring>());
//        }
//
//        [BurstCompile]
//        struct EaseJob : IJobParallelFor
//        {
//            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Entity> Entities;
//            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<TweenBase> Tweens;
//
//            public void Execute(int index)
//            {
//                var _index = Entities[index];
//                var tween = Tweens[_index];
//                float value = tween.NormalizedTime;
//                value = math.clamp(value, 0.0f, 1.0f);
//                value =
//                (math.sin(value * (float) math.PI * (0.2f + 2.5f * value * value * value)) *
//                 math.pow(1f - value, 2.2f) +
//                 value) * (1f + (1f - value) * 1.2f);
//                tween.Value = value;
//                Tweens[_index] = tween;
//            }
//        }
//
//        protected override JobHandle OnUpdate(JobHandle inputDeps)
//        {
//            var total = m_Group.CalculateLength();
//            if (total == 0) return inputDeps;
//            var job = new EaseJob
//            {
//                Entities = m_Group.ToEntityArray(Allocator.TempJob),
//                Tweens = GetComponentDataFromEntity<TweenBase>(false)
//            }.Schedule(total, 1, inputDeps);
//            return job;
//        }
//    }
}