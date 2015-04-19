using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {

	public Material ButtonMaterial;
	public GameObject ButtonPrefab;
	public float periodDuration = 2;
	public float colorDeltaRange = 0.5f;
	public Color BaseColor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		m_timer += Time.deltaTime;
		if (m_timer > periodDuration)
			m_timer = 0;

		float colorPercent = Mathf.Sin (m_timer / periodDuration * 2 * Mathf.PI) * 0.5f * colorDeltaRange + 0.5f;

		ButtonMaterial.color = new Color(colorPercent * BaseColor.r, colorPercent * BaseColor.g,
		                                 colorPercent * BaseColor.b, ButtonMaterial.color.a);
	}

	float m_timer;
}
