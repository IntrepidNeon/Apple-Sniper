using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Test : MonoBehaviour
    {
        public Mesh_explosion mesh_explosion;

        // Start is called before the first frame update
        private void OnEnable()
        {
            this.reset();
        }

        private void OnDisable()
        {
            this.mesh_explosion.destory_all_pieces();
        }

        // Update is called once per frame
        void Update()
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == this.gameObject.name)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        this.GetComponent<Collider>().enabled = false;

                        StartCoroutine(this.mesh_explosion.split_mesh(false));
                    }
                }

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.reset();
            }
        }

        public void reset()
        {
            if (this.gameObject.activeSelf == true)
            {
                //消除所有碎片
                this.mesh_explosion.destory_all_pieces();

                this.GetComponent<Collider>().enabled = true;

                this.GetComponent<Renderer>().enabled = true;
            }
        }
    }
