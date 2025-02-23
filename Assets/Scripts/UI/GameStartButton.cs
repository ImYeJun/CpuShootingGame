using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class GameStartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData){
        StartFading(0.5f, 0.2f); // Smooth fade to 0.5 alpha over 0.2 seconds
    }

    public void OnPointerExit(PointerEventData eventData){
        StartFading(1.0f, 0.2f); // Smooth fade back to 1.0 alpha over 0.2 seconds
    }

    public void OnPointerClick(PointerEventData eventData){
        GameSceneManager.Instance.LoadScene(SceneType.PlayScene);
    }

    private void StartFading(float targetAlpha, float duration) {
        if (fadeCoroutine != null) {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeAlpha(targetAlpha, duration));
    }

    private IEnumerator FadeAlpha(float targetAlpha, float duration) {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        fadeCoroutine = null;
    }
}
