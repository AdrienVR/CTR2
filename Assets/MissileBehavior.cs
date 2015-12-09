using UnityEngine;

public class MissileBehavior : WeaponBehavior
{
    public float DistanceBehindKart = 1.5f;

    public float ChangingTargetDelay = 1;

    public float RotationFactor = 5;

    public float Speed = 20f;

    public float HeightFromGround = 2;

    public override void Initialize(PlayerController owner)
    {
        base.Initialize(owner);

        if (Owner.IsSuper())
        {
            Speed *= 2;
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
            float minDistance = float.MaxValue;
            foreach(PlayerController enemy in PlayerManager.Instance.GetEnemies(Owner))
            {
                float enemyDistance = (transform.position - enemy.transform.position).sqrMagnitude;
                if (enemyDistance < minDistance)
                {
                    m_target = enemy;
                    minDistance = enemyDistance;
                }
            }
            m_changingTargetTimer = ChangingTargetDelay;
        }

        Vector3 enemyDirection = (m_target.transform.position - transform.position);
        enemyDirection.y = 0;

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
                player.Die(Owner, name);
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private static int s_groundLayerMask = 1 << LayerMask.NameToLayer("Ground");

    private Rigidbody m_rigidbody;
    private PlayerController m_target;
    private float m_changingTargetTimer;
}
