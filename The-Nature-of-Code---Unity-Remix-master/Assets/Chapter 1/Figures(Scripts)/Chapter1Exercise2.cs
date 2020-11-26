using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Exercise2 : MonoBehaviour
{
    private NewCh1Mover2 mover;

    void Start()
    {
        mover = new NewCh1Mover2();
    }
    void Update()
    {
        mover.Update();
        mover.CheckEdges();
    }
}

public class NewCh1Mover2
{
    // The basic properties of a mover class
    private Vector2 location, velocity, acceleration;
    private float topSpeed;
    float accelerationScale = 1f;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public NewCh1Mover2()
    {
        findWindowLimits();
        location = Vector2.zero; // Vector2.zero is a (0, 0) vector
        //has the mover start out still
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        topSpeed = 2F;

        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Update()
    {
        accelerationScale += 0.02f;
        acceleration.x = accelerationScale * Mathf.PerlinNoise(Time.time * .5f, 0.0f);
        acceleration.y = accelerationScale * Mathf.PerlinNoise(Time.time * 1f, 0.0f);

        // Speeds up the mover
        velocity += acceleration * Time.deltaTime; 

        // Limit Velocity to the top speed
        velocity = Vector2.ClampMagnitude(velocity, topSpeed);

        // Moves the mover
        location += velocity * Time.deltaTime;

        mover.transform.position = new Vector2(location.x, location.y);
    }

    public void CheckEdges()
    {
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




