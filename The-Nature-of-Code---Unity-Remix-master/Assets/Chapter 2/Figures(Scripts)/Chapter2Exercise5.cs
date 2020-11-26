using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Exercise5 : MonoBehaviour
{
    // Geometry defined in the inspector
    public float floorY;
    public Transform moverSpawnTransform;
    // Expose the components required to create the water
    public Transform fluidCornerA;
    public Transform fluidCornerB;
    public Material waterMaterial;
    public float fluidDrag;

    private List<Ch2Mover5> Movers = new List<Ch2Mover5>();
    private List<Fluid5> Fluids = new List<Fluid5>();

    void Start()
    {
        while (Movers.Count < 30)
        {
            Vector3 moverSpawnPosition = moverSpawnTransform.position + Vector3.right * Random.Range(-7, 7) + new Vector3(0f,3f,0f);
            Movers.Add(new Ch2Mover5(
                moverSpawnPosition,
                floorY
            ));
        }
        // Add the fluid to our scene
        Fluids.Add(new Fluid5(
            fluidCornerA.position,
            fluidCornerB.position,
            fluidDrag,
            waterMaterial
        ));
    }
    void FixedUpdate()
    {
        // Apply the forces to each of the Movers
        foreach (Ch2Mover5 mover in Movers)
        {
            // Check for interaction with any of our fluids
            foreach (Fluid5 fluid in Fluids)
            {
                if (mover.IsInside(fluid))
                {
                    // Apply a friction force that directly opposes the current motion
                    Vector3 friction = mover.body.velocity;

                    friction.Normalize();
                    friction *= -fluid.dragCoefficient * mover.body.transform.localScale.x;
                    mover.body.AddForce(friction, ForceMode.Force);
                }
            }
            mover.CheckBoundaries();
        }
    }
}

public class Ch2Mover5
{
    public Rigidbody body;
    private GameObject gameObject;
    private float size;

    private float yMin;

    public Ch2Mover5(Vector3 position, float yMin)
    {
        this.yMin = yMin;

        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body = gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<BoxCollider>());

        // Generate random properties for this mover
        size = Random.Range(0.2f, 0.5f);

        gameObject.transform.position = position + Vector3.up * size;

        gameObject.transform.localScale = 2 * size * Vector3.one;

        body.mass = size*size*size * (4f/3f);
    }

    // Checks to ensure the body stays within the boundaries
    public void CheckBoundaries()
    {
        Vector3 restrainedVelocity = body.velocity;
        if (body.position.y - size < yMin)
        {
            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
        }
        body.velocity = restrainedVelocity;
    }

    public bool IsInside(Fluid5 fluid)
    {
        // Check to see if the mover is inside the range on each axis.
        if (body.position.x > fluid.minBoundary.x &&
            body.position.x < fluid.maxBoundary.x &&
            body.position.y > fluid.minBoundary.y &&
            body.position.y < fluid.maxBoundary.y &&
            body.position.z > fluid.minBoundary.z &&
            body.position.z < fluid.maxBoundary.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class Fluid5
{
    public Vector3 minBoundary;
    public Vector3 maxBoundary;
    public float dragCoefficient;

    public Fluid5(Vector3 corner1, Vector3 corner2, float dragCoefficient, Material material)
    {
        // Get the minimum and maximum corners of the rectangular prism
        // This code allows the designer to place the volume corners at
        // any of the eight possible diagonals of a rectangular prism.
        minBoundary = new Vector3(
            Mathf.Min(corner1.x, corner2.x),
            Mathf.Min(corner1.y, corner2.y),
            Mathf.Min(corner1.z, corner2.z)
        );
        maxBoundary = new Vector3(
            Mathf.Max(corner1.x, corner2.x),
            Mathf.Max(corner1.y, corner2.y),
            Mathf.Max(corner1.z, corner2.z)
        );
        this.dragCoefficient = dragCoefficient;

        // Create the presence of the object in 3D space
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.GetComponent<Renderer>().material = material;
        // Remove undesired components that come with the primitive
        obj.GetComponent<BoxCollider>().enabled = false;
        Object.Destroy(obj.GetComponent<BoxCollider>());
        // Position and scale the new cube to match the boundaries.
        obj.transform.position = (corner1 + corner2) / 2;
        obj.transform.localScale = new Vector3(
            Mathf.Abs(corner2.x - corner1.x),
            Mathf.Abs(corner2.y - corner1.y),
            Mathf.Abs(corner2.z - corner1.z)
        );
    }
}
