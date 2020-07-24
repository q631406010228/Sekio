using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using CodeMonkey;

public class HealthPostureUIVisual : MonoBehaviour {

    private const float POSTURE_BAR_WIDTH = 560f;
    private const float DAMAGED_HEALTH_SHORTEN_TIMER_MAX = 1f;

    private Image healthBarImage;
    private Image healthBarDamagedImage;
    private RectTransform postureBarRectTransform;
    private Image postureBarImage;
    private GameObject postureBarHighlightGameObject;
    private float healthBarDamagedShortenTimer;

    public HealthPostureSystem healthPostureSystem;

    private void Awake() {
        healthBarImage = transform.Find("healthBar").GetComponent<Image>();
        healthBarDamagedImage = transform.Find("healthBarDamaged").GetComponent<Image>();
        postureBarRectTransform = transform.Find("postureBar").GetComponent<RectTransform>();
        postureBarImage = transform.Find("postureBar").GetComponent<Image>();
        postureBarHighlightGameObject = transform.Find("postureBarHighlight").gameObject;
    }

    private void Start() {
        HealthPostureSystem healthPostureSystem = new HealthPostureSystem();
        SetHealthPostureSystem(healthPostureSystem);
        
        //CMDebug.ButtonUI(new Vector2(-100, -150), "Health Damage", () => healthPostureSystem.HealthDamage(10));
        //CMDebug.ButtonUI(new Vector2(-100, -200), "Health Heal", () => healthPostureSystem.HealthHeal(10));
        //CMDebug.ButtonUI(new Vector2(+100, -150), "Posture Increase", () => healthPostureSystem.PostureIncrease(10));
        //CMDebug.ButtonUI(new Vector2(+100, -200), "Posture Decrease", () => healthPostureSystem.PostureDecrease(10));
    }

    private void Update() {
        healthBarDamagedShortenTimer -= Time.deltaTime;
        if (healthBarDamagedShortenTimer <= 0f) {
            // Shorten damaged bar
            if (healthBarImage.fillAmount < healthBarDamagedImage.fillAmount) {
                float shortenAmount = 2f * Time.deltaTime;
                healthBarDamagedImage.fillAmount = healthBarDamagedImage.fillAmount - shortenAmount;
            }
        }
    }

    public void SetHealthPostureSystem(HealthPostureSystem healthPostureSystem) {
        // Set the HealthPostureSystem to display
        this.healthPostureSystem = healthPostureSystem;
        // Update starting values
        SetHealth(healthPostureSystem.GetHealthNormalized());
        SetPosture(healthPostureSystem.GetPostureNormalized());
        // Subscribe to changing events
        healthPostureSystem.OnHealthChanged += HealthPostureSystem_OnHealthChanged;
        healthPostureSystem.OnPostureChanged += HealthPostureSystem_OnPostureChanged;
        healthPostureSystem.OnDead += HealthPostureSystem_OnDead;
        healthPostureSystem.OnPostureBroken += HealthPostureSystem_OnPostureBroken;
    }

    private void HealthPostureSystem_OnPostureBroken(object sender, System.EventArgs e) {
        CMDebug.TextPopupMouse("Posture Broken!");
        //postureBarHighlightGameObject.SetActive(true);
    }

    private void HealthPostureSystem_OnDead(object sender, System.EventArgs e) {
        CMDebug.TextPopupMouse("Dead!");
    }

    private void HealthPostureSystem_OnPostureChanged(object sender, System.EventArgs e) {
        // Posture changed, update bar
        SetPosture(healthPostureSystem.GetPostureNormalized());
    }

    private void HealthPostureSystem_OnHealthChanged(object sender, System.EventArgs e) {
        // Health changed, update bar
        SetHealth(healthPostureSystem.GetHealthNormalized());
    }

    public void SetHealth(float healthNormalized) {
        healthBarImage.fillAmount = healthNormalized;

        if (healthBarDamagedImage.fillAmount > healthBarImage.fillAmount) {
            // Damaged, set timer to shorten damagedImage
            healthBarDamagedShortenTimer = DAMAGED_HEALTH_SHORTEN_TIMER_MAX;
        } else {
            // Healed, updated damaged image to same size as current health bar
            healthBarDamagedImage.fillAmount = healthBarImage.fillAmount;
        }
    }

    public void SetPosture(float postureNormalized) {
        postureBarRectTransform.sizeDelta = new Vector2(postureNormalized * POSTURE_BAR_WIDTH, postureBarRectTransform.sizeDelta.y);
        Color postureBarColor = new Color(1, 1 - postureNormalized * 1f, 0);
        postureBarImage.color = postureBarColor;
    }
}
