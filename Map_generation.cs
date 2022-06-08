using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Reflection;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public class Room_data
{
    public string id;
    public string entry_count = "1";
    public string room_type;
    public string nospace_count = "0";
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
    public GameObject door_gameobject;
    public GameObject random_wall;
    public GameObject player;
    public GameObject camera;

    GameObject direction_reference_obj;
    GameObject door;
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
    float midpoint_x;
    float midpoint_z;
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

    Vector3 new_door_pos;
    List<GameObject> tile_list;
    Dictionary<int, List<GameObject>> wall_dict;
    List<GameObject> door_list;
    List<GameObject> door_list_destroy;
    List<GameObject> room_wall_list;

    int rand_num;
    int random_number;
    int key;
    int random_existing_room_index;
    int random_existing_room2_index;
    int rand_range;
    int entry_count_toint;
    int nospace_count_toint;
    int room_class;
    int last_room_class;
    int rand_room2_index;
    int max_nospace_count;
    bool rand_room2_index_exist;
    string random_existing_room_mid;
    bool s;
    bool space_for_rooms;
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collider_pos, collider_size);
        //Gizmos.DrawWireCube(dir, new Vector3(0.5f, 1f, 0.5f));
        //Gizmos.DrawWireCube(dir2, new Vector3(0.5f, 1f, 0.5f));
        //Debug.Log("Gizmos, collider_pos : " + collider_pos + "| collider_size :" + collider_size);
        

        
        if (direction_reference_obj != null){
            Gizmos.color = Color.blue;
            Vector3 direction = direction_reference_obj.transform.TransformDirection(Vector3.forward) * 10;
            Gizmos.DrawRay(direction_reference_obj.transform.position, direction);
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {

        // Initialization and defining variables
        Vector3 new_door_pos = new Vector3(1, 0, 1);
        room_spawnpoint_x = new_door_pos.x;
        room_spawnpoint_z = new_door_pos.z;

        tile_list = new List<GameObject>();
        wall_dict = new Dictionary<int, List<GameObject>>();
        door_list = new List<GameObject>();
        door_list_destroy = new List<GameObject>();
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
        //v is object name id
        int v = 0;
        int[] polarity_num = { -1, 1 };
        int max_room_count = 200;
        int current_room_count = 0;
        int max_loop_count = max_room_count * 20;
        int max_nospace_count = 10;
        Room_data[] roomproperties = new Room_data[max_room_count];
        Room_data room_properties;

        List<int> existing_room_index = new List<int>();
        List<int> existing_room_space_index = new List<int>();
        List<int> existing_room_nospace_index = new List<int>();
        List<int> existing_room_nospace_index_debug = new List<int>();
        List<int> existing_room2_index = new List<int>();
        List<int> existing_room2_index_1entry = new List<int>();
        

        for (int loop = 0; loop < max_loop_count; loop++)
        {
            Debug.Log("loop : " + loop);
            Debug.Log("current_room_count : " + current_room_count);
            //yield return new WaitForSeconds(1.0f);

            //Probability calculation
            rand_range = Random.Range(0, 101);

            roomproperties[current_room_count] = new Room_data();
            room_properties = roomproperties[current_room_count];

            //room class = 0 is room
            //room class = 1 is collidor
            if(rand_range >= 20){room_properties.room_type = "0";}
            else{room_properties.room_type = "1";}

            if (room_properties.room_type == "0")
            //Room
            {
                
                // Random room size with odd number
                room_x = (int)Random.Range(2, 5) * 2 + 1;
                room_z = (int)Random.Range(2, 5) * 2 + 1;
            }
            else if (room_properties.room_type == "1")
            //Collidor
            {
                // x or z collidor
                int xz_collidor = Random.Range(0, 2);
                if (xz_collidor == 0)
                {
                    room_x = (int)Random.Range(6, 8) * 2 + 1;
                    room_z = (int)Random.Range(1, 2) * 2 + 1;
                }
                else
                {
                    room_x = (int)Random.Range(1, 2) * 2 + 1;
                    room_z = (int)Random.Range(6, 8) * 2 + 1;
                }
                
            }
            

            // Random positive or negative direction for generating next room
            int random_direction_polarity = Random.Range(0, 2);
            direction_room = polarity_num[random_direction_polarity];

            // Mid point of a room
            midpoint_x = (room_x) / 2 * (direction_room * 2);
            midpoint_z = (room_z) / 2 * (direction_room * 2);
            
            // absolute values of a position
            float room_x_abs = Mathf.Abs(room_x);
            float room_z_abs = Mathf.Abs(room_z);

            // Space for generating room
            space_for_rooms = true;
            rand_room2_index_exist = false;
            

            // first room
            if (loop == 0)
            {
                roomproperties[0].id = "0";
                roomproperties[0].entry_count = "0";
                
                //first room is a reference for other rooms
            }
            else
            {

                // Remove rooms with no space for generating next room in the list
                existing_room_space_index = existing_room_index.Except(existing_room_nospace_index).ToList();
                random_existing_room_index = existing_room_space_index[Random.Range(0, existing_room_space_index.Count)];
                Debug.Log("existingroom_Count : " + existing_room_index.Count);
                Debug.Log("existingroom_nospaceCount : " + existing_room_nospace_index.Count);

                

                // If room_type 2 only has one entry count
                // existing_room2_index gets room_type 2 with only one entry count
                for (int r = 0; r < existing_room2_index.Count; r++)
                {
                    if (roomproperties[existing_room2_index[0]].entry_count == "1" && roomproperties[existing_room2_index[0]].room_type == "1")
                    {
                        rand_room2_index_exist = true;
                        //rand_room2_index = Random.Range(0, existing_room2_index.Count);
                        if (existing_room2_index.Count < 2)
                        {
                            random_existing_room_index = existing_room2_index[0];
                        }
                        else
                        {
                            random_existing_room_index = existing_room2_index[-1];
                        }

                    }

                }


            // if (roomproperties[random_existing_room_index].nospace_count == max_nospace_count)
            // {continue;}

           



                // Mid point of a random existing room
                random_existing_room_mid = roomproperties[random_existing_room_index].middle_tile_pos;
                random_existing_room_mid = random_existing_room_mid.Substring(1, random_existing_room_mid.Length-2);
                
                //Convert from String to Vector3
                string[] rermArray =  random_existing_room_mid.Split(',');
                Vector3 rand_exist_rom_mid = new Vector3(float.Parse(rermArray[0]), float.Parse(rermArray[1]), float.Parse(rermArray[2]));
                //Debug.Log("midpoint_xz : " + rand_exist_rom_mid.x + " | " + rand_exist_rom_mid.z);

                // Get a random wall gameobject from a random room
                for (int i = 0; i < 20; i++)
                {
                    rand_num = Random.Range(0, wall_dict[random_existing_room_index].Count);
                    random_wall = wall_dict[random_existing_room_index][rand_num];
                    if (random_wall != null)
                    {
                        break;
                    }
                }
                
                
                new_door_pos = new Vector3(random_wall.transform.position.x, height + 2.0f, random_wall.transform.position.z);


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
                    direction_reference_obj = Instantiate(door_gameobject, random_wall_position, random_wall_rotation); 
                }
                //Debug.Log("direction_ref_pos : " + direction_reference_obj.transform.position);

                // Position references
                dir = new Vector3(new_door_pos.x, 2, rand_exist_rom_mid.z);
                dir2 = new Vector3(rand_exist_rom_mid.x, 2, new_door_pos.z);


                Collider[] Colliders_detect = Physics.OverlapBox(dir, new Vector3(0.2f, 1.8f, 0.2f), Quaternion.identity);
                //Check when there is a new collider coming into contact with the box
                if (0 != Colliders_detect.Length)
                {
                    
                    targetDirect = (dir2 - new_door_pos) * -1;
                    direction_reference_obj.transform.rotation = Quaternion.LookRotation(targetDirect);
                }

                Collider[] Colliders_detect2 = Physics.OverlapBox(dir2, new Vector3(0.2f, 1.8f, 0.2f), Quaternion.identity);
                if (0 != Colliders_detect2.Length)
                {
                    
                    targetDirect = (dir - new_door_pos) * -1;
                    direction_reference_obj.transform.rotation = Quaternion.LookRotation(targetDirect);
                }
                
                // Get reference position
                Vector3 pos_ref = new Vector3(Mathf.Round(direction_reference_obj.transform.position.x), 
                                                direction_reference_obj.transform.position.y, 
                                                Mathf.Round(direction_reference_obj.transform.position.z));
                // Get forward direction of the reference position
                Vector3 pos_ref_plus = direction_reference_obj.transform.position + direction_reference_obj.transform.forward;
                Vector3 pos_ref_plus_rounded = new Vector3(Mathf.Round(pos_ref_plus.x), pos_ref_plus.y, Mathf.Round(pos_ref_plus.z));
                room_spawnpoint_x = pos_ref_plus_rounded.x;
                room_spawnpoint_z = pos_ref_plus_rounded.z;

                direction_reference_obj.SetActive(false);

            }
            //Debug.Log("room_spawnpoint_x_z : " + $"({room_spawnpoint_x}" + $", {room_spawnpoint_z})");


            //Check for empty space to generate room with room_collider
            collider_size = new Vector3(room_x * 1.9f, 3f, room_z * 1.9f);
            collider_pos = new Vector3(room_spawnpoint_x + (room_x-1) * direction_room, 1f, room_spawnpoint_z + (room_z-1) * direction_room);

            room_collider = Physics.OverlapBox(collider_pos, collider_size / 2, Quaternion.identity);

            if (room_collider.Length != 0)
            {
                //Debug.Log("Collision :" + room_collider[0].gameObject);
                //Debug.Log("Collision_length :" + room_collider.Length);
                //Debug.Log("nospace, collider_pos : " + collider_pos + "| collider_size :" + collider_size);
                //Debug.Log("roomxz : " + room_x + " | " + room_z);

                space_for_rooms = false;

                nospace_count_toint = int.Parse(roomproperties[random_existing_room_index].nospace_count);
                nospace_count_toint++;
                roomproperties[random_existing_room_index].nospace_count = nospace_count_toint.ToString();

                if (int.Parse(roomproperties[random_existing_room_index].nospace_count) >= max_nospace_count)
                {
                    if (!existing_room_nospace_index.Contains(int.Parse(roomproperties[random_existing_room_index].id)))
                    {
                        existing_room_nospace_index.Add(int.Parse(roomproperties[random_existing_room_index].id));
                        Debug.Log("nospace_index : " + existing_room_nospace_index.Count);

                    }
                }
            }

            if (space_for_rooms == true)
            {

                //Serializing Json data
                room_properties.direction_polarity = random_direction_polarity.ToString();
                existing_room_index.Add(current_room_count);
                room_properties.id = current_room_count.ToString();

                if (room_properties.room_type == "1")
                {
                    existing_room2_index.Add(current_room_count);}

                if (rand_room2_index_exist == true)
                {existing_room2_index.RemoveAt(rand_room2_index);}
                

                
                //when run the code at least second times
                if (loop >= 1)
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

                    Destroy(random_wall);

                    door = Instantiate(door_gameobject, new_door_pos, random_wall.transform.rotation);
                    door.name = "door" + v.ToString();
                    v = name_cycle++;
                    door_list.Add(door);

                    // Generate door by probability
                    if (rand_range < 30){
                        door_list_destroy.Add(door);
                    }

                    entry_count_toint = int.Parse(roomproperties[random_existing_room_index].entry_count);
                    entry_count_toint++;
                    roomproperties[random_existing_room_index].entry_count = entry_count_toint.ToString();                   

                }

                int tile_num = 0;

                //generating tiles
                for (int i = 0; i <= room_z_abs - 1; i++)
                {
                    for (int j = 0; j <= room_x_abs - 1; j++)
                    {
                        //yield return new WaitForSeconds(0.02f);
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
                room_properties.first_tile_pos = first_tile_pos.ToString();
                room_properties.last_tile_pos = last_tile_pos.ToString();
                room_properties.middle_tile_pos = middle_tile_pos.ToString();

                tile_list.Clear();

                //List contains walls and will be cleared in every loop
                room_wall_list.Clear();

                //generating wall for x axis
                for (int j = 0; j <= room_x_abs - 1; j++)
                {
                    wall_posx = new Vector3(room_spawnpoint_x + j * 2 * direction_room, height + 2.0f, room_spawnpoint_z + (1.0f * direction_room * -1));
                    wallCollider(wall_posx);

                    if (wall_Collider.Length == 0){
                        clone_wallx = Instantiate(wall_gameobject, wall_posx, Quaternion.Euler(0, 0, 0));
                        clone_wallx.name = "wall_a" + v.ToString();
                        v = name_cycle++;
                        room_wall_list.Add(clone_wallx);

                    }

                    wall_posx2 = new Vector3(room_spawnpoint_x + j * 2 * direction_room, height + 2.0f, (room_spawnpoint_z + midpoint_z * 2 - direction_room * 2) + (1.0f * direction_room));
                    wallCollider(wall_posx2);

                    if (wall_Collider.Length == 0){
                        clone_wallx2 = Instantiate(wall_gameobject, wall_posx2, Quaternion.Euler(0, 0, 0));
                        clone_wallx2.name = "wall_b" + v.ToString();
                        v = name_cycle++;
                        room_wall_list.Add(clone_wallx2);

                    }

                }

                //generating wall for z axis
                for (int j = 0; j <= room_z_abs - 1; j++)
                {
                    wall_posz = new Vector3(room_spawnpoint_x + (1.0f * direction_room * -1), height + 2.0f, room_spawnpoint_z + j * 2 * direction_room);
                    wallCollider(wall_posz);

                    if (wall_Collider.Length == 0){
                        clone_wallz = Instantiate(wall_gameobject, wall_posz, Quaternion.Euler(0, 90, 0));
                        clone_wallz.name = "wall_c" + v.ToString();
                        v = name_cycle++;
                        room_wall_list.Add(clone_wallz);

                    }

                    wall_posz2 = new Vector3((room_spawnpoint_x + midpoint_x * 2 - direction_room * 2) + (1.0f * direction_room), height + 2.0f, room_spawnpoint_z + j * 2 * direction_room);
                    wallCollider(wall_posz2);

                    if (wall_Collider.Length == 0){
                        clone_wallz2 = Instantiate(wall_gameobject, wall_posz2, Quaternion.Euler(0, 90, 0));
                        clone_wallz2.name = "wall_d" + v.ToString();
                        v = name_cycle++;
                        room_wall_list.Add(clone_wallz2);

                    }

                }
                key = existing_room_index[existing_room_index.Count - 1];
                wall_dict.Add(key, new List<GameObject>());
                for(int wall = 0; wall < room_wall_list.Count; wall++){
                    
                    wall_dict[key].Add(room_wall_list[wall]);
                }
                
                string roomjson = JsonHelper.ToJson(roomproperties, 0);
                Debug.Log(roomjson);

                current_room_count++;
                if (current_room_count >= max_room_count){break;}

            }
            

        }

        // Destroy some of the doors
        for (int d = 0; d < door_list_destroy.Count; d++)
        {
            Destroy(door_list_destroy[d]);
        }

        //StartCoroutine(coroutine_doordestroy());
        yield return null;
    }

    void wallCollider(Vector3 wall_pos)
    {
        wall_Collider = Physics.OverlapBox(wall_pos, new Vector3(0.3f, 0.5f, 0.3f), Quaternion.identity);
    }

    IEnumerator coroutine_doordestroy()
    {
        return null;
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


