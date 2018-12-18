using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modal_Base : MonoBehaviour {

    public Animator m_animator;

    protected const string ANIMATOR_STATE_BOOL = "active";

    public virtual void onExitAnimationCompleted()
    { 
        //stub implementation for animation call
    }

    protected void dismissModal()
    {
        //Animate out
        m_animator.SetBool(ANIMATOR_STATE_BOOL, false);
    }
}
