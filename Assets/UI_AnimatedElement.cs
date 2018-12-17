using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AnimatedElement : MonoBehaviour {

    public Animator m_animator;
    public Text m_text;
    public Image m_image;

    private UI_Manager m_ui_manager;

	public void triggerAnimatedText(string animation_trigger, string output_text, Sprite output_sprite)
    {
        //reset the dismiss bool in case it was previously used to close the element
        m_animator.SetBool("dismiss", false);

        //set the values of the output elements
        m_text.text = output_text;

        if (output_sprite != null)
        {
            m_image.sprite = output_sprite;
        }

        //animate in
        m_animator.SetTrigger(animation_trigger);

        if(m_ui_manager == null)
        {
            m_ui_manager = UI_Manager.GetInstance();
        }
    }

    public void dismissEarly()
    {
        m_animator.SetBool("dismiss", true);
    }

    public void onExitAnimationCompleted()
    {
        m_ui_manager.onElementAnimationCompleted();
    }
}
