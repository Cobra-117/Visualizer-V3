using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Text;
using NAudio.Wave;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class BPMDetector : MonoBehaviour
{
    private string filename = null;
    private short[] leftChn;
    private short[] rightChn;
    private double BPM;
    private double sampleRate = 44100;
    private double trackLength = 0;

    public AudioSource audioSource;
    public AudioClip clip;

    public Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

    void Start()
    {   
        sampleRate = 44100;
        Debug.Log("sampleRate1: " + sampleRate.ToString());
        filename = "Assets/Resources/silence.wav";
        audioSource = this.gameObject.GetComponent<AudioSource>();
        //LoadMusic(filename);
        //Detect();
    }
    void Update()
    {
        Debug.Log("BPM: " + getBPM().ToString());
        if (GLOBAL.CurrentMusic.isLoaded == false) {
            LoadMusic(GLOBAL.CurrentMusic.name);
            var audiopath = Path.Combine(Application.streamingAssetsPath, GLOBAL.CurrentMusic.name);
            filename = audiopath;
            //filename = "/home/tLacheroy/CMGT/Visualizer-V3/Assets/StreamingAssets/silence.wav";
            Detect();
            GLOBAL.CurrentMusic.isLoaded = true;
        }
    }
    public double getBPM()
    {
        return BPM;
    }

    public BPMDetector(string filename)
    {
        this.filename = filename;
        Detect();
    }

    public BPMDetector(short[] leftChn, short[] rightChn)
    {
        this.leftChn = leftChn;
        this.rightChn = rightChn;
        Detect();
    }

    private void Detect()
    {
        if (filename != null)
        {
            using (WaveFileReader reader = new WaveFileReader(filename))
            {
                byte[] buffer = new byte[reader.Length];
                int read = reader.Read(buffer, 0, buffer.Length);
                short[] sampleBuffer = new short[read / 2];
                Buffer.BlockCopy(buffer, 0, sampleBuffer, 0, read);

                List<short> chan1 = new List<short>();
                List<short> chan2 = new List<short>();

                Debug.Log("Buffer lenght: " + sampleBuffer.Length.ToString());
                for (int i = 0; i < sampleBuffer.Length; i += 2)
                {
                    chan1.Add(sampleBuffer[i]);
                    chan2.Add(sampleBuffer[i + 1]);
                }

                leftChn = chan1.ToArray();
                rightChn = chan2.ToArray();
            }
        }

        trackLength = (float)leftChn.Length / sampleRate;
        Debug.Log("LeftChans: " + leftChn.Length.ToString());
        Debug.Log("TrackLgt1: " + ((float)trackLength).ToString());
        Debug.Log("sampleRate: " + sampleRate.ToString());

        // 0.1s window ... 0.1*44100 = 4410 samples, lets adjust this to 3600 
        int sampleStep = 3600;

        // calculate energy over windows of size sampleSetep
        List<double> energies = new List<double>();
        for (int i = 0; i < leftChn.Length - sampleStep - 1; i += sampleStep)
        {
            energies.Add(rangeQuadSum(leftChn, i, i + sampleStep));
        }
        Debug.Log("Energy : " + energies.Count.ToString());
        int beats = 0;
        double average = 0;
        double sumOfSquaresOfDifferences = 0;
        double variance = 0;
        double newC = 0;
        List<double> variances = new List<double>();

        // how many energies before and after index for local energy average
        int offset = 10;

        for (int i = offset; i <= energies.Count - offset - 1; i++)
        {
            // calculate local energy average
            double currentEnergy = energies[i];
            double qwe = rangeSum(energies.ToArray(), i - offset, i - 1) + currentEnergy + rangeSum(energies.ToArray(), i + 1, i + offset);
            qwe /= offset * 2 + 1;

            // calculate energy variance of nearby energies
            List<double> nearbyEnergies = energies.Skip(i - 5).Take(5).Concat(energies.Skip(i + 1).Take(5)).ToList<double>();
            average = nearbyEnergies.Average();
            sumOfSquaresOfDifferences = nearbyEnergies.Select(val => (val - average) * (val - average)).Sum();
            variance = (sumOfSquaresOfDifferences / nearbyEnergies.Count) / Math.Pow(10, 22);

            // experimental linear regression - constant calculated according to local energy variance
            newC = variance * 0.009 + 1.385;
            if (currentEnergy > newC * qwe)
                beats++;
        }
        //Debug.Log("Beats : " + beats.ToString());
        BPM = beats / (trackLength / 60);
        //Debug.Log("track lenght:" + (trackLength / 60).ToString());
        Debug.Log("BPM2: " + BPM.ToString());
        GLOBAL.CurrentMusic.BPM = (int)BPM;

    }

    private static double rangeQuadSum(short[] samples, int start, int stop)
    {
        double tmp = 0;
        for (int i = start; i <= stop; i++)
        {
            tmp += Math.Pow(samples[i], 2);
        }

        return tmp;
    }

    private static double rangeSum(double[] data, int start, int stop)
    {
        double tmp = 0;
        for (int i = start; i <= stop; i++)
        {
            tmp += data[i];
        }

        return tmp;
    }

    public async void LoadMusic(string path) {
        //clip = Resources.Load<AudioClip>(Application.persistentDataPath + "piano.wav");
        
        //audioSource.Play();
        /*Detect();
        var fileNames = Directory.GetFiles(Application.persistentDataPath, "*.jpg");
        foreach(var fileName in fileNames)
        {
            Debug.Log("filename : " +fileName);
            StartCoroutine (LoadFile(fileName));
        }*/
        //clip = Resources.Load(Path.Combine( Application.dataPath, "Resources", "Axel_F.wav")) as AudioClip;
        Debug.Log("GlbName : " + GLOBAL.CurrentMusic.name);
        //clip = Resources.Load<AudioClip>(System.IO.Path.GetFileNameWithoutExtension(GLOBAL.CurrentMusic.name));
        var audiopath = Path.Combine(Application.streamingAssetsPath, GLOBAL.CurrentMusic.name);
        Debug.Log("path: " + audiopath.ToString());
        await LoadClip(audiopath);
        audioSource.clip = clip;
        if (clip)
            Debug.Log("Frequency :" + clip.frequency.ToString());
        else
            Debug.Log("Clip in null");
        audioSource.Play();
        //clip.
        //filename = Path.Combine( Application.dataPath, "Axel_F.wav");
        Detect();
    }

     async Task<AudioClip> LoadClip(string path)
    {
     //AudioClip clip = null;
     //clip = Resources.Load(Path.Combine( Application.dataPath, "Axel_F.wav")) as AudioClip;
     //return clip;
     //path = Path.Combine(Application.streamingAssetsPath, path);
     Debug.Log("path (load clip): " + path);
     using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
     {
         uwr.SendWebRequest();
 
         // wrap tasks in try/catch, otherwise it'll fail silently
         try
         {
             while (!uwr.isDone) await Task.Delay(5);
 
             if (uwr.result == UnityWebRequest.Result.ConnectionError 
                || uwr.result == UnityWebRequest.Result.ProtocolError) 
                Debug.Log($"{uwr.error}");
             else
             {
                 clip = DownloadHandlerAudioClip.GetContent(uwr);
                //string assetPath = AssetDatabase.GetAssetPath(clip.GetInstanceID());
                //Debug.Log("clip path: " + assetPath);
             }
         }
         catch (Exception err)
         {
             Debug.Log($"{err.Message}, {err.StackTrace}");
         }
     }
        Debug.Log("returning clip");
     return clip;
 }

    private IEnumerator LoadFile(string filePath)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(filePath))
    //using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV))
    {
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            // Get downloaded asset bundle
            /*var texture = DownloadHandlerTexture.GetContent(uwr);

            // Something with the texture e.g.
            // store it to later access it by fileName
            var fileName = Path.GetFileName(filePath);
            textures[fileName] = texture;*/
            //clip= DownloadHandlerAudioClip.GetContent(uwr);
            //clip = (AudioClip)tmpclip;
        }
    }
    }
}