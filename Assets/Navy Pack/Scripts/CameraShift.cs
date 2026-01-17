using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    [SerializeField]
    private Camera _thisCamera;
    [SerializeField]
    private Camera _nextCamera;
    [SerializeField]
    private float _shiftSpeedSec = 0f;

    private float _timeFrameSec = 0f;

    void Update()
    {
        _timeFrameSec += Time.deltaTime;

        if (_timeFrameSec >= _shiftSpeedSec)
        {
            // カメラ切り替え
            _thisCamera.gameObject
                .SetActive(false);

            _nextCamera.gameObject
                .SetActive(true);

            // 自分自身のフレーム更新
            _timeFrameSec = 0f;
        }
    }
}
