using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    public float rayDistance = 5.0f;  
    public LayerMask doorLayer;      
    public float rayHeightOffset = 1.5f;

    private InteractionObject currentInteraction = null;

    private void Start()
    {

    }
    void Update()
    {
        if (!GameManager.Instance.IsMovable())
            return;

        CheckInteraction();

        if (currentInteraction != null)
        {
            if (Input.GetKeyDown(KeyCode.F)) 
            {
                currentInteraction.Interaction();
            }
        }
     }

    void CheckInteraction()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * rayHeightOffset;

        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(rayOrigin, forward, out hit, rayDistance))
        {
            if ((doorLayer.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                hit.collider.gameObject.GetComponent<DoorInteraction>().OpenDoor();
            }
            else
            {
                hit.collider.gameObject.TryGetComponent<InteractionObject>(out currentInteraction);
            }
        }
        else
        {
            currentInteraction = null;
        }

        CanvasManager.instance.ScreenInteractionText(currentInteraction != null);
        Debug.DrawRay(rayOrigin, forward * rayDistance, Color.red);
    }

}
