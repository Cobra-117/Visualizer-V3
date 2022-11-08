using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLOBAL : MonoBehaviour
{
    public static class CurrentMusic {
        public static string ID = "";

        public static string SpotifyID = "";
        public static string name = "";
        public static string artist = "";
        public static int BPM = 70;
        public static string status = "Loading";
        public static string path = "";
        public static bool isLoaded = true;
    }

    public static COLORMODES currentColorMode = COLORMODES.JOYFUL;

    public enum COLORMODES {
        JOYFUL,
        SAD,
        ROMANTIC
    }
}
