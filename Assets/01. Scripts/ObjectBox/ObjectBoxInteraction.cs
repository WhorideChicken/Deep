using UnityEngine;

public class ObjectBoxInteraction : InteractionObject
{
    [SerializeField] Transform _loading;
    private bool _IsDone = false;
    public override void Interaction()
    {

    }

    public override void HoldingInteraction(float value = 0.0f)
    {
        if (!_IsDone)
        {
            Vector3 currentRotation = _loading.transform.localEulerAngles;
            if (currentRotation.z < 180)
            {
                currentRotation.z += value;
                Debug.Log(currentRotation.z);
                _loading.transform.localEulerAngles = currentRotation;
            }
            else
            {
                _IsDone = true;
                _loading.transform.localRotation = Quaternion.identity;
                _loading.gameObject.SetActive(false);
            }
        }
        else
        {

        }
    }
}
