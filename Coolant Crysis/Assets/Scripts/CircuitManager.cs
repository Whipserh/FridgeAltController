using TMPro;
using UnityEngine;

public class outPacket
{
    public float dial1, dial2, slider1, slider2;
}

public class CircuitManager : MonoBehaviour
{
    public TextMeshProUGUI dial1TXT, dial2TXT, slider1TXT, slider2TXT, leftTxt, rightTxt;
    public float dial1, dial2, slider1, slider2;
    public bool left, right;
    public float fanPower, fanAngle, thermometerAngle;


    
    void Update()
    {
        sendResult();
        displayControllerInfo();
    }
    
    public void updateValues(float dialA, float dialB, float sliderA, float sliderB)
    {
        dial1 = dialA;
        dial2 = dialB;
        slider1 = sliderA;
        slider2 = sliderB;
    }

    public void displayControllerInfo()
    {
        dial1TXT.text = "Dial 1: " + dial1;
        dial2TXT.text = "Dial 2: " + dial2;
        slider1TXT.text = "Slider 1: " + slider1;
        slider2TXT.text = "Slider 2: " + slider2;
        leftTxt.text = "Left Button: " + left;
        rightTxt.text = "Right Button: " + right;
    }

    //talks to controller on what output to send
    public outPacket sendResult()
    {
        outPacket a = new outPacket();
        a.dial1 = dial1;
        a.dial2 = dial2;
        a.slider1 = slider1;
        a.slider2 = slider2;
        return a;
    }
}
