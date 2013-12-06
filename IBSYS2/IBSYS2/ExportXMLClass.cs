using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.OleDb;
using System.Xml.Linq;

namespace IBSYS2
{
    class ExportXMLClass
    {
        //Übergabe von mehrdimensionalen Array der anderen Parteien
        public void XMLExport() //OleDbCommand cmd
        {

            XmlDocument doc = new XmlDocument();
            XmlNode myRoot; //, myNode;
            //XmlAttribute attrib;

            myRoot = doc.CreateElement("input");
            doc.AppendChild(myRoot);
            //Array mit den Überelementen
            //string[] childnodesXML = new string[] { "warehousestock", "inwardstockmovement","futureinwardstockmovement", "idletimecosts", "waitinglistworkstations", "waitingliststock", "ordersinwork", "completedorders", "cycletimes", "result" };
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


            string[] childNodesXML = new string[] { "qualitycontrol", "sellwish", "selldirect", "orderlist", "productionlist", "workingtimelist" };
            string[] art = new string[] { "1", "2", "3" };
            string[] sellwishArr = new string[] { "article", "quantity" };

            XmlTextWriter myXmlTextWriter = new XmlTextWriter(@"C:\XML\TestAppendXML1.xml", null);
            myXmlTextWriter.Formatting = Formatting.Indented;
            myXmlTextWriter.WriteStartDocument(false);

            myXmlTextWriter.WriteStartElement("input");
            myXmlTextWriter.WriteStartElement("qualitycontrol", null);
            myXmlTextWriter.WriteAttributeString("type", "no");
            myXmlTextWriter.WriteAttributeString("losequantity", "0");
            myXmlTextWriter.WriteAttributeString("delay", "0");
            myXmlTextWriter.WriteEndElement();
            //Bereich sellwish
            myXmlTextWriter.WriteStartElement("sellwish", null);
            for (int i = 0; i < 3; i++)
            {
                myXmlTextWriter.WriteStartElement("item", null);
                myXmlTextWriter.WriteAttributeString("article", art[i]);
                myXmlTextWriter.WriteAttributeString("quantity", "Wert muss übergeben werden");
                myXmlTextWriter.WriteEndElement();
            }
            myXmlTextWriter.WriteEndElement();

            //Bereich selldirect
            string[] selldirectArr = new string[] { "article", "quantity", "price", "penalty" };

            myXmlTextWriter.WriteStartElement("selldirect", null);
            for (int i = 0; i < 3; i++ )
            {
                myXmlTextWriter.WriteStartElement("item", null);
                for (int x = 0; x<selldirectArr.Length;x++)
                {
                    myXmlTextWriter.WriteAttributeString("article", art[i]);
                    myXmlTextWriter.WriteAttributeString("quantity", "Wert muss übergeben werden");
                    myXmlTextWriter.WriteAttributeString("price", "Wert muss übergeben werden");
                    myXmlTextWriter.WriteAttributeString("penalty", "Wert muss übergeben werden");
                }
                myXmlTextWriter.WriteEndElement();
            }
            myXmlTextWriter.WriteEndElement();

            myXmlTextWriter.WriteStartElement("orderlist", null);
            for (int i = 0; i < 3; i++)
            {
                myXmlTextWriter.WriteStartElement("order", null);
                myXmlTextWriter.WriteAttributeString("article", art[i]);
                myXmlTextWriter.WriteAttributeString("quantity", "Wert muss übergeben werden");
                myXmlTextWriter.WriteAttributeString("modus", "Wert muss übergeben werden");
                myXmlTextWriter.WriteEndElement();
            }
            myXmlTextWriter.WriteEndElement();

            myXmlTextWriter.WriteStartElement("productionlist", null);
            for (int i = 0; i < 3; i++)
            {
                myXmlTextWriter.WriteStartElement("production", null);
                myXmlTextWriter.WriteAttributeString("article", art[i]);
                myXmlTextWriter.WriteAttributeString("quantity", "Wert muss übergeben werden");

                myXmlTextWriter.WriteEndElement();
            }
            myXmlTextWriter.WriteEndElement();

            myXmlTextWriter.WriteStartElement("workingtimelist", null);
            for (int i = 0; i < 3; i++)
            {
                myXmlTextWriter.WriteStartElement("workingtime", null);
                myXmlTextWriter.WriteAttributeString("station", art[i]);
                myXmlTextWriter.WriteAttributeString("shift", "Wert muss übergeben werden");
                myXmlTextWriter.WriteAttributeString("overtime", "Wert muss übergeben werden");
                myXmlTextWriter.WriteEndElement();
            }

            myXmlTextWriter.WriteEndElement();
            myXmlTextWriter.WriteEndElement();
            myXmlTextWriter.Flush();
            myXmlTextWriter.Close();
        }
    }
}
