using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
    [Header("General")]
    public GameObject generalSettings;

    [Header("Speed")]
    public GameObject speedSettings;

    [Header("Sound")]
    public GameObject soundSettings;

    public void GeneralTab() {
        generalSettings.SetActive(true);
        speedSettings.SetActive(false);
        soundSettings.SetActive(false);
    }

    public void SpeedTab() {
        generalSettings.SetActive(false);
        speedSettings.SetActive(true);
        soundSettings.SetActive(false);
    }

    public void SoundTab() {
        generalSettings.SetActive(false);
        speedSettings.SetActive(false);
        soundSettings.SetActive(true);
    }
}
