using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;

    private Character character;
    private void Update()
    {
        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount-=Time.deltaTime;
        }

        powerImage.fillAmount=character.currentPower/character.maxPower;
    }

    /// <summary>
    /// 接受health百分比，变更血槽
    /// </summary>
    /// <param name="percentage">百分比：current/max</param>
    public void OnHealthChange(float percentage)
    {
        healthImage.fillAmount = percentage;
    }

    /// <summary>
    /// 冲刺耐力槽显示
    /// </summary>
    /// <param name="percentage">耐力百分比</param>
    public void OnPowerDisplay(Character character)
    {
        this.character=character;
    }
}
