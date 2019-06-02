using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera camera;

    [SerializeField] GameObject[] players;

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
		camera = GetComponent<Camera>();
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void LateUpdate()
    {
        if (players.Length == 0)
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
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, newZoom, Time.deltaTime);
    }

    private Vector3 GetPlayersCenterPoint()
    {
        if (players.Length == 1)
            return players[0].transform.position;

        Bounds bounds = new Bounds(players[0].transform.position, Vector3.zero);

        foreach (GameObject player in players)
        {
            bounds.Encapsulate(player.transform.position);
        }

        return bounds.center;
    }

    private float GetPlayersGreatestDistance()
    {
        Bounds bounds = new Bounds(players[0].transform.position, Vector3.zero);

        foreach (GameObject player in players)
        {
            bounds.Encapsulate(player.transform.position);
        }

        return bounds.size.x + bounds.size.z;
    }
}
