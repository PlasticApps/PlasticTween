using System.Collections.Generic;
using PlasticApps.Components;
using PlasticApps.Systems.Groups;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace PlasticApps.Systems
{
    // main thread for using callbacks
    [UpdateBefore(typeof(TweenEraseSystem))]
    [UpdateInGroup(typeof(TweenSystems))]
    public class TweenCommandSystem : ComponentSystem
    {
        private EntityCommandBufferSystem m_EndFrameSystem;

        private EntityQuery m_AllTweens;
        private EntityQuery m_PlayingTweens;
        private EntityQuery m_PausedTweens;
        private EntityQuery m_TweenCompleteCallbacks;

        private Dictionary<int, System.Action> m_Callbacks;

        protected override void OnCreate()
        {
            m_Callbacks = new Dictionary<int, System.Action>();
            m_EndFrameSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();

            m_AllTweens = GetEntityQuery(ComponentType.ReadOnly<TweenTag>(), ComponentType.Exclude<TweenComplete>());

            m_PlayingTweens = GetEntityQuery(ComponentType.ReadOnly<TweenTag>(), ComponentType.Exclude<TweenPaused>(),
                ComponentType.Exclude<TweenComplete>());

            m_PausedTweens = GetEntityQuery(ComponentType.ReadOnly<TweenTag>(), ComponentType.ReadOnly<TweenPaused>(),
                ComponentType.Exclude<TweenComplete>());

            m_TweenCompleteCallbacks = GetEntityQuery(ComponentType.ReadOnly<TweenCompleteCallback>(),
                ComponentType.ReadOnly<TweenComplete>());
        }

        protected override void OnDestroy()
        {
            m_Callbacks.Clear();
        }

        private void AddCommandTween<T>(int tagId, T command, EntityQuery tweens) where T : struct, IComponentData
        {
            var totalTweens = tweens.CalculateLength();
            if (totalTweens == 0) return;
            var entities = tweens.ToEntityArray(Allocator.TempJob);
            var tags = tweens.ToComponentDataArray<TweenTag>(Allocator.TempJob);
            var commandBuffer = m_EndFrameSystem.CreateCommandBuffer();

            for (int i = 0; i < totalTweens; i++)
            {
                var entity = entities[i];
                var tag = tags[i];
                if (tagId == -1 || tag.Value == tagId)
                {
                    commandBuffer.AddComponent<T>(entity, command);
                }
            }

            entities.Dispose();
            tags.Dispose();
        }

        private void RemoveCommandTween<T>(int tagId, EntityQuery tweens) where T : struct, IComponentData
        {
            var totalTweens = tweens.CalculateLength();
            if (totalTweens == 0) return;
            var entities = tweens.ToEntityArray(Allocator.TempJob);
            var tags = tweens.ToComponentDataArray<TweenTag>(Allocator.TempJob);
            var commandBuffer = m_EndFrameSystem.CreateCommandBuffer();

            for (int i = 0; i < totalTweens; i++)
            {
                var entity = entities[i];
                var tag = tags[i];
                if (tagId == -1 || tag.Value == tagId)
                {
                    commandBuffer.RemoveComponent(entity, new ComponentType(typeof(T)));
                }
            }
            entities.Dispose();
            tags.Dispose();
        }

        public void StopTween(int tagId = -1)
        {
            AddCommandTween(tagId, new TweenComplete(), m_AllTweens);
        }

        public void PauseTween(int tagId = -1)
        {
            AddCommandTween(tagId, new TweenPaused(), m_PlayingTweens);
        }

        public void UnPauseTween(int tagId = -1)
        {
            RemoveCommandTween<TweenPaused>(tagId, m_PausedTweens);
        }

        public void AddCompleteCallback(Entity entity, System.Action action)
        {
            m_Callbacks.Add(entity.Index, action);
            EntityManager.AddComponentData(entity, new TweenCompleteCallback {Value = entity.Index});
        }

        protected override void OnUpdate()
        {
            int total = m_TweenCompleteCallbacks.CalculateLength();
            if (total == 0) return;
            var entities = m_TweenCompleteCallbacks.ToEntityArray(Allocator.TempJob);
            for (int i = 0; i < total; i++)
            {
                var id = entities[i].Index;
                if (m_Callbacks.ContainsKey(id))
                {
                    m_Callbacks[id].Invoke();
                    m_Callbacks.Remove(id);
                }
            }
            entities.Dispose();
        }
    }
}