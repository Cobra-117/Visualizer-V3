using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParticlesManager : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float simulatedSpeedBPM = 75;
    public float simulatedColorsBPM = 150;

    public Color[] colors;
    // Start is called before the first frame update
    void Start()
    {
        colors = new Color[] {new Color(133, 8, 194)/*purple*/,
        new Color(8, 195, 177)/*cyan*/, new Color(8, 195, 16)/*green*/,
        new Color(195, 8, 22)/*red*/, new Color(248, 11, 156)/*pink*/ };
        particleSystem = GetComponent<ParticleSystem>();
        StartCoroutine(changeColor());
    }

    // Update is called once per frame
    void Update()
    {
        var mainModule = particleSystem.main;
        mainModule.simulationSpeed = ComputeSimSpeed();
    }

    IEnumerator changeColor()
    {
        var mainModule = particleSystem.main;
        Random.InitState((int)(Time.time));
        for(;;) {
            Color pickedColor = colors[Random.Range(0, colors.Length)];
            mainModule.startColor = new ParticleSystem.MinMaxGradient(new Color(pickedColor.r/255, pickedColor.g/255, pickedColor.b/255));
            //yield return new WaitForSeconds(1/GLOBAL.CurrentMusic.BPM);
            yield return new WaitForSeconds(GetColorChangeSpeed());
        }
    }

    public float ComputeSimSpeed()
    {
        return (0.0165714f  * simulatedSpeedBPM - 1.02857f);
        //150 = 1.5
        //100 = 0.5
        //75 = 0.3
    }

    public float GetColorChangeSpeed()
    {
        return (8.9f - 0.06f * simulatedColorsBPM);
        //150 = 0.2
        //100 = 2
        //75 = 5
    }
}
