using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitsController : MonoBehaviour
{
    private Action _onDisappear;

    private static List<FruitsController> _listFruitsController = new List<FruitsController>();
    public static List<FruitsController> ListFruitsController => _listFruitsController;

    private FruitsType _type;
    public FruitsType Type => _type;

    private Data _data;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private Color _color;

    public void SetType(FruitsType type)
    {
        _type = type;
    }

    public void SetData(Data data)
    {
        _data = data;
    }

    public void SetColor(Color color)
    {
        _color = color;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = _color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _listFruitsController.Add(this);
    }

    void OnDestroy()
    {
        _listFruitsController.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -10)
        {
            Destroy(this.gameObject);

            GameManager.SetGameOver();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 3) return;

        FruitsController other = null;
        foreach (var fruit in _listFruitsController)
        {
            if (fruit.gameObject == collision.gameObject)
            {
                other = fruit;
                break;
            }
        }

        if (other == null) return;

        if (other.Type == this.Type)
        {
            // 相手側消す
            Destroy(other.gameObject);

            // 自分成長
            FruitsType nextType = this.Type + 1;

            if (nextType == FruitsType.MAX)
            {
                Destroy(this.gameObject);
            }
            else
            {
                this._type = nextType;
                this._data = new Data(nextType, this);
                this.SetColor(GameManager.ColorPallet[nextType]);

                this.transform.position = Vector3.Lerp(this.transform.position, other.transform.position, 0.5f);
            }

            _onDisappear?.Invoke();
            GameManager.AddPoint((int)nextType);
        }
    }

    /// <summary>消えた時のコールバックに処理をバインド</summary>
    /// <param name="cb">バインドする処理</param>
    public void BindOnDisappearCallBack(Action cb)
    {
        _onDisappear += cb;
    }
}
