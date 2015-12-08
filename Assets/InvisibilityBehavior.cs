using UnityEngine;
using System.Collections;

public class InvisibilityBehavior : WeaponBehavior
{
	
	public float BaseLifetime = 7f;
	
	public override void Initialize(PlayerController owner)
	{
		base.Initialize(owner);
		CameraConfig.SetLayerRecursively(owner.gameObject, LayerMask.NameToLayer("layer_j" + (owner.PlayerIndex + 1)));
		AudioManager.Instance.Play ("in_invisibility");
		for(int i=0; i< Owner.transform.GetChild(0).childCount;i++)
		{
			Owner.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled=false;
		}
		if (owner.KartState.InvisibilityEquiped != null)
		{
			owner.KartState.InvisibilityEquiped.SetLifetime();
			Destroy(gameObject);
			return;
		}
		
		transform.SetParent(owner.transform);
		transform.localPosition = Vector3.zero;
		
		owner.KartState.InvisibilityEquiped = this;
		SetLifetime();
	}

	
	public void SetLifetime()
	{
		if (Owner.IsSuper())
		{
			m_lifetime += BaseLifetime * 2;
		}
		else
		{
			m_lifetime += BaseLifetime;
		}
	}
	
	void Update()
	{
		m_lifetime -= Time.deltaTime;
		if (m_lifetime < 0)
		{
			//AudioManager.Instance.PlayDefaultMapMusic();
			Owner.KartState.InvisibilityEquiped = null;
			for(int i=0; i< Owner.transform.GetChild(0).childCount;i++)
			{
				Owner.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled=true;
			}
			AudioManager.Instance.Play ("out_invisibility");
			CameraConfig.SetLayerRecursively(Owner.gameObject, LayerMask.NameToLayer("Default"));
			Destroy(gameObject);
			return;
		}
			}

	private float m_lifetime = 0f;
}
