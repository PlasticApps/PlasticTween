# PlasticTween
**Tween Library for Unity3D(ECS+JOBS)**


Testing ECS+Jobs - Tween Library implementation.
Works with Entity(as relationship) and GameObjects, also has 12 eases(Linear, Lerp, Quad, Cubic, Quint, Spring, Sine, Back, Bounce, Expo, Elastic, Circ, Square).

**Requirements: Unity3D 2019.1.0b9 or later**


_Callbacks_

````
Tween.Delay(1.0f, () => { // do things })
Tween.MoveEntity(...).OnTweenComplete(() => {});
````

_Entities_

````
Tween.MoveEntity(
    entity, 
    duration,
    targetPosition, originPosition,
    EasyType.Spring, 
    -1, 
    true, 
    -1
);
````

_GameObjects_

````
Tween.MoveGameObject(
    gameObject, 
    duration, 
    targetVector, 
    EasyType.Spring, 
    -1, 
    isPingPong
); 
````

_Pause and Stop_
````
Tween.PauseByTag(int tagId);
Tween.UnPauseByTag(int tagId);
Tween.PauseAll();
Tween.UnPauseAll();

Tween.StopByTag(int tagId);
Tween.StopAll();
````

_Same projects_
ECS-Tween - https://github.com/Xerios/ECS-Tween
