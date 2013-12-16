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