
using UnityEngine;

namespace Assets.scripts.visual
{
    public class UIHealthDamage : MonoBehaviour
    {
        [Header("Leave empty to auto-grab the 6 child hearts inside this prefab")]
        [SerializeField] private Animator[] heartAnimators;
        [SerializeField] private string damagedBoolName = "IsDamaged";

        private int damagedBoolHash;
        private int lastHealth = -1;

        private void Awake()
        {
            damagedBoolHash = Animator.StringToHash(damagedBoolName);

            if (heartAnimators == null || heartAnimators.Length == 0)
            {
                heartAnimators = GetComponentsInChildren<Animator>(true);
            }
        }

        private void OnEnable()
        {
            if (lastHealth >= 0)
            {
                UpdateHearts(lastHealth);
            }
        }

        public void UpdateHearts(int currentHealth)
        {
            lastHealth = currentHealth;

            if (heartAnimators == null || heartAnimators.Length == 0)
            {
                heartAnimators = GetComponentsInChildren<Animator>(true);
            }

            for (int i = 0; i < heartAnimators.Length; i++)
            {
                Animator anim = heartAnimators[i];
                if (anim != null)
                {
                    bool isHeartDamaged = i >= currentHealth;
                    anim.SetBool(damagedBoolHash, isHeartDamaged);
                }
            }
        }
    }
}