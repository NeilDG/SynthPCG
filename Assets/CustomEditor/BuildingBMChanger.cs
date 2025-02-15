using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildingBMChanger : MonoBehaviour
{

    private float counter = 11;
    private MeshRenderer m_Renderer;
    private int index = 0;
    private List<Texture> r_Textures = new List<Texture>();
    private Texture currentText;
    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        //tex = Resources.Load<Texture>("roadtextures/Roads-test-2");

        var info = new DirectoryInfo("assets/resources/buildingtextures");
        var fileinfo = info.GetFiles();
        foreach (var file in fileinfo)
        {
            string filestring = file.ToString();
            if (filestring.EndsWith(".png"))
            {
                string[] tokens = filestring.Split('\\');
                string last = tokens[tokens.Length - 1];
                tokens = last.Split('.');
                last = tokens[0];
                Texture tex = Resources.Load<Texture>("buildingtextures/" + last);
                r_Textures.Add(tex);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        //print(counter);
        if (counter >  10)
        {
            index = Random.Range(0, r_Textures.Count);
            currentText = r_Textures[index];
            counter = 0;

            var materials = m_Renderer.materials;
            foreach(var material in materials)
            {
                if (material.name.Contains("Atlas-1")){
                        material.SetTexture("_BaseMap", currentText);
                }
            }

        }
    }
}
