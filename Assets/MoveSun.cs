using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSun : MonoBehaviour
{

    [SerializeField]
    private float _speedMoveSunAnglePerSer = 1f;

    // Update is called once per frame
    void Update()
    {
        var angle = this.transform.eulerAngles;
        angle.x += _speedMoveSunAnglePerSer * Time.deltaTime;
        this.transform.eulerAngles = angle;
            
    }
}
