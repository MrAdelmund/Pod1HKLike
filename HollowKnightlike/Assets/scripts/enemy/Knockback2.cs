using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Knockback2 : MonoBehaviour
{
    public PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerMovement.KBCounter = playerMovement.KBTotalTime;
        if (collision.transform.position.x <= transform.position.x)
        {
            playerMovement.KnockFromRight = true;
        }
        if (collision.transform.position.x > transform.position.x)
        {
            playerMovement.KnockFromRight = false;
        }
    }
}
