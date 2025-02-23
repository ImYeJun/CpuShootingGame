using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static BgmManager Instance { get; private set; }
    [SerializeField] private List<AudioClip> bgmContainer;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps the instance across scenes
        }
        else
        {
            Destroy(gameObject); 
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void SetBgm(BgmType type){
        audioSource.Stop();

        audioSource.clip = bgmContainer[(int)type];

        audioSource.Play();

        switch (type){
            case BgmType.GameOverBgm:
                audioSource.loop = false;
                break;
            default:
                audioSource.loop = true;
                break;
        }
    }

    public void StopBgm(){
        audioSource.Stop();
    }
}

public enum BgmType{
    PlaySceneBgm,
    GameOverBgm
}
