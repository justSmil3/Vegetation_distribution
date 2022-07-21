using System.Collections.Generic;
using UnityEngine;

// TODO change public functions to private as soon as the eventsyste works
public class ObjectSpawner : MonoBehaviour
{
    Dictionary<MonoBehaviour, List<int>> requesetToList; // this is for later so i can identify all created lists. I want to build this out \
    // in combination with a full event system i want to build so i dont have to fight with this all the time.
    List<List<GameObject>> objectPools;
    
    private void Awake() {
        objectPools = new List<List<GameObject>>();
    }

    // this is just temporarly left to public, until the event system is done.
    public int InitiatePool(/*MonoBehaviour Requester, */GameObject obj, int count = 1000)
    {
        int id  = objectPools.Count;
        List<GameObject> spawnedObjects = new List<GameObject>();
        for(int i = 0; i < count; i++)
        {
            GameObject tmp = Instantiate(obj);
            spawnedObjects.Add(tmp);
            tmp.SetActive(false);
        }
        objectPools.Add(spawnedObjects);
        // if(requesetToList.ContainsKey(Requester))
        // {
        //     requesetToList[Requester].Add(id);
        // }
        // else
        // {
        //     List<int> idList = new List<int>();
        //     idList.Add(id);
        //     requesetToList.Add(Requester, idList);
        // }
        return id;
    }

#region  SpawnObject
    // this is just temporarly left to public, until the event system is done.
    //! this is only repeated so often because the end goal is to make this a general purpos object spawner !!! 
    public GameObject SpawnObject(MonoBehaviour Requester, int id = -1)
    {   
        return SpawnObject(Requester, Vector3.zero, Quaternion.identity, id);
    }
    public GameObject SpawnObject(MonoBehaviour Requester, Vector3 pos, int id = -1)
    {   
        return SpawnObject(Requester, pos, Quaternion.identity, id);
    }
    public GameObject SpawnObject(MonoBehaviour Requester, Quaternion rot, int id = -1)
    {   
        return SpawnObject(Requester, Vector3.zero, rot, id);
    }
    public GameObject SpawnObject(MonoBehaviour Requester, Vector3 pos, Quaternion rot, int id = -1)
    {   
        if(id == -1)
        {
            id = requesetToList[Requester][0];
        }
        return SpawnObject(id, pos, rot);
    }
    public GameObject SpawnObject(int id)
    {   
        return SpawnObject(id, Vector3.zero, Quaternion.identity);
    }
    public GameObject SpawnObject(int id, Vector3 pos)
    {   
        return SpawnObject(id, pos, Quaternion.identity);
    }
    public GameObject SpawnObject(int id, Quaternion rot)
    {   
        return SpawnObject(id, Vector3.zero, rot);
    }
    public GameObject SpawnObject(int id, Vector3 pos, Quaternion rot)
    {   
        List<GameObject> objects = objectPools[id];
        GameObject objectToSpawn = objects[0];
        objects.RemoveAt(0);
        objects.Add(objectToSpawn);
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = pos;
        // objectToSpawn.transform.rotation = rot;
        return objectToSpawn;
    }
#endregion
    
    // this is just temporarly left to public, until the event system is done.
    public void DespawnObject(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(false);
    }

    private void OnDestroy() 
    {
        DeleteAllPools();    
    }

    // Methodes to be implimented down the line
    void DeletePool(int id){}
    void DeletePool(int id, MonoBehaviour Requester){}
    void DeleteAllPools(){}
    void DeleteAllPools(MonoBehaviour Requester){}
}
