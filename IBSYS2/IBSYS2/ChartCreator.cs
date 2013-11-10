using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using Visiblox.Charts;


//http://www.visiblox.com/visibloxcharts/quick-start-guide-for-wpf,-silverlight-and-windows-phone/introduction/


namespace IBSYS2
{
    public partial class ChartCreator : UserControl
    {
        public ChartCreator()
        {
            InitializeComponent();
  
            //We need one data series for each chart series
            DataSeries<double, double> xData = new DataSeries<double, double>("y=x");
            DataSeries<double, double> xSquaredData = new DataSeries<double, double>("y=x^2");
            DataSeries<double, double> xCubedData = new DataSeries<double, double>("y=x^3");
  
            //Add the data points to the data series according to the correct equation
            for (double i = 0.0; i < 2; i += 0.01)
            {
                xData.Add(new DataPoint<double, double>() { X = i, Y = i });
                xSquaredData.Add(new DataPoint<double, double>() { X = i, Y = i * i });
                xCubedData.Add(new DataPoint<double, double>() { X = i, Y = i * i * i });
            }
  
            //Finally, associate the data series with the chart series
            //exampleChart.Series[0].DataSeries = xData;
            //exampleChart.Series[1].DataSeries = xSquaredData;
            //exampleChart.Series[2].DataSeries = xCubedData;
        }

        private void ChartCreator_Load(object sender, EventArgs e)
        {

  
        }
    }
}
