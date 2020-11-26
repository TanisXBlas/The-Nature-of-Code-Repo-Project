using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Exercise1 : MonoBehaviour
{
    public float floor;
    public float leftWall;
    public float rightWall;
    public float ceiling;
    public Transform moverTransform;

    Ch2Mover1 mover;

    // Start is called before the first frame update
    void Start()
    {
        mover = new Ch2Mover1(moverTransform.position, leftWall, rightWall, floor, ceiling);
    }

    private void FixedUpdate()
    {
        float windScale = 4f;
        float heliumScale = 2f;

        Vector3 wind = new Vector3(windScale * Mathf.PerlinNoise(Time.time * 2f, 0.0f),0f, 0f);
        Vector3 helium = new Vector3(0f,heliumScale * Mathf.PerlinNoise(Time.time * .04f, 0.0f), 0f);

        //Apply wind forces to object
        mover.rigidbody.AddForce(wind, ForceMode.Impulse);
        mover.rigidbody.AddForce(helium, ForceMode.Impulse);
        mover.CheckLimits();
    }
}
public class Ch2Mover1
{
    public Rigidbody rigidbody;
    private GameObject gameObject;
    private float radius;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    public Ch2Mover1(Vector3 position, float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.yMin = yMin;
        this.xMax = xMax;
        this.yMax = yMax;

        //Creates object to be visible
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rigidbody = gameObject.AddComponent< Rigidbody > ();

        radius = 1f;

        //Object placement to be at the bottom of the sphere
        gameObject.transform.position = position + Vector3.up * radius;

        //Scales up the GameObject
        gameObject.transform.localScale = 2 * radius * Vector3.one;

        //Calculates mass where mass is proportional to volume/even density
        rigidbody.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;


    }
    public void CheckLimits()
    {
        Vector3 velocityLimit = rigidbody.velocity;
        if (rigidbody.position.y - radius < yMin)
        {
            velocityLimit.y = Mathf.Abs(velocityLimit.y);
        }
        else if (rigidbody.position.y + radius > yMax)
        {
            velocityLimit.y = -Mathf.Abs(velocityLimit.y);
        }
        if (rigidbody.position.x - radius < xMax)
        {
            velocityLimit.x = -Mathf.Abs(velocityLimit.x);
        }
        else if (rigidbody.position.x + radius > xMin)
        {
            velocityLimit.x = Mathf.Abs(velocityLimit.x);
        }
        rigidbody.velocity = velocityLimit;
    }
}
