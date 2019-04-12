using System;
using System.Collections;
using System.Collections.Generic;
using PlasticApps.Components;
using PlasticApps.Systems.Groups;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Jobs;

namespace PlasticApps.Systems
{
    [UpdateBefore(typeof(TweenCompleteSystem))]
    [UpdateInGroup(typeof(TweenSystems))]
    public class TweenEraseSystem : ComponentSystem
    {
        private EntityQuery m_QueryCompleted;

        protected override void OnCreateManager()
        {
            m_QueryCompleted = GetEntityQuery(ComponentType.ReadOnly<TweenComplete>());
        }

        protected override void OnUpdate()
        {
            int total = m_QueryCompleted.CalculateLength();
            if (total == 0) return;
            var entities = m_QueryCompleted.ToEntityArray(Allocator.TempJob);

            for (int i = 0; i < total; ++i)
                PostUpdateCommands.DestroyEntity(entities[i]);

            entities.Dispose();
        }
    }
}