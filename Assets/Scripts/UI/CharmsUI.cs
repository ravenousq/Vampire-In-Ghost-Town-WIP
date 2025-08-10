using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CharmsUI : MonoBehaviour
{
    [SerializeField] private ItemDisplay display;
    [SerializeField] private GameObject charmsParent;
    [SerializeField] private GameObject charmsSlotParent;
    [SerializeField] private TextMeshProUGUI equipSnippet;
    [SerializeField] private GameObject returnSnippet;
    private ItemSlotUI[] charms;
    private ItemSlotUI[] charmSlots;
    private int slotIndex;
    private int charmIndex;
    private ItemData currentData;
    private bool navigatingCharms = true;

    private void Awake()
    {
        charms = charmsParent.GetComponentsInChildren<ItemSlotUI>();
        charmSlots = charmsSlotParent.GetComponentsInChildren<ItemSlotUI>();
    }

    private void Start()
    {
        SwitchToCharm(0);
        returnSnippet.SetActive(false);
    }

    private void Update()
    {
        if (navigatingCharms)
        {
            if (Input.GetKeyDown(KeyCode.W))
                SwitchToCharm(charmIndex - 7 < 0 ? charmIndex + 14 : charmIndex - 7);

            if (Input.GetKeyDown(KeyCode.A))
                SwitchToCharm(charmIndex % 7 == 0 ? charmIndex + 6 : charmIndex - 1);

            if (Input.GetKeyDown(KeyCode.S))
                SwitchToCharm(charmIndex + 7 > charms.Length - 1 ? charmIndex - 14 : charmIndex + 7);

            if (Input.GetKeyDown(KeyCode.D))
                SwitchToCharm((charmIndex + 1) % 7 == 0 ? charmIndex - 6 : charmIndex + 1);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
                SwitchToSlot(slotIndex - 1 < 0 ? charmSlots.Length - 1 : slotIndex - 1);

            if (Input.GetKeyDown(KeyCode.D))
                SwitchToSlot(slotIndex + 1 == charmSlots.Length ? 0 : slotIndex + 1);

            if (Input.GetKeyDown(KeyCode.X))
            {
                returnSnippet.SetActive(false);

                SwitchToCharm(charmIndex);

                SwitchToSlot(-1);
                
                navigatingCharms = !navigatingCharms;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //navigatingCharms = !navigatingCharms;

            if (!navigatingCharms)
            {
                if (charms[charmIndex]?.item?.itemData != null)
                    Inventory.instance.EquipCharm(charms[charmIndex].item.itemData as CharmData, slotIndex);

                if (charms[charmIndex].item != null)
                    SwitchToCharm(GetCharmIndex(charmSlots[slotIndex].item));
                else
                    SwitchToCharm(charmIndex);

                SwitchToSlot(-1);

                returnSnippet.SetActive(false);
                
                navigatingCharms = !navigatingCharms;
            }
            else
            {
                int slotToEquip = FirstFreeSlot();

                if (currentData == null)
                    return;

                if (IsCharmEquipped(charms[charmIndex]?.item.itemData as CharmData) >= 0)
                    {
                        Inventory.instance.UnequipCharm(charms[charmIndex]?.item.itemData as CharmData, IsCharmEquipped(charms[charmIndex]?.item.itemData as CharmData));

                        foreach (ItemSlotUI slot in charmSlots)
                            if (slot.item != null)
                                Inventory.instance.EquipCharm(slot.item.itemData as CharmData, FirstFreeSlot());

                        return;
                    }

                if (slotToEquip >= 0)
                    Inventory.instance.EquipCharm(charms[charmIndex].item.itemData as CharmData, slotToEquip);

                else
                {
                    returnSnippet.SetActive(true);

                    SwitchToSlot(0);
                    navigatingCharms = !navigatingCharms;
                }
            }
        }
    }

    private int GetCharmIndex(InventoryItem item)
    {
        if (item == null)
            return charmIndex;

        for (int i = 0; i < charms.Length; i++)
        {
            if (charms[i].item.itemData == item.itemData)
                return i;
        }

        return charmIndex;
    }

    public void SwitchToSlot(int index)
    {
        if (index == -1)
        {
            charmSlots[slotIndex].Select(false);
            return;
        }

        if (index != slotIndex)
            charmSlots[slotIndex].Select(false);

        slotIndex = index;

        charmSlots[slotIndex].Select(true);
        currentData = charmSlots[slotIndex]?.item?.itemData;
        display.SetUp(currentData);
    }

    public void SwitchToCharm(int index)
    {
        if (charms == null || charms.Length == 0)
            Awake();

        if (index == -1)
        {
            charms[charmIndex].Select(false);
            return;
        }

        if (index != charmIndex)
            charms[charmIndex].Select(false);

        charmIndex = index;

        charms[charmIndex].Select(true);
        currentData = charms[charmIndex]?.item?.itemData;
        display.SetUp(currentData);

        equipSnippet.text = IsCharmEquipped(currentData as CharmData) >= 0 ? "<color=#81675D>C</color><color=#493B39> - Unquip</color>" : "<color=#81675D>C</color><color=#493B39> - Equip</color>";
    }

    public int IsCharmEquipped(CharmData charm)
    {
        if (charm == null)
            return -1;

        for (int i = 0; i < charmSlots.Length; i++)
        {
            if (charmSlots[i]?.item?.itemData == charm)
                return i;
        }

        return -1;
    }

    private void OnEnable()
    {
        SwitchToCharm(0);
        charmIndex = 0;
        navigatingCharms = true;
        SwitchToSlot(-1);
    }

    private int FirstFreeSlot()
    {
        for (int i = 0; i < charmSlots.Length; i++)
            if (charmSlots[i].item == null)
                return i;
        
        return -1;
    }
}
