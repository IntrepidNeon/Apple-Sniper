using UnityEngine;
using System.Collections;
using System.Collections.Generic;


    public class Mesh_explosion : MonoBehaviour
    {
        [Header("The number of fragments indicates how many fragments are generated, 1 means all fragments, 2 means one-half generation, 3 means one-third, and so on")]
        public int piece_multiple;

        [Header("The size of the fragment, 1 represents the original size, 2 represents 2 times the size, and 3 represents 3 times the size")]
        public int piece_size_scale;

        [Header("Whether the fragments need to be destroyed")]
        public bool is_need_piece_destory;

        [Header("Debris destruction time")]
        public float[] piece_destory_time = new float[2];

        [Header("Let the fragments rotate or crack apart by force")]
        public bool use_rotation_or_Force;

        [Header("Direction of rotation")]
        public Vector3 rotate_direction;

        [Header("Speed of rotation")]
        public float rotate_speed;

        [Header("Use explosive force or directional force, true is explosive force, false is directional force")]
        public bool use_explosion_Force_or_direction;

        [Header("The magnitude and radius of the explosive force")]
        public float explosion_force_strengh;
        public float explosion_force_radius;

        [Header("Directional force vector")]
        [SerializeField] private Vector3 v3_direction_force;

        [Header("Audio Source  Audio Clip")]
        public AudioSource audio_source;
        public AudioClip audio_clip_explosion;

        private List<GameObject> list_piece = new List<GameObject>();

        public void setForceDirection(Vector3 newDirection)
        {
            v3_direction_force = newDirection;
        }
        public IEnumerator split_mesh(bool destroy)
        {
            //Must contain MeshFilter or SkinnedMeshRenderer components MeshFilterSkinnedMeshRenderer
            if (this.GetComponent<MeshFilter>() == null || this.GetComponent<SkinnedMeshRenderer>() == null)
                yield return null;

            //Disable collider collider
            if (this.GetComponent<Collider>())
                this.GetComponent<Collider>().enabled = false;

            //Get mesh mesh
            Mesh M = new Mesh();
            if (this.GetComponent<MeshFilter>())
                M = GetComponent<MeshFilter>().mesh;
            else if (this.GetComponent<SkinnedMeshRenderer>())
                M = GetComponent<SkinnedMeshRenderer>().sharedMesh;

            //Get material material
            Material[] materials = new Material[0];
            if (GetComponent<MeshRenderer>())
                materials = GetComponent<MeshRenderer>().materials;
            else if (GetComponent<SkinnedMeshRenderer>())
                materials = GetComponent<SkinnedMeshRenderer>().materials;

            //Cut the object into pieces
            Vector3[] verts = M.vertices;
            Vector3[] normals = M.normals;
            Vector2[] uvs = M.uv;
            for (int submesh = 0; submesh < M.subMeshCount; submesh++)
            {

                int[] indices = M.GetTriangles(submesh);

                for (int i = 0; i < indices.Length; i += (3 * this.piece_multiple))
                {
                    Vector3[] newVerts = new Vector3[3];
                    Vector3[] newNormals = new Vector3[3];
                    Vector2[] newUvs = new Vector2[3];
                    for (int n = 0; n < 3; n++)
                    {
                        int index = indices[i + n];
                        newVerts[n] = verts[index];
                        newNormals[n] = normals[index];

                        if (uvs.Length >= (index + 1))
                            newUvs[n] = uvs[index];
                    }

                    Mesh mesh = new Mesh();
                    mesh.vertices = newVerts;
                    mesh.normals = newNormals;
                    mesh.uv = newUvs;

                    mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                    GameObject game_object_piece = new GameObject("piece" + (i / 3));

                    game_object_piece.transform.rotation = transform.rotation;


                    game_object_piece.AddComponent<MeshRenderer>().material = materials[submesh];
                    game_object_piece.AddComponent<MeshFilter>().mesh = mesh;

                    game_object_piece.transform.localScale = transform.localScale * this.piece_size_scale;
                    game_object_piece.transform.position = transform.position;

                    Vector3 v = (game_object_piece.GetComponent<MeshRenderer>().bounds.center - transform.position) / this.piece_size_scale * (this.piece_size_scale - 1);
                    game_object_piece.transform.position = transform.position - v;

                    //rotate
                    if (this.use_rotation_or_Force)
                    {
                        Vector3 v3 = game_object_piece.GetComponent<MeshRenderer>().bounds.center;
                        game_object_piece.AddComponent<Rotate_self>().set_direction_and_speed(this.rotate_direction, this.rotate_speed);

                    }
                    //Force, separation
                    else
                    {
                        game_object_piece.AddComponent<BoxCollider>();

                        if (this.use_explosion_Force_or_direction)
                            game_object_piece.AddComponent<Rigidbody>().AddExplosionForce(this.explosion_force_strengh, transform.position, this.explosion_force_radius);
                        else
                            game_object_piece.AddComponent<Rigidbody>().AddForce(this.v3_direction_force, ForceMode.Force);

                        //play audio
                        if (this.audio_source != null && this.audio_clip_explosion != null)
                        {
                            this.audio_source.PlayOneShot(this.audio_clip_explosion);
                        }
                    }

                    //Destroy the pieces
                    if (this.is_need_piece_destory)
                        Destroy(game_object_piece, Random.Range(this.piece_destory_time[0], this.piece_destory_time[1]));

                    //add to list
                    this.list_piece.Add(game_object_piece);
                }
            }

            //Do not display the original mesh
            this.GetComponent<Renderer>().enabled = false;

            //Destroy objects if needed
            yield return new WaitForSeconds(1.0f);
            if (destroy == true)
                Destroy(this.gameObject);
        }

        //destory pieces
        public void destory_all_pieces()
        {
            for (int i = 0; i < this.list_piece.Count; i++)
            {
                GameObject obj = this.list_piece[i];
                Destroy(obj);
            }
        }
    }
