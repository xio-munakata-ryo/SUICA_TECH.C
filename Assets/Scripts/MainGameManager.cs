using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Score;
using System.Threading.Tasks;

public class MainGameManager : SingletonMonoBehaviour<MainGameManager>
{
    protected override bool dontDestroyOnLoad => false;

    [SerializeField, Header("フルーツのプレハブ")]
    private FruitsController[] _fruitsPrefabsArray = null;

    [SerializeField, Header("フルーツを生成するワールドY座標")]
    private float _fruitsGenerateYPos = 4.0f;

    [SerializeField, Header("フルーツ生成可能なワールドX座標の最大値")]
    private float _fruitsGenerateXPosMax = 2.75f;

    [SerializeField, Header("フルーツ生成可能なワールドX座標の最小値")]
    private float _fruitsGenerateXPosMin = -2.75f;

    [SerializeField, Header("ゲームオーバーUI")]
    private GameObject _gameOverUI = null;

    [SerializeField, Header("スコア倍率(果物番号にかけてスコアにする)")]
    private int _scoreMultiply = 100;

    private List<FruitsData> _fruitsInstanceList = new List<FruitsData>();

    private FruitsType _nextGenerateFruitsType = FruitsType.None;

    private List<FruitsController> _listFruitsController = new List<FruitsController>();
    public List<FruitsController> ListFruitsController => _listFruitsController;

    private bool _isGameOver = false;

    public bool IsGameOver => _isGameOver;

    public int ScoreMultiply => _scoreMultiply;

    public void OnGameOver()
    {
        _isGameOver = true;
        _gameOverUI.SetActive(_isGameOver);
    }

    public async void Retry()
    {
        foreach (var fruit in new List<FruitsController>(_listFruitsController))
        {
            PoolManager.Instance.StackObject(fruit);
        }
        ScoreManager.ResetScore();
        try
        {
        await TiledFadeManager.Instance.FadeOut();
        }
        catch (System.OperationCanceledException)
        {
            return;
        }
        _gameOverUI.SetActive(false);
        try
        {
            await TiledFadeManager.Instance.FadeIn();
        }
        catch (System.OperationCanceledException)
        {
            return;
        }
        _isGameOver = false;
    }

    private FruitsType GetRandomFruitsData()
    {
        int typeIndex = Random.Range(1, 3);
        FruitsType type = (FruitsType)System.Enum.ToObject(typeof(FruitsType), typeIndex);

        return type;
    }

    public void GenerateFruitsByType(FruitsType type, Vector2 generatePos)
    {
        var prefab = _fruitsPrefabsArray[(int)type];
        var instance = (FruitsController)PoolManager.Instance.ReleaseObject(prefab, generatePos, Quaternion.identity);
        var data = new FruitsData(type, instance);
        instance.SetFruitsType(data);
        _fruitsInstanceList.Add(data);
    }

    // Start is called before the first frame update
    void Start()
    {
        _nextGenerateFruitsType = GetRandomFruitsData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isGameOver)
        {
            Vector2 mouseScreenPos = Input.mousePosition;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            var generatePos = new Vector2
            (
                Mathf.Clamp
                (
                    mouseWorldPos.x,
                    _fruitsGenerateXPosMin + FruitsData.GetRadiusAccordingType(_nextGenerateFruitsType),
                    _fruitsGenerateXPosMax - FruitsData.GetRadiusAccordingType(_nextGenerateFruitsType)
                ),
                _fruitsGenerateYPos
            );
            if ((int)_nextGenerateFruitsType < _fruitsPrefabsArray.Length)
            {
                GenerateFruitsByType(_nextGenerateFruitsType, generatePos);
            }
            _nextGenerateFruitsType = GetRandomFruitsData();
        }
    }
}
