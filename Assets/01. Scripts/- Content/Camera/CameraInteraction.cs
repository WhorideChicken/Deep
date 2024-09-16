using System.Collections;
using UnityEngine;

public class CameraInteraction : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 100.0f;

    private bool _isDragging = false;
    private float _currentYRotation = 45.0f;
    private float _holdingXRotation = 30.0f;

    private void Update()
    {
        HandleMouseInput();
    }

    private void LateUpdate()
    {
        RotateCamera();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }

    private void RotateCamera()
    {
        if (_isDragging && GameManager.Instance.IsMovable())
        {
            float mouseX = Input.GetAxis("Mouse X");

            _currentYRotation += mouseX * _rotationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Euler(_holdingXRotation, _currentYRotation, 0.0f);
        }
    }
}
