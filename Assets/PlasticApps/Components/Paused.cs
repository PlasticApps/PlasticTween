using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace PlasticApps.Components
{
    /// <summary>
    /// Tag - Paused Tweens excluded from update systems
    /// </summary>
    public struct TweenPaused : IComponentData
    {
    }
}