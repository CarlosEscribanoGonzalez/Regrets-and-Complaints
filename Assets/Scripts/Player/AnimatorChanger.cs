using UnityEngine;

public class AnimatorChanger : MonoBehaviour
{
    private Animator animator;
    public Object[] controllers;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Object newController = controllers[IterationController.numIteration];
        if(newController != null) animator.runtimeAnimatorController = (RuntimeAnimatorController)newController;
    }
}
