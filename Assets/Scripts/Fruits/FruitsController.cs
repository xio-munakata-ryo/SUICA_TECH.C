using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Score;
using Cysharp.Threading.Tasks;
public class FruitsController : PooledObject
{
    [SerializeField, Header("アクティブであれる最低のY座標")]
    private float _activableYPosMin = -10f;

    private FruitsData _fruitsData;

    protected override void OnReleaseAction()
    {
        MainGameManager.Instance.ListFruitsController.Add(this);
    }

    protected override void OnStackAction()
    {
        MainGameManager.Instance.ListFruitsController.Remove(this);
    }

    public void SetFruitsType(FruitsData fruitsData)
    {
        _fruitsData = fruitsData;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= _activableYPosMin)
        {
            PoolManager.Instance.StackObject(this);
            MainGameManager.Instance.OnGameOver();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (MainGameManager.Instance.IsGameOver) return;
        if (collision.gameObject.layer != this.gameObject.layer) return;

        FruitsController other = null;
        foreach (var fruit in MainGameManager.Instance.ListFruitsController)
        {
            if (fruit.gameObject == collision.gameObject)
            {
                other = fruit;
                break;
            }
        }
        if (other == null) return;

        if (other._fruitsData.Type != _fruitsData.Type) return;

        CameraShaker.Instance.Shake(Camera.main.transform, 0.2f, 0.3f, 5f).Forget();
        PoolManager.Instance.StackObject(other);
        FruitsType nextType = _fruitsData.Type + 1;
        PoolManager.Instance.StackObject(this);
        ScoreManager.AddScore((int)nextType * MainGameManager.Instance.ScoreMultiply);
        if (nextType == FruitsType.Max) return;

        MainGameManager.Instance.GenerateFruitsByType(nextType, transform.position);
    }
}
