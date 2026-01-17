using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CameraShaker : SingletonMonoBehaviour<CameraShaker>
{
    private float _defaultShakeDuration = 0.2f;
    private float _defaultShakeMagnitude = 0.3f;
    private Vector3 _originPos = Vector3.zero;

    protected override bool dontDestroyOnLoad => true;

    public async UniTask Shake(Transform camera, float duration, float magnitude, float speed)
    {
        _originPos = camera.localPosition;
        float timer = 0.0f;

        while (timer < duration)
        {
            float x = Mathf.PerlinNoise(Time.time * speed, 0.0f) - 0.5f;
            float y = Mathf.PerlinNoise(0.0f, Time.time * speed) - 0.5f;

            camera.localPosition = new Vector3
            (
                _originPos.x + x * magnitude,
                _originPos.y + y * magnitude,
                _originPos.z
            );

            timer += Time.deltaTime;
            await UniTask.Yield();
        }

        camera.localPosition = Vector3.Lerp(transform.position, _originPos, Time.deltaTime * 5f);
    }
}
