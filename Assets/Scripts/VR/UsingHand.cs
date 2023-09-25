using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingHand : MonoBehaviour
{
    public bool usingRighthand = false;
    public bool usingLefthand = false;
    private static UsingHand _Instance;
    public static UsingHand Instance { get { return _Instance; } }

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this);
    }

    public void Usinglefthand()
    {
        usingLefthand = !usingLefthand;
    }

    public void Usingrighthand()
    {
        usingRighthand = !usingRighthand;
    }

}
