using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatLog : MonoBehaviour
{
    public GameObject chatLogItem;
    public float spaceBetweenItems;
    public float displayTime;
    public List<float> displayTimes = new List<float>();
    public List<ChatLogItem> items = new List<ChatLogItem>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItems();
    }

    public void AddChatItem(string text)
    {
        GameObject item = Instantiate(chatLogItem, transform.position, transform.rotation, transform) as GameObject;
        item.transform.localPosition = new Vector3(0,  -1 * items.Count * spaceBetweenItems, 0);
        ChatLogItem cli = item.GetComponent<ChatLogItem>();
        items.Add(cli);
        cli.life = displayTime;
        cli.text.text = text;
    }

    void UpdateItems()
    {
        for (int i=0; i<items.Count;i++)
        {
            items[i].life -= Time.deltaTime;
            if (items[i].life < 0)
            {
                GameObject toDestroy = items[i].gameObject;
                items.RemoveAt(i);
                Destroy(toDestroy);

                for (int j = 0;j<items.Count;j++)
                {
                    items[j].transform.localPosition += new Vector3(0,1 * spaceBetweenItems,0);
                }
            }
        }
    }
}
