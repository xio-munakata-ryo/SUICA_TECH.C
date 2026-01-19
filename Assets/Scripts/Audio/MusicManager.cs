using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クロスフェード入り、Sound-BGM-Manager
/// </summary>
public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("MusicManager");
                    _instance = obj.AddComponent<MusicManager>();

                    AudioSource[] auds = new AudioSource[2];

                    GameObject asBgmA = new GameObject("BGMAudioSourceA");
                    auds[0] = asBgmA.AddComponent<AudioSource>();
                    auds[0].volume = 0.35f;
                    asBgmA.transform.SetParent(obj.transform);

                    GameObject asBgmB = new GameObject("BGMAudioSourceB");
                    auds[1] = asBgmB.AddComponent<AudioSource>();
                    auds[1].volume = 0.35f;
                    asBgmB.transform.SetParent(obj.transform);

                    _instance.SetBGMAudioSource(auds);

                    GameObject asSeA = new GameObject("SEAudioSourceA");
                    var aud = asSeA.AddComponent<AudioSource>();
                    aud.volume = 0.75f;
                    asBgmA.transform.SetParent(obj.transform);

                    _instance.SetSEAudioSource(aud);
                }
            }
            return _instance;
        }
    }

    [SerializeField] private AudioSource[] _asBgmPair = new AudioSource[2];
    [SerializeField] private AudioSource _asSe = null;
    private bool _currentAudioSourceIsIndexZero = false;
    private AudioSource _nextAs => _currentAudioSourceIsIndexZero ? _asBgmPair[1] : _asBgmPair[0];
    private AudioSource _currentAs => _currentAudioSourceIsIndexZero ? _asBgmPair[0] : _asBgmPair[1];

    [SerializeField] private List<AudioClip> _acBgms = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _acSes = new List<AudioClip>();

    [SerializeField]
    private float _fadeInTimeSec = 2f;
    [SerializeField]
    private float _fadeOutTimeSec = 2f;
    private float _bgmDefaultVolume = 1f;
    private float _seDefaultVolume = 1f;
    private float _currentBGMLengthTimeSec = 0f;

    private Coroutine _fadeInCoroutine = null;
    private Coroutine _fadeOutCoroutine = null;

    private List<Coroutine> _listCoroutines = new List<Coroutine>();

    private int _currentBGMIndex = 0;

    public void PlaySE(int id)
    {
        _asSe.PlayOneShot(_acSes[id], _seDefaultVolume);
    }

    public void StopBGM()
    {
        // FadeOut FadeIn が残っていたら、処理を破棄する
        for (int i = 0; i < _listCoroutines.Count; i++)
        {
            if (_listCoroutines[i] != null) _listCoroutines[i] = null;
        }
        _listCoroutines.Clear();
        
        _fadeOutCoroutine = StartCoroutine(FadeOut_Cor(_currentAs));
    }

    private void Start()
    {
        // 乱数の種（シード値）を設定する
        // 現在時刻のミリ秒をシードにして、毎回違う乱数の回答がでるようにする
        UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);

        _bgmDefaultVolume = _nextAs.volume;

        _seDefaultVolume = _asSe.volume;
    }

    // Start is called before the first frame update
    public void PlayBGM()
    {
        if (_acBgms.Count > 0)
        {
            if (_acBgms.Count > 1)
            {
                _currentBGMIndex = UnityEngine.Random.Range(0, _acBgms.Count);
            }
            _nextAs.clip = _acBgms[_currentBGMIndex];
            _nextAs.Play();
            _nextAs.volume = 0f;
            _fadeInCoroutine = StartCoroutine(FadeIn_Cor(_nextAs));
            _listCoroutines.Add(_fadeInCoroutine);
            _currentBGMLengthTimeSec = _acBgms[_currentBGMIndex].length;
            var c = StartCoroutine(AutoChangeBGM_Cor(_nextAs, _currentAs));
            _listCoroutines.Add(c);
            _currentAudioSourceIsIndexZero = !_currentAudioSourceIsIndexZero;
        }
    }

    private IEnumerator AutoChangeBGM_Cor(AudioSource beOutAs, AudioSource beInAs)
    {
        yield return new WaitForSeconds(_currentBGMLengthTimeSec - _fadeOutTimeSec);
        // FadeOutをさせる際に、フェイドインが残っていたら、処理を破棄する
        if (_fadeInCoroutine != null) _fadeInCoroutine = null;
        _fadeOutCoroutine = StartCoroutine(FadeOut_Cor(beOutAs));
        _listCoroutines.Add(_fadeOutCoroutine);
        // クロスフェード開始
        int index = 0;
        if (_acBgms.Count > 1)
        {
            UnityEngine.Random.Range(0, _acBgms.Count - 1);
            if (_currentBGMIndex == index) index++;
            _currentBGMIndex = index;
        }
        beInAs.clip = _acBgms[index];
        beInAs.Play();
        beInAs.volume = 0f;
        _fadeInCoroutine = StartCoroutine(FadeIn_Cor(beInAs));
        _listCoroutines.Add(_fadeInCoroutine);
        _currentBGMLengthTimeSec = _acBgms[index].length;
        var c = StartCoroutine(AutoChangeBGM_Cor(beInAs, beOutAs));
        _listCoroutines.Add(c);
        _currentAudioSourceIsIndexZero = !_currentAudioSourceIsIndexZero;

        yield return new WaitForSeconds(_fadeOutTimeSec);

        beOutAs.Stop();
    }

    private IEnumerator FadeOut_Cor(AudioSource audioSource)
    {
        float frame = 0f;
        float startFadeOutVolume = _currentAs.volume;
        while (audioSource.volume > 0f)
        {
            frame += Time.deltaTime;

            float per = frame / _fadeOutTimeSec;
            if (per < 0f) per = 0f;

            float v = Mathf.Lerp(startFadeOutVolume, 0f, per);
            audioSource.volume = v;

            yield return null;
        }
        audioSource.Stop();
    }

    private IEnumerator FadeIn_Cor(AudioSource audioSource)
    {
        float frame = 0f;
        while (audioSource.volume < _bgmDefaultVolume)
        {
            frame += Time.deltaTime;

            float per = frame / _fadeInTimeSec;
            if (per > 1f) per = 1f;

            float v = Mathf.Lerp(0f, _bgmDefaultVolume, per);
            audioSource.volume = v;

            yield return null;
        }
    }

    public void SetBGMAudioSource(AudioSource[] aus)
    {
        _asBgmPair = aus;
    }

    public void SetSEAudioSource(AudioSource aus)
    {
        _asSe = aus;
    }
}
