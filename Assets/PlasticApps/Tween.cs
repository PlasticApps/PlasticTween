//#define TWEEN_AS_COMPONENT

using Unity.Entities;
using UnityEngine;
using PlasticApps.Components;
using PlasticApps.Components.Ease;
using PlasticApps.Systems;
using Unity.Mathematics;
using Unity.Transforms;


namespace PlasticApps
{
    public static class Tween
    {
        public static int AddEase(Entity entity, EaseType ease)
        {
            var entityManager = World.Active.EntityManager;
            int easeTypeIndex;
            switch (ease)
            {
                case EaseType.linear:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseLinear));
                    entityManager.AddComponentData(entity, new TweenEaseLinear());
                    break;
                case EaseType.spring:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseSpring));
                    entityManager.AddComponentData(entity, new TweenEaseSpring());
                    break;
                case EaseType.easeInQuad:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInQuad));
                    entityManager.AddComponentData(entity, new TweenEaseInQuad());
                    break;
                case EaseType.easeOutQuad:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutQuad));
                    entityManager.AddComponentData(entity, new TweenEaseOutQuad());
                    break;
                case EaseType.easeInOutQuad:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutQuad));
                    entityManager.AddComponentData(entity, new TweenEaseInOutQuad());
                    break;
                case EaseType.easeInCubic:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInCubic));
                    entityManager.AddComponentData(entity, new TweenEaseInCubic());
                    break;
                case EaseType.easeOutCubic:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutCubic));
                    entityManager.AddComponentData(entity, new TweenEaseOutCubic());
                    break;
                case EaseType.easeInOutCubic:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutCubic));
                    entityManager.AddComponentData(entity, new TweenEaseInOutCubic());
                    break;
                case EaseType.easeInQuart:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInQuart));
                    entityManager.AddComponentData(entity, new TweenEaseInQuart());
                    break;
                case EaseType.easeOutQuart:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutQuart));
                    entityManager.AddComponentData(entity, new TweenEaseOutQuart());
                    break;
                case EaseType.easeInOutQuart:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutQuart));
                    entityManager.AddComponentData(entity, new TweenEaseInOutQuart());
                    break;
                case EaseType.easeInQuint:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInQuint));
                    entityManager.AddComponentData(entity, new TweenEaseInQuint());
                    break;
                case EaseType.easeOutQuint:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutQuint));
                    entityManager.AddComponentData(entity, new TweenEaseOutQuint());
                    break;
                case EaseType.easeInOutQuint:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutQuint));
                    entityManager.AddComponentData(entity, new TweenEaseInOutQuint());
                    break;
                case EaseType.easeInSine:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInSine));
                    entityManager.AddComponentData(entity, new TweenEaseInSine());
                    break;
                case EaseType.easeOutSine:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutSine));
                    entityManager.AddComponentData(entity, new TweenEaseOutSine());
                    break;
                case EaseType.easeInOutSine:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutSine));
                    entityManager.AddComponentData(entity, new TweenEaseInOutSine());
                    break;
                case EaseType.easeInExpo:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInExpo));
                    entityManager.AddComponentData(entity, new TweenEaseInExpo());
                    break;
                case EaseType.easeOutExpo:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutExpo));
                    entityManager.AddComponentData(entity, new TweenEaseOutExpo());
                    break;
                case EaseType.easeInOutExpo:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutExpo));
                    entityManager.AddComponentData(entity, new TweenEaseInOutExpo());
                    break;
                case EaseType.easeInCirc:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInCirc));
                    entityManager.AddComponentData(entity, new TweenEaseInCirc());
                    break;
                case EaseType.easeOutCirc:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutCirc));
                    entityManager.AddComponentData(entity, new TweenEaseOutCirc());
                    break;
                case EaseType.easeInOutCirc:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutCirc));
                    entityManager.AddComponentData(entity, new TweenEaseInOutCirc());
                    break;
                case EaseType.easeInBounce:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInBounce));
                    entityManager.AddComponentData(entity, new TweenEaseInBounce());
                    break;
                case EaseType.easeOutBounce:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutBounce));
                    entityManager.AddComponentData(entity, new TweenEaseOutBounce());
                    break;
                case EaseType.easeInOutBounce:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutBounce));
                    entityManager.AddComponentData(entity, new TweenEaseInOutBounce());
                    break;
                case EaseType.easeInBack:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInBack));
                    entityManager.AddComponentData(entity, new TweenEaseInBack());
                    break;
                case EaseType.easeOutBack:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutBack));
                    entityManager.AddComponentData(entity, new TweenEaseOutBack());
                    break;
                case EaseType.easeInOutBack:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutBack));
                    entityManager.AddComponentData(entity, new TweenEaseInOutBack());
                    break;
                case EaseType.easeInElastic:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInElastic));
                    entityManager.AddComponentData(entity, new TweenEaseInElastic());
                    break;
                case EaseType.easeOutElastic:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutElastic));
                    entityManager.AddComponentData(entity, new TweenEaseOutElastic());
                    break;
                case EaseType.easeInOutElastic:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutElastic));
                    entityManager.AddComponentData(entity, new TweenEaseInOutElastic());
                    break;
                case EaseType.easeInSquare:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInSquare));
                    entityManager.AddComponentData(entity, new TweenEaseInSquare());
                    break;
                case EaseType.easeOutSquare:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseOutSquare));
                    entityManager.AddComponentData(entity, new TweenEaseOutSquare());
                    break;
                case EaseType.easeInOutSquare:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutSquare));
                    entityManager.AddComponentData(entity, new TweenEaseInOutSquare());
                    break;
                default:
                    easeTypeIndex = TypeManager.GetTypeIndex(typeof(TweenEaseInOutQuad));
                    entityManager.AddComponentData(entity, new TweenEaseInOutQuad());
                    break;
            }
            return easeTypeIndex;
        }

        public static Entity MoveGameObject(
            GameObject go, float time, Vector3 to)
        {
            return MoveGameObject(go, time, to, go.transform.position, EaseType.linear);
        }

        public static Entity MoveGameObject(
            GameObject go, float time, Vector3 to, EaseType easeType, int loop = -1, bool pingPong = false)
        {
            return MoveGameObject(go, time, to, go.transform.position, easeType, loop, pingPong);
        }


        public static Entity MoveGameObject(
            GameObject go,
            float time,
            Vector3 to, Vector3 from,
            EaseType easeType = EaseType.linear,
            int loop = -1, bool pingPong = false,
            int tagId = 0)
        {
            float3 from3 = new float3(from.x, from.y, from.z);
            float3 to3 = new float3(to.x, to.y, to.z);

            var entityManager = World.Active.EntityManager;
            var entity = GameObjectEntity.AddToEntityManager(entityManager, go);
            entityManager.AddComponentData(entity, new Translation {Value = from3});
            entityManager.AddComponentData(entity, new TweenGameObject());
            MoveEntity(entity, time, to3, from3, easeType, loop, pingPong, tagId);
            return entity;
        }

        public static Entity RotateGameObject(
            GameObject go,
            float time,
            Vector3 to, Vector3 from,
            EaseType easeType = EaseType.linear,
            int loop = -1, bool pingPong = false,
            int tagId = 0)
        {
            float3 from3 = new float3(from.x, from.y, from.z);
            float3 to3 = new float3(to.x, to.y, to.z);

            var entityManager = World.Active.EntityManager;
            var entity = GameObjectEntity.AddToEntityManager(entityManager, go);
            entityManager.AddComponentData(entity, new Translation {Value = from3});
            entityManager.AddComponentData(entity, new TweenGameObject());
            RotateEntity(entity, time, to3, from3, easeType, loop, pingPong, tagId);
            return entity;
        }
        
        public static Entity ScaleGameObject(
            GameObject go,
            float time,
            Vector3 to, Vector3 from,
            EaseType easeType = EaseType.linear,
            int loop = -1, bool pingPong = false,
            int tagId = 0)
        {
            float3 from3 = new float3(from.x, from.y, from.z);
            float3 to3 = new float3(to.x, to.y, to.z);

            var entityManager = World.Active.EntityManager;
            var entity = GameObjectEntity.AddToEntityManager(entityManager, go);
            entityManager.AddComponentData(entity, new Translation {Value = from3});
            entityManager.AddComponentData(entity, new TweenGameObject());
            ScaleEntity(entity, time, to3, from3, easeType, loop, pingPong, tagId);
            return entity;
        }
        
        static Entity InstantiateTweenSource(Entity entitySource, float time, EaseType easeType, int loop,
            byte pingPong,
            int tagId)
        {
            var entityManager = World.Active.EntityManager;
            Entity entity;

#if TWEEN_AS_COMPONENT
            entity = entitySource;
#else
            if (entityManager.HasComponent(entitySource, typeof(TweenGameObject)))
                entity = entitySource;
            else
                entity = entityManager.CreateEntity();
#endif

            if (!entityManager.HasComponent(entity, typeof(TweenBase)))
                InstantiateTween(entity, time, easeType, loop, pingPong, tagId);

            if (!entityManager.HasComponent(entity, typeof(TweenEntity)))
                entityManager.AddComponentData(entity, new TweenEntity {Value = entitySource});

            return entity;
        }

        static Entity InstantiateTween(Entity entity, float time, EaseType easeType, int loop, byte pingPong, int tagId)
        {
            var entityManager = World.Active.EntityManager;
            entityManager.AddComponentData(entity, new TweenBase
            {
                Duration = time,
                Loop = loop,
                PingPong = pingPong,
                EaseTypeId = AddEase(entity, easeType)
            });
            entityManager.AddComponentData(entity, new TweenTag {Value = tagId});
            return entity;
        }

        public static Entity MoveEntity(
            Entity entitySource,
            float time,
            float3 to3, float3 from3,
            EaseType easeType = EaseType.linear,
            int loop = -1, bool pingPong = false,
            int tagId = 0, bool self = false)
        {
            var entityManager = World.Active.EntityManager;
            var entity = InstantiateTweenSource(entitySource, time, easeType, loop, (byte) (pingPong ? 1 : 0), tagId);
            if (!entityManager.HasComponent(entity, typeof(TweenMove)))
                entityManager.AddComponentData(entity, new TweenMove {From = from3, To = to3});

            return entity;
        }

        public static Entity RotateEntity(
            Entity entitySource,
            float time,
            float3 to3, float3 from3,
            EaseType easeType = EaseType.linear,
            int loop = -1, bool pingPong = false,
            int tagId = 0, bool self = false)
        {
            var entityManager = World.Active.EntityManager;
            var entity = InstantiateTweenSource(entitySource, time, easeType, loop, (byte) (pingPong ? 1 : 0), tagId);
            if (!entityManager.HasComponent(entity, typeof(TweenRotate)))
                entityManager.AddComponentData(entity, new TweenRotate {From = from3, To = to3});

            return entity;
        }

        public static Entity ScaleEntity(
            Entity entitySource,
            float time,
            float3 to3, float3 from3,
            EaseType easeType = EaseType.linear,
            int loop = -1, bool pingPong = false,
            int tagId = 0, bool self = false)
        {
            var entityManager = World.Active.EntityManager;
            var entity = InstantiateTweenSource(entitySource, time, easeType, loop, (byte) (pingPong ? 1 : 0), tagId);
            if (!entityManager.HasComponent(entity, typeof(TweenScale)))
                entityManager.AddComponentData(entity, new TweenScale {From = from3, To = to3});
            return entity;
        }


        public static Entity Delay(float time, System.Action callback, int tagId = -1)
        {
            var entityManager = World.Active.EntityManager;
            var entity = InstantiateTween(entityManager.CreateEntity(), time, EaseType.linear, 0, 0x0, tagId);
            entity.OnTweenComplete(callback);
            return entity;
        }

        public static void OnTweenComplete(this Entity entity, System.Action callback)
        {
            var entityManager = World.Active.EntityManager;
            if (entityManager.HasComponent(entity, typeof(TweenBase)))
                World.Active.GetOrCreateSystem<TweenCommandSystem>().AddCompleteCallback(entity, callback);
        }

        public static void PauseByTag(int tagId)
        {
            World.Active.GetOrCreateSystem<TweenCommandSystem>().PauseTween(tagId);
        }

        public static void PauseAll()
        {
            World.Active.GetOrCreateSystem<TweenCommandSystem>().PauseTween(-1);
        }

        public static void UnPauseByTag(int tagId)
        {
            World.Active.GetOrCreateSystem<TweenCommandSystem>().UnPauseTween(tagId);
        }

        public static void UnPauseAll()
        {
            World.Active.GetOrCreateSystem<TweenCommandSystem>().UnPauseTween(-1);
        }

        public static void StopByTag(int tagId)
        {
            World.Active.GetOrCreateSystem<TweenCommandSystem>().StopTween(tagId);
        }

        public static void StopAll()
        {
            World.Active.GetOrCreateSystem<TweenCommandSystem>().StopTween(-1);
        }
    }
}