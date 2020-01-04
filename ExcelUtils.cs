using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace nts2r_editor_wpf
{
    public static class ExcelUtils
    {
        private static Excel.Application _excel;
        private static Excel.Workbook _book;
        private static string FileUrl;

        public static bool OpenExcel(string fileUrl)
        {
            if (_excel == null)
            {
                _excel = new Microsoft.Office.Interop.Excel.Application(); //引用Excel对象 
            }

            FileUrl = fileUrl;
            _book = File.Exists(fileUrl) ? _excel.Application.Workbooks.Open(fileUrl) : _excel.Application.Workbooks.Add(true);
            _excel.Visible = true;

            return true;
        }

        public static bool ExportAll(Commander commander)
        {
            ExportMilitary(commander);
            return true;
        }

        public static bool ExportMilitary(Commander commander)
        {
            bool found = false;
            foreach (Excel.Worksheet sheet in _book.Sheets)
            {
                if (sheet.Name == "武将")
                {
                    found = true;
                }
            }

            Excel.Worksheet militarySheet;
            if (found == false)
            {
                militarySheet = _book.Sheets.Add();
                militarySheet.Name = "武将";
            }
            else
            {
                militarySheet = (Excel.Worksheet)_book.Sheets["武将"];
            }
            int offset = 2;
            militarySheet.Cells[1, 1] = "番号";
            militarySheet.Cells[1, 2] = "简体中文名字";
            militarySheet.Cells[1, 3] = "战斗中文名字";
            militarySheet.Cells[1, 4] = "合成等级";
            militarySheet.Cells[1, 5] = "是否可合成";
            militarySheet.Cells[1, 6] = "是否可做合成素材";
            militarySheet.Cells[1, 7] = "登场章节";
            militarySheet.Cells[1, 8] = "数据地址";
            militarySheet.Cells[1, 9] = "武力";
            militarySheet.Cells[1, 10] = "智力";
            militarySheet.Cells[1, 11] = "速度";
            militarySheet.Cells[1, 12] = "武器";
            militarySheet.Cells[1, 13] = "地形";
            militarySheet.Cells[1, 14] = "大将";
            militarySheet.Cells[1, 15] = "加成百分比%";
            militarySheet.Cells[1, 16] = "仁";
            militarySheet.Cells[1, 17] = "慧";
            militarySheet.Cells[1, 18] = "挡";
            militarySheet.Cells[1, 19] = "奇";
            militarySheet.Cells[1, 20] = "避";
            militarySheet.Cells[1, 21] = "攻";
            militarySheet.Cells[1, 22] = "武";
            militarySheet.Cells[1, 23] = "智";
            militarySheet.Cells[1, 24] = "术";
            militarySheet.Cells[1, 25] = "返";
            militarySheet.Cells[1, 26] = "魂";
            militarySheet.Cells[1, 27] = "觉";
            militarySheet.Cells[1, 28] = "防";
            militarySheet.Cells[1, 29] = "谋";
            militarySheet.Cells[1, 30] = "疗";
            militarySheet.Cells[1, 31] = "临";
            militarySheet.Cells[1, 32] = "识";
            militarySheet.Cells[1, 33] = "奋";
            militarySheet.Cells[1, 34] = "统";
            militarySheet.Cells[1, 35] = "命";
            militarySheet.Cells[1, 36] = "攻击次数";
            militarySheet.Cells[1, 37] = "策略次数";
            militarySheet.Cells[1, 38] = "攻击倍率";
            Excel.Range captionRange = militarySheet.Range[militarySheet.Cells[1, 1], militarySheet.Cells[1, militarySheet.Columns.Count]];
            captionRange.Font.Bold = true;
            for (int index = 0; index <= 0xFF; index++)
            {
                var military = commander.GetMilitary(index);
                militarySheet.Cells[offset + index, 1].NumberFormat = "\"0x\"@";
                militarySheet.Cells[offset + index, 1] = index.ToString("X2");
                militarySheet.Cells[offset + index, 4] = military.CompositeLimitLevel;
                militarySheet.Cells[offset + index, 7] = military.Chapter;
                militarySheet.Cells[offset + index, 8].NumberFormat = "\"0x\"@";
                militarySheet.Cells[offset + index, 8] = military.Address.ToString("X5");
                
                militarySheet.Cells[offset + index, 9] = military.Force;
                militarySheet.Cells[offset + index, 10] = military.Wit;
                militarySheet.Cells[offset + index, 11] = military.Speed;
                militarySheet.Cells[offset + index, 16] = military._skill.ren == 0x01 ? "仁" : "";
                militarySheet.Cells[offset + index, 17] = military._skill.hui == 0x01 ? "慧" : "";
                militarySheet.Cells[offset + index, 18] = military._skill.dang == 0x01 ? "挡" : "";
                militarySheet.Cells[offset + index, 19] = military._skill.qi == 0x01 ? "奇" : "";
                militarySheet.Cells[offset + index, 20] = military._skill.bi == 0x01 ? "避" : "";
                militarySheet.Cells[offset + index, 21] = military._skill.gong == 0x01 ? "攻" : "";
                militarySheet.Cells[offset + index, 22] = military._skill.wu == 0x01 ? "武" : "";
                militarySheet.Cells[offset + index, 23] = military._skill.zhi == 0x01 ? "智" : "";
                militarySheet.Cells[offset + index, 24] = military._skill.shu == 0x01 ? "术" : "";
                militarySheet.Cells[offset + index, 25] = military._skill.fan == 0x01 ? "返" : "";
                militarySheet.Cells[offset + index, 26] = military._skill.hun == 0x01 ? "魂" : "";
                militarySheet.Cells[offset + index, 27] = military._skill.jue == 0x01 ? "觉" : "";
                militarySheet.Cells[offset + index, 28] = military._skill.fang == 0x01 ? "防" : "";
                militarySheet.Cells[offset + index, 29] = military._skill.mou == 0x01 ? "谋" : "";
                militarySheet.Cells[offset + index, 30] = military._skill.liao == 0x01 ? "疗" : "";
                militarySheet.Cells[offset + index, 31] = military._skill.lin == 0x01 ? "临" : "";
                militarySheet.Cells[offset + index, 32] = military._skill.shi == 0x01 ? "识" : "";
                militarySheet.Cells[offset + index, 33] = military._skill.fen == 0x01 ? "奋" : "";
                militarySheet.Cells[offset + index, 34] = military._skill.tong == 0x01 ? "统" : "";
                militarySheet.Cells[offset + index, 35] = military._skill.ming == 0x01 ? "命" : "";
                militarySheet.Cells[offset + index, 36] = military.AttackCount;
                militarySheet.Cells[offset + index, 37] = military.StratagemCount;
                militarySheet.Cells[offset + index, 38] = (military.MilitaryLimit / 256.0).ToString("F4");
            }

            militarySheet.UsedRange.Font.Name = "思源宋体";
            militarySheet.UsedRange.Font.Size = 12;
            militarySheet.UsedRange.EntireColumn.AutoFit();
            militarySheet.UsedRange.EntireRow.AutoFit();
            return true;
        }

        public static void Save()
        {
            if (File.Exists(FileUrl))
                _book.Save();
            else
                _book.SaveAs(FileUrl);
        }
    }
}
