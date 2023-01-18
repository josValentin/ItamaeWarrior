using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    void Start()
    {
#if !UNITY_STANDALONE
        ResizeSpriteToScreen();
#endif
        gameObject.SetActive(false);

    }

    void ResizeSpriteToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null) return;


        transform.localScale = new Vector3(1, 1, 1);

        var width = sr.sprite.bounds.size.x;
        var height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 targetScale = Vector3.one;
        targetScale.x = worldScreenWidth / width;
        targetScale.y = worldScreenHeight / height;

        transform.localScale = targetScale;
    }

    void MakeDamage()
    {
        if (GameManager.Instance.sushiTempList.Count > 0)
        {
            for (int i = GameManager.Instance.sushiTempList.Count - 1; i >= 0; i--)
            {
                GameManager.Instance.sushiTempList[i].Die();
            }
        }

        player.isOnSpecialAttack = false;
        player.ActivePlayer();

        Time.timeScale = 1;
    }

    void Disappear()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ReactiveSpecialAttackBehaviour();
    }
}
