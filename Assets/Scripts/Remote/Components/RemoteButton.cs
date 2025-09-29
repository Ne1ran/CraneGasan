using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Remote.Components
{
    public class RemoteButton : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }
    }
}
