using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class CharacterStackSystem : MonoBehaviour
{
    [SerializeField] private Transform stackPoint;
    [SerializeField] private Transform trashPoint;
    public int characterStackLimit;
    public List<StackObjectBase> stackedRawObjects;
    [SerializeField] private List<StackObjectBase> stackedFinalObjects;

    private bool isTakingObject;
    private bool isDroppingObject;
    private float yAxis;

    IEnumerator TakeStack(MachineControllerBase machineControllerBase)
    {
        if (isTakingObject) yield break;
        isTakingObject = true;

        if (machineControllerBase.spawnedObjects.Count >= 1)
        {
            var stackableObject = machineControllerBase.spawnedObjects[^1];
            machineControllerBase.spawnedObjects.Remove(stackableObject);

            stackableObject.transform.parent = stackPoint;
            stackableObject.transform.localRotation = quaternion.Euler(Vector3.zero);

            stackableObject.transform.DOLocalJump(new Vector3(0, 0 + yAxis, 0), 1, 1, .4f).OnComplete((() =>
            {
                if (machineControllerBase as SpawnerMachineController && stackedFinalObjects.Count == 0)
                {
                    stackedRawObjects.Add(stackableObject);
                }
                else if (machineControllerBase as TransformerMachineController && stackedRawObjects.Count == 0)
                {
                    stackedFinalObjects.Add(stackableObject);
                }
            }));

            yAxis += .5f;

            int totalObjects = machineControllerBase.spawnedObjects.Count;
            int newYAxisLevel = totalObjects / machineControllerBase.stackDropPoints.Length;
            machineControllerBase.yAxis = newYAxisLevel * machineControllerBase.yAxisOffset;

            machineControllerBase.pointIndex = totalObjects % machineControllerBase.stackDropPoints.Length;
        }

        yield return new WaitForSeconds(.1f);

        isTakingObject = false;
    }

    IEnumerator DropRawObjects(TransformerMachineController transformerMachineController)
    {
        if (isDroppingObject) yield break;
        isDroppingObject = true;

        if (stackedRawObjects.Count >= 1 && transformerMachineController.rawObjectsList.Count <
            transformerMachineController.rawObjectsStackLimit)
        {
            var dropableRawObject = stackedRawObjects[^1];
            stackedRawObjects.Remove(dropableRawObject);

            Transform nextPointPosition =
                transformerMachineController.rawObjectStackPoints[
                    transformerMachineController.rawObjectStackPointIndex];

            var targetPosition = new Vector3(nextPointPosition.position.x,
                nextPointPosition.position.y + transformerMachineController.rawObjectStackPointYAxis,
                nextPointPosition.position.z);

            dropableRawObject.transform.DOJump(targetPosition, 1f, 1, .4f).OnComplete((() =>
            {
                dropableRawObject.transform.parent = nextPointPosition;
                dropableRawObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                transformerMachineController.rawObjectsList.Add(dropableRawObject);
            }));

            yAxis -= .5f;
            if (yAxis < 0) yAxis = 0;

            if (transformerMachineController.rawObjectStackPointIndex <
                transformerMachineController.rawObjectStackPoints.Length - 1)
            {
                transformerMachineController.rawObjectStackPointIndex++;
            }
            else
            {
                transformerMachineController.rawObjectStackPointIndex = 0;
                transformerMachineController.rawObjectStackPointYAxis +=
                    transformerMachineController.rawObjectStackPointYAxisOffset;
            }
        }

        yield return new WaitForSeconds(.2f);

        isDroppingObject = false;
    }

    IEnumerator DropFinalObjects()
    {
        if (isDroppingObject) yield break;
        isDroppingObject = true;

        if (stackedFinalObjects.Count >= 1)
        {
            var dropableFinalObject = stackedFinalObjects[^1];
            stackedFinalObjects.Remove(dropableFinalObject);

            dropableFinalObject.transform.DOJump(trashPoint.position, 1f, 1, .4f).OnComplete((() =>
            {
                dropableFinalObject.transform.parent = trashPoint;
                dropableFinalObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                dropableFinalObject.DestroyObject();
            }));
            
            yAxis -= .5f;
            if (yAxis < 0) yAxis = 0;
        }

        yield return new WaitForSeconds(.2f);

        isDroppingObject = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TakeArea"))
        {
            var machineControllerBase = other.GetComponentInParent<MachineControllerBase>();
            StartCoroutine(TakeStack(machineControllerBase));
        }

        if (other.CompareTag("DropArea"))
        {
            var transformerMachineController = other.GetComponentInParent<TransformerMachineController>();
            StartCoroutine(DropRawObjects(transformerMachineController));
        }

        if (other.CompareTag("TrashArea"))
        {
            StartCoroutine(DropFinalObjects());
        }
    }
}