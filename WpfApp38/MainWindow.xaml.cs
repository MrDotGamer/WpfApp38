﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp38.Models;
using WpfApp38.ViewModel;

namespace WpfApp38
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        delegate List<ParametersViewModel> ParametersList(List<Parametras> source, List<Parametras> target);
        ParametersList lstDelegate = null;
        List<Parametras> SourceParametersList;
        List<Parametras> TargetParametersList;
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
            lstDelegate = ParametersViewModel.AddedResultList;
            lstDelegate += ParametersViewModel.ModifiedResultList;
            lstDelegate += ParametersViewModel.RemovedResultList;
            lstDelegate += ParametersViewModel.UnchangedResultList;
        }

        private void SourceButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = Const.DafaultExtention,
                Filter = Const.DefaultFilter
            };
            if (openFileDialog.ShowDialog() == true)
            { 
                 SourceLbl.Content = openFileDialog.FileName;
                 SourceParametersList =  OpenFile(openFileDialog.FileName);
            }
            if (SourceParametersList != null && TargetParametersList != null)
            {
                TableGrid.Visibility = Visibility.Visible;
                DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                ResultLbl.Content = ParametersViewModel.TotalStatusResult(SourceParametersList, TargetParametersList);
            }
        }

        private void TargetButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = Const.DafaultExtention,
                Filter = Const.DefaultFilter
            };
            if (openFileDialog.ShowDialog() == true)
            {
                TargetLbl.Content = openFileDialog.FileName;
                TargetParametersList = OpenFile(openFileDialog.FileName);
            }
            if (SourceParametersList != null && TargetParametersList != null)
            {
                TableGrid.Visibility = Visibility.Visible;
                DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                ResultLbl.Content = ParametersViewModel.TotalStatusResult(SourceParametersList, TargetParametersList);
            }
        }

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.Item is ParametersViewModel item)
            {
                switch (item.Status)
                {
                    case Const.Unchanged:
                        e.Row.Background = new SolidColorBrush(Colors.Silver);
                        break;
                    case Const.Added:
                        e.Row.Background = new SolidColorBrush(Colors.LimeGreen);
                        break;
                    case Const.Removed:
                        e.Row.Background = new SolidColorBrush(Colors.Tomato);
                        break;
                    case Const.Modified:
                        e.Row.Background = new SolidColorBrush(Color.FromRgb(255, 255, 94));
                        break;
                }
            }
        }

        private List<ParametersViewModel> GetList(List<Parametras> sourceParametersList, List<Parametras> targetParametersList)
        {
            List<ParametersViewModel> list = new List<ParametersViewModel>();
            
                var methodsList = lstDelegate.GetInvocationList();
                foreach (var method in methodsList)
                {
                    var invockedMethodResult = (List<ParametersViewModel>)method.DynamicInvoke(sourceParametersList, targetParametersList);
                    list = list.Concat(invockedMethodResult).ToList();
                }

            return list;
        }
        private List<Parametras> OpenFile(string path)
        {
            return Parametras.ReadParametersFromFile(path);
        }

        private void Unchanged_Checked(object sender, RoutedEventArgs e)
        {
            ParametersList lstUnchanged = ParametersViewModel.UnchangedResultList;
            var checkbox = (CheckBox)sender;

            if (lstDelegate != null)
            {
                var d = lstDelegate.GetInvocationList();
                if ((bool)checkbox.IsChecked && d.Contains(lstUnchanged))
                {
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
                else if ((bool)checkbox.IsChecked && !d.Contains(lstUnchanged))
                {
                    lstDelegate += ParametersViewModel.UnchangedResultList;
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
                else if (!(bool)checkbox.IsChecked && d.Contains(lstUnchanged))
                {
                    if (lstDelegate.GetInvocationList().Count() != 1)
                    {
                        lstDelegate -= ParametersViewModel.UnchangedResultList;
                    }
                    else
                    {
                        Unchanged.IsChecked = true;
                    }
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
            }
        }

        private void Modified_Checked(object sender, RoutedEventArgs e)
        {
            ParametersList lstModified = ParametersViewModel.ModifiedResultList;
            var checkbox = (CheckBox)sender;
            if (lstDelegate != null)
            {
                var d = lstDelegate.GetInvocationList();
                if ((bool)checkbox.IsChecked && d.Contains(lstModified))
                {
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
                else if ((bool)checkbox.IsChecked && !d.Contains(lstModified))
                {
                    lstDelegate += ParametersViewModel.ModifiedResultList;
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
                else if (!(bool)checkbox.IsChecked && d.Contains(lstModified))
                {
                    if (lstDelegate.GetInvocationList().Count() != 1)
                    {
                        lstDelegate -= ParametersViewModel.ModifiedResultList;
                    }
                    else
                    {
                        Modified.IsChecked = true;
                    }
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
            }
        }

        private void Added_Checked(object sender, RoutedEventArgs e)
        {
            ParametersList lstAdded = ParametersViewModel.AddedResultList;
            var checkbox = (CheckBox)sender;
            if (lstDelegate != null)
            {
                var d = lstDelegate.GetInvocationList();
                if ((bool)checkbox.IsChecked && d.Contains(lstAdded))
                {
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
                else if ((bool)checkbox.IsChecked && !d.Contains(lstAdded))
                {
                    lstDelegate += ParametersViewModel.AddedResultList;
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
                else if (!(bool)checkbox.IsChecked && d.Contains(lstAdded))
                {
                    if (lstDelegate.GetInvocationList().Count() != 1)
                    {
                        lstDelegate -= ParametersViewModel.AddedResultList;
                    }
                    else
                    {
                        Added.IsChecked = true;
                    }
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
            }
        }

        private void Removed_Checked(object sender, RoutedEventArgs e)
        {
            ParametersList lstRemoved = ParametersViewModel.RemovedResultList;
            var checkbox = (CheckBox)sender;
            if (lstDelegate != null)
            {
                var d = lstDelegate.GetInvocationList();
                if ((bool)checkbox.IsChecked && d.Contains(lstRemoved))
                {
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
                else if ((bool)checkbox.IsChecked && !d.Contains(lstRemoved))
                {
                    lstDelegate += ParametersViewModel.RemovedResultList;
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
                else if (!(bool)checkbox.IsChecked && d.Contains(lstRemoved))
                {
                    if (lstDelegate.GetInvocationList().Count() != 1)
                    {
                        lstDelegate -= ParametersViewModel.RemovedResultList;
                    }
                    else
                    {
                        Removed.IsChecked = true;
                    }
                    DataGrid.ItemsSource = GetList(SourceParametersList, TargetParametersList);
                }
            }
        }
        private void SearchTexBox_LostFocus(object sender,RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
        }
        private void SearchById_KeyUp(object sender, KeyEventArgs e)
        {
            var source = SourceParametersList.Where(x => x.Id.StartsWith(SearchTextBox.Text)).ToList();
            var target = TargetParametersList.Where(x => x.Id.StartsWith(SearchTextBox.Text)).ToList();
            DataGrid.ItemsSource = GetList(source, target);
        }
    }
}
