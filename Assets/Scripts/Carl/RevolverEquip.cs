using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEngine;

public class RevolverEquip : MonoBehaviour, IEquip
{
    public GunType gunType; //Change this in the prefab to be the correct gun type
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void Equip()
    {
        //Code to attach the weapon to the player.
    }

    public void UnEquip()
    {
        //Code to detach the weapon from the player and return it to the table.
    }

    public void HighLight()
    {
        //Code to highlight the weapon when the player is targeting it when it's not equipped.
    }
}
