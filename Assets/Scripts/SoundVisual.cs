using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VisualType
{
    Line,
    Circle,
    Round,
}
public class SoundVisual : MonoBehaviour
{
    private const int SAMPLE_SIZE = 512; //frequency resolution


    public int CubeCount = 32; // to spawn how many cube
    public float ScaleTimes = 125.0f; // to multiply a number to have somthing we can see
    public float SmoothScale = 25.0f; // to reduce the size off
    public float MaxCubeScale = 20.0f;// to make sure cube scale is less than this value
    public float ReducePercentage = 0.1f;//to reduce average size

    [SerializeField]
    private VisualType m_visualType;
    [SerializeField]
    private GameObject m_cubePrefab;
    [SerializeField]
    public Transform m_circleRoot;
    [SerializeField]
    public Transform m_lineRoot;
    [SerializeField]
    public Transform m_roundRoot;

    private AudioSource m_source;
    [SerializeField]
    private float[] m_spectrums; //to populate with audio samples
    private Transform[] m_cubes; //to contain every single cube's transform
    private float[] m_cubeScales; //to stored what is the scale of object number

    private void Start()
    {
        m_source = GetComponent<AudioSource>();
        m_spectrums = new float[SAMPLE_SIZE];
        m_cubeScales = new float[CubeCount];
        m_cubes = new Transform[CubeCount];
        switch (m_visualType)
        {
            case VisualType.Line:
                SpawnLine();
                break;
            case VisualType.Circle:
                SpawnCircle();
                break;
            case VisualType.Round:
                SpawnRound();
                break;
            default:
                break;
        }
    }


    private void Update()
    {
        AnalyzeSound();
        UpdateScale();
    }

    private void AnalyzeSound()
    {
        //Get sound spectrum
        m_source.GetSpectrumData(m_spectrums, 0, FFTWindow.BlackmanHarris);
    }

    private void SpawnLine()
    {
        for (int i = 0; i < CubeCount; i++)
        {
            GameObject go = GameObject.Instantiate(m_cubePrefab);
            go.transform.SetParent(m_lineRoot);
            m_cubes[i] = go.transform;
            m_cubes[i].localPosition = Vector3.right * i;
        }
    }

    private void SpawnCircle()
    {
        Vector3 center = Vector3.zero;

        float radius = 10.0f;

        for (int i = 0; i < CubeCount; i++)
        {
            float angle = i * 1.0f / CubeCount;
            angle = angle * Mathf.PI * 2;

            float x = center.x + Mathf.Cos(angle) * radius;
            float y = center.y + Mathf.Sin(angle) * radius;

            Vector3 pos = center + new Vector3(x, y, 0);
            GameObject go = GameObject.Instantiate(m_cubePrefab);
            go.transform.SetParent(m_circleRoot);

            go.transform.position = pos;
            go.transform.rotation = Quaternion.LookRotation(Vector3.forward, pos);

            m_cubes[i] = go.transform;

        }
    }

    private void SpawnRound()
    {
        for (int i = 0; i < CubeCount; i++)
        {
            GameObject go = GameObject.Instantiate(m_cubePrefab);
            go.transform.SetParent(m_roundRoot);
            m_roundRoot.eulerAngles = new Vector3(0, -(360.0f / CubeCount) * i, 0);
            go.transform.position = Vector3.forward * 20;
            m_cubes[i] = go.transform;
        }
    }

    private void UpdateScale()
    {
        int cubeIndex = 0; //0-32
        int spectrumIndex = 0; //0-512
        int averageSize = (int)((SAMPLE_SIZE * ReducePercentage) / CubeCount); // every single cube to get a certain amount of the sample (ex.2 cube  256 sample per cube)//16
        while (cubeIndex < CubeCount)
        {
            float sum = 0;
            for (int i = 0; i < averageSize; i++)
            {
                sum += m_spectrums[spectrumIndex];
                spectrumIndex++;
            }

            float scaleY = (sum / averageSize) * ScaleTimes;

            m_cubeScales[cubeIndex] -= SmoothScale * Time.deltaTime;

            if (m_cubeScales[cubeIndex] < scaleY)
            {
                m_cubeScales[cubeIndex] = scaleY;
            }

            if (m_cubeScales[cubeIndex] > MaxCubeScale)
            {
                m_cubeScales[cubeIndex] = MaxCubeScale;
            }

            switch (m_visualType)
            {
                case VisualType.Line:
                case VisualType.Circle:
                    m_cubes[cubeIndex].localScale = Vector3.one + Vector3.up * m_cubeScales[cubeIndex];
                    break;
                case VisualType.Round:
                    m_cubes[cubeIndex].localScale = Vector3.one + Vector3.up * m_cubeScales[cubeIndex];
                    var posX = m_cubes[cubeIndex].position.x;
                    var posZ = m_cubes[cubeIndex].position.z;
                    m_cubes[cubeIndex].position = new Vector3(posX, m_cubes[cubeIndex].localScale.y / 2, posZ);
                    break;
                default:
                    break;
            }
            cubeIndex++;
        }
    }


}




