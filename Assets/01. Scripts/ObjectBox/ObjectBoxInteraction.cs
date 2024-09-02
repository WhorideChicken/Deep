using UnityEngine;

public class ObjectBoxInteraction : InteractionObject
{
    [SerializeField] Transform _loading;
    public override void Interaction()
    {

    }

    public override void HoldingInteraction(float value = 0.0f)
    {
        Vector3 currentRotation = _loading.transform.localEulerAngles;
        currentRotation.z += value;
        _loading.transform.localEulerAngles = currentRotation;
    }
}
