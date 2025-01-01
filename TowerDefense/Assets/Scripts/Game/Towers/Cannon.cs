using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.GPC;
using System;
using UnityEngine.UIElements;

namespace TowerDefense
{
    public class Cannon : Turret
    {
        [SerializeField]
        private GameObject _cannonProjectilePrefab;

        private GameObject _cannonBall;

        [SerializeField]
        private float _initialAngle = 30;

        private bool _fired = false;

        private Vector3 _projectileDirection;

        private bool _firstUpdate_Cannon = true;

        public bool CanFire {get; set;}

        private Transform _selectedIndicator;

        private Material _validTargetMat;

        private Transform _invalidTargetIndicator;

        private Transform _temp = null;

        public override void Awake()
        {
            base.Awake();
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            _selectedIndicator = transform.Find("SelectedIndicator");
            _selectedIndicator.transform.localScale = new Vector3(2,transform.localScale.y,2);

            _invalidTargetIndicator = transform.Find("InvalidTargetIndicator");
            //_invalidTargetIndicator.transform.localScale = new Vector3(Base_Detection_Range*2,transform.localScale.y,Base_Detection_Range*2);

            _detectionRangeIndicator.transform.localScale = new Vector3(Base_Detection_Range*2,transform.localScale.y,Base_Detection_Range*2);
            _detectionTrigger.radius = Base_Detection_Range;
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();

            if(_firstUpdate_Cannon)
            {
                _firstUpdate_Cannon = false;
                ((ICommandReceiver)this).BindReceiver("LShiftCmd");
            }

            _fireTimer += Time.deltaTime;
            if(_fireTimer >= FireInterval)
            {

                if(CanFire && Targets.Count > 0)
                {

                    var _lookAt = new Vector3(_detectionTrigger.center.x,transform.position.y,_detectionTrigger.center.z);
                    TowerHead.LookAt(_lookAt);
                    Fire();

                }
                _fireTimer = 0f;
            }

        }

        public override void Select()
        {
            base.Select();
            if(Selected)
            {
                _selectedIndicator.gameObject.SetActive(true);
                if(_temp)
                    _temp.gameObject.SetActive(true);
            }
            else
            {
                _selectedIndicator.gameObject.SetActive(false);
                if(_temp)
                    _temp.gameObject.SetActive(false);
            }
        }

        public override void ReceiveCommand(Command command)//ReceiveInputCommand
        {
            base.ReceiveCommand(command);

            if(!Selected)
                return;

            var v1 = _muzzlePoint.position;
            v1.y = 0;

            var v2 = GameManager.MouseWorldPosition();
            v2.y = 0;

            //Update cannon fire target
            if(command.Name == "LShiftCmd" && command.Executing)
            {
                CanFire = false;
                if((v2-v1).magnitude > 10)
                {
                    if(!_detectionRangeIndicator.gameObject.activeSelf)
                    {
                        _detectionRangeIndicator.gameObject.SetActive(true);
                        _invalidTargetIndicator.gameObject.SetActive(false);
                        _temp = _detectionRangeIndicator;
                    }

                    _projectileDirection = transform.position + (_temp.position-transform.position) - v1;
                }

                else if((v2-v1).magnitude > 2)
                {
                    if(_detectionRangeIndicator.gameObject.activeSelf)
                    {
                        _detectionRangeIndicator.gameObject.SetActive(false);
                        _invalidTargetIndicator.gameObject.SetActive(true);
                    }
                    _temp = _invalidTargetIndicator;
                }
                else
                {
                    _temp = null;
                }

                if(_temp == null)
                    return;

                _temp.position = new Vector3(v2.x,_temp.position.y, v2.z);

                if(_temp == _detectionRangeIndicator)
                    _detectionTrigger.center = transform.InverseTransformPoint(transform.position + (_detectionRangeIndicator.position-transform.position));

                //public static void DrawRay(Vector3 start, Vector3 dir, Color color = Color.white, float duration = 0.0f, bool depthTest = true);
                Debug.DrawRay(_muzzlePoint.position, _projectileDirection, Color.red);

            }

            else if(command.Name == "LShiftCmd" && command.Stopped)
            {
                if((v2-v1).magnitude > 10)
                {
                    CanFire = true;
                }
            }
        }

        private void Fire()
        {
            _cannonBall = Instantiate(_cannonProjectilePrefab, _muzzlePoint.position, Quaternion.identity);
            _cannonBall.GetComponent<CannonProjectile>().Initialize(_projectileDirection);
            //_vy = _vy0;
        }
    }
}