using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Exercise3 : MonoBehaviour
{
    public GameObject vObject;
    public GameObject uObject;
    public GameObject wObject;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        vObject.transform.position = new Vector2(1, 5);

        Vector2 v = vObject.transform.position;
        Vector2 u = multiplyVector(v, 2); //u equals v multiplied by 2
        Vector2 w = subtractVectors(v, u)/3; //Divide the Vector2 w by 3. The Vector2 w equals v minus u.

        uObject.transform.position = u;
        wObject.transform.position = w;
    }

    static Vector2 multiplyVector(Vector2 toMultiply, float scaleFactor)
    {
        float x = toMultiply.x * scaleFactor;
        float y = toMultiply.y * scaleFactor;
        return new Vector2(x, y);
    }
    static Vector2 subtractVectors(Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x - vectorB.x;
        float newY = vectorA.y - vectorB.y;
        return new Vector2(newX, newY);
    }
}
