using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            ExcelExport.IsEnabled = false;
            ExcelImportMilitary.IsEnabled = false;
            ExcelImportSpecial.IsEnabled = false;
            MapperModify.IsEnabled = false;
        }

        private void FileOpen_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "游戏文件|*.nes",
                RestoreDirectory = true,
                FilterIndex = 1
            };
            if (openFileDialog.ShowDialog() != true) return;
            Utils.OpenFile(openFileDialog.FileName);
            _commander = new Commander();
            ExcelExport.IsEnabled = true;
            ExcelImportMilitary.IsEnabled = true;
            ExcelImportSpecial.IsEnabled = true;
            MapperModify.IsEnabled = true;
            // ExcelUtils.OpenExcel(Utils.GetExcelUrl());
        }

        private void WebsiteOpen_OnClick(object sender, RoutedEventArgs e)
        {
            const string uri = @"https://nts2r.gitee.io";

            // Launch the URI
            Process.Start(uri);
        }

        private void ExcelExport_OnClick(object sender, RoutedEventArgs e)
        {
            ExcelUtils.OpenExcel(Utils.GetExcelUrl());
            ExcelUtils.ExportAll(_commander);
            ExcelUtils.Save();
        }

        private void ExcelImportMilitary_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExcelImportSpecial_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MapperModify_OnClick(object sender, RoutedEventArgs e)
        {
//            MessageBox.Show("4(VisualNes)\n195(FCEUX)\n198(Other)\n224(nestopia)", "推荐Mapper值");
            string newMapperValue = Interaction.InputBox("4(VisualNes)\n195(FCEUX)\n198(Other)\n224(nestopia)", "请修改Mapper", Utils.GetMapper().ToString());
            Debug.WriteLine($"new mapper {newMapperValue}");
            
        }
    }
}
