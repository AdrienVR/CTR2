using UnityEngine;

public class MissileBehavior : WeaponBehavior
{
    public float DistanceBehindKart;
    public float ChangingTargetDelay;
    public float RotationFactor;
    public float Speed;
    public float HeightFromGround;
    public float SuperMultiplicator = 1.4f;

    public override void Initialize(PlayerController owner)
    {
        base.Initialize(owner);

        if (Owner.IsSuper())
        {
            Speed *= SuperMultiplicator;
            RotationFactor *= SuperMultiplicator;
        }
        transform.forward = Owner.transform.forward;
        m_rigidbody = GetComponent<Rigidbody>();

        transform.position = Owner.transform.position + Owner.transform.forward * DistanceBehindKart + Vector3.up * HeightFromGround;
    }

    void Update()
    {
        m_changingTargetTimer -= Time.deltaTime;
        if (m_changingTargetTimer < 0)
        {
            m_target = null;
            float minDistance = float.MaxValue;
            foreach(PlayerController enemy in PlayerManager.Instance.GetEnemies(Owner))
            {
                if (enemy.KartState.InvisibilityEquiped != null)
                    continue;
                float enemyDistance = (transform.position - enemy.transform.position).sqrMagnitude;

                if (enemyDistance < minDistance)
                {
                    m_target = enemy;
                    minDistance = enemyDistance;
					m_bipDelay = (enemyDistance/10000+0.15f)/1.5f;
                }
            }
            m_changingTargetTimer = ChangingTargetDelay;
        }
		m_bipTimer -= Time.deltaTime;
		if(m_bipTimer <0)
		{
			AudioManager.Instance.Play("bipMissile2");
			m_bipTimer = m_bipDelay;
		}
        Vector3 enemyDirection = transform.forward;
        if (m_target != null)
        {
            enemyDirection = (m_target.transform.position - transform.position);
            enemyDirection.y = 0;
        }

        RaycastHit hitGround;
        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up * 3, out hitGround, 3, s_groundLayerMask) == false)
        {
            // is in air
            enemyDirection.y = -10f;
            if (transform.forward.y > 5)
            {
                transform.forward = new Vector3(transform.forward.x, 5, transform.forward.z);
            }
        }
        else if (transform.forward.y < 0)
        {
            transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        }

        Vector3 direction = Vector3.Lerp(transform.forward, enemyDirection.normalized, Time.deltaTime * RotationFactor);
        transform.forward = direction;

        m_rigidbody.position += (direction * Speed);
    }
    
    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player != Owner)
            {
				Explode();
                player.Hit(Owner, name);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
			Explode();
        }
    }

	private void Explode()
	{
		AudioManager.Instance.Play("loudExplosion");
		Destroy(gameObject);
	}

    private static int s_groundLayerMask = 1 << LayerMask.NameToLayer("Ground");

    private Rigidbody m_rigidbody;
    private PlayerController m_target;
	private float m_changingTargetTimer, m_bipTimer, m_bipDelay;
}
