using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Elixir
{

    public class DialogWindow : MonoBehaviour
    {
        public Text title;
        public Text message;
        public Text button;
        public Text buttonOK;
        public Button okButton;
        callback onClose;
        callback onOk;
        GameObject temporaryEventSystem;

        public delegate void callback();

        public void Show(string title, string message, string button, callback onClose, string buttonOK = "OK", callback onOk = null) {
            if(EventSystem.current == null) {
                temporaryEventSystem = new GameObject("EventSystem");
                temporaryEventSystem.AddComponent<EventSystem>();
                temporaryEventSystem.AddComponent<StandaloneInputModule>();
                GameObject.DontDestroyOnLoad(temporaryEventSystem);
            }


            this.title.text = title;
            this.message.text = message;
            this.button.text = button;
            this.onClose = onClose;
            this.buttonOK.text = buttonOK;
            this.onOk = onOk;
            okButton.gameObject.SetActive(onOk != null);
            gameObject.SetActive(true);
            ElixirController.OnOpenDialog?.Invoke();

        }

        public void OnClose() {
            if (temporaryEventSystem != null) { Destroy(temporaryEventSystem); temporaryEventSystem = null; }
            ElixirController.OnCloseDialog?.Invoke();
            gameObject.SetActive(false);
            onClose?.Invoke();
        }
        public void OnOk() {
            if (temporaryEventSystem != null) { Destroy(temporaryEventSystem); temporaryEventSystem = null; }
            ElixirController.OnCloseDialog?.Invoke();
            gameObject.SetActive(false);
            onOk?.Invoke();
        }

    }
}