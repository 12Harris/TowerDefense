using System.Collections;
using System.Collections.Generic;
using Harris.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace TowerDefense
{
    public class MachineGunTurret : Turret
    {

        private LineRenderer _lineRenderer;
        // Start is called before the first frame update

        public override void Awake()
        {
            base.Awake();

            _lineRenderer = GetComponent<LineRenderer>();

        }
        public override void Start()
        {
            base.Start();
            _detectionRangeIndicator.transform.localScale = new Vector3(Base_Detection_Range*2,transform.localScale.y,Base_Detection_Range*2);
            //_detectionTrigger.Radius = Base_Detection_Range;
        }

        // Update is called once per frame
        [System.Obsolete]
        public override void Update()
        {
            base.Update();

            if(!IsActive)
                return;

            if(Targets.Count > 0)
            {

                var _lookAt = new Vector3(Targets[CurrentTarget].transform.position.x, TowerHead.transform.position.y, Targets[CurrentTarget].transform.position.z);
                TowerHead.LookAt(_lookAt);
                //public static void DrawRay(Vector3 start, Vector3 dir, Color color = Color.white, float duration = 0.0f, bool depthTest = true);
                //Debug.DrawRay(_muzzlePoint.position, _lookAt-_muzzlePoint.position, Color.red);
                if(!_lineRenderer.enabled) _lineRenderer.enabled = true;
                _lineRenderer.SetColors(Color.red, Color.red);
                _lineRenderer.SetWidth(0.1f, 0.1f);
                _lineRenderer.SetPosition(0, _muzzlePoint.position);
                _lineRenderer.SetPosition(1, _lookAt);


            }
            else
            {
                _lineRenderer.enabled = false;
            }
        }

        void OnDrawGizmosSelected()
        {

            Gizmos.color = new Color(1, 1, 0, 0.75F);
        }
    }
}