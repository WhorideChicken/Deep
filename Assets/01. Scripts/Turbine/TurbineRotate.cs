using UnityEngine;

public class TurbineRotate : MonoBehaviour
{
    public bool IsOn = true;
    public float rotationSpeed = 50f; // 회전 속도 (초당 회전 각도)
    void Update()
    {
        if(IsOn)
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
