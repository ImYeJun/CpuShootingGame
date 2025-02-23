using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void Onclick(){
        GameSceneManager.Instance.LoadScene(SceneType.PlayScene);
    }
}
