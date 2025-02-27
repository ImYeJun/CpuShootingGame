using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameStartButtonText : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private float intervalTime;

    private void Awake() {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        StartCoroutine(nameof(ExecuteEffect));
    }

    IEnumerator ExecuteEffect(){
        textMeshProUGUI.alpha = 0.0f;

        yield return new WaitForSeconds(intervalTime);
        
        textMeshProUGUI.alpha = 1.0f;

        yield return new WaitForSeconds(intervalTime);

        StartCoroutine(nameof(ExecuteEffect));
    }
}
