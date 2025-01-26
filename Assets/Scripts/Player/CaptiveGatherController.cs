using System.Collections.Generic;
using UnityEngine;

namespace GGJ.BubbleFall
{
    public class CaptiveGatherController : MonoBehaviour, IInteractWithCaptive
    {
        public int TotalCaptives { get; private set; }

        private Stack<ICanBeCaptured> m_activeCaptives;

        public void CarryCaptive(ICanBeCaptured captive)
        {
            Debug.Log("Carry captive");

            m_activeCaptives ??= new Stack<ICanBeCaptured>();

            //Prevent from holding it twice
            if (m_activeCaptives.Contains(captive))
                return;

            m_activeCaptives.Push(captive);
            //captive.transform.gameObject.SetActive(false);

            TotalCaptives = m_activeCaptives.Count;
        }

        public ICanBeCaptured RequestCaptive()
        {
            m_activeCaptives.TryPop(out var captive);
            TotalCaptives = m_activeCaptives.Count;

            return captive;
        }
    }
}