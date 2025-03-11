using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
   [Header("事件监听")]
   public  VoidEventSO afterSceneLoadedEvent;
   
   private CinemachineConfiner2D confiner2D;
   public CinemachineImpulseSource impulseSource;
   public VoidEventSO cameraShakeEvent;
   
   private void Awake()
   {
      confiner2D = GetComponent<CinemachineConfiner2D>();
      
   }

   private void OnEnable()
   {
      cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
      afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
   }

   private void OnDisable()
   {
      cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
      afterSceneLoadedEvent.OnEventRaised-=OnAfterSceneLoadedEvent;     
   }

   private void OnAfterSceneLoadedEvent()
   {
      GetNewCameraBounds();
   }

   private void OnCameraShakeEvent()
   {
      impulseSource.GenerateImpulse();
   }
   

   void GetNewCameraBounds()
   {
      var obj=GameObject.FindGameObjectWithTag("Bounds");
      if (obj == null)
         return;
      //将摄像机移动边界设置为bounds的碰撞体
      confiner2D.m_BoundingShape2D=obj.GetComponent<Collider2D>();
      //清除上一个边界的缓存
      confiner2D.InvalidateCache();
   }
}
