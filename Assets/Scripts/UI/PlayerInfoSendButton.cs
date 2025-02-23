using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfoSendButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerIdInputField;
    [SerializeField] TextMeshProUGUI invalidInputGuideText;
    [SerializeField] GameObject retryButton;

    public void Onclick()
    {
        string playerId = playerIdInputField.text.Trim(); 
        if (playerId.Length > 0)
        {
            // * Something stupid character is attached as a last charaacter of playerId so has to be removed
            playerId = playerId.Substring(0, playerId.Length - 1);
        }

        if (playerId.Length == 9 && IsInteger(playerId))
        {
            Debug.Log($"Score : {ScoreManager.Instance.Score}");
            Debug.Log($"Input Id : {playerId}");
            SendJsonData("https://example.com/api/receive", new UserData { userId = playerId, score = ScoreManager.Instance.Score});
            
            retryButton.SetActive(true);
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Invalid player ID.");
            invalidInputGuideText.text = "It's not valid Student ID Form";
        }
    }

    private bool IsInteger(string input)
    {
        foreach (char c in input)
        {
            if (!char.IsDigit(c)) return false; 
        }
        return true; 
    }

    public class UserData
    {
        public string userId;
        public int score;
    }

    public void SendJsonData(string url, UserData data)
    {
        string json = JsonUtility.ToJson(data);
        StartCoroutine(SendJson(url, json));
    }

    private IEnumerator SendJson(string url, string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
