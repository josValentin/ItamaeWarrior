using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elixir {
    public class LoginWindow : MonoBehaviour
    {
        public CanvasGroup group;
        public InputField login;
        public InputField password;

        public void Show() {
            gameObject.SetActive(true);            
        }

        public void OnLogin() {

        }
        public void OnLostPassword() {

        }
        public void OnCreateAccount() {

        }

    }
}