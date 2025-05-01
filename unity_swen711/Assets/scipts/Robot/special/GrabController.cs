using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class GrabController: MonoBehaviour
    {
        public Transform grabDetect;
        public Transform boxholder;
        public float rayDist;


        void Update()
        {
            RaycastHit2D grabCheck = Physics2D.Raycast(grabDetect.position, Vector2.up * transform.localScale, rayDist);

            if (grabCheck.collider != null && grabCheck.collider.tag == "Package")
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    grabCheck.collider.gameObject.transform.parent = boxholder;
                    grabCheck.collider.gameObject.transform.position = boxholder.position;
                    grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    
                }
                else
                {
                    grabCheck.collider.gameObject.transform.parent = null;
                    grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                }
            }
        }
    }
