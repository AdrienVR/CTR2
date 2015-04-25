using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControllerInterface 
{
	// Singleton
	public static ControllerInterface Instance;

	public static int NumberOfController 
	{
		get{return Instance.instantiatedControllers;}
	}

	private static List<ControllerBase> allControllers = new List<ControllerBase>();
	
	private int nControllers;
	private int instantiatedControllers;

	public ControllerInterface()
	{
		Instance = this;
		allControllers = new List<ControllerBase>();
		allControllers.Add(new ControllerBase("Keyboard1"));
		allControllers.Add(new ControllerBase("Keyboard2"));
		allControllers.Add(new ControllerBase("Keyboard3"));
		UpdateInternal();
	}
	
	public void UpdateInternal() 
	{
		if (Input.GetJoystickNames ().Length != instantiatedControllers)
			InitJoysticks ();
		foreach(ControllerBase controller in allControllers)
		{
			controller.UpdateInternal();
		}
	}
	
	public static ControllerBase GetController(int i)
	{
		if (i > allControllers.Count - 1)
			return allControllers[allControllers.Count - 1];
		return allControllers[i];
	}

	private void InitJoysticks()
	{
		nControllers = Input.GetJoystickNames ().Length;

		if (instantiatedControllers > nControllers)
		{
			allControllers.RemoveAt(instantiatedControllers--);
			ControllerResources.controllers = instantiatedControllers;
		}
		else if (instantiatedControllers < nControllers)
		{
			allControllers.Insert(instantiatedControllers, new ControllerBase(Input.GetJoystickNames ()[instantiatedControllers]));
			instantiatedControllers++;
			ControllerResources.controllers = instantiatedControllers;
		}
		
		Debug.Log ("Now "+nControllers+" controllers detected");
	}
	
	
	public static float GetAxis(string actionName)
	{
		foreach(ControllerBase controller in allControllers)
		{
			if (controller.GetAxis(actionName) != 0)
			{
				return controller.GetAxis(actionName);
			}
		}
		return 0;
	}
	
	public static bool GetKey(string actionName)
	{
		bool result = false;
		foreach(ControllerBase controller in allControllers)
		{
			result |= controller.GetKey(actionName);
		}
		return result;
	}
	
	public static bool GetKeyDown(string actionName)
	{
		bool result = false;
		foreach(ControllerBase controller in allControllers)
		{
			result |= controller.GetKeyDown(actionName);
		}
		return result;
	}
	
	public static bool GetKeyUp(string actionName)
	{
		bool result = false;
		foreach(ControllerBase controller in allControllers)
		{
			result |= controller.GetKeyUp(actionName);
		}
		return result;
	}

	
	// TODO
	public static void Test(int i)
	{
		var dictionary = new Dictionary<string, string>();
		dictionary["perls"] = "dot";
		dictionary["net"] = "perls";
		dictionary["dot"] = "net";
		Instance.Write(dictionary, Path.Combine(Application.dataPath,"controller_"+i+".bin"));
		
		dictionary = Read(Path.Combine(Application.dataPath,"controller_"+i+".bin"));
		foreach (var pair in dictionary)
		{
			Debug.Log(pair);
		}
	}
	
	private void Write(Dictionary<string, string> dictionary, string file)
	{
		using (FileStream fs = File.OpenWrite(file))
			using (BinaryWriter writer = new BinaryWriter(fs))
		{
			// Put count.
			writer.Write(dictionary.Count);
			// Write pairs.
			foreach (var pair in dictionary)
			{
				writer.Write(pair.Key);
				writer.Write(pair.Value);
			}
		}
	}
	
	static Dictionary<string, string> Read(string file)
	{
		var result = new Dictionary<string, string>();
		using (FileStream fs = File.OpenRead(file))
			using (BinaryReader reader = new BinaryReader(fs))
		{
			// Get count.
			int count = reader.ReadInt32();
			// Read in all pairs.
			for (int i = 0; i < count; i++)
			{
				string key = reader.ReadString();
				string value = reader.ReadString();
				result[key] = value;
			}
		}
		return result;
	}
}
