using System;
using UnityEngine;

namespace Remote.Components
{
    public class RemoteController : MonoBehaviour
    {
        [field: SerializeField]
        public RemoteButton UpButton { get; set; } = null!;
        [field: SerializeField]
        public RemoteButton DownButton { get; set; } = null!;
        [field: SerializeField]
        public RemoteButton NorthButton { get; set; } = null!;
        [field: SerializeField]
        public RemoteButton SouthButton { get; set; } = null!;
        [field: SerializeField]
        public RemoteButton EastButton { get; set; } = null!;
        [field: SerializeField]
        public RemoteButton WestButton { get; set; } = null!;

        public event Action OnUpClicked;
        public event Action OnDownClicked;
        public event Action OnNorthClicked;
        public event Action OnSouthClicked;
        public event Action OnWestClicked;
        public event Action OnEastClicked;

        private void Awake()
        {
            UpButton.OnClick += () => OnUpClicked?.Invoke();
            DownButton.OnClick += () => OnDownClicked?.Invoke();
            NorthButton.OnClick += () => OnNorthClicked?.Invoke();
            SouthButton.OnClick += () => OnSouthClicked?.Invoke();
            WestButton.OnClick += () => OnWestClicked?.Invoke();
            EastButton.OnClick += () => OnEastClicked?.Invoke();
        }
    }
}