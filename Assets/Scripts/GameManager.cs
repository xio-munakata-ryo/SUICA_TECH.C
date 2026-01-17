using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private TMPro.TextMeshProUGUI _pointTMP;
    private static TMPro.TextMeshProUGUI _staticPointTMP;
    private static int _numPoints = 0;

    private static CameraShake _cameraShake;

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
        // _cameraShake
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
            if(d.FruitsObj != null)
            {
                Destroy(d.FruitsObj.gameObject);
            }
        }
        _listData.Clear();
        SetPoint(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);

        _cameraShake = Camera.main.GetComponent<CameraShake>();

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

        //ポイント用UI　初期化
        _staticPointTMP = _pointTMP;
        SetPoint(0);
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
            fruitsPopPos.y = 2.5f;

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
    }
}
