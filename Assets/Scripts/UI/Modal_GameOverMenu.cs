using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modal_GameOverMenu : Modal_Base
{

    #region Member Vars

    public Text m_game_description_text;
    public Image[] m_winning_sprite_dancers;

    private const string DRAW_TEXT = "Draw game...nobody wins.";
    private const string WINNER_TEXT = "{0}      Wins!";

    #endregion

    #region Enter/Exit Methods

    public void triggerModal(bool winner_declared, string winning_player, Sprite winning_sprite)
    {
        //Set display based on win parameters
        if(!winner_declared)
        {
            m_game_description_text.text = DRAW_TEXT;

            foreach(Image img in m_winning_sprite_dancers)
            {
                img.gameObject.SetActive(false);
            }
        }
        else //winner declared
        {
            m_game_description_text.text = string.Format(WINNER_TEXT, winning_player);

            foreach (Image img in m_winning_sprite_dancers)
            {
                img.gameObject.SetActive(true);
                img.sprite = winning_sprite;
            }
        }

        //Animate in
        m_animator.SetBool("active", true);
    }

    #endregion

    #region Button Methods

    public void onPlayAgain()
    {
        dismissModal();
        TTTGameManager.GetInstance().playAgain();
    }

    public void OnGuitGame()
    {
        dismissModal();
        ApplicationLifecycleManager.GoToMainMenu();
    }

    #endregion

}
