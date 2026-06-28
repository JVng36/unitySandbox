using UnityEngine;
using UnityEngine.InputSystem;

public class TileCursor : MonoBehaviour
{
    [SerializeField] private Grid grid;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // 1. Where is the mouse in screen pixels?
        Vector2 screenPos = Mouse.current.position.ReadValue();

        // 2. Convert to world  space (z=-10 because our camera sits at z=-10)
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

        // 3. Ask the Grid which cell that world position belongs to
        Vector3Int cell = grid.WorldToCell(worldPos);

        // 4. Ask the Grid what the center world position of that cell is
        Vector3 cellCenter = grid.GetCellCenterWorld(cell);

        // 5. Snap this object to that position
        transform.position = cellCenter;

        // Press E to hoe the tile under the cursor
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            FarmManager.Instance.HoeTile(cell);
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            FarmManager.Instance.WaterTile(cell);
        }
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            FarmManager.Instance.TryUntillTile(cell);
        }
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            FarmManager.Instance.ResetTile(cell);  
        }

    }
}