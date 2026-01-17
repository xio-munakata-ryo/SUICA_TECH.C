using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 originalCameraPos;

    private void Start()
    {
        // カメラの位置を取得
        originalCameraPos = Camera.main.transform.position;
        Shake();
    }

    public void Shake()
    {
        StartCoroutine("ShakeCamera");
    }

    IEnumerator ShakeCamera()
    {
        // カメラを揺らす
        for (int i = 0; i < 5; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * 0.5f;
            Camera.main.transform.position = new Vector3(randomPos.x, randomPos.y, originalCameraPos.z);
            yield return null;
        }

        // カメラを元の位置に戻す
        Camera.main.transform.position = originalCameraPos;
    }
}