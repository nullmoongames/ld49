// Crest Ocean System

// Copyright 2020 Wave Harmonic Ltd

// Thanks to @VizzzU for contributing this.

using UnityEngine;

namespace Crest
{
    /// <summary>
    /// Applies simple approximation of buoyancy force - force based on submerged depth and torque based on alignment
    /// to water normal.
    /// </summary>
    [AddComponentMenu(Internal.Constants.MENU_PREFIX_SCRIPTS + "Simple Floating Object")]
    public class BoatPositionController : FloatingObjectBase
    {
        [Header("Buoyancy Force")]
        [Tooltip("Offsets center of object to raise it (or lower it) in the water."), SerializeField]
        float _raiseObject = 1f;

        [Header("Wave Response")]
        [Tooltip("Diameter of object, for physics purposes. The larger this value, the more filtered/smooth the wave response will be."), SerializeField]
        float _objectWidth = 3f;
        public override float ObjectWidth { get { return _objectWidth; } }



        [Header("Debug")]
        [SerializeField] bool _debugDraw = false;

        bool _inWater;
        public override bool InWater { get { return _inWater; } }

        public override Vector3 Velocity => _rb.velocity;
        private Vector3 _pos;
        public float _offset;

        Rigidbody _rb;

        SampleHeightHelper _sampleHeightHelper = new SampleHeightHelper();
        SampleFlowHelper _sampleFlowHelper = new SampleFlowHelper();

        void Start()
        {
            //_rb = GetComponent<Rigidbody>();

            if (OceanRenderer.Instance == null)
            {
                enabled = false;
                return;
            }
        }

        void FixedUpdate()
        {
            UnityEngine.Profiling.Profiler.BeginSample("SimpleFloatingObject.FixedUpdate");

            if (OceanRenderer.Instance == null)
            {
                UnityEngine.Profiling.Profiler.EndSample();
                return;
            }

            _sampleHeightHelper.Init(transform.position, _objectWidth, true);
            _sampleHeightHelper.Sample(out Vector3 disp, out var normal, out var waterSurfaceVel);

            {
                _sampleFlowHelper.Init(transform.position, ObjectWidth);

                _sampleFlowHelper.Sample(out var surfaceFlow);
                waterSurfaceVel += new Vector3(surfaceFlow.x, 0, surfaceFlow.y);
            }



            if (_debugDraw)
            {
                Debug.DrawLine(transform.position + 5f * Vector3.up, transform.position + 5f * Vector3.up + waterSurfaceVel,
                    new Color(1, 1, 1, 0.6f));
            }

            //var velocityRelativeToWater = _rb.velocity - waterSurfaceVel;

            float height = disp.y + OceanRenderer.Instance.SeaLevel;
            //Debug.Log("WaterSurfacelevel : " + height);
            _pos = transform.position;
            _pos.y = height + _offset;
            transform.position = _pos;
            float bottomDepth = height - transform.position.y + _raiseObject;
            //Debug.Log("depth : " + bottomDepth);
            _inWater = bottomDepth > 0f;
            if (!_inWater)
            {
                UnityEngine.Profiling.Profiler.EndSample();
                return;
            }
 

            UnityEngine.Profiling.Profiler.EndSample();
        }

 
    }
}
