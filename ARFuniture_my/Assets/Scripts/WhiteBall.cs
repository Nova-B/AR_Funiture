using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBall : MonoBehaviour
{
    Rigidbody rigid;
    AudioSource audioSource;

    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip scoreClip;

    float timer = 0f;
    float stopCoolTime = 1f;

    public List<RedBall> redBalls = new List<RedBall>();
    public bool getScore;
    static int score = 0;

    public bool isRoll
    {
        get
        {
            //float xSpeed = rigid.velocity.x;
            //float zSpeed = rigid.velocity.z;
            //Vector3 speedVec = new Vector3(xSpeed, 0, zSpeed);
            //float speed = Mathf.Abs(speedVec.magnitude);

            if (rigid.velocity.magnitude < 0.01)
            {
                timer += Time.deltaTime;
                if (timer > stopCoolTime)
                {
                    return false;
                }
                return true;
            }

            timer = 0f;
            return true;


            //if(speed < 0.01 && !completeStop)
            //{
            //    Debug.Log("Stop1");
            //    lastStopTime = Time.time;
            //    completeStop = true;
            //}
            //else if (speed < 0.01 && completeStop)
            //{
            //    if(Time.time > lastStopTime + stopCoolTime)
            //    {
            //        Debug.Log("Stop");
            //        completeStop = false;
            //        return false;
            //    }
            //    return true;
            //}
            //else
            //{
            //    Debug.Log("Roll");
            //    completeStop = false;
            //    return true;
            //}
        }
        private set { }
    }
    public void Score()
    {
        if (redBalls[0].isHitted && redBalls[1].isHitted)
        {
            if (!getScore)
            {
                score++;
                //scoreText.text = "Score : " + score;
            }
            getScore = true;
        }
        else
        {
            getScore = false;
        }
    }

    public void Hit(RaycastHit hit, Camera mainCam)
    {
        audioSource.PlayOneShot(hitClip);
        rigid.AddForceAtPosition(mainCam.transform.forward * 0.5f, hit.point, ForceMode.Impulse); //카메라 방향으로 Hit 점에 힘을 가함
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }


    private void Start()
    {
        //rigid.AddForceAtPosition(transform.right * 100, transform.position, ForceMode.Impulse);
    }

    private void Update()
    {
        if (isRoll)
        {
            Score();
        }
        else
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }
}
