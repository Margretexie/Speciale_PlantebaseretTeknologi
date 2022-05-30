using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Average : MonoBehaviour
{
    private Queue<float> samples = new Queue<float>();
    private int windowSize = 6;
    private float sampleAccumulator;
    public float MovingAverage { get; private set; }
    
    public void ComputeAverage(float newSample)
    {
        
        
        sampleAccumulator += newSample;
        samples.Enqueue(newSample);

        if (samples.Count > windowSize)
        {
            sampleAccumulator -= samples.Dequeue();
        }

        MovingAverage = sampleAccumulator / samples.Count;
        //Debug.Log(MovingAverage);
    }

     void Update()
    {
        ComputeAverage(GetComponent<Communication>().EMG);
    }

}
