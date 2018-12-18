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

    #region Classes, Enums

    public enum PlayerUIIndex
    { 
        Player1 = 0,
        Player2 = 1
    }

    #endregion

    #region Member Vars

    public GameObject m_input_blocker_go;//a big button that blocks input to lower layers while an element is active.
    public Modal_GameOverMenu m_gameover_modal;
    public Modal_TwoButtonPrompt m_binary_choice_modal;
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

    public void presentGameOverModal(bool winner_declared, float delay, string winning_player = "", Sprite winning_sprite = null)
    {
        StartCoroutine(EpresentGameOverModalAfterDelay(winner_declared, delay, winning_player, winning_sprite));
    }



    public void presentBinaryChoiceModal(string title, string description, string b1_text, string b2_text,
                                TTTGameManager.VoidCallback b1_callback, TTTGameManager.VoidCallback b2_callback)
    {
        m_binary_choice_modal.triggerModal(title, description, b1_text, b2_text, b1_callback, b2_callback);
    }

    #endregion

    #region Coroutines

    private IEnumerator EpresentGameOverModalAfterDelay(bool winner_declared, float delay, string winning_player = "", Sprite winning_sprite = null)
    {
        yield return new WaitForSeconds(delay);
        m_gameover_modal.triggerModal(winner_declared, winning_player, winning_sprite);
    }

    #endregion


}
