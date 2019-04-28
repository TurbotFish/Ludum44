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
    public Animator menuAnim;
    public bool inLootbox;
    public bool canPass;
    public int lootboxPrice;
    public Transform itemA, itemB, itemC;
    public TextMeshPro itemAText, itemBText, itemCText;
    public GameObject itemAGO, itemBGO, itemCGO;
    public float objectsRotationSpeed;


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
        if (!inMenu&&playerCount<=1)
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
        p.playerName = names[Random.Range(0, names.Count - 1)];
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
        yield return new WaitForSeconds(2);
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
        Camera.main.transform.localPosition = Vector3.zero;
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
        if (money >= lootboxPrice)
        {
            inLootbox = true;
            money -= lootboxPrice;
            menuAnim.SetTrigger("in");
            yield return new WaitForSeconds(0.5f);
            lootboxAnim.SetTrigger("buy");
            yield return new WaitForSeconds(1.2f);
            UnlockItem(itemA, itemAGO, itemAText);
            UnlockItem(itemB, itemBGO, itemBText);
            UnlockItem(itemC, itemCGO, itemCText);
            menuAnim.SetTrigger("objects");

            yield return new WaitForSeconds(4);
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
        Debug.Log(message);
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
            Debug.Log("canlootV");
        }
        if (unlockableWeapons.Count > 0)
        {
            canLootWeapon = true;
            Debug.Log("canlootW");

        }
        if (unlockableSkins.Count > 0)
        {
            canLootSkin = true;
            Debug.Log("canlootS");

        }
        if (unlockableHairs.Count > 0)
        {
            canLootHair = true;
            Debug.Log("canlootH");

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
            v.transform.localScale *= 0.6f;
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

}
