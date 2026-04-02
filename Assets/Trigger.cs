using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggereEnter;
    [SerializeField] UnityEvent onTriggereExit;
    public Interactor holder;
    private int objective = 0;
    public Text collectedText;
    private int objectives = 4;
    public Text victoryText;
    public TextMeshPro dropoffText;
    public FirstPersonController script;

    void Start()
    {
        collectedText.text = "Collected: " + objective + " / " + objectives;
    }

    void OnTriggerEnter(Collider other)
    {
        holder.InDropoffZone(true);
    }

    void OnTriggerExit(Collider other)
    {
        holder.InDropoffZone(false);
    }

    public void ObjectDeposit(int change)
    {
        objective += change;
        collectedText.text = "Collected: " + objective + " / " + objectives;
        if (objective == objectives)
        {
            victoryText.enabled = true;
            dropoffText.enabled = false;
            script.playVictoryAnim();
        }
    }
}
