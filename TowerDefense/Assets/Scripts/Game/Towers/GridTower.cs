using System;
using System.Collections;
using System.Collections.Generic;
using Harris.GPC;
using Harris.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Algorithms_C__Harris.Lists;

namespace TowerDefense
{
    public abstract class GridTower : MonoBehaviour, ICommandReceiver
    {

        private BoxCollider _boxCollider;
        private bool _selected = false;
        private bool _isActive = false;

        private int _layerMaskTurret;
        private int _turretLayer=3;

        private bool _firstUpdate = false;

        private static GridTower _selectedTurret;
        public static GridTower SelectedTurret { get =>_selectedTurret; set => _selectedTurret = value;}

        public void Activate()
        {
            _isActive = true;
        }

        public virtual void Awake()
        {
            _selected = false;
        }

        // Start is called before the first frame update
        public virtual void Start()
        {
            var layerMaskTurret = 1 << _turretLayer;
        }

        // Update is called once per frame
        public virtual void Update()
        {

            if(_firstUpdate)
            {
                _firstUpdate = false;
                ((ICommandReceiver)this).BindReceiver("LeftMouseClickCmd");
            }

        }

        public virtual void Select()
        {
            if(_selected)
            {
                
            }
            else
            {
                
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

        public void EnableCollider(bool value)
        {
            _boxCollider.enabled = value;
        }

        

    }
}