using System;
using GGJ.BubbleFall;
using UnityEngine;
using Utilities.Debugging;

namespace GGJ.BubbleFall
{
    public class GoalZone : MonoBehaviour
    {
        private Collider2D _collider;
        private PlayerMovementV2 _player;

        private bool _isHit;

        public static event Action GoalHit;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _collider = GetComponent<Collider2D>();
            _player = FindFirstObjectByType<PlayerMovementV2>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isHit)
                CheckPlayer();
        }

        private void CheckPlayer()
        {
            if (_collider.bounds.Contains(_player.transform.position) && _player.IsGrounded)
            {
                _isHit = true;
                // invoke event
                GoalHit?.Invoke();
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_player)
                Draw.Label(transform.position, $"Player: {_player.transform.position} \n IsInZone: {_collider.bounds.Contains(_player.transform.position)}");
        }

#endif


    }
}