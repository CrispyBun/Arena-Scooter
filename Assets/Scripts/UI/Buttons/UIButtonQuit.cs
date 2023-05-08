using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonQuit : UIButton
{
    protected override void OnButtonClick()
    {
        Application.Quit();
    }
}
