using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControllerInterface 
{

	public int nControllers;
	public int instantiatedControllers;

	public List<ControllerBase> allControllers;

	// Singleton
	public static ControllerInterface Instance;

	public ControllerInterface()
	{
		Instance = this;
		allControllers = new List<ControllerBase>();
		allControllers.Add(new ControllerBase("keyboard"));
		allControllers.Add(new ControllerBase("keyboard"));
		allControllers.Add(new ControllerBase("keyboard"));
		allControllers.Add(new ControllerBase("none"));
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
	
	public ControllerBase GetController(int i)
	{
		return allControllers[i];
	}

	public void InitJoysticks()
	{
		nControllers = Input.GetJoystickNames ().Length;

		if (instantiatedControllers > nControllers)
		{
			allControllers.RemoveAt(instantiatedControllers--);
			ControllerResources.xbox --;
		}
		else if (instantiatedControllers < nControllers)
		{
			allControllers.Insert(instantiatedControllers++, new ControllerBase("xbox"));
		}
		
		Debug.Log ("Now "+nControllers+" controllers detected");
	}
}
