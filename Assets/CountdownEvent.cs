using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownEvent : MonoBehaviour
{
    public void CallStartGame()
    {
        GameObject.FindObjectOfType<GameController>().StartGame();
    }
}
