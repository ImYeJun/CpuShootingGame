using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class BypassCertificate : CertificateHandler {
    protected override bool ValidateCertificate(byte[] certificateData) {
        // Always accept the certificate (not secure for production)
        return true;
    }
}
public class PlayerInfoSendButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerPhoneNumberInputField;
    [SerializeField] TextMeshProUGUI playerNickNameInputField;
    [SerializeField] TextMeshProUGUI invalidInputGuideText;
    [SerializeField] GameObject retryButton;
    private List<UserData> userData;
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)){
            GetRequest("https://api.jbnucpu.co.kr:8443/event");
        }
    }

    public void Onclick()
    {
        string playerPhoneNumber = playerPhoneNumberInputField.text.Trim(); 
        string playerNickName = playerNickNameInputField.text;

        if (playerPhoneNumber.Length > 0)
        {
            // * Something stupid character is attached as a last charaacter of playerId so has to be removed
            playerPhoneNumber = playerPhoneNumber.Substring(0, playerPhoneNumber.Length - 1);
        }

        if (playerPhoneNumber.Length == 11 && IsInteger(playerPhoneNumber))
        {
            Debug.Log($"Score : {ScoreManager.Instance.Score}");
            Debug.Log($"Input Phone Number : {playerPhoneNumber}");
            Debug.Log($"Input NickName : {playerNickName}");

            //* For the fast revising, userId field is considered as playerPhoneNumber
            SendJsonData("https://api.jbnucpu.co.kr:8443/event", new UserData { userId = playerPhoneNumber, nickName = playerNickName,score = ScoreManager.Instance.Score});
            retryButton.SetActive(true);
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Invalid player phone number.");
            invalidInputGuideText.text = "유효한 전화번호 형식이 아닙니다";
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
            Application.ExternalCall("receive user data : ", request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Application.ExternalCall("fail to receive user data", "{\"error\": \"" + request.error + "\"}");
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
    public string nickName;
    public int score;
}

// [System.Serializable]
// public class UserDataList
// {
//     public List<UserData> users;
// }
