using System.Collections;
using UnityEngine;

public class CameraInteraction : MonoBehaviour
{
    #region 회전
    public float rotationSpeed = 100.0f;       

    private bool isDragging = false;      
    private float currentYRotation;
    #endregion 회전
    void Awake()
    {
        currentYRotation = 45;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        // 마우스 버튼을 놓으면 드래그 중지
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // 드래그 중일 때 마우스 이동에 따라 Y축 회전
        if (isDragging)
        {
            float mouseX = Input.GetAxis("Mouse X");  // 마우스 X 이동값

            // 회전 계산 (제한 없이)
            currentYRotation += mouseX * rotationSpeed * Time.deltaTime;

            // 카메라의 Y축 회전 적용
            transform.rotation = Quaternion.Euler(30, currentYRotation, 0);
        }
    }

}
