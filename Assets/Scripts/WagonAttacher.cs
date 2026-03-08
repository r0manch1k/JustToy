using UnityEngine;
using UnityEngine.InputSystem;

public class WagonAttacher : MonoBehaviour
{
    private Wagon _nearbyWagon;
    private Wagon _lastAttachedWagon;
    private InputAction _interactAction;

    private void Awake()
    {
        _interactAction = InputSystem.actions.FindAction("Player/Interact");
    }

    private void OnEnable()
    {
        _interactAction.performed += OnInteract;
    }

    private void OnDisable()
    {
        _interactAction.performed -= OnInteract;
    }

    public void SetNearbyWagon(Wagon wagon)
    {
        _nearbyWagon = wagon;
    }

    public void ClearNearbyWagon(Wagon wagon)
    {
        if (_nearbyWagon == wagon)
            _nearbyWagon = null;
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (_nearbyWagon == null || _nearbyWagon.IsAttached) return;

        Transform leader = _lastAttachedWagon != null
            ? _lastAttachedWagon.transform
            : transform;

        _nearbyWagon.AttachTo(leader);

        if (_lastAttachedWagon != null)
            _lastAttachedWagon.SetNextWagon(_nearbyWagon);

        _lastAttachedWagon = _nearbyWagon;
        _nearbyWagon = null;
    }
}
