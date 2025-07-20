using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class NPCShopUI : ItemsUI
{
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private CinemachineCamera cinemachine;
    [SerializeField] private float cameraZoom;
    [SerializeField] private float cameraOffset;
    [SerializeField] private float shiftSpeed;
    private float defaultCameraZoom;
    private CinemachineFollow cinemachineFollow;
    private List<ItemData> stock;
    private NPC currentNPC;
    private int exitIndex;

    protected override void Awake()
    {
        base.Awake();

        stock = new List<ItemData>();
        cinemachineFollow = cinemachine.GetComponent<CinemachineFollow>();
        defaultCameraZoom = cinemachine.Lens.OrthographicSize;
    }

    public void SetUp(NPC npc, int index)
    {
        Awake();
        Start();

        currentNPC = npc;
        exitIndex = index;

        if(!gameObject.activeSelf)
        {
            stock.Clear();
            return;
        }

        npcName.text = npc.GetName();

        if(npc.stock.Count != 0)
            for (int i = 0; i < npc.stock.Count; i++)
            {
                items[i].UpdateSlot(new InventoryItem(npc.stock[i]));

                stock.Add(npc.stock[i]);
            }
        
        currentData = stock.Count > 0 ? stock[0] : null;

        display.SetUp(
            currentData == null ? "" : currentData.itemName,
            currentData == null ? "" : currentData.itemDescription,
            currentData == null ? "" : currentData.price.ToString()
        );
    }

    private void OnEnable()
    {
        if (UI.instance.canTurnOnGameMenu)
            UI.instance.LockGameMenu();
    }

    private void OnDisable()
    {
        if (!UI.instance.canTurnOnGameMenu)
            UI.instance.LockGameMenu();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            cinemachine.Lens.OrthographicSize = defaultCameraZoom;
            cinemachineFollow.FollowOffset.x = 0;
            DialogueManager.instance.NextLine(exitIndex);
            UI.instance.SwitchShop(currentNPC, exitIndex);
            return;
        }

        cinemachine.Lens.OrthographicSize = Mathf.Clamp(Mathf.MoveTowards(cinemachine.Lens.OrthographicSize, cameraZoom, shiftSpeed * Time.unscaledDeltaTime), cameraZoom, defaultCameraZoom);
        cinemachineFollow.FollowOffset.x = Mathf.Clamp(Mathf.MoveTowards(cinemachineFollow.FollowOffset.x, cameraOffset, shiftSpeed * Time.unscaledDeltaTime), 0, cameraOffset);

        if (Input.GetKeyDown(KeyCode.W))
            SwitchTo(selectedIndex - 5 < 0 ? selectedIndex + 5 : selectedIndex - 5, true);

        if (Input.GetKeyDown(KeyCode.A))
            SwitchTo(selectedIndex % 5 == 0 ? selectedIndex + 4 : selectedIndex - 1, true);

        if (Input.GetKeyDown(KeyCode.S))
            SwitchTo(selectedIndex + 5 > items.Length - 1 ? selectedIndex - 5 : selectedIndex + 5, true);

        if (Input.GetKeyDown(KeyCode.D))
            SwitchTo((selectedIndex + 1) % 5 == 0 ? selectedIndex - 4 : selectedIndex + 1, true);

        if (Input.GetKeyDown(KeyCode.C) && currentData != null)
        {
            if (PlayerManager.instance.CanAfford(currentData.price))
            {
                PlayerManager.instance.RemoveCurrency(currentData.price);
                UI.instance.ModifySouls(-currentData.price);
                Inventory.instance.AddItemMute(currentData);
                currentNPC.RemoveItemFromStock(currentData);
                stock.Remove(currentData);

                // foreach (ItemData item in stock)
                //     Debug.Log(item.itemName);

                currentData = items[selectedIndex + 1]?.item?.itemData;
                items[selectedIndex].CleanUpSlot();

                for (int i = selectedIndex + 1; i < stock.Count + 1; i++)
                {
                    if (items[i].item == null)
                        break;

                    items[i - 1].UpdateSlot(items[i].item);
                    items[i].CleanUpSlot();
                }

                display.SetUp(
                    currentData == null ? "" : currentData.itemName,
                    currentData == null ? "" : currentData.itemDescription,
                    currentData == null ? "" : currentData.price.ToString()
                );
            }
        }
    }
}
