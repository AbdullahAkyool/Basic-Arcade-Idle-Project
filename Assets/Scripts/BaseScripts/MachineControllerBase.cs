using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MachineControllerBase : MonoBehaviour
{
    [Header("----- Spawn -----")]
    public StackObjectBase spawnObjectPrefab;
    public List<StackObjectBase> spawnedObjects;
    public Transform spawnPoint;
    public int spawnLimit;
    public Transform[] stackDropPoints;
    public int pointIndex;
    public float yAxis = 0f;
    public float yAxisOffset;

    [Header("----- Pool -----")]
    [SerializeField] private int poolSize;
    private Queue<StackObjectBase> objectPool;
    protected bool canSpawn;

    private void Awake()
    {
        objectPool = new Queue<StackObjectBase>();
    }

   protected virtual void Start()
    {
        GrowPool();
        StartCoroutine(SpawnCo());
    }

    private void GrowPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var newObject = Instantiate(spawnObjectPrefab, spawnPoint);
            newObject.machineControllerBase = this;
            newObject.gameObject.SetActive(false);
            objectPool.Enqueue(newObject);
        }
    }

    protected void Spawn(Vector3 woodTargetPos)
    {
        if (objectPool.Count <= 0)
        {
            GrowPool();
        }

        var newObject = objectPool.Dequeue();
        newObject.transform.position = spawnPoint.position;

        newObject.jumpPosition = woodTargetPos;

        newObject.gameObject.SetActive(true);
        spawnedObjects.Add(newObject);
    }

    protected abstract IEnumerator SpawnCo();

    public void DeSpawn(StackObjectBase stackObject)
    {
        stackObject.gameObject.SetActive(false);
        stackObject.transform.parent = spawnPoint;
        objectPool.Enqueue(stackObject);
    }
}
