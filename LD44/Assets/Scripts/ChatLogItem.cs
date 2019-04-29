using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatLogItem : MonoBehaviour
{
    public float life;
    [Range(0,1)]
    public float startToDisappear;
    public TextMeshPro text;
    public SpriteRenderer sprite;

    float startTextAlpha;
    Color startSpriteColor;
    // Start is called before the first frame update
    void Start()
    {
        startTextAlpha = text.alpha;
        startSpriteColor = sprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (life<startToDisappear && life>0)
        {
            text.alpha = startTextAlpha - (startTextAlpha * (life / startToDisappear));
            sprite.color = new Color(startSpriteColor.r, startSpriteColor.g, startSpriteColor.b, startSpriteColor.a - (startSpriteColor.a * (life / startSpriteColor.a)));

        }*/
    }
}
