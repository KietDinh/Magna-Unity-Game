using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameMenu : MonoBehaviour
{
    // if no private or public, default is private
    GameObject pausedMenu;
    GameObject loseMenu;
    GameObject winMenu;
    GameObject fallCheck;
    GameObject soundMenu;
    bool menu_isActive;
    public bool isPaused;

    [SerializeField] GameObject DefaultMusic;
    [SerializeField] GameObject LoseMusic;
    bool loseCheck = false;
    GameObject player;
    GameObject boss;
    // Update is called once per frame
    void Start()
    {
        isPaused = false;
        menu_isActive = false;
        player = GameObject.FindGameObjectsWithTag("Player")[0].gameObject;
        boss = GameObject.FindGameObjectsWithTag("Boss")[0].gameObject;

        pausedMenu = transform.GetChild(0).gameObject;
        winMenu = transform.GetChild(1).gameObject;
        loseMenu = transform.GetChild(2).gameObject;
        fallCheck = transform.GetChild(3).gameObject;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !loseMenu.activeSelf && !winMenu.activeSelf)
        {
            Debug.Log(loseMenu.activeSelf);
            Debug.Log(winMenu.activeSelf);
            if (isPaused)
            {
                Resume();
            }
            else
            {
                isPaused = true;

                pausedMenu.SetActive(true);

                Time.timeScale = 0f;
            }
        }
        else if ((!player && !winMenu.activeSelf))
        {
            /* 
             * This if triggers when
                    player is death and winmenu is not active 
            */

            loseMenu.SetActive(true);
        }
        else if (player)
        {
            /* 
             * This if triggers when
                    player position is below a certain point (falling)
            */
            if ((player.transform.position.y <= fallCheck.transform.position.y))
            {
                Destroy(DefaultMusic);
                if(!loseCheck)
                {
                    Instantiate(LoseMusic);
                    loseCheck = true;
                }
                
                loseMenu.SetActive(true);
            }
               

            else if (!boss)
            {
                Invoke("winning", 3f);
            }
        }

    }
    void winning()
    {
        Time.timeScale = 0;
        winMenu.SetActive(true);
    }
    public void Resume()
    {
        isPaused = false;
        pausedMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void RePlay()
    {
        cleanUp();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void cleanUp()
    {
        var bullet = GameObject.FindGameObjectsWithTag("Boss Bullet");
        foreach (var i in bullet)
        {
            Destroy(i);
        }
        var bulletDestroy = GameObject.FindGameObjectsWithTag("Bullet Destroyed");
        foreach (var i in bulletDestroy)
        {
            Destroy(i);
        }

    }
}
