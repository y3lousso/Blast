using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour {

    public int bpm = 100;
    public string outputPath = "MusicsData/file.txt";

    List<bool> list = new List<bool>();

    private bool isRunning = false;

    public Image image;

    public void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        image.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        //Start
        if (!isRunning)
        {
            if (Input.GetKeyDown("down"))
            {
                // offset to center the interval around the cube -> easier to generate a good file
                InvokeRepeating("NextFrame", 30f / bpm, 60f / bpm);
                list.Add(true);
                image.color = Color.green;
                isRunning = true;
            }
        }
        else
        {
            if (Input.GetKeyDown("down"))
            {
                list[list.Count - 1] = true;
                image.color = Color.green;
            }
            if (Input.GetKeyDown("up"))
            {
                image.color = Color.black;
                isRunning = false;
                CancelInvoke("NextFrame");
                WriteToFile();
            }
        }
    }

    private void NextFrame()
    {
        list.Add(false);
        image.color = Color.red;
    }

    private void WriteToFile()
    {
        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }
        FileStream fs = File.Create(outputPath);
        fs.Close();

        using (var tw = new StreamWriter(outputPath, true))
        {
            foreach (bool b in list)
            {
                tw.WriteLine(b);
            }
        }
        Application.Quit();
    }
}
