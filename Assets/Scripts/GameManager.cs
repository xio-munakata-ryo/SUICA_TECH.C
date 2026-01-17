using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private List<FruitsController> _listPrefabFruits = new List<FruitsController>();

    private List<Data> _listData = new List<Data>();

    private static Dictionary<FruitsType, Color> _colorPallet = new Dictionary<FruitsType, Color>();
    public static Dictionary<FruitsType, Color> ColorPallet => _colorPallet;

    [SerializeField] private GameObject _gameOverUI;
    private static GameObject _staticGameOverUI;

    private static bool _isGameOver = false;
    public static bool IsGameOver => _isGameOver;

    [SerializeField] private TMPro.TextMeshProUGUI _pointTMP;
    private static TMPro.TextMeshProUGUI _staticPointTMP;
    private static int _numPoints = 0;

    [SerializeField] private LineRenderer _overLine = null;
    private static LineRenderer _staticOverLine = null;
    public static float OverLineY => _staticOverLine.GetPosition(0).y;
    [SerializeField] private float _overLineTimeSec = 3f;
    public static float StaticOverLineTimeSec = 3f;
    private float _overLineTimeFrame = 0f;

    private FruitsController _popFruitsShadow = null;

    private FruitsType _nextFruitsType = FruitsType.None;

    public static void SetPoint(int point)
    {
        _numPoints = point;
        _staticPointTMP.text = $"POINT : {_numPoints.ToString("0000000")}";
    }

    public static void AddPoint(int point)
    {
        _numPoints += point;
        _staticPointTMP.text = $"POINT : {_numPoints.ToString("0000000")}";
    }

    public static void SetGameOver()
    {
        _isGameOver = true;
        _staticGameOverUI.SetActive(true);

        // 背景の物理演算を全て止める
        FruitsController.SetAllRigidbodyToCinematic();
    }

    public void Retry()
    {
        // UI群　非表示
        _staticGameOverUI = _gameOverUI;
        _staticGameOverUI.SetActive(false);

        // フラグ初期化
        _isGameOver = false;

        // リトライになったら、全てのフルーツを消す
        foreach (var d in _listData)
        {
            if (d.FruitsObj != null)
            {
                Destroy(d.FruitsObj.gameObject);
            }
        }
        _listData.Clear();
        SetPoint(0);

        // ライン越えカウントの初期化
        _overLineTimeFrame = 0f;
    }

    public IEnumerator InnerRetryCoroutine(float duration = 0.5f)
    {
        yield return new WaitForSeconds(duration);

        Retry();
    }

    public void WaitDurationAndRetry(float duration)
    {
        StartCoroutine(InnerRetryCoroutine(duration));
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);

        for (int i = 0; i < _listPrefabFruits.Count; i++)
        {
            Color c = _listPrefabFruits[i].GetComponent<SpriteRenderer>().color;
            _colorPallet.Add((FruitsType)(i + 1), c);
        }

        // UI群　非表示
        _staticGameOverUI = _gameOverUI;
        _staticGameOverUI.SetActive(false);

        // フラグ初期化
        _isGameOver = false;

        // ポイント用UI　初期化
        _staticPointTMP = _pointTMP;
        SetPoint(0);

        // 超えて一定時間経過するとゲームオーバーなLineを取得
        _staticOverLine = _overLine;
        StaticOverLineTimeSec = _overLineTimeSec;

        // 次に生成される予定のフルーツの影を、マウス移動に合わせて画面に表示
        _popFruitsShadow = Instantiate(_listPrefabFruits[0]);

        //出てくるフルーツを小さめに
        _nextFruitsType
         = (FruitsType)(UnityEngine.Random.Range(1
            , _listPrefabFruits.Count <= 5 ? _listPrefabFruits.Count : 6));
        _popFruitsShadow.SetType(_nextFruitsType);
        Data nd = new Data(_nextFruitsType, _popFruitsShadow);
        _popFruitsShadow.SetData(nd);
        Color color = ColorPallet[_nextFruitsType];
        color.a = 0.5f;
        _popFruitsShadow.SetColor(color);
        _popFruitsShadow.SetDrawOrder(1000);
        Destroy(_popFruitsShadow.Collider);
        Destroy(_popFruitsShadow.Rigidbody);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameOver) return;

        Vector2 mPos = Input.mousePosition;
        Vector2 wPos = Camera.main.ScreenToWorldPoint(mPos);
        if (wPos.x < -1.6) wPos.x = -1.6f;
        if (wPos.x > 1.6) wPos.x = 1.6f;

        _popFruitsShadow.transform.position = new Vector3(wPos.x, OverLineY, 0f);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 fruitsPopPos = wPos;

            fruitsPopPos.y = OverLineY;

            FruitsController c = Instantiate(_listPrefabFruits[(int)_nextFruitsType - 1]
                                    , fruitsPopPos
                                    , Quaternion.identity);
            c.SetType(_nextFruitsType);
            Data d = new Data(_nextFruitsType, c);
            c.SetData(d);
            c.SetColor(ColorPallet[_nextFruitsType]);
            _listData.Add(d);

            // フルーツの影を変更
            //出てくるフルーツを小さめに
            _nextFruitsType
             = (FruitsType)(UnityEngine.Random.Range(1
                , _listPrefabFruits.Count <= 5 ? _listPrefabFruits.Count : 6));

            _popFruitsShadow.SetType(_nextFruitsType);
            Data nd = new Data(_nextFruitsType, _popFruitsShadow);
            _popFruitsShadow.SetData(nd);
            Color color = ColorPallet[_nextFruitsType];
            color.a = 0.5f;
            _popFruitsShadow.SetColor(color);
        }

        // フルーツのどれかが境界線を超えたら
        if (_isGameOver == false && FruitsController.HasOverLineFrouts(OverLineY))
        {
            _overLineTimeFrame += Time.deltaTime;

            if (_overLineTimeFrame >= _overLineTimeSec)
            {
                SetGameOver();
            }
        }
        else
        {
            _overLineTimeFrame = 0f;
        }
    }
}
