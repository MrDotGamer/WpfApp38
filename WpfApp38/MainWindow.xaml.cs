using System.Collections.Generic;
using System.Windows;
using WpfApp38.Models;

namespace WpfApp38
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Parametras> sourceParameters = new List<Parametras> {
                new Parametras
                {
                    Id = "1",
                    Value = "Text"
                },
                new Parametras
                {
                    Id = "Text",
                    Value = "Text2"
                },
                new Parametras
                {
                    Id = "2",
                    Value = "1"
                },
                new Parametras
                {
                    Id = "132",
                    Value = "2.0.3.8"
                }
            };
            List<Parametras> targetParameters = new List<Parametras>
            {
                new Parametras
                {
                    Id = "1",
                    Value = "Text2"
                },
                new Parametras
                {
                    Id = "2",
                    Value = "1"
                },
                new Parametras
                {
                    Id = "Text2",
                    Value ="Parametras"
                },
                new Parametras
                {
                    Id = "130",
                    Value = "2.3.0.1"
                }
            };
            Parametras.WriteParametersToFile(sourceParameters, Const.SourceFileName);
            Parametras.WriteParametersToFile(targetParameters, Const.TargetFileName);
        }
    }
}
