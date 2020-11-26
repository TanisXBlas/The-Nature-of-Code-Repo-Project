using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Exercise3 : MonoBehaviour
{
    public float floor;
    public float leftWall;
    public float rightWall;
    public float ceiling;
    public Transform moverTransform;

    private List<Ch2Mover3> Movers = new List<Ch2Mover3>();
    // Define constant forces in our environment
    private Vector3 wind = new Vector3(0.002f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        // Create copys of our mover and add them to our list
        while (Movers.Count < 30)
        {
            Movers.Add(new Ch2Mover3(
                moverTransform.position,
                leftWall,
                rightWall,
                floor,
                ceiling
            ));
        }
    }

    private void FixedUpdate()
    {
        float frictionStrength;
        float slipperyStrength;
        foreach (Ch2Mover3 mover in Movers)
        {
            // ForceMode.Impulse takes mass into account
            mover.rigidbody.AddForce(wind, ForceMode.Impulse);

            // Apply a friction force that directly opposes the current motion
            //First pocket of friction
            if (mover.rigidbody.transform.position.x < -6 && mover.rigidbody.transform.position.y > 2)
            {
                frictionStrength = 0.5f;

                Vector3 friction = mover.rigidbody.velocity;
                friction.Normalize();
                friction *= -frictionStrength;
                mover.rigidbody.AddForce(friction, ForceMode.Force);
            }
            //First slippery pocket
            else if (mover.rigidbody.transform.position.x > -6 && mover.rigidbody.transform.position.x < -2 && mover.rigidbody.transform.position.y <= 2 )
            {
                slipperyStrength = 2f;

                Vector3 slippery = mover.rigidbody.velocity;
                slippery.Normalize();
                slippery *= slipperyStrength;
                mover.rigidbody.AddForce(slippery, ForceMode.Force);
            }
            //Second friction pocket
            else if (mover.rigidbody.transform.position.x >= -2)
            {
                frictionStrength = 1f;

                Vector3 friction = mover.rigidbody.velocity;
                friction.Normalize();
                friction *= -frictionStrength;
                mover.rigidbody.AddForce(friction, ForceMode.Force);
            }

            mover.CheckLimits();
        }
    }
}
public class Ch2Mover3
{
    public Rigidbody rigidbody;
    private GameObject gameObject;
    private float radius;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    public Ch2Mover3(Vector3 position, float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.yMin = yMin;
        this.xMax = xMax;
        this.yMax = yMax;

        //Creates object to be visible
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rigidbody = gameObject.AddComponent<Rigidbody>();
        // Remove functionality that comes with the primitive that we don't want
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());

        //Spheres of various sizes
        radius = Random.Range(0.1f, 0.4f);

        //Object placement to be at the bottom of the sphere
        gameObject.transform.position = position + Vector3.up * radius;

        //Scales up the GameObject
        gameObject.transform.localScale = 2 * radius * Vector3.one;

        //Calculates mass where mass is proportional to volume/even density
        rigidbody.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;


    }
    public void CheckLimits()
    {
        Vector3 velocityLimits = rigidbody.velocity;
        if (rigidbody.position.y - radius < yMin)
        {
            velocityLimits.y = Mathf.Abs(velocityLimits.y);
        }
        else if (rigidbody.position.y + radius > yMax)
        {
            velocityLimits.y = -Mathf.Abs(velocityLimits.y);
        }
        if (rigidbody.position.x - radius < xMin)
        {
            velocityLimits.x = Mathf.Abs(velocityLimits.x);
        }
        else if (rigidbody.position.x + radius > xMax)
        {
            velocityLimits.x = -Mathf.Abs(velocityLimits.x);

        }
        rigidbody.velocity = velocityLimits;
    }
}
