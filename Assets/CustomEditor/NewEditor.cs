using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using SVS;

public class NewEditor : EditorWindow
{
    private CityGenerator cityGenerator;

    //private bool generateLightmapUVs = false;
    private bool intenseTraffic = false;
    // Start is called before the first frame update
    private LSystemGenerator lsystem;


    [MenuItem("Window/Custom City Generator")]
    static void Init()
    {
        
        NewEditor window = (NewEditor)EditorWindow.GetWindow(typeof(NewEditor));
        window.Show();
    }

    void OnGUI()
    {
        //Window Label
        GUILayout.Space(10);
        GUILayout.Label("Custom City Generator", EditorStyles.boldLabel);
        
        //Load Assets
        EditorGUILayout.BeginHorizontal();
        if (!cityGenerator)
            cityGenerator = AssetDatabase.LoadAssetAtPath("Assets/Fantastic City Generator/Generate.prefab", (typeof(CityGenerator))) as CityGenerator;
        LoadAssets();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);


        //Generate Streets
        GUILayout.BeginVertical("box");
        GUILayout.Space(5);
        GUILayout.Label(new GUIContent("Generate Streets", "Make City"));
        GUILayout.Space(5);

        //Fix generate function to incorporate LSystem
        GUILayout.BeginHorizontal("box");
        string test;
        if (!lsystem)
            lsystem = AssetDatabase.LoadAssetAtPath("Assets/CustomEditor/lsystem.prefab", (typeof(LSystemGenerator))) as LSystemGenerator;
        

        // To change 
       //Rule[] rules = new Rule[1];
        //rules[0] = getBaseRule();
        //rules[1] = getBaseRule2();

        if (GUILayout.Button("Small"))
        {
            //DefineLSystemGenerator(rules, 1, "[F]--F", true, 0.3f);
            test = GenerateSequence(0);
            cityGenerator.GenerateStreetBasedOnSequence(test);
            //Debug.Log(test);
        }
        if (GUILayout.Button("Medium"))
        {
            //DefineLSystemGenerator(rules, 2, "[F]--F", true, 0.3f);
            test = GenerateSequence(1);
            cityGenerator.GenerateStreetBasedOnSequence(test);
            //Debug.Log(test);
        }
        if (GUILayout.Button("Large"))
        {
            //DefineLSystemGenerator(rules, 3, "[F]--F", true, 0.3f);
            test = GenerateSequence(2);
            cityGenerator.GenerateStreetBasedOnSequence(test);
            //Debug.Log(test);
        }
        GUILayout.Space(5);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.Space(10);

        //Generate Buildings
        GUILayout.BeginVertical("box");
        GUILayout.Space(5);
        GUILayout.Label(new GUIContent("Buildings", "Make or Clear Buildings"));
        GUILayout.Space(5);

        GUILayout.BeginHorizontal("box");
        GUILayout.Space(5);

        if (GUILayout.Button("Generate Buildings"))
        {
            if (!GameObject.Find("Marcador")) return;
            cityGenerator.GenerateAllBuildings();
        }

        if (GUILayout.Button("Clear Buildings"))
        {
            if (!GameObject.Find("Marcador")) return;
            cityGenerator.DestroyBuildings();
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(10);

        GUILayout.BeginVertical("box");
        GUILayout.Space(5);
        GUILayout.Label(new GUIContent("Traffic System", "Make or Clear Traffic System"));
        GUILayout.Space(5);
        GUILayout.BeginHorizontal("box");
        GUILayout.Space(5);

        //Generate Cars
        if (GUILayout.Button("Add Traffic System"))
        {

            if (EditorApplication.isPlaying)
            {
                Debug.Log("Not allowed in play mode");
                return;
            }
            //Debug.Log(intenseTraffic);
            AddVehicles(intenseTraffic);
        }

        //Remove Cars
        if (GUILayout.Button("Remove Traffic System"))
        {
            DestroyImmediate(GameObject.Find("CarContainer"));
        }
        GUILayout.EndHorizontal();

        intenseTraffic = GUILayout.Toggle(intenseTraffic, "Intense Traffic", GUILayout.Width(240));
        GUILayout.EndVertical();
        GUILayout.Space(10);
    }


    public void LoadAssets()
    {

        string[] s;

        //BB - Street buildings in suburban areas (not in the corner)
        s = System.IO.Directory.GetFiles("Assets/Fantastic City Generator/Buildings/Prefabs/BB", "*.prefab");
        if (cityGenerator.BB.Length != s.Length)
            cityGenerator.BB = LoadAssets_sub(s);

        //BC - Down Town Buildings(Not in the corner)
        s = System.IO.Directory.GetFiles("Assets/Fantastic City Generator/Buildings/Prefabs/BC", "*.prefab");
        if (cityGenerator.BC.Length != s.Length)
            cityGenerator.BC = LoadAssets_sub(s);

        //BK - Buildings that occupy an entire block
        s = System.IO.Directory.GetFiles("Assets/Fantastic City Generator/Buildings/Prefabs/BK", "*.prefab");
        if (cityGenerator.BK.Length != s.Length)
            cityGenerator.BK = LoadAssets_sub(s);

        //BR - Residential buildings in suburban areas (not in the corner)
        s = System.IO.Directory.GetFiles("Assets/Fantastic City Generator/Buildings/Prefabs/BR", "*.prefab");
        if (cityGenerator.BR.Length != s.Length)
            cityGenerator.BR = LoadAssets_sub(s);

        //DC - Corner buildings that occupy both sides of the block
        s = System.IO.Directory.GetFiles("Assets/Fantastic City Generator/Buildings/Prefabs/DC", "*.prefab");
        if (cityGenerator.DC.Length != s.Length)
            cityGenerator.DC = LoadAssets_sub(s);

        //EB - Corner buildings in suburban areas
        s = System.IO.Directory.GetFiles("Assets/Fantastic City Generator/Buildings/Prefabs/EB", "*.prefab");
        if (cityGenerator.EB.Length != s.Length)
            cityGenerator.EB = LoadAssets_sub(s);

        //EC - Down Town Corner Buildings 
        s = System.IO.Directory.GetFiles("Assets/Fantastic City Generator/Buildings/Prefabs/EC", "*.prefab");
        if (cityGenerator.EC.Length != s.Length)
            cityGenerator.EC = LoadAssets_sub(s);

        //MB - Buildings that occupy both sides of the block
        s = System.IO.Directory.GetFiles("Assets/Fantastic City Generator/Buildings/Prefabs/MB", "*.prefab");
        if (cityGenerator.MB.Length != s.Length)
            cityGenerator.MB = LoadAssets_sub(s);

        //SB - Large buildings that occupy larger blocks
        s = System.IO.Directory.GetFiles("Assets/Fantastic City Generator/Buildings/Prefabs/SB", "*.prefab");
        if (cityGenerator.SB.Length != s.Length)
            cityGenerator.SB = LoadAssets_sub(s);


    }

    private GameObject[] LoadAssets_sub(string[] s)
    {

        int i = s.Length;
        GameObject[] g = new GameObject[i];

        for (int h = 0; h < i; h++)
            g[h] = AssetDatabase.LoadAssetAtPath(s[h], typeof(GameObject)) as GameObject;

        return g;

    }

    private void AddVehicles(bool additionalCars = false)
    {


        if (GameObject.Find("RoadMark") && GameObject.Find("RoadMarkRev"))
            InverseCarDirection(true);

        TrafficSystem trafficSystem = AssetDatabase.LoadAssetAtPath("Assets/Fantastic City Generator/Traffic System/Traffic System.prefab", (typeof(TrafficSystem))) as TrafficSystem;
        trafficSystem.LoadCars(additionalCars);
    }

    private void InverseCarDirection(bool actualside)
    {


        GameObject[] roadMark = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name.Equals("Road-Mark")).ToArray();
        for (int i = 0; i < roadMark.Length; i++)
            roadMark[i].transform.Find("RoadMark").gameObject.SetActive(actualside);

        roadMark = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name.Equals("Road-Mark-Rev")).ToArray();
        for (int i = 0; i < roadMark.Length; i++)
            roadMark[i].transform.Find("RoadMarkRev").gameObject.SetActive(!actualside);


    }

    private string GenerateSequence(int size)
    {
        string seq = null;
        switch (size){
            case 0: 
                lsystem.iterationLimit = 1;
                seq = lsystem.GenerateSentence();
                Debug.Log(seq);
                break;
            case 1:
                lsystem.iterationLimit = 2;
                seq = lsystem.GenerateSentence();
                break;
            case 2:
                lsystem.iterationLimit = 3;
                seq = lsystem.GenerateSentence();
                break;
            default:
                break;

        }

        return seq;
    }

    private void DefineLSystemGenerator(Rule[] rules, int iterationLimit, string root, bool ignore, float chance)
    {
        lsystem.rules = new Rule[rules.Length];
        lsystem.rules = rules;
        lsystem.iterationLimit = iterationLimit;
        lsystem.rootSentence = root;
        lsystem.chanceToIgnoreRule = chance;
        lsystem.randomIgnoreRuleModifier = ignore;
        //lsystem.
    }

    private Rule getBaseRule()
    {
        Rule rule = (Rule)CreateInstance("Rule");
        rule.letter = "F";
        rule.results = new string[3];
        rule.results[0] = "F[+F][-F]";
        //rule.results[1] = "[+FF]";
        //rule.results[2] = "[+FF]";
        //rule.results[0] = "[+F][-F]";
        //rule.results[1] = "[+F]F[-F]";
        //rule.results[2] = "[-F]F[+F]";
        rule.randomResult = true;

        return rule;
    }

    private Rule getBaseRule2()
    {
        Rule rule = (Rule)CreateInstance("Rule");
        rule.letter = "B";
        rule.results = new string[3];
        rule.results[0] = "[+F][-F]";
        rule.results[1] = "[+F]F[-F]";
        rule.results[2] = "[-F]F[+F]";
        rule.randomResult = true;

        return rule;
    }
}
