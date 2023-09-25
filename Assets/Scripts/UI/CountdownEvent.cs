using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownEvent : MonoBehaviour
{
    public void CallStartGame() //Used by the countdown animation! Dont Remove!
    {
        GameObject.FindObjectOfType<GameController>().StartGame();
    }
}
