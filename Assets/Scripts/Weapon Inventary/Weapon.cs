﻿using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts.Weapon_Inventary
{
    public class Weapon : InventoryItem, IWeapon
    {

        public int Ammunition;
        public float ReloadTime = 0.5f;
        public GameObject BulletPrefab;

        private Transform _bulletSpawnPosition;
        public Transform BulletSpawnPosition
        {
            get
            {
                if (_bulletSpawnPosition == null)
                {
                    Vector3 spawnPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);

                    _bulletSpawnPosition = transform;
                }


                return _bulletSpawnPosition;

            }
        }


        private float _lastShootTime;

        void Start()
        {
            if(_bulletSpawnPosition == null)
            {
                _bulletSpawnPosition = transform.FindChild("AttackSpawnPoint");
            }
        }

        public override int UseAbleAmount
        {
            get { return Ammunition; }
        }


        void Update()
        {


        }

        public void Attack()
        {

            if (Ammunition == 0)
            {
                return;
            }
            if (Time.time - _lastShootTime < ReloadTime)
            {

                return;

            }


            var bullet = (GameObject)Instantiate(
            BulletPrefab,
            BulletSpawnPosition.position,
            BulletSpawnPosition.rotation);
            Bullet component = bullet.GetComponent<Bullet>();
            Ammunition--;
            _lastShootTime = Time.time;
            component.shooter = gameObject;

        }

        public override void OnBeingSelected()
        {
            _lastShootTime = Time.time;
        }
        public override void Use()
        {
            Attack();
        }


        protected override void OnCreateCopy(InventoryItem addComponent)
        {
            Weapon weapon = addComponent as Weapon;
            weapon.Ammunition = Ammunition;
            weapon.BulletPrefab = BulletPrefab;
            //weapon._bulletSpawnPosition = _bulletSpawnPosition;
            weapon.ReloadTime = ReloadTime;
            weapon._lastShootTime = _lastShootTime;
            weapon.InventaryItemName = InventaryItemName;
        }
    }


}
