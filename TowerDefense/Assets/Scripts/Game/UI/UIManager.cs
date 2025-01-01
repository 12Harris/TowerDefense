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


        private void Awake()
        {
            UIEventBus.Subscribe(UIEventTypes.TURRETBUTTONCLICK, new MGTurretButtonClickedHandler());
            UIEventBus.Subscribe(UIEventTypes.CANNONBUTTONCLICK, new CannonButtonClickedHandler());
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
}