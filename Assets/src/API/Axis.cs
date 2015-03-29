using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Axis {

	private string m_action;
	private string m_axisName;

	private List<int> m_valueStates;
	
	public Axis(string action, string axisName) {
		m_action = action;
		m_axisName = axisName;
	}
}