using UnityEngine;


public class UIHealthDamage : MonoBehaviour
{
    [SerializeField] private Animator uiAnimator;
    [SerializeField] private string damagedBoolName = "IsDamaged";

    // Call this when the player takes damage
    public void OnPlayerTakeDamage()
    {
        if (uiAnimator != null)
        {
            uiAnimator.SetBool(damagedBoolName, true);
        }
    }

    // Optional: Call this if the player heals and you want to revert the image
    public void OnPlayerHeal()
    {
        if (uiAnimator != null)
        {
            uiAnimator.SetBool(damagedBoolName, false);
        }
    }
}