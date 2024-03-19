using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class StackObjectBase : MonoBehaviour
{
    public Vector3 jumpPosition;
    public MachineControllerBase machineControllerBase;
    public string objectId;
    private void OnEnable()
    {
        Jump();
    }

    private void Jump()
    {
        transform.DOJump(jumpPosition,2,1,.2f);
    }

    public void DestroyObject()
    {
        machineControllerBase.DeSpawn(this);
    }
}
