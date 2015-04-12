using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable]
public class AudioManager : MonoBehaviour
{

	public List<AudioCategory> audioCategories;
	public AudioSource loopSource;

	// Singleton
	public static AudioManager Instance;
	
	void Start() 
	{
		Instance = this;
	}

	public static void PlayDefaultMapMusic()
	{
		if (Application.loadedLevelName == "plage")
		{
			Instance._Play("skullrock", true);
		}
	}
	
	public static void Play(string soundName, bool loop = false)
	{
		Instance._Play(soundName, loop);
	}
	
	public static void SetCategoryVolume(string category, float volume)
	{
		Instance._SetCategoryVolume(category, volume);
	}

	private void _SetCategoryVolume(string category, float volume)
	{
		foreach(AudioSource source in gameObject.GetComponents<AudioSource>())
		{
			foreach(AudioCategory audioCategory in audioCategories)
			{
				if (audioCategory.clips.IndexOf(source.clip) != -1)
				{
					source.volume = volume;
				}
			}
		}
	}

	private void _Play(string soundName, bool loop = false)
	{
		foreach(AudioCategory audioCategory in audioCategories)
		{
			foreach(AudioClip clip in audioCategory.clips)
			{
				if (clip.name == soundName)
				{
					if (loop == false)
					{
						AudioSource source = gameObject.AddComponent<AudioSource>();
						source.PlayOneShot(clip);
						Destroy(source, clip.length);
						return;
					}
					else
					{
						loopSource.clip = clip;
						loopSource.Play();
						return;
					}
				}
			}
		}
		Debug.LogError(soundName + "not found in any category ! Sound not played...");
	}
}
