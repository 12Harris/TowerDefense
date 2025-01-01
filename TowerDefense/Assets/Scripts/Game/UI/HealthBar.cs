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
    class HealthBar : MonoBehaviour
    {
        private Image _bar;
        public float CurrentFillAmount { get => _bar.fillAmount; set => _bar.fillAmount = value; }

        private void Awake()
        {
            _bar = transform.Find("Canvas/Image").GetComponent<Image>();
        }

        public void Reset()
        {
            _bar.fillAmount = 1f;
        }

        private void Update()
        {
            //_bar.transform.parent.forward = new Vector3(-Camera.main.transform.forward.x,0,-Camera.main.transform.forward.z);
            var lookAtPoint = new Vector3(Camera.main.transform.position.x,_bar.transform.position.y,Camera.main.transform.position.z);
            _bar.transform.LookAt(lookAtPoint);
        }
    }
}
