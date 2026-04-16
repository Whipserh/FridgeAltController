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
        slider.maxValue = maxCapacity;
        slider.value = 0;
    }

    protected override void updateComponent()
    {
        //update the slider UI
        if(slider.value != powerCapacity) slider.value = powerCapacity;

        if(hasPower)
        {
            //increase the amount of charge being held on the capacitor
            powerCapacity = Mathf.Clamp((chargeSpeed * Time.deltaTime)+powerCapacity, 0, maxCapacity);

            //check to see if we hit the max charge
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
