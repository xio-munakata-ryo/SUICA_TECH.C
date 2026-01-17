using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class CameraManager : MonoBehaviour
{
    private Vector3 _defaultStrength = new Vector3(0.6f, 0.2f, 0f);

    /// <summary>
    /// ƒJƒƒ‰‚ª—h‚ê‚é‰‰o
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="strength"></param>
    /// <param name="vibrato"></param>
    /// <param name="randomness"></param>
    /// <param name="snapping"></param>
    /// <param name="fadeOut"></param>
    /// <returns></returns>
    public async UniTask CameraShakeEvent(float duration = 1f, Vector3 strength = default, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true)
    {
        if (strength == default)
        {
            strength = _defaultStrength;
        }
        await transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut).ToUniTask();
        await UniTask.WaitForSeconds(2f);
    }
}
