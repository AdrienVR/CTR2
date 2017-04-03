using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager Instance;

    public AudioCategoryManager AudioCategoryManager;
    public AudioSource m_loopingUISource;
    public AudioSource m_loopingMusicSource;

    public List<AudioSource> m_sources = new List<AudioSource>(64);

#if UNITY_EDITOR
    void OnValidate()
    {
        m_loopingUISource.loop = true;
        m_loopingMusicSource.loop = true;
    }
#endif

    void Awake() 
	{
        if (!enabled)
            return;
        Instance = this;
        m_audioCategories = AudioCategoryManager.audioCategories;
        

        m_categoryVolumes = new Dictionary<string, float>();
		foreach(AudioCategory category in m_audioCategories)
		{
			m_categoryVolumes[category.name] = 1;
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

    public void PlayOverrideMusic(string soundName)
    {
        m_overridingMusic = soundName;
        Play(soundName, true, true);
    }

    public void StopOverrideMusic(string soundName)
    {
        if (soundName == m_overridingMusic)
            Play(m_currentMusic, true);
    }

    public void Play(string soundName, bool loop = false, bool _override = false)
	{
        if (loop && !_override)
            m_currentMusic = soundName;

        foreach (AudioCategory audioCategory in m_audioCategories)
		{
			foreach(AudioClip clip in audioCategory.clips)
			{
				if (clip.name == soundName)
				{
					if (loop == false)
                    {
                        AudioSource source = null;
                        for (int i  = 0; i < m_sources.Count;i++)
                        {
                            if (m_sources[i].enabled == false)
                            {
                                source = m_sources[i];
                                source.enabled = true;
                                break;
                            }
                        }
                        if (source == null)
                        {
                            source = gameObject.AddComponent<AudioSource>();
                            m_sources.Add(source);
                        }
                        source.volume = m_categoryVolumes[audioCategory.name];
						source.PlayOneShot(clip);
                        StartCoroutine(DisableSourceIn(source, clip.length));
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
		Debug.LogError(soundName + " not found in any category ! Sound not played...");
    }

    IEnumerator DisableSourceIn(AudioSource _source, float _duration)
    {
        yield return Yielders.Get(_duration);
        _source.enabled = false;

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
    public void PauseLoopingMusic()
    {
        m_loopingMusicSource.Pause();
    }
    public void ResumeLoopingMusic()
    {
        m_loopingMusicSource.Play();
    }

    public void StopLoopingUI()
    {
        m_loopingUISource.Stop();
    }

    public void StopLoopingMusic()
    {
        m_loopingMusicSource.Stop();
    }

    private string m_overridingMusic;
    private string m_currentMusic;

    private List<AudioCategory> m_audioCategories;

    private Dictionary<string, float> m_categoryVolumes;

}
