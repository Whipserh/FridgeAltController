using UnityEngine;

//drains power from the ciruit board lowering the charge
public class LEDComponent : CircuitComponent
{
    [SerializeField] private float chargeCost;//charge it takes from cost


    //switches the sprite if it has power and lowers the charge by an amount
    protected override void updateComponent()
    {
        
    }
}
