using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class Room_data
{
    public string first_tile_pos;
    public string last_tile_pos;
    public string middle_tile_pos;
    public string direction_polarity;
    //public string playerNick;
}

public class Map_generation : MonoBehaviour


{
    public GameObject tile_gameobject;
    public GameObject wall_gameobject;
    public GameObject door;
    public GameObject new_door;
    public GameObject direction_reference_obj;
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
    Dictionary<int, List<GameObject>> wall_dict;
    List<GameObject> door_list;
    List<GameObject> room_wall_list;

    int rand_num;
    int random_number;
    int key;
    int random_existing_room_index;
    // int room_spawnpoint_x_direction = 1;
    // int room_spawnpoint_z_direction = 1;
    string random_existing_room_mid;
    bool s;
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collider_pos, collider_size);
        Gizmos.DrawWireCube(dir, new Vector3(0.5f, 1f, 0.5f));
        Gizmos.DrawWireCube(dir2, new Vector3(0.5f, 1f, 0.5f));
        //Debug.Log("Gizmos, collider_pos : " + collider_pos + "| collider_size :" + collider_size);
        

        
        if (direction_reference_obj != null){
            Gizmos.color = Color.blue;
            Vector3 direction = direction_reference_obj.transform.TransformDirection(Vector3.forward) * 10;
            Gizmos.DrawRay(direction_reference_obj.transform.position, direction);
        }
        
        
        // try
        // {
        //     Gizmos.DrawRay(direction_reference_obj.transform.position, direction);
        // }
        // catch(Exception){}
        

        //Gizmos.DrawWireCube(new Vector3(new_door_pos.x + 1, new_door_pos.y, new_door_pos.z), new Vector3(0.5f, 0.5f, 0.5f));
        //Gizmos.DrawWireCube(new Vector3(new_door_pos.x - 1, new_door_pos.y, new_door_pos.z), new Vector3(0.5f, 0.5f, 0.5f));
        //Gizmos.DrawWireCube(new Vector3(new_door_pos.x, new_door_pos.y, new_door_pos.z + 1), new Vector3(0.5f, 0.5f, 0.5f));
        //Gizmos.DrawWireCube(new Vector3(new_door_pos.x, new_door_pos.y, new_door_pos.z - 1), new Vector3(0.5f, 0.5f, 0.5f));

    }


    // Start is called before the first frame update
    void Start()
    {

        // Initialization and defining variables
        Vector3 new_door_pos = new Vector3(1, 0, 1);
        room_spawnpoint_x = new_door_pos.x;
        room_spawnpoint_z = new_door_pos.z;

        tile_list = new List<GameObject>();
        wall_list = new List<GameObject>();
        wall_dict = new Dictionary<int, List<GameObject>>();
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
        int room_count = 800;
        Room_data[] roomproperties = new Room_data[room_count];
        List<int> existing_room_index = new List<int>();
        

        for (room = 0; room <= room_count; room++)
        {
            //Debug.Log("room : " + room);
            //yield return new WaitForSeconds(1.0f);

            // Random position with odd number
            room_x = (int)Random.Range(2, 5) * 2 + 1;
            room_z = (int)Random.Range(2, 5) * 2 + 1;

            // Random direction for generating next room
            // Negative or positive direction
            // Assign direction for next room
            int random_direction_polarity = Random.Range(0, 2);
            direction_room = polarity_num[random_direction_polarity];


            // Mid point of a room
            midpoint_x = (room_x) / 2 * (direction_room * 2);
            midpoint_z = (room_z) / 2 * (direction_room * 2);
            

            // absolute values of a position
            float room_x_abs = Mathf.Abs(room_x);
            float room_z_abs = Mathf.Abs(room_z);

            bool space_for_rooms = true;
            random_tile_num = Random.Range(-4, 4);

            // first room
            if (room == 0)
            {
                //first room is a reference for other rooms
            }
            else
            {

                // Mid point of a random existing room
                random_existing_room_index = existing_room_index[Random.Range(0, existing_room_index.Count)];
                random_existing_room_mid = roomproperties[random_existing_room_index].middle_tile_pos;
                random_existing_room_mid = random_existing_room_mid.Substring(1, random_existing_room_mid.Length-2);
                
                //Convert from String to Vector3
                string[] rermArray =  random_existing_room_mid.Split(',');
                Vector3 rand_exist_rom_mid = new Vector3(float.Parse(rermArray[0]), float.Parse(rermArray[1]), float.Parse(rermArray[2]));
                Debug.Log("midpoint_xz : " + rand_exist_rom_mid.x + " | " + rand_exist_rom_mid.z);

                //Find a random wall in the room
                for (int q = 0; q < 100; q++)
                {
                    for (int i = 0; i > -1; i++)
                    {
                        rand_num = Random.Range(0, wall_dict[random_existing_room_index].Count);
                        random_wall = wall_dict[random_existing_room_index][rand_num];
                        if (random_wall != null)
                        {
                            break;
                        }
                    }
                    try
                    {
                        new_door_pos = new Vector3(random_wall.transform.position.x, height + 2.0f, random_wall.transform.position.z);
                    }
                    catch(Exception){}

                    Vector3 random_wall_position = new Vector3(random_wall.transform.position.x, height + 2.0f,
                                                            random_wall.transform.position.z);
                                                                
                    Quaternion random_wall_rotation = Quaternion.Euler(random_wall.transform.rotation.x,
                                                                random_wall.transform.rotation.y,
                                                                random_wall.transform.rotation.z);                                      

                    // Generate a temporary door(object) to reference directions
                    if (direction_reference_obj){
                        direction_reference_obj.SetActive(true);
                        direction_reference_obj.transform.position = random_wall_position;
                        direction_reference_obj.transform.rotation = random_wall_rotation;
                    }
                    else
                    {
                        direction_reference_obj = Instantiate(door, random_wall_position, random_wall_rotation); 
                    }
                    Debug.Log("direction_ref_pos : " + direction_reference_obj.transform.position);

                    // Position references
                    dir = new Vector3(new_door_pos.x, 2, rand_exist_rom_mid.z);
                    dir2 = new Vector3(rand_exist_rom_mid.x, 2, new_door_pos.z);
                    //dir_negative = new Vector3(new_door_pos.x, 2, midpoint_z);
                    //dir2_negative = new Vector3(midpoint_x, 2, new_door_pos.z);


                    Collider[] Colliders_detect = Physics.OverlapBox(dir, new Vector3(0.2f, 1.8f, 0.2f), Quaternion.identity);
                    //Check when there is a new collider coming into contact with the box
                    if (0 != Colliders_detect.Length)
                    {
                        
                        targetDirect = (dir2 - new_door_pos) * -1;
                        direction_reference_obj.transform.rotation = Quaternion.LookRotation(targetDirect);
                        //Debug.Log("Collided in direction x : " + direction_reference_obj.transform.position);
                        Debug.Log("dir2 : " + dir2);
                    }

                    Collider[] Colliders_detect2 = Physics.OverlapBox(dir2, new Vector3(0.2f, 1.8f, 0.2f), Quaternion.identity);
                    if (0 != Colliders_detect2.Length)
                    {
                        
                        targetDirect = (dir - new_door_pos) * -1;
                        direction_reference_obj.transform.rotation = Quaternion.LookRotation(targetDirect);
                        //Debug.Log("Collided in direction z : " + direction_reference_obj.transform.position);
                        Debug.Log("dir : " + dir);
                    }

                    // Position references(negative values)
                    
                    // Get reference position
                    Vector3 pos_ref = new Vector3(Mathf.Round(direction_reference_obj.transform.position.x), 
                                                    direction_reference_obj.transform.position.y, 
                                                    Mathf.Round(direction_reference_obj.transform.position.z));
                    Debug.Log("pos_ref" + direction_reference_obj.transform.position);
                    // Get forward direction of the reference position
                    Vector3 pos_ref_plus = direction_reference_obj.transform.position + direction_reference_obj.transform.forward;
                    Vector3 pos_ref_plus_rounded = new Vector3(Mathf.Round(pos_ref_plus.x), pos_ref_plus.y, Mathf.Round(pos_ref_plus.z));
                    room_spawnpoint_x = pos_ref_plus_rounded.x;
                    room_spawnpoint_z = pos_ref_plus_rounded.z;

                    // if (room_spawnpoint_x < 0){
                    // room_spawnpoint_x_direction = -1;
                    // }
                    // if (room_spawnpoint_x > 0){
                    //     room_spawnpoint_x_direction = 1;
                    //     }
                    // if (room_spawnpoint_z < 0){
                    //     room_spawnpoint_z_direction = -1;
                    // }
                    // if (room_spawnpoint_z > 0){
                    //     room_spawnpoint_z_direction = 1;
                    // }
                    //Debug.Log("room_sp_x : " + room_spawnpoint_x_direction);
                    //Debug.Log("room_sp_z : " + room_spawnpoint_z_direction);

                    direction_reference_obj.SetActive(false);

                    if (pos_ref.x % 2==0 && pos_ref.z % 2 == 0)
                    {
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            Debug.Log("room_spawnpoint_x_z : " + $"({room_spawnpoint_x}" + $", {room_spawnpoint_z})");


            //Check for empty space to generate room with room_collider
            collider_size = new Vector3(room_x * 1.9f, 3f, room_z * 1.9f);
            collider_pos = new Vector3(room_spawnpoint_x + (room_x-1) * direction_room, 1f, room_spawnpoint_z + (room_z-1) * direction_room);
            Debug.Log("collider_pos : " + collider_pos);
            Debug.Log("roomxz : " + room_x + " | " + room_z);
            room_collider = Physics.OverlapBox(collider_pos, collider_size / 2, Quaternion.identity);
            if (room_collider.Length != 0)
            {
                //Debug.Log("Collision :" + room_collider[0].gameObject);
                //Debug.Log("Collision_length :" + room_collider.Length);
                //Debug.Log("nospace, collider_pos : " + collider_pos + "| collider_size :" + collider_size);

                space_for_rooms = false;
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
                //Serializing Json data
                roomproperties[room] = new Room_data();
                roomproperties[room].direction_polarity = random_direction_polarity.ToString();
                existing_room_index.Add(room);

                

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
                    // if (doorcollx || doorcollz)
                    // {

                    // }
                    // else
                    // {
                    //     Destroy(new_door);
                    // }

                    //room_spawnpoint_x = new_door_pos.x + new_door.transform.forward.x;
                    //room_spawnpoint_z = new_door_pos.z + new_door.transform.forward.z;
                    //room_spawnpoint_x = new_door_pos.x + direction_reference_obj.transform.forward.x;
                    //room_spawnpoint_z = new_door_pos.z + direction_reference_obj.transform.forward.z;


                }

                int tile_num = 0;

                //generating tiles
                for (int i = 0; i <= room_z_abs - 1; i++)
                {
                    for (int j = 0; j <= room_x_abs - 1; j++)
                    {
                        //yield return new WaitForSeconds(0.1f);
                        tile_pos = new Vector3(room_spawnpoint_x + j * 2 * direction_room, height, room_spawnpoint_z + i * 2 * direction_room);

                        tile_Collider = Physics.OverlapBox(new Vector3(tile_pos.x - 0.25f, height, tile_pos.z), new Vector3(0.1f, 0, 0.1f), Quaternion.identity);
                        tile_Collider2 = Physics.OverlapBox(new Vector3(tile_pos.x + 0.25f, height, tile_pos.z), new Vector3(0.1f, 0, 0.1f), Quaternion.identity);
                        tile_Collider3 = Physics.OverlapBox(new Vector3(tile_pos.x, height, tile_pos.z - 0.25f), new Vector3(0.1f, 0, 0.1f), Quaternion.identity);
                        tile_Collider4 = Physics.OverlapBox(new Vector3(tile_pos.x, height, tile_pos.z + 0.25f), new Vector3(0.1f, 0, 0.1f), Quaternion.identity);

                        if (tile_Collider.Length == 0 || tile_Collider2.Length == 0 || tile_Collider3.Length == 0 || tile_Collider4.Length == 0)
                        {
                            tile = Instantiate(tile_gameobject, tile_pos, Quaternion.Euler(90, 115, 115));
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

                Vector3 first_tile_pos = tile_list[0].transform.position;
                Vector3 last_tile_pos = tile_list[tile_list.Count - 1].transform.position;
                Vector3 middle_tile_pos = new Vector3((first_tile_pos.x + last_tile_pos.x) / 2, height, (first_tile_pos.z + last_tile_pos.z) / 2);
                //collider_size = new Vector3(room_x * 2, 3f, room_z * 2);
                //collider_pos = new Vector3(middle_tile_pos.x, 1, middle_tile_pos.z);
                //Debug.Log("roomsize_x : " + room_x + " | " + "roomsize_z : " + room_z);
                //Debug.Log("first/middle/last tile :" + first_tile_pos + " " + middle_tile_pos + " " + last_tile_pos);

                //room_collider = Physics.OverlapBox(collider_pos, collider_size, Quaternion.identity);
                roomproperties[room].first_tile_pos = first_tile_pos.ToString();
                roomproperties[room].last_tile_pos = last_tile_pos.ToString();
                roomproperties[room].middle_tile_pos = middle_tile_pos.ToString();

                // if (room_collider.Length != 0)
                // {
                //     for (int coll = 0; coll <= room_collider.Length - 1; coll++)
                //     {
                //         //Debug.Log("roomwalllist count : " + (room_wall_list.Count));
                //         //Debug.Log("colltotal count : " + (room_collider.Length));
                //         for (int wall = 0; wall <= room_wall_list.Count; wall++)
                //         {
                //             //Debug.Log("wall count : " + wall);
                //             //Debug.Log("coll count : " + coll);

                //             if (room_collider[coll].gameObject.CompareTag("wall"))
                //             {
                //                 GameObject collided_wall = room_collider[coll].gameObject;
                //                 Vector3 collided_wall_pos = room_collider[coll].gameObject.transform.position;
                //                 bool area_x = collided_wall_pos.x >= first_tile_pos.x && collided_wall_pos.x <= last_tile_pos.x;
                //                 bool negative_area_x = collided_wall_pos.x <= first_tile_pos.x && collided_wall_pos.x >= last_tile_pos.x;
                //                 bool area_z = collided_wall_pos.z >= first_tile_pos.z && collided_wall_pos.z <= last_tile_pos.z;
                //                 bool negative_area_z = collided_wall_pos.z <= first_tile_pos.z && collided_wall_pos.z >= last_tile_pos.z;
                //                 bool situation1 = area_x && area_z;
                //                 bool situation2 = negative_area_x && area_z;
                //                 bool situation3 = area_x && negative_area_z;
                //                 bool situation4 = negative_area_x && negative_area_z;
                //                 if (situation1 || situation2 || situation3 || situation4)
                //                 {
                //                     //Destroy(collided_wall);
                //                 }

                //                 //Debug.Log("destroyed : " + room_collider[coll].gameObject);
                //             }
                //         }
                //     }
                // }

                tile_list.Clear();

                //List contains walls and will be cleared in every loop
                room_wall_list.Clear();

                //generating wall for x axis
                for (int j = 0; j <= room_x_abs - 1; j++)
                {
                    wall_posx = new Vector3(room_spawnpoint_x + j * 2 * direction_room, height + 2.0f, room_spawnpoint_z + (1.0f * direction_room * -1));
                    wallCollider_x(wall_posx);

                    if (wall_Collider.Length == 0){
                        clone_wallx = Instantiate(wall_gameobject, wall_posx, Quaternion.Euler(0, 0, 0));
                        clone_wallx.name = "wall_a" + v.ToString();
                        v = name_cycle++;
                        wall_list.Add(clone_wallx);
                        room_wall_list.Add(clone_wallx);

                    }
                    // if (wall_Collider.Length == 0 && wall_Collider2.Length == 0)
                    // {
                    //     if (wall_Collider3.Length == 0 && wall_Collider4.Length == 0)
                    //     {


                    //     }

                    // }



                    wall_posx2 = new Vector3(room_spawnpoint_x + j * 2 * direction_room, height + 2.0f, (room_spawnpoint_z + midpoint_z * 2 - direction_room * 2) + (1.0f * direction_room));
                    wallCollider_x(wall_posx2);

                    if (wall_Collider.Length == 0){
                        clone_wallx2 = Instantiate(wall_gameobject, wall_posx2, Quaternion.Euler(0, 0, 0));
                        clone_wallx2.name = "wall_b" + v.ToString();
                        v = name_cycle++;
                        wall_list.Add(clone_wallx2);
                        room_wall_list.Add(clone_wallx2);

                    }

                //     if (wall_Collider.Length == 0 && wall_Collider2.Length == 0)
                //     {
                //         if (wall_Collider3.Length == 0 && wall_Collider4.Length == 0)
                //         {
                //             clone_wallx2 = Instantiate(wall, wall_posx2, Quaternion.Euler(0, 0, 0));
                //             clone_wallx2.name = "wall_b" + v.ToString();
                //             v = name_cycle++;
                //             wall_list.Add(clone_wallx2);
                //             room_wall_list.Add(clone_wallx2);
                //         }

                //     }

                }

                //generating wall for z axis
                for (int j = 0; j <= room_z_abs - 1; j++)
                {
                    wall_posz = new Vector3(room_spawnpoint_x + (1.0f * direction_room * -1), height + 2.0f, room_spawnpoint_z + j * 2 * direction_room);
                    wallCollider_z(wall_posz);

                    if (wall_Collider.Length == 0){
                        clone_wallz = Instantiate(wall_gameobject, wall_posz, Quaternion.Euler(0, 90, 0));
                        clone_wallz.name = "wall_c" + v.ToString();
                        v = name_cycle++;
                        wall_list.Add(clone_wallz);
                        room_wall_list.Add(clone_wallz);

                    }

                    // if (wall_Collider.Length == 0 && wall_Collider2.Length == 0)
                    // {
                    //     if (wall_Collider3.Length == 0 && wall_Collider4.Length == 0)
                    //     {
                    //         clone_wallz = Instantiate(wall, wall_posz, Quaternion.Euler(0, 90, 0));
                    //         clone_wallz.name = "wall_c" + v.ToString();
                    //         v = name_cycle++;
                    //         wall_list.Add(clone_wallz);
                    //         room_wall_list.Add(clone_wallz);
                    //     }


                    // }

                    wall_posz2 = new Vector3((room_spawnpoint_x + midpoint_x * 2 - direction_room * 2) + (1.0f * direction_room), height + 2.0f, room_spawnpoint_z + j * 2 * direction_room);
                    wallCollider_z(wall_posz2);

                    if (wall_Collider.Length == 0){
                        clone_wallz2 = Instantiate(wall_gameobject, wall_posz2, Quaternion.Euler(0, 90, 0));
                        clone_wallz2.name = "wall_d" + v.ToString();
                        v = name_cycle++;
                        wall_list.Add(clone_wallz2);
                        room_wall_list.Add(clone_wallz2);

                    }

                //     if (wall_Collider.Length == 0 && wall_Collider2.Length == 0)
                //     {
                //         if (wall_Collider3.Length == 0 && wall_Collider4.Length == 0)
                //         {
                //             clone_wallz2 = Instantiate(wall, wall_posz2, Quaternion.Euler(0, 90, 0));
                //             clone_wallz2.name = "wall_d" + v.ToString();
                //             v = name_cycle++;
                //             wall_list.Add(clone_wallz2);
                //             room_wall_list.Add(clone_wallz2);
                //         }

                //     }

                }
                key = existing_room_index[existing_room_index.Count - 1];
                wall_dict.Add(key, new List<GameObject>());
                for(int wall = 0; wall < room_wall_list.Count; wall++){
                    
                    wall_dict[key].Add(room_wall_list[wall]);
                }
                
                string roomjson = JsonHelper.ToJson(roomproperties, 0);
                Debug.Log(roomjson);

            }

            

        }

        StartCoroutine(coroutine_doordestroy());
        yield return null;
    }

    void wallCollider_x(Vector3 wall_pos)
    {
        wall_Collider = Physics.OverlapBox(wall_pos, new Vector3(0.05f, 0, 0.05f), Quaternion.identity);
        //wall_Collider = Physics.OverlapBox(new Vector3(wall_pos.x + 333f, height + 2.0f, wall_pos.z), new Vector3(0.05f, 0, 0.05f), Quaternion.identity);
        //wall_Collider2 = Physics.OverlapBox(new Vector3(wall_pos.x + 333f, height + 2.0f, wall_pos.z), new Vector3(0.05f, 0, 0.05f), Quaternion.identity);
        //wall_Collider3 = Physics.OverlapBox(new Vector3(wall_pos.x, 2f, wall_pos.z + 333f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
        //wall_Collider4 = Physics.OverlapBox(new Vector3(wall_pos.x, 2f, wall_pos.z + 333f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);

        //wall_Collider5 = Physics.OverlapBox(wall_pos, new Vector3(0.2f, 0.5f, 0.2f), Quaternion.identity);
    }

    void wallCollider_z(Vector3 wall_pos)
    {
        wall_Collider = Physics.OverlapBox(wall_pos, new Vector3(0.05f, 0, 0.05f), Quaternion.identity);

        //wall_Collider = Physics.OverlapBox(new Vector3(wall_pos.x, height + 2.0f, wall_pos.z + 333f), new Vector3(0.05f, 0, 0.05f), Quaternion.identity);
        //wall_Collider2 = Physics.OverlapBox(new Vector3(wall_pos.x, height + 2.0f, wall_pos.z + 333f), new Vector3(0.05f, 0, 0.05f), Quaternion.identity);
        //wall_Collider3 = Physics.OverlapBox(new Vector3(wall_pos.x, 2f, wall_pos.z + 333f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
        //wall_Collider4 = Physics.OverlapBox(new Vector3(wall_pos.x, 2f, wall_pos.z + 333f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);

        //wall_Collider5 = Physics.OverlapBox(wall_pos, new Vector3(0.2f, 0.5f, 0.2f), Quaternion.identity);
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

    public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Rooms;
    }

    // public static string ToJson<T>(T[] array)
    // {
    //     Wrapper<T> wrapper = new Wrapper<T>();
    //     wrapper.Rooms = array;
    //     return JsonUtility.ToJson(wrapper);
    // }

    public static string ToJson<T>(T[] array, int array_type)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        switch (array_type)
        {
        case 0:
            wrapper.Rooms = array;
            break;
        case 1:
            wrapper.Rooms_wall = array;
            break;
        }
        
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Rooms;
        public T[] Rooms_wall;
    }
}
}
