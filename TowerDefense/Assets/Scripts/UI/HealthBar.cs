using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image _bar;
        public float CurrentFillAmount { get => _bar.fillAmount; set => _bar.fillAmount = value; }

        public void Reset()
        {
            _bar.fillAmount = 1f;
        }
    }
}
