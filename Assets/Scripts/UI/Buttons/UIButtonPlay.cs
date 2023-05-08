using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonPlay : UIButton
{
    protected override void OnButtonClick()
    {
        SceneManager.LoadScene("Arena");
    }
}
