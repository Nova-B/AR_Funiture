using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FurnitureBtn : MonoBehaviour
{
    [SerializeField]
    private Image btnIcon;
    [SerializeField]
    private TextMeshProUGUI btnText;
    [SerializeField]
    int id;

    public void Setup(FurnitureData btnData)
    {
        btnIcon.sprite = btnData.furnitureSprite;
        btnText.text = btnData.furnitureName;
        id = btnData.id;
    }
}
