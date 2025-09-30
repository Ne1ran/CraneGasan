using System;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using Gazan.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Gazan.Components
{
    public class GazanController : MonoBehaviour
    {
        private const string DangerZoneTag = "DangerZone";

        [field: SerializeField]
        public GazanButton GazanButton { get; set; } = null!;
        [field: SerializeField]
        public RectTransform MainScreen { get; set; } = null!;
        [field: SerializeField]
        public RectTransform LoadingScreen { get; set; } = null!;
        [field: SerializeField]
        public TextMeshProUGUI DistanceText { get; set; } = null!;
        [field: SerializeField]
        public TextMeshProUGUI GasText { get; set; } = null!;
        [field: SerializeField]
        public TextMeshProUGUI LoadingText { get; set; } = null!;
        [field: SerializeField]
        public Image LoadingImage { get; set; } = null!;
        [field: SerializeField, Tooltip("Time in seconds to activate/deactivate gazan")]
        public float GazanActivateTime { get; set; } = 3f;
        [field: SerializeField, Tooltip("Time in seconds to show new gas parameters")]
        public float GasChangeTimer { get; set; } = 3f;

        private Image MainScreenImage { get; set; } = null!;
        private GazanState _currentGazanState;

        private bool _isHolding;
        private float _holdTime = 0f;
        private float _generateNextGasTimer = 0f;

        private readonly List<Transform> _dangerObjTransforms = new();

        private void Awake()
        {
            _currentGazanState = GazanState.Disabled;
            MainScreenImage = MainScreen.GetComponent<Image>()!;
            GazanButton.OnClickStarted += OnButtonClickStarted;
            GazanButton.OnClickFinished += OnButtonClickFinished;
        }

        private void OnDestroy()
        {
            GazanButton.OnClickStarted -= OnButtonClickStarted;
            GazanButton.OnClickFinished -= OnButtonClickFinished;
        }

        private void Update()
        {
            if (_isHolding) {
                _holdTime += Time.deltaTime;
            }

            if (_holdTime >= GazanActivateTime) {
                SwitchGazanState();
            }

            HandleState();
        }

        private void OnButtonClickFinished()
        {
            ResetLoading();
        }

        private void OnButtonClickStarted()
        {
            if (_currentGazanState == GazanState.Disabled) {
                LoadingScreenActive = true;
            }

            _isHolding = true;
        }

        private void SwitchGazanState()
        {
            _generateNextGasTimer = 0f;
            ResetLoading();

            switch (_currentGazanState) {
                case GazanState.Active: {
                    MainScreenActive = false;
                    _currentGazanState = GazanState.Disabled;
                    DisplayAlpha = 0f;
                    return;
                }
                case GazanState.Disabled: {
                    FindDangerObjects();

                    LoadingScreenActive = false;
                    MainScreenActive = true;
                    _currentGazanState = GazanState.Active;
                    DisplayAlpha = 0f;
                    MainScreenImage.DOFade(0.5f, 0.75f).Play();
                    GasTextString = Math.Round(UnityEngine.Random.Range(0f, 100f), 1).ToString(CultureInfo.InvariantCulture);
                    return;
                }
            }
        }

        private void FindDangerObjects()
        {
            _dangerObjTransforms.Clear();
            GameObject[] dangerObjects = GameObject.FindGameObjectsWithTag(DangerZoneTag);
            foreach (GameObject dangerObject in dangerObjects) {
                _dangerObjTransforms.Add(dangerObject.transform);
            }
        }

        private void HandleState()
        {
            switch (_currentGazanState) {
                case GazanState.Active: {
                    UpdateDangerInfo();
                    if (_generateNextGasTimer >= GasChangeTimer) {
                        GasTextString = Math.Round(UnityEngine.Random.Range(0f, 100f), 1).ToString(CultureInfo.InvariantCulture);
                        _generateNextGasTimer = 0f;
                    }

                    _generateNextGasTimer += Time.deltaTime;
                    break;
                }
                case GazanState.Disabled:
                    if (_isHolding) {
                        LoadingValue = Mathf.Clamp01(_holdTime / GazanActivateTime);
                    }

                    break;
            }
        }

        private void UpdateDangerInfo()
        {
            Vector3 currentPosition = transform.position;
            float? nearestDistance = MathUtils.FindNearestDistance(currentPosition, _dangerObjTransforms);
            DangerDistanceText = nearestDistance == null ? "No target" : Math.Round(nearestDistance.Value, 2).ToString(CultureInfo.InvariantCulture);
        }

        private void ResetLoading()
        {
            _isHolding = false;
            _holdTime = 0f;
            LoadingValue = 0f;
        }

        private bool MainScreenActive
        {
            set => MainScreen.gameObject.SetActive(value);
        }

        private bool LoadingScreenActive
        {
            set => LoadingScreen.gameObject.SetActive(value);
        }

        private string DangerDistanceText
        {
            set => DistanceText.text = value;
        }

        private string GasTextString
        {
            set => GasText.text = value;
        }

        private float LoadingValue
        {
            set
            {
                LoadingImage.fillAmount = value;
                LoadingText.text = Mathf.RoundToInt(value * 100f).ToString();
            }
        }

        private float DisplayAlpha
        {
            set
            {
                Color startingColor = MainScreenImage.color;
                MainScreenImage.color = new(startingColor.a, startingColor.g, startingColor.b, value);
            }
        }
    }
}