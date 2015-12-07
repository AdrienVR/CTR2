using UnityEngine;
using System.Collections;

public class testPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {

        m_controller = ControllerManager.Instance.GetController(0);
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (m_controller.GetKey("validate"))
        {
            m_rigidbody.position += transform.forward * Time.deltaTime*2f;
        }
    }
    private ControllerBase m_controller;
    private Rigidbody m_rigidbody;
}
