using PlasticApps.Components;
using PlasticApps.Systems.Groups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;

namespace PlasticApps.Systems.SubSystems
{
    [UpdateInGroup(typeof(TweenSystems))]
    [UpdateAfter(typeof(TweenValuesSystems))]
    public class TweenTransformSystem : JobComponentSystem
    {
//        [BurstCompile]
        struct ApplyMoveJob : IJobParallelForTransform
        {
            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Translation> positions;

            public void Execute(int index, TransformAccess transform)
            {
                transform.position = positions[index].Value;
            }
        }

        EntityQuery m_TransformGroup;

        protected override void OnCreateManager()
        {
            m_TransformGroup =
                GetEntityQuery(ComponentType.ReadOnly(typeof(TweenBase)),
                    ComponentType.ReadOnly(typeof(TweenGameObject)), ComponentType.ReadOnly(typeof(Translation)),
                    ComponentType.ReadWrite<Transform>());
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            int total = m_TransformGroup.CalculateLength();
            if (total == 0) return inputDeps;

            var transforms = m_TransformGroup.GetTransformAccessArray();
            var positions = m_TransformGroup.ToComponentDataArray<Translation>(Allocator.TempJob);
            var applyMoveJob = new ApplyMoveJob
            {
                positions = positions,
            };

            return applyMoveJob.Schedule(transforms, inputDeps);
        }
    }
}