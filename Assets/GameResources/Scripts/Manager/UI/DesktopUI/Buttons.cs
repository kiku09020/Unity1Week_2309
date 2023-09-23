using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameController.UI
{
    public class Buttons : MonoBehaviour
    {

        //--------------------------------------------------

        public void RetryButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitButton()
        {
            SceneManager.LoadScene("DesktopScene");
		}

        public void NextButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}