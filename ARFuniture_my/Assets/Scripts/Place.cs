using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Place : MonoBehaviour
{
    #region Variable
    /// <summary>
    /// ╫л╠шео
    /// </summary>
    public static Place Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Place>();
            }
            return instance;
        }
    }

    private static Place instance;

    #region Show & Set selected Furniture Variable
    [Header("Show & Set selected Furniture Variable")]
    public ARRaycastManager arRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public Transform pool;

    Vector2 camCenter;
    public GameObject selectFurniture;

    [SerializeField] AudioClip setFurnitureClip;
    #endregion

    #region Furniture touch rotation & Scale Varialbe
    [Header("Furniture touch rotation & Scale Varialbe")]
    float initialDistance;
    Vector2 prevPosition_FurniRot;
    Vector3 initialScale;
    #endregion
    #endregion

    /*public void Select(int type)
    {
        if (selectFurniture != null)
        {
            Destroy(selectFurniture);
            selectFurniture = null;
        }
        if (type == 0)
        {
            selectFurniture = Instantiate(furniturePrefab[0]);
        }
        else if (type == 1)
        {
            selectFurniture = Instantiate(furniturePrefab[1]);
        }
    }*/

    #region Show & Set selected Furniture
    public void Set()
    {
        if(selectFurniture != null)
        {
            SoundManager.instance.SFXPlay("Set Funiture", setFurnitureClip);
            //selectFurniture.transform.localScale = new Vector3(100, 100, 100);
            selectFurniture.transform.SetParent(pool);
            selectFurniture = null;
        }
    }

    void ShowselectedFurniture()
    {
        if (selectFurniture != null)
        {
            camCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            if (arRaycastManager.Raycast(camCenter, hits, TrackableType.PlaneWithinPolygon))
            {
                ARPlane arPlane = hits[0].trackable.GetComponent<ARPlane>();
                if (arPlane != null)
                {
                    selectFurniture.transform.position = hits[0].pose.position;
                    //selectFurniture.transform.localScale = Vector3.one * 100;
                }
            }
            else
            {
                selectFurniture.transform.localScale = Vector3.zero;
            }
        }
    }
    #endregion

    #region Furniture touch rotation & Scale
    void FurnitureZoomInOut()
    {
        if (selectFurniture != null)
        {
            if(Input.touchCount == 1)//Rotation
            {
                Touch touchZero = Input.GetTouch(0);
                if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled)
                {
                    return;
                }
                if (touchZero.phase == TouchPhase.Began)
                {
                    prevPosition_FurniRot = Input.GetTouch(0).position;
                }
                else
                {
                    var deltaY = -(Input.mousePosition.x - prevPosition_FurniRot.x) * 0.1f;

                    selectFurniture.transform.Rotate(0, deltaY, 0, Space.Self);
                    prevPosition_FurniRot = Input.GetTouch(0).position;
                }
                
            }
            if (Input.touchCount >= 2)//Scale
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if(touchZero.phase == TouchPhase.Ended ||touchZero.phase == TouchPhase.Canceled
                    || touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
                {
                    return;
                }

                if(touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
                {
                    initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    initialScale = selectFurniture.transform.localScale;
                }
                else
                {
                    float currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    if (Mathf.Approximately(initialDistance, 0)) return;

                    var factor = currentDistance / initialDistance;
                    selectFurniture.transform.localScale = initialScale * factor;
                }
            }
        }
    }
    #endregion

    private void Update()
    {
        FurnitureZoomInOut();
        ShowselectedFurniture();
    }
}
