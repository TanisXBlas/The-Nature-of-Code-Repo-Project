using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionExercise5 : MonoBehaviour
{
    private NewWalkerIntro5 newWalker5;

    // Start is called before the first frame update
    void Start()
    {
        // Create the walker
        newWalker5 = new NewWalkerIntro5();
    }

    // Update is called once per frame
    void Update()
    {
        //Have the walker move
        newWalker5.step();
        newWalker5.CheckEdges();
    }
}

public class NewWalkerIntro5
{
    Vector2 location;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    //Perlin
    float heightScale = 2;
    float widthScale = 1;

    // Gives the class a GameObject to draw on the screen
    public GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public NewWalkerIntro5()
    {
        findWindowLimits();
        location = Vector2.zero;

        //Gets material renderer of mover
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void step()
    {
        widthScale += Mathf.PerlinNoise(Time.time * 1, 0.0f);
        heightScale += Mathf.PerlinNoise(Time.time * .5f, 0.0f);

        float height = 0f * heightScale;
        float width = .02f * widthScale;
        Vector3 pos = mover.transform.position;
        pos.y = height;
        pos.x = width;
        mover.transform.position = pos;
    }

    public void CheckEdges()
    {
        location = mover.transform.position;
        if ((location.x > maximumPos.x) || (location.x < minimumPos.x))
        {
            reset();
        }

        if ((location.y > maximumPos.y) || (location.y < minimumPos.y))
        {
            reset();
        }
        mover.transform.position = location;
    }

    void reset()
    {
        location = Vector2.zero;
        heightScale = 2;
        widthScale = 1;
    }

    private void findWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}

