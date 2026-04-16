using UnityEngine;
using UnityEngine.UI;
/**
 * Description:
 * Capacitors take charge and store an certain amount before discharging it through the rest of the its connected to
 */
public class Capacitor : CircuitComponent
{
    [Header("Capacitor")]
    [SerializeField] private Slider slider;
    [SerializeField] private float powerCapacity, chargeSpeed, maxCapacity;

     void Start()
    {
        slider.value = 0;
    }

    protected override void updateComponent()
    {
        //update the capacity
        if(slider.value != powerCapacity) slider.value = powerCapacity;

        if(hasPower)
        {
            powerCapacity += chargeSpeed * Time.deltaTime;
            if(maxCapacity <= powerCapacity)
            {
                overPower = true;
                base.updatePower();//send the super charge pulse through the system
                overPower = false;
                powerCapacity = 0;
            }
        }
    }
 
}
