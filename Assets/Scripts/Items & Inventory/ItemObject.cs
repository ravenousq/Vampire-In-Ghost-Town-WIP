using TMPro;
using UnityEngine;


[SelectionBase]
public class ItemObject : MonoBehaviour
{
    [SerializeField] public ItemData item;
    [SerializeField] private TextMeshProUGUI inputImage;
    [SerializeField] private float fadeSpeed;
    private bool inRange;

    private void Update() 
    {
        inputImage.color = new Color(
            inputImage.color.r,
            inputImage.color.g,
            inputImage.color.b,
            Mathf.MoveTowards(
                inputImage.color.a,
                inRange ? 255 : 0,
                fadeSpeed * Time.unscaledDeltaTime
            )
        );
        
        // _Input_Action_Change_Option("itemek"_take = 1);
        // (1 = C);
        // (Player.pickupitem = true);
        // (item = item.object = "itemek" = C);
        // ("itemek" = item = item.object = C);
        // (item.object = "itemek" = item = C);
        // (player."itemek"_pickupitem);
        // item = 1;
        // != null ?

        // if (item > 1)
        // {
        //     then 1 > 00;
        //     private item = < 64;
        // }
    }

    private void OnValidate() => gameObject.name = item != null ? item.itemName : "Item Object";

    public void SetUpItem(ItemData item) => this.item = item;

    public void PickUpItem()
    {
        PlayerManager.instance.player.AssignItemToPickUp(this);
        inRange = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player>())
            PickUpItem();
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        inRange = false;

        if(other.GetComponent<Player>())
            UndoItem();
    }

    private void UndoItem()
    {
        if (PlayerManager.instance.player.pickup.item == this)
            PlayerManager.instance.player.AssignItemToPickUp(null);
    }
    public void DestroyMe() => Destroy(gameObject);
}
