using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    private Hero[] players;
    private List<Hero> playerList;

    [Header("Camera Movement")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothness;

    [Header("Zoom Options")]
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float zoomLimiter;

    private Vector3 velocity;

	private void Awake()
    {
		cam = GetComponent<Camera>();
    }

    private void Start()
    {
		players = FindObjectsOfType<Hero>();
		playerList = players.ToList();
	}

    private void LateUpdate()
    {
        if (playerList.Count == 0)
            return;

        MoveCamera();
        Zoom();
    }

    private void MoveCamera()
    {
        Vector3 newPosition;

        newPosition = GetPlayersCenterPoint() + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothness);
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetPlayersGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private Vector3 GetPlayersCenterPoint()
    {
        if (playerList.Count == 1)
            return playerList[0].transform.position;

        Bounds bounds = new Bounds(players[0].transform.position, Vector3.zero);

		for(int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].Dead) playerList.Remove(playerList[i]);
			bounds.Encapsulate(playerList[i].transform.position);
		}

   //     foreach (Hero player in playerList)
   //     {
			//if (!player.Dead)
			//	bounds.Encapsulate(player.transform.position);
   //     }

        return bounds.center;
    }

    private float GetPlayersGreatestDistance()
    {
        Bounds bounds = new Bounds(playerList[0].transform.position, Vector3.zero);

		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].Dead) playerList.Remove(playerList[i]);
			bounds.Encapsulate(playerList[i].transform.position);
		}

		//foreach (Hero player in playerList)
  //      {
		//	if (!player.Dead)
		//		bounds.Encapsulate(player.transform.position);
  //      }

        return bounds.size.x + bounds.size.z;
    }
}
