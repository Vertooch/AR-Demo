using System;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController[] animationControllers;

	private const string ATTACK_TRIGGER = "Attack";

    private void Start()
    {
        // Swap for the idle animation
        SwapAnimationController();
    }

    public void AttackSoldier(SoldierController target)
	{
        // Swap again for the attack animation to create more unique soldiers
        SwapAnimationController();
        transform.LookAt(target.gameObject.transform);
        animator.SetTrigger(ATTACK_TRIGGER);
	}

    // Destroy each soldier once the animation sequence finishes.
    // This method is triggered using an animation event.
    public void FinishAttack()
    {
        Destroy(gameObject);
    }

    // Give the soldiers some variety using Animation Override Controllers
    private void SwapAnimationController()
    {
        int randomIndex = UnityEngine.Random.Range(0, animationControllers.Length);
        animator.runtimeAnimatorController = animationControllers[randomIndex];
    }
}
