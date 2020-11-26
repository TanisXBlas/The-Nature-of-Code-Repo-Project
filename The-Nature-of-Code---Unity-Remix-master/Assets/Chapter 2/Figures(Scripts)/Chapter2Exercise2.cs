using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Exercise2 : MonoBehaviour
{
    // Geometry defined in the inspector.
    public float floorY;
    public float leftWallX;
    public float rightWallX;
    public float ceiling;
    public Transform moverSpawnTransform;

    private List<Ch2Mover2> Movers = new List<Ch2Mover2>();
    // Define constant forces in our environment
    private Vector3 left = new Vector3(0.0004f, 0f, 0f);
    private Vector3 right = new Vector3(0.0004f, 0f, 0f);
    private Vector3 top = new Vector3(0, 0.0004f, 0f);
    private Vector3 down = new Vector3(0, 0.0004f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        // Create copys of our mover and add them to our list
        while (Movers.Count < 30)
        {
            Movers.Add(new Ch2Mover2(moverSpawnTransform.position,leftWallX,rightWallX,floorY,ceiling));
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Apply the forces to each of the Movers
        foreach (Ch2Mover2 mover in Movers)
        {
            float PowerFromLeft = DistanceX(mover.body.transform.position.x,leftWallX);
            float PowerFromRight = DistanceX(mover.body.transform.position.x, rightWallX);
            float PowerFromFloor = DistanceY(mover.body.transform.position.y, floorY);
            float PowerFromCeiling = DistanceY(mover.body.transform.position.y, ceiling);

            // ForceMode.Impulse takes mass into account
            //Sends object to the left
            if (mover.body.transform.position.x >= 0)
            {
                mover.body.AddForce(right * -PowerFromRight, ForceMode.Impulse);
            }
            //Sends object to the right
            if (mover.body.transform.position.x < 0)
            {
                mover.body.AddForce(left * PowerFromLeft, ForceMode.Impulse);
            }
            //Sends object to the bottom
            if (mover.body.transform.position.y >= 0)
            {
                mover.body.AddForce(top * -PowerFromCeiling, ForceMode.Impulse);
            }
            //Sends object to the top
            if (mover.body.transform.position.y < 0)
            {
                mover.body.AddForce(down * PowerFromFloor, ForceMode.Impulse);
            }

            mover.CheckBoundaries();
        }
    }
    //Checks distance/power based on their position from x=0
    public float DistanceX(float moverX, float limit)
    {
        if (moverX < 0 && limit <= moverX)
        {
            moverX = moverX * -1;
        }
        else if (moverX > 0 && moverX <= limit)
        {
            moverX = moverX;
        }
        float distance = moverX;
        return distance;
    }
    //Checks distance/power based on their position from y=0
    public float DistanceY(float moverY, float limit)
    {
        if (moverY < 0 && limit <= moverY)
        {
            moverY = moverY * -1;
        }
        else if (moverY > 0 && moverY <= limit)
        {
            moverY = moverY;
        }
        float distance = moverY;
        return distance;
    }
    public Vector2 multiplyVector(Vector2 toMultiply, float scaleFactor)
    {
        float x = toMultiply.x * scaleFactor;
        float y = toMultiply.y * scaleFactor;
        return new Vector2(x, y);
    }
}

public class Ch2Mover2
{
    public Rigidbody body;
    private GameObject gameObject;
    private float radius;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    public Ch2Mover2(Vector3 position, float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;

        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());

        //using our own version of gravity
        body.useGravity = false;

        // Makes the object different sizes
        radius = Random.Range(0.1f, 0.4f);

        // Place our mover at the specified spawn position
        gameObject.transform.position = position + Vector3.up * radius;

        // Scale up object
        gameObject.transform.localScale = 2 * radius * Vector3.one;
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;
    }

    // Checks to ensure the body stays within the boundaries
    public void CheckBoundaries()
    {
        Vector3 restrainedVelocity = body.velocity;
        if (body.position.y - radius < yMin)
        {
            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
        }
        else if (body.position.y + radius > yMax)
        {
            restrainedVelocity.y = -Mathf.Abs(restrainedVelocity.y);
        }
        if (body.position.x - radius < xMin)
        {
            restrainedVelocity.x = Mathf.Abs(restrainedVelocity.x);
        }
        else if (body.position.x + radius > xMax)
        {
            restrainedVelocity.x = -Mathf.Abs(restrainedVelocity.x);

        }
        body.velocity = restrainedVelocity;
    }
}
