using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    
    public IEnumerator StartCountDown_Cor(System.Action cb)
    {
        float count = 3;
        while (count > 0)
        {
            _text.text = count.ToString();
            yield return new WaitForSeconds(1);
            count--;
        }
        _text.text = "Start";
        yield return new WaitForSeconds(1);
        cb?.Invoke();
    }
}
