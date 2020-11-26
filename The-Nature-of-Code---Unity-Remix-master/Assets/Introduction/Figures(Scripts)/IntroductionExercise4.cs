using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionExercise4 : MonoBehaviour
{
    introMover4 walker4;
    void Start()
    {
        walker4 = new introMover4();
    }
    void FixedUpdate()
    {
        walker4.step();
        walker4.CheckEdges();
    }
}


public class introMover4
{
    private Vector3 location;

    private Vector2 minimumPos, maximumPos;
    
    public GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public introMover4()
    {
        findWindowLimits();
        location = Vector2.zero;
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void step()
    {
        location = mover.transform.position;

        float stepsizeX = montecarlo();
        float stepsizeY = montecarlo();

        float stepx = Random.Range(-stepsizeX, stepsizeX);
        float stepy = Random.Range(-stepsizeY, stepsizeY);

        location.x += stepx;
        location.y += stepy;

        mover.transform.position += location * Time.deltaTime;
    }

    float montecarlo()
    {
        while (true)
        {
            float r1 = Random.Range(0, 10);
            float probability = r1*r1;
            float r2 = Random.Range(0, 10);

            if (r2 < probability)
            {
                return r1;
            }

        }
    }

    public void CheckEdges()
    {
        //Sets mover back to middle of the screen

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
