using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour {

    public enum PlayerUIIndex
    { 
        Player1 = 0,
        Player2 = 1
    }
    #region Member Vars

    public UI_AnimatedElement[] m_animated_elements;


    //constants
    private const string ENTRY_ANIMATION_TRIGGER = "enter";

    #endregion

    #region Public Access

    public void triggerAnimatedText(PlayerUIIndex element_index, string output_text, Sprite output_sprite = null)
    {
        m_animated_elements[(int)element_index].triggerAnimatedText(ENTRY_ANIMATION_TRIGGER, output_text, output_sprite);
    }

    #endregion



}
