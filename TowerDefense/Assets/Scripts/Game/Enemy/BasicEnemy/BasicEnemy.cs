using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.GPC;

namespace TowerDefense
{
    public class BasicEnemy : Enemy
    {
        protected override void Awake()
        {
            base.Awake();
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            MaxSpeed =2;
            Armor = 1;
            R_MachineGun = 0;
            R_LaserTurret = 0.2f;
            R_RocketTurret = 0.3f;
            R_Cannon = 0.4f;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        public static int GetMachineGunDamageCost(MachineGunTurret turret)
        {
            var base_damage = (turret.Base_Attack_Power - Armor) * 10;
            var damage_cost = base_damage - base_damage * R_MachineGun;
            return (int)damage_cost;
        }

        public void EatPlant()
        {
            
        }
    }
}
