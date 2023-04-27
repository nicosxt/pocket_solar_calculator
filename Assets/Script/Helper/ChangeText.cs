using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeText : MonoBehaviour
{
    public string originalText;
    public string toggleText;
    public TextMeshProUGUI text;

    bool toggled = false;
    // Start is called before the first frame update
    void Start()
    {
        text.text = originalText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleText(){
        toggled = !toggled;
        text.text = toggled ? toggleText : originalText;
    }

}
