using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Rotate_self : MonoBehaviour
    {
        public float speed;
        public Vector3 rotate_direction;

        // Update is called once per frame
        void Update()
        {
            transform.RotateAround(this.GetComponent<MeshRenderer>().bounds.center, this.rotate_direction, this.speed);
        }

        public void set_direction_and_speed(Vector3 rotate_direction, float speed)
        {
            this.rotate_direction = rotate_direction;
            this.speed = speed;
        }
    }

