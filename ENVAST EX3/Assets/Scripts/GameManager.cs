using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject btn;
    List<tab> listTabs;
    List<content> listContent;
    public GameObject msgCanvas;
    public class tab
    {
        
        public string id;
        public string title;
        public int index;
        public bool enabled;

        public tab(string id, string title, int index, bool enabled)
        {
            this.id = id;
            this.title = title;
            this.index = index;
            this.enabled = enabled;
        }
    }
    public class colorTY
    {
        public int red;
        public int green;
        public int blue;
        public colorTY(int red, int green, int blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }
    }
    public class content
    {
        public string name;
        public colorTY color;
        public string tabId;
        public string message;

        public content(string name, colorTY color, string tabId, string message)
        {
            this.name = name;
            this.color = color;
            this.tabId = tabId;
            this.message = message;
        }
    }
    public void LoadJson()
    {
        using (StreamReader r = new StreamReader("Assets/tabs.json"))
        {
            string jsonstring = r.ReadToEnd();
            JObject obj = JObject.Parse(jsonstring);
            listTabs = new List<tab>();
            var jsonArray = JArray.Parse(obj["tabs"].ToString());

            //iterate all values in array
            foreach (var jToken in jsonArray)
            {
                listTabs.Add(new tab(jToken["id"].ToString(), jToken["title"].ToString(), int.Parse(jToken["index"].ToString()), bool.Parse(jToken["enabled"].ToString())));
            }

            listContent = new List<content>();
            var jsonArraycontent = JArray.Parse(obj["content"].ToString());
            //iterate all values in array
            foreach (var jToken in jsonArraycontent)
            {
                listContent.Add(new content(jToken["name"].ToString(), new colorTY(int.Parse(jToken["color"]["red"].ToString()), int.Parse(jToken["color"]["green"].ToString()), int.Parse(jToken["color"]["blue"].ToString())), jToken["tabId"].ToString(), jToken["message"].ToString()));
            }

        }
    }

    public void CreateButton(string text, bool enabled, int n)
    {

        Canvas canvasComponent = GameObject.Find("Canvas1").GetComponent<Canvas>();
        GameObject myButtonPrefab = btn;


        //Instantiate button
        GameObject actualButton = GameObject.Instantiate(myButtonPrefab, canvasComponent.gameObject.transform) as GameObject;
        actualButton.transform.SetParent(canvasComponent.gameObject.transform, false);

        //n = number of buttons
        Vector3 v = new Vector3(actualButton.transform.position.x + (n * 200), actualButton.transform.position.y, actualButton.transform.position.z);
        actualButton.transform.position = v;

        //setting button text
        Text txt = actualButton.GetComponentInChildren<Text>();
        txt.color = Color.white;
        txt.text = text;

        //enabled
        actualButton.GetComponent<Button>().interactable = enabled;

        //onclick methode
        actualButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { generateBlocks(txt.text); });

    }


    public void generateBlock(string text, colorTY color,string message, int n)
    {

        Canvas canvasComponent = GameObject.Find("Canvas1").GetComponent<Canvas>();
        GameObject myButtonPrefab = btn;


        //Instantiate button
        GameObject actualButton = GameObject.Instantiate(myButtonPrefab, canvasComponent.gameObject.transform) as GameObject;
        actualButton.transform.SetParent(canvasComponent.gameObject.transform, false);

        //n = number of buttons
        Vector3 v = new Vector3(actualButton.transform.position.x, actualButton.transform.position.y + (n * -50), actualButton.transform.position.z);
        actualButton.transform.position = v;

        //setting button text
        Text txt = actualButton.GetComponentInChildren<Text>();
        txt.color = Color.white;
        txt.text = text;

        //setting spite color
        Color NewColor = new Color(color.red, color.green, color.blue);
        Image img = actualButton.GetComponent<Image>();
        img.color = NewColor;

        actualButton.tag = "Player";

        //onclick methode
        actualButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { showMessage(message); });

    }
    //show message
    public void showMessage(string msg)
    {
        if (!msgCanvas.activeInHierarchy)
        {
            msgCanvas.SetActive(true);
            GameObject msgd = msgCanvas.transform.Find("msgtxt").gameObject;
            msgd.GetComponent<Text>().text = msg;
        }
        else
        {
            msgCanvas.SetActive(false);
        }

    }
    public void removeButtonbyTag()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
    }
    public void generateBlocks(string titre)
    {
        int i = 1;
        removeButtonbyTag();
        //generate blocks
        string id = listTabs.Find(item => item.title == titre).id;
        foreach (content c in listContent)
        {
            if (c.tabId == id)
            {
                generateBlock(c.name, c.color,c.message, i);
                i++;
            }
        }
    }
    public void Start()
    {
        LoadJson();
        // sort list Tabls by index
        listTabs.Sort((x, y) =>
            x.index.CompareTo(y.index));

        //create tabs
        int i = 0;
        foreach (tab t in listTabs)
        {
            CreateButton(t.title, t.enabled, i);
            i++;
        }

    }
}
