using UnityEngine;

public class PlayerShoot : MonoBehaviour {
    private Camera _mainCamera;
    [SerializeField] private Animator _gunAnimator;

    [SerializeField] private float _shootCooldown = 0.2f;
    private float _timer;

    private void Awake() {
        _mainCamera = Camera.main;
    }

    private void Start() {
        InputManager.Instance.OnShoot += InputManager_OnShoot;
    }

    private void Update() {
        if (_timer > 0f)
            _timer -= Time.deltaTime;
    }

    private void InputManager_OnShoot(object sender, System.EventArgs e) {
        if (_timer > 0f) return;

        _timer = _shootCooldown;

        if (_gunAnimator != null)
            _gunAnimator.SetTrigger("Shoot");

        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out RaycastHit hitInfo, 100f, ~0, QueryTriggerInteraction.Collide))
            if (hitInfo.collider.TryGetComponent(out IHitable hitable))
                hitable.Hit(hitInfo);


    }
}
