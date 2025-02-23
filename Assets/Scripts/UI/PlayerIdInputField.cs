using UnityEngine;
using TMPro;  // Required for TextMeshPro InputField

public class PlayerIdInputField : MonoBehaviour
{
    private TMP_InputField inputField; // Assign in Inspector

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        
        // Ensure keyboard appears on mobile
        inputField.onSelect.AddListener(ShowKeyboard);
        // inputField.onEndEdit.AddListener(HandleInput);
    }

    void ShowKeyboard(string text)
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    // void HandleInput(string text)
    // {
    //     Debug.Log("User Input: " + text);
    // }
}
