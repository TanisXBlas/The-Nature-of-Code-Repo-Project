using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionExercise3 : MonoBehaviour
{
    public Material transparencyPrefab;
    void FixedUpdate()
    {
        float num = Random.Range(Random.Range(-30, 30), Random.Range(-30, 30));
        float sd = 20;
        float mean = 0;

        float x = sd * num + mean;

        //This creates a sphere GameObject
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer r = sphere.GetComponent<Renderer>();
        sphere.GetComponent<SphereCollider>().enabled = false;

        //To set a color
        Color32 newColor = new Color(0.3f, 0.4f, 0.6f, 0.3f);
        r.material = transparencyPrefab;
        r.material.color = newColor;

        Object.Destroy(sphere.GetComponent<SphereCollider>());

        sphere.transform.position = new Vector3(x, 0F, 0F) * Time.deltaTime;
    }
}
