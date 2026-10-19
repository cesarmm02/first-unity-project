using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{
    // start with a speed for the enemy
    public Vector3 velocity;
    public Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = RandomVector(-5f, 5f);
        velocity = rb.linearVelocity;
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    Vector3 RandomVector(float min, float max) 
    {
		    var x = Random.Range(min, max);
		    var y = Random.Range(min, max);
		    return new Vector2(x, y);
	}
    
    void OnCollisionEnter2D(Collision2D other)
    {
       // rb.linearVelocity = Vector2.Reflect(rb.linearVelocity, Vector3.right);
    }
}
