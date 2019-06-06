using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    private CharacterMovement[] Players;

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
		Players = FindObjectsOfType<CharacterMovement>();
    }

    private void LateUpdate()
    {
        if (Players.Length == 0)
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
        if (Players.Length == 1)
            return Players[0].transform.position;

        Bounds bounds = new Bounds(Players[0].transform.position, Vector3.zero);

        foreach (CharacterMovement player in Players)
        {
            bounds.Encapsulate(player.transform.position);
        }

        return bounds.center;
    }

    private float GetPlayersGreatestDistance()
    {
        Bounds bounds = new Bounds(Players[0].transform.position, Vector3.zero);

        foreach (CharacterMovement player in Players)
        {
            bounds.Encapsulate(player.transform.position);
        }

        return bounds.size.x + bounds.size.z;
    }
}
