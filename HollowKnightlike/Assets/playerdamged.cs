using UnityEngine;

public class playerdamged : StateMachineBehaviour
{
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
    }
}
