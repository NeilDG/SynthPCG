using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BaseMapChanger : MonoBehaviour
{

    private float counter = 11;
    private MeshRenderer m_Renderer;
    private int index = 0;
    private int index2 = 0;
    private List<Texture> r_Textures = new List<Texture>();
    private List<Texture> a_Textures = new List<Texture>();
    private Texture currentText;
    private Texture a_currentText;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        //tex = Resources.Load<Texture>("roadtextures/Roads-test-2");

        var info = new DirectoryInfo("assets/resources/roadtextures");
        var fileinfo = info.GetFiles();
        foreach (var file in fileinfo)
        {
            string filestring = file.ToString();
            if (filestring.EndsWith(".png")){
                string[] tokens = filestring.Split('\\');
                string last = tokens[tokens.Length - 1];
                tokens = last.Split('.');
                last = tokens[0];
                Texture tex = Resources.Load<Texture>("roadtextures/" + last);
                r_Textures.Add(tex);
            }
            
        }

        var info1 = new DirectoryInfo("assets/resources/buildingtextures");
        var fileinfo1 = info1.GetFiles();
        foreach (var file in fileinfo1)
        {
            string filestring = file.ToString();
            if (filestring.EndsWith(".png"))
            {
                string[] tokens = filestring.Split('\\');
                string last = tokens[tokens.Length - 1];
                tokens = last.Split('.');
                last = tokens[0];
                Texture tex = Resources.Load<Texture>("buildingtextures/" + last);
                a_Textures.Add(tex);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Texture currentText = r_Textures[index];
        counter += Time.deltaTime;
        //print(counter);
        if (counter > 20)
        {
            counter = 0;
            m_Renderer.material.SetTexture("_BaseMap", currentText);
            index += 1;
            
            if (index >= r_Textures.Count)
            {
                index = 0;
            }
            currentText = r_Textures[index];

        } */

        counter += Time.deltaTime;
        //print(counter);
        if (counter > 10)
        {
            //print(r_Textures.Count);
            index = Random.Range(0, r_Textures.Count);
            index2 = Random.Range(0, a_Textures.Count);
            currentText = r_Textures[index];
            a_currentText = a_Textures[index2];
            counter = 0;

            var materials = m_Renderer.materials;
            foreach (var material in materials)
            {
                if (material.name.Contains("Roads"))
                {
                    material.SetTexture("_BaseMap", currentText);
                }
                if (material.name.Contains("Atlas-1"))
                {
                    material.SetTexture("_BaseMap", a_currentText);
                }
            }

        }
    }
}
