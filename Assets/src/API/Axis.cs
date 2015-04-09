using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Axis : VirtualKey
{
	
	public new string actionName;
	public new string keyName;

	private List<float> m_valueStates;
	private float m_currentValue;
	private int m_index;

	private float m_minValue;
	private float m_maxValue;

	private float thresholdAxis;
	
	public Axis(string actionName, string axisName, float minValue, float maxValue) 
	{
		this.actionName = actionName;
		this.keyName = axisName;

		thresholdAxis = Game.thresholdAxis;

		m_minValue = minValue;
		m_maxValue = maxValue;

		m_valueStates = new List<float>();
		for (int i = 0 ; i < 5 ; i++)
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
		m_currentValue = Input.GetAxis (keyName);
		m_valueStates[4] = m_currentValue;
		m_valueStates.RemoveAt(0);
	}

	public override bool GetKey()
	{
		return (GetState(m_currentValue) == State.On);
	}

	public override bool GetKeyDown()
	{
		if (GetState(m_currentValue) == State.On)
		{
			for(int i = 3 ; i>= 0 ; i--)
			{
				if (GetState(m_valueStates[i]) == State.On)
					break;
				
				if (GetState(m_valueStates[i]) == State.Off)
					return true;
			}
		}
		return false;
	}
	
	public override bool GetKeyUp()
	{
		if (GetState(m_currentValue) == State.Off)
		{
			for(int i = 3 ; i>= 0 ; i--)
			{
				if (GetState(m_valueStates[i]) == State.Off)
					break;
				
				if (GetState(m_valueStates[i]) == State.On)
					return true;
			}
		}
		return false;
	}
	
	
	public override float GetAxis()
	{
		float raw = Input.GetAxis(keyName);
		if (m_minValue != 0)
		{
			return raw / m_minValue;
		}
		else
		{
			return raw / m_maxValue;
		}
	}

	private State GetState(float axisValue)
	{
		if (axisValue < m_maxValue && axisValue > m_minValue)
		{
			if (axisValue > m_minValue + (m_maxValue - m_minValue) * 0.5f + thresholdAxis)
			{
				return State.On;
			}
			else if (axisValue > m_minValue + (m_maxValue - m_minValue) * 0.5f - thresholdAxis)
			{
				return State.Between;
			}
			else
			{
				return State.Off;
			}
		}
		return State.NaN;
	}

	enum State
	{
		On = 1,
		Off = 2,
		NaN = 3,
		Between = 4
	}
}