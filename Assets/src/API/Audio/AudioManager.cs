using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager Instance {
        get
        {
            if (s_instance == null)
            {
                GameObject go = new GameObject("AudioManager");
                DontDestroyOnLoad(go);
                s_instance = go.AddComponent<AudioManager>();
                go.AddComponent<AudioListener>();
            }
            return s_instance;
        }

    }
    private static AudioManager s_instance;

    void Awake() 
	{
        s_instance = this;
        m_audioCategories = PrefabReferences.Instance.AudioCategoryManager.audioCategories;

        m_loopingUISource = gameObject.AddComponent<AudioSource>();
        m_loopingUISource.loop = true;

        m_loopingMusicSource = gameObject.AddComponent<AudioSource>();
        m_loopingMusicSource.loop = true;

        m_categoryVolumes = new Dictionary<string, float>();
		foreach(AudioCategory category in m_audioCategories)
		{
			m_categoryVolumes[category.name] = 1;
		}
	}

	public void PlayDefaultMapMusic()
	{
		if (Application.loadedLevelName == "plage")
		{
			Play("skullrock", true);
		}
	}

    public void SetCategoryVolume(string category, float volume)
	{
		m_categoryVolumes[category] = volume;
		foreach(AudioSource source in gameObject.GetComponents<AudioSource>())
		{
			foreach(AudioCategory audioCategory in m_audioCategories)
			{
				if (audioCategory.clips.IndexOf(source.clip) != -1)
				{
					source.volume = volume;
				}
			}
		}
	}

    public void Play(string soundName, bool loop = false)
	{
		foreach(AudioCategory audioCategory in m_audioCategories)
		{
			foreach(AudioClip clip in audioCategory.clips)
			{
				if (clip.name == soundName)
				{
					if (loop == false)
					{
						AudioSource source = gameObject.AddComponent<AudioSource>();
						source.volume = m_categoryVolumes[audioCategory.name];
						source.PlayOneShot(clip);
						Destroy(source, clip.length);
						return;
					}
					else
					{
						m_loopingMusicSource.clip = clip;
						m_loopingMusicSource.Play();
						return;
					}
				}
			}
		}
		Debug.LogError(soundName + "not found in any category ! Sound not played...");
    }

    public void PlayLoopingUI(string name)
    {
        foreach (AudioCategory audioCategory in m_audioCategories)
        {
            foreach (AudioClip clip in audioCategory.clips)
            {
                if (clip.name == name)
                {
                    m_loopingUISource.clip = clip;
                    m_loopingUISource.Play();
                    return;
                }
            }
        }
    }

    public void StopLoopingUI()
    {
        m_loopingUISource.Stop();
    }

    public void StopLoopingMusic()
    {
        m_loopingMusicSource.Stop();
    }

    private AudioSource m_loopingMusicSource;
    private AudioSource m_loopingUISource;

    private List<AudioCategory> m_audioCategories;

    private Dictionary<string, float> m_categoryVolumes;

}
