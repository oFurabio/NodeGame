using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {
    private Camera _mainCamera;
    private CharacterController _controller;

    [SerializeField] private float speed = 5f;

    private void Awake() {
        _controller = GetComponent<CharacterController>();
        _mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        MovePlayerFromCamera();
    }

    private void MovePlayerFromCamera() {
        float hor = InputManager.Instance.GetMovementVector().x;
        float ver = InputManager.Instance.GetMovementVector().z;

        Vector3 forward = Vector3.ProjectOnPlane(_mainCamera.transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(_mainCamera.transform.right, Vector3.up).normalized;

        Vector3 desiredMoveDirection = forward * ver + right * hor;

        if (desiredMoveDirection.sqrMagnitude > 1f)
            desiredMoveDirection.Normalize();

        _controller.SimpleMove(desiredMoveDirection * speed);
    }
}
