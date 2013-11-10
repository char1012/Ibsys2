using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace IBSYS2
{

    public class Artikel
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int Startamount { get; set; }
        public decimal Pct { get; set; }
        public decimal Price { get; set; }
        public decimal Stockvalue { get; set; }

        public Artikel(int id, int amount, int startamount, decimal pct, decimal price, decimal stockvalue)
        {
            Id = id;
            Amount = amount;
            Startamount = startamount;
            Pct = pct;
            Price = price;
            Stockvalue = stockvalue;

        }

        public Artikel()
        {
            // TODO: Complete member initialization
        }
    }

    public class Order
    {
        //<order orderperiod="5" id="1" mode="5" article="22" amount="340" time="44640" materialcosts="1989,00" ordercosts="50,00" entirecosts="2039,00" piececosts="6,00"/>
        public int Orderperiod { get; set; }
        public int Id { get; set; }
        public int Mode { get; set; }
        public int Article { get; set; }
        public int Amount { get; set; }
        public int Time { get; set; }
        public decimal Materialcosts { get; set; }
        public decimal Ordercosts { get; set; }
        public decimal Entirecosts { get; set; }
        public decimal Piececosts { get; set; }


        public Order(int orderperiod, int id, int mode, int article, int time, decimal materialcosts, decimal ordercosts, decimal entirecosts, decimal piececosts)
        {
            Orderperiod = orderperiod;
            Id = id;
            Mode = mode;
            Article = article;
            Time = time;
            Materialcosts = materialcosts;
            Ordercosts = ordercosts;
            Entirecosts = entirecosts;
            Piececosts = piececosts;

        }

        public Order()
        {
            // TODO: Complete member initialization
        }

    }


    class XMLReaderClass
    {
        public void XMLReader(OleDbCommand cmd, String filename)
        {
            try
            {
                XmlReader reader = XmlReader.Create(filename);

                List<Artikel> artikelliste = new List<Artikel>();
                Artikel art = null;
                String Überelement = "";
                List<Order> orderliste = new List<Order>();
                Order ord = null;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Warehousestock":
                                MessageBox.Show("Warehousestock");
                                break;
                            case "article":
                                art = new Artikel();
                                artikelliste.Add(art);
                                if (reader.HasAttributes) //Attributsliste durchlaufen
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name == "id")
                                            art.Id = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "amount")
                                            art.Amount = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "startamount")
                                            art.Startamount = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "pct")
                                            art.Pct = Convert.ToDecimal(reader.Value);
                                        else if (reader.Name == "price")
                                            art.Price = Convert.ToDecimal(reader.Value);
                                        else if (reader.Name == "stockvalue")
                                            art.Stockvalue = Convert.ToDecimal(reader.Value);
                                    }

                                }
                                break;
                            case "inwardstockmovement":
                                MessageBox.Show("inwardstockmovement");
                                Überelement = "inwardstockmovement";
                                break;
                            case "futureinwardstockmovement":
                                MessageBox.Show("futureinwardstockmovement");
                                Überelement = "futureinwardstockmovement";
                                break;
                            case "order":
                                MessageBox.Show("order in " + Überelement);
                                ord = new Order();
                                orderliste.Add(ord);
                                if (reader.HasAttributes) //Attributsliste durchlaufen
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        cmd.CommandText = "";
                                        if (reader.Name == "orderperiod")
                                            ord.Orderperiod = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "id")
                                            ord.Id = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "mode")
                                            ord.Mode = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "article")
                                            ord.Article = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "time")
                                            ord.Time = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "materialcosts")
                                            ord.Materialcosts = Convert.ToDecimal(reader.Value);
                                        else if (reader.Name == "ordercosts")
                                            ord.Ordercosts = Convert.ToDecimal(reader.Value);
                                        else if (reader.Name == "entirecosts")
                                            ord.Entirecosts = Convert.ToDecimal(reader.Value);
                                        else if (reader.Name == "piececosts")
                                            ord.Piececosts = Convert.ToDecimal(reader.Value);
                                    }
                                }
                                //                                MessageBox.Show("SQL-Statement mit folgenden Parametern: " + "\nInwardstockmovement " + ord.Orderperiod + " Orderperiod, " + ord.Id + " ID, " + ord.Mode + " Mode, " + ord.Article + " Article, " + ord.Time + " Time, " + ord.Materialcosts + " IDMaterialcosts " + ord.Ordercosts + " Ordercosts, " + ord.Entirecosts + " Entirecosts, " + ord.Piececosts + " Piececosts");
                                if (Überelement == "inwardstockmovement")
                                {
                                    //cmd.CommandText = @"insert into Bestellung (B_Periode,LI_Art_FK,T_ID_FK,Menge,Liefer_Zeit,Materialkosten,Lieferkosten,Gesamtkosten, Stückkosten, Ausstehend) values (" + ord.Orderperiod + "," + ord.Id + "," + ord.Mode + "," + ord.Article + "," + ord.Time + "," + ord.Materialcosts + "," + ord.Ordercosts + "," + ord.Entirecosts + "," + ord.Piececosts + ",Nein)";
                                    cmd.CommandText = @"insert into Bestellung (B_Periode,T_ID_FK,Menge,Liefer_Zeit,Materialkosten,Lieferkosten,Gesamtkosten, Stückkosten,Ausstehend) values (" + ord.Orderperiod + "," + ord.Id + "," + ord.Amount + "," + ord.Mode + "," + ord.Article + "," + ord.Time + "," + ord.Materialcosts + "," + ord.Ordercosts + "," + ord.Entirecosts + "," + ord.Piececosts + ",'Nein')";
                                    MessageBox.Show("SQL-Statement inwardstockmovement \n+ " + cmd.CommandText);

                                }
                                else if (Überelement == "futureinwardstockmovement")
                                {
                                    //SQL-Statement noch anpassen
                                    MessageBox.Show("futureinwardstockmovement - SQL-Statement\n" + " " + cmd.CommandText);
                                    cmd.CommandText = @"insert into Bestellung (B_Periode,LI_Art_FK,T_ID_FK,Menge,Liefer_Zeit,Materialkosten,Lieferkosten,Gesamtkosten, Stückkosten,Ausstehend) values ('" + ord.Orderperiod + "','" + ord.Id + "','" + ord.Mode + "','" + ord.Article + "','" + ord.Time + "','" + ord.Materialcosts + "','" + ord.Ordercosts + "','" + ord.Entirecosts + "','" + ord.Piececosts + "','Ja')";
                                }
                                cmd.ExecuteNonQuery();
                                System.Windows.Forms.MessageBox.Show("Tabelle Bestellung erfolgreich erweitert ", "Caption", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                break;

                            case "idletimecosts":
                                Überelement = "idletimecosts";
                                MessageBox.Show("idletimecosts");
                                break;
                            case "workplace":
                                Überelement = "workplace";
                                //<workplace id="1" setupevents="2" idletime="330" wageidletimecosts="148,50" wagecosts="1426,50" machineidletimecosts="3,30"/>
                                int w_id = '0';
                                int w_setupevents = '0';
                                int w_idletime = '0';
                                int w_wageidletimecosts = '0';
                                int w_wagecosts = '0';
                                int w_machineidletimecosts = '0';
                                if (reader.HasAttributes) //Attributsliste durchlaufen
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        cmd.CommandText = "";
                                        if (reader.Name == "id")
                                            w_id = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "setupevents")
                                            w_setupevents = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "idletime")
                                            w_idletime = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "wageidletimecosts")
                                            w_wageidletimecosts = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "wagecosts")
                                            w_wagecosts = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "machineidletimecosts")
                                            w_machineidletimecosts = Convert.ToInt32(reader.Value);
                                    }
                                }
                                cmd.CommandText = @"insert into Bestellung (B_Periode,T_ID_FK,Menge,Liefer_Zeit,Materialkosten,Lieferkosten,Gesamtkosten, Stückkosten,Ausstehend) values (" + ord.Orderperiod + "," + ord.Id + "," + ord.Amount + "," + ord.Mode + "," + ord.Article + "," + ord.Time + "," + ord.Materialcosts + "," + ord.Ordercosts + "," + ord.Entirecosts + "," + ord.Piececosts + ",'Nein')"; cmd.ExecuteNonQuery();
                                System.Windows.Forms.MessageBox.Show("Tabelle Bestellung erfolgreich erweitert ", "Caption", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                break;
                            case "waitinglistworkstations":
                                Überelement = "waitinglistworkstations";
                                MessageBox.Show("waitinglistworkstations");
                                if (reader.HasAttributes) //Attributsliste durchlaufen
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        cmd.CommandText = "";
                                        if (reader.Name == "orderperiod")
                                            ord.Orderperiod = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "id")
                                            ord.Id = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "mode")
                                            ord.Mode = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "article")
                                            ord.Article = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "time")
                                            ord.Time = Convert.ToInt32(reader.Value);
                                        else if (reader.Name == "materialcosts")
                                            ord.Materialcosts = Convert.ToDecimal(reader.Value);
                                        else if (reader.Name == "ordercosts")
                                            ord.Ordercosts = Convert.ToDecimal(reader.Value);
                                        else if (reader.Name == "entirecosts")
                                            ord.Entirecosts = Convert.ToDecimal(reader.Value);
                                    }
                                }
                                break;
                            case "ordersinwork":
                                Überelement = "ordersinwork";
                                MessageBox.Show("ordersinwork");
                                if (reader.HasAttributes) //Attributsliste durchlaufen
                                {
                                    while (reader.MoveToNextAttribute())
                                    {

                                    }
                                }
                                break;
                            case "completedorders":
                                Überelement = "completedorders";
                                MessageBox.Show("completedorders");
                                if (reader.HasAttributes) //Attributsliste durchlaufen
                                {
                                    while (reader.MoveToNextAttribute())
                                    {

                                    }
                                }
                                break;
                            case "result":
                                Überelement = "result";
                                MessageBox.Show("result ");
                                if (reader.HasAttributes) //Attributsliste durchlaufen
                                {
                                    while (reader.MoveToNextAttribute())
                                    {

                                    }
                                }
                                break;
                        }
                    }
                }
                reader.Close();
            }
            catch
            { }

        }

    }
}
