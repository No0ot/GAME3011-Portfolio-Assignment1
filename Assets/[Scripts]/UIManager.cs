using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreCount;
    public TMP_Text scansRemaining;
    public TMP_Text extractsRemaining;

    // Update is called once per frame
    void Update()
    {
        scoreCount.text = "Score: " + GameManager.Instance.score;
        scansRemaining.text = "Remaining: " + GameManager.Instance.numScans;
        extractsRemaining.text = "Remaining: " + GameManager.Instance.numExtracts;
    }
}
