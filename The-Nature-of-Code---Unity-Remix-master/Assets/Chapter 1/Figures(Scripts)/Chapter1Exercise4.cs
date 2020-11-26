using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Exercise4 : MonoBehaviour
{
    NewCh1Mover4 mover;
    void Start()
    {
        mover = new NewCh1Mover4();
    }
    void Update()
    {
        //allows the mover to follow the mouse
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mover.subtractVectors(mousePos, mover.location);
        if (dir.magnitude >= mousePos.magnitude)
        {
            mover.acceleration = mover.multiplyVector(dir.normalized, 6f);
        }
        else
        {
            mover.acceleration = mover.multiplyVector(dir.normalized, 2f);
        }
        mover.Update();
    }

}

public class NewCh1Mover4
{
    // The basic properties of a mover class
    public Vector2 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public NewCh1Mover4()
    {
        findWindowLimits();
        location = Vector2.zero; 
        velocity = Vector2.zero;
        topSpeed = 2;

        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Update()
    {
        CheckEdges();

        if (velocity.magnitude <= topSpeed)
        {
            velocity += acceleration * Time.deltaTime;

            velocity = Vector2.ClampMagnitude(velocity, topSpeed);

            location += velocity * Time.deltaTime;

            mover.transform.position = new Vector3(location.x, location.y, 0);
        }
        else
        {
            velocity -= acceleration * Time.deltaTime;
            location += velocity * Time.deltaTime;
            mover.transform.position = new Vector3(location.x, location.y, 0);
        }
    }

    public void CheckEdges()
    {
        //sends object to the opposite edge of the screen
        if (location.x > maximumPos.x)
        {
            location.x -= maximumPos.x - minimumPos.x;
            acceleration = Vector2.zero;
            velocity = Vector2.zero;
        }
        else if (location.x < minimumPos.x)
        {
            location.x += maximumPos.x - minimumPos.x;
            acceleration = Vector2.zero;
            velocity = Vector2.zero;
        }
        if (location.y > maximumPos.y)
        {
            location.y -= maximumPos.y - minimumPos.y;
            acceleration = Vector2.zero;
            velocity = Vector2.zero;
        }
        else if (location.y < minimumPos.y)
        {
            location.y += maximumPos.y - minimumPos.y;
            acceleration = Vector2.zero;
            velocity = Vector2.zero;
        }
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
    public Vector2 subtractVectors(Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x - vectorB.x;
        float newY = vectorA.y - vectorB.y;
        return new Vector2(newX, newY);
    }
    public Vector2 multiplyVector(Vector2 toMultiply, float scaleFactor)
    {
        float x = toMultiply.x * scaleFactor;
        float y = toMultiply.y * scaleFactor;
        return new Vector2(x, y);
    }
}
