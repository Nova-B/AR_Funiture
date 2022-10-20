using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gaze : MonoBehaviour
{
    List<Info> infos = new List<Info>();
    public Text debug;
    bool hasInfo;
    GameObject infoObj;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindInfo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FindInfo()
    {
        while(true)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            {
                GameObject go = hit.collider.gameObject;
                if (go.CompareTag("hasInfo"))
                {
                    Info info = go.GetComponentInChildren<Info>();
                    if (info != null)
                    {
                        info.OpenInfo();
                        hasInfo = true;
                    }
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
