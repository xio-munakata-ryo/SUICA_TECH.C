using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFoward : MonoBehaviour
{
    private Transform _transform;

    private Vector3 _defPosition;

    [SerializeField]

    private float _speedPerSec = 1f;

    [SerializeField]

    private float _animationTimeSec = 1f;

    private float _timeFrameSec = 0f;

    private bool _isMoving = true;


    // Start is called before the first frame update
    void Start()
    {
        // 同GameObjectにくっついてるComponentクラスをタイプ指定して取得してくる
        _transform = this.gameObject.GetComponent<Transform>();
        _defPosition = transform.position;
        _isMoving = true;
    }

    private void SetDefault()
    {
        _timeFrameSec = _timeFrameSec - _animationTimeSec; // アニメーションの抜け時間を補完
        transform.position 
            = _defPosition + transform.forward * _speedPerSec * _timeFrameSec;
        _isMoving = true; // TODO 将来的に、アニメーションを再度起動するメソッドをつける
    }

    // Update is called once per frame
    void Update()
    {
       // 以降の処理を行いません、ガード節
        if (_isMoving == false)
        {
            SetDefault();
            return;
        }

        _timeFrameSec += Time.deltaTime;
        float per1 = _timeFrameSec / _animationTimeSec;

        if (per1 >= 1f)
        {
            transform.position 
                = _defPosition + transform.forward * _speedPerSec * _animationTimeSec;
            _isMoving = false;
        }
        else 
        {

            // 指定秒数で指定方向（今回は Foward）に向かって動く
            _transform.position += transform.forward * _speedPerSec * Time.deltaTime;
        }
    }
}
