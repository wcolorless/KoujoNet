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
using KoujoNet;
using System.IO;
using KoujoNet.core.Algorithms.KNN;
using KoujoNet.core.Data;

namespace TestApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataSet XORTrainDataSet;
        private DataSet IrisTrainDataSet;
        private DataSet KNNTrainDataSet;
        private Net XORnet;
        private Net Irisnet;
        private KNNClassifier _knnClassifier;

        public MainWindow()
        {
            InitializeComponent();
            ComplexityIris.ItemsSource = new string[] {"Little", "Middle", "Big" };
            ComplexityIris.SelectedIndex = 0;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void LoadXOR(object sender, RoutedEventArgs e)
        {
            ConsistType Consist = new ConsistType();
            Consist.Add(ColumnType.Create(ColumnRoleType.Label, ColumnDataType.Double));
            Consist.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double));
            Consist.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double));
            Consist.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double));
            XORTrainDataSet = TextLoader.LoadFromCSV(Directory.GetCurrentDirectory() + "//Datasets//xorTrain.txt", Consist);
        }

        private void LearnXOR(object sender, RoutedEventArgs e)
        {
            XORnet = Net.Create(XORTrainDataSet, ActionTypes.Regression, NetSize.Little, ActivateType.Sigmoid, LearnerType.BackPropagation, 0.01);
            XORnet.SetLearningData(XORTrainDataSet);
            XORnet.Learn(2000);
        }

        private void SaveXOR(object sender, RoutedEventArgs e)
        {
            XORnet.Save(Directory.GetCurrentDirectory() + "//XORModel.m");
        }

        private void PredictXOR(object sender, RoutedEventArgs e)
        {
            var in1 = Convert.ToDouble(XORIN1.Text);
            var in2 = Convert.ToDouble(XORIN2.Text);
            var preRes = XORnet.Compute(new double[] { in1, in2 });
            XORRESULT.Text = preRes[0].ToString();
        }

        private void LoadIris(object sender, RoutedEventArgs e)
        {
            ConsistType Consist = new ConsistType();
            Consist.Add(ColumnType.Create(ColumnRoleType.Label, ColumnDataType.Double));
            Consist.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double));
            Consist.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double));
            Consist.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double));
            Consist.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double));
            IrisTrainDataSet = TextLoader.LoadFromCSV(Directory.GetCurrentDirectory() + "//Datasets//iris-train.txt", Consist);
            var totalClasses = IrisTrainDataSet.CountClasses();
            var totalLines = IrisTrainDataSet.Count;
            IrisTotalClasses.Text = totalClasses.ToString();
            IrisTotalLines.Text = totalLines.ToString();
            MessageBox.Show("Successfully loaded and detected " + totalClasses.ToString() + " classes");
        }

        private void LearnIris(object sender, RoutedEventArgs e)
        {
            var Complexity = (NetSize)ComplexityIris.SelectedIndex;
            Irisnet = Net.Create(IrisTrainDataSet, ActionTypes.Classifications, Complexity, ActivateType.Sigmoid, LearnerType.BackPropagation, 0.01);
            Irisnet.SetLearningData(IrisTrainDataSet);
            Irisnet.Learn(2000);
        }

        private void SaveIris(object sender, RoutedEventArgs e)
        {
            Irisnet.Save(Directory.GetCurrentDirectory() + "//IrisModel.m");
        }

        private void PredictIris(object sender, RoutedEventArgs e)
        {
            var in1 = Convert.ToDouble(IRISIN1.Text);
            var in2 = Convert.ToDouble(IRISIN2.Text);
            var in3 = Convert.ToDouble(IRISIN3.Text);
            var in4 = Convert.ToDouble(IRISIN4.Text);
            var preRes = Irisnet.Compute(new double[] { in1, in2, in3, in4 });
            IRISRESULT.Text = preRes[0].ToString();
        }

        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void LoadKNN(object Sender, RoutedEventArgs E)
        {
            ConsistType Consist = new ConsistType();
            Consist.Add(ColumnType.Create(ColumnRoleType.Label, ColumnDataType.String, "Label"));
            Consist.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double, "Square"));
            Consist.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double, "Storeys"));
            Consist.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double, "Cost"));
            KNNTrainDataSet = TextLoader.LoadFromCSV(Directory.GetCurrentDirectory() + "//Datasets//KNNHouse.txt", Consist);
        }

        private void SettingKNN(object Sender, RoutedEventArgs E)
        {
            _knnClassifier = new KNNClassifier(KNNTrainDataSet);
            _knnClassifier.Init();
        }

        private void PredictKNN(object Sender, RoutedEventArgs E)
        {
            var elapsedTimer = new Stopwatch();
            var newRow = new DataRow();
            var square = Convert.ToDouble(KNNSquare.Text);
            var storeys = Convert.ToDouble(KNNStoreys.Text);
            var cost = Convert.ToDouble(KNNCost.Text);
            newRow.Add(ColumnType.Create(ColumnRoleType.Label, ColumnDataType.String), "Find");
            newRow.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double, "Square"), square);
            newRow.Add(ColumnType.Create(ColumnRoleType.Data, ColumnDataType.Double, "Storeys"), storeys);
            newRow.Add(ColumnType.Create(ColumnRoleType.Data,ColumnDataType.Double, "Cost"), cost);
            elapsedTimer.Reset();
            elapsedTimer.Start();
            var result = _knnClassifier.Classification(newRow);
            elapsedTimer.Stop();
            KNNTimeElapsed.Text = elapsedTimer.ElapsedTicks.ToString() + "; ticks (1 tick = 100 nano)";
            KNNResult.Text = result;
        }
    }
}
