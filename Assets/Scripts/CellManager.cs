using UnityEngine;

public class CellManager : MonoBehaviour
{
    private GameManager _gm;
    private Cell[] _cells;

    private void Start()
    {
        _gm = FindFirstObjectByType<GameManager>();
        _cells = GetComponentsInChildren<Cell>();
        foreach (Cell cell in _cells)
            cell.onSelect += ChangeSelection;
    }

    private void ChangeSelection(int id)
    {
        foreach (Cell cell in _cells)
            if (cell.skinID != id)
                cell.Deselect();
        _gm.ChangeSkin(id);
    }
}
