using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    public Image myImage;
    public Slider mySlider;
    public Color orgColor;
    public float Value
    {
        get => mySlider.value;
        set
        {
            mySlider.value = value;
            myImage.color = Color.Lerp(Color.red, orgColor, value);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        orgColor = myImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
