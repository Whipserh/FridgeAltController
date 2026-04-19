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
    private float fanPower, fanAngle, thermometerAngle;
    public SerialPortManager serialPortManager;
    public float newTA=0, newFA=0, newFP=0;
    void Update()
    {
        thermometerAngle = newTA;
        fanAngle = newFA;
        fanPower = newFP;
        sendResult();
        displayControllerInfo();
    }
    

    //DONE
    public void updateValues(string command, string value)
    {
        Debug.Log("I'm reading inpupt");
        switch (command) {
            case "POT1":
                dial1 = int.Parse(value);
                break;
            case "POT2":
                dial2 = int.Parse(value);
                break;
            case "SLIDER1":
                slider1 = int.Parse(value);
                break;
            case "SLIDER2":
                slider2 = int.Parse(value);
                break;
            case "LEFT":
                left = value.Equals("UP");
                break;
            case "RIGHT":
                right = value.Equals("UP");
                break;
        }

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
    public void sendResult()
    {
        if (thermometerAngle!=newTA || fanAngle!=newFA || fanPower != newFP)
            serialPortManager.sendMessage(thermometerAngle+","+fanAngle+","+fanPower);
        thermometerAngle = newTA;
        fanAngle = newFA;
        fanPower = newFP;
    }
}
