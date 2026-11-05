using UnityEngine;

public class projectile : MonoBehaviour
{
    //the bullet object that the enemy will shoot
    public GameObject bullet;
    //how fast the bullets will move
    public float bulletSpeed = 5;
    //how often the enemy will shoot
    public float shootDelay = 3;
    //how close the player has to be for the enemy to shoot at them
    public float shootTriggerDistance = 4;
    float timer = 0;
    //player reference
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //every X seconds, we want to shoot at the player if the player is close
        //delta means change
        //so delta time means the change in time between frame updates
        //here we are adding the slivers of time between frames, to figure out the passage of time
        timer += Time.deltaTime;
        if (timer > shootDelay)
        {
            //destination - starting position to calculate the vector
            Vector3 shootDir = player.transform.position - transform.position;
            if (shootDir.magnitude < shootTriggerDistance)
            {
                timer = 0;
                //we have waited long enough to shoot
                //we spawn the bullet
                GameObject shot = Instantiate(bullet, transform.position, Quaternion.identity);
                //we normalize the vector so that the bullets speed is consistent regardless of how far away the player is
                shootDir.Normalize();
                //push the bullet in the shoot direction, scaled up by the speed
                shot.GetComponent<Rigidbody2D>().linearVelocity = shootDir * bulletSpeed;
                Destroy(shot, 10);
            }
        }

    }
}
