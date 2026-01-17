using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.Rendering.Universal;

[DefaultExecutionOrder(-100)]
public class TiledFadeManager : MonoBehaviour
{
    [SerializeField, Header("デフォルトのフェード時間")]
    private float _fadeDuration = 2.0f;

    [SerializeField, Header("デフォルトのフェードカラー")]
    private Color _fadeColor = Color.black;

    [SerializeField, Header("カメラ")]
    private Camera _camera = null;

    [SerializeField, Header("Image")]
    private Image _image;

    private const string FADE_VALUE = "_Fade_Value";

    public bool IsFade{ get; private set; } = false;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    /// <summary>
    /// 待機可能：フェードアウトインを行い、その合間でシーンのロードを行う
    /// </summary>
    /// <param name="sceneName">ロードするシーンの名前</param>
    /// <returns></returns>
    public async UniTask FadeOutAndLoadScene(string sceneName)
    {

        try
        {
            await FadeOutAndLoadScene(_fadeDuration, _fadeColor, sceneName);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：フェードアウトインを行い、その合間でシーンのロードを行う
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間：秒</param>
    /// <param name="sceneName">ロードするシーンの名前</param>
    /// <returns></returns>
    public async UniTask FadeOutAndLoadScene(float fadeDuration, string sceneName)
    {

        try
        {
            await FadeOutAndLoadScene(fadeDuration, _fadeColor, sceneName);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：フェードアウトインを行い、その合間でシーンのロードを行う
    /// </summary>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <param name="sceneName">ロードするシーンの名前</param>
    /// <returns></returns>
    public async UniTask FadeOutAndLoadScene(Color fadeColor, string sceneName)
    {
        try
        {
            await FadeOutAndLoadScene(_fadeDuration, fadeColor, sceneName);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：フェードアウトインを行い、その合間でシーンのロードを行う
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間：秒</param>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <param name="sceneName">ロードするシーンの名前</param>
    /// <returns></returns>
    public async UniTask FadeOutAndLoadScene(float fadeDuration, Color fadeColor, string sceneName)
    {
        var source = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, this.GetCancellationTokenOnDestroy());
        var token = source.Token;
        try
        {
            await FadeOut(fadeDuration, fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        try
        {
            await SceneManager.LoadSceneAsync(sceneName)
                                .ToUniTask(cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            return;
        }
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(_camera);

        try
        {
            await FadeIn(fadeDuration, fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードアウトを行う
    /// 全体を覆うImageが透明度0から1に向かう
    /// </summary>
    /// <returns></returns>
    public async UniTask FadeOut()
    {
        try
        {
            await FadeOut(_fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードインを行う
    /// 全体を覆うImageが透明度1fから0fに向かう
    /// </summary>
    /// <returns></returns>
    public async UniTask FadeIn()
    {
        try
        {
            await FadeIn(_fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードアウトを行う
    /// 全体を覆うImageが透明度0から1に向かう
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間：秒</param>
    /// <returns></returns>
    public async UniTask FadeOut(float fadeDuration)
    {
        try
        {
            await FadeOut(fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }


    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードインを行う
    /// 全体を覆うImageが透明度1fから0fに向かう
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間</param>
    /// <returns></returns>
    public async UniTask FadeIn(float fadeDuration)
    {
        try
        {
            await FadeIn(fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードアウトを行う
    /// 全体を覆うImageが透明度0から1に向かう
    /// </summary>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <returns></returns>
    public async UniTask FadeOut(Color fadeColor)
    {
        try
        {
            await FadeOut(_fadeDuration, fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードインを行う
    /// 全体を覆うImageが透明度1fから0fに向かう
    /// </summary>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <returns></returns>
    public async UniTask FadeIn(Color fadeColor)
    {
        try
        {
            await FadeIn(_fadeDuration, fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードアウトを行う
    /// 全体を覆うImageが透明度0から1に向かう
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間：秒</param>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <returns></returns>
    public async UniTask FadeOut(float fadeDuration, Color fadeColor)
    {
        if (_image == null)
        {
            Debug.LogWarning("Imageがアタッチされていません");
            return;
        }

        IsFade = true;
        // _cts.Cancel();
        // _cts = new CancellationTokenSource();
        var source = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, this.GetCancellationTokenOnDestroy());
        var token = source.Token;
        _image.material = new Material(_image.material);
        float frameCount = 0f;
        _image.color = fadeColor;
        _image.gameObject.SetActive(true);
        _image.enabled = true;
        _image.material.SetFloat(FADE_VALUE, 0f);
        float currentFade = _image.material.GetFloat(FADE_VALUE);

        if (fadeDuration <= 0f)
        {
            _image.material.SetFloat(FADE_VALUE, 1f);
            return;
        }
        while(frameCount < fadeDuration)
        {
            float per = Mathf.Clamp(frameCount / fadeDuration, 0f, 1f);
            _image.material.SetFloat(FADE_VALUE, Mathf.Lerp(currentFade, 1f, per));
            // color.a = Mathf.Lerp(currentAlpha, 1f, per);
            // _image.color = color;
            // unscaledDeltaTime：Time.Scaleの影響を受けない
            frameCount += Time.deltaTime;
            try
            {
                await UniTask.Yield(token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
        _image.material.SetFloat(FADE_VALUE, 1f);
        IsFade = false;
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードインを行う
    /// 全体を覆うImageが透明度1fから0fに向かう
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間：秒</param>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <returns></returns>
    public async UniTask FadeIn(float fadeDuration, Color fadeColor)
    {
        if (_image == null)
        {
            Debug.LogWarning("Imageがアタッチされていません");
            return;
        }

        IsFade = true;
        // _cts.Cancel();
        // _cts = new CancellationTokenSource();
        var source = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, this.GetCancellationTokenOnDestroy());
        var token = source.Token;
        _image.material = new Material(_image.material);
        float frameCount = 0f;
        _image.color = fadeColor;
        _image.gameObject.SetActive(true);
        _image.enabled = true;
        _image.material.SetFloat(FADE_VALUE, 1f);
        float currentFade = _image.material.GetFloat(FADE_VALUE);

        if (fadeDuration <= 0f)
        {
            _image.material.SetFloat(FADE_VALUE, 0f);
            return;
        }
        while(frameCount < fadeDuration)
        {
            float per = Mathf.Clamp(frameCount / fadeDuration, 0f, 1f);
            _image.material.SetFloat(FADE_VALUE, Mathf.Lerp(currentFade, 0f, per));
            // unscaledDeltaTime：Time.Scaleの影響を受けない
            frameCount += Time.deltaTime;
            try
            {
                await UniTask.Yield(token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
        _image.material.SetFloat(FADE_VALUE, 0f);
        _image.enabled = false;
        IsFade = false;
    }

    void Start()
    {
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(_camera);
    }

    #region シングルトンパターン
    private static TiledFadeManager _instance;
    public static TiledFadeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TiledFadeManager>();
                if (_instance == null)
                {
                    Debug.LogWarning("FadeUIが存在しません");
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
            // DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion
}
