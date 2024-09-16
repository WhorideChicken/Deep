using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    [SerializeField] private float _rayDistance = 5.0f;
    [SerializeField] private LayerMask _doorLayer;
    [SerializeField] private float _rayHeightOffset = 1.5f;
    [SerializeField] private float _increaseRate = 100f;

    private InteractionObject _currentInteraction = null;

    void Update()
    {
        if (!GameManager.Instance.IsMovable())
            return;

        CheckInteraction();

        if (_currentInteraction != null)
        {
            HandleInteractionInput();
        }
    }

    private void CheckInteraction()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * _rayHeightOffset;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, forward, out hit, _rayDistance))
        {
            int hitLayer = hit.collider.gameObject.layer;
            if (IsInLayerMask(hitLayer, _doorLayer))
            {
                hit.collider.gameObject.GetComponent<DoorInteraction>()?.OpenDoor();
            }
            else
            {
                hit.collider.gameObject.TryGetComponent(out _currentInteraction);
            }
        }
        else
        {
            _currentInteraction = null;
        }

        CanvasManager.Instance.ToggleInteractionText(_currentInteraction != null);
        Debug.DrawRay(rayOrigin, forward * _rayDistance, Color.red);
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && !_currentInteraction.IsHolding)
        {
            _currentInteraction.Interaction();
        }
        else if (Input.GetKey(KeyCode.F) && _currentInteraction.IsHolding)
        {
            float interactionValue = _increaseRate * Time.deltaTime;
            _currentInteraction.HoldingInteraction(interactionValue);
        }
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << layer)) > 0);
    }

}
