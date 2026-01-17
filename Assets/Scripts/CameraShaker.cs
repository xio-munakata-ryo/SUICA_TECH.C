using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;

    private CinemachineBasicMultiChannelPerlin cam;

    public static CameraShaker Instance
    {
        get; private set;
    }

    private float shakeTimer;

    private void Awake()
    {
        Instance = this;

        cam = _cam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                cam.m_AmplitudeGain = 0f;
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        cam.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
}
