using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour {
    static public ShopPanel shopPanel { get; set; }
    public GameObject gameBottomPanel;
    public GameObject dragPanel;

    private void Awake()
    {
        shopPanel = this;
    }
    private void OnEnable()
    {
        gameBottomPanel.SetActive(false);
        dragPanel.SetActive(false);
    }

    private void OnDisable()
    {
        gameBottomPanel.SetActive(true);
        dragPanel.SetActive(true);
    }


}
