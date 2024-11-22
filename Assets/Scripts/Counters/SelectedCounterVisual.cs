 using System;
 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        PlayerInteractions.Instance.OnSelectedCounterChanged += Player_OnOnSelectedCounterChanged;
    }

    private void Player_OnOnSelectedCounterChanged(object sender, PlayerInteractions.OnSelectedCounterChangedEventsArgs e)
    {
        if (e.slectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }

    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }

    }
}
