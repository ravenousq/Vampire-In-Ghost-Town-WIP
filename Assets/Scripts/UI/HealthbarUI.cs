
using UnityEngine;
using UnityEngine.UI;

enum BarType { Health, Poise}
public class HealthbarUI : MonoBehaviour
{
    RectTransform myTransform;
    private Slider slider;
    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        myTransform = GetComponent<RectTransform>();
    }

    [SerializeField] BarType barType;
    [SerializeField] private Entity entity;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private Image[] images;
    private float currentAlpha;
    private bool enemyStats;

    private void Start()
    {
        if (!entity)
            entity = GetComponentInParent<Entity>();

        if (!stats)
            stats = GetComponentInParent<CharacterStats>();

        if (!GetComponentInParent<UI>())
            entity.OnFlipped += FlipUI;

        enemyStats = stats.GetComponent<EnemyStats>();

        if (barType == BarType.Health)
        {
            stats.OnDamaged += UpdateHealthUI;
            stats.OnHealed += UpdateHealthUI;
            UpdateHealthUI();
        }
        else
        {
            stats.OnPoiseChanged += UpdatePoiseUI;
            UpdatePoiseUI();
        }
    }

    private void Update()
    {
        currentAlpha = images[0].color.a;

        if ((!SkillManager.instance.showHealthbars && enemyStats && currentAlpha == 1) || stats.HP <= 0)
            foreach (Image image in images)
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        else if (SkillManager.instance.showHealthbars && enemyStats && currentAlpha == 0)
            foreach (Image image in images)
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.health.GetValue();
        slider.value = stats.HP;
    }

    private void UpdatePoiseUI()
    {
        slider.maxValue = stats.poise.GetValue() * 5;
        slider.value = 100 - stats.poiseTracker;
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    public void OnDisable() 
    {
        if (!entity)
            return;

        entity.OnFlipped -= FlipUI;
        stats.OnDamaged -= barType == BarType.Health ? UpdateHealthUI : null;
        stats.OnHealed -= barType == BarType.Health ? UpdateHealthUI : null;
        stats.OnPoiseChanged -= barType == BarType.Poise ? UpdatePoiseUI : null;
    }
}
