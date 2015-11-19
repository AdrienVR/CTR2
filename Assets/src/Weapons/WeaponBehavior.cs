using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WeaponBehavior : MonoBehaviour
{
    public KartScript Owner;
    public AnimationClip explosionClip;
    public Color explosionColor;
    public Vector3 vitesseInitiale;
    public Vector3 vitesseInstant;
    public GameObject owner;
    public float lifeTime = 12;
    public float explosionRadius = 3f;

    public bool disamorced = false;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
    }

    void OnTriggerStay(Collider other)
    {
    }


    void OnCollisionEnter(Collision collision)
    {
    }
    
    private bool m_isAlive;
    private bool m_exploded = false;

    private bool m_lockExplosion = false;
    private KartScript m_kartCollided;
}