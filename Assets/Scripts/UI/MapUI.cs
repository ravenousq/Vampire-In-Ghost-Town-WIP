using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MapUI : MonoBehaviour, ISaveManager
{
    [SerializeField] private Camera MC;
    [SerializeField] private PixelPerfectCamera PPC;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private int minZoom, maxZoom;
    [SerializeField] private Transform maps;
    [Header("Markers")]
    [SerializeField] private Sprite filledMarker;
    [SerializeField] private Sprite emptyMarker;
    [SerializeField] private Image[] menuMarkers;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private TextMeshProUGUI markerSnippet;
    private List<KeyValuePair<GameObject, Vector2>> markers = new List<KeyValuePair<GameObject, Vector2>>();
    private int availableMarkers;
    private bool isReturning;
    private int defaultZoom;
    private Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;
        defaultZoom = PPC.assetsPPU;
    }

    private void Update()
    {
        if (isReturning)
        {
            FocusPlayer();
            return;
        }

        KeyValuePair<GameObject, Vector2> dummy;
        markerSnippet.text = isMarkerDetected(out dummy) ? "<color=#81675D>C</color><color=#493B39> - Remove Marker</color>" : "<color=#81675D>C</color><color=#493B39> - Place Marker</color>";

        if (Input.GetKeyDown(KeyCode.V))
            isReturning = true;

        if (Input.GetKeyDown(KeyCode.C))
        {
            Vector2 camPos = new Vector2(MC.transform.position.x, MC.transform.position.y);

            if (isMarkerDetected(out KeyValuePair<GameObject, Vector2> marker))
                DeleteMarker(marker);
            else if(availableMarkers > 0)
                CreateMarker(camPos);
        }

        Navigate();
        ManageZoom();

        PPC.assetsPPU = Mathf.Clamp(PPC.assetsPPU, minZoom, maxZoom);
    }

    private void FocusPlayer()
    {
        MC.transform.position = Vector3.MoveTowards(MC.transform.position, player.transform.position + Vector3.back * 10, MC.orthographicSize * 2 * Time.unscaledDeltaTime);
        isReturning = Vector2.Distance(new Vector2(MC.transform.position.x, MC.transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y)) > .1f;
        PPC.assetsPPU = isReturning ? PPC.assetsPPU : maxZoom;
    }

    private void CreateMarker(Vector2 position)
    {
        Vector2 mapPos = new Vector2(maps.transform.position.x, maps.transform.position.y);

        markers.Add(new KeyValuePair<GameObject, Vector2>(Instantiate(markerPrefab, new Vector3(position.x, position.y), Quaternion.identity), position - mapPos));
        menuMarkers[^availableMarkers].sprite = emptyMarker;
        availableMarkers--;
    }

    private void ManageZoom()
    {
        if (Input.GetKeyDown(KeyCode.X))
            PPC.assetsPPU -= 1;
        else if (Input.GetKeyDown(KeyCode.Z))
            PPC.assetsPPU += 1;
    }

    private void Navigate()
    {
        MC.transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * MC.orthographicSize * Time.unscaledDeltaTime;
    }

    private void OnEnable()
    {
        if (player)
        {
            MC.transform.position = player.transform.position + Vector3.back * 10;
            PPC.assetsPPU = defaultZoom;
        }
    }

    private bool isMarkerDetected(out KeyValuePair<GameObject, Vector2> position)
    {
        Vector2 mapPos = new Vector2(maps.transform.position.x, maps.transform.position.y);

        foreach (KeyValuePair<GameObject, Vector2> markerPos in markers)
            if (Vector2.Distance(new Vector2(MC.transform.position.x, MC.transform.position.y), markerPos.Value + mapPos) < 5)
            {
                position = markerPos;
                return true;
            }

        position = new KeyValuePair<GameObject, Vector2>();
        return false;
    }

    public void PlayerMarkerCollision(GameObject marker)
    {
        foreach (KeyValuePair<GameObject, Vector2> mark in markers)
        {
            if (mark.Key == marker)
            {
                DeleteMarker(mark);
                return;
            }
        }
    }

    private void DeleteMarker(KeyValuePair<GameObject, Vector2> marker)
    {
        Destroy(marker.Key);
        markers.Remove(marker);
        availableMarkers++;
        menuMarkers[^availableMarkers].sprite = filledMarker;
    }

    public void LoadData(GameData data)
    {
        availableMarkers = menuMarkers.Length;

        if (data.markers == null)
            return;

        for (int i = 0; i < data.markers.Length; i += 2)
            CreateMarker(new Vector2(maps.transform.position.x + data.markers[i], maps.transform.position.y + data.markers[i + 1]));
    }

    public void SaveData(ref GameData data)
    {
        if (markers.Count == 0)
        {
            data.markers = new float[] { };
            return;
        }

        data.markers = new float[markers.Count * 2];

        for (int i = 0; i < markers.Count * 2; i += 2)
        {
            data.markers[i] = markers[i / 2].Value.x;
            data.markers[i + 1] = markers[i / 2].Value.y;
        }
    }
}
