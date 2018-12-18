using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modal_TwoButtonPrompt : Modal_Base
{

    #region Member Vars

    public Text m_title_text;
    public Text m_description_text;

    public Text m_button_1_text;
    public Text m_button_2_text;

    private TTTGameManager.VoidCallback m_button_1_callback;
    private TTTGameManager.VoidCallback m_button_2_callback;

    #endregion

    #region Entry/Exit Methods

    public void triggerModal(string title, string description, string b1_text, string b2_text,
                                TTTGameManager.VoidCallback b1_callback, TTTGameManager.VoidCallback b2_callback)
    {
        m_title_text.text = title;
        m_description_text.text = description;

        m_button_1_text.text = b1_text;
        m_button_2_text.text = b2_text;

        m_button_1_callback = b1_callback;
        m_button_2_callback = b2_callback;

        //Animate in
        m_animator.SetBool("active", true);
    }

    public void onButton1Pressed()
    {
        m_button_1_callback();
        dismissModal();
    }

    public void onButton2Pressed()
    {
        m_button_2_callback();
        dismissModal();
    }

    #endregion

}
