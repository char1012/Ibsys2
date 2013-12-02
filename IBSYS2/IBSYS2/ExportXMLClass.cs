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
            XmlAttribute attrib;

            myRoot = doc.CreateElement("results");
            doc.AppendChild(myRoot);
            //Array mit den Überelementen
            string[] childnodesXML = new string[] { "warehousestock", "inwardstockmovement","futureinwardstockmovement", "idletimecosts", "waitinglistworkstations", "waitingliststock", "ordersinwork", "completedorders", "cycletimes", "result" };
            //Arrays für Attribute der jeweiligen Überelemente inklusive InnerText
            string[] resultsAttributes = new string[] {"game", "group", "period"};
            //warehousestock/article
            string[] warehousestockXML = new string[] { "id", "amount", "startamount", "pct", "price", "stockvalue"};
            //inwardstockmovement/order
            string[] inwardstockmovementXML = new string[] { "orderperiod", "id", "mode", "article", "amount", "time", "materialcosts", "ordercosts", "entirecosts", "piececosts" };
            //futureinwardstockmovement\order
            string[] futureinwardstockmovementXML = new string[] { "orderperiod", "id", "mode", "article", "amount" };
            //idletimecosts/workplace
            string[] idletimecostsXML = new string[] { "id", "setupevents", "idletime", "wageidletimecosts", "wagecosts", "machineidletimecosts" };
            //Ersten beiden Elemente von Oberlement waitinglistworkstations\workplace, die restlichen Elemente von ...\waitinglist 
            string[] waitinglistworkstationsXML = new string[] { "id", "timeneed", "period", "order", "firstbatch", "lastbatch", "item", "amount", "timeneed" };
            //ID von Oberlement, der rest von waitinglist(Kindelelement)
            string[] waitingliststockXML = new string[] { "id", "period", "order", "item", "amount" };
            //<workplace id="1" period="7" order="7" batch="13" item="54" amount="10" timeneed="30"/>
            string[] ordersinworkXML = new string[] { "id", "period", "order", "batch", "item", "amount", "timeneed" };
            //Ersten sechs Paramter von <order/>, die anderen von <batch/>
            string[] completedordersXML = new string[] { "period", "id", "item", "quantity", "cost", "averageunitcosts", "id", "amount", "cycletime", "cost" };
            //Ersten zwei Paramter von <cycletimes/>, Rest von <order/>
            string[] cycletimesXML = new string[] { "startedorders", "waitingorders", "id", "period", "starttime", "finishtime", "cycletimemin", "cycletimefactor" };
            string[] resultXML = new string[] {  };

            

            for (int i = 0; i < childnodesXML.Length; i++)
            {
                myNode = doc.CreateElement(childnodesXML[i]);
                myRoot.AppendChild(myNode);
            }

            myNode = doc.CreateElement("Test1");
            myRoot.AppendChild(myNode);

            attrib = doc.CreateAttribute("Attribute1");
            attrib.InnerText = "AttributeText1";
            myNode.Attributes.Append(attrib);

            myRoot.AppendChild(doc.CreateElement("Test2"));
            doc.Save(@"c:\TestAppendXML.xml");



        }
    }
}
