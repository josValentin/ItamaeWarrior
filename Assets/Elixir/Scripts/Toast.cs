using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elixir { 
    public class Toast : MonoBehaviour {
        public struct ToastData {
            public string   text;
            public float    timeToShow;
        }
        Queue<ToastData> toasts = new Queue<ToastData>();

        public Text     text;
        Vector2         positionTarget;
        Vector2         positionOrigin;
//        float           timeToShow = 1;
//        float           speedToShow = 1;
//        float           timeToHide = 0f;
        RectTransform rect;
        void Awake() {
            rect = GetComponent<RectTransform>();
            positionTarget = rect.anchoredPosition = new Vector3(0, 64);
        }

        public void Show(string text, float time=3f) {
            toasts.Enqueue(new ToastData() { text = text, timeToShow = time } );
        }

        private void Update() {
            if(!showingToast && toasts.Count > 0) {
                StartCoroutine(ToastShower(toasts.Dequeue()));
            }
        }

        bool showingToast = false;
        IEnumerator ToastShower(ToastData data) {
            showingToast = true;
            text.text = data.text;
            float timeToShow = 0;
            while (timeToShow < 1) {
                timeToShow += Time.deltaTime * 2;
                rect.anchoredPosition = Vector2.Lerp(new Vector3(0, 64), new Vector3(0, 0), timeToShow);
                yield return null;
            }
            yield return new WaitForSeconds(data.timeToShow);
            timeToShow = 1;
            while (timeToShow > 0) {
                timeToShow -= Time.deltaTime * 2;
                rect.anchoredPosition = Vector2.Lerp(new Vector3(0, 64), new Vector3(0, 0), timeToShow);
                yield return null;
            }
            showingToast = false;
        }


    }

}