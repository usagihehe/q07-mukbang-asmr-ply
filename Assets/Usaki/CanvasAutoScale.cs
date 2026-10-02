using UnityEngine;
using UnityEngine.UI;

namespace Usaki
{
    public class CanvasAutoScale : MonoBehaviour
    {
        public CanvasScaler canvasScaler;

        private int lastScreenWidth;
        private int lastScreenHeight;
        private float lastAspectRatio;

        private WaitForSeconds wait = new WaitForSeconds(0.1f);
        public static System.Action OnOrientationChanged;
        private void Reset()
        {
            canvasScaler = GetComponent<CanvasScaler>();
        }

        private void OnEnable()
        {
            UpdateCanvasScale();

            StartCoroutine(CheckOrientationRoutine());

            //if ((float)Screen.width / (float)Screen.height >= 0.6f)
            //    canvasScaler.matchWidthOrHeight = 1;
            //else canvasScaler.matchWidthOrHeight = 0;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Start()
        {
            StartCoroutine(CheckOrientationRoutine());
        }

        private System.Collections.IEnumerator CheckOrientationRoutine()
        {
            while (true)
            {
                yield return wait;

                if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
                {
                    UpdateCanvasScale();
                }
            }
        }

        private void UpdateCanvasScale()
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            lastAspectRatio = (float)Screen.width / (float)Screen.height;

            if (lastAspectRatio >= 0.6f)
                canvasScaler.matchWidthOrHeight = 1;
            else
                canvasScaler.matchWidthOrHeight = 0;
        }
    }
}
