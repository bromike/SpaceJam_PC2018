using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public class GameState
    {
        public List<GameObject> players = new List<GameObject>();

        public GameState() { }

        public bool win = false;
        public bool atMenu = true;
        public bool gameOn = false;
        public bool startingGame = false;

        public void startGame()
        {
            gameOn = true;
            win = false;
            atMenu = false;
            startingGame = false;
        }

        void winner()
        {
            gameOn = false;
            win = true;
        }

        public void inMenu()
        {
            atMenu = true;
            win = false;
        }

        public void inGame()
        {
            atMenu = false;
            gameOn = true;
        }
        public int verifyPlayerCount()
        {
            int count = 0;
            foreach (GameObject player in players)
            {
                if (player.activeInHierarchy)
                    count++;
            }
            return count;
        }

        public GameObject getLastPlayer()
        {
            foreach (GameObject player in players)
            {
                if (player.activeInHierarchy)
                    return player;
            }
            return null;
        }
    }

    public static GameManager instance;
    public static GameState gameState;
    public static GameObject menu;
    public static GameObject ui;

    GameManager _instance()
    {
        if (instance == null)
            return instance = new GameManager();
        else
            return instance;
    }

    GameState _gameState()
    {
        if (gameState == null)
            return gameState = new GameState();
        else
            return gameState;
    }

    GameObject _menu()
    {
        if (menu == null)
            return menu = GameObject.Find("Menu");
        else
            return menu;
    }

    GameObject _ui()
    {
        if (ui == null)
            return ui = GameObject.Find("UI");
        else
            return ui;
    }

    void Start()
    {
        instance = _instance();
        gameState = _gameState();
        menu = _menu();
        ui = _ui();
        menu.SetActive(true);
        ui.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            string playerName = "player_%d" + i;
            GameObject player = new GameObject(playerName);
            PlayerController control = player.AddComponent<PlayerController>();
            

        }
    }
    
    void Update()
    {
        if (gameState.atMenu)
        {
            if(gameState.startingGame)
            {
                gameState.startGame();
            }
        }

        if (gameState.gameOn)
        {
            if (gameState.verifyPlayerCount() == 1)
            {
                GameObject winPlayer = gameState.getLastPlayer();
                if (winPlayer != null)
                {
                    gameState.win = true;
                    playWinAnim();
                    //wait for anim to end before going back to the main menu.
                }
                else
                {
                    playNoContestAnim();
                    //no contest.
                }
            }
        }

        if (gameState.win)
        {

        }
    }

    void playWinAnim()
    {
        //TODO:
        /*
         * Get the GameObject to animate when some one wins 
         */
    }

    void playNoContestAnim()
    {
        ui.GetComponentInChildren<Image>(); 
    }

    void swapCanvasUIMenu()
    {
        if (menu.activeInHierarchy)
        {
            menu.SetActive(false);
            ui.SetActive(true);
        }
        else
        {
            menu.SetActive(true);
            ui.SetActive(false);
        }
    }

    void swapMenues()
    {
        foreach(Button btn in ui.GetComponents<Button>())
        {
            if (btn.tag == "startMenuBtn")
                btn.GetComponent<GameObject>().SetActive(!btn.GetComponent<Button>().IsActive());
        }
    }

    public void OnClickStart()
    {
        swapMenues();
    }
}
