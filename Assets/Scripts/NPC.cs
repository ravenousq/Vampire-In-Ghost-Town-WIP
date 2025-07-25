using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour,ISaveManager
{
    public SpriteRenderer sr { get; protected set; }
    public Animator anim { get; protected set; }
    public BoxCollider2D triggerArea { get; protected set; }
    public Dialogue dialogue { get; protected set; }

    protected private void Awake() 
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();    
        triggerArea = GetComponent<BoxCollider2D>();
        dialogue = GetComponent<Dialogue>();

        foreach (ItemData item in stock)
            defaultStock.Add(item);
    }

    [SerializeField] protected string npcName;
    [SerializeField] protected Transform dialoguePoint;
    [Tooltip("-1 => facing left, 1 => facing right")]
    [SerializeField] protected int requiredFacingDir;

    [Header("Shop")]
    [SerializeField] public List<ItemData> stock;
    protected List<ItemData> defaultStock = new List<ItemData>();
    protected DialogueManager dialogueManager;
    protected Player player;
    protected bool canStartDialogue;
    private float timer = .3f;
    private bool dialogueOngoing;
    private bool inRange;

    private void Start()
    {
        player = PlayerManager.instance.player;
        dialogueManager = DialogueManager.instance;
    }

    private void Update() 
    {
        if(inRange && !dialogueOngoing)
            timer -= Time.deltaTime;

        if(timer < 0 && !canStartDialogue)
        {
            dialogueManager.EnableTalkUI(true);
            canStartDialogue = true;
        }

        if(canStartDialogue)
        {
            if(Input.GetKeyDown(KeyCode.C) && player.IsGroundDetected() && Time.timeScale == 1)
            {   
                timer = 10;
                canStartDialogue = false;
                dialogueOngoing = true;
                player.DialogueStarted(dialoguePoint, requiredFacingDir);
                dialogueManager.InitializeDialogue(dialogue);
            }
        }    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>())
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.GetComponent<Player>())
        {
            inRange = false;
            canStartDialogue = false;
            dialogueManager.EnableTalkUI(false);
            timer = .3f;
        }
    }

    public void DialogueEnded()
    {
        timer = 1;
        dialogueOngoing = false;
    } 

    public void RemoveItemFromStock(ItemData item) => stock.Remove(item);

    public string GetName() => npcName;

    private void OnValidate() 
    {
        gameObject.name = npcName;    
    }

    public void LoadData(GameData data)
    {

        if (data.npcs.TryGetValue(npcName, out int firstIndex))
            dialogue.AssignFirstLine(dialogue.dialogueTree[firstIndex]);

        if (data.npcShops.TryGetValue(npcName, out string newStock))
        {
            stock.Clear();

            for (int i = 0; i < defaultStock.Count; i++)
                if (newStock[i] == 'T')
                    stock.Add(defaultStock[i]);
        }
    }

    public void SaveData(ref GameData data)
    {
        SaveDialogueLines(ref data);
        SaveShop(ref data);
    }

    private void SaveShop(ref GameData data)
    {
        Dictionary<string, string> helper = new Dictionary<string, string>();

        foreach (KeyValuePair<string, string> pair in data.npcShops)
            if (pair.Key != npcName)
                helper.Add(pair.Key, pair.Value);

        data.npcShops.Clear();

        string helperString = String.Empty;

        //Solution for no duplicates in stock
        foreach (ItemData item in defaultStock)
            helperString += stock.Contains(item) ? 'T' : 'F';

        helper.Add(npcName, helperString);

        foreach (KeyValuePair<string, string> pair in helper)
            data.npcShops.Add(pair.Key, pair.Value);
    }

    private void SaveDialogueLines(ref GameData data)
    {
        Dictionary<string, int> helper = new Dictionary<string, int>();

        foreach (KeyValuePair<string, int> pair in data.npcs)
            if (pair.Key != npcName)
                helper.Add(pair.Key, pair.Value);

        data.npcs.Clear();

        helper.Add(npcName, dialogue.dialogueTree.IndexOf(dialogue.firstLine));

        foreach (KeyValuePair<string, int> pair in helper)
            data.npcs.Add(pair.Key, pair.Value);
    }
}
