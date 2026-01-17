using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CinemachineImpulseSource))]
public class FruitsController : MonoBehaviour
{
    private static List<FruitsController> _listFruitsController = new List<FruitsController>();
    public static List<FruitsController> ListFruitsController => _listFruitsController;

    private FruitsType _type;
    public FruitsType Type => _type;

    private Data _data;

   [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    [SerializeField] private Sprite sprite3;
    [SerializeField] private Sprite sprite4;
    [SerializeField] private Sprite sprite5;
    [SerializeField] private Sprite sprite6;
    [SerializeField] private Sprite sprite7;
    [SerializeField] private Sprite sprite8;
    [SerializeField] private Sprite sprite9;
    [SerializeField] private Sprite sprite10;
    [SerializeField] private Sprite sprite11;

    [SerializeField] private int Rank = 0;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        switch(Rank)
        {
                case 1:
                spriteRenderer.sprite = sprite1;
                break;
                case 2:
                spriteRenderer.sprite = sprite2;
                break;
            case 3:
                spriteRenderer.sprite = sprite3;
                break;
            case 4:
                spriteRenderer.sprite = sprite4;
                break;
            case 5:
                spriteRenderer.sprite = sprite5;
                break;
            case 6:
                spriteRenderer.sprite = sprite6;
                break;
            case 7:
                spriteRenderer.sprite = sprite7;
                break;
            case 8:
                spriteRenderer.sprite = sprite8;
                break;
            case 9:
                spriteRenderer.sprite = sprite9;
                break;
            case 10:
                spriteRenderer.sprite = sprite10;
                break;
            case 11:
                spriteRenderer.sprite = sprite11;
                break;
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
            Rank++;

            // 自分成長

            var impulseSource = GetComponent<CinemachineImpulseSource>();
            if(impulseSource != null)
            {
            impulseSource.GenerateImpulse(1.5f);
            }

            //impulseSource.GenerateImpulse();
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
}
