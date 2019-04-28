using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public enum Transition { ToTuto, ToMenu, ToBattle, ToLootbox }
    public Transition transition;
    public Animation anim;
    public AudioSource sound;
    public bool clickable = true;

    public void OnClick()
    {
        sound.Play();
        anim.Play();
        StartCoroutine(Clickable());
    }

    private IEnumerator Clickable()
    {
        clickable = false;
        yield return new WaitForSeconds(3);
        clickable = true;
    }

}
