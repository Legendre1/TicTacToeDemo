using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AnimatedElement : MonoBehaviour {

    public Animator m_animator;
    public Text m_text;
    public Image m_image;

	public void triggerAnimatedText(string animation_trigger, string output_text, Sprite output_sprite)
    {
        m_animator.SetTrigger(animation_trigger);
        m_text.text = output_text;

        if(output_sprite != null)
        {
            m_image.sprite = output_sprite;
        }
    }
}
