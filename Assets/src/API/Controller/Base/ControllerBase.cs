using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControllerBase 
{

	private Dictionary <string, List<VirtualKey>> buttons;

	public string newKeyAction;
	public VirtualKey newKey;

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

		if (string.IsNullOrEmpty(newKeyAction) == false)
		{
			foreach(KeyCode keycode in KeyCodes.codes)
			{
				if (Input.GetKey(keycode))
				{
					newKey = new Key(keycode, newKeyAction);
					buttons[newKeyAction][0] = newKey;
					newKeyAction = "";
				}
			}
			foreach(string axis in KeyCodes.axis)
			{
				float axisValue = Input.GetAxis(axis);
				if (Mathf.Abs (axisValue) > 0.9f)
				{
					float max = 1;
					if (axisValue < 0)
						max = -1;
					newKey = new Axis(newKeyAction,axis,0, max);
					buttons[newKeyAction][0] = newKey;
					newKeyAction = "";
				}
			}
		}
	}

	public string GetNameKey(string actionName)
	{
		return buttons[actionName][0].keyName;
	}
	
	public void ListenNewKey(string actionName)
	{
		newKeyAction = actionName;
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
