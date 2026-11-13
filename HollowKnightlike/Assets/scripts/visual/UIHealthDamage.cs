using UnityEngine;

namespace Assets.scripts.visual
{
    public class UIHealthDamage : MonoBehaviour
    {
        [Header("Drag all 6 Heart UI Objects here (Left to Right)")]
        [SerializeField] private Animator[] heartAnimators;
        [SerializeField] private string damagedBoolName = "IsDamaged";

        // Call this whenever health changes and pass the current health value
        public void UpdateHearts(int currentHealth)
        {
            for (int i = 0; i < heartAnimators.Length; i++)
            {
                if (heartAnimators[i] != null)
                {
                    // Example: If health is 5, heart index 5 (the 6th heart) becomes damaged (true)
                    bool isHeartDamaged = i >= currentHealth;
                    heartAnimators[i].SetBool(damagedBoolName, isHeartDamaged);
                }
            }
        }
    }
}