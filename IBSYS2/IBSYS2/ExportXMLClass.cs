using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.OleDb;
using System.Xml.Linq;
using System.Threading;
using System.Globalization;

namespace IBSYS2
{
    class ExportXMLClass
    {

        /*TO-DO
         * Benötige folgende Daten aus den Forms davor bzw. der Ergebnis-Form:
         * Jeweils zweidimensionale Arrays:
         * * sellwish: jeweiliger Artikel (P1, P2, P3) und die dazugehörige Anzahl
         * * selldirect: jeweiliger Artikel (P1, P2, P3) dazugehörige Anzahl, der Preis dazu sowie mögliche Sanktionen
         * * orderlist: welcher Artikel bestellt wird in welcher Menge und welchem Modus
         * * productionlist: welcher Artikel in welcher Menge hergestellt wird
         * * workingtimelist: an welchem Arbeitsplatz mit welcher Schicht mit wieviel Überstunden in Minuten gearbeitet wird
         * */

        //orderlist = kaufautraege, 6 dimensional, item 0o, 4und5, überall wo prod / ord menge null ist, nicht eintragen
        //productionlist = prodReihenfolge, 0, 1, überall wo prod / ord menge null ist, nicht eintragen
        //woringtimelist = kapaztät
        //für kapazität prüfen, ob id = 5, falls der Fall, gesamte Zeile löschen
        //sellwish = auftraege, ersten drei einträge fürP1-P3
        //selldirect = selldirect


        public void XMLExport(String pfad, int[,] kaufauftraege, List<List<int>> prodReihenfolge, int[,] kapazitaet, int[] auftraege, double[,] direktverkaeufe) //OleDbCommand cmd
        {
            XmlDocument doc = new XmlDocument();
            XmlNode myRoot; //, myNode;

            myRoot = doc.CreateElement("input");
            doc.AppendChild(myRoot);

            string[] childNodesXML = new string[] { "qualitycontrol", "sellwish", "selldirect", "orderlist", "productionlist", "workingtimelist" };
            string[] art = new string[] { "1", "2", "3" };
            string[] sellwishArr = new string[] { "article", "quantity" };

            //Mockupdaten für die angeforderten Daten
            String[] sellwish_Array_Fields = { "article", "quantity" };
            String[,] sellwish_Array_Values = new string[,] { { "1", "200" }, { "2", "200" }, { "3", "100" } };
            String[] selldirect_Array_Fields = { "article", "quantity", "price", "penalty" };
            String[,] selldirect_Array_Values = new string[,] { { "1", "0", "0.0", "0.0" }, { "2", "0", "0.0", "0.0" }, { "3", "150", "210.0", "20.0" } };
            String[] orderlist_Array_Fields = { "article", "quantity", "modus" };
            String[,] orderlist_Array_Values = { { "25", "3600", "5" }, { "32", "3730", "5" }, { "33", "820", "4" }, { "34", "23300", "4" }, { "36", "625", "5" } };

            XmlTextWriter myXmlTextWriter = new XmlTextWriter(pfad + @"\TestAppendXML1.xml", null);
            myXmlTextWriter.Formatting = Formatting.Indented;

            myXmlTextWriter.WriteStartElement("input");
            myXmlTextWriter.WriteStartElement("qualitycontrol", null);
            myXmlTextWriter.WriteAttributeString("type", "no");
            myXmlTextWriter.WriteAttributeString("losequantity", "0");
            myXmlTextWriter.WriteAttributeString("delay", "0");
            myXmlTextWriter.WriteEndElement();
            //Bereich sellwish - ErgebnisArray: auftraege, ersten drei einträge fürP1-P3
            myXmlTextWriter.WriteStartElement("sellwish", null);
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    myXmlTextWriter.WriteStartElement("item", null);
                    //for (int t = 0; t < 2; t++)
                    //{
                    myXmlTextWriter.WriteAttributeString("article", "" + (i+1));
                    myXmlTextWriter.WriteAttributeString("quantity", Convert.ToString(auftraege[i]));
                    //}
                    myXmlTextWriter.WriteEndElement();
                }
                catch(Exception ex)
                {

                }
            }
            myXmlTextWriter.WriteEndElement();
            myXmlTextWriter.WriteStartElement("selldirect", null);
            for (int i = 0; i < 3; i++)
            {
                //if (direktverkaeufe[i, 0] != 0)
                //{
                myXmlTextWriter.WriteStartElement("item", null);
                for (int x = 0; x < 4; x++)
                {
                    if (x == 0)
                    {
                        direktverkaeufe[i, x] = i + 1;
                        myXmlTextWriter.WriteAttributeString(selldirect_Array_Fields[x], Convert.ToString(direktverkaeufe[i, x]));
                    }
                    else if (x == 2 || x == 3)
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
                        myXmlTextWriter.WriteAttributeString(selldirect_Array_Fields[x], direktverkaeufe[i, x].ToString("F"));
                    }
                    else
                    {
                        myXmlTextWriter.WriteAttributeString(selldirect_Array_Fields[x], Convert.ToString(direktverkaeufe[i, x]));//selldirect_Array_Values[i, x]);
                    }
                    //MessageBox.Show("Selldirect - Feld" + x + ": " + selldirect_Array_Fields[x] + ", Wert: " + Convert.ToString(direktverkaeufe[i, x]));
                }
                myXmlTextWriter.WriteEndElement();
                //}
                //else
                //{
                //    //MessageBox.Show("Werte sind null: "+selldirect_Array_Fields[x]+ ", " + direktverkaeufe[i,x]);
                //}
            }
            myXmlTextWriter.WriteEndElement();

            myXmlTextWriter.WriteStartElement("orderlist", null);
            for (int i = 0; i < (kaufauftraege.Length / 6); i++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (kaufauftraege[i, 5] != 0)
                    {
                        myXmlTextWriter.WriteStartElement("order", null);
                        if (x == 0)
                        {
                            myXmlTextWriter.WriteAttributeString(orderlist_Array_Fields[x], Convert.ToString(kaufauftraege[i, 0]));//orderlist_Array_Values[i, x]);
                            //MessageBox.Show("orderlist - "+i+" Feld: " + orderlist_Array_Fields[x] + ", Wert: " + Convert.ToString(kaufauftraege[i, 0]));
                        }
                        else if (x == 1)
                        {
                            if (kaufauftraege[i, 4] != 0)
                            {
                                myXmlTextWriter.WriteAttributeString(orderlist_Array_Fields[x], Convert.ToString(kaufauftraege[i, 4]));//orderlist_Array_Values[i, x]);
                                //MessageBox.Show("orderlist - " + i + " Feld: " + orderlist_Array_Fields[x] + ", Wert: " + Convert.ToString(kaufauftraege[i, 4]));
                            }
                        }
                        else if (x == 2)
                        {
                            myXmlTextWriter.WriteAttributeString(orderlist_Array_Fields[x], Convert.ToString(kaufauftraege[i, 5]));//orderlist_Array_Values[i, x]);
                            //MessageBox.Show("orderlist - " + i + " Feld: " + orderlist_Array_Fields[x] + ", Wert: " + Convert.ToString(kaufauftraege[i, 5]));
                        }
                        else
                        { }
                        myXmlTextWriter.WriteEndElement();
                    }
                }
            }
            myXmlTextWriter.WriteEndElement();
            //prodReihenfolge
            myXmlTextWriter.WriteStartElement("productionlist", null);
            for (int i = 0; i < (prodReihenfolge.Count); i++)
            {
                if (prodReihenfolge[i][1] != 0)
                {
                    myXmlTextWriter.WriteStartElement("production", null);
                    myXmlTextWriter.WriteAttributeString("article", Convert.ToString(prodReihenfolge[i][0]));//art[i]);
                    myXmlTextWriter.WriteAttributeString("quantity", Convert.ToString(prodReihenfolge[i][1]));
                    myXmlTextWriter.WriteEndElement();
                    //MessageBox.Show("article: " + Convert.ToString(prodReihenfolge[i, 0]) + ", quantity: "+Convert.ToString(prodReihenfolge[i,1]));
                }
            }
            myXmlTextWriter.WriteEndElement();
            //kapazitaet
            myXmlTextWriter.WriteStartElement("workingtimelist", null);
            for (int i = 0; i < (kapazitaet.Length / 5); i++)
            {
                if (kapazitaet[i, 0] != 5)
                {

                    myXmlTextWriter.WriteStartElement("workingtime", null);
                    myXmlTextWriter.WriteAttributeString("station", Convert.ToString(kapazitaet[i, 0]));//art[i]);
                    myXmlTextWriter.WriteAttributeString("shift", Convert.ToString(kapazitaet[i, 1]));
                    myXmlTextWriter.WriteAttributeString("overtime", Convert.ToString(kapazitaet[i, 2]));
                    myXmlTextWriter.WriteEndElement();
                    //MessageBox.Show("workingtimelist - station: " + Convert.ToString(kapazitaet[i, 0]) + ", shift " + Convert.ToString(kapazitaet[i, 1]) + ", overtime " + Convert.ToString(kapazitaet[i, 2]));
                }
            }

            myXmlTextWriter.WriteEndElement();
            myXmlTextWriter.WriteEndElement();
            myXmlTextWriter.Flush();
            myXmlTextWriter.Close();
        }
    }
}
