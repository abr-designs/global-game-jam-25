using System;
using System.Collections;
using GameInput;
using UnityEngine;

namespace GGJ.BubbleFall
{
    class PlayerHealth : MonoBehaviour, ICanBeDamaged
    {
        public static event Action<int, int> OnPlayerHealthChange;
        public static event Action OnPlayerDeath;

        // When to kill player when they fall;
        [SerializeField] private float deathYValue;

        public int MaxHealth = 3;
        public int CurrentHealth = 0;

        [SerializeField] private float damageTimerWindow = .5f;
        private float _damageTimer = 0f;

        public bool IsAlive => CurrentHealth > 0;

        private Animator _playerAnim;

        void Start()
        {
            _playerAnim = GetComponentInChildren<Animator>();
        }

        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
            _damageTimer = damageTimerWindow;
            OnPlayerHealthChange?.Invoke(CurrentHealth, MaxHealth);
        }

        public void ReceiveDamage(int damage)
        {
            // Can't receive damage if we are still in the window
            if (_damageTimer > 0) return;

            CurrentHealth = Math.Max(CurrentHealth - damage, 0);
            _damageTimer = damageTimerWindow;

            // If the player took positive damage we handle effects
            if (damage > 0)
            {
                StartCoroutine(TakeHitCoroutine());
            }

            OnPlayerHealthChange?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth <= 0)
                KillPlayer();
        }


        private void KillPlayer()
        {
            // TODO -- animations etc
            gameObject.SetActive(false);
            OnPlayerDeath?.Invoke();
        }

        // Kill the player if they fall off the screen
        private void Update()
        {
            _damageTimer -= Time.deltaTime;

            if (IsAlive && transform.position.y < deathYValue)
            {
                ReceiveDamage(MaxHealth);
            }
        }

        private IEnumerator TakeHitCoroutine()
        {
            GameInputDelegator.SetInputLock(true);
            _playerAnim.Play("Hit");
            // TODO -- VFX here
            yield return new WaitForSeconds(damageTimerWindow / 2f);
            GameInputDelegator.SetInputLock(false);
        }

    }
}
