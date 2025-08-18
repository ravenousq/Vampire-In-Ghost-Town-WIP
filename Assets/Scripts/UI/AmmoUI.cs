
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image ammoImage;
    [SerializeField] private Color textColor;
    private bool checker = true;
    private bool setUp;

    private void Awake()
    {
        ammoText = GetComponentInChildren<TextMeshProUGUI>();
    }

    Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void Update() 
    {

        if (checker)
        {
            if (SkillManager.instance.isSkillUnlocked("Constellation of Tears"))
            {
                checker = false;
                ammoImage.color = Color.white;
                ammoText.color = textColor;
                UpdateAmmo();
            }
        }

        if (!setUp && player?.skills?.shoot != null)
        {
            setUp = true;
            player.skills.shoot.OnAmmoChange += UpdateAmmo;
        }
    }

    private void UpdateAmmo() 
    {
        ammoText.text = player.skills.shoot.currentAmmo.ToString();

        ammoText.color = player.skills.shoot.currentAmmo < 4 ? Color.red : Color.white;
    }
}
