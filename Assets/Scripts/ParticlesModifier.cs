using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ParticlesModifier : MonoBehaviour
{
    //public BPMDetector bPMDetector;
    public ParticleSystem particleSystem;
    //public BPMDetector bpm

    public Color[] colors;// =  {new Color(0.52f, 0.03f, 0.76f)/*purple*/,
   // new Color(0.03f, 0.89f, 0.78f)/*cyan*/, new Color(8, 195, 16)/*green*/,
   // new Color(195, 8, 22)/*red*/, new Color(248, 11, 156)/*pink*/ 
    //};
    //public Color[] colors;
    float lastColorChange;
    float energy = 0.1f;
    public float nsm = 5.0f;

    public float colorChangeInterval;
    private float goldenNumber = 1.618033f;

    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        colors = new Color[] {new Color(133, 8, 194)/*purple*/,
        new Color(8, 195, 177)/*cyan*/, new Color(8, 195, 16)/*green*/,
        new Color(195, 8, 22)/*red*/, new Color(248, 11, 156)/*pink*/ };
        particleSystem = GetComponent<ParticleSystem>();
        lastColorChange = Time.time;
        Debug.Log("Size: " + colors.Length);
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
        //mainModule.simulationSpeed = GetSimSpeedFromBPM();
        Debug.Log("Sim speed: " + mainModule.simulationSpeed.ToString());
        //text.text =  GLOBAL.CurrentMusic.BPM.ToString();
        //mainModule.startColor = new Color(135, 8, 195);
        //particleSystem.emission.rateOverTime =  BPM * 5;
        
    }

    IEnumerator changeColor()
    {
        var mainModule = particleSystem.main;
        Random.InitState((int)(GLOBAL.CurrentMusic.BPM * goldenNumber));
        for(;;) {
            Color pickedColor = colors[Random.Range(0, colors.Length)];
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

    public float GetSimSpeedFromBPM()
    {
        //return (0.0162857f * GLOBAL.CurrentMusic.BPM - 0.864286f);
        return (0.0165714f  * GLOBAL.CurrentMusic.BPM - 1.02857f);
        //150 = 1.5
        //100 = 0.5
        //75 = 0.3
    }

    public float GetColorChangeSpeed()
    {
        return (8.9f - 0.06f * GLOBAL.CurrentMusic.BPM);
        //150 = 0.2
        //100 = 2
        //75 = 5
    }

}
