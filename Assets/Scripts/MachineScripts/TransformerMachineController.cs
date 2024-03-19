using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class TransformerMachineController : MachineControllerBase
{
    public Transform rawObjectJumpPosAtMachine;

    [Header("----- Raw Objects Control -----")]
    public List<StackObjectBase> rawObjectsList;

    public int rawObjectsStackLimit;
    public Transform[] rawObjectStackPoints;
    public int rawObjectStackPointIndex;
    public float rawObjectStackPointYAxis = 0f;
    public float rawObjectStackPointYAxisOffset;

    protected override IEnumerator SpawnCo()
    {
        while (true)
        {
            canSpawn = spawnedObjects.Count < spawnLimit && rawObjectsList.Count >= 1;

            if (canSpawn)
            {
                var lastRawObject = rawObjectsList[^1];
                
                rawObjectsList.Remove(lastRawObject);
                
                lastRawObject.transform.DOJump(rawObjectJumpPosAtMachine.position, 1, 1, .2f).OnComplete((() =>
                {
                    lastRawObject.DestroyObject();

                    var targetPosition = new Vector3(stackDropPoints[pointIndex].position.x,
                        stackDropPoints[pointIndex].position.y + yAxis, stackDropPoints[pointIndex].position.z);
                    Spawn(targetPosition);

                    if (pointIndex < stackDropPoints.Length - 1)
                    {
                        pointIndex++;
                    }
                    else
                    {
                        pointIndex = 0;
                        yAxis += yAxisOffset;
                    }
                }));
                
                int totalObjects = rawObjectsList.Count;
                int newYAxisLevel = totalObjects / rawObjectStackPoints.Length;
                rawObjectStackPointYAxis = newYAxisLevel * rawObjectStackPointYAxisOffset;
                    
                rawObjectStackPointIndex = totalObjects % rawObjectStackPoints.Length;

            }

            yield return new WaitForSeconds(.1f);
        }
    }
}