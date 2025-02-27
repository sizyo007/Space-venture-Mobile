using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI laserCountText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    public void UpdateScoreUi(int val) => scoreText.text = val.ToString();
    public void UpdateLaserCountUi(int val) => laserCountText.text = val + "/5";
}
