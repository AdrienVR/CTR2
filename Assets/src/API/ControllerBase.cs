using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControllerBase 
{

	private Dictionary <string, VirtualKey> buttons;

	public ControllerBase  (string type) 
	{
		Debug.Log("Initializing controller "+type);
		this.buttons = new Dictionary<string, VirtualKey>();

		Dictionary <string, KeyCode> buttons = ControllerResources.GetButtons(type);
		Dictionary <string, string> axis = ControllerResources.GetAxis(type);
		Dictionary <string, float> defaultAxisValues = ControllerResources.GetAxisValues(type);
		foreach(string actionName in buttons.Keys)
		{
			this.buttons[actionName] = new Key(buttons[actionName], actionName);
		}
		foreach(string actionName in axis.Keys)
		{
			if (defaultAxisValues[actionName] > 0)
				this.buttons[actionName] = new Axis(actionName, axis[actionName], 0, defaultAxisValues[actionName]);
			else
				this.buttons[actionName] = new Axis(actionName, axis[actionName], defaultAxisValues[actionName], 0);
    	}
	}

	public void UpdateInternal()
	{
		foreach(VirtualKey button in buttons.Values)
		{
			button.UpdateInternal();
		}
	}
	
	public virtual float GetAxis(string actionName)
	{
		return buttons[actionName].GetAxis();
	}
	
	public virtual bool GetKey(string actionName)
	{
		return buttons[actionName].GetKey();
	}
	
	public virtual bool GetKeyDown(string actionName)
	{
		return buttons[actionName].GetKeyDown();
	}
	
	public virtual bool GetKeyUp(string actionName)
	{
		return buttons[actionName].GetKeyUp();
  	}

}
