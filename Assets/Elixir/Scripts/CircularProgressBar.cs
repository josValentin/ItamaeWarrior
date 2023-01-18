using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elixir {

    public class CircularProgressBar : MonoBehaviour {
        public CanvasGroup group;
        public float minTimePettion = 2f;


        static int  showCounter = 0;
        float       alphaTimer = 0;
        bool        neverReachMinimous=true;

        float timer = 0;
        float angle = 0;
        public Image image;
        // Start is called before the first frame update
        public void Show() {
            showCounter++;
            gameObject.SetActive(true);
            group.interactable = true;
        }
        public void Hide() {
            showCounter--;
        }

        // Update is called once per frame
        void Update() {
            if (showCounter>0 || neverReachMinimous) {
                alphaTimer = alphaTimer + Time.deltaTime;
                if(alphaTimer> minTimePettion) neverReachMinimous = false;
            } else {
                alphaTimer = alphaTimer - Time.deltaTime;
                if (alphaTimer <= 0) {
                    alphaTimer = 0;
                    neverReachMinimous = true;
                    gameObject.SetActive(false);
                    group.interactable = false;
                }
            }
            group.alpha = alphaTimer;

            // Controla la animacion del loop.
            float ltimer = timer < 1.5f ? timer : (2.5f - timer);
            image.fillAmount = Mathf.Lerp(0, 1, ltimer);
            //        image.transform.Rotate( new Vector3(0,0,240) * Time.deltaTime );
            float langle = -360 * (timer >= 1.5f ? (2.5f - timer) : 0);
            image.transform.rotation = Quaternion.Euler(0, 0, angle + langle);
            timer += Time.deltaTime;
            angle += 180 * Time.deltaTime;
            if (timer > 2.5f) timer -= 3.0f;
        }
    }
}
