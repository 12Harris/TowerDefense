using System;
using System.Collections;
using System.Collections.Generic;
using Harris.GPC;
using Harris.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TowerDefense
{
    public abstract class Turret : MonoBehaviour, ICommandReceiver
    {

        [SerializeField]
        private float _base_detection_range;
        public float Base_Detection_Range => _base_detection_range;
        protected Transform _detectionRangeIndicator;
        protected SphereCollider _detectionTrigger;

        [SerializeField]
        private float _base_rof;
        public float Base_ROF => _base_rof;

        public float FireInterval => 1/_base_rof;

        protected float _fireTimer = 0f;

        [SerializeField]
        private float _base_attack_power;
        public float Base_Attack_Power => _base_attack_power;

        private Transform _towerHead;
        public Transform TowerHead => _towerHead;

        protected Transform _muzzlePoint;

        private RotateObject _headRotationComponent;

        [SerializeField]
        private List<Enemy> _targets;
        public List<Enemy> Targets => _targets;

        private int _currentTarget = 0;
        public int CurrentTarget => _currentTarget;

        public static event Action<Enemy> _onTargetInRange;
        public static event Action<Enemy> _onTargetLost;

        private List<Turret> _friends;
        public List<Turret> Friends => _friends;

        private bool _selected;
        public bool Selected {get=> _selected; set => _selected = value;}

        private int _turretLayer = 7;

        //[SerializeField]
        private int _waypointLayer = 3;

        private int _turret_waypoint_layer;
        public int Turret_Waypoint_Layer => _turret_waypoint_layer;

        private bool _mouseOverTurret = false;

        private static Turret _selectedTurret;
        public static Turret SelectedTurret { get =>_selectedTurret; set => _selectedTurret = value;}

        private Collider _boxCollider;

        public Collider BoxCollider => _boxCollider;

        private bool _firstUpdate = true;

        private bool _isActive = false;

        public bool IsActive => _isActive;

        public void Activate()
        {
            _isActive = true;
        }

        public virtual void Awake()
        {
            _selected = false;
            _headRotationComponent = GetComponent<RotateObject>();
            _towerHead = transform.Find("Head");
            _detectionRangeIndicator = transform.Find("DetectionRangeIndicator");
            _detectionTrigger = gameObject.GetComponent<SphereCollider>();
            _targets = new List<Enemy>();
            _friends = new List<Turret>();
            _muzzlePoint = transform.Find("Head/Barrell/MuzzlePoint");
            _boxCollider = GetComponent<BoxCollider>();
        }

        // Start is called before the first frame update
        public virtual void Start()
        {
            var layerMaskTurret = 1 << _turretLayer;
            var layerMaskWaypoint = 1 << _waypointLayer;
            _turret_waypoint_layer = layerMaskTurret | layerMaskWaypoint;
        }

        // Update is called once per frame
        public virtual void Update()
        {

            if(_firstUpdate)
            {
                _firstUpdate = false;
                ((ICommandReceiver)this).BindReceiver("LeftMouseClickCmd");
            }

            if(_targets.Count > 0)
            {
                
                //Target Calculation
                if(_targets.Count < 3)
                {
                    _currentTarget = 0;

                }
                else
                {
    
                    int i = 0;
                    Turret bestFriend = null;

                    //Find "best friend" of this turret so that each has its own target
                    while(i < _friends.Count && _friends[i].Targets.Count == _targets.Count && bestFriend == null)
                    {
                        bool hasSameTargets = true;
                        for(int j = 0; j < _targets.Count; j++)
                        {
                            if(_friends[i].Targets[j] != _targets[j])
                                hasSameTargets = false;
                        }

                        if(hasSameTargets)
                        {
                            Debug.Log("BEST FRIEND!");
                            bestFriend = _friends[i];
                        }
                        else
                        {
                            i++;
                        }
                    } 

                    if(bestFriend != null)
                    {
                        bestFriend.Targets.RemoveAt(0);
                    }
                }
            }
        }

        public virtual void Select()
        {
            if(_selected)
            {
                _detectionRangeIndicator.gameObject.SetActive(true);
            }
            else
            {
                _detectionRangeIndicator.gameObject.SetActive(false);
            }
        }


        public virtual void ReceiveCommand(Command command)
        {
            if(command.Name == "LeftMouseClickCmd" && command.Triggered)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);   
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << _turretLayer);

                bool hitBoxCollider = false;

                for (int i = 0; i < hits.Length; i++)
                {
                    if(hits[i].collider == _boxCollider)
                    {
                        _selectedTurret = this;
                        _selected = !_selected;
                        hitBoxCollider = true;
                    }
                }

                if(!hitBoxCollider)
                    _selected = false;
                Select();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                var enemy = other.gameObject.GetComponent<Enemy>();
                //enemy.AddPotentialAttacker(this);

                _targets.Add(enemy);

            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                var enemy = other.gameObject.GetComponent<Enemy>();

                _targets.Remove(enemy);

                //enemy.RemovePotentialAttacker(this);
            }
        }


        public virtual Vector3 GetFinalPlacementLocation(Vector3 placementPosition) //Put this logic into turret module(abstract) cannon placement range...
        {
            //public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction);
  
            RaycastHit rayHit;               

            var rayDirections = new Vector3[4];
            rayDirections[0] = Vector3.forward;
            rayDirections[1] = Vector3.right;
            rayDirections[2] = -Vector3.forward;
            rayDirections[3] = -Vector3.right;

            //public static void DrawRay(Vector3 start, Vector3 dir, Color color = Color.white, float duration = 0.0f, bool depthTest = true);
            
            foreach(var rayDirection in rayDirections)
            {
                Debug.DrawRay(placementPosition - rayDirection + Vector3.up*0.5f, rayDirection * 2f, Color.green);
                if (Physics.Raycast( placementPosition - rayDirection + Vector3.up*0.5f, rayDirection ,out rayHit,2f, _turret_waypoint_layer))
                {
                    if(rayHit.collider.gameObject.tag == "Turret" && rayHit.collider == rayHit.collider.gameObject.GetComponent<Turret>().BoxCollider)
                        return Vector3.zero;

                    else if(rayHit.collider.gameObject.tag != "Turret")
                        return Vector3.zero;
                }
            }
            return transform.position;
            
        }
    }
}