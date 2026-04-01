using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenChest : MonoBehaviour, IInteractable
{
    public Animator anim;
    public bool chestOpened = false;
    public GameObject pitchfork;
    public Text shootText;


    public void Interact()
    {
        if (!chestOpened)
        {
            anim.SetTrigger("Open");
            chestOpened = true;
            StartCoroutine(activateWeapon());
        }
    }

    IEnumerator activateWeapon()
    {
        yield return new WaitForSeconds(1.5f);
        pitchfork.SetActive(true);
        shootText.enabled = true;
    }
}
