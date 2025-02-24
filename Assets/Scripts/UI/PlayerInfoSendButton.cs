using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfoSendButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerIdInputField;
    [SerializeField] TextMeshProUGUI invalidInputGuideText;
    [SerializeField] GameObject retryButton;
    private List<UserData> userData;
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)){
            GetRequest("https://api.jbnucpu.co.kr/event");
        }
    }

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
            SendJsonData("https://api.jbnucpu.co.kr/event", new UserData { userId = playerId, score = ScoreManager.Instance.Score});
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

    public void GetRequest(string url)
    {
        StartCoroutine(SendGetRequest(url));
    }

    private IEnumerator SendGetRequest(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                
                // Deserialize into a wrapper object
                // UserDataList userDataList = JsonUtility.FromJson<UserDataList>(request.downloadHandler.text, );

                // if (userDataList != null && userDataList.users.Count > 0)
                // {
                //     userData = JsonUtility.FromJson<UserDataList>(request.downloadHandler.text).users;

                //     Debug.Log(userData[0].userId);
                // }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}

public class UserData
{
    public string userId;
    public int score;
}

// [System.Serializable]
// public class UserDataList
// {
//     public List<UserData> users;
// }
