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
    public class TweenLocalTransformSystem : JobComponentSystem
    {
        //[BurstCompile]
        struct ApplyTranslationJob : IJobParallelForTransform
        {
            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Translation> positions;

            public void Execute(int index, TransformAccess transform)
            {
                transform.localPosition = positions[index].Value;
            }
        }

        struct ApplyRotationJob : IJobParallelForTransform
        {
            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Rotation> rotations;

            public void Execute(int index, TransformAccess transform)
            {
                transform.localRotation = rotations[index].Value;
            }
        }

        EntityQuery m_TranslateGroup;
        EntityQuery m_RotateGroup;

        protected override void OnCreateManager()
        {
            m_TranslateGroup =
                GetEntityQuery(ComponentType.ReadOnly(typeof(TweenBase)),
                    ComponentType.ReadOnly(typeof(TweenGameObjectLocal)), ComponentType.ReadOnly(typeof(Translation)),
                    ComponentType.ReadWrite<Transform>());

            m_RotateGroup =
                GetEntityQuery(ComponentType.ReadOnly(typeof(TweenBase)),
                    ComponentType.ReadOnly(typeof(TweenGameObjectLocal)), ComponentType.ReadOnly(typeof(Rotation)),
                    ComponentType.ReadWrite<Transform>());
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            int translationLength = m_TranslateGroup.CalculateLength();
            int rotationLength = m_RotateGroup.CalculateLength();
            if (translationLength == 0 && rotationLength == 0) return inputDeps;

            var translateTransforms = m_TranslateGroup.GetTransformAccessArray();
            var positions = m_TranslateGroup.ToComponentDataArray<Translation>(Allocator.TempJob);
            var applyTranslationJob = new ApplyTranslationJob
            {
                positions = positions,
            };

            var translateHandle = applyTranslationJob.Schedule(translateTransforms, inputDeps);

            var rotateTransforms = m_RotateGroup.GetTransformAccessArray();
            var rotations = m_RotateGroup.ToComponentDataArray<Rotation>(Allocator.TempJob);
            var applyRotationJob = new ApplyRotationJob
            {
                rotations = rotations,
            };

            return applyRotationJob.Schedule(rotateTransforms, translateHandle);
        }
    }
}