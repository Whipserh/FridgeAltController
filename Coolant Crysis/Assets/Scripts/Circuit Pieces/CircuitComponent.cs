using System.Security.Policy;
using UnityEngine;

public class CircuitComponent : MonoBehaviour
{
    [Header("Base Component")]
    public bool hasPower, overPower;
    private float timer;
    protected float overloadCoolDown = 3;
    public CircuitComponent[] nextComponents;
    public float voltage;

    
    void Start()
    {
        hasPower = false;
        timer = 0;
    }

    //update 
    void Update()
    {
        //cool down the component, if the component reaches the cooldown it resets
        if (overPower) timer += Time.deltaTime;
        else { timer = 0; turnOff(); }

        //do update the component each turn
        updateComponent();
    }

    //updates the component in overload as well
    protected virtual void updateComponent()
    {
        if(hasPower) updatePower();
    }

    #region turnOn/Off fucntions
    //turn on component and update the power to the next components
    public void turnOn()
    {
        hasPower = true;
    }

    //turn off components
    public void turnOff()
    {
        hasPower = false;
        overPower = false;
        updatePower();
    }
    #endregion

    //update the power of the next connected Components 
    public void updatePower()
    {
        foreach (CircuitComponent c in nextComponents)
        {
            if (overPower) c.overLoadComponent(); 
            else if (hasPower)c.turnOn(); 
            else c.turnOff();
        }
    }


    //turn the overload component on
    public void overLoadComponent()
    {
        overPower = true;
        updatePower();
    }
}
