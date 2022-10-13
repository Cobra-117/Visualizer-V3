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
        public static int BPM = 10;
        public static string status = "Loading";
    }
}
