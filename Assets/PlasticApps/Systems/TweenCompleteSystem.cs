using Unity.Jobs;
using Unity.Entities;
using PlasticApps.Components;
using PlasticApps.Systems.Groups;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlasticApps.Systems
{
    [UpdateAfter(typeof(TweenUpdateSystem))]
    [UpdateInGroup(typeof(TweenSystems))]
    public class TweenCompleteSystem : JobComponentSystem
    {
        [ExcludeComponent(typeof(TweenPaused), typeof(TweenComplete))]
        struct JobComplete : IJobForEachWithEntity<TweenBase>
        {
            [ReadOnly] public EntityCommandBuffer CommandBuffer;

            public void Execute(Entity entity, int index, ref TweenBase tween)
            {
                if ((tween.NormalizedTime >= 1 && tween.Reversed == 0) ||
                    (tween.NormalizedTime <= 0 && tween.Reversed == 1))
                {
                    if (tween.PingPong == 1 && tween.Reversed == 0)
                    {
                        tween.Reversed = 1;
                        tween.Time -= tween.Duration;
                    }
                    else if (tween.Loop == -1 || tween.Loop > 0)
                    {
                        if (tween.Loop > 0)
                            tween.Loop -= 1;
                        tween.Reversed = 0;
                        tween.Time -= tween.Duration;
                    }
                    else
                    {
                        CommandBuffer.AddComponent(entity, new TweenComplete());
                    }
                }
            }
        }

        private EntityCommandBufferSystem m_EndFrameSystem;

        protected override void OnCreateManager()
        {
            m_EndFrameSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new JobComplete()
            {
                CommandBuffer = m_EndFrameSystem.CreateCommandBuffer()
            }.ScheduleSingle(this, inputDeps);
            m_EndFrameSystem.AddJobHandleForProducer(job);
            return job;
        }
    }
}