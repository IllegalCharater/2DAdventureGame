using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("事件监听")]
    public VoidEventSO newGameEvent;
    [Header("基本属性")] 
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;
    [Header("无敌时间设置")]
    public float invulnerableDuration;
    [HideInInspector]public float invulnerableCounter;
    public bool isInvulnerable;
    //传递受伤事件
    public UnityEvent<Character> OnHealthChanged;
    //受伤
    public UnityEvent<Transform> OnTakeDamage;
    //死亡
    public UnityEvent OnDeath;
    private void NewGameStart()
    {
        currentHealth = maxHealth;
        currentPower = maxPower;
        OnHealthChanged?.Invoke(this);//传递事件
    }

    private void Start()
    {
        NewGameStart();
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGameStart;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGameStart;
        ISaveable saveable = this;
        saveable.UnregisterSaveData();
    }

    private void Update()
    {
        //无敌时间计时器
        if (isInvulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                isInvulnerable = false;
            }
        }
        //耐力条回复
        if (currentPower < maxPower)
        {
            currentPower += powerRecoverSpeed * Time.deltaTime;
        }
    }

    //受伤事件
    public void TakeDamage(Attack attacker)
    {
        if(isInvulnerable)
             return;
        if (currentHealth - attacker.damage > 0)
        {
            //受伤
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            //受伤动画
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            //死亡
            currentHealth = 0;
            OnDeath?.Invoke();
        }
        OnHealthChanged?.Invoke(this);
    }

    //无敌时间计时器触发
    private void TriggerInvulnerable()
    {
        if (!isInvulnerable)
        {
            isInvulnerable = true;
            invulnerableCounter = invulnerableDuration;//重置无敌计时器和无敌状态
        }
    }
    //耐力条事件
    public void OnSlide(float cost)
    {
        currentPower -= cost;
        OnHealthChanged?.Invoke(this);
    }

    //遇水死亡
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            currentHealth = 0;
            OnHealthChanged?.Invoke(this);
            OnDeath?.Invoke();//死亡并刷新血条UI
        }
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveDate(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = transform.position;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, transform.position);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID];
        }
    }
}
