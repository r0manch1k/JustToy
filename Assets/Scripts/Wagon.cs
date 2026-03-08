using UnityEngine;

public class Wagon : MonoBehaviour
{
    [SerializeField] private float followDistance = 3f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private GameObject indicator;

    private Transform _leader;
    private bool _isAttached;
    private Wagon _nextWagon;
    private SphereCollider _trigger;

    public bool IsAttached => _isAttached;

    private void Awake()
    {
        _trigger = gameObject.AddComponent<SphereCollider>();
        _trigger.isTrigger = true;
        _trigger.radius = detectionRadius;

        if (!TryGetComponent<Rigidbody>(out _))
        {
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        if (indicator != null)
            indicator.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAttached) return;
        if (other.TryGetComponent<WagonAttacher>(out var attacher))
        {
            attacher.SetNearbyWagon(this);
            if (indicator != null)
                indicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isAttached) return;
        if (other.TryGetComponent<WagonAttacher>(out var attacher))
        {
            attacher.ClearNearbyWagon(this);
            if (indicator != null)
                indicator.SetActive(false);
        }
    }

    public void AttachTo(Transform newLeader)
    {
        _leader = newLeader;
        _isAttached = true;
        _trigger.enabled = false;

        if (indicator != null)
            indicator.SetActive(false);

        transform.position = _leader.position - _leader.forward * followDistance;
        transform.rotation = _leader.rotation;
    }

    public void SetNextWagon(Wagon wagon)
    {
        _nextWagon = wagon;
    }

    private void LateUpdate()
    {
        if (!_isAttached || _leader == null) return;

        Vector3 toLeader = _leader.position - transform.position;
        float distance = toLeader.magnitude;

        if (distance > followDistance)
            transform.position = _leader.position - toLeader.normalized * followDistance;

        Vector3 flatDir = toLeader;
        flatDir.y = 0f;
        if (flatDir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(flatDir.normalized);
    }
}
