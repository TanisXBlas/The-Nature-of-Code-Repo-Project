using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Exercise4 : MonoBehaviour
{
    // Geometry defined in the inspector
    public float floorY;
    public Transform moverSpawnTransform;
    // Expose the components required to create the water
    public Transform fluidCornerA;
    public Transform fluidCornerB;
    public Material waterMaterial;
    public float fluidDrag;

    private List<Ch2Mover4> Movers = new List<Ch2Mover4>();
    private List<Fluid4> Fluids = new List<Fluid4>();

    void Start()
    {
        while (Movers.Count < 30)
        {
            Vector3 moverSpawnPosition = moverSpawnTransform.position + Vector3.right * Random.Range(-7, 7) + Vector3.up * Random.Range(-4, 4);
            Movers.Add(new Ch2Mover4(
                moverSpawnPosition,
                floorY
            ));
        }
        // Add the fluid to our scene
        Fluids.Add(new Fluid4(
            fluidCornerA.position,
            fluidCornerB.position,
            fluidDrag,
            waterMaterial
        ));
    }
    void FixedUpdate()
    {
        // Apply the forces to each of the Movers
        foreach (Ch2Mover4 mover in Movers)
        {
            // Check for interaction with any of our fluids
            foreach (Fluid4 fluid in Fluids)
            {
                if (mover.IsInside(fluid))
                {
                    // Apply a friction force that directly opposes the current motion
                    Vector3 friction = mover.body.velocity;

                    friction.Normalize();
                    friction *= -fluid.dragCoefficient;
                    mover.body.AddForce(friction, ForceMode.Force);
                }
            }

            mover.CheckBoundaries();
        }
    }
}

public class Ch2Mover4
{
    public Rigidbody body;
    private GameObject gameObject;
    private float radius;

    private float yMin;

    public Ch2Mover4(Vector3 position, float yMin)
    {
        this.yMin = yMin;

        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());

        // Generate random properties for this mover
        radius = Random.Range(0.2f, 0.6f);

        gameObject.transform.position = position + Vector3.up * radius;

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
        body.velocity = restrainedVelocity;
    }

    public bool IsInside(Fluid4 fluid)
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

public class Fluid4
{
    public Vector3 minBoundary;
    public Vector3 maxBoundary;
    public float dragCoefficient;

    public Fluid4(Vector3 corner1, Vector3 corner2, float dragCoefficient, Material material)
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
