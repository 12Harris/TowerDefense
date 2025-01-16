using UnityEngine;
using UnityEngine.Events;
using Harris.GPC;

namespace TowerDefense
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField]
        private TowerButton _machineGunButton;
        public TowerButton MachineGunButton => _machineGunButton;


        [SerializeField]
        private TowerButton _cannonButton;
        public TowerButton CannonButton => _cannonButton;

        [SerializeField]
        private TowerButton _gridTowerGunButton;
        public TowerButton GridTowerButton => _gridTowerGunButton;

        [SerializeField]
        private HealthBar _healthBar1Prefab;
        public HealthBar HealthBar1Prefab => _healthBar1Prefab;

        [SerializeField]
        private HealthBar _healthBar2Prefab;
        public HealthBar HealthBar2Prefab => _healthBar2Prefab;

        [SerializeField]
        private Transform _uiCanvas;
        public Transform UICanvas => _uiCanvas;

        public static Transform GetUICanvas()
        {
            return Instance.UICanvas;
        }

        public static HealthBar SpawnHealthBar1(Transform positionTransform)
        {
            var worldToScreen = Camera.main.WorldToScreenPoint(positionTransform.position);

            var healthBar = Instantiate(Instance.HealthBar1Prefab, worldToScreen, Quaternion.identity);
            healthBar.SetPositionTransform(positionTransform);
            return healthBar;
        }

         public static HealthBar SpawnHealthBar2(Transform positionTransform)
        {
            var worldToScreen = Camera.main.WorldToScreenPoint(positionTransform.position);

            var healthBar = Instantiate(Instance.HealthBar2Prefab, worldToScreen, Quaternion.identity);
            healthBar.SetPositionTransform(positionTransform);
            return healthBar;
        }


        private void Awake()
        {
            UIEventBus.Subscribe(UIEventTypes.TURRETBUTTONCLICK, new MGTurretButtonClickedHandler());
            UIEventBus.Subscribe(UIEventTypes.CANNONBUTTONCLICK, new CannonButtonClickedHandler());
            UIEventBus.Subscribe(UIEventTypes.GRIDTOWERBUTTONCLICK, new GridTowerButtonClickedHandler());
        }

        private void Start()
        {
            
        }
    }

    public class MGTurretButtonClickedHandler: IEventListener
    {
        public void Update(ISubject subject)
        {
            HandleEvent(subject as Harris.GPC.Event);
        }
        public void HandleEvent(Harris.GPC.Event ev)
        {
            Debug.Log("Turret Button clicked at Time: " + (ev as UIEvent).OccuredTime);
            GameManager.SpawnTurret("MachineGun");
        }
    }

    public class CannonButtonClickedHandler: IEventListener
    {
        public void Update(ISubject subject)
        {
            HandleEvent(subject as Harris.GPC.Event);
        }
        public void HandleEvent(Harris.GPC.Event ev)
        {
            Debug.Log("Cannon Button clicked at Time: " + (ev as UIEvent).OccuredTime);
            GameManager.SpawnTurret("Cannon");
        }
    }

        public class GridTowerButtonClickedHandler: IEventListener
    {
        public void Update(ISubject subject)
        {
            HandleEvent(subject as Harris.GPC.Event);
        }
        public void HandleEvent(Harris.GPC.Event ev)
        {
            Debug.Log("Grid Tower Button clicked at Time: " + (ev as UIEvent).OccuredTime);
            GameManager.SpawnTurret("GridTower");
        }
    }
}