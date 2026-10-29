using Unity.VisualScripting.ReorderableList;
using UnityEngine;


public class playerAnimator : MonoBehaviour
{

    Animator player;
    void Start()
    {
        player = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      

        if(GetComponent<PlayerMovement>().moveInput != 0)
        {
            player.SetBool("moving", true);
        }
        else
        {
            player.SetBool("moving", false);
        }

        if(!GetComponent<PlayerMovement>().isGrounded && GetComponent<Rigidbody2D>().linearVelocityY < 0.5f)
        {
            player.SetBool("falling", true);

            player.SetBool("jumping", false);
        }
        else if (!GetComponent<PlayerMovement>().isGrounded)
        {
            player.SetBool("falling", false);

            player.SetBool("jumping", true);
        }
        else
        {
            player.SetBool("falling", false);

            player.SetBool("jumping", false);
        }

        if (!GetComponent<PlayerMovement>().canDoubleJump)
        {
            player.SetBool("dJumping", true);
        }
        else
        {
            player.SetBool("dJumping", false);
        }

        if (GetComponent<PlayerMovement>().isDashing)
        {
            player.SetBool("dashing", true);
        }
        else
        {
            player.SetBool("dashing", false);
        }

        if (GetComponent<PlayerMovement>().isTouchingWall)
        {
            player.SetBool("clinging", true);
        }
        else
        {
            player.SetBool("clinging", false);
        }

        if (GetComponent<PlayerAttack>().isAttacking)
        {
            player.SetBool("attacking", true);
        }
        else
        {
            player.SetBool("attacking", false);
        }
    }
}
