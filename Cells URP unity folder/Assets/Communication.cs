using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Communication : MonoBehaviour
{
    SerialPort data_stream = new SerialPort("COM4", 4800);
    public string receivedstring;
    public string[] datas;
    public float EMG;
    public float MAEMG;
    public float speed;
    public int tempdata;
    public float temp;
    public int moist;
    public GameObject plane;

    private string msg = "h";
    private float waitTime = 5.0f;
    private float timer = 0.0f;

    private Material material;
    public static float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
    void Start()
    {
        data_stream.Open();
        print("data stream opened");
        material = GetComponent<MeshRenderer>().sharedMaterial;
        plane = GameObject.Find("Plane");
    }

    void Update()
    {
        receivedstring = data_stream.ReadLine();

        string[] datas = receivedstring.Split(',');
        EMG = float.Parse(datas[0]);
        tempdata = int.Parse(datas[1]);
        temp = (float)tempdata / 10;
        moist = int.Parse(datas[2]);
        //MAEMG = GetComponent<Average>().MovingAverage;


        timer += Time.deltaTime;

        if (timer > waitTime)       //only update material after a few seconds, otherwise the visual glitches 
        {
            //print("material change");
            material.SetFloat("_speed", temp);           //plant cells move faster in higher temp, and slower in low temp
            material.SetFloat("_saturation", moist);    //dryness of the soil = dryness in color
            timer = timer - waitTime;
        }

        if (plane.GetComponent<RippleTrigger>().Difference > 0.2f || plane.GetComponent<RippleTrigger>().Difference < -0.2f ||  Input.GetKeyDown("r"))
        {
            data_stream.Write(msg);
            print(msg);
        }

    }


        public void OnApplicationQuit()
    {

        if (data_stream != null)
        {
            if (data_stream.IsOpen)
            {
                print("closing serial port");
                data_stream.Close();
            }

            data_stream = null;
        }

    }


}
