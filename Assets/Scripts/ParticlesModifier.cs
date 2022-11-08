using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ParticlesModifier : MonoBehaviour
{
    //public BPMDetector bPMDetector;
    public ParticleSystem particleSystem;
    //public BPMDetector bpm

    public Color[] joyfulColors;
    public Color[] romanticColors;

     public Color[] sadColors;
    float lastColorChange;
    float energy = 0.1f;
    //public float nsm = 5.0f;

    public float colorChangeInterval;
    private float goldenNumber = 1.618033f;

    public TextMeshProUGUI text;

    public Toggle automaticMod;
    public Slider simSpeedSlider;
    public Slider colorSpeedSlider;


    // Start is called before the first frame update
    void Start()
    {
        joyfulColors = new Color[] {new Color(133, 8, 194)/*purple*/,
        new Color(8, 195, 177)/*cyan*/, new Color(8, 195, 16)/*green*/,
        new Color(195, 8, 22)/*red*/, new Color(248, 11, 156)/*pink*/ };

        romanticColors = new Color[] { new Color(195, 8, 22)/*red*/, 
        new Color(248, 11, 156)/*pink*/ };

        sadColors = new Color[] { new Color(0, 0, 0)/*black*/, 
        new Color(92, 92, 92)/*dark grey*/, new Color(152, 152, 152) /*light grey*/,
        new Color(212, 212, 212)/*dark grey*/};
        particleSystem = GetComponent<ParticleSystem>();
        lastColorChange = Time.time;
        //Debug.Log("Size: " + colors.Length);
        StartCoroutine(changeColor());
    }

    // Update is called once per frame
    void Update()
    {
        double BPM = GLOBAL.CurrentMusic.BPM;
        var emission = particleSystem.emission;
        var mainModule = particleSystem.main;
        float test  = (float)BPM * 5; 
        //mainModule.simulationSpeed = Mathf.Sqrt((float)GLOBAL.CurrentMusic.BPM * (float)GLOBAL.CurrentMusic.BPM* goldenNumber)/100;
        //energy = float.Parse(GameObject.FindGameObjectWithTag("EnergyOBJ").name);
        mainModule.simulationSpeed = ComputeSimSpeed();
        Debug.Log("Sim speed: " + mainModule.simulationSpeed.ToString());
        Debug.Log("Color change interval: " + GetColorChangeSpeed().ToString());
        //text.text =  GLOBAL.CurrentMusic.BPM.ToString();
        //mainModule.startColor = new Color(135, 8, 195);
        //particleSystem.emission.rateOverTime =  BPM * 5;
        
    }

    IEnumerator changeColor()
    {
        var mainModule = particleSystem.main;
        Random.InitState((int)(GLOBAL.CurrentMusic.BPM * goldenNumber));
        for(;;) {
            Color pickedColor = joyfulColors[0];
            
            switch (GLOBAL.currentColorMode)
            {
                case GLOBAL.COLORMODES.JOYFUL:
                    pickedColor = joyfulColors[Random.Range(0, joyfulColors.Length)];
                    break;
                case GLOBAL.COLORMODES.ROMANTIC:
                    pickedColor = romanticColors[Random.Range(0, romanticColors.Length)];
                    break;
                case GLOBAL.COLORMODES.SAD:
                    pickedColor = sadColors[Random.Range(0, sadColors.Length)];
                    break;
                default: break;
            }
           
            mainModule.startColor = new ParticleSystem.MinMaxGradient(new Color(pickedColor.r/255, pickedColor.g/255, pickedColor.b/255));
            //yield return new WaitForSeconds(1/GLOBAL.CurrentMusic.BPM);
            yield return new WaitForSeconds(GetColorChangeSpeed());
        }
    }

    void test() 
    {
        UnityEngine.GameObject globalObject = UnityEngine.GameObject.FindGameObjectWithTag("GLOBAL");
        globalObject.GetComponents<GLOBAL>();
        UnityEngine.Debug.Log("BPM in PYTH: " + GLOBAL.CurrentMusic.BPM);
    }

    public float ComputeSimSpeed()
    {
        //return (0.0162857f * GLOBAL.CurrentMusic.BPM - 0.864286f);
        if (automaticMod.isOn && Application.platform != RuntimePlatform.WebGLPlayer)
            return (0.0165714f  * GLOBAL.CurrentMusic.BPM - 1.02857f);
        else {
            return simSpeedSlider.value;
        }
        //150 = 1.5
        //100 = 0.5
        //75 = 0.3
    }

    public float GetColorChangeSpeed()
    {
        if (automaticMod.isOn && Application.platform != RuntimePlatform.WebGLPlayer)
            return (8.9f - 0.06f * GLOBAL.CurrentMusic.BPM);
        else 
            return (colorSpeedSlider.value);
        //150 = 0.2
        //100 = 2
        //75 = 5
    }

}
