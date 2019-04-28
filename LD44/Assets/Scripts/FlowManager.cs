using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlowManager : Singleton<FlowManager>
{
    public static bool inMenu;
    public Transform camGamePos;
    public Transform camMenuPos;
    public Transform camTutoPos;
    public Transform cam;

    [Header("Transition")]
    public Animator doorsAnim;

    [Header("EndBattle")]
    public Animator endBattle;

    [Header("LootBox")]
    public Animator lootboxAnim;
    public bool inLootbox;

    [Header("InGameUI")]
    public TextMeshPro topPlayerText;
    public TextMeshPro killsText;
    public TextMeshPro deathsText;

    [Header("PlayerData")]
    public PlayerInfoDB db;
    public int money;
    public static int killCount;
    public static int deathCount;
    public static int killStreak;
    public int playerCount;
    public List<PlayerInfo> players = new List<PlayerInfo>();
    public List<GameObject> mapItems = new List<GameObject>();
    public Transform spawnerList;
    public int numberOfItem;
    public List<Spawner> mapSpawners = new List<Spawner>();
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoToTuto());
        ResetPlayerData();

    }

    // Update is called once per frame
    void Update()
    {
        if (!inMenu&&playerCount<=1)
        {
            StartCoroutine(EndBattle());
        }
    }

    public void SpawnItems()
    {
        mapSpawners.Clear();
        mapItems.Clear();
        foreach (Transform spawn in spawnerList)
        {
            mapSpawners.Add(spawn.GetComponent<Spawner>());
        }
        List<int> numbers = new List<int>(mapSpawners.Count);
        int toRemove = mapSpawners.Count - numberOfItem;
        for (int i = 0;i<toRemove;i++)
        {
            numbers.RemoveAt(Random.Range(0, numbers.Count));
        }

        for (int j=0;j<numbers.Count;j++)
        {
            mapSpawners[numbers[j]].Spawn();
        }
    }

    public void AddPlayer(PlayerInfo p)
    {
        p.playerName = FlowManager.Instance.db.names[Random.Range(0, FlowManager.Instance.db.names.Count - 1)];
        players.Add(p);
        playerCount++;
    }
    public void RemovePlayer(PlayerInfo p, bool kill)
    {
        players.Remove(p);
        playerCount--;
        if (kill)
        {
            killCount++;
        }
        else
        {
            deathCount++;
        }
    }

    public void ResetPlayerData()
    {
        killCount = 0;
        deathCount = 0;
        killStreak = 0;
    }

    public void ClickTransition(ButtonScript.Transition transition)
    {
        if (transition == ButtonScript.Transition.ToBattle)
        {
            StartCoroutine(GoToBattle());
        }
        else if (transition == ButtonScript.Transition.ToMenu)
        {
            StartCoroutine(GoToMenu());
        }
        else if (transition == ButtonScript.Transition.ToLootbox)
        {
            StartCoroutine(GoToLootbox());
        }
        else if (transition == ButtonScript.Transition.ToTuto)
        {
            StartCoroutine(GoToTuto());
        }
    }
    
    private IEnumerator StartOfTheGame()
    {
        inMenu = true;
        CrowdController.lockControls = true;
        yield return new WaitForSeconds(0);
        CloseDoors();
    }

    private void CloseDoors()
    {
        doorsAnim.SetBool("close", true);
    }

    private void OpenDoors()
    {
        doorsAnim.SetBool("close", false);
    }


    private IEnumerator GoToBattle()
    {
        inMenu = true;
        CrowdController.lockControls = true;
        CloseDoors();
        yield return new WaitForSeconds(1);
        SpawnItems();
        ResetPlayerData();
        cam.position = camGamePos.position;
        cam.rotation = camGamePos.rotation;
        yield return new WaitForSeconds(1);
        OpenDoors();
        yield return new WaitForSeconds(1);
        StartCoroutine(GetComponent<CrowdSpawner>().InstantiateCrowd());
        yield return new WaitForSeconds(2);
        inMenu = false;
        CrowdController.lockControls = false;

    }

    private IEnumerator EndBattle()
    {
        inMenu = true;
        CrowdController.lockControls = true;
        yield return new WaitForSeconds(0.5f);
        endBattle.SetBool("on", true);
    }

    private IEnumerator GoToMenu()
    {
        inMenu = true;
        CrowdController.lockControls = true;
        CloseDoors();
        yield return new WaitForSeconds(1);
        endBattle.SetBool("on", false);
        cam.position = camMenuPos.position;
        cam.rotation = camMenuPos.rotation;

        yield return new WaitForSeconds(1);
        OpenDoors();

    }

    private IEnumerator GoToTuto()
    {
        inMenu = true;
        CrowdController.lockControls = true;
        CloseDoors();
        yield return new WaitForSeconds(1);
        cam.position = camTutoPos.position;
        cam.rotation = camTutoPos.rotation;

        yield return new WaitForSeconds(1);
        OpenDoors();

    }

    private IEnumerator GoToLootbox()
    {
        inMenu = true;
        CrowdController.lockControls = true;
        Debug.Log("INLOOTBOX");
        yield return new WaitForSeconds(4);
        inLootbox = true;
    }

    private IEnumerator QuitLootbox()
    {
        inMenu = true;
        CrowdController.lockControls = true;
        inLootbox = false;
        Debug.Log("OUTLOOTBOX");

        yield return new WaitForSeconds(1);

    }

    public void SendChatMessage(string message)
    {
        Debug.Log(message);
    }


}
