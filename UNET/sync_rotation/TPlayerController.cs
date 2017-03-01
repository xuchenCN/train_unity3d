using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Resources.scripts
{
   class TPlayerController : NetworkBehaviour
   {

      void Awake()
      {

      }

      void Start()
      {

      }

      void Update()
      {
         if (!isLocalPlayer)
         {
            return;
         }
         // CrossPlatformInputManager.GetButtonDown("w");

         float h = CrossPlatformInputManager.GetAxis("Horizontal");
         float v = CrossPlatformInputManager.GetAxis("Vertical");

         if (h != 0 || v != 0)
         {
            GetComponent<Animator>().SetBool("Run", true);
         }
         else if (h == 0 && v == 0)
         {
            GetComponent<Animator>().SetBool("Run", false);
         }

         // transFormValue = (v * Vector3.forward + h * Vector3.right) * Time.deltaTime;
         Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
         Vector3 transFormValue = v * m_CamForward + h * Camera.main.transform.right;
         if (transFormValue.x != 0 || transFormValue.z != 0)
         {
            //this.rotation = Quaternion.LookRotation(transFormValue).eulerAngles;
            //CmdChangeRot(this.rotation);
            //transform.rotation = Quaternion.Euler(rotation);
            //transform.localRotation = Quaternion.LookRotation(transFormValue);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(transFormValue), Time.deltaTime * 15);

            Debug.Log("sent " + transform.localRotation);
            GetComponent<Play_syncRotation>().TransmitRotation(transform.rotation);
         }
         //Debug.Log("update : " + GetComponent<NetworkIdentity>().netId + ":" + transform.rotation);
         //Debug.Log(h + "-" + v + transFormValue + "--" + m_TurnAmount);

         // transform.Rotate(0, 90, 0);
         transFormValue *= Time.deltaTime;

         // Debug.Log("transFormValue" + );

         transform.Translate(transFormValue * 5, Space.World);

         Camera.main.transform.LookAt(this.transform);
         Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 8f, this.transform.position.z + 8f);
      }

   }
}
