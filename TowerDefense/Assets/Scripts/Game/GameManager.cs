using UnityEngine;
using UnityEngine.Events;
using Harris.GPC;
using Unity.Plastic.Newtonsoft.Json.Serialization;

namespace TowerDefense
{
    public class GameManager : BaseGameManager, ICommandReceiver
    {
        [SerializeField]
        private GameObject _mg_turretPrefab;

        [SerializeField]
        private GameObject _cannonPrefab;

        private GameObject towerToPlace;

        private bool startingTowerPlacement = false;

        public static GameManager Instance;

        private MouseInput _mouseInput;

        private KeyboardInput _kbInput;

        private PlayerInputActions _inputActions;

        [SerializeField]
        private static int _groundLayer = 6;
        private static int _grounLayerMask;

        private int _turretLayer = 7;

        //[SerializeField]
        private int _waypointLayer = 3;

        private int _turret_waypoint_layer;

        private static Vector3 _oldMouseWorldPosition = Vector3.zero;

        private bool firstUpdate = true;

        private Vector3 _placementPosition;

        private Vector3 _finalPlacementPosition = Vector3.zero;

        private void Awake()
        {
            GameEventBus.Subscribe(GameEventTypes.START, new GameStartHandler());
            _mouseInput = GetComponent<MouseInput>();
            _kbInput = GetComponent<KeyboardInput>();
            Instance = this;
        }

        private void Start()
        {
            GameEventBus.Execute(GameEventTypes.START);


            _inputActions = new PlayerInputActions();
            _mouseInput.InputActions = _inputActions;
            _mouseInput.Initialize();

            _kbInput.InputActions = _inputActions;
            _kbInput.Initialize();
            
            var layerMaskTurret = 1 << _turretLayer;
            var layerMaskWaypoint = 1 << _waypointLayer;
            _turret_waypoint_layer = layerMaskTurret | layerMaskWaypoint;

            _grounLayerMask = 1 << _groundLayer;
        }

        private void Update()
        {

            if(firstUpdate)
            {
                ((ICommandReceiver)this).BindReceiver("LeftMouseClickCmd");
                ((ICommandReceiver)this).BindReceiver("LShiftCmd");
                firstUpdate = false;
            }

            if(startingTowerPlacement)
            {
                Debug.Log("update tower placement");

                _placementPosition = MouseWorldPosition();
                towerToPlace.transform.position =  _placementPosition + Vector3.up*0.5f;
                towerToPlace.GetComponent<BoxCollider>().enabled = false;
                _finalPlacementPosition = towerToPlace.GetComponent<Turret>().GetFinalPlacementLocation(_placementPosition);

                if(_finalPlacementPosition != Vector3.zero)
                {
                    towerToPlace.transform.Find("Head/Barrell").gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                    towerToPlace.transform.Find("Body").gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
                else
                {
                    towerToPlace.transform.Find("Head/Barrell").gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    towerToPlace.transform.Find("Body").gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                }
                
            }

            if(Turret.SelectedTurret != null)
            {
                Debug.Log("selected Turret = " + Turret.SelectedTurret);
            }
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition); 
        }

        public static Vector3 MouseWorldPosition()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);     
            RaycastHit rayHit;               
            
            if (Physics.Raycast(ray, out rayHit,Mathf.Infinity,_grounLayerMask))
            {
                _oldMouseWorldPosition = rayHit.point;
                return rayHit.point;
            }
            return _oldMouseWorldPosition;
        }

        public static void SpawnTurret(string turretType)
        {
            Instance.startingTowerPlacement = true;

            if(turretType == "MachineGun")
                Instance.towerToPlace = Instantiate(Instance._mg_turretPrefab, MouseWorldPosition(), Quaternion.identity);

            else if(turretType == "Cannon")
                Instance.towerToPlace = Instantiate(Instance._cannonPrefab, MouseWorldPosition(), Quaternion.identity);
        }


        public void ReceiveCommand(Command command)
        {
            if(command.Name == "LeftMouseClickCmd")
            {
                if(command.Triggered && towerToPlace != null && _finalPlacementPosition != Vector3.zero)
                {
                    Debug.Log("Placing Tower!");

                    startingTowerPlacement = false;
                    towerToPlace.GetComponent<BoxCollider>().enabled = true;
                    towerToPlace.GetComponent<Turret>().Activate();

                    //Determine any friend turrets in the near vicinity
                    var turret = towerToPlace.GetComponent<Turret>();
                    //turret.Selected = true;
                    Turret.SelectedTurret = turret;

                    Collider[] hitColliders = Physics.OverlapSphere(towerToPlace.transform.position, turret.Base_Detection_Range, 1 << _turretLayer);

                    foreach (var hitCollider in hitColliders)
                    {

                        if(hitCollider.gameObject == towerToPlace)
                        {
                            continue;
                        }
                        Debug.Log("hit collider: " + hitCollider.transform);
                        var otherTurret = hitCollider.gameObject.GetComponent<Turret>();

                        if(!turret.Friends.Contains(otherTurret))
                        {
                            turret.Friends.Add(hitCollider.gameObject.GetComponent<Turret>());
                            turret.Friends[turret.Friends.Count-1].Friends.Add(turret);
                        }
                    }

                    foreach(var friend in turret.Friends)
                    {
                        Debug.Log(turret + " has friend: " + friend);
                    }

                    towerToPlace = null;

                }
            }
        }
    }

    public class GameStartHandler : IEventListener
    {
        public void Update(ISubject subject)
        {
            HandleEvent(subject as Harris.GPC.Event);
        }
        public void HandleEvent(Harris.GPC.Event ev)
        {
            Debug.Log("Game Started at Time: " + (ev as GameEvent).OccuredTime);
        }
    }
}