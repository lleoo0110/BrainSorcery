using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour
{

    //ƒƒ\ƒbƒh
    //==============================================================================

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Initiate.Fade("Exhibition Game", Color.black, 1f);
        }
    }
}