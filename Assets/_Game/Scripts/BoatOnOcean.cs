using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
    public class BoatOnOcean : MonoBehaviour
    {

        public bool executeInEditMode = true;
        public bool drawGizmos = true;

        public enum UpdateMode { Update, LateUpdate, FixedUpdate };
        public UpdateMode updateMode = UpdateMode.Update;

        [Header("-- Ocean ------------------")]      
        // layermask
        public LayerMask oceanLayerMask;
        // offset
        public Vector3 oceanOffset = Vector3.zero;

        [Header("-- Boat body-------------")]   
        public float bodyLength = 2;
        public float bodyWidth = 1;
        
        public float gizmoSphereDiameter = 1;



        private Vector3 _posFront;
        private Vector3 _posBack;
        private Vector3 _posLeft;
        private Vector3 _posRight;

        protected void Update() 
        {
            #if UNITY_EDITOR
                if(executeInEditMode && !Application.isPlaying)
                {
                    _Update();
                    return;
                }                    
            #endif
            if(updateMode == UpdateMode.Update)
                _Update();
        }
        protected void LateUpdate() 
        {
            if(updateMode == UpdateMode.LateUpdate)
                _Update();
        }
        protected void FixedUpdate() 
        {
            if(updateMode == UpdateMode.FixedUpdate)
                _Update();
        }
        
        protected void _Update()
        {
            #if UNITY_EDITOR
                if(!executeInEditMode && !Application.isPlaying)
                    return;
            #endif

            _UpdateFollowTerrain();
        }


        void OnDrawGizmos()
        {
            if(!drawGizmos)
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_posFront, gizmoSphereDiameter);
            Gizmos.DrawSphere(_posBack, gizmoSphereDiameter);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_posLeft, gizmoSphereDiameter);
            Gizmos.DrawSphere(_posRight, gizmoSphereDiameter);
        }



        private void _UpdateFollowTerrain()
        {
            // 4 raycasts
            _posFront = _RaycastTerrain(transform.position + transform.forward * bodyLength / 2);
            _posFront = _RaycastTerrain(_posFront);            
            _posBack = _RaycastTerrain(transform.position - transform.forward * bodyLength / 2);
            _posBack = _RaycastTerrain(_posBack);            
            _posLeft = _RaycastTerrain(transform.position - transform.right * bodyWidth / 2);
            _posLeft = _RaycastTerrain(_posLeft);            
            _posRight = _RaycastTerrain(transform.position + transform.right * bodyWidth / 2);
            _posRight = _RaycastTerrain(_posRight);

            // average position
            Vector3 averageCenter = ((_posFront + _posBack) / 2 + (_posLeft + _posRight) / 2) / 2;
            Vector3 newPos = transform.position;
            newPos.y = averageCenter.y;
            transform.position = newPos;


            Vector3 goodPosition = transform.position;

            // angle x
            transform.position = _posBack;
            float angleX = GetAngleXFrom(transform, _posFront);

            // angle z
            transform.position = _posLeft;
            float angleZ = GetAngleZFrom(transform, _posRight);
        
            // restore good position
            transform.position = goodPosition + oceanOffset;


            Vector3 eulerAngles = new Vector3(
                transform.eulerAngles.x - angleX, 
                transform.eulerAngles.y,
                transform.eulerAngles.z - angleZ +90
            );
            transform.eulerAngles = eulerAngles;
        }

        private Vector3 _RaycastTerrain(Vector3 __position)
        {
            Ray ray = new Ray(__position + Vector3.up * 100, Vector3.down);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1000, oceanLayerMask))
            {
                __position = hit.point;
            }
            return __position;
        }

        public Vector3 GetAnglesFrom(Transform __from, Vector3 __to)
        {
            Vector3 localTarget = __from.InverseTransformPoint(__to);   
            return new Vector3
            (
                Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg,
                Mathf.Atan2(localTarget.z, localTarget.x) * Mathf.Rad2Deg,
                Mathf.Atan2(localTarget.x, localTarget.y) * Mathf.Rad2Deg
            );
        }


        public  float GetAngleXFrom(Transform __from, Vector3 __to)
        {
            Vector3 localTarget = __from.InverseTransformPoint(__to);        
            return Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;
        }
        public float GetAngleYFrom(Transform __from, Vector3 __to)
        {
            Vector3 localTarget = __from.InverseTransformPoint(__to);        
            return Mathf.Atan2(localTarget.z, localTarget.x) * Mathf.Rad2Deg;
        }
        public float GetAngleZFrom(Transform __from, Vector3 __to)
        {
            Vector3 localTarget = __from.InverseTransformPoint(__to);        
            return Mathf.Atan2(localTarget.x, localTarget.y) * Mathf.Rad2Deg;
        }

    }