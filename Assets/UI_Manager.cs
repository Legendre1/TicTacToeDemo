using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour {

    #region Singleton Access

    private static UI_Manager s_instance;

    public static UI_Manager GetInstance()
    {
        return s_instance;
    }

    #endregion

    public enum PlayerUIIndex
    { 
        Player1 = 0,
        Player2 = 1
    }
    #region Member Vars

    public GameObject m_input_blocker_go;//a big button that blocks input to lower layers while an element is active.
    public UI_AnimatedElement[] m_animated_elements;


    //constants
    private const string ENTRY_ANIMATION_TRIGGER = "enter";

    #endregion

    private void Start()
    {
        s_instance = this;
    }

    #region Public Access

    public void triggerAnimatedText(PlayerUIIndex element_index, string output_text, Sprite output_sprite = null)
    {
        m_input_blocker_go.SetActive(true);
        m_animated_elements[(int)element_index].triggerAnimatedText(ENTRY_ANIMATION_TRIGGER, output_text, output_sprite);
    }

    public void onDismissButtonPressed()
    {
        //the invisible full screen button dismisses any active animated elements
        foreach (UI_AnimatedElement element in m_animated_elements)
        {
            element.dismissEarly();
        }
    }

    public void onElementAnimationCompleted()
    {
        m_input_blocker_go.SetActive(false);
    }

    #endregion



}
