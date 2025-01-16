using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class TowerButton : MonoBehaviour
    {

        private Button _button;
        
        [SerializeField]
        private int _id = 0;

        private void Awake()
        {
            _button = GetComponent<Button>();

            if(_id == 0)//Turret Button
                _button.onClick.AddListener(() => UIEventBus.Execute(UIEventTypes.TURRETBUTTONCLICK));

            else if(_id == 1)//Cannon Button
                _button.onClick.AddListener(() => UIEventBus.Execute(UIEventTypes.CANNONBUTTONCLICK));

            else if(_id == 2)//Grid Tower Button
                _button.onClick.AddListener(() => UIEventBus.Execute(UIEventTypes.GRIDTOWERBUTTONCLICK));
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}