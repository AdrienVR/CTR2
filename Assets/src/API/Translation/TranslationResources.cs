using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;

public class TranslationResources
{
	const string relativePath = "Config";

	private static IDictionary translationDictionaries;
	private static IDictionary currentDictionary;

	public static void InitializeTranslation()
	{
		if (translationDictionaries == null)
		{
			string path = Path.Combine(Application.dataPath, Path.Combine(relativePath, "TranslationFile.JSON"));
			string file = File.ReadAllText(path);

			translationDictionaries = (IDictionary) Json.Deserialize(file);
		}
		string language = GetCurrentLanguage();
		if (translationDictionaries.Contains(language) == false)
		{
			Debug.LogError("Bad initialization for language : " + language);
			return;
		}
		currentDictionary = (IDictionary) translationDictionaries[language];// as Dictionary <string, string>;
	}

	public static string GetCurrentLanguage()
	{
		switch (Application.systemLanguage)
		{
			case SystemLanguage.French:
				return "French";
			case SystemLanguage.English:
				return "English";
			case SystemLanguage.Chinese:
				//return "Xiao Wu Yin";
				return "English";
			default:
				return "English";
		}
	}

	public static string GetTraductionOf(string original)
	{
		if (currentDictionary == null)
		{
			InitializeTranslation();
		}
		return (string)currentDictionary[original];
	}
}
