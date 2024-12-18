using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private Move playermove;
    private void Start()
    {
        if (GameObject.FindWithTag("Player") == null) return;
        playermove=GameObject.FindWithTag("Player")
            .GetComponent<Move>();
    }

    public void MapBittonClick()
    {
        Display.displays[1].Activate();
        //playermove.enabled = false;
    }
    public void MapBittonClick_Exit()
    {
        playermove.enabled = false;
    }
}
