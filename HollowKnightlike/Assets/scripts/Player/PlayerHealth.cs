using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
     private int currentHealth;
    public int health = 6;
    //the text object we want to update
    public TMP_Text text;
    //when the player hits an enemy
    //reduce the player health
    //then, if health is too low, you die, aka reload the level
   
    void Start()
    {
        text.text = "Health: " + health;

        //CHALLENGE
        //update your player collectable code to display how many collectables you have

        //Change player shoot to have a fixed amount of ammo. Display ammo on the canvas, don't shoot if you don't have ammo

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("I hit an enemy!");
            health -= 1;
            //update my health text
            
            if (health <= 0)
            {
                Debug.Log("You died!");
                SceneManager.LoadScene("DEATH");
            }
        
        }
        if (collision.gameObject.tag == "HealthPack")
        {
            health += 1;
            text.text = "Health: " + health;
        }
    }
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"Player took {damageAmount} damage! Health remaining: {currentHealth}");

        // Optional: Trigger a hurt animation or flash effect here
        // anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("You died!");
        SceneManager.LoadScene("DEATH");
    }
}
