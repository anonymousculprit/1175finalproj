using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteBehaviour : MonoBehaviour
{
    SpriteRenderer spr;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    public void ResetTrigger(string str) => anim.ResetTrigger(str);
    public void OnDeath() => StartCoroutine(FadeObject());

    IEnumerator FadeObject()
    {
        float t = 0f;
        float time = 1f;
        Color cCol = spr.color;
        Color transp = new Color(spr.color.r, spr.color.g, spr.color.b, 0);

        while (t < time)
        {
            t += Time.deltaTime;
            spr.color = Color.Lerp(cCol, transp, t / time);
            yield return null;
        }

        gameObject.SetActive(false);
        yield return null;
    }

}
