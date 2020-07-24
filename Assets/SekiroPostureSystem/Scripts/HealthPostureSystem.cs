/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPostureSystem {

    public event EventHandler OnDead;
    public event EventHandler OnPostureBroken;
    public event EventHandler OnHealthChanged;
    public event EventHandler OnPostureChanged;

    public int healthAmount;
    private int healthAmountMax;
    public int postureAmount;
    public int postureAmountMax;

    public HealthPostureSystem() {
        healthAmountMax = 100;
        postureAmountMax = 100;
        healthAmount = healthAmountMax;
        postureAmount = 0;
    }

    public float GetHealthNormalized() {
        return (float)healthAmount / healthAmountMax;
    }

    public float GetPostureNormalized() {
        return (float)postureAmount / postureAmountMax;
    }

    public void HealthDamage(int damageAmount) {
        healthAmount -= damageAmount;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);

        if (healthAmount <= 0) {
            // Character is Dead
            healthAmount = 0;
            if (OnDead != null) OnDead(this, EventArgs.Empty);
        }
    }

    public void HealthHeal(int healAmount) {
        healthAmount += healAmount;
        if (healthAmount > healthAmountMax) healthAmount = healthAmountMax;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void PostureIncrease(int amount) {
        postureAmount += amount;
        if (OnPostureChanged != null) OnPostureChanged(this, EventArgs.Empty);

        if (postureAmount >= postureAmountMax) {
            // Posture broken
            postureAmount = postureAmountMax;
            if (OnPostureBroken != null) OnPostureBroken(this, EventArgs.Empty);
        }
    }

    public void PostureDecrease(int amount) {
        postureAmount -= amount;
        if (postureAmount <= 0) postureAmount = 0;
        if (OnPostureChanged != null) OnPostureChanged(this, EventArgs.Empty);
    }
}
