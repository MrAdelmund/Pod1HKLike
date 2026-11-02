using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }

    }
}
