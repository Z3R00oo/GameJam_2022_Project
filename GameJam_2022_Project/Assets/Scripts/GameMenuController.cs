using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public GameObject titleMenu;
    public GameObject aboutMenu;

    public AudioSource buttonSoundFX;

    private int randomScene;

    private void Start()
    {
        randomScene = Random.Range(1, SceneManager.sceneCount);

        
    }


    public void PlayGame()
    {
        buttonSoundFX.Play();
        SceneManager.LoadScene(randomScene);
        
    }

    public void AboutSession()
    {
        buttonSoundFX.Play();

        titleMenu.SetActive(false);
        aboutMenu.SetActive(true);
    }

    public void ExitGame()
    {
        buttonSoundFX.Play();

        Application.Quit();
    }

    public void BackButton()
    {
        buttonSoundFX.Play();

        aboutMenu.SetActive(false);
        titleMenu.SetActive(true);
    }
}
