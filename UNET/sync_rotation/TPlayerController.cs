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

      [SerializeField]
      private float m_moveSpeed = 5;
      public float moveSpeed { get { return m_moveSpeed; } }

      [SerializeField]
      private float m_syncPositionThreshold = 0.25f;
      public float syncPositionThreshold { get { return m_syncPositionThreshold; } }

      [SerializeField]
      private int m_rotationSpeed = 15;
      public int rotationSpeed { get { return m_rotationSpeed; } }

      void Awake()
      {

      }

      void Start()
      {
         if (isLocalPlayer)
         {
            GetComponent<Play_syncRotation>().Transmit(transform.transform.position, transform.rotation);
         }

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
         float distance = 0;
         if (transFormValue.x != 0 || transFormValue.z != 0)
         {
            distance = Vector3.Distance(transform.position, transFormValue);
            //this.rotation = Quaternion.LookRotation(transFormValue).eulerAngles;
            //CmdChangeRot(this.rotation);
            //transform.rotation = Quaternion.Euler(rotation);
            //transform.localRotation = Quaternion.LookRotation(transFormValue);
            Quaternion lerpRota = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(transFormValue), Time.deltaTime * 15);
            if (transform.localRotation != lerpRota)
            {
               transform.localRotation = lerpRota;
               //Debug.Log("sent " + transform.localRotation);
               //GetComponent<Play_syncRotation>().TransmitRotation(transform.localRotation);
            }

            //Debug.Log("update : " + GetComponent<NetworkIdentity>().netId + ":" + transform.rotation);
            //Debug.Log(h + "-" + v + transFormValue + "--" + m_TurnAmount);

            // transform.Rotate(0, 90, 0);
            transFormValue *= Time.deltaTime;

            // Debug.Log("transFormValue" + );

            transform.Translate(transFormValue * m_moveSpeed, Space.World);
            if (distance >= m_syncPositionThreshold)
            {
               GetComponent<Play_syncRotation>().Transmit(transform.transform.position, transform.localRotation);
            }

         }
         Camera.main.transform.LookAt(this.transform);
         Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 8f, this.transform.position.z + 8f);
      }

   }
}
