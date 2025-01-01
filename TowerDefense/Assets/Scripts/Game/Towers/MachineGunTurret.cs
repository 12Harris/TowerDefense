using System.Collections;
using System.Collections.Generic;
using Harris.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace TowerDefense
{
    public class MachineGunTurret : Turret
    {

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            _detectionRangeIndicator.transform.localScale = new Vector3(Base_Detection_Range*2,transform.localScale.y,Base_Detection_Range*2);
            _detectionTrigger.radius = Base_Detection_Range;
        }

        // Update is called once per frame
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
                Debug.DrawRay(_muzzlePoint.position, _lookAt-_muzzlePoint.position, Color.red);
            }
        }

        void OnDrawGizmosSelected()
        {
            // Display the explosion radius when selected
            Gizmos.color = new Color(1, 1, 0, 0.75F);
            //Gizmos.DrawSphere(transform.position, Base_Detection_Range);
        }
    }
}