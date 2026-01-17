using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitsController : MonoBehaviour
{
    private static List<FruitsController> _listFruitsController = new List<FruitsController>();
    public static List<FruitsController> ListFruitsController => _listFruitsController;

    private FruitsType _type;
    public FruitsType Type => _type;

    private Data _data;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private Color _color;

    private float _flashFrame = 0f;

    [SerializeField]
    private Rigidbody2D _rigidbody = null;
    public Rigidbody2D Rigidbody => _rigidbody;

    public static void SetAllRigidbodyToCinematic()
    {
        foreach (var controller in _listFruitsController)
        {
            controller.Rigidbody.simulated = false;
        }
    }

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
        if (GameManager.IsGameOver) return;

        if (this.transform.position.y < -10)
        {
            Destroy(this.gameObject);

            GameManager.SetGameOver();
        }

        // ゲームオーバー部分を超えてたら、　赤くチカチカさせる
        if (GameManager.OverLineY < this.transform.position.y)
        {
            _flashFrame += Time.deltaTime / GameManager.StaticOverLineTimeSec;

            // 最初は遅く、徐々に早く
            float per = _flashFrame * _flashFrame * _flashFrame * _flashFrame;
            per = Mathf.Sin(per * Mathf.PI * 14f) * 0.5f + 0.5f;

            spriteRenderer.color = Color.Lerp(_color, Color.black, per);
        }
        else
        {
            _flashFrame = 0f;
            spriteRenderer.color = _color;
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

            GameManager.AddPoint((int)nextType);
        }
    }

    public static bool HasOverLineFrouts(float y)
    {

        foreach (var fruit in _listFruitsController)
        {
            if (fruit.transform.position.y > y) return true;
        }

        return false;
    }
}
