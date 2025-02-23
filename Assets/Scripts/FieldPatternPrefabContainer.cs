using UnityEngine;

public class FieldPatternPrefabContainer : MonoBehaviour
{
    public static FieldPatternPrefabContainer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
