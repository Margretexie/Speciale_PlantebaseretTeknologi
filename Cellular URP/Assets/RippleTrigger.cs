using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleTrigger : MonoBehaviour
{
    private Queue<float> samples = new Queue<float>();
    private int windowSize = 2;
    private float sampleAccumulator;
    private Material material;
    public GameObject plane;
    public float Difference { get; private set; }

    public float rippleAmount = 1f;
    float rippleTarget = 1f;
    public float rippleSpeed;
    public bool randomFocalPoint;

    void Start()
    {
        
        material = GetComponent<MeshRenderer>().material;
        plane = GameObject.Find("Plane");
    }
    public void ComputeDifference(float newSample)
    {


        sampleAccumulator += newSample;
        samples.Enqueue(newSample);

        if (samples.Count > windowSize)
        {
            sampleAccumulator -= samples.Dequeue();
        }

        Difference = (newSample - samples.Peek()) / newSample;
        
        

    }

    void Update()
    {
        ComputeDifference(plane.GetComponent<Communication>().EMG);

       /* if(Input.GetKeyDown("r"))
        {
            triggerRipple();
        }*/

        rippleAmount = Mathf.Lerp(rippleAmount, rippleTarget, Time.deltaTime * rippleSpeed);



        material.SetFloat("_Ripple", rippleAmount);
        
        if (Difference > 0.2f || Difference < -0.2f || Input.GetKeyDown("r"))
        {
           
            //produce ripple effect
            print("Triggered");
            triggerRipple();
            
        }

    }

    public void triggerRipple()
    {
        if (rippleAmount > 0.8f)
        {
            if (randomFocalPoint)
            {
                material.SetVector("_FocalPoint", new Vector2(Random.value, Random.value));
            }
            rippleAmount = 0f;
            print("Ripple reset");
        }
    }
}
