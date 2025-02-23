using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance { get; private set; }
    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> soundEffects;
    [SerializeField] private float pitchVariation;

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

    public void Play(SoundEffectType soundEffectType){
        audioSource.PlayOneShot(soundEffects[(int)soundEffectType]);
    }

    public void PlayWithRandomPitch(SoundEffectType soundEffectType){
        float originPitch = audioSource.pitch;
        float randomPitch = 1.0f + Random.Range(-pitchVariation, pitchVariation);

        audioSource.pitch = randomPitch;

        audioSource.PlayOneShot(soundEffects[(int)soundEffectType]);

        audioSource.pitch = originPitch;
    }
}

public enum SoundEffectType{
    EnemyHit,
    PlayerHit
}