using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBall : MonoBehaviour
{
    public bool isHitted;
    [SerializeField] WhiteBall whiteBall;
    AudioSource audioSource;
    [SerializeField] AudioClip hitClip;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        isHitted = false;
    }

    private void Update()
    {
        if (!whiteBall.isRoll)
        {
            Debug.Log("redBall Stop");
            isHitted = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Debug.Log("hit");

            audioSource.PlayOneShot(hitClip);
            isHitted = true;
        }
    }
}
