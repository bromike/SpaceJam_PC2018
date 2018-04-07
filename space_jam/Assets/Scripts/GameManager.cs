using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState
{
    public List<GameObject> players;

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

    public void playerSelection()
    {
        gameOn = false;
        win = false;
        atMenu = false;
        startingGame = true;
    }

    public void winner()
    {
        gameOn = false;
        win = true;
        atMenu = false;
        startingGame = false;
    }

    public void inMenu()
    {
        atMenu = true;
        gameOn = false;
        win = false;
        startingGame = false;
        foreach (GameObject player in players)
        {
            player.transform.GetChild(0).gameObject.SetActive(false);
            player.GetComponent<CharacterController>().enabled = false;
        }

    }

    public void inGame()
    {
        win = false;
        atMenu = false;
        startingGame = false;
        gameOn = true;
        foreach (GameObject player in players)
        {
            player.transform.GetChild(0).gameObject.SetActive(true);
            player.GetComponent<CharacterController>().enabled = true;
        }
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

[System.Serializable]
public class HammerData
{
    public float dashTime = 250.0f;   // in ms
    public float hammerForce = 20.0f;
}

public class GameManager : MonoBehaviour
{
    public HammerData hammerData;
    public List<GameObject> players;

    public static GameManager instance;
    public static GameState gameState;

    public static GameObject menu;
    public static GameObject ui;
    public GameObject arena;

    public GameObject lightningStrike;
    public int scorchTime;
    public int variantionRange;
    public int strikeFreqStage1;
    public int strikeFreqStage2;
    public int strikeFreqStage3;
    public int stageTime1;
    public int stageTime2;
    public int stageTime3;

    public Transform[] playerPos;
    public GameObject playerObj;

    bool lightning = false;
    bool isInAnim = false;
    


    GameManager _instance()
    {
        if (instance == null)
            return instance = this;
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
        gameState.players = players;
        foreach(GameObject player in players)
        {
            player.transform.GetChild(0).gameObject.SetActive(false);
        }
        setupMenu();
    }

    void Update()
    {
        if (gameState.atMenu)
        {
            StopAllCoroutines();
            setupMenu();
        }
        if (gameState.startingGame)
        {
            foreach (GameObject player in gameState.players)
            {
                player.transform.GetChild(0).gameObject.SetActive(false);
            }
            gameState.inGame();
        }

        if (gameState.gameOn)
        {
            if (!lightning)
                StartCoroutine(lightningStorm());
            if (gameState.verifyPlayerCount() <= 1)
            {
                GameObject winPlayer = gameState.getLastAlivePlayer();
                if (winPlayer != null)
                {
                    StartCoroutine(playWinAnim());
                    //wait for anim to end before going back to the main menu.
                }
                else
                {
                    playNoContestAnim();
                    //no contest.
                }
                StartCoroutine(playWinAnim());
                gameState.winner();
                lightning = !lightning;
            }
        }

        if (gameState.win)
        {
            if (!isInAnim)
            {
                resetPlayer();
                gameState.inMenu();
            }
        }
    }

    public void onClickWin()
    {
        int count = 4;
        foreach (GameObject player in gameState.players)
        {
            player.SetActive(false);
            count--;
            if (count == 1)
                break;
        }
    }
    
    public void onClickReturn()
    {
        gameState.winner();
    }

    public void onClickStart4()
    {
        gameState.inGame();
        swapCanvasUIMenu();
        foreach (GameObject player in gameState.players)
        {
            player.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void onClickStart2()
    {
        gameState.inGame();
        swapCanvasUIMenu();
        for(int i = 0;i < 2;i++)
        {
            players[i].transform.GetChild(0).gameObject.SetActive(true);
        }
        players[2].gameObject.SetActive(false);
        players[3].gameObject.SetActive(false);
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

    void swapMenu()
    {
        foreach (Button btn in menu.GetComponentsInChildren<Button>(true))
        {
            if (btn.tag == "startMenuBtn")
                btn.gameObject.SetActive(false);
        }

        Image[] playerIcons = menu.GetComponentsInChildren<Image>(true);
        foreach (Image icon in playerIcons)
        {
            if (icon.tag == "icon")
                icon.gameObject.SetActive(true);
        }
    }

    void setupMenu()
    {
        ui.SetActive(false);
        menu.SetActive(true);

        Button[] btns = menu.GetComponentsInChildren<Button>(true);
        foreach (Button btn in btns)
            btn.gameObject.SetActive(true);

        Image[] playerIcons = menu.GetComponentsInChildren<Image>(true);
        foreach (Image icon in playerIcons)
            if (icon.tag == "icon")
                icon.gameObject.SetActive(false);

        gameState.inMenu();
    }

    public void activatePlayerRdyIcon(int playerId)
    {
        string iconName = "Player (%d)" + playerId;
        GameObject icon = GameObject.Find(iconName);
        if (icon == null)
            return;
        icon.SetActive(true);
    }

    public void playerLeave(int playerId)
    {
        if (gameState.startingGame)
        {
            string iconName = "Player (%d)" + playerId;
            GameObject icon = GameObject.Find(iconName);
            if (icon == null)
                return;
            icon.SetActive(true);       //Will just Change the look.
        }
    }

    public void StartPressed()
    {
        if (Input.GetButton("Start") && gameState.startingGame)
        {
            gameState.startGame();
            menu.SetActive(false);
            ui.SetActive(true);
        }
    }

    void resetPlayer()
    {
        foreach(GameObject player in players)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = playerPos[player.GetComponent<PlayerController>().playerId-1].position;
            player.transform.rotation = playerPos[player.GetComponent<PlayerController>().playerId-1].rotation;
            player.SetActive(true);
        }
    }

    IEnumerator lightningStorm()
    {
        if (!lightning)
            lightning = true;

        int freq = strikeFreqStage1;
        while (lightning)
        {
            lightningStrike.GetComponent<lightningStrike>().duration = scorchTime;
            Vector3 pos = Vector3.zero;
            Random.InitState((int)Time.time*10);
            Vector3 variation = new Vector3(Random.Range(-variantionRange, variantionRange), 0, Random.Range(-variantionRange, variantionRange));
            int num = 0;
            foreach (GameObject player in players)
            {
                if (player.activeInHierarchy)
                {
                    num++;
                    pos += player.transform.position;
                }
            }
            pos /= num;
            pos += variation;
            pos.y = 0;
            if (Time.time > stageTime1 && Time.time < stageTime2)
            {
                Instantiate<GameObject>(lightningStrike, pos, new Quaternion(0, 0, 0, 0));
            }
            else if (Time.time > stageTime2 && Time.time < stageTime3)
            {
                freq = strikeFreqStage2;
                Instantiate<GameObject>(lightningStrike, pos, new Quaternion(0, 0, 0, 0));
            }
            else if(Time.time > stageTime3)
            {
                freq = strikeFreqStage3;
                Instantiate<GameObject>(lightningStrike, pos, new Quaternion(0, 0, 0, 0));
            }

            //Debug
            //Instantiate<GameObject>(lightningStrike, pos, new Quaternion(0, 0, 0, 0));

            yield return new WaitForSeconds(freq);
        }
    }

    IEnumerator playWinAnim()
    {
        isInAnim = true;
        Image winner = GameObject.Find("PlayerWin (" + gameState.getLastAlivePlayer().GetComponent<PlayerController>().playerId +")").GetComponent<Image>();
        float oldY = winner.rectTransform.position.y;
        float y = winner.rectTransform.position.y;
        while (isInAnim)
        {
            y = 2;
            winner.rectTransform.Translate(0, -y, 0);
            if (winner.rectTransform.position.y <= 305)
            {
                winner.rectTransform.position = new Vector3(winner.rectTransform.position.x, winner.rectTransform.position.y, winner.rectTransform.position.z);
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(2);
        Debug.Log("it has been 2 secc");
        winner.rectTransform.position =new Vector3(winner.rectTransform.position.x, oldY, winner.rectTransform.position.z);
        isInAnim = false;
        StopCoroutine(playWinAnim());
    }

    IEnumerator playNoContestAnim()
    {
        isInAnim = true;
        while (isInAnim)
        {
            ui.GetComponentInChildren<Image>();
            //if animStopped
            // isInAnim = false;
            yield return null;
        }
    }

}
