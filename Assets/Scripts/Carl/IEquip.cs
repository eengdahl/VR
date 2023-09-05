using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquip
{
    public void Equip();
    //Called when you want to equip a weapon to the hand

    public void UnEquip();
    //Called when you want to unequip a weapon and return it to the table/stand
    
    public void HighLight();
    //Called when you don't have anything equipped and want to highlight the weapon that you will equip
}
