using TMPro;
using UnityEngine;

public class ScoreResultText : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    private void Awake() {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        textMeshProUGUI.text = "Score:" + ScoreManager.Instance.Score.ToString("D6");
    }
}
