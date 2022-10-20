using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private static UIManager instance;

    #region Menu Variable

    [Header("스와이프 메뉴")]
    [SerializeField] GameObject panelMenuLeft;
    [SerializeField] GameObject panelMenuRight;
    [SerializeField] GameObject btn_Scroll;

    [Header("오른쪽 스와이프 메뉴")]
    [SerializeField] GameObject btn_base;
    [SerializeField] GameObject btn_parent;
    [SerializeField] FurnitureData[] furnitureDatas;

    [Header("체크버튼")]
    [SerializeField] GameObject btn_select;

    [Header("사운드")]
    [SerializeField] AudioClip swipeClip;
    [SerializeField] AudioClip leftBtnClickClip;
    [SerializeField] AudioClip rightBtnClickClip;
    #endregion

    #region sport play variable
    [Header("Sport Play")]
    [SerializeField] Image aimImg;
    [SerializeField] LayerMask ballLayer;
    [SerializeField] Button hitBtn;
    [SerializeField] Camera mainCam;
    RaycastHit hits;

    #endregion
    //가구
    List<GameObject> furnitureList = new List<GameObject>();

    #region SwipeMenu Anim
    public void SwipeMenuLeft()//왼쪽메뉴 애니메이션
    {
        if (panelMenuLeft != null)
        {
            Animator leftMenuAnimator = panelMenuLeft.GetComponent<Animator>();
            if (leftMenuAnimator != null)
            {
                bool curState = leftMenuAnimator.GetBool("Show");
                leftMenuAnimator.SetBool("Show", !curState);
                if (curState)//펼쳐진 상태라면
                {
                    SwipeMenuRightHide();
                }
                StartCoroutine(ChangeSwipeMenuLeftBtnImg());
                //audioSource.PlayOneShot(swipeClip);
                SoundManager.instance.SFXPlay("swipe", swipeClip);
            }
        }
    }

    IEnumerator ChangeSwipeMenuLeftBtnImg()//왼쪽메뉴 화살표 방향 바꾸기
    {
        if (btn_Scroll != null)
        {
            RectTransform btn_RTS = btn_Scroll.GetComponent<RectTransform>();
            if (btn_RTS != null)
            {
                yield return new WaitForSeconds(0.3f);
                btn_RTS.localScale = btn_RTS.localScale * -1;
            }
        }
    }

    public void SwipeMenuRightShow()
    {
        if (panelMenuRight != null)
        {
            Animator rightMenuAnimator = panelMenuRight.GetComponent<Animator>();
            if (rightMenuAnimator != null && !rightMenuAnimator.GetBool("Show"))
            {
                rightMenuAnimator.SetBool("Show", true);
                CheckButtonActive(true);
            }
        }
    }

    public void SwipeMenuRightHide()
    {
        if (panelMenuRight != null)
        {
            Animator rightMenuAnimator = panelMenuRight.GetComponent<Animator>();
            if (rightMenuAnimator != null && rightMenuAnimator.GetBool("Show"))
            {
                rightMenuAnimator.SetBool("Show", false);
                CheckButtonActive(false);
            }

        }
    }
    #endregion

    #region Setup right menu
    void RightMenuSetUp()
    {
        for(int i = 0; i < furnitureDatas.Length; i++)
        {
            GameObject btn = Instantiate(btn_base, btn_parent.transform.position, Quaternion.identity, btn_parent.transform);
            btn.name = "FurnitureBtn_" + i;
            furnitureDatas[i].id = i;

            int index = i;
            btn.GetComponent<FurnitureBtn>().Setup(furnitureDatas[i]);
            btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    InstaniateSelectedFurniture(index);
                    RightMenuSound();
                }
            );
            furnitureList.Add(btn);
        }
    }

    void RightMenuSound()
    {
        SoundManager.instance.SFXPlay("RightBtn", rightBtnClickClip);
    }

    void InstaniateSelectedFurniture(int id)
    {
        if(Place.Instance != null)
        {
            if(Place.Instance.selectFurniture != null)
            {
                Destroy(Place.Instance.selectFurniture);
            }
            Place.Instance.selectFurniture = Instantiate(furnitureDatas[id].furniturePrefab);
        }
    }

    public void ShowRightMenuItem(int kind)
    {
        for (int i = 0; i < furnitureDatas.Length; i++)
        {
            if((int)furnitureDatas[i].kind == kind)
            {
                btn_parent.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                btn_parent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        SoundManager.instance.SFXPlay("LeftBtnClick", leftBtnClickClip);
    }

    public void ShowAllFurniture()
    {
        for (int i = 0; i < furnitureList.Count; i++)
        {
            furnitureList[i].SetActive(true);
        }
        SoundManager.instance.SFXPlay("LeftBtnClick", leftBtnClickClip);
    }
    #endregion

    #region Funiture select button
    void CheckButtonActive(bool state)
    {
        btn_select.SetActive(state);
    }
    #endregion

    #region sport play
    void SportPlayUISetUp(bool state)
    {
        aimImg.gameObject.SetActive(state);
        hitBtn.gameObject.SetActive(state);
    }
    public void Aim()
    {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hits, ballLayer))
        {
            if (hits.transform.tag == "Ball")   //칠 수 있는 흰색 볼이 타겟 될 때
            {
                SportPlayUISetUp(true);
                GameObject whiteBall = hits.collider.gameObject;
                hitBtn.onClick.AddListener(() => //hits 매개변수를 기존 버튼의 onClick으로 넘길 수가 없어 추가
                        whiteBall.GetComponent<WhiteBall>().Hit(hits, mainCam)
                    );
                aimImg.color = Color.red;
                if (!whiteBall.GetComponent<WhiteBall>().isRoll)
                {
                    hitBtn.interactable = true; //칠 수 있는 버튼 상호작용 가능
                }
            }
            else //흰색 볼이 타겟 안될 때
            {
                aimImg.color = Color.white;
                SportPlayUISetUp(false);
            }
        }
    }
    #endregion

    private void Start()
    {
        RightMenuSetUp();
        SportPlayUISetUp(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Aim();
    }
}
