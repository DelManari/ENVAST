using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Duplicate : MonoBehaviour
{
    public InputField inf;
    public Button B;
    public GameObject sprit;
    public GameObject spawnPoint;
    int count;

    public void clickedButton()
    {
        count = int.Parse(inf.text.ToString());
        spawn2(count);
    }
    public void spawn2(int count)
    {
        var pos = spawnPoint.transform;
        float x = pos.position.x;
        float y = pos.position.y;

        for (int i = count; i > 0; i--)
        {
            y += 67.5f;
            spawn(i,x,y);
        }
    }
    public void spawn(int count ,float x,float y)
    {
        var pos = spawnPoint.transform;
        for (int i = 0; i < count; i++)
        {
            x += 67.5f;

            Instantiate(sprit,new Vector2(x,y), pos.rotation);
        }
    }
}
