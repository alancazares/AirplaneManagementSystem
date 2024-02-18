using UnityEngine;

public class FenceController : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component attached to this GameObject
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    // Function to trigger the "openFence" animation
    public void OpenFence()
    {
        if (animator != null)
        {
            animator.ResetTrigger("CloseFenceTrigger");
            animator.SetTrigger("OpenFenceTrigger"); // Trigger the "openFence" animation
        }
        else
        {
            Debug.LogError("Animator component not found!");
        }
    }

    public void CloseFence()
    {
        if (animator != null)
        {
            animator.ResetTrigger("OpenFenceTrigger");
            animator.SetTrigger("CloseFenceTrigger"); // Trigger the "closeFence" animation
        }
        else
        {
            Debug.LogError("Animator component not found!");
        }
    }
}
