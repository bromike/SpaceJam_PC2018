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

        public GameObject getLastAlivePlayer()
        {
            foreach (GameObject player in players)
            {
                if (player.activeInHierarchy)
                    return player;
            }
            return null;
        }

        public bool has2Player()
        {
            int count = 0;
            foreach (GameObject player in players)
            {
                PlayerController ctrl = player.GetComponent<PlayerController>();
                if (ctrl.isRdy)
                    count++;
                if (count >= 2)
                    return true;
            }
            return false;
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

        setupMenu();

        for (int i = 0; i < 4; i++)
        {
            string playerName = "player_%d" + i;
            GameObject player = new GameObject(playerName);
            PlayerController control = player.AddComponent<PlayerController>();
            control.playerId = i;
            player.tag = "Player";
        }
    }

    void Update()
    {
        if (gameState.atMenu)
        {
            if (gameState.startingGame && gameState.has2Player())
            {
                foreach (GameObject player in gameState.players)
                {
                    if (!player.GetComponent<PlayerController>().isRdy)
                        player.SetActive(false);
                }
                gameState.startGame();
            }
        }

        if (gameState.gameOn)
        {
            if (gameState.verifyPlayerCount() == 1)
            {
                GameObject winPlayer = gameState.getLastAlivePlayer();
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
            //Reset the Body to 0
            foreach (GameObject player in gameState.players)
            {
                player.SetActive(true);
                //player.3dBody.SetActive(false);
            }
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

    public void OnClickStart()
    {
        swapMenues();
    }

    void swapMenues()
    {
        foreach (Button btn in ui.GetComponents<Button>())
        {
            if (btn.tag == "startMenuBtn")
                btn.GetComponent<GameObject>().SetActive(!btn.GetComponent<Button>().IsActive());
        }
    }


    void setupMenu()
    {
        Image[] playerIcons = menu.GetComponentsInChildren<Image>();
        foreach(Image icon in playerIcons)
            if(icon.tag == "icon")
                icon.gameObject.SetActive(false);
    }
}
