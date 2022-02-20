using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class first_script : MonoBehaviour


{
    public GameObject wooden_tile;
    public GameObject wall;
    public GameObject door;
    public GameObject new_door;
    public GameObject obj_direction;
    public GameObject random_wall;
    public GameObject Cube_1;

    GameObject tile;
    GameObject clone_wallx;
    GameObject clone_wallx2;
    GameObject clone_wallz;
    GameObject clone_wallz2;

    bool start;

    public Vector3 dir;
    public Vector3 dir2;
    public Vector3 dir_negative;
    public Vector3 dir2_negative;
    public Vector3 targetDirect;
    Vector3 tile_pos;
    Vector3 wall_posx;
    Vector3 wall_posx2;
    Vector3 wall_posz;
    Vector3 wall_posz2;

    public float room_spawnpoint_x;
    public float room_spawnpoint_z;
    public float room_spawnpoint_x_offset = 0;
    public float room_spawnpoint_z_offset = 0;
    public float room_x;
    public float room_z;
    public int direction_room;
    public List<int> alternate_num;

    float height = 0;
    float spx_direction;
    float spz_direction;
    float midpoint_x;
    float midpoint_z;
    int room;
    int rand;
    int random_tile_num;
    Vector3 collider_size;
    Vector3 collider_pos;
    Collider[] room_collider;
    Collider[] door_Collider;
    Collider[] door_Collider2;
    Collider[] door_Collider3;
    Collider[] door_Collider4;
    Collider[] tile_Collider;
    Collider[] tile_Collider2;
    Collider[] tile_Collider3;
    Collider[] tile_Collider4;
    Collider[] wall_Collider;
    Collider[] wall_Collider2;
    Collider[] wall_Collider3;
    Collider[] wall_Collider4;
    Collider[] wall_Collider5;

    Vector3 new_door_pos;
    List<GameObject> tile_list;
    List<GameObject> wall_list;
    List<GameObject> door_list;
    List<GameObject> room_wall_list;

    int rand_num;
    int random_number;
    bool s;
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collider_pos, collider_size);
        Gizmos.color = Color.blue;
        if (obj_direction)
        {
            Gizmos.DrawWireCube(obj_direction.transform.position, new Vector3(1f, 3f, 1f));
        }

        //Gizmos.DrawWireCube(new Vector3(new_door_pos.x + 1, new_door_pos.y, new_door_pos.z), new Vector3(0.5f, 0.5f, 0.5f));
        //Gizmos.DrawWireCube(new Vector3(new_door_pos.x - 1, new_door_pos.y, new_door_pos.z), new Vector3(0.5f, 0.5f, 0.5f));
        //Gizmos.DrawWireCube(new Vector3(new_door_pos.x, new_door_pos.y, new_door_pos.z + 1), new Vector3(0.5f, 0.5f, 0.5f));
        //Gizmos.DrawWireCube(new Vector3(new_door_pos.x, new_door_pos.y, new_door_pos.z - 1), new Vector3(0.5f, 0.5f, 0.5f));
        /*
        if (s == true)
        {
            Gizmos.DrawWireCube(new Vector3(wall_posz.x, wall_posz.y, wall_posz.z - 0.7f), new Vector3(0.2f, 0.5f, 0.2f));
            Gizmos.DrawWireCube(new Vector3(wall_posz.x, wall_posz.y, wall_posz.z + 0.7f), new Vector3(0.2f, 0.5f, 0.2f));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(new Vector3(room_spawnpoint_x_offset + (0.9f * spx_direction * -1), 2.0f, room_spawnpoint_z_offset + 0 * 2 * spz_direction), new Vector3(0.2f, 0.5f, 0.2f));

        }
        
        */

        //room_collider = Physics.OverlapBox(collider_pos, collider_size, Quaternion.identity);
        //collider_size = new Vector3(room_x_abs, 3f, room_z_abs);
        //collider_pos = new Vector3(room_spawnpoint_x + room_x * spx_direction, 1f, room_spawnpoint_z + room_z * spz_direction);


    }


    // Start is called before the first frame update
    void Start()
    {
        //start = true;



        Vector3 new_door_pos = new Vector3(0, 0, 0);
        room_spawnpoint_x = new_door_pos.x;
        room_spawnpoint_z = new_door_pos.z;

        tile_list = new List<GameObject>();
        wall_list = new List<GameObject>();
        door_list = new List<GameObject>();
        room_wall_list = new List<GameObject>();
        alternate_num = new List<int>();

        StartCoroutine(coroutine_mapgenerator());

    }



    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator coroutine_mapgenerator()
    {



        int name_cycle = 0;
        int v = 0;
        int[] polarity_num = { -1, 1 };

        //Instantiate(Cube_1, new Vector3(0, 0, 0), Quaternion.identity);


        for (room = 0; room <= 300; room++)
        {
            yield return new WaitForSeconds(2f);

            room_x = (int)Random.Range(2, 5) * 2 + 1;
            room_z = (int)Random.Range(2, 5) * 2 + 1;
            int random_direction = Random.Range(0, 2);
            int random_x = (Random.Range(0, 2)) * direction_room;
            int random_z = (Random.Range(0, 2)) * direction_room;
            direction_room = polarity_num[random_direction];
            midpoint_x = (room_x) / 2 * (direction_room * 2);
            midpoint_z = (room_z) / 2 * (direction_room * 2);
            float room_x_abs = Mathf.Abs(room_x);
            float room_z_abs = Mathf.Abs(room_z);
            bool space_for_rooms = true;
            random_tile_num = Random.Range(-4, 4);

            spx_direction = direction_room;
            spz_direction = direction_room;

            //Debug.Log("room : " + room);
            if (room == 0)
            {

            }
            else
            {
                if (spx_direction > 0)
                {
                    spx_direction = 1;
                }
                else
                {
                    spx_direction = -1;
                }

                if (spz_direction > 0)
                {
                    spz_direction = 1;
                }
                else
                {
                    spz_direction = -1;
                }


                //Find random wall in the room

                //get position of the random wall
                //rand_num = Random.Range(0, wall_list.Count);
                //static Random rnd = new Random();
                //rand_num = rnd.Next(0, wall_list.Count);
                //random_wall = wall_list[rand_num];

                for (int q = 0; q < 100; q++)
                {
                    for (int i = 0; i > -1; i++)
                    {
                        rand_num = Random.Range(0, wall_list.Count);
                        random_wall = wall_list[rand_num];
                        if (random_wall != null)
                        {
                            break;
                        }
                    }
                    try
                    {
                        new_door_pos = new Vector3(random_wall.transform.position.x, height + 2.0f, random_wall.transform.position.z);
                        //Debug.Log("newdoorpos : " + new_door_pos);
                    }
                    catch (Exception)
                    {

                    }

                    obj_direction = Instantiate(door, new Vector3(random_wall.transform.position.x, height + 2.0f, random_wall.transform.position.z), Quaternion.Euler(random_wall.transform.rotation.x, random_wall.transform.rotation.y, random_wall.transform.rotation.z));

                    dir = new Vector3(new_door_pos.x, 2, midpoint_z);
                    dir2 = new Vector3(midpoint_x, 2, new_door_pos.z);

                    int define_xz = 0;
                    Collider[] Colliders_detect = Physics.OverlapBox(dir, new Vector3(0.2f, 1.8f, 0.2f), Quaternion.identity);
                    //Check when there is a new collider coming into contact with the box
                    if (0 != Colliders_detect.Length)
                    {
                        //offset_x = 0;
                        define_xz = 0;
                        Debug.Log("direction x : " + obj_direction.transform.position);
                    }

                    Collider[] Colliders_detect2 = Physics.OverlapBox(dir2, new Vector3(0.2f, 1.8f, 0.2f), Quaternion.identity);
                    if (0 != Colliders_detect2.Length)
                    {
                        //offset_z = 0;
                        define_xz = 1;
                        Debug.Log("direction z : " + obj_direction.transform.position);
                    }


                    dir_negative = new Vector3(new_door_pos.x, 2, midpoint_z);
                    dir2_negative = new Vector3(midpoint_x, 2, new_door_pos.z);


                    if (define_xz == 0)
                    {

                        targetDirect = (dir2_negative - new_door_pos) * -1;
                        obj_direction.transform.rotation = Quaternion.LookRotation(targetDirect);


                    }
                    else
                    {

                        targetDirect = (dir_negative - new_door_pos) * -1;
                        obj_direction.transform.rotation = Quaternion.LookRotation(targetDirect);
                        //Debug.Log("direction2 : " + obj_direction.transform.position);

                    }
                    Vector3 obj_pos = obj_direction.transform.position;
                    Vector3 obj_posplus = obj_direction.transform.position + obj_direction.transform.forward;
                    room_spawnpoint_x = obj_posplus.x;
                    room_spawnpoint_z = obj_posplus.z;
                    Destroy(obj_direction);

                    //Debug.Log("obj_pos_x : " + obj_pos.x % 2);
                    //Debug.Log("obj_pos_z : " + obj_pos.z % 2);


                    if (obj_pos.x % 2==0 && obj_pos.z % 2 == 0)
                    {
                        break;
                    }
                    else
                    {
                        break;
                    }
                }






            }
            Debug.Log("room_spawnpoint_x : " + room_spawnpoint_x);
            Debug.Log("room_spawnpoint_z : " + room_spawnpoint_z);
            //Debug.Log("midpoint = " + new Vector3(midpoint_x, 0, midpoint_z));


            //check for space to generate room
            room_collider = Physics.OverlapBox(collider_pos, collider_size, Quaternion.identity);
            //collider_size = new Vector3(room_x, 3f, room_z);
            //collider_pos = new Vector3(room_spawnpoint_x + room_x * spx_direction, 1f, room_spawnpoint_z + room_z * spz_direction);

            if (room_collider.Length != 0)
            {
 
                //space_for_rooms = false;
                /*
                for (int i = 1; i < room_collider.Length; i++)
                {
                    Debug.Log("roomcolllength : " + room_collider[i].gameObject);
                }
                */

            }

            //Debug.Log("space_for_room : " + space_for_rooms);
            if (space_for_rooms == true)
            {


                //when run the code at least second times
                if (room >= 1)
                {
                    /*
                    wall_Collider = Physics.OverlapBox(random_wall.transform.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
                    if (wall_Collider.Length == 2)
                    {
                        Debug.Log("door_coll " + wall_Collider[1].gameObject);
                        Destroy(wall_Collider[1].gameObject);
                        Destroy(wall_Collider[0].gameObject);
                    }
                    */

                    //Destroy(random_wall);
                    //wall_list.Remove(random_wall);




                    door_Collider = Physics.OverlapBox(new Vector3(new_door_pos.x + 1, 2, new_door_pos.z), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
                    door_Collider2 = Physics.OverlapBox(new Vector3(new_door_pos.x - 1, 2, new_door_pos.z), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
                    door_Collider3 = Physics.OverlapBox(new Vector3(new_door_pos.x, 2, new_door_pos.z + 1), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
                    door_Collider4 = Physics.OverlapBox(new Vector3(new_door_pos.x, 2, new_door_pos.z - 1), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);

                    Boolean doorcollx = door_Collider.Length == 0 && door_Collider2.Length == 0;
                    Boolean doorcollz = door_Collider3.Length == 0 && door_Collider4.Length == 0;

                    new_door = Instantiate(door, new_door_pos, Quaternion.Euler(random_wall.transform.rotation.x, random_wall.transform.rotation.y, random_wall.transform.rotation.z));

                    new_door.name = "door" + v.ToString();
                    v = name_cycle++;
                    door_list.Add(new_door);
                    if (doorcollx || doorcollz)
                    {

                    }
                    else
                    {
                        Destroy(new_door);
                    }

                    //room_spawnpoint_x = new_door_pos.x + new_door.transform.forward.x;
                    //room_spawnpoint_z = new_door_pos.z + new_door.transform.forward.z;
                    //room_spawnpoint_x = new_door_pos.x + obj_direction.transform.forward.x;
                    //room_spawnpoint_z = new_door_pos.z + obj_direction.transform.forward.z;


                }
                //room position offset
                //float room_spawnpoint_x_offset = room_spawnpoint_x + random_tile_num * spx_direction * offset_x;
                //float room_spawnpoint_z_offset = room_spawnpoint_z + random_tile_num * spz_direction * offset_z;
                //float room_spawnpoint_x_offset = room_spawnpoint_x;
                //float room_spawnpoint_z_offset = room_spawnpoint_z;

                int tile_num = 0;

                //generating tiles
                for (int i = 0; i <= room_z_abs - 1; i++)
                {
                    for (int j = 0; j <= room_x_abs - 1; j++)
                    {
                        //yield return new WaitForSeconds(0.1f);
                        tile_pos = new Vector3(room_spawnpoint_x + j * 2 * spx_direction, height, room_spawnpoint_z + i * 2 * spz_direction);

                        tile_Collider = Physics.OverlapBox(new Vector3(tile_pos.x - 0.25f, height, tile_pos.z), new Vector3(0.1f, 0, 0.1f), Quaternion.identity);
                        tile_Collider2 = Physics.OverlapBox(new Vector3(tile_pos.x + 0.25f, height, tile_pos.z), new Vector3(0.1f, 0, 0.1f), Quaternion.identity);
                        tile_Collider3 = Physics.OverlapBox(new Vector3(tile_pos.x, height, tile_pos.z - 0.25f), new Vector3(0.1f, 0, 0.1f), Quaternion.identity);
                        tile_Collider4 = Physics.OverlapBox(new Vector3(tile_pos.x, height, tile_pos.z + 0.25f), new Vector3(0.1f, 0, 0.1f), Quaternion.identity);

                        if (tile_Collider.Length == 0 || tile_Collider2.Length == 0 || tile_Collider3.Length == 0 || tile_Collider4.Length == 0)
                        {
                            tile = Instantiate(wooden_tile, tile_pos, Quaternion.Euler(90, 115, 115));
                            tile_list.Add(tile);
                        }
                        else
                        {
                            //tile_list.Add(tile_Collider[0].gameObject);
                        }

                        tile.name = "tile" + v.ToString();
                        v = name_cycle++;
                        tile_num++;
                    }



                }
                /*
                for (int t = 0; t <= tile_list.Count - 1; t++)
                {
                    Debug.Log(tile_list[t].transform.position);
                }
                */
                //Debug.Log("tilenum :" + tile_num);
                //Debug.Log("Coll_pos1 : " + collider_pos);
                Vector3 first_tile_pos = tile_list[0].transform.position;
                Vector3 last_tile_pos = tile_list[tile_list.Count - 1].transform.position;
                Vector3 middle_tile_pos = new Vector3((first_tile_pos.x + last_tile_pos.x) / 2, height, (first_tile_pos.z + last_tile_pos.z) / 2);
                collider_size = new Vector3(room_x * 2, 3f, room_z * 2);
                collider_pos = new Vector3(middle_tile_pos.x, 1, middle_tile_pos.z);
                Debug.Log("rx" + room_x);
                Debug.Log("rz" + room_z);
                Debug.Log(first_tile_pos);
                Debug.Log(last_tile_pos);
                Debug.Log(middle_tile_pos);
                room_collider = Physics.OverlapBox(collider_pos, collider_size, Quaternion.identity);
                //Debug.Log("Coll_pos2 : " + collider_pos);

                if (room_collider.Length != 0)
                {
                    for (int coll = 0; coll <= room_collider.Length - 1; coll++)
                    {
                        //Debug.Log("roomwalllist count : " + (room_wall_list.Count));
                        //Debug.Log("colltotal count : " + (room_collider.Length));
                        for (int wall = 0; wall <= room_wall_list.Count; wall++)
                        {
                            //Debug.Log("wall count : " + wall);
                            //Debug.Log("coll count : " + coll);

                            if (room_collider[coll].gameObject.CompareTag("wall"))
                            {
                                GameObject collided_wall = room_collider[coll].gameObject;
                                Vector3 collided_wall_pos = room_collider[coll].gameObject.transform.position;
                                bool area_x = collided_wall_pos.x >= first_tile_pos.x && collided_wall_pos.x <= last_tile_pos.x;
                                bool negative_area_x = collided_wall_pos.x <= first_tile_pos.x && collided_wall_pos.x >= last_tile_pos.x;
                                bool area_z = collided_wall_pos.z >= first_tile_pos.z && collided_wall_pos.z <= last_tile_pos.z;
                                bool negative_area_z = collided_wall_pos.z <= first_tile_pos.z && collided_wall_pos.z >= last_tile_pos.z;
                                bool situation1 = area_x && area_z;
                                bool situation2 = negative_area_x && area_z;
                                bool situation3 = area_x && negative_area_z;
                                bool situation4 = negative_area_x && negative_area_z;
                                if (situation1 || situation2 || situation3 || situation4)
                                {
                                    //Destroy(collided_wall);
                                }

                                //Debug.Log("destroyed : " + room_collider[coll].gameObject);
                            }
                        }
                    }
                }

                tile_list.Clear();

                //List contains walls
                room_wall_list.Clear();

                //generating wall for x axis
                for (int j = 0; j <= room_x_abs - 1; j++)
                {
                    wall_posx = new Vector3(room_spawnpoint_x + j * 2 * spx_direction, height + 2.0f, room_spawnpoint_z + (1.1f * spz_direction * -1));
                    wallCollider_x(wall_posx);


                    if (wall_Collider.Length == 0 && wall_Collider2.Length == 0)
                    {
                        if (wall_Collider3.Length == 0 && wall_Collider4.Length == 0)
                        {

                            clone_wallx = Instantiate(wall, wall_posx, Quaternion.Euler(0, 0, 0));
                            clone_wallx.name = "wall_a" + v.ToString();
                            v = name_cycle++;
                            wall_list.Add(clone_wallx);
                            room_wall_list.Add(clone_wallx);
                        }

                    }



                    wall_posx2 = new Vector3(room_spawnpoint_x + j * 2 * spx_direction, height + 2.0f, (room_spawnpoint_z + midpoint_z * 2 - spz_direction * 2) + (1.1f * spz_direction));
                    wallCollider_x(wall_posx2);

                    if (wall_Collider.Length == 0 && wall_Collider2.Length == 0)
                    {
                        if (wall_Collider3.Length == 0 && wall_Collider4.Length == 0)
                        {
                            clone_wallx2 = Instantiate(wall, wall_posx2, Quaternion.Euler(0, 0, 0));
                            clone_wallx2.name = "wall_b" + v.ToString();
                            v = name_cycle++;
                            wall_list.Add(clone_wallx2);
                            room_wall_list.Add(clone_wallx2);
                        }

                    }

                }

                //generating wall for z axis
                for (int j = 0; j <= room_z_abs - 1; j++)
                {
                    wall_posz = new Vector3(room_spawnpoint_x + (1.1f * spx_direction * -1), height + 2.0f, room_spawnpoint_z + j * 2 * spz_direction);
                    wallCollider_z(wall_posz);

                    if (wall_Collider.Length == 0 && wall_Collider2.Length == 0)
                    {
                        if (wall_Collider3.Length == 0 && wall_Collider4.Length == 0)
                        {
                            clone_wallz = Instantiate(wall, wall_posz, Quaternion.Euler(0, 90, 0));
                            clone_wallz.name = "wall_c" + v.ToString();
                            v = name_cycle++;
                            wall_list.Add(clone_wallz);
                            room_wall_list.Add(clone_wallz);
                        }


                    }

                    wall_posz2 = new Vector3((room_spawnpoint_x + midpoint_x * 2 - spx_direction * 2) + (1.1f * spx_direction), height + 2.0f, room_spawnpoint_z + j * 2 * spz_direction);
                    wallCollider_z(wall_posz2);

                    if (wall_Collider.Length == 0 && wall_Collider2.Length == 0)
                    {
                        if (wall_Collider3.Length == 0 && wall_Collider4.Length == 0)
                        {
                            clone_wallz2 = Instantiate(wall, wall_posz2, Quaternion.Euler(0, 90, 0));
                            clone_wallz2.name = "wall_d" + v.ToString();
                            v = name_cycle++;
                            wall_list.Add(clone_wallz2);
                            room_wall_list.Add(clone_wallz2);
                        }

                    }

                }


            }

        }

        StartCoroutine(coroutine_doordestroy());
        yield return null;
    }

    void wallCollider_x(Vector3 wall_pos)
    {
        wall_Collider = Physics.OverlapBox(new Vector3(wall_pos.x + 333f, height + 2.0f, wall_pos.z), new Vector3(0.05f, 0, 0.05f), Quaternion.identity);
        wall_Collider2 = Physics.OverlapBox(new Vector3(wall_pos.x + 333f, height + 2.0f, wall_pos.z), new Vector3(0.05f, 0, 0.05f), Quaternion.identity);
        wall_Collider3 = Physics.OverlapBox(new Vector3(wall_pos.x, 2f, wall_pos.z + 333f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
        wall_Collider4 = Physics.OverlapBox(new Vector3(wall_pos.x, 2f, wall_pos.z + 333f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);

        wall_Collider5 = Physics.OverlapBox(wall_pos, new Vector3(0.2f, 0.5f, 0.2f), Quaternion.identity);
    }

    void wallCollider_z(Vector3 wall_pos)
    {
        wall_Collider = Physics.OverlapBox(new Vector3(wall_pos.x, height + 2.0f, wall_pos.z + 333f), new Vector3(0.05f, 0, 0.05f), Quaternion.identity);
        wall_Collider2 = Physics.OverlapBox(new Vector3(wall_pos.x, height + 2.0f, wall_pos.z + 333f), new Vector3(0.05f, 0, 0.05f), Quaternion.identity);
        wall_Collider3 = Physics.OverlapBox(new Vector3(wall_pos.x, 2f, wall_pos.z + 333f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
        wall_Collider4 = Physics.OverlapBox(new Vector3(wall_pos.x, 2f, wall_pos.z + 333f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);

        wall_Collider5 = Physics.OverlapBox(wall_pos, new Vector3(0.2f, 0.5f, 0.2f), Quaternion.identity);
    }


    IEnumerator coroutine_doordestroy()
    {
        yield return new WaitForSeconds(3);
        //Debug.Log("start");
        for (int i = 0; i < door_list.Count; i++)
        {
            random_number = Random.Range(1, 100 + 1);
            door_Collider = Physics.OverlapBox(door_list[i].transform.position, new Vector3(0.5f, 1.0f, 0.5f), Quaternion.identity);


            for (int j = 0; j < door_Collider.Length; j++)
            {
                Debug.Log("doorcoll : " + door_Collider[j].gameObject);
            }
            if (door_Collider.Length >= 2)
            {
                try
                {
                    for (int coll = 0; door_Collider.Length > 0; coll++)
                    {
                        if (door_Collider[coll].gameObject.CompareTag("door"))
                        {

                        }
                        else
                        {
                            //Destroy(door_Collider[coll].gameObject);
                        }

                    }
                    //Destroy(door_Collider[1].gameObject);
                    //Destroy(door_Collider[2].gameObject);
                }
                catch (Exception)
                {

                }


            }
            /*
            if (random_number <= 5)
            {
                Destroy(door_list[i]);
            }
            */

        }
        yield return null;

    }
}
