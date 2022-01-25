using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMerger : MonoBehaviour
{
    [SerializeField] GameObject merger;
    [SerializeField] Sprite[] MergeSprites;
    GameObject mergerReference;
    int CellsReady;

    public void Merge(List<GameObject> cellsToMerge, int celltype, bool MidCellMerge = false)
    {
        StartCoroutine(MergeCells(cellsToMerge, celltype, MidCellMerge));
    }

    IEnumerator MergeCells(List<GameObject> cellsToMerge, int cellType, bool midCellMerge)
    {
        Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
        Vector2 spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
        GameObject cellMerger = Instantiate(merger, spawnPosition, Quaternion.identity);
        cellMerger.GetComponent<SpriteRenderer>().sprite = MergeSprites[cellType];
        mergerReference = cellMerger;
        yield return new WaitForSeconds(0.5f);
        CellsReady = cellsToMerge.Count;
        for (int i = 0; i < cellsToMerge.Count; i++)
        {
            cellsToMerge[i].TryGetComponent(out Cell_Base cell_Base);
            cell_Base.combineTarget = cellMerger.transform.position;
            cell_Base.ToggleCollider();
            cell_Base.StartMerge();
            cell_Base.myCellMerger = this;
        }

        while (CellsReady > 0)
        {

            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < cellsToMerge.Count; i++)
        {
            Destroy(cellsToMerge[i]);

        }
        cellsToMerge.Clear();
        CellSpawner myCellSpawner = GameManager.gameManager.organManager.cellSpawner;
        if (midCellMerge)
        {
            Debug.Log("Cell with no info spawned");
            //myCellSpawner.SpawnMedRedBloodCell(cellMerger.transform.position);
            myCellSpawner.InstantiateCells(cellMerger.transform.position, false, cellType, 1, true);
            myCellSpawner.CheckMedCellsMerge(cellType);
        }
        else
        {
            myCellSpawner.InstantiateCells(cellMerger.transform.position);
            myCellSpawner.CanBuyCell = true;
        }
        //LTDescr d = LeanTween.scale(cellMerger, new Vector3(cellMerger.transform.position.x * 1.2f, cellMerger.transform.position.y * 1.2f, 1), .4f).setEase(LeanTweenType.easeInExpo);
        LTDescr f = LeanTween.alpha(cellMerger, 0, .4f).setEase(LeanTweenType.easeInExpo);

        f.setOnComplete(DestroyMerger);
        void DestroyMerger()
        {
            Destroy(cellMerger);
        }
    }

    public void DestroyMergerReference()
    {
        if (mergerReference != null)
            Destroy(mergerReference);
    }


    public void CellArrived()
    {
        CellsReady--;
        float xSize = mergerReference.transform.localScale.x * 1.1f;
        float ySize = mergerReference.transform.localScale.y * 1.1f;
        mergerReference.transform.localScale = new Vector3(xSize, ySize, 1);

    }
}
