using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Exercise1 : MonoBehaviour
{
    // Declare a mover object
    private NewCh1Mover mover;

    // Start is called before the first frame update
    void Start()
    {
        // Create a Mover object
        mover = new NewCh1Mover();
    }

    void Update()
    {
        mover.Update();
        mover.CheckEdges();
    }
}

public class NewCh1Mover
{
    private Vector2 location, velocity, acceleration;
    private float topSpeed;
    private bool brakes = true;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public NewCh1Mover()
    {
        findWindowLimits();
        location = Vector2.zero; // Vector2.zero is a (0, 0) vector
        velocity = Vector2.zero;
        topSpeed = 10F;
        brakes = true;

        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Update()
    {
        if (brakes == true)
        {
            topSpeed = 0f;
        }
        else if (brakes == false)
        {
            topSpeed = 10f;
        }
        // Speeds up the mover
        if (Input.GetKey(KeyCode.DownArrow))
        {
            acceleration = new Vector2(-0.1F, -1F);

            if (velocity.x <= 0 && velocity.y <= 0)
            {
                brakes = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            acceleration = new Vector2(0.1F, 1F);
            brakes = false;
        }
        // Limit Velocity to the top speed
        velocity = Vector2.ClampMagnitude(velocity, topSpeed);

        velocity += acceleration * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

        // Moves the mover
        location += velocity * Time.deltaTime;

        // Updates the GameObject of this movement
        mover.transform.position = new Vector2(location.x, location.y);
    }

    public void CheckEdges()
    {
        //Updates the game object to appear at other parts of the screen
        if (location.x > maximumPos.x)
        {
            location.x -= maximumPos.x - minimumPos.x;
        }
        else if (location.x < minimumPos.x)
        {
            location.x += maximumPos.x - minimumPos.x;
        }
        if (location.y > maximumPos.y)
        {
            location.y -= maximumPos.y - minimumPos.y;
        }
        else if (location.y < minimumPos.y)
        {
            location.y += maximumPos.y - minimumPos.y;
        }
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;

        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
