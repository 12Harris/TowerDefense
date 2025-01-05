using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class HealthBar : MonoBehaviour
    {
        private Image _bar;
        public float CurrentFillAmount { get => _bar.fillAmount; set => _bar.fillAmount = value; }

        private Transform _positionTransform = null;

        private void Awake()
        {
            _bar = transform.Find("Canvas/Image").GetComponent<Image>();
        }

        public void Reset()
        {
            _bar.fillAmount = 1f;
        }

        public void SetPositionTransform(Transform positionTransform)
        {
            _positionTransform = positionTransform;
            transform.parent = UIManager.GetUICanvas();
        }

        private void Update()
        {
            if(_positionTransform != null)
            {
                var worldToScreen = Camera.main.WorldToScreenPoint(_positionTransform.position);
                worldToScreen.z = UIManager.GetUICanvas().position.z;
                transform.position = worldToScreen;                

                var lookAtPoint = _bar.transform.parent.parent.position - Vector3.forward;
                lookAtPoint.y = _bar.transform.parent.parent.position.y;
                _bar.transform.parent.parent.LookAt(lookAtPoint);
            }
        }
    }
}
