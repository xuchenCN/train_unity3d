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
     // [SyncVar]
      private Quaternion playerQuaternion;

      //[SerializeField]
      // private Transform playerTransform;

      void FixedUpdate()
      {
         if (!isLocalPlayer)
         {
            if(transform.localRotation != playerQuaternion)
            {
               Debug.Log("local " + transform.localRotation + " playerQuaternion " + playerQuaternion);
               //transform.localRotation = playerQuaternion;
               transform.localRotation = Quaternion.Lerp(transform.localRotation, playerQuaternion, Time.deltaTime * 15);
            }

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
      */
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

   }
}
