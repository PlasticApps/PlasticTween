using PlasticApps;
using PlasticApps.Components.Ease;
using UnityEngine;

public class TweenMoveTest : MonoBehaviour
{
    public float delay = 0;
    public float duration;
    public EaseType ease;
    public Vector3 target = Vector3.up * 10;
    public bool isLoop;
    public bool isPingPong;

    void Awake()
    {
        Tween.Delay(delay,
            () => { Tween.MoveGameObject(gameObject, duration, transform.position + target, ease, isLoop ? -1 : 0, isPingPong); });
    }
}