using UnityEngine;
using UnityEngine.Events;
using Harris.GPC;

namespace TowerDefense
{
    public class GameManager : BaseGameManager, ICommandReceiver
    {
        [SerializeField]
        private GameObject _mg_turretPrefab;

        [SerializeField]
        private GameObject _cannonPrefab;

         [SerializeField]
        private GameObject _gridTowerPrefab;

        private GameObject towerToPlace;

        private bool startingTowerPlacement = false;

        private bool startingGridTowerPlacement = false;

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

        [SerializeField]
        private Transform _selectedNode;

        private static int _gridNodeLayer = 10;
        private static int _gridNodeLayerMask; 

        private LineRenderer _enemyPathRenderer;


        private void Awake()
        {
            GameEventBus.Subscribe(GameEventTypes.START, new GameStartHandler());
            _mouseInput = GetComponent<MouseInput>();
            _kbInput = GetComponent<KeyboardInput>();
            _enemyPathRenderer = GetComponent<LineRenderer>();
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
            _gridNodeLayerMask = 1 << _gridNodeLayer;
        }

        private void Update()
        {

            if(firstUpdate)
            {
                ((ICommandReceiver)this).BindReceiver("LeftMouseClickCmd");
                ((ICommandReceiver)this).BindReceiver("LShiftCmd");
                firstUpdate = false;
            }


            //Visualize enemy path
            var lengthOfLineRenderer = PolygonalMap.Instance.FinalPath.Count;
            _enemyPathRenderer.positionCount = lengthOfLineRenderer;
            var points = new Vector3[lengthOfLineRenderer];

            for (int i = 0; i < lengthOfLineRenderer; i++)
            {
                points[i] = PolygonalMap.Instance.FinalPath[i].vPosition;
            }
            _enemyPathRenderer.SetPositions(points);
            _enemyPathRenderer.widthMultiplier = 0.2f;
            _enemyPathRenderer.SetColors(Color.red, Color.red);

            Debug.Log("enemy path has: " + PolygonalMap.Instance.FinalPath.Count + " waypoints!");

            //Tower Placement
            if(startingTowerPlacement)
            {
                Debug.Log("update tower placement");

                _placementPosition = MouseWorldPosition();

                towerToPlace.transform.position =  _placementPosition + Vector3.up*0.5f;
                //towerToPlace.GetComponent<BoxCollider>().enabled = false;
                //towerToPlace.GetComponent<Turret>().EnableCollider(false);
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

            else if(startingGridTowerPlacement)
            {
                _placementPosition = MouseGridPosition();
                //_selectedNode.position = new Vector3(_placementPosition.x,0.01f,_placementPosition.z);
                towerToPlace.transform.position =  new Vector3(_placementPosition.x,0.5f,_placementPosition.z);
                _finalPlacementPosition = towerToPlace.GetComponent<GridTower>().GetFinalPlacementLocation(_placementPosition);
                if(_finalPlacementPosition != Vector3.zero)
                {
                    towerToPlace.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
                else
                {
                    towerToPlace.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
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

        public static Vector3 MouseGridPosition()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);     
            RaycastHit rayHit;               
            
            if (Physics.Raycast(ray, out rayHit,Mathf.Infinity,_gridNodeLayerMask))
            {
                _oldMouseWorldPosition = rayHit.collider.transform.position;
                return rayHit.collider.transform.position;
            }
            return _oldMouseWorldPosition;
        }

        public static void SpawnTurret(string turretType)
        {

            if(turretType == "MachineGun")
            {
                Instance.startingTowerPlacement = true;
                Instance.towerToPlace = Instantiate(Instance._mg_turretPrefab, MouseWorldPosition(), Quaternion.identity);
            }

            else if(turretType == "Cannon")
            {
                Instance.startingTowerPlacement = true;
                Instance.towerToPlace = Instantiate(Instance._cannonPrefab, MouseWorldPosition(), Quaternion.identity);
            }

            else if(turretType == "GridTower")
            {
                Instance.towerToPlace = Instantiate(Instance._gridTowerPrefab, MouseWorldPosition(), Quaternion.identity);
                Instance.startingGridTowerPlacement = true;
            }
        }


        public void ReceiveCommand(Command command)
        {
            if(command.Name == "LeftMouseClickCmd")
            {
                if(command.Triggered && towerToPlace != null && _finalPlacementPosition != Vector3.zero)
                {
                    Debug.Log("Placing Tower!");

                    if(startingTowerPlacement)
                    {
                        startingTowerPlacement = false;
                        //towerToPlace.GetComponent<BoxCollider>().enabled = true;
                        towerToPlace.GetComponent<Turret>().EnableCollider(true);
                        towerToPlace.GetComponent<Turret>().Activate();

                        //Determine any friend turrets in the near vicinity
                        var turret = towerToPlace.GetComponent<Turret>();
                        //turret.Selected = true;
                        Turret.SelectedTurret = turret;

                        Collider[] hitColliders = Physics.OverlapSphere(towerToPlace.transform.position, turret.Base_Detection_Range, 1 << _turretLayer);

                        foreach (var hitCollider in hitColliders)
                        {

                            if(hitCollider.transform.parent.gameObject == towerToPlace)
                            {
                                continue;
                            }
                            Debug.Log("hit collider: " + hitCollider.transform);
                            var otherTurret = hitCollider.transform.parent.gameObject.GetComponent<Turret>();

                            if(!turret.Friends.Contains(otherTurret))
                            {
                                turret.Friends.Add(hitCollider.transform.parent.gameObject.GetComponent<Turret>());
                                turret.Friends[turret.Friends.Count-1].Friends.Add(turret);
                            }
                        }

                        foreach(var friend in turret.Friends)
                        {
                            Debug.Log(turret + " has friend: " + friend);
                        }
                    }
                    else
                    {
                        startingGridTowerPlacement = false;
                        PolygonalMap.Instance.GetNode(towerToPlace.transform.position)._gridTower = towerToPlace.GetComponent<GridTower>();
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