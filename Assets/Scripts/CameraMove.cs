using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class CameraMove : MonoBehaviour
{ 
    public static CameraMove Instance;
    
    private void Awake() 
    { 
        Instance = this; 
    }

    // ƒJƒƒ‰‚ğ—h‚ç‚·
    public IEnumerator Shake(float duration, float magnitude) 
    { 
        Vector3 originalPos = transform.localPosition; 
        float elapsed = 0f; 

        while (elapsed < duration) 
        { float x = Random.Range(-5f, 5f) * magnitude; 
            float y = Random.Range(-5f, 5f) * magnitude; 
            transform.localPosition = new Vector3(x, y, originalPos.z); 
            elapsed += Time.deltaTime; yield return null; 
        }
        
        transform.localPosition = originalPos; 
    }
    
    public void PlayShake(float duration = 0.1f, float magnitude = 0.2f) 
    { 
        StartCoroutine(Shake(duration, magnitude)); 
    } 
}
