﻿using System;
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
        //Übergabe von mehrdimensionalen Array der anderen Parteien

        // Brotkrumenleiste noch verhindern (Vor und zurückgehen)
        // Bei falscher Eingabe in Felder Text noch rausnehmen
        // Präsentation Jan
        // Doku unter Kap. Startseite

                    try
            {
                ExportXMLClass exp = new ExportXMLClass();
                exp.XMLExport();
            }
            catch(Exception ex)
            {
                MessageBox.Show(""+ex);
            }


        public void XMLExport() //OleDbCommand cmd
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
            String[] orderlist_Array_Fields = {"article", "quantity", "modus" };
            String[,] orderlist_Array_Values = {{"25", "3600", "5"}, {"32", "3730", "5"},{"33", "820", "4"},{"34", "23300", "4"},{"36", "625", "5"}};

            XmlTextWriter myXmlTextWriter = new XmlTextWriter(@"C:\XML\TestAppendXML1.xml", null);
            myXmlTextWriter.Formatting = Formatting.Indented;

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
                for (int t = 0; t < 2; t++)
                {
                    myXmlTextWriter.WriteAttributeString(sellwish_Array_Fields[t], sellwish_Array_Values[i,t]);
                }
                myXmlTextWriter.WriteEndElement();
            }
            myXmlTextWriter.WriteEndElement();
            myXmlTextWriter.WriteStartElement("selldirect", null);
            for (int i = 0; i < 3; i++ )
            {
                myXmlTextWriter.WriteStartElement("item", null);
                for (int x = 0; x<4;x++)
                {
                    myXmlTextWriter.WriteAttributeString(selldirect_Array_Fields[x], selldirect_Array_Values[i, x]);
                }
                myXmlTextWriter.WriteEndElement();
            }
            myXmlTextWriter.WriteEndElement();

            myXmlTextWriter.WriteStartElement("orderlist", null);
            System.Windows.Forms.MessageBox.Show(orderlist_Array_Values.Length + " Länge");
            for (int i = 0; i < (orderlist_Array_Values.Length/3); i++)
            {
                myXmlTextWriter.WriteStartElement("order", null);
                for (int x = 0; x < 3; x++)
                {
                    myXmlTextWriter.WriteAttributeString(orderlist_Array_Fields[x], orderlist_Array_Values[i, x]);
                }
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
