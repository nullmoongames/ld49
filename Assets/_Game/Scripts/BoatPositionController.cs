using UnityEngine;
using Crest;

public class BoatPositionController : MonoBehaviour
{
    float _objectWidth = 3f;



    [Header("Debug")]
    [SerializeField] bool _debugDraw = false;
    private Vector3 _pos;
    public float _offset;


    SampleHeightHelper _sampleHeightHelper = new SampleHeightHelper();
    SampleFlowHelper _sampleFlowHelper = new SampleFlowHelper();

    void Start()
    {

        if (OceanRenderer.Instance == null)
        {
            enabled = false;
            return;
        }
    }

    void FixedUpdate()
    {

        if (OceanRenderer.Instance == null)
        {
            UnityEngine.Profiling.Profiler.EndSample();
            return;
        }

        _sampleHeightHelper.Init(transform.position, _objectWidth, true);
        _sampleHeightHelper.Sample(out Vector3 disp, out var normal, out var waterSurfaceVel);

        {
            _sampleFlowHelper.Init(transform.position, 70);

            _sampleFlowHelper.Sample(out var surfaceFlow);
            waterSurfaceVel += new Vector3(surfaceFlow.x, 0, surfaceFlow.y);
        }



        if (_debugDraw)
        {
            Debug.DrawLine(transform.position + 5f * Vector3.up, transform.position + 5f * Vector3.up + waterSurfaceVel,
                new Color(1, 1, 1, 0.6f));
        }



        float height = disp.y + OceanRenderer.Instance.SeaLevel;
        //Debug.Log("WaterSurfacelevel : " + height);
        _pos = transform.position;
        _pos.y = height + _offset;
        transform.position = _pos;

        UnityEngine.Profiling.Profiler.EndSample();
    }


}
