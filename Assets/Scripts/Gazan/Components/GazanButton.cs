using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gazan.Components
{
    public class GazanButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnClickStarted;
        public event Action OnClickFinished;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClickStarted?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnClickFinished?.Invoke();
        }
    }
}