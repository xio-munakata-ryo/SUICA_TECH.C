using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private static CameraShaker _instance;
    public static CameraShaker Instance =>_instance;

    [SerializeField] CinemachineImpulseSource _implseSource;

    void Awake()
    {
        _instance = this;
    }

    public void Shake()
    {
        _implseSource.GenerateImpulse();
    }
}
