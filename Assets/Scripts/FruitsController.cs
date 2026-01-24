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

    [SerializeField] private Collider2D _collider = null;
    public Collider2D Collider => _collider;

    private float _size = 1f;

    public static void SetAllRigidbodyToCinematic()
    {
        foreach (var controller in _listFruitsController)
        {
            if (controller.Rigidbody != null)
            {
                Destroy(controller.Rigidbody);
            }
        }
    }

    public void SetDrawOrder(int orderNum)
    {
        spriteRenderer.sortingOrder = orderNum;
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

    private IEnumerator PopBornCor()
    {
        float frame = 0f;

        while (frame < 0.25f)
        {
            frame += Time.deltaTime;
            float per = frame / 0.25f;
            per = 1f - (1f - per) * (1 - per) * (1 - per) * (1 - per);

            this.transform.localScale = Vector3.one * per * _size;

            yield return null;
        }

        this.transform.localScale = Vector3.one * _size;
        if (this.Rigidbody != null) this.Rigidbody.simulated = true;
        if (this.Collider != null) this.Collider.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _listFruitsController.Add(this);

        // Unityのあほみたいな物理演算だと、同位置から丸を落とすと上に積むことができちゃうので、わずかに出現位置をブレさせる
        _rigidbody.velocity = new Vector2(UnityEngine.Random.Range(-0.1f, 0.1f), 0f);

        Pop();
    }

    public void Pop()
    {
        // 大きさを記録して、発生時にポップするアニメーションをコルーチンで実装
        _size = this.transform.localScale.x;
        this.transform.localScale = Vector3.zero;
         if (this.Rigidbody != null) this.Rigidbody.simulated = false;
         if (this.Collider != null) this.Collider.enabled = false;
        StartCoroutine(PopBornCor());
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

                this.Pop();

                MusicManager.Instance.PlaySE(1); // 合体音
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
