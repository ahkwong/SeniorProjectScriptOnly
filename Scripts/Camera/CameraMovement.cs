using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Assets.Scripts.Entities.Hero;
using Senior;

public class CameraMovement : MonoBehaviour
{

    public float lerpspeed = 5f;
    public float cameraFloor = 4f;
    public float cameraCeiling = 7f;
    public Vector3 offset = Vector3.zero;
    private Vector3 cameraPos = Vector3.zero;
    public List<GameObject> players;
    private float centerOfAllPlayerXPosition, centerOfAllPlayerYPosition, centerOfAllPlayerZPosition,
                    meanX, meanY, meanZ, maxZ, minZ,
                    avgChange, maxX, minX, maxY, minY;

    void Start()
    {
    }

    void OnEnable()
    {
        Player.HeroSpawned += FindPlayers;
        Player.HeroDied += DestroyPlayers;
    }

    void OnDisable()
    {
        Player.HeroSpawned -= FindPlayers;
        Player.HeroDied -= DestroyPlayers;
    }

    void FindPlayers(Player player)
    {
        players.Add(player.GetComponentInChildren<Hero>().gameObject);
    }

    void DestroyPlayers(Player player)
    {
        players.Remove(player.GetComponentInChildren<Hero>().gameObject);
    }

    void FixedUpdate()
    {

        // player at the right most position
        maxX = 0;
        for (int i = 0; i < players.Count; i++)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(players[i].transform.position);

            if (pos.x > maxX)
                maxX = pos.x;
        }

        // player at the left most position
        minX = maxX;
        for (int i = 0; i < players.Count; i++)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(players[i].transform.position);

            if (pos.x < minX)
                minX = pos.x;
        }

        //player at the upper most position
        maxY = 0;
        for (int i = 0; i < players.Count; i++)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(players[i].transform.position);

            if (pos.y > maxY)
                maxY = pos.y;
        }

        //player at the bottom most position
        minY = maxY;
        for (int i = 0; i < players.Count; i++)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(players[i].transform.position);

            if (pos.y < minY)
                minY = pos.y;
        }

        //player at the upper most position
        maxZ = 0;
        for (int i = 0; i < players.Count; i++)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(players[i].transform.position);

            if (pos.z > maxZ)
                maxZ = pos.z;
        }

        //player at the bottom most position
        minZ = maxZ;
        for (int i = 0; i < players.Count; i++)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(players[i].transform.position);

            if (pos.z < minZ)
                minZ = pos.z;
        }

        //center of the players in screen space
        centerOfAllPlayerXPosition = (maxX + minX) / 2;
        centerOfAllPlayerYPosition = (maxY + minY) / 2;
        centerOfAllPlayerZPosition = (maxZ + minZ) / 2;

        ////mean of x and y
        //meanX = maxX - minX;
        //meanY = maxY - minY;
        //meanZ = maxZ - minZ;
        ////average of both means
        //avgChange = (meanX + meanY + meanZ) / 3;
        //Debug.Log(avgChange);
        ////min and max distance of the camera
        //if (avgChange < cameraFloor)
        //    avgChange = cameraFloor;
        //if (avgChange > cameraCeiling)
        //    avgChange = cameraCeiling;

        //setting camera position
        cameraPos.x = centerOfAllPlayerXPosition;
        cameraPos.y = centerOfAllPlayerYPosition;
        cameraPos.z = centerOfAllPlayerZPosition;
        cameraPos += offset;
        //Debug.Log(cameraPos);
        Vector3 poss = Camera.main.ScreenToWorldPoint(cameraPos);
        //Debug.Log(poss);
        //lerp old camera position to new player position

        transform.position = Vector3.Lerp(transform.position, poss, lerpspeed * Time.deltaTime);
    }

    //void FixedUpdate()
    //{
    //    //finding how many players
    //    numberOfPlayer = players.Count;

    //    //player at the right most position
    //    maxX = 0;
    //    for (int i = 0; i < numberOfPlayer; i++)
    //        if (players[i].transform.position.x > maxX) maxX = players[i].transform.position.x;

    //    //player at the left most position
    //    minX = maxX;
    //    for (int i = 0; i < numberOfPlayer; i++)
    //        if (players[i].transform.position.x < minX) minX = players[i].transform.position.x;

    //    //player at the upper most position
    //    maxZ = 0;
    //    for (int i = 0; i < numberOfPlayer; i++)
    //        if (players[i].transform.position.z > maxZ) maxZ = players[i].transform.position.z;

    //    //player at the bottom most position
    //    minZ = maxZ;
    //    for (int i = 0; i < numberOfPlayer; i++)
    //        if (players[i].transform.position.z < minZ) minZ = players[i].transform.position.z;

    //    //center of the players in world space
    //    centerOfAllPlayerXPosition = (maxX + minX) / 2;
    //    centerOfAllPlayerZPosition = (maxZ + minZ) / 2;

    //    //mean of x and z
    //    meanX = maxX - minX;
    //    meanZ = maxZ - minZ;

    //    //average of both means
    //    avgChange = (meanX + meanZ) / 2;

    //    //min and max distance of the camera
    //    if (avgChange < cameraFloor)
    //        avgChange = cameraFloor;
    //    if (avgChange > cameraCeiling)
    //        avgChange = cameraCeiling;

    //    //setting camera position
    //    cameraPos.x = centerOfAllPlayerXPosition;
    //    cameraPos.y = 0;
    //    cameraPos.z = centerOfAllPlayerZPosition - avgChange;

    //    //lerp old camera position to new player position
    //    transform.position = Vector3.Lerp(transform.position, cameraPos, lerpspeed * Time.deltaTime);
    //}
}
