using System;
using UnityEngine;
using Zenject;

namespace ModestTree
{
    public class PlayerShootHandler : ITickable
    {
        readonly PlayerModel _player;
        readonly Settings _settings;
        readonly Bullet.Factory _bulletFactory;
        readonly PlayerInputState _inputState;

        float _lastFireTime;

        public PlayerShootHandler(
            PlayerInputState inputState,
            Bullet.Factory bulletFactory,
            Settings settings,
            PlayerModel player)
        {
            _player = player;
            _settings = settings;
            _bulletFactory = bulletFactory;
            _inputState = inputState;
        }

        public void Tick()
        {
            if (_inputState.IsFiring && Time.realtimeSinceStartup - _lastFireTime > _settings.MaxShootInterval)
            {
                _lastFireTime = Time.realtimeSinceStartup;
                Fire();
            }
        }

        void Fire()
        {
            var bullet = _bulletFactory.Create(
                _settings.BulletSpeed, _settings.BulletLifetime, BulletTypes.FromPlayer);

            bullet.transform.position = _player.Position + _player.LookDir * _settings.BulletOffsetDistance;
            bullet.transform.rotation = _player.Rotation;
        }

        [Serializable]
        public class Settings
        {
            public float BulletLifetime;
            public float BulletSpeed;
            public float MaxShootInterval;
            public float BulletOffsetDistance;
        }
    }
}