using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CR_CellView_Translator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _seeCellsText;
    [SerializeField] TextMeshProUGUI _cellsTabText;
    [SerializeField] TextMeshProUGUI _buyCellText;
    [SerializeField] TextMeshProUGUI _cellInfoTitle;

    [Header("SELECT CELL TEXTS")]
    [SerializeField] TextMeshProUGUI _redBloodCellText;
    [SerializeField] TextMeshProUGUI _whiteBloodCellText;
    [SerializeField] TextMeshProUGUI _helperBloodCellText;

    public void UpdateTexts()
    {
        var language = (int)CR_Data.data._language;
        _seeCellsText.text = LanguageManager.instance.cellViewTexts.seeCells[language];
        _cellsTabText.text = LanguageManager.instance.cellViewTexts.cells[language];
        _buyCellText.text = LanguageManager.instance.cellViewTexts.buyCells[language];
        _cellInfoTitle.text = LanguageManager.instance.cellViewTexts.cells[language];
        _redBloodCellText.text = LanguageManager.instance.cellViewTexts.redBloodCells[language];
       if(_whiteBloodCellText != null) _whiteBloodCellText.text = LanguageManager.instance.cellViewTexts.whiteBloodCells[language];
       if (_helperBloodCellText != null) _helperBloodCellText.text = LanguageManager.instance.cellViewTexts.helperBloodCells[language];
    }
}
