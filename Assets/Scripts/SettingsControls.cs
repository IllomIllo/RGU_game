using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsControls : MonoBehaviour
{
    public void BackPressed()
    {
      SceneManager.LoadScene("MainMenu");
        
    }
}
