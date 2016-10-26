using UnityEngine;
using System.Collections;

public class MapScript : MonoBehaviour
{

    public delegate void MapAction(Vector3 position);
    public static event MapAction MapCreatedSuccess;

    //public GameObject[] playerObject;
    //public GameObject monsterObject;
    //private GameObject[] monsters;
    //public GameObject Tile;
    //private Transform Floor;
    //private int size = 20;
    //public int numberofspawn = 1;

    //temp
    private Vector3 startPosition = new Vector3(0,0,0);

	void Start ()
    {
        //initializing empty Floor object
        //Floor = new GameObject("Floor").transform;

        ////creating 20x20 tiles and parenting them under Floor object
        //for (int horizontal = 0; horizontal < size; horizontal++)
        //{
        //    for (int vertical = 0; vertical < size; vertical++)
        //    {
        //        GameObject TilePiece = GameObject.Instantiate(Tile, new Vector3(horizontal, 0f, vertical), Quaternion.identity) as GameObject;
        //        TilePiece.name = "Tile" + horizontal + vertical;
        //        TilePiece.transform.SetParent(Floor);
        //    }
        //}

        // instead of position give the spawning tile go
	    if (MapCreatedSuccess != null)
	        MapCreatedSuccess(startPosition);
	    //spawning player
	    //GameObject Player1 = GameObject.Instantiate(playerObject[0], new Vector3(5f, 0.1f, 5f), Quaternion.identity) as GameObject;
	    //Player1.name = "Player1";
	    //GameObject Player2 = GameObject.Instantiate(playerObject[1], new Vector3(6f, 0.1f, 6f), Quaternion.identity) as GameObject;
	    //Player2.name = "Player2";
    }

    void Update()
    {
        ////spawning monster
        //monsters = GameObject.FindGameObjectsWithTag("Monster");
        ////Debug.Log(monsters.Length);
        //if (monsters.Length==0)
        //{
        //    for (int i = 0; i < numberofspawn; i++)
        //    {
        //        GameObject Monster = GameObject.Instantiate(monsterObject, new Vector3((i + 4), 0.1f, 6f), Quaternion.identity) as GameObject;
        //        Monster.name = "Monster " + i;
        //    }
        //}
    }
}
