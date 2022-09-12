#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//공통 NPOI
using NPOI;
using NPOI.SS.UserModel;
//표준 xls 버젼 excel시트
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
//확장 xlsx 버젼 excel 시트
using NPOI.XSSF;
using NPOI.XSSF.UserModel;

using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "ExcelImporter", menuName = "Excel/ExcelImporter", order = 0)]
public class ExcelImporter : ScriptableObject
{
    [FilePath]
    public string filePath;

    public List<string> sheetNameList;
    public List<string> keyList;

    public List<SerializableDictionary<string, string>> loadDataContainer;

    public IWorkbook GetWorkBook()
    {
        try
        {
            Debug.Log($"[Excel-Importer] Start :: Read => Path : {filePath}");

            if (filePath.Contains(".xlsx"))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    Debug.Log("[Excel-Importer] Success :: Xlsx Read");
                    return new XSSFWorkbook(fs);
                }
            }
            else if (filePath.Contains(".xls"))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    Debug.Log("[Excel-Importer] Success :: Xls Read");
                    return new HSSFWorkbook(fs);
                }
            }
            else
            {
                throw new Exception("Not Support File.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[Excel-Importer] Error ::  {e.Message}");
        }

        return null;
    }

    public IRow GetRow(ISheet sheet, int rowIndex)
    {
        var row = sheet.GetRow(rowIndex);

        if (row == null)
        {
            Debug.LogError($"[Excel-Importer] Error :: Not Search {rowIndex} Row");
        }

        return row;
    }

    public ICell GetCell(IRow row, int celIndex)
    {
        var cell = row.GetCell(celIndex);

        if (cell == null)
        {
            Debug.LogError($"[Excel-Importer] Error :: Not Search {celIndex} Cell");
        }

        return cell;
    }

    public ICell GetCell(ISheet sheet, int rowIndex, int celIndex)
    {
        var row = sheet.GetRow(rowIndex);
        return GetCell(row, celIndex);
    }
    public object GetCellValue(ICell cell)
    {
        object cValue = string.Empty;
        switch (cell.CellType)
        {
            case CellType.Numeric:
                cValue = cell.NumericCellValue;
                break;
            case CellType.String:
                cValue = cell.StringCellValue;
                break;
            case CellType.Boolean:
                cValue = cell.BooleanCellValue;
                break;
            case CellType.Error:
                cValue = cell.ErrorCellValue;
                break;
            default:
                cValue = cell.StringCellValue;
                break;
        }
        return cValue;
    }


    [Button("엑셀 파일 불러오기")]
    public void ReadFile()
    {
        sheetNameList.Clear();
        keyList.Clear();

        var book = GetWorkBook();
        //시트 이름들 가져오기
        for (var i = 0; i < book.NumberOfSheets; ++i)
        {
            sheetNameList.Add(book.GetSheetName(i));
        }
    }

    [Button("특정 시트 불러오기")]
    public void ReadSheet(string sheetName)
    {
        keyList.Clear();
        loadDataContainer.Clear();

        var book = GetWorkBook();
        var sheet = book.GetSheet(sheetName);

        var rowCount = sheet.LastRowNum;

        var firstRow = GetRow(sheet, 0);
        //키 이름들 가져오기
        for (var i = 0; i < firstRow.LastCellNum; ++i)
        {
            var cell = GetCell(firstRow, i);
            keyList.Add(cell.StringCellValue);
        }

        //데이터 맵핑
        for (var i = 1; i <= rowCount; ++i)
        {
            var row = sheet.GetRow(i);
            var rowDic = new SerializableDictionary<string, string>();
            //셀 순서대로 접근
            for (var k = 0; k < row.LastCellNum; ++k)
            {
                //Key - Value 세팅
                var cell = GetCell(row, k);
                rowDic.Add(keyList[k], GetCellValue(cell).ToString());
            }

            loadDataContainer.Add(rowDic);
        }
    }

    public List<SerializableDictionary<string, string>> GetSheetData(string sheetName)
    {
        var loadDataContainer = new List<SerializableDictionary<string, string>>();
        var keyList = new List<string>();

        var book = GetWorkBook();
        var sheet = book.GetSheet(sheetName);

        var rowCount = sheet.LastRowNum;

        var firstRow = GetRow(sheet, 0);
        //키 이름들 가져오기
        for (var i = 0; i < firstRow.LastCellNum; ++i)
        {
            var cell = GetCell(firstRow, i);
            keyList.Add(cell.StringCellValue);
        }

        //데이터 맵핑
        for (var i = 1; i <= rowCount; ++i)
        {
            var row = sheet.GetRow(i);
            var rowDic = new SerializableDictionary<string, string>();
            //셀 순서대로 접근
            for (var k = 0; k < row.LastCellNum; ++k)
            {
                //Key - Value 세팅
                var cell = GetCell(row, k);
                rowDic.Add(keyList[k], GetCellValue(cell).ToString());
            }

            loadDataContainer.Add(rowDic);
        }

        return loadDataContainer;
    }


}
#endif