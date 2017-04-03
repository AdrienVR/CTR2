using UnityEngine;

// this class manages the mouvements

public class KartControllerRay : MonoBehaviour
{	
	public static bool stop = true;
	public float speedCoeff = 20;
	public float turnCoeff = 45;
	public float timeMaxAcceleration = 1;

	private Vector3 postForce;

    public KartInput m_input;

	// Use this for initialization
	void Start ()
	{
	}

	public void UpdateGameplay()
	{
		Vector3 localForward = Vector3.forward;
		if(m_input.moveForward && !m_input.stop)
		{
			m_accelerationTime += 2 * Time.deltaTime;
			postForce = localForward*speedCoeff;
		}
		else if(m_input.vertical < 0)
		{
			m_accelerationTime += 2 * Time.deltaTime;
			postForce = -localForward*speedCoeff;
		}
		// wheels turning
		//if ((controller.GetKey("stop") || controller.GetKey("validate")))
		{
			if(m_input.horizontal > 0)
			{
				m_angle += 1;
			}
			else if(m_input.horizontal < 0)
            {
				m_angle -= 1;
			}
		}
		
		if(m_input.jump)//&& System.Math.Abs(Vector3.Angle(rigidbody.velocity,forwardNormal))>45)
		{
			//Debug.Log("Angle : "+transform.localRotation.eulerAngles.x+","+transform.localRotation.eulerAngles.z);
			//rigidbody.velocity = new Vector3(0,-26f,0);
		}
		
	}

	private float m_angle;
	
	void FixedUpdate()
	{

		//Debug.DrawRay(transform.position, -transform.up * 2);
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, 2f)){
			transform.up = Vector3.Slerp(transform.up, hit.normal, 0.05f);
		}
		transform.Rotate(Vector3.up, m_angle);

		m_accelerationTime -= Time.deltaTime;
		UpdateGameplay();
		transform.Rotate(Vector3.up, m_angle);

		m_accelerationTime = Mathf.Clamp(m_accelerationTime, 0, timeMaxAcceleration);

		if (m_accelerationTime > 0)
			GetComponent<Rigidbody>().velocity = Vector3.Slerp(Vector3.zero,postForce,m_accelerationTime/timeMaxAcceleration);
	}

	private float m_accelerationTime;
}
