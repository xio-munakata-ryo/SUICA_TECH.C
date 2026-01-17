using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    #region シングルトンパターン
    public static ScoreDisplayer _instance = null;
    public static ScoreDisplayer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreDisplayer>();
                if (_instance == null)
                {
                    Debug.LogWarning("ScoreDisplayerが存在しません");
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    [SerializeField, Header("スコア表示用テキスト")]
    private TextMeshProUGUI _scoreTMP = null;

    [SerializeField, Header("スコア更新反映時間：秒")]
    private float _scoreUpdateReflectTimeSec = 0.5f;

    [SerializeField, Header("スコア前に表示するテキスト")]
    private string _scoreBeforeText = "SCORE : ";

    [SerializeField, Header("スコア後に表示するテキスト")]
    private string _scoreAfterText = " pt.";

    // 現在表示中のスコア値
    private int _displayScore = 0;

    void Start()
    {
        if (_scoreTMP == null)
        {
            Debug.Log("ScoreDisplayer：スコア表示用テキストが設定されていません");
        }
        UpdateScoreText(Score.ScoreManager.Score);
    }

    /// <summary>
    /// スコア表示テキストを更新
    /// </summary>
    /// <param name="newScore"></param>
    public void UpdateScoreText(int newScore)
    {
        if (_scoreTMP == null)
        {
            Debug.Log("ScoreDisplayer：スコア表示用テキストが設定されていません");
        }
        StopAllCoroutines();
        StartCoroutine(ReflectScoreUpdateWithAnim(newScore));
    }

    /// <summary>
    /// スコア値の更新をアニメーション付きでテキストに反映する
    /// </summary>
    /// <param name="newScore"></param>
    /// <returns></returns>
    private IEnumerator ReflectScoreUpdateWithAnim(int newScore)
    {
        float elapsedTime = 0.0f;
        int startingScore = _displayScore;
        while (elapsedTime < _scoreUpdateReflectTimeSec)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _scoreUpdateReflectTimeSec);
            _displayScore = Mathf.RoundToInt(Mathf.Lerp(startingScore, newScore, t));
            _scoreTMP.text = $"{_scoreBeforeText}{_displayScore}{_scoreAfterText}";
            yield return null;
        }
        _displayScore = newScore;
        _scoreTMP.text = $"{_scoreBeforeText}{_displayScore}{_scoreAfterText}";
    }
}
