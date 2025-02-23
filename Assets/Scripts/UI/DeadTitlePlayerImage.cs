using System.Collections;
using UnityEngine;

public class DeadTitlePlayerImage : MonoBehaviour
{
    [SerializeField] private GameObject playerIdInputContainer;
    private CanvasGroup canvasGroup;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start() {
        StartCoroutine(FadeImage());
    }

    private IEnumerator FadeImage() {
        float fadeInTime = 1.0f;
        float startTime = Time.time;
        while (Time.time - startTime < fadeInTime) {
            float alpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeInTime);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = 1f;  

        yield return new WaitForSeconds(1.2f);

        float fadeOutTime = 0.5f;
        startTime = Time.time;
        while (Time.time - startTime < fadeOutTime) {
            float alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeOutTime);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = 0f; 

        playerIdInputContainer.SetActive(true);
    }
}
