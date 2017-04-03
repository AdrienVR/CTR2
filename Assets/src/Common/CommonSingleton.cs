
using UnityEngine;

public class CommonSingleton : MonoBehaviour
{
    public static CommonSingleton Instance;

    [SerializeField][HideInInspector]
    private MonoBehaviour[] m_behaviors;

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            for (int i = 0; i < m_behaviors.Length; i++)
            {
                m_behaviors[i].enabled = true;
            }
        }
        Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 2;
    }

    [ContextMenu("Validate")]
    void OnValidate()
    {
        m_behaviors = GetComponentsInChildren<MonoBehaviour>();
        for(int i = 0; i< m_behaviors.Length;i++)
        {
            m_behaviors[i].enabled = false;
        }
    }
}
