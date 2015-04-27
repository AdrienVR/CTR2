using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class Key : VirtualKey
{
	private KeyCode keyCode;
	
	public Key(KeyCode keyCode, string actionName) 
	{
		SetKey(keyCode);
		this.actionName = actionName;
	}

	public void SetKey(KeyCode keyCode)
	{
		keyName = keyCode.ToString();
		this.keyCode = keyCode;
	}

	public override bool GetKey()
	{
		return Input.GetKey(keyCode);
	}

	public override bool GetKeyDown()
	{
		return Input.GetKeyDown(keyCode);
	}
	
	public override bool GetKeyUp()
	{
		return Input.GetKeyUp(keyCode);
	}

	
	public override float GetAxis()
	{
		if (GetKey() == true)
			return 1;
		return 0;
	}
}