using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStackSystem : MonoBehaviour
{
    [SerializeField] private Transform stackPoint;
    [SerializeField] private Transform trashPoint;
    public int characterStackLimit;
    public List<StackObjectBase> stackedObjects;

    private bool isTakingObject;
    private bool isDroppingObject;
    private float yAxis;

   private Coroutine stackCoroutine;
   private Type allowedType;

    IEnumerator TakeStack(MachineControllerBase machineControllerBase)
    {
        while (true)
        {
            if (machineControllerBase.spawnedObjects.Count >= 1)
            {
                var stackableObject = machineControllerBase.spawnedObjects[^1];
                
                if (stackedObjects.Count == 0 || stackableObject.GetType() == allowedType)
                {
                    machineControllerBase.spawnedObjects.Remove(stackableObject);

                    stackableObject.transform.parent = stackPoint;
                    stackableObject.transform.localRotation = quaternion.Euler(Vector3.zero);

                    stackableObject.transform.DOLocalJump(new Vector3(0, 0 + yAxis, 0), 1, 1, .4f).OnComplete((() =>
                    {
                        stackedObjects.Add(stackableObject);
                        //allowedType ??= stackedObjects[^1].GetType();
                        allowedType = stackedObjects[^1].GetType();
                    }));

                    yAxis += .5f;

                    int totalObjects = machineControllerBase.spawnedObjects.Count;
                    int newYAxisLevel = totalObjects / machineControllerBase.stackDropPoints.Length;
                    machineControllerBase.yAxis = newYAxisLevel * machineControllerBase.yAxisOffset;

                    machineControllerBase.pointIndex = totalObjects % machineControllerBase.stackDropPoints.Length;
                }
            }

            yield return new WaitForSeconds(.2f);
        }
    }

    IEnumerator DropRawObjects(TransformerMachineController transformerMachineController)
    {
        if (isDroppingObject) yield break;
        isDroppingObject = true;

        if (stackedObjects.Count >= 1)
        {
            if (stackedObjects[0].GetComponent<RawObject>() && transformerMachineController.rawObjectsList.Count <
                transformerMachineController.rawObjectsStackLimit)
            {
                var dropableRawObject = stackedObjects[^1];
                stackedObjects.Remove(dropableRawObject);

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
        }

        yield return new WaitForSeconds(.2f);

        isDroppingObject = false;
    }

    IEnumerator DropStackedObjects()
    {
        if (isDroppingObject) yield break;
        isDroppingObject = true;

        if (stackedObjects.Count >= 1)
        {
            var dropableFinalObject = stackedObjects[^1];
            stackedObjects.Remove(dropableFinalObject);

            dropableFinalObject.transform.DOJump(trashPoint.position, 1f, 1, .4f).OnComplete((() =>
            {
                dropableFinalObject.transform.parent = trashPoint;
                dropableFinalObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                dropableFinalObject.DestroyObject();

               if(dropableFinalObject.GetComponent<FinalObject>()) MoneyManager.Instance.EarnMoney(Random.Range(2, 10));
            }));

            yAxis -= .5f;
            if (yAxis < 0) yAxis = 0;
        }

        yield return new WaitForSeconds(.2f);

        isDroppingObject = false;
    }

    private void OnTriggerStay(Collider other)
    {
        // if (other.CompareTag("TakeArea"))
        // {
        //     var machineControllerBase = other.GetComponentInParent<MachineControllerBase>();
        //     StartCoroutine(TakeStack(machineControllerBase));
        // }

        if (other.CompareTag("DropArea"))
        {
            var transformerMachineController = other.GetComponentInParent<TransformerMachineController>();
            StartCoroutine(DropRawObjects(transformerMachineController));
        }

        if (other.CompareTag("TrashArea"))
        {
            StartCoroutine(DropStackedObjects());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TakeArea"))
        {
            if (stackCoroutine == null)
            {
                var machineControllerBase = other.GetComponentInParent<MachineControllerBase>();
                stackCoroutine = StartCoroutine(TakeStack(machineControllerBase));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TakeArea"))
        {
            if (stackCoroutine != null)
            {
                StopCoroutine(stackCoroutine);
                stackCoroutine = null;
            }
        }
    }
}