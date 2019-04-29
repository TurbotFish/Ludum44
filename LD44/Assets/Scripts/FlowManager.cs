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

    [Header("Musics and Audio")]
    public float musicVolume;
    public AudioSource audioSource;
    public AudioClip musicBattle;
    public AudioClip musicMenu;

    [Header("Save")]
    public BonhommeController playerSave;
    public Weapon weaponSave;

    [Header("Transition")]
    public Animator doorsAnim;
    public MouseController mouseController;

    [Header("EndBattle")]
    public Animator endBattle;
    public Transform playerAvatar;
    public TextMeshPro endPlayerText, playerWonText, endKillsText, endDeathsText, endKillStreakText, goldEarnedText, killsNumberText, killStreakNumberText, killsNumberGoldText, killStreakNumberGoldText, totalGoldText;
    public GameObject endBattleCoin;
    
    [Header("Menu")]
    public Transform playerAvatarMenu;
    public TextMeshPro moneyText;

    [Header("LootBox")]
    public Animator lootboxAnim;
    public Animator menuAnim;
    public bool inLootbox;
    public bool canPass;
    public int lootboxPrice;
    public Transform itemA, itemB, itemC;
    public TextMeshPro itemAText, itemBText, itemCText;
    public GameObject itemAGO, itemBGO, itemCGO;
    public float objectsRotationSpeed;

    [Header("Zone")]
    public PlayZone zone;

    [Header("InGameUI")]
    public GameObject inGameUI;
    public TextMeshPro topPlayerText;
    public TextMeshPro goldText;
    public TextMeshPro remainingPlayerText;

    [Header("Chat")]
    public ChatLog chatLog;

    [Header("PlayerData")]
    public PlayerInfoDB db;
    public int money;
    public GameObject crown;
    public string topPlayerName;
    public PlayerInfo topPlayerAlive;
    public int killCount;
    public int deathCount;
    public int killStreak;
    public int playerCount;
    public List<PlayerInfo> players = new List<PlayerInfo>();
    public List<GameObject> mapItems = new List<GameObject>();
    public Transform spawnerList;
    public int numberOfItem;
    public List<Spawner> mapSpawners = new List<Spawner>();

    [Header("DATABASE")]
    [Space(10)]
    [Header("NAMES")]
    public List<string> names;

    [Header("WEAPONS")]
    public List<GameObject> availableWeapons;
    public List<GameObject> unlockableWeapons;

    [Header("VEHICLES")]
    public List<GameObject> availableVehicles;
    public List<GameObject> unlockableVehicles;

    [Header("SKINS")]
    public List<GameObject> availableSkins;
    public List<GameObject> unlockableSkins;

    [Header("HAIRS")]
    public List<GameObject> availableHairs;
    public List<GameObject> unlockableHairs;
    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine(GoToTuto());
        ResetPlayerData();

        //SYNC DB:
        names = new List<string>(db.names);
        availableWeapons = new List<GameObject>(db.availableWeapons);
        unlockableWeapons = new List<GameObject>(db.unlockableWeapons);
        availableVehicles = new List<GameObject>(db.availableVehicles);
        unlockableVehicles = new List<GameObject>(db.unlockableVehicles);
        availableSkins = new List<GameObject>(db.availableSkins);
        unlockableSkins = new List<GameObject>(db.unlockableSkins);
        availableHairs = new List<GameObject>(db.availableHairs);
        unlockableHairs = new List<GameObject>(db.unlockableHairs);


    }

    // Update is called once per frame
    void Update()
    {
        if (!inMenu&&players.Count<=1)
        {
            StartCoroutine(EndBattle());

        }

        if (inLootbox && canPass && Input.GetButton("Action"))
        {
            StartCoroutine(QuitLootbox());
        }
        itemA.transform.Rotate(0, objectsRotationSpeed, 0);
        itemB.transform.Rotate(0, objectsRotationSpeed, 0);
        itemC.transform.Rotate(0, objectsRotationSpeed, 0);

        goldText.text = "earned:  <b>" + (killCount+ killStreak).ToString()+ "</b>";
        remainingPlayerText.text = "Remaining players:  <b>"+players.Count.ToString() + "</b>";
        topPlayerText.text = "Top killstreak: <b>" + topPlayerName + "</b> with <b>" + killStreak.ToString() + "</b> kills";
    }

    public void SpawnItems()
    {
        mapSpawners.Clear();
        mapItems.Clear();
        foreach (Transform spawn in spawnerList)
        {
            mapSpawners.Add(spawn.GetComponent<Spawner>());
        }
        List<int> numbers = new List<int>();
        for (int h = 0; h < mapSpawners.Count; h++)
        {
            numbers.Add(h);
        }
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
        players.Add(p);
        playerCount++;
    }

    public void RemovePlayer(PlayerInfo p, bool kill)
    {
        if (players.Contains(p))
        {
            if (playerCount>1)
            {
                players.Remove(p);
            }
            playerCount--;
        }

        if (kill)
        {
            killCount++;
        }
        else
        {
            deathCount++;
        }

        p.Reset();
        CheckKillStreak(p);
    }

    public void ResetPlayerData()
    {
        killCount = 0;
        deathCount = 0;
        killStreak = 0;
        topPlayerName = "no one";

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
    
    /*private IEnumerator StartOfTheGame()
    {
        zone.gameObject.SetActive(false);
        inMenu = true;
        CrowdController.lockControls = true;
        yield return new WaitForSeconds(0);
        CloseDoors();
    }*/

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
        foreach(GameObject go in mapItems)
        {
            if (go != null)
            {
                Destroy(go);
            }
        }
        mapItems.Clear();
        inMenu = true;
        CrowdController.lockControls = true;
        CloseDoors();
        yield return new WaitForSeconds(1);
        mouseController.StartZoom();

        SpawnItems();
        ResetPlayerData();
        cam.position = camGamePos.position;
        cam.rotation = camGamePos.rotation;
        yield return new WaitForSeconds(1);
        OpenDoors();
        yield return new WaitForSeconds(0.75f);


        if (audioSource.clip != musicBattle || audioSource.clip == musicBattle && !audioSource.isPlaying)
        {
            audioSource.volume = musicVolume;
            audioSource.Stop();
            audioSource.clip = musicBattle;
            audioSource.Play();
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(GetComponent<CrowdSpawner>().InstantiateCrowd());
        yield return new WaitForSeconds(3);
        inGameUI.SetActive(true);
        zone.gameObject.SetActive(true);
        zone.Reset();
        inMenu = false;
        CrowdController.lockControls = false;
        zone.progressing = true;

    }

    private IEnumerator EndBattle()
    {
        inGameUI.SetActive(false);
        playerSave = players[0].GetComponent<BonhommeController>();
        if (playerSave.weapon != null)
        {
            weaponSave = playerSave.weapon;
            mapItems.Remove(playerSave.weapon.gameObject);
        }
        if (playerSave.inVehicle == true)
        {
            playerSave.ExitVehicle(true);
        }
        players.Clear();
        zone.progressing = false;
        zone.gameObject.SetActive(false);
        inMenu = true;
        CrowdController.lockControls = true;
        yield return new WaitForSeconds(0.5f);
        endBattle.SetBool("on", true);
        audioSource.volume = musicVolume * 0.50f;

        playerSave.transform.position = playerAvatar.position;
        playerSave.transform.rotation = playerAvatar.rotation;
        playerSave.transform.SetParent(playerAvatar);
        playerSave.GetComponent<Rigidbody>().isKinematic = true;

        yield return new WaitForSeconds(0.75f);
        playerWonText.gameObject.SetActive(true);
        playerWonText.text = "<b>"+playerSave.playerInfo.playerName+"</b> won the Battle!";
        endPlayerText.text = playerSave.playerInfo.playerName;
        yield return new WaitForSeconds(0.2f);
        endKillsText.gameObject.SetActive(true);
        endKillsText.text = killCount.ToString() + " players were killed";
        yield return new WaitForSeconds(0.2f);
        endDeathsText.gameObject.SetActive(true);
        endDeathsText.text = deathCount.ToString() + " died by themselves...";
        yield return new WaitForSeconds(0.2f);
        endKillStreakText.gameObject.SetActive(true);
        endKillStreakText.text = "Top killstreak was <b>" + topPlayerName + "</b> with <b>" + killStreak.ToString() + "</b> kills";
        yield return new WaitForSeconds(0.2f);
        goldEarnedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        killsNumberText.gameObject.SetActive(true);
        killsNumberGoldText.gameObject.SetActive(true);
        killsNumberGoldText.text = killCount.ToString();
        yield return new WaitForSeconds(0.2f);
        killStreakNumberText.gameObject.SetActive(true);
        killStreakNumberGoldText.gameObject.SetActive(true);
        killStreakNumberGoldText.text = killStreak.ToString();
        yield return new WaitForSeconds(0.2f);
        endBattleCoin.SetActive(true);
        totalGoldText.gameObject.SetActive(true);
        totalGoldText.text = (killCount + killStreak).ToString();

        money += killCount+killStreak;
        moneyText.text = money.ToString();

    }

    private IEnumerator GoToMenu()
    {
        moneyText.text = money.ToString();
        inMenu = true;
        CrowdController.lockControls = true;
        CloseDoors();
        yield return new WaitForSeconds(1);

        playerWonText.gameObject.SetActive(false);
        endKillsText.gameObject.SetActive(false);
        endDeathsText.gameObject.SetActive(false);
        endKillStreakText.gameObject.SetActive(false);
        killsNumberText.gameObject.SetActive(false);
        killsNumberGoldText.gameObject.SetActive(false);
        killStreakNumberText.gameObject.SetActive(false);
        killStreakNumberGoldText.gameObject.SetActive(false);
        endBattleCoin.SetActive(false);
        goldEarnedText.gameObject.SetActive(false);
        totalGoldText.gameObject.SetActive(false);

        zone.gameObject.SetActive(false);
        endBattle.SetBool("on", false);
        cam.position = camMenuPos.position;
        cam.rotation = camMenuPos.rotation;
        Camera.main.transform.localPosition = Vector3.zero;
        yield return new WaitForSeconds(1);
        OpenDoors();
        playerSave.transform.position = playerAvatarMenu.position;
        playerSave.transform.rotation = playerAvatarMenu.rotation;
        playerSave.transform.SetParent(playerAvatarMenu);
        playerSave.GetComponent<Rigidbody>().isKinematic = true;

        if (audioSource.clip != musicMenu || audioSource.clip == musicMenu && !audioSource.isPlaying)
        {
            audioSource.volume = musicVolume;
            audioSource.Stop();
            audioSource.clip = musicMenu;
            audioSource.Play();
        }


    }

    private IEnumerator GoToTuto()
    {
        zone.gameObject.SetActive(false);
        inMenu = true;
        CrowdController.lockControls = true;
        CloseDoors();
        yield return new WaitForSeconds(1);
        cam.position = camTutoPos.position;
        cam.rotation = camTutoPos.rotation;

        yield return new WaitForSeconds(1);
        OpenDoors();

        audioSource.volume = musicVolume;
        audioSource.Stop();
        audioSource.clip = musicMenu;
        audioSource.Play();

    }

    private IEnumerator GoToLootbox()
    {
        inMenu = true;
        CrowdController.lockControls = true;
        if (money >= lootboxPrice)
        {
            inLootbox = true;
            money -= lootboxPrice;
            moneyText.text = money.ToString();
            menuAnim.SetTrigger("in");
            yield return new WaitForSeconds(0.5f);
            lootboxAnim.SetTrigger("buy");
            yield return new WaitForSeconds(1.2f);
            UnlockItem(itemA, itemAGO, itemAText);
            UnlockItem(itemB, itemBGO, itemBText);
            UnlockItem(itemC, itemCGO, itemCText);
            menuAnim.SetTrigger("objects");

            yield return new WaitForSeconds(3);
            canPass = true;
        }
        else
        {
            Debug.Log("NOT ENOUGH MONEY");
        }

    }

    private IEnumerator QuitLootbox()
    {
        canPass = false;
        inMenu = true;
        CrowdController.lockControls = true;
        inLootbox = false;
        menuAnim.SetTrigger("out");
        yield return new WaitForSeconds(2f);
        lootboxAnim.SetTrigger("quit");
        Destroy(itemA.GetChild(0).gameObject);
        Destroy(itemB.GetChild(0).gameObject);
        Destroy(itemC.GetChild(0).gameObject);

    }

    public void SendChatMessage(string message)
    {
        chatLog.AddChatItem(message);
    }

    public void UnlockItem(Transform item, GameObject itemGO, TextMeshPro text)
    {
        bool canLootVehicle = false;
        bool canLootWeapon = false;
        bool canLootSkin = false;
        bool canLootHair = false;


        if (unlockableVehicles.Count>0)
        {
            canLootVehicle = true;
            //Debug.Log("canlootV");
        }
        if (unlockableWeapons.Count > 0)
        {
            canLootWeapon = true;
            //Debug.Log("canlootW");

        }
        if (unlockableSkins.Count > 0)
        {
            canLootSkin = true;
            //Debug.Log("canlootS");

        }
        if (unlockableHairs.Count > 0)
        {
            canLootHair = true;
            //Debug.Log("canlootH");

        }

        int l = -1;

        for (int i = 0; i<100;i++)
        {
            float p = Random.Range(0, 100);
            if (p <= 35 && canLootVehicle)
            {
                i = 100;
                l = 0;
            }
            else if (p > 35 && p <= 70 && canLootWeapon)
            {
                i = 100;
                l = 1;
            }
            else if (p > 75 && p <= 80 && canLootSkin)
            {
                i = 100;
                l = 2;
            }
            else if (p > 85 && p <= 100 && canLootHair)
            {
                i = 100;
                l = 3;
            }
        }


        //VEHICLES
        if (l == 0)
        {
            int index = Random.Range(0, unlockableVehicles.Count);
            availableVehicles.Add(unlockableVehicles[index]);

            GameObject v = Instantiate(unlockableVehicles[index], item.position, item.rotation, item) as GameObject;
            itemGO = v;
            v.GetComponent<VehicleController>().enabled = false;
            v.GetComponent<Rigidbody>().isKinematic = true;
            v.transform.localScale *= 0.45f;
            ParticleSystem[] particles = v.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in particles)
            {
                Destroy(ps.gameObject);
            }
            text.text = v.GetComponent<AssetName>().asset;

            unlockableVehicles.RemoveAt(index);
        }

        //WEAPONS
        else if (l==1)
        {
            int index = Random.Range(0, unlockableWeapons.Count);
            availableWeapons.Add(unlockableWeapons[index]);

            GameObject w = Instantiate(unlockableWeapons[index], item.position, item.rotation, item) as GameObject;
            itemGO = w;
            w.transform.localPosition += new Vector3(0, 0.5f, 0);
            w.transform.GetChild(0).localEulerAngles += new Vector3(-90, 0, 0);

            w.GetComponent<Weapon>().enabled = false;
            text.text = w.GetComponent<AssetName>().asset;

            unlockableWeapons.RemoveAt(index);
        }
        //SKINS
        else if (l==2)
        {
            int index = Random.Range(0, unlockableSkins.Count);
            availableSkins.Add(unlockableSkins[index]);

            GameObject s = Instantiate(unlockableSkins[index], item.position, item.rotation, item) as GameObject;
            s.transform.eulerAngles += new Vector3(-90, 0, 0);
            s.transform.localPosition += new Vector3(0, -0.75f, 0);

            itemGO = s;
            //s.GetComponent<VehicleController>().enabled = false;
            text.text = s.GetComponent<AssetName>().asset;

            unlockableSkins.RemoveAt(index);
        }
        //HAIRS
        else if (l==3)
        {
            int index = Random.Range(0, unlockableHairs.Count);
           availableHairs.Add(unlockableHairs[index]);

            GameObject h = Instantiate(unlockableHairs[index], item.position, item.rotation, item) as GameObject;
            h.transform.eulerAngles += new Vector3(0, 0, -90f);
            itemGO = h;
            //v.GetComponent<VehicleController>().enabled = false;
            text.text = h.GetComponent<AssetName>().asset;

            unlockableHairs.RemoveAt(index);
        }
        //HAIRS
        else if (l == -1)
        {
            Debug.Log("no more items");
            return;
        }

    }

    public void CheckKillStreak(PlayerInfo p)
    {
        if (p.kills > killStreak)
        {
            killStreak = p.kills;
            if (topPlayerName != p.playerName)
            {
                topPlayerName = p.playerName;

            }
        }

        int ks = 0;
        for (int i = 0; i<players.Count;i++)
        {
            if (players[i].kills>ks)
            {
                topPlayerAlive = players[i];
            }
        }
        
        if (topPlayerAlive!= null)
        {
            crown.transform.SetParent(topPlayerAlive.transform);
            crown.transform.localPosition = new Vector3(0, 2.1f, 0);
        }


    }

}
