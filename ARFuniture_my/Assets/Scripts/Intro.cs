using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    IEnumerator IntroAnim()
    {
        transform.localScale = 0.4f * Vector3.one;

        LeanTween.scale(gameObject, Vector3.one, 2f).setEase(LeanTweenType.easeInOutBack);
        yield return new WaitForSeconds(2f);
        float alpha = 1;
        gameObject.GetComponent<Image>().CrossFadeAlpha(0, 1.8f, false);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
        yield return null;
    }

    private void Start()
    {
        StartCoroutine("IntroAnim");
    }
}
