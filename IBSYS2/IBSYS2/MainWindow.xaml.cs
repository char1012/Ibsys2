using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using System.Globalization;
using System.Windows.Input;
using Visiblox.Charts;
using System.Windows;


namespace IBSYS2
{

    public partial class BarChartExample : UserControl
    {
        public BarChartExample()
        {
            InitializeComponent();

            //Initiale Anlegung der Serie inklusive Seriennamen
            var series1 = new DataSeries<String, Double>("Kinderfahrrad");
            var series2 = new DataSeries<String, Double>("Damenfahrrad");
            var series3 = new DataSeries<String, Double>("Herrenfahrrad");

            //Befüllung der Serie mit Daten aus DB
            //SQL-Statement
            series1.Add(new DataPoint<String, Double>("Periode x", 1));

            //Serie einem Visiblox-Datentyp zuweisen und ins Chart einbinden (.Add-Befehl)
            ColumnSeries cseries1 = new ColumnSeries();
            cseries1.DataSeries = series1;

            MainChart.Series.Add(cseries1);

            //Change HighlightedStyle to Normal style and add mouse enter and leave events on series
            foreach (BarSeries series in MainChart.Series)
            {
                series.MouseEnter += new MouseEventHandler(series_MouseEnter);
                series.MouseLeave += new MouseEventHandler(series_MouseLeave);
            }

        }

        /// Mouse has entered one of the bar datapoints - set cursor to hand
        void series_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Mouse has left one of the bar datapoints - set cursor to arrow
        /// </summary>
        void series_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        //Listener setzen durch Einfügen Button in welche Form?
        //private void button_newCharts(object sender, RoutedEventArgs e)
        //{
        //    BarChartExample lala = BarChartExample.GetInstance();
        //    lala.Closed += new EventHandler(
        //                        delegate(object obj, EventArgs args)
        //                        {
        //                            lala.deleteInstance();
        //                        });
        //    lala.Show();
        //    lala.Focus();
        //}

        internal static BarChartExample GetInstance()
        {
            throw new NotImplementedException();
        }

        public EventHandler Closed { get; set; }

        internal void deleteInstance()
        {
            throw new NotImplementedException();
        }

        internal void Show()
        {
            throw new NotImplementedException();
        }
    }



    // Data model

    /// <summary>
    /// A list of debt levels
    /// </summary>
    public class DebtLevelList : List<DebtLevel> { }

    /// <summary>
    /// A debt level object
    /// </summary>
    public class DebtLevel
    {
        /// <summary>
        /// The Country, as a string, that this debt data point applies to
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The Percent of GDP value for this country
        /// </summary>
        public double PercentGDP { get; set; }
    }
}