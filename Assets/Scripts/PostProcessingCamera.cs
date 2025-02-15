using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using System.IO;

public class PostProcessingCamera : MonoBehaviour 
{ 

    public GameObject postprocessing;
    private GameObject mainCamera;
    private float counter;

    // Settings for post procesing
    private float bloomIntensity;
    private float hueShift;
    private float saturation;
    private float exposure;
    private int file_counter;
    private int data_count = 3000;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = gameObject;
        counter = 0;
        file_counter = 0;

        StreamWriter writer = new StreamWriter(Application.dataPath + "/dataset/labels.txt", true);
        string labels = "filename, hueshift, bloomintensity, exposure, saturation";
        writer.WriteLine(labels);
        writer.Close();

    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > 3)
        {
            counter = 0;
            changeSettings();
            StartCoroutine(takeScreenshot());
            file_counter++;
            saveSettings();
            
        }
        if (file_counter >= data_count)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    private void changeSettings()
    {
        
        UnityEngine.Rendering.VolumeProfile volumeProfile = postprocessing.GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        
        UnityEngine.Rendering.Universal.Bloom bloom;
        UnityEngine.Rendering.Universal.ColorAdjustments adjustments;
        if (!volumeProfile.TryGet(out bloom)) throw new System.NullReferenceException(nameof(bloom));
        if (!volumeProfile.TryGet(out adjustments)) throw new System.NullReferenceException(nameof(adjustments));

        bloomIntensity = GetRandomNumber(0.0, 25.0);
        exposure = GetRandomNumber(-2.0, 3.0);
        hueShift = GetRandomNumber(-180.0, 180.0);
        saturation = GetRandomNumber(-100.0, 100.0);

        bloom.intensity.Override(bloomIntensity);
        adjustments.postExposure.Override(exposure);
        adjustments.hueShift.Override(hueShift);
        adjustments.saturation.Override(saturation);

        /*
        Debug.Log(bloomIntensity);
        Debug.Log(exposure);
        Debug.Log(hueShift);
        Debug.Log(saturation);

        
        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

        vignette.intensity.Override(0.5f);*/
    }

    private void saveSettings()
    {
        string filename = "data_" + file_counter + ".png";
        SettingConfig newConfig = new SettingConfig(filename, hueShift, bloomIntensity, exposure, saturation);

        StreamWriter writer = new StreamWriter(Application.dataPath + "/dataset/labels.txt", true);
        writer.WriteLine(newConfig.ToString());
        writer.Close();
    }

    private IEnumerator takeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        string filename = "data_" + file_counter + ".png";
        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/dataset/" + filename, byteArray);
    }

    private float GetRandomNumber(double minimum, double maximum)
    {
        System.Random random = new System.Random();
        return (float) (random.NextDouble() * (maximum - minimum) + minimum);
    }
}
