using UnityEngine;

public class ZombieSpawnManager : MonoBehaviour {
    [SerializeField] private GameObject _zombiePrefab;
    [SerializeField] private int _poolSize = 5;
    [SerializeField] private bool _poolCanExpand = false;
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private float _spawnRadius = 10f;
    [SerializeField] private float _yPos = 0f;
    private float _timer;

    private void Start() {
        for (int i = 0; i < _poolSize; i++) {
            GameObject zombie = Instantiate(_zombiePrefab, transform);
            zombie.SetActive(false);
        }
    }

    private void Update() {
        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval) {
            SpawnZombie();
            _timer = 0f;
        }
    }

    private void SpawnZombie() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject zombie = transform.GetChild(i).gameObject;

            if (!zombie.activeInHierarchy) {
                Vector3 spawnPos = Random.insideUnitSphere * _spawnRadius;
                zombie.transform.position = new Vector3(spawnPos.x, _yPos, spawnPos.z);
                zombie.SetActive(true);
                return;
            }
        }

        GameObject newZombie = ExpandPool();
        if (newZombie != null) {
            newZombie.SetActive(true);
        }
    }

    private GameObject ExpandPool() {
        if(!_poolCanExpand) return null;

        GameObject zombie = Instantiate(_zombiePrefab, transform);
        zombie.SetActive(false);
        _poolSize++;
        return zombie;
    }
}
