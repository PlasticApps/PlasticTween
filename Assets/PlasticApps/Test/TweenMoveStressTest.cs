using PlasticApps.Components;
using PlasticApps.Components.Ease;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlasticApps.Test
{
    public class TweenMoveStressTest : MonoBehaviour
    {
        public GameObject prefab;

        public int CountX = 100;
        public int CountY = 100;

        [System.Serializable]
        public struct TweenProp
        {
            public float duration;
            public EaseType easeType;
            public int loop;
            public bool pinPong;
        }

        public TweenProp moveProp;
        public TweenProp scaleProp;
        public TweenProp rotateProp;

        public float height = 10.0f;
        public float timeOffsetX = 0.1f;
        public float timeOffsetY = 0.2f;

        [Range(0.5f, 4)] public float timeScale = 1;

        private TweenProp m_LastMove;
        private TweenProp m_LastScale;

        void Start()
        {
            m_LastMove = moveProp;
            m_LastScale = scaleProp;

            Time.timeScale = timeScale;

            Entity prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.prefab, World.Active);
            var entityManager = World.Active.EntityManager;

            float halfX = CountX / 2.0f;
            float halfY = CountX / 2.0f;
            Vector2 c = new Vector2(halfX, halfY);

            for (int x = 0; x < CountX; x++)
            {
                for (int y = 0; y < CountY; y++)
                {
                    var instance = entityManager.Instantiate(prefab);
                    var position = transform.TransformPoint(new float3(x * 1.3F,
                        noise.cnoise(new float2(x, y) * 0.21F) * 2, y * 1.3F));

                    if (entityManager.HasComponent(instance, typeof(Translation)))
                        entityManager.SetComponentData(instance, new Translation {Value = position});
                    else
                        entityManager.AddComponentData(instance, new Translation {Value = position});

                    if (entityManager.HasComponent(instance, typeof(NonUniformScale)))
                        entityManager.SetComponentData(instance, new NonUniformScale {Value = Vector3.one});
                    else
                        entityManager.AddComponentData(instance, new NonUniformScale {Value = Vector3.one});

                    if (entityManager.HasComponent(instance, typeof(RotationEulerXYZ)))
                        entityManager.SetComponentData(instance, new RotationEulerXYZ());
                    else
                        entityManager.AddComponentData(instance, new RotationEulerXYZ());


                    float offset = 1.0f - Vector2.Distance(new Vector2(x, y), c) / c.magnitude;
                    offset *= height;

                    float tx = timeOffsetX * Mathf.Min(Mathf.Abs(CountX - x), x);
                    float ty = timeOffsetY; // * Mathf.Abs(y - halfY);// * Mathf.Min(Mathf.Abs(CountY - y), y);

                    Tween.Delay((tx + ty), () =>
                    {
//                        
                        Tween.MoveEntity(instance, moveProp.duration,
                            position + Vector3.up * offset, position,
                            moveProp.easeType, moveProp.loop, moveProp.pinPong, x);

                        Tween.ScaleEntity(instance, scaleProp.duration,
                            Vector3.one * 0.01f, Vector3.one,
                            scaleProp.easeType, scaleProp.loop, scaleProp.pinPong, x);

                        Tween.RotateEntity(instance, rotateProp.duration,
                            new Vector3(-30, 0, 0) * Mathf.Deg2Rad,
                            new Vector3(60, 360, 0) * Mathf.Deg2Rad,
                            rotateProp.easeType, rotateProp.loop, rotateProp.pinPong, x);
                    });
                }
            }
        }

        void Update()
        {
            if (!m_LastMove.Equals(moveProp))
            {
                m_LastMove = moveProp;
                ForceTweenUpdate(m_LastMove.duration, m_LastMove.easeType, false);
            }
            if (!m_LastScale.Equals(scaleProp))
            {
                m_LastScale = scaleProp;
                ForceTweenUpdate(m_LastScale.duration, m_LastScale.easeType, true);
            }
        }

        void ForceTweenUpdate(float duration, EaseType easeType, bool scale)
        {
            var manager = World.Active.EntityManager;
            var entities = manager.GetAllEntities(Allocator.TempJob);
            for (var i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                if (manager.HasComponent(entity, scale ? typeof(TweenScale) : typeof(TweenMove)))
                {
                    var t = manager.GetComponentData<TweenBase>(entity);
                    manager.RemoveComponent(entity, TypeManager.GetType(t.EaseTypeId));
                    t.Duration = duration;
                    t.EaseTypeId = Tween.AddEase(entity, easeType);
                    manager.SetComponentData(entity, t);
                }
            }
            entities.Dispose();
        }
    }
}