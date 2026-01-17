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
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mPos = Input.mousePosition;
            Vector2 wPos = Camera.main.ScreenToWorldPoint(mPos);
            Vector3 fruitsPopPos = wPos;

            if (wPos.x < -1.6) fruitsPopPos.x = -1.6f;
            if (wPos.x > 1.6) fruitsPopPos.x = 1.6f;
            fruitsPopPos.y = OverLineY;

            FruitsType popedFruitsType
             = (FruitsType)(UnityEngine.Random.Range(0, _listPrefabFruits.Count) + 1);

            FruitsController c = Instantiate(_listPrefabFruits[(int)popedFruitsType - 1]
                                    , fruitsPopPos
                                    , Quaternion.identity);
            c.SetType(popedFruitsType);
            Data d = new Data(popedFruitsType, c);
            c.SetData(d);
            c.SetColor(ColorPallet[popedFruitsType]);
            _listData.Add(d);
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
