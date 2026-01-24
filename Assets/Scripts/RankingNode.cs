using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class RankingNode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameTxt;
    [SerializeField] private TextMeshProUGUI _pointTxt;
    [SerializeField] private Image _backImage;

    public void SetNameTxt(string name)
    {
        _nameTxt.text = name;
    }

    public void SetPointTxt(int point)
    {
        GameManager.DrawPointText(_pointTxt, point);
    }

    public void SetFlashBackImage()
    {
        StartCoroutine(FlashBackCor());
    }

    private IEnumerator FlashBackCor()
    {
        float frame = 0f;

        Color baseColor = this._backImage.color;

        while (true)
        {
            frame += Time.deltaTime;

            if (frame > 16) frame -= 32;

            float per = Mathf.Sin(frame * Mathf.PI) * 0.5f + 0.5f;

            this._backImage.color = Color.Lerp(baseColor, Color.black, per);

            yield return null;
        }
    }
}
