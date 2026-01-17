using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CameraShaker : SingletonMonoBehaviour<CameraShaker>
{

    protected override bool dontDestroyOnLoad => true;

    /// <summary>
    /// カメラを揺らす
    /// </summary>
    /// <param name="camera">揺らすカメラ</param>
    /// <param name="duration">揺らす秒数</param>
    /// <param name="magnitude">揺れの大きさ</param>
    /// <param name="speed">揺れの速さ</param>
    /// <returns></returns>
    public async UniTask Shake(Transform camera, float duration = 0.2f, float magnitude = 0.6f, float speed = 5.0f)
    {
        Vector3 originPos = camera.localPosition;
        float timer = 0.0f;

        while (timer < duration)
        {
            float x = Mathf.PerlinNoise(Time.time * speed, 0.0f) - 0.5f;
            float y = Mathf.PerlinNoise(0.0f, Time.time * speed) - 0.5f;

            camera.localPosition = new Vector3
            (
                originPos.x + x * magnitude,
                originPos.y + y * magnitude,
                originPos.z
            );

            timer += Time.deltaTime;
            await UniTask.Yield();
        }

        camera.localPosition = Vector3.Lerp(transform.position, originPos, Time.deltaTime * 5f);
    }
}
