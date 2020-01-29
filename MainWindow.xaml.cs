// Copyright (c) 2020 Rabenda
// The code under release by MIT License
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

namespace nts2r_editor_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Commander _commander;
        public MainWindow()
        {
            InitializeComponent();
            DisabledExcelFunction();
            MapperModify.IsEnabled = false;
            FileSave.IsEnabled = false;
        }

        private void EnabledExcelFunction()
        {
            ExcelItem.IsEnabled = true;
            ExcelExport.IsEnabled = true;
            ExcelImportMilitary.IsEnabled = true;
            ExcelImportSpecial.IsEnabled = true;
        }

        private void DisabledExcelFunction()
        {
            ExcelItem.IsEnabled = false;
            ExcelExport.IsEnabled = false;
            ExcelImportMilitary.IsEnabled = false;
            ExcelImportSpecial.IsEnabled = false;
        }

        private void FileOpen_OnClick(object sender, RoutedEventArgs e)
        {
            var filename = Utils.OpenFileDialog("Game");
            if (filename == string.Empty)
            {
                MessageBox.Show("没有选择文件", "警告");
                return;
            }
            Utils.OpenFile(filename);
            _commander = new Commander();
            if (Utils.IsExcelInstalled())
            {
                EnabledExcelFunction();
            }
            MapperModify.IsEnabled = true;
            FileSave.IsEnabled = true;

            Utils.ParseConfig();
        }

        private void WebsiteOpen_OnClick(object sender, RoutedEventArgs e)
        {
            const string uri = @"https://nts2r.gitee.io";

            // Launch the URI
            Process.Start(uri);
        }

        private void ExcelExport_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("导出完成之前，导入导出功能暂时不可用", "警告");
            DisabledExcelFunction();
            new Thread(
                () =>
                {
                    ExcelUtils.OpenExcel(Utils.GetExcelUrl());
                    ExcelUtils.ExportAll(_commander);
                    ExcelUtils.Save();
                    ExcelUtils.CloseExcel();
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new Action(this.EnabledExcelFunction
                        ));
                    
                }
                ).Start();
        }

        private void ExcelImportMilitary_OnClick(object sender, RoutedEventArgs e)
        {
            var filename = Utils.OpenFileDialog("Excel");
            if (filename == string.Empty)
            {
                MessageBox.Show("没有选择文件", "警告");
                return;
            }
            ExcelUtils.OpenExcel(Utils.GetExcelUrl());
            ExcelUtils.ImportMilitary(_commander);
            ExcelUtils.CloseExcel();
        }

        private void ExcelImportSpecial_OnClick(object sender, RoutedEventArgs e)
        {
            var filename = Utils.OpenFileDialog("Excel");
            if (filename == string.Empty)
            {
                MessageBox.Show("没有选择文件", "警告");
                return;
            }
            throw new NotImplementedException();
        }

        private void MapperModify_OnClick(object sender, RoutedEventArgs e)
        {
//            MessageBox.Show("4(VisualNes)\n195(FCEUX)\n198(Other)\n224(nestopia)", "推荐Mapper值");
            string newMapperValue = Interaction.InputBox("4(VisualNes)\n195(FCEUX)\n198(Other)\n224(nestopia)", "请修改Mapper", Utils.GetMapper().ToString());
            Debug.WriteLine($"new mapper {newMapperValue}");
            try
            {
                byte mapperValue = Convert.ToByte(newMapperValue);
                Utils.SetMapper(mapperValue);
                MessageBox.Show($"mapper值修改为: {mapperValue}\n请保存", "成功");
            }
            catch (FormatException)
            {
                MessageBox.Show("mapper值格式不对", "警告");
            }
            catch (OverflowException)
            {
                MessageBox.Show("mapper值范围(0-255)", "警告");
            }
        }

        private void AboutItem_OnClick(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new About();
            aboutWindow.ShowDialog();
        }

        private void FileSave_OnClick(object sender, RoutedEventArgs e)
        {
            Utils.SaveFile();
        }
    }
}
