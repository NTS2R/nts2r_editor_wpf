// Copyright (c) 2020 Rabenda
// The code under release by MIT License
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace nts2r_editor_wpf
{
    public static class ExcelUtils
    {
        private static Excel.Application _excel;
        private static Excel.Workbook _book;
        private static string FileUrl;
        private static readonly int militaryOffset = 2;
        public static bool OpenExcel(string fileUrl)
        {
            if (_excel == null)
            {
                _excel = new Microsoft.Office.Interop.Excel.Application(); //引用Excel对象 
            }

            FileUrl = fileUrl;
            _book = File.Exists(fileUrl) ? _excel.Application.Workbooks.Open(fileUrl) : _excel.Application.Workbooks.Add(true);
#if DEBUG
            _excel.Visible = true;
#endif
            return true;
        }

        public static bool CloseExcel()
        {
            _book?.Close(SaveChanges: true);
            _excel?.Quit();
            _excel = null;
            return true;
        }

        public static bool ExportAll(Commander commander)
        {
            ExportMilitary(commander);
            return true;
        }

        public static bool ExportMilitary(Commander commander)
        {
            FontDialog selectFontDialog = new FontDialog();
            var result = selectFontDialog.ShowDialog();
            var fontName = string.Empty;
            float fontSize;
            if (result == DialogResult.OK)
            {
                fontName = selectFontDialog.Font.Name;
                fontSize = selectFontDialog.Font.Size;
            }
            else
            {
                return false;
            }
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
            
            militarySheet.Cells[1, 1] = "番号";
            militarySheet.Cells[1, 2] = "简体名字";
            militarySheet.Cells[1, 3] = "战斗名字";
            militarySheet.Cells[1, 4] = "合成等级";
            militarySheet.Cells[1, 5] = "合成";
            militarySheet.Cells[1, 6] = "合成素材";
            militarySheet.Cells[1, 7] = "登场章节";
            militarySheet.Cells[1, 8] = "数据地址";
            militarySheet.Cells[1, 9] = "武力";
            militarySheet.Cells[1, 10] = "智力";
            militarySheet.Cells[1, 11] = "速度";
            militarySheet.Cells[1, 12] = "武器";
            militarySheet.Cells[1, 13] = "地形";
            militarySheet.Cells[1, 14] = "大将";
            militarySheet.Cells[1, 15] = "加成%";
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
            var generalSkillData = Utils.GetAllGeneral();
            for (int index = 0; index <= 0xFF; index++)
            {
                var military = commander.GetMilitary(index);
                militarySheet.Cells[militaryOffset + index, 1].NumberFormat = "\"0x\"@";
                militarySheet.Cells[militaryOffset + index, 1] = index.ToString("X2");
                militarySheet.Cells[militaryOffset + index, 2] = Utils.GetChsName(military.ChsNameBytes.ToArray());
                militarySheet.Cells[militaryOffset + index, 3] = Utils.GetChtName(military.ChtNameBytes, military.ChtNameControl);
                militarySheet.Cells[militaryOffset + index, 4] = military.CompositeLimitLevel;
                militarySheet.Cells[militaryOffset + index, 7] = military.Chapter;
                militarySheet.Cells[militaryOffset + index, 8].NumberFormat = "\"0x\"@";
                militarySheet.Cells[militaryOffset + index, 8] = military.Address.ToString("X5");
                
                militarySheet.Cells[militaryOffset + index, 9] = military.Force;
                militarySheet.Cells[militaryOffset + index, 10] = military.Wit;
                militarySheet.Cells[militaryOffset + index, 11] = military.Speed;
                militarySheet.Cells[militaryOffset + index, 12] = Utils.GetDegradeName(military.DegradeCategory);
                militarySheet.Cells[militaryOffset + index, 13] = Utils.GetTerrainName(military.Terrain);
                militarySheet.Cells[militaryOffset + index, 15].NumberFormat = "#0.00";
                var general = generalSkillData[Convert.ToByte(index)];
                if (general.Item1 != string.Empty)
                {
                    militarySheet.Cells[militaryOffset + index, 14] = general.Item1;
                    militarySheet.Cells[militaryOffset + index, 15] = (general.Item2 / 256.0 * 100).ToString("f2");
                }
                militarySheet.Cells[militaryOffset + index, 16] = military._skill.ren == 0x01 ? "仁" : "";
                militarySheet.Cells[militaryOffset + index, 17] = military._skill.hui == 0x01 ? "慧" : "";
                militarySheet.Cells[militaryOffset + index, 18] = military._skill.dang == 0x01 ? "挡" : "";
                militarySheet.Cells[militaryOffset + index, 19] = military._skill.qi == 0x01 ? "奇" : "";
                militarySheet.Cells[militaryOffset + index, 20] = military._skill.bi == 0x01 ? "避" : "";
                militarySheet.Cells[militaryOffset + index, 21] = military._skill.gong == 0x01 ? "攻" : "";
                militarySheet.Cells[militaryOffset + index, 22] = military._skill.wu == 0x01 ? "武" : "";
                militarySheet.Cells[militaryOffset + index, 23] = military._skill.zhi == 0x01 ? "智" : "";
                militarySheet.Cells[militaryOffset + index, 24] = military._skill.shu == 0x01 ? "术" : "";
                militarySheet.Cells[militaryOffset + index, 25] = military._skill.fan == 0x01 ? "返" : "";
                militarySheet.Cells[militaryOffset + index, 26] = military._skill.hun == 0x01 ? "魂" : "";
                militarySheet.Cells[militaryOffset + index, 27] = military._skill.jue == 0x01 ? "觉" : "";
                militarySheet.Cells[militaryOffset + index, 28] = military._skill.fang == 0x01 ? "防" : "";
                militarySheet.Cells[militaryOffset + index, 29] = military._skill.mou == 0x01 ? "谋" : "";
                militarySheet.Cells[militaryOffset + index, 30] = military._skill.liao == 0x01 ? "疗" : "";
                militarySheet.Cells[militaryOffset + index, 31] = military._skill.lin == 0x01 ? "临" : "";
                militarySheet.Cells[militaryOffset + index, 32] = military._skill.shi == 0x01 ? "识" : "";
                militarySheet.Cells[militaryOffset + index, 33] = military._skill.fen == 0x01 ? "奋" : "";
                militarySheet.Cells[militaryOffset + index, 34] = military._skill.tong == 0x01 ? "统" : "";
                militarySheet.Cells[militaryOffset + index, 35] = military._skill.ming == 0x01 ? "命" : "";
                militarySheet.Cells[militaryOffset + index, 36] = military.AttackCount;
                militarySheet.Cells[militaryOffset + index, 37] = military.StratagemCount;
                militarySheet.Cells[militaryOffset + index, 38].NumberFormat = "#0.0000";
                militarySheet.Cells[militaryOffset + index, 38] = (military.MilitaryLimit / 256.0).ToString("#0.0000");
            }
            Debug.WriteLine(Utils.GetNotCompositeToObject());
            Debug.WriteLine(Utils.GetNotCompositeAsObject());
            foreach (var index in Utils.GetNotCompositeToObject())
            {
                militarySheet.Cells[militaryOffset + index, 5] = "否";
            }

            foreach (var index in Utils.GetNotCompositeAsObject())
            {
                militarySheet.Cells[militaryOffset + index, 6] = "否";
            }

            militarySheet.UsedRange.Font.Name = fontName;
            militarySheet.UsedRange.Font.Size = fontSize;
            militarySheet.UsedRange.EntireColumn.AutoFit();
            militarySheet.UsedRange.EntireRow.AutoFit();
            return true;
        }

        public static bool ImportMilitary(Commander commander)
        {
            bool found = false;
            foreach (Excel.Worksheet sheet in _book.Sheets)
            {
                if (sheet.Name == "武将")
                {
                    found = true;
                }
            }

            if (found == false)
                return false;

            Excel.Worksheet militarySheet = (Excel.Worksheet)_book.Sheets["武将"];
            Excel.Range militaryRange = militarySheet.UsedRange;
            object[,] data = (object[,]) militaryRange.Value2;
            // Excel.Range rng = militarySheet.Cells.get_Range("B2", "B" + rowsint);   //item
            for (int index = 0; index <= 0xFF; index++)
            {
                var military = commander.GetMilitary(index);

                byte excelIndex = Convert.ToByte(data[militaryOffset + index, 1].ToString(), 16);
                
                if (excelIndex != index)
                    continue;
                // Debug.Write($"{index:X2}: ");
                // for (int i = 7; i <= 11; i++)
                //     Debug.Write(message: $"{i} -> {data[militaryOffset+index, i].ToString()}");
                // Debug.WriteLine("");
                military.CompositeLimitLevel = Convert.ToByte(data[militaryOffset + index, 4].ToString());
                military.Chapter = Convert.ToByte(data[militaryOffset + index, 7].ToString());
                military.Address = Convert.ToInt32(data[militaryOffset + index, 8].ToString(), 16);
                military.Force = Convert.ToByte(data[militaryOffset + index, 9].ToString()); 
                military.Wit = Convert.ToByte(data[militaryOffset + index, 10].ToString());
                military.Speed = Convert.ToByte(data[militaryOffset + index, 11].ToString());
            }

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
