using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemCh1 : MonoBehaviour
{
    private butterfly butterflyGO;
    // Start is called before the first frame update
    void Start()
    {
        butterflyGO = new butterfly();
    }

    // Update is called once per frame
    void Update()
    {
        butterflyGO.Update();
    }
}
public class butterfly
{
    Vector3 location;
    //Perlin
    float heightScale = 5;
    float widthScale = 3;
    float lengthScale = 10;
    private GameObject butterflyGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public butterfly()
    {
        location = Vector3.zero;
    }
    public void Update()
    {
        widthScale += .02f;
        lengthScale += .002f;
        heightScale += .001f;

        float height = heightScale * Mathf.PerlinNoise(Time.time * .5f, 0.0f);
        float width = widthScale * Mathf.PerlinNoise(Time.time * 1, 0.0f);
        float length = lengthScale * Mathf.PerlinNoise(Time.time * 1, 0.0f);
        Vector3 pos = butterflyGO.transform.position;
        pos.y = height;
        pos.x = width;
        pos.z = length;
        butterflyGO.transform.position = pos;
    }
}
