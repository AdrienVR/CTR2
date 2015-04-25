using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControllerBase 
{

	private Dictionary <string, List<VirtualKey>> buttons;

	public ControllerBase  (string type) 
	{
		Debug.Log("Initializing controller "+type);
		this.buttons = ControllerResources.GetButtons(type);
	}

	public void UpdateInternal()
	{
		foreach(List<VirtualKey> buttonList in buttons.Values)
		{
			foreach(VirtualKey button in buttonList)
			{
				button.UpdateInternal();
			}
		}
	}

	public string GetNameKey(string actionName)
	{
		if (buttons[actionName][0] as Axis != null)
			return (buttons[actionName][0] as Axis).keyName;
		return (buttons[actionName][0] as Key).keyName;
	}
	
	public float GetAxis(string actionName)
	{
		foreach(VirtualKey button in buttons[actionName])
		{
			if (button.GetAxis() != 0)
				return button.GetAxis();
		}
		return 0;
	}
	
	public bool GetKey(string actionName)
	{
		foreach(VirtualKey button in buttons[actionName])
		{
			if (button.GetKey() != false)
				return button.GetKey();
		}
		return false;
	}
	
	public bool GetKeyDown(string actionName)
	{
		foreach(VirtualKey button in buttons[actionName])
		{
			if (button.GetKeyDown() != false)
				return button.GetKeyDown();
		}
		return false;
	}
	
	public bool GetKeyUp(string actionName)
	{
		foreach(VirtualKey button in buttons[actionName])
		{
			if (button.GetKeyUp() != false)
				return button.GetKeyUp();
		}
		return false;
  	}

}
