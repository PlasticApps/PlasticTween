//#define SINGLE_ENTITY

#define JOB_EACH
using PlasticApps.Systems.Groups;
using PlasticApps.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace PlasticApps.Systems
{
    [UpdateInGroup(typeof(TweenValuesSystems))]
    public class TweenPositionSystem : JobComponentSystem
    {
        // first implementation tween attach to source entity 
#if SINGLE_ENTITY
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        struct TweenValueJob : IJobForEach<TweenBase, TweenMove, Translation>
        {
            public void Execute([ReadOnly] ref TweenBase tween, [ReadOnly] ref TweenMove move,
                ref Translation position)
            {
                position.Value = math.lerp(move.From, move.To, tween.Value);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new TweenValueJob().Schedule(this, inputDeps);
        }

        #else
#if !JOB_EACH
//        second implementation tween entity with tweening entity 
//        [BurstCompile]
        struct TweenValueJob : IJobParallelFor
        {
            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<TweenEntity> Tweens;
            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<TweenMove> Moves;
            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<TweenBase> Bases;

            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Translation> Translations;

            public void Execute(int index)
            {
                Entity transformEntity = Tweens[index].Value;
                var move = Moves[index];
                var tween = Bases[index];
                if (Translations.Exists(transformEntity))
                {
                    var v = math.lerp(move.From, move.To, tween.Value);
                    Translations[transformEntity] = new Translation {Value = v};
                }
            }
        }

        private EntityQuery m_QueryCompleted;

        protected override void OnCreate()
        {
            m_QueryCompleted = GetEntityQuery(
                ComponentType.ReadOnly<TweenBase>(),
                ComponentType.ReadOnly<TweenMove>(),
                ComponentType.ReadOnly<TweenEntity>(),
                ComponentType.Exclude<TweenComplete>(),
                ComponentType.Exclude<TweenPaused>());
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            int total = m_QueryCompleted.CalculateLength();
            var moveRigidbodies = new TweenValueJob
            {
                Translations = GetComponentDataFromEntity<Translation>(false),
                Tweens = m_QueryCompleted.ToComponentDataArray<TweenEntity>(Allocator.TempJob),
                Moves = m_QueryCompleted.ToComponentDataArray<TweenMove>(Allocator.TempJob),
                Bases = m_QueryCompleted.ToComponentDataArray<TweenBase>(Allocator.TempJob)
            }.Schedule(total, 64, inputDeps);

            return moveRigidbodies;
        }
    #else
//        3 implementation 
//        [BurstCompile]
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        struct TweenValueJob : IJobForEach<TweenBase, TweenMove, TweenEntity>
        {
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Translation> Translations;

            public void Execute([ReadOnly] ref TweenBase tween, [ReadOnly] ref TweenMove move,
                [ReadOnly] ref TweenEntity entity)
            {
                if (Translations.Exists(entity.Value))
                {
                    Translations[entity.Value] = new Translation
                    {
                        Value = math.lerp(move.From, move.To, tween.Value)
                    };
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new TweenValueJob
            {
                Translations = GetComponentDataFromEntity<Translation>()
            }.Schedule(this, inputDeps);
        }
    #endif
#endif
    }
}