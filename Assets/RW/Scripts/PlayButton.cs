using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

namespace RayWenderlich.SpaceInvadersUnity
{
    public class PlayButton : MonoBehaviour
        {
            public void ButtonMoveScene(string Main)
                {
                    SceneManager.LoadScene(Main);
                }
   
        
        }

}
