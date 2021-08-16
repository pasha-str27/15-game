using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonManager : MonoBehaviour
{
    public GameObject audio;

    public void pauseGame()
    {
        Time.timeScale = 0;
    }

    public void exit()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    public void unPauseGame()
    {
        Time.timeScale = 1;
    }

    public void playAudio()
    {
        Destroy(Instantiate(audio), 3);
    }
}
