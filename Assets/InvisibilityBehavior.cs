using UnityEngine;

public class InvisibilityBehavior : WeaponBehavior
{
	public float BaseLifetime = 7f;
	
	public override void Initialize(bool backWard)
	{
		
		CameraConfig.SetLayerRecursively(Owner.gameObject, Consts.Layer.layer2d_j1 + Owner.PlayerIndex);
		AudioManager.Instance.Play ("in_invisibility");
		for(int i=0; i< Owner.transform.GetChild(0).childCount;i++)
		{
			Owner.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled=false;
		}
		if (Owner.KartState.InvisibilityEquiped != null)
		{
			Owner.KartState.InvisibilityEquiped.SetLifetime();
			Destroy(gameObject);
			return;
		}
		
		transform.SetParent(Owner.transform);
		transform.localPosition = Vector3.zero;
		
		Owner.KartState.InvisibilityEquiped = this;
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
			CameraConfig.SetLayerRecursively(Owner.gameObject, Consts.Layer.Default);
			Destroy(gameObject);
			return;
		}
			}

	private float m_lifetime = 0f;
}
