using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    
    private void Update() 
    {
        textMeshProUGUI.text = ScoreManager.Instance.Score.ToString("D6");
    }
}
