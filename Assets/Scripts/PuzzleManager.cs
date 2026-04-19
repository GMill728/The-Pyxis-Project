using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 3;
    public int height = 3;

    [Header("Tile Prefab")]
    public LightTile tilePrefab;

    [Header("Light Bar")]
    public GameObject lightBarPrefab;
    public GameObject solveMarkerPrefab;
    public Transform lightBarParent;

    private LightTile[,] grid;
    private GameObject[] pips;
    private GameObject solveMarkerInstance;

    private int lightCount;
    public int solveNumber;
    public bool isSolved = false;

    void Start()
    {
        grid = new LightTile[width, height];
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SpawnTiles();
        CreateLightBar();
        UpdateLightState();
    }

    void Update()
    {
        if (isSolved){ChangeScene();}
    }

    void SpawnTiles()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                LightTile tile = Instantiate(tilePrefab, transform);

                tile.x = x;
                tile.y = y;
                tile.board = this;

                tile.isOn = Random.value > 0.5f;

                grid[x, y] = tile;
            }
        }
    }

    void CreateLightBar()
    {
        int maxPips = width * height + 1;
        pips = new GameObject[maxPips];

        solveNumber = width / 2 + height / 2 - 1;

        float parentHeight = GetParentHeight();
        float spacing = (maxPips > 1) ? parentHeight / (maxPips - 1) : 0f;
        float startY = -parentHeight / 2f + spacing / 2;

        SpawnPips(maxPips, spacing, startY);

        SpawnSolveMarker(spacing, startY);
    }

    void SpawnPips(int maxPips, float spacing, float startY)
    {
        for (int i = 0; i < maxPips; i++)
        {
            GameObject pip = Instantiate(lightBarPrefab, lightBarParent);
            pip.transform.localPosition = new Vector3(0f, startY + i * spacing, 0f);
            pips[i] = pip;
        }
    }


    void SpawnSolveMarker(float spacing, float startY)
    {
        if (solveMarkerPrefab != null)
        {
            solveMarkerInstance = Instantiate(solveMarkerPrefab, lightBarParent);
            float markerY = startY + (solveNumber - 1) * spacing + spacing * 0.5f;
            solveMarkerInstance.transform.localPosition = new Vector3(0f, markerY, 0f);
        }
    }


    float GetParentHeight()
    {
        RectTransform rect = lightBarParent.GetComponent<RectTransform>();
        if (rect != null)
            return rect.rect.height;

        Renderer r = lightBarParent.GetComponent<Renderer>();
        return r != null ? r.bounds.size.y : 5f;
    }

    public void PressTile(int x, int y)
    {
        ToggleTile(x, y);
        ToggleTile(x + 1, y);
        ToggleTile(x - 1, y);
        ToggleTile(x, y + 1);
        ToggleTile(x, y - 1);

        UpdateLightState();
    }

    void ToggleTile(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            grid[x, y].Toggle();
        }
    }

    void UpdateLightState()
    {
        lightCount = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y].isOn)
                    lightCount++;
            }
        }

        UpdateLightBar();
        CheckForSolve();
    }

    void UpdateLightBar()
    {
        for (int i = 0; i < pips.Length; i++)
        {
            pips[i].SetActive(i < lightCount);
        }
    }

    void CheckForSolve()
    {
        isSolved = (lightCount <= solveNumber);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("ProcGen Test");
    }
}