using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpawnerMachineController : MachineControllerBase
{
    protected override IEnumerator SpawnCo()
    {
        while (true)
        {
            canSpawn = spawnedObjects.Count < spawnLimit;

            if (canSpawn)
            {
                var targetPosition = new Vector3(stackDropPoints[pointIndex].position.x, stackDropPoints[pointIndex].position.y + yAxis, stackDropPoints[pointIndex].position.z);
                Spawn(targetPosition);
            
                if (pointIndex < stackDropPoints.Length-1)
                {
                    pointIndex++;
                }
                else
                {
                    pointIndex = 0;
                    yAxis += yAxisOffset;
                }
            }
            
            yield return new WaitForSeconds(.2f);
        }
    }
}

//public Transform stackPoint;
// public int rowCount;
// public int columnCount;
// public float spawnRate;
//
// public float xOffset;
// public float zOffset;

// int totalSpawned = 0;
// while (totalSpawned < spawnLimit)
// {
//   for (int x = 0; x < rowCount; x++)
//   {
//     zOffset = 0;
//
//     for (int z = 0; z < columnCount; z++)
//     {
//       if (totalSpawned >= spawnLimit)
//         break;
//
//       var targetPosition = new Vector3(stackPoint.position.x + xOffset, stackPoint.position.y + yHeightIncrease, stackPoint.position.z + zOffset);
//       Spawn(targetPosition);
//
//       totalSpawned++;
//       zOffset += .25f;
//             
//       yield return new WaitForSeconds(spawnRate);
//     
//       if (totalSpawned >= spawnLimit)
//         break;
//     }
//
//     xOffset += .25f;
//     if (totalSpawned >= spawnLimit)
//       break;
//   }
//   xOffset = 0;
//   yHeightIncrease += .25f;
// }