using UnityEngine;
using UnityEngine.Events;

public class SceneTransitor : MonoBehaviour
{
    static bool firstTimeShow = true;
    [SerializeField]
    private UnityEngine.UI.Image imgItawaLogo;

    private void Start()
    {
        if (firstTimeShow)
        {
            imgItawaLogo.enabled = false;
            firstTimeShow = false;
        }
    }

    public void CoverTransition()
    {
        if (!imgItawaLogo.enabled)
            imgItawaLogo.enabled = true;

        GetComponent<Animator>().SetTrigger("Cover");
    }

    [SerializeField] private UnityEvent OnTransitionFinishEvent;

    public void OnTransitionFinish()
    {
        OnTransitionFinishEvent?.Invoke();
    }
}
