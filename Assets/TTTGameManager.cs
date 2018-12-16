using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTGameManager : MonoBehaviour {

    public Modal_MarkSelector m_mark_selector_modal;
    
	// Use this for initialization
	void Start () {

        m_mark_selector_modal.triggerModal(1);


    }
	
	
}
