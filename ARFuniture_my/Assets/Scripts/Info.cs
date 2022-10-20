using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Info : MonoBehaviour
{
    const float speed = 6f;

    [SerializeField]
    Transform sectionInfo;

    Vector3 desiredScale = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        sectionInfo.localScale = Vector3.Lerp(sectionInfo.localScale, desiredScale, Time.deltaTime * speed) ;

    }

    public void OpenInfo()
    {
        desiredScale = Vector3.one;
    }

    public void CloseInfo()
    {
        desiredScale = Vector3.zero;
    }
}
