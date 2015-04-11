
public abstract class VirtualKey
{

	public string actionName;
	public string keyName;

	public virtual void UpdateInternal()
	{
	}

	public abstract bool GetKey();

	public abstract bool GetKeyDown();
	
	public abstract bool GetKeyUp();
	
	public abstract float GetAxis();

}