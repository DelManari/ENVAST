using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public class coordination
    {
        public float x;
        public float y;
        public float z;

        public coordination(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    List<coordination> list;
    List<GameObject> targets;
    int current = 0;
    public float speed = 1;
    public float WPradius = 0.5f;
    public GameObject point;
	
	//creating list from json
    public void LoadJson()
    {
        using (StreamReader r = new StreamReader("Assets/waypoints.json"))
        {
            string jsonstring = r.ReadToEnd();
            JObject obj = JObject.Parse(jsonstring);
            list = new List<coordination>();
            var jsonArray = JArray.Parse(obj["waypoints"].ToString());

            //iterate all values in array
            foreach (var jToken in jsonArray)
            {
                list.Add(new coordination(float.Parse(jToken["x"].ToString()),float.Parse(jToken["y"].ToString()),float.Parse(jToken["z"].ToString())));
                
            }
        }
    }
    void Update()
    {
        StartCoroutine("Move");

    }
    private IEnumerator Move()
    {
        if (Vector3.Distance(targets[current].transform.position, transform.position) < WPradius)
        {
            current++;
            if (current >= targets.ToArray().Length)
            {
                current = 0;
            }  
        }
		//wait 2 seconds
        yield return new WaitForSecondsRealtime(2f);
        transform.position = Vector3.MoveTowards(transform.position, targets[current].transform.position, Time.deltaTime * speed);
    }
    public void Start()
    {
        LoadJson();
        Instanceed();
        transform.position = targets[current].transform.position;
    }

    private void Instanceed()
    {
        targets = new List<GameObject>();

        foreach (coordination c in list)
        {
            GameObject instance = Instantiate(point, new Vector3(c.x, c.y, c.z), Quaternion.identity) as GameObject;
            targets.Add(instance);

        }
    }
}
