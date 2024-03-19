using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStateSystem : MonoBehaviour
{
    [Header("--States--")] 
    public bool isEmpty;
    public bool isTaking;
    public bool isCarrying;
    public bool isDropping;

    protected CharacterStackSystem characterStackSystem;

    protected virtual void Awake()
    {
        characterStackSystem = GetComponent<CharacterStackSystem>();
    }

    protected void Start()
    {
        isEmpty = true;
    }
    
    public abstract void CharacterEmptyState();
    public abstract void CharacterTakingState();
    public abstract void CharacterCarryingState();
    public abstract void CharacterDroppingState();
}
