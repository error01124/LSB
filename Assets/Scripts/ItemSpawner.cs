using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private GameObject _droppedItemPrefab;

    private void Start()
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            Spawn(spawnPoint.position);
        }
    }

    private void Spawn(Vector3 position)
    {
        var droppedItem = Instantiate(_droppedItemPrefab);
        droppedItem.transform.position = position;
    }
}
