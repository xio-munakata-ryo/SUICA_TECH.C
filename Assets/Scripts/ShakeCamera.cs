using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    private Vector3 _initPosition;

    // Start is called before the first frame update
    void Start()
    {
        _initPosition = this.transform.position;
    }

    /// <summary>カメラを縦揺れさせる</summary>
    /// <param name="duration">一回ごとの動作時間</param>
    /// <param name="verSwingPow">揺れの大きさ</param>
    public IEnumerator Cor_ShakeVertical(float duration, float verSwingPow = 0.2f)
    {
        float time = 0.0f;

        // 初期位置から上へ
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            Vector3 targetPos = new Vector3(_initPosition.x, _initPosition.y + verSwingPow, _initPosition.z);
            this.transform.position = Vector3.Lerp(_initPosition, targetPos, t);
            yield return null;
        }

        // 初期位置より下へ
        Vector3 startPos = this.transform.position;
        time = 0.0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            Vector3 targetPos = new Vector3(_initPosition.x, _initPosition.y - verSwingPow, _initPosition.z);
            this.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // 初期位置へ
        startPos = this.transform.position;
        time = 0.0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            Vector3 targetPos = _initPosition;
            this.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
    }
}
