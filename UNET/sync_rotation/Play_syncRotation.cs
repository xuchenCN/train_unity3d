using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Resources.scripts
{
   class Play_syncRotation : NetworkBehaviour
   {
      [SyncVar]
      private Quaternion playerQuaternion;

      [SyncVar]
      private Vector3 newPosition;

      private float speed;
      private float syncThreshold;
      private float rotationSpeed;

      //[SerializeField]
      // private Transform playerTransform;

      void Start()
      {
         speed = GetComponent<TPlayerController>().moveSpeed;
         syncThreshold = GetComponent<TPlayerController>().syncPositionThreshold;
         rotationSpeed = GetComponent<TPlayerController>().rotationSpeed;
      }

      void FixedUpdate()
      {
         if (!isLocalPlayer)
         {
            if(transform.localRotation != playerQuaternion)
            {
               //Debug.Log("local " + transform.localRotation + " playerQuaternion " + playerQuaternion);
               //transform.localRotation = playerQuaternion;
               transform.localRotation = Quaternion.Lerp(transform.localRotation, playerQuaternion, Time.deltaTime * rotationSpeed);
            }
            Move_Torwards(transform.transform.position, newPosition, speed);

         }
         
      }
      /*
      void lerpRoation()
      {
         Debug.Log("local " + transform.localRotation + " playerQuaternion " + playerQuaternion);
         if (!isLocalPlayer)
         {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, playerQuaternion, Time.deltaTime * 15);
           
         }
      }
     
      [Command]
      void CmdProviderRotationToServer(Quaternion playerRota)
      {
         this.playerQuaternion = playerRota;
      }

      [Client]
      public void TransmitRotation(Quaternion playerQuaternion)
      {
         if(isLocalPlayer)
         {
            CmdProviderRotationToServer(playerQuaternion);
         }
      }
       */

      [Command]
      void CmdProviderNewPositionToServer(Vector3 playerPos, Quaternion playerQuaternion)
      {
         this.newPosition = playerPos;
         this.playerQuaternion = playerQuaternion;
      }

      [Client]
      public void Transmit(Vector3 playerPos, Quaternion playerQuaternion)
      {
         if (isLocalPlayer)
         {
            CmdProviderNewPositionToServer(playerPos, playerQuaternion);
         }
      }


      private bool Move_Torwards(Vector3 startPos, Vector3 endPos, float moveMax)
      {
         if (Vector3.Distance(startPos, endPos) <= syncThreshold)
         {
            return true;
         }
         else
         {
            Vector3 moveForward = endPos - startPos;
            moveForward.y = 0;
            moveForward.Normalize();
            //Quaternion lerpRota = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(moveForward), Time.deltaTime * 15);
            //Debug.Log(startPos + ":" + endPos + ":" + lerpRota);
            float maxDistanceDelta = Time.deltaTime * moveMax;   
            Vector3 targetPos = Vector3.MoveTowards(transform.position, endPos, maxDistanceDelta);
            //Debug.Log("MoveTowards:" + targetPos + " maxDistanceDelta " + maxDistanceDelta);
            transform.position = targetPos;
            return false;
         }

      }
   }
}
