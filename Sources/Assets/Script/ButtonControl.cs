using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour {
    public ButtonControl.ButtonType bt;
    private void OnMouseDown()
    {
        if (bt == ButtonControl.ButtonType.btnPlay)
            transform.localScale = new Vector4(3.5f, 4.5f);

        if (bt == ButtonControl.ButtonType.btnExit)
            transform.localScale = new Vector4(6f, 3.5f);

        if (bt == ButtonControl.ButtonType.btnEasy)
            transform.localScale = new Vector4(3.5f, 3.5f);

        if (bt == ButtonControl.ButtonType.btnNormal)
            transform.localScale = new Vector4(3.5f, 3.5f);

        if (bt == ButtonControl.ButtonType.btnHard)
            transform.localScale = new Vector4(3.5f, 3.5f);

        if (bt == ButtonControl.ButtonType.btnMenu)
            transform.localScale = new Vector4(4f, 5f);
        if (bt == ButtonControl.ButtonType.btnAbout)
            transform.localScale = new Vector4(1.25f,1.25f);
    }
    private void OnMouseUp()
    {
        
        if (bt == ButtonControl.ButtonType.btnPlay)
            Application.LoadLevel("ChooseLevel");

        if (bt == ButtonControl.ButtonType.btnExit)
            Application.Quit();

        if (bt == ButtonControl.ButtonType.btnEasy)
            Application.LoadLevel("1");

        if (bt == ButtonControl.ButtonType.btnNormal)
            Application.LoadLevel("2");

        if (bt == ButtonControl.ButtonType.btnHard)
            Application.LoadLevel("3");

        if (bt == ButtonControl.ButtonType.btnMenu)
            Application.LoadLevel("StartMenu");

        if (bt == ButtonControl.ButtonType.btnAbout)
            Application.LoadLevel("About");

    }


    public enum ButtonType
    {
        btnPlay,
        btnExit,
        btnEasy,
        btnNormal,
        btnHard,
        btnMenu,
        btnAbout
    }
}
