using TMPro;
using UnityEngine;


[SelectionBase]
public class ItemObject : MonoBehaviour
{
    private AudioSource au;

    private void Awake()
    {
        au = audioPoint.GetComponent<AudioSource>();
    }

    [SerializeField] public ItemData item;
    [SerializeField] private TextMeshProUGUI inputImage;
    [SerializeField] private Transform audioPoint;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float ambientRange = 5;
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

        if (SkillManager.instance.chimingItems)
            AdjustDirectionalSound.Adjuster(au, PlayerManager.instance.player, ambientRange);
        else
            au.volume = 0;

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
        if (other.GetComponent<Player>())
            PickUpItem();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
            UndoItem();
    }

    private void UndoItem()
    {
        inRange = false;

        if (PlayerManager.instance.player.pickup.item == this)
            PlayerManager.instance.player.AssignItemToPickUp(null);
    }
    public void DestroyMe() => Destroy(gameObject);
    
    private void OnDrawGizmos()
    {
        if (audioPoint != null)
            Gizmos.DrawWireSphere(audioPoint.position, ambientRange);
    }
}
