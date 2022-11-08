using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorButton : MonoBehaviour
{
    public TextMeshProUGUI text;

    private GLOBAL.COLORMODES colormode = GLOBAL.COLORMODES.JOYFUL;

    public void onClick()
    {
        switch (colormode)
        {
            case GLOBAL.COLORMODES.JOYFUL: 
                colormode = GLOBAL.COLORMODES.ROMANTIC;
                text.text = "Romantic";
                break;
            case GLOBAL.COLORMODES.ROMANTIC: colormode = 
                GLOBAL.COLORMODES.SAD; 
                text.text = "Sad";
                break;
            case GLOBAL.COLORMODES.SAD: colormode = 
                GLOBAL.COLORMODES.JOYFUL;
                text.text = "Joyful";
                break;
            default: break;
        }
        GLOBAL.currentColorMode = colormode;
    }
}
