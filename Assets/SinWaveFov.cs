using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaveFov : MonoBehaviour
{
    [SerializeField]
    private Camera _targetCamera = null;

    private float _defFov = 60f;

    [SerializeField]
    private float _clcleTimeSec = 1f;

    [SerializeField]
    private float _SinMoveValue = 10f;

    [SerializeField]
    private float _timeFlameSec = 0f;


    // Start is called before the first frame update
    void Start()
    {
        _defFov = _targetCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        _timeFlameSec += Time.deltaTime;
        if(_timeFlameSec > 256f) _timeFlameSec -= 256f;

        _targetCamera.fieldOfView
            = Mathf.Sin(Mathf.PI * 0.5f / _clcleTimeSec * _timeFlameSec) * _SinMoveValue
            + _defFov;
    }
}
