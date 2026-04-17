using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IronSight.Core
{
    public sealed class AppBootstrap : MonoBehaviour
    {
        [SerializeField] private float bootDelaySeconds = 1.5f;
        [SerializeField] private string mainSceneName = "MainScene";

        private Canvas _bootCanvas;

        private void Awake()
        {
            EnsureBootCanvas();
        }

        private void Start()
        {
            StartCoroutine(BootSequence());
        }

        private IEnumerator BootSequence()
        {
            yield return new WaitForSecondsRealtime(bootDelaySeconds);
            yield return SceneManager.LoadSceneAsync(mainSceneName);
        }

        private void EnsureBootCanvas()
        {
            if (_bootCanvas != null)
            {
                return;
            }

            var existingCanvas = transform.root.GetComponentInChildren<Canvas>();
            if (existingCanvas != null)
            {
                _bootCanvas = existingCanvas;
                return;
            }

            var canvasObject = new GameObject("Canvas_Boot", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(null, false);

            _bootCanvas = canvasObject.GetComponent<Canvas>();
            _bootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _bootCanvas.pixelPerfect = true;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            CreateBackground(canvasObject.transform);
            CreateText(canvasObject.transform, "TitleText", "IronSight MR", 88, new Vector2(0f, 70f), FontStyle.Bold);
            CreateText(canvasObject.transform, "LoadingText", "Initializing system...", 32, new Vector2(0f, -24f), FontStyle.Normal);
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
            image.color = new Color(0.07f, 0.075f, 0.085f, 1f);
        }

        private static void CreateText(Transform parent, string objectName, string content, int fontSize, Vector2 anchoredPosition, FontStyle fontStyle)
        {
            var textObject = new GameObject(objectName, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(900f, 120f);
            rectTransform.anchoredPosition = anchoredPosition;

            var text = textObject.GetComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = objectName == "TitleText"
                ? new Color(0.82f, 0.82f, 0.8f, 1f)
                : new Color(0.56f, 0.58f, 0.58f, 1f);
        }
    }
}
