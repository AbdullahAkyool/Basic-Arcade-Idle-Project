using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    public CharacterStateSystem characterStateSystem;
    private Animator stateMachineAnimator;

    private void Awake()
    {
        characterStateSystem = GetComponent<CharacterStateSystem>();
        stateMachineAnimator = GetComponent<Animator>();
    }
    
    public void SetStateTransition()
    {
        stateMachineAnimator.SetBool("isEmpty",characterStateSystem.isEmpty);
        stateMachineAnimator.SetBool("isTaking",characterStateSystem.isTaking);
        stateMachineAnimator.SetBool("isCarrying",characterStateSystem.isCarrying);
        stateMachineAnimator.SetBool("isDropping",characterStateSystem.isDropping);
    }
    
    public void ResetAllConditions()
    {
        stateMachineAnimator.SetBool("isEmpty", false);
        stateMachineAnimator.SetBool("isTaking", false);
        stateMachineAnimator.SetBool("isCarrying", false);
        stateMachineAnimator.SetBool("isDropping", false);
    }
}
