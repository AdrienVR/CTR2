using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Axis : VirtualKey
{
	private string axisName;

	public int historyCount = 5;

	private List<float> m_valueStates;
	private float m_currentValue;
	private int m_index;

	//private float m_minValue;
	private float m_maxValue;

	private float thresholdAxis = 0.9f;
	
	public Axis(string actionName, string axisName, float minValue, float maxValue) 
	{
		this.actionName = actionName;
		this.axisName = axisName;
		this.keyName = axisName + (maxValue == 1 ? "+" : "-");

		//m_minValue = minValue;
		m_maxValue = maxValue;

		m_valueStates = new List<float>();
		for (int i = 0 ; i < historyCount ; i++)
		{
			m_valueStates.Add(0);
		}
	}

	public override void UpdateInternal()
	{
		UpdateValues();
	}

	private void UpdateValues()
	{
		m_currentValue = Input.GetAxis (axisName);
		m_valueStates.RemoveAt(0);
		m_valueStates.Insert(m_valueStates.Count, m_currentValue);
	}

	public override bool GetKey()
	{
		return (GetState(m_currentValue) == State.On);
	}

	public override bool GetKeyDown()
	{
		if (GetState(m_currentValue) == State.On)
		{
			if (GetState(m_valueStates[historyCount - 2]) == State.On)
				return false;
			else
				return true;
		}
		return false;
	}
	
	public override bool GetKeyUp()
	{
		if (GetState(m_currentValue) == State.Off)
		{
			if (GetState(m_valueStates[historyCount - 2]) == State.Off)
				return false;
			else
				return true;
		}
		return false;
	}
	
	
	public override float GetAxis()
	{
		float raw = Input.GetAxis(axisName);
		return raw / m_maxValue;
	}

	private State GetState(float axisValue)
	{
		float maxValue = m_maxValue;
		if (m_maxValue < 0)
		{
			maxValue *= -1;
			axisValue *= -1;
		}

		if (axisValue > maxValue * thresholdAxis)
		{
			return State.On;
		}
		else if (axisValue > maxValue * (1f - thresholdAxis))
		{
			return State.Between;
		}
		else
		{
			return State.Off;
		}
	}

	enum State
	{
		On = 1,
		Off = 2,
		NaN = 3,
		Between = 4
	}
}