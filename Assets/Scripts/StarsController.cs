using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator[] stars = GetComponentsInChildren<Animator>();

        for (int i = 0; i < stars.Length; i++)
        {
            float delay = Random.Range(0, 1.5f);
            StartCoroutine(PlayStarAnimation(stars[i], delay));
        }
    }

    IEnumerator PlayStarAnimation(Animator starAnim, float delay)
    {
        yield return new WaitForSeconds(delay);

        starAnim.SetTrigger("Play");
    }
}
