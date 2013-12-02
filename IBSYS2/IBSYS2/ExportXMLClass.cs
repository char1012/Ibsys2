using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.OleDb;

namespace IBSYS2
{
    class ExportXMLClass
    {
        //Übergabe von mehrdimensionalen Array der anderen Parteien
        public void XMLExport(OleDbCommand cmd)
        {

            XmlDocument doc = new XmlDocument();
            XmlNode myRoot, myNode;

            myRoot = doc.CreateElement("results");
            doc.AppendChild(myRoot);
            //Array mit den Überelementen
            string[] childnodesXML = new string[] { "warehousestock", "inwardstockmovement","futureinwardstockmovement", "idletimecosts", "waitinglistworkstations", "waitingliststock", "ordersinwork", "completedorders", "cycletimes", "result" };
            //Arrays für Attribute der jeweiligen Überelemente inklusive InnerText
            string[] warehousestockXML = new string[] { };
            string[] inwardstockmovementXML = new string[] { };
            string[] futureinwardstockmovementXML = new string[] { };
            string[] idletimecostsXML = new string[] { };
            string[] waitinglistworkstationsXML = new string[] { };
            //
            string[] waitingliststockXML = new string[] { };
            //<workplace id="1" period="7" order="7" batch="13" item="54" amount="10" timeneed="30"/>
            string[] ordersinworkXML = new string[] { "id", "period", "order", "batch", "item", "amount", "timeneed" };
            //Ersten sechs Paramter von <order/>, die anderen von <batch/>
            string[] completedordersXML = new string[] { "period", "id", "item", "quantity", "cost", "averageunitcosts", "id", "amount", "cycletime", "cost" };
            //Ersten zwei Paramter von <cycletimes/>, Rest von <order/>
            string[] cycletimesXML = new string[] { "startedorders", "waitingorders", "id", "period", "starttime", "finishtime", "cycletimemin", "cycletimefactor" };
            //
            string[] resultXML = new string[] {  };

            

            for (int i = 0; i < childnodesXML.Length; i++)
            {
                myNode = doc.CreateElement(childnodesXML[i]);
                myRoot.AppendChild(myNode);
            }

            myNode = doc.CreateElement("Test1");
            myRoot.AppendChild(myNode);


            myRoot.AppendChild(doc.CreateElement("Test2"));


            doc.Save(@"c:\AppendChild.xml");



        }
    }
}
