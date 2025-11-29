using System;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }

    private InputSystem_Actions _inputActions;

    public event EventHandler OnShoot;

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start() {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Enable();

        _inputActions.Player.Attack.performed += Attack_performed;
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnShoot?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMovementVector() {
        Vector2 inputVector = _inputActions.Player.Move.ReadValue<Vector2>();
        return new Vector3(inputVector.x, 0, inputVector.y);
    }


}
