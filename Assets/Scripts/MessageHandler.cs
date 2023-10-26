using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageHandler : MonoBehaviour
{
    private float fadeAwayTime;
    private TextMeshProUGUI text;

    public MessageHandler(float fadeAwayTime, TextMeshProUGUI text)
    {
        this.fadeAwayTime = fadeAwayTime;
        this.text = text;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeAwayTime > 0)
        {
            fadeAwayTime -= Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, fadeAwayTime);
        }
    }

    void showMessage(string message)
    {

    }
}
