using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class Labeller : MonoBehaviour
{
    
    public Material[] Material;
    public GameObject Tile;
    Renderer Rend;
    

    TextMeshPro Label;
    public Vector2Int Coords = new Vector2Int();
    GridManager GridManager;
    [SerializeField] public bool BattleSpot = false;

    private void Awake()
    {
        GridManager = FindObjectOfType<GridManager>();
        Label = GetComponentInChildren<TextMeshPro>();
        DisplayCoords();              
    }

    private void Start()
    {

        Rend = Tile.GetComponent<Renderer>();
        Rend.enabled = true;
        if (BattleSpot)
        {
            Rend.sharedMaterial = Material[1];
        }
        else
        {
            Rend.sharedMaterial = Material[0];
        }
    }

    private void Update()
    {
        DisplayCoords();
        transform.name = Coords.ToString();

        Rend = Tile.GetComponent<Renderer>();
        Rend.enabled = true;
        
        if (BattleSpot)
        {
            Rend.sharedMaterial = Material[1];
        }
        else
        {
            Rend.sharedMaterial = Material[0];
        }
    }

    private void DisplayCoords()
    {
        if (!GridManager) { return; }
        Coords.x = Mathf.RoundToInt(transform.position.x / GridManager.UnityGridSize);
        Coords.y = Mathf.RoundToInt(transform.position.z / GridManager.UnityGridSize);

        Label.text = Coords.ToString();
    }
}
