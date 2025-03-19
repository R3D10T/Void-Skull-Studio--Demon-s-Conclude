using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitController : MonoBehaviour
{
    [SerializeField] int movementSpeed = 1;
    [SerializeField] Transform selectedUnit;
    [SerializeField] Transform PlayCam;
    bool unitSelected = false;
    GridManager idManager;
    private bool battleStart = false;
    public PlayerTemplate Player;

    public int CameraDistance;

    // Start is called before the first frame update
    void Start()
    {
        idManager = FindObjectOfType<GridManager>();
        battleStart = false;

        selectedUnit.transform.position = new Vector3(Player.gridCoords.x, selectedUnit.position.y, Player.gridCoords.y);
        PlayCam.transform.position = new Vector3(Player.gridCoords.x, PlayCam.position.y, (Player.gridCoords.y - CameraDistance));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);

            if(hasHit)
            {
                if (hit.transform.tag == "Tile")
                {
                    Vector2Int targetCoords = hit.transform.GetComponent<Labeller>().Coords;
                    Vector2Int startCoords = new Vector2Int((int)selectedUnit.position.x, (int)selectedUnit.position.y) / idManager.UnityGridSize;

                    Vector2Int PosMoveRange = new Vector2Int(startCoords.x + movementSpeed, startCoords.y + movementSpeed);
                    Vector2Int NegMoveRange = new Vector2Int(startCoords.x - movementSpeed, startCoords.y - movementSpeed);

                    battleStart = hit.transform.GetComponent<Labeller>().BattleSpot;

                    selectedUnit.transform.position = new Vector3(targetCoords.x, selectedUnit.position.y, targetCoords.y);
                    PlayCam.transform.position = new Vector3((targetCoords.x), PlayCam.position.y, (targetCoords.y - CameraDistance));
                    Player.gridCoords = targetCoords;
                    /*
                    if (targetCoords.x <= PosMoveRange.x & targetCoords.x >= NegMoveRange.x) 
                    {
                        //selectedUnit.transform.position = new Vector3(targetCoords.x, selectedUnit.position.y, selectedUnit.position.y);
                        //PlayCam.transform.position = new Vector3(targetCoords.x, PlayCam.position.y, selectedUnit.position.y);

                        if (targetCoords.y <= PosMoveRange.y & targetCoords.y >= NegMoveRange.y)
                        {
                            selectedUnit.transform.position = new Vector3(targetCoords.x, selectedUnit.position.y, targetCoords.y);
                            PlayCam.transform.position = new Vector3(targetCoords.x, PlayCam.position.y, targetCoords.y);
                            Player.gridCoords = targetCoords;
                        }
                    }
                    */
                    if (battleStart)
                    {
                        hit.transform.GetComponent<Labeller>().BattleSpot = false;
                        Battle();
                    }

                    
                }

                if (hit.transform.tag == "Player")
                {
                    selectedUnit = hit.transform;
                    unitSelected = true;
                }
            }
        }

        if (Player.Exp >= Player.ExpToNext)
        {
            LevelUp();
        }
    }

    private void Battle()
    {
        SceneManager.LoadScene(2);
    }

    public void LevelUp()
    {
        Player.Exp = Player.Exp - Player.ExpToNext;
        Player.ExpToNext = Player.ExpToNext * 2;
        Player.MaxHP += 10;
        Player.Atk += 5;
        Player.Def += 5;
        Player.Level += 1;
    }
}
