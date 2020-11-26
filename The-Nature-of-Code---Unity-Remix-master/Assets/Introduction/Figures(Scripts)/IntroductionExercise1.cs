using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionExercise1 : MonoBehaviour
{
    IntroMover introMover;
    // Start is called before the first frame update
    void Start()
    {
        introMover = new IntroMover();
    }

    private void FixedUpdate()
    {
        introMover.step(); //mover takes one step with set probabilty
        introMover.CheckEdges(); //mover is checked if they reach the edge of the screen

    }
}
public class IntroMover
{
    private Vector3 location;
    private Vector2 minimumPos, maximumPos;
    public GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public IntroMover()
    {
        findWindowLimits();
        location = Vector2.zero;

        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void step()
    {
        location = mover.transform.position;
        //Each frame choose a new Random number 0,1,2,3, 
        //If the number is equal to one of those values, take a step
        //Tendency to move down and right
        int choice = Random.Range(0, 4);
        if (choice == 0)
        {
            location.x++;

        }
        else if (choice == 1)
        {
            location.x++;
        }
        else if (choice == 3)
        {
            location.y--;
        }
        else
        {
            location.y--;
        }

        mover.transform.position += location * Time.deltaTime;
    }
    public void CheckEdges()
    {
        //used to bring mover back to the middle of the screen
        location = mover.transform.position;

        if (location.x > maximumPos.x)
        {
            location = Vector2.zero;
        }
        else if (location.x < minimumPos.x)
        {
            location = Vector2.zero;
        }
        if (location.y > maximumPos.y)
        {
            location = Vector2.zero;
        }
        else if (location.y < minimumPos.y)
        {
            location = Vector2.zero;
        }
        mover.transform.position = location;
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;

        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
