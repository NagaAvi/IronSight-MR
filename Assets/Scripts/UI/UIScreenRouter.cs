using IronSight.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace IronSight.UI
{
    public sealed class UIScreenRouter : MonoBehaviour
    {
        private const string BuiltinFontName = "LegacyRuntime.ttf";

        private GameManager _gameManager;
        private Canvas _canvas;
        private GameObject _dashboardPanel;
        private GameObject _roomPreparationPanel;
        private Text _statusText;

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            EnsureCanvas();
        }

        public void ShowDashboard()
        {
            EnsureCanvas();
            _dashboardPanel.SetActive(true);
            _roomPreparationPanel.SetActive(false);
        }

        public void ShowRoomPreparation()
        {
            EnsureCanvas();
            _dashboardPanel.SetActive(false);
            _roomPreparationPanel.SetActive(true);
        }

        public void SetStatus(string message)
        {
            EnsureCanvas();
            _statusText.text = message;
        }

        private void EnsureCanvas()
        {
            if (_canvas != null)
            {
                return;
            }

            var existingCanvas = transform.root.GetComponentInChildren<Canvas>();
            if (existingCanvas != null)
            {
                _canvas = existingCanvas;
                return;
            }

            var canvasObject = new GameObject("Canvas_Main", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(null, false);

            _canvas = canvasObject.GetComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.pixelPerfect = true;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            CreateBackground(canvasObject.transform);
            _dashboardPanel = CreateDashboardPanel(canvasObject.transform);
            _roomPreparationPanel = CreateRoomPreparationPanel(canvasObject.transform);
            CreateStatusPanel(canvasObject.transform);
            EnsureEventSystem();
        }

        private GameObject CreateDashboardPanel(Transform parent)
        {
            var panel = CreatePanel(parent, "DashboardPanel", new Vector2(780f, 540f));

            CreateText(panel.transform, "TitleText", "IronSight MR", 76, new Vector2(0f, 145f), new Vector2(700f, 110f), FontStyle.Bold, new Color(0.82f, 0.82f, 0.8f, 1f));
            CreateText(panel.transform, "SubtitleText", "Adaptive MR Emergency Response Training", 28, new Vector2(0f, 72f), new Vector2(700f, 70f), FontStyle.Normal, new Color(0.57f, 0.59f, 0.59f, 1f));

            CreateButton(panel.transform, "StartButton", "Start", new Vector2(0f, -25f), () => _gameManager.OpenRoomPreparation());
            CreateButton(panel.transform, "QuitButton", "Quit", new Vector2(0f, -135f), () => _gameManager.QuitApplication());

            return panel;
        }

        private GameObject CreateRoomPreparationPanel(Transform parent)
        {
            var panel = CreatePanel(parent, "RoomPreparationPanel", new Vector2(860f, 580f));

            CreateText(panel.transform, "HeaderText", "Prepare Training Space", 62, new Vector2(0f, 170f), new Vector2(720f, 100f), FontStyle.Bold, new Color(0.82f, 0.82f, 0.8f, 1f));
            CreateText(panel.transform, "DescriptionText", "Select an existing room setup or create a new one before starting training.", 28, new Vector2(0f, 86f), new Vector2(720f, 90f), FontStyle.Normal, new Color(0.57f, 0.59f, 0.59f, 1f));

            CreateButton(panel.transform, "UseExistingButton", "Use Existing Room Setup", new Vector2(0f, -10f), () => _gameManager.UseExistingRoomSetup());
            CreateButton(panel.transform, "CreateNewButton", "Create New Room Setup", new Vector2(0f, -120f), () => _gameManager.CreateNewRoomSetup());
            CreateButton(panel.transform, "BackButton", "Back", new Vector2(0f, -230f), () => _gameManager.EnterDashboard(), new Vector2(320f, 78f));

            panel.SetActive(false);
            return panel;
        }

        private void CreateStatusPanel(Transform parent)
        {
            var statusPanel = new GameObject("StatusPanel", typeof(RectTransform), typeof(Image));
            statusPanel.transform.SetParent(parent, false);

            var rectTransform = statusPanel.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, 36f);
            rectTransform.sizeDelta = new Vector2(860f, 92f);

            var image = statusPanel.GetComponent<Image>();
            image.color = new Color(0.11f, 0.12f, 0.13f, 0.92f);

            _statusText = CreateText(statusPanel.transform, "StatusText", string.Empty, 24, Vector2.zero, new Vector2(780f, 56f), FontStyle.Normal, new Color(0.62f, 0.64f, 0.64f, 1f));
            _statusText.alignment = TextAnchor.MiddleCenter;
        }

        private static void CreateBackground(Transform parent)
        {
            var background = new GameObject("Background", typeof(RectTransform), typeof(Image));
            background.transform.SetParent(parent, false);

            var rectTransform = background.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            var image = background.GetComponent<Image>();
            image.color = new Color(0.06f, 0.065f, 0.075f, 1f);
        }

        private static void EnsureEventSystem()
        {
            if (Object.FindFirstObjectByType<EventSystem>() != null)
            {
                return;
            }

            var eventSystemObject = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            eventSystemObject.transform.SetParent(null, false);
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 size)
        {
            var panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);

            var rectTransform = panel.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = size;
            rectTransform.anchoredPosition = new Vector2(0f, 28f);

            var image = panel.GetComponent<Image>();
            image.color = new Color(0.12f, 0.125f, 0.135f, 0.95f);

            return panel;
        }

        private static Text CreateText(Transform parent, string objectName, string content, int fontSize, Vector2 anchoredPosition, Vector2 size, FontStyle fontStyle, Color color)
        {
            var textObject = new GameObject(objectName, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = size;
            rectTransform.anchoredPosition = anchoredPosition;

            var text = textObject.GetComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>(BuiltinFontName);
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = color;

            return text;
        }

        private static void CreateButton(Transform parent, string objectName, string label, Vector2 anchoredPosition, UnityEngine.Events.UnityAction onClick, Vector2? sizeOverride = null)
        {
            var buttonObject = new GameObject(objectName, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);

            var rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = sizeOverride ?? new Vector2(440f, 84f);
            rectTransform.anchoredPosition = anchoredPosition;

            var image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.2f, 0.21f, 0.22f, 1f);

            var button = buttonObject.GetComponent<Button>();
            var colors = button.colors;
            colors.normalColor = new Color(0.2f, 0.21f, 0.22f, 1f);
            colors.highlightedColor = new Color(0.28f, 0.29f, 0.3f, 1f);
            colors.pressedColor = new Color(0.16f, 0.17f, 0.18f, 1f);
            colors.selectedColor = colors.highlightedColor;
            colors.disabledColor = new Color(0.12f, 0.12f, 0.12f, 0.7f);
            button.colors = colors;
            button.onClick.AddListener(onClick);

            CreateText(buttonObject.transform, "Label", label, 28, Vector2.zero, rectTransform.sizeDelta, FontStyle.Bold, new Color(0.82f, 0.82f, 0.8f, 1f));
        }
    }
}
