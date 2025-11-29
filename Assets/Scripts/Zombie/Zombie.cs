using System;
using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour, IHitable {
    [Header("Ragdoll Parts")]
    [SerializeField] private Collider[] _ragdollColliders;
    [SerializeField] private float _ragdollForceMultiplier = 10f;
    [SerializeField] private float _despawnDelay = 5f;
    private Rigidbody[] _ragdollRigidbodies;
    private Transform target;
    public static event EventHandler OnZombieKilled;

    [Header("Hit Detection")]
    [SerializeField] private BoxCollider hitDetectionCollider; // Novo

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    private Animator _animator;
    private bool _dead = false;

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Start() {
        DisableRagdoll();
    }

    private void DisableRagdoll() {
        _dead = false;

        foreach (var rb in _ragdollRigidbodies)
            rb.isKinematic = true;

        foreach (var col in _ragdollColliders)
            col.enabled = false;

        // Garante que o collider principal está ativo
        if (hitDetectionCollider != null)
            hitDetectionCollider.enabled = true;

        _animator.enabled = true;
    }

    private void EnableRagdoll() {
        _animator.SetBool("Walking", false);
        _animator.enabled = false;

        // Desativa o collider principal
        if (hitDetectionCollider != null)
            hitDetectionCollider.enabled = false;

        foreach (var rb in _ragdollRigidbodies)
            rb.isKinematic = false;

        foreach (var col in _ragdollColliders)
            col.enabled = true;
    }

    public void Hit(RaycastHit hit) {
        if (_dead) return;

        _dead = true;
        EnableRagdoll();

        Rigidbody closestRb = null;
        float closestDist = float.MaxValue;

        foreach (var rb in _ragdollRigidbodies) {
            float dist = Vector3.Distance(rb.position, hit.point);
            if (dist < closestDist) {
                closestDist = dist;
                closestRb = rb;
            }
        }

        if (closestRb != null) {
            Vector3 forceDirection = hit.normal * -1.0f;
            closestRb.AddForceAtPosition(10.0f * _ragdollForceMultiplier * forceDirection, hit.point, ForceMode.Impulse);
        }

        OnZombieKilled?.Invoke(this, EventArgs.Empty);
        StartCoroutine(DespawnAfterSeconds(_despawnDelay));
    }

    private void Update() {
        MoveToPlayer();
    }

    private IEnumerator DespawnAfterSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
        DisableRagdoll();
    }

    private void MoveToPlayer() {
        if (_dead) return;
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        transform.position += _moveSpeed * Time.deltaTime * direction;
        _animator.SetBool("Walking", true);
    }
}