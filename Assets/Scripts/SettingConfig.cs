using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingConfig
{
    private string filename { get; set; }
    private float hueShift { get; set; }
    private float bloomintensity { get; set; }
    private float exposure { get; set; }
    private float saturation { get; set; }

    public SettingConfig(string file, float hue, float bloom, float exp, float sat)
    {
        filename = file;
        hueShift = hue;
        bloomintensity = bloom;
        exposure = exp;
        saturation = sat;
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}, {3}, {4}", filename, hueShift, bloomintensity, exposure, saturation);
    }

}
