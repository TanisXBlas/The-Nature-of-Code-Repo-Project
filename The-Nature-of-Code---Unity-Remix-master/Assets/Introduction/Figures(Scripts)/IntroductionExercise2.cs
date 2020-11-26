using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionExercise2 : MonoBehaviour
{
    private NewWalkerIntro3 walkeri3;
    // Start is called before the first frame update
    void Start()
    {
        walkeri3 = new NewWalkerIntro3();
    }

    // Update is called once per frame
    void Update()
    {
        walkeri3.step();
        walkeri3.draw();
    }
}
public class NewWalkerIntro3
{
    public int x;
    public int y;
    float num;
    GameObject newWalkerGO;
    List<GameObject> newWalkers = new List<GameObject>();

    // Start is called before the first frame update
    public NewWalkerIntro3()
    {
        newWalkerGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);     //We need to create a new material for WebGL
        Renderer r = newWalkerGO.GetComponent<Renderer>();
        newWalkerGO.GetComponent<SphereCollider>().enabled = false;
        UnityEngine.Object.Destroy(newWalkerGO.GetComponent<SphereCollider>());
        r.material = new Material(Shader.Find("Diffuse"));

        x = 0;
        y = 0;
    }


    public void step()
    {
        num = UnityEngine.Random.Range(0f, 1f);
        Vector3 worldPosition;
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = Camera.main.nearClipPlane;

        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //Each frame choose a new Random number 0-1;
        //If the number is less than the the float take a step
        //Each step is in a different direction; includes to follow mouse cursor
        if (num < 0.1F)
        {
            x++;
        }
        else if (num < 0.3F)
        {
            x--;
        }
        else if (num < .45F)
        {
            y++;
        }
        else if (num < .5F)
        {
            y--;
        }
        else //For the mover to follow the mouse
        {
            if (worldPosition.x > x && worldPosition.y < y)
            {
                x++;
                y--;
            }
            else if (worldPosition.x > x && worldPosition.y > y)
            {
                x++;
                y++;
            }
            else if (worldPosition.x < x && worldPosition.y < y)
            {
                x--;
                y--;
            }
            else if (worldPosition.x < x && worldPosition.y > y)
            {
                x--;
                y++;
            }
        }
        newWalkerGO.transform.position = new Vector3(x, y, 0F);
    }

    public void draw() //Draws the location of the mover object each step
    {

        if (newWalkers.Count <= 60)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Renderer r = sphere.GetComponent<Renderer>();
            sphere.GetComponent<SphereCollider>().enabled = false;
            UnityEngine.Object.Destroy(sphere.GetComponent<SphereCollider>());
            r.material = new Material(Shader.Find("Diffuse"));
            //This sets our ink "sphere game objects" at the position of the Walker GameObject (walkerGO) at the current frame
            //to draw the path
            sphere.transform.position = new Vector3(newWalkerGO.transform.position.x, newWalkerGO.transform.position.y, 0F);
            newWalkers.Add(sphere);
        }
        else if (newWalkers.Count <= 1)
        {
            foreach (GameObject walker in newWalkers)
            {
                GameObject.Destroy(walker);
            }
        }
    }
}
