// Copyright (c) 2020 Rabenda
// The code under release by MIT License
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace nts2r_editor_wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            ExcelUtils.CloseExcel();
            base.OnExit(e);
        }
    }
}
