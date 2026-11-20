using System.Collections;
using UnityEngine;

public class SceneFadeIn : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 0.75f;

    private IEnumerator Start()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 1f;

        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            cg.alpha = 1f - Mathf.Clamp01(timer / fadeInDuration);
            yield return null;
        }
        cg.alpha = 0f;
    }
}