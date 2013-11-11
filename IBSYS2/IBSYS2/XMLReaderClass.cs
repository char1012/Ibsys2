using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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

        public class Idletime
        {
            public int Id { get; set; }
            public int Setupevents { get; set; }
            public int Idletimes { get; set; }
            public decimal Wageidletimecosts { get; set; }
            public decimal Wagecosts { get; set; }
            public decimal Machineidletimecosts { get; set; }


            public Idletime(int id, int setupevents, int idletimes, decimal wageidletimecosts, decimal wagecosts, decimal machineidletimecosts)
            {
                Id = id;
                Setupevents = setupevents;
                Idletimes = idletimes;
                Wageidletimecosts = wageidletimecosts;
                Wagecosts = wagecosts;
                Machineidletimecosts = machineidletimecosts;

            }

            public Idletime()
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
                        List<Idletime> idleliste = new List<Idletime>();
                        Idletime idle = null;
                        System.Windows.Forms.MessageBox.Show("Anfang XMLReader");

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "Warehousestock":
                                        //MessageBox.Show("Warehousestock");
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
                                        try
                                        {
                                            cmd.CommandText = @"insert into Lager (Teilenummer_FK, Bestand, Prozent, Teilewert, Lagerwert, Periode) values ('" + art.Id + "','" + art.Amount + "','" + art.Pct + "','" + art.Price + "','" + art.Stockvalue + "','7')";
                                            //Console.WriteLine("SQL-Statement: \n" + cmd.CommandText);
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                                        }
                                        break;
                                    case "inwardstockmovement":
                                        //MessageBox.Show("inwardstockmovement");
                                        Überelement = "inwardstockmovement";
                                        break;
                                    case "futureinwardstockmovement":
                                        //MessageBox.Show("futureinwardstockmovement");
                                        Überelement = "futureinwardstockmovement";
                                        break;
                                    case "order":
                                        //MessageBox.Show("order in " + Überelement);
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
                                        if (Überelement == "inwardstockmovement")
                                        {
                                            //cmd.CommandText = @"insert into Bestellung (B_Periode,LI_Art_FK,T_ID_FK,Menge,Liefer_Zeit,Materialkosten,Lieferkosten,Gesamtkosten, Stückkosten, Ausstehend) values (" + ord.Orderperiod + "," + ord.Id + "," + ord.Mode + "," + ord.Article + "," + ord.Time + "," + ord.Materialcosts + "," + ord.Ordercosts + "," + ord.Entirecosts + "," + ord.Piececosts + ",Nein)";
                                            cmd.CommandText = @"insert into Bestellung (Teilenummer_FK, Menge, Modus_FK, Bestellperiode, Eingegangen, Lieferzeit, Materialkosten, Lieferkosten, Gesamtkosten, Stückkosten) values ('" + ord.Id + "','" + ord.Amount + "','" + ord.Mode + "','7'" + ",True,'" + ord.Time + "','" + ord.Materialcosts + "','" + ord.Ordercosts + "','" + ord.Entirecosts + "','" + ord.Piececosts + "')";
                                            //System.Windows.Forms.MessageBox.Show("SQL-Statement inwardstockmovement \n+ " + cmd.CommandText);

                                        }
                                        else if (Überelement == "futureinwardstockmovement")
                                        {
                                            //SQL-Statement noch anpassen
                                            cmd.CommandText = @"insert into Bestellung (Teilenummer_FK, Menge, Modus_FK, Bestellperiode, Eingegangen, Materialkosten, Lieferkosten, Gesamtkosten, Stückkosten) values ('" + ord.Id + "','" + ord.Amount + "','" + ord.Mode + "','7'" + ",False,'" + ord.Materialcosts + "','" + ord.Ordercosts + "','" + ord.Entirecosts + "','" + ord.Piececosts + "')";
                                            //System.Windows.Forms.MessageBox.Show("futureinwardstockmovement - SQL-Statement\n" + " " + cmd.CommandText);

                                        }
                                        cmd.ExecuteNonQuery();
                                        //System.Windows.Forms.MessageBox.Show("Tabelle Bestellung erfolgreich erweitert ", "Caption", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information);
                                        break;

                                    case "idletimecosts":
                                        //MessageBox.Show("futureinwardstockmovement");
                                        Überelement = "idletimecosts";
                                        break;
                                    case "waitinglistworkstations":
                                        //MessageBox.Show("futureinwardstockmovement");
                                        Überelement = "waitinglistworkstations";
                                        break;
                                    case "workplace":
                                        idle = new Idletime();
                                        idleliste.Add(idle);
                                        //MessageBox.Show("idletimecosts");
                                        if (reader.HasAttributes) //Attributsliste durchlaufen
                                        {
                                            while (reader.MoveToNextAttribute())
                                            {
                                                //cmd.CommandText = "";
                                                if (reader.Name == "id")
                                                    idle.Id = Convert.ToInt32(reader.Value);
                                                else if (reader.Name == "setupevents")
                                                    idle.Setupevents = Convert.ToInt32(reader.Value);
                                                else if (reader.Name == "idletime")
                                                    idle.Idletimes = Convert.ToInt32(reader.Value);
                                                else if (reader.Name == "wageidletimecosts")
                                                    idle.Wageidletimecosts = Convert.ToDecimal(reader.Value);
                                                else if (reader.Name == "wagecosts")
                                                    idle.Wagecosts = Convert.ToDecimal(reader.Value);
                                                else if (reader.Name == "machineidletimecosts")
                                                    idle.Machineidletimecosts = Convert.ToDecimal(reader.Value);
                                            }
                                        }
                                        if (Überelement == "idletimecosts")
                                        {
                                            cmd.CommandText = @"insert into Leerzeitenkosten (Arbeitsplatz_FK, Rüstvorgänge, Leerzeit_min, Lohnleerkosten, Lohnkosten, Maschinenstillstandskosten, Periode) values ('" + idle.Id + "','" + idle.Setupevents + "','" + idle.Idletimes + "','" + idle.Wageidletimecosts + "','" + idle.Wagecosts + "','" + idle.Machineidletimecosts + "','7')";
                                            System.Windows.Forms.MessageBox.Show("SQL-Statement idletime \n+ " + cmd.CommandText);
                                        }
                                        else if (Überelement == "waitinglistworkstations")
                                        {
                                            //SQL-Statement anpassen
                                            //cmd.CommandText = @"insert into Warteliste_Arbeitsplatz (Arbeitsplatz_FK, Rüstvorgänge, Leerzeit_min, Lohnleerkosten, Lohnkosten, Maschinenstillstandskosten, Periode) values ('" + idle.Id + "','" + idle.Setupevents + "','" + idle.Idletimes + "','" + idle.Wageidletimecosts + "','" + idle.Wagecosts + "','" + idle.Machineidletimecosts + "','7')";
                                            System.Windows.Forms.MessageBox.Show("SQL-Statement Warteliste_Arbeitsplatz \n+ " + cmd.CommandText);
                                        }
                                        cmd.ExecuteNonQuery();
                                        System.Windows.Forms.MessageBox.Show("Tabelle Leerzeitenkosten erfolgreich erweitert ", "Caption", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information);
                                        break;
                                    case "ordersinwork":
                                        Überelement = "ordersinwork";
                                        //MessageBox.Show("ordersinwork");
                                        if (reader.HasAttributes) //Attributsliste durchlaufen
                                        {
                                            while (reader.MoveToNextAttribute())
                                            {

                                            }
                                        }
                                        break;
                                    case "completedorders":
                                        Überelement = "completedorders";
                                        //MessageBox.Show("completedorders");
                                        if (reader.HasAttributes) //Attributsliste durchlaufen
                                        {
                                            while (reader.MoveToNextAttribute())
                                            {

                                            }
                                        }
                                        break;
                                    case "result":
                                        Überelement = "result";
                                        //MessageBox.Show("result ");
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
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                    }

                }

            }
        }
