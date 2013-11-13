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


        public Order(int orderperiod, int id, int mode, int article, int time, decimal materialcosts, decimal ordercosts, decimal entirecosts, decimal piececosts, int amount)
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
            Amount = amount;

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
            public int Item { get; set; }
            public decimal Timeneed { get; set; }
            public decimal Amount { get; set; }



            public Idletime(int id, int setupevents, int idletimes, decimal wageidletimecosts, decimal wagecosts, decimal machineidletimecosts, int item, int timeneed, int amount)
            {
                Id = id;
                Setupevents = setupevents;
                Idletimes = idletimes;
                Wageidletimecosts = wageidletimecosts;
                Wagecosts = wagecosts;
                Machineidletimecosts = machineidletimecosts;
                Item = item;
                Timeneed = timeneed;
                Amount = amount;

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
                        int period = 0;

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "results":
                                        System.Windows.Forms.MessageBox.Show("Results");
                                        if (reader.HasAttributes)
                                        {
                                            while (reader.MoveToNextAttribute())
                                            {
                                                if (reader.Name == "period")
                                                {
                                                    period = Convert.ToInt32(reader.Value);
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    case "Warehousestock":
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
                                            cmd.CommandText = @"insert into Lager (Teilenummer_FK, Bestand, Prozent, Teilewert, Lagerwert, Periode) values ('" + art.Id + "','" + art.Amount + "','" + art.Pct + "','" + art.Price + "','" + art.Stockvalue + "','" + period + "')";
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                                        }
                                        break;
                                    case "inwardstockmovement":
                                        Überelement = "inwardstockmovement";
                                        break;
                                    case "futureinwardstockmovement":
                                        Überelement = "futureinwardstockmovement";
                                        break;
                                    case "order":
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
                                            cmd.CommandText = @"insert into Bestellung (Teilenummer_FK, Menge, Modus_FK, Bestellperiode, Eingegangen, Lieferzeit, Materialkosten, Lieferkosten, Gesamtkosten, Stückkosten) values ('" + ord.Id + "','" + ord.Amount + "','" + ord.Mode + "','" + period + "'" + ",True,'" + ord.Time + "','" + ord.Materialcosts + "','" + ord.Ordercosts + "','" + ord.Entirecosts + "','" + ord.Piececosts + "')";

                                        }
                                        else if (Überelement == "futureinwardstockmovement")
                                        {
                                            cmd.CommandText = @"insert into Bestellung (Teilenummer_FK, Menge, Modus_FK, Bestellperiode, Eingegangen, Materialkosten, Lieferkosten, Gesamtkosten, Stückkosten) values ('" + ord.Id + "','" + ord.Amount + "','" + ord.Mode + "','" + period + "'" + ",False,'" + ord.Materialcosts + "','" + ord.Ordercosts + "','" + ord.Entirecosts + "','" + ord.Piececosts + "')";

                                        }
                                        cmd.ExecuteNonQuery();
                                        break;

                                    case "idletimecosts":
                                        Überelement = "idletimecosts";
                                        break;
                                    case "waitinglistworkstations":
                                        Überelement = "waitinglistworkstations";
                                        break;
                                    case "ordersinwork":
                                        Überelement = "ordersinwork";
                                        break;
                                    case "workplace":
                                        idle = new Idletime();
                                        idleliste.Add(idle);
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
                                                else if (reader.Name == "Amount")
                                                    idle.Amount = Convert.ToInt32(reader.Value);
                                                else if (reader.Name == "item")
                                                    idle.Item = Convert.ToInt32(reader.Value);
                                                else if (reader.Name == "timeneed")
                                                    idle.Timeneed = Convert.ToInt32(reader.Value);


                                            }
                                        }
                                        if (Überelement == "idletimecosts")
                                        {
                                            cmd.CommandText = @"insert into Leerzeitenkosten (Arbeitsplatz_FK, Rüstvorgänge, Leerzeit_min, Lohnleerkosten, Lohnkosten, Maschinenstillstandskosten, Periode) values ('" + idle.Id + "','" + idle.Setupevents + "','" + idle.Idletimes + "','" + idle.Wageidletimecosts + "','" + idle.Wagecosts + "','" + idle.Machineidletimecosts + "','" + period + "')";
                                        }
                                      //  else if (Überelement == "waitinglistworkstations")
                                        //{
                                            //SQL-Statement anpassen
                                           // cmd.CommandText = @"insert into Warteliste_Arbeitsplatz (Arbeitsplatz_FK, Teilenummer_FK, Menge, Zeitbedarf, Periode) values ('" + idle.Id + "','" + idle.Setupevents + "','" + idle.Idletimes + "','" + idle.Wageidletimecosts + "','" + idle.Wagecosts + "','" + idle.Machineidletimecosts + "','" + period + "')";
                                           // System.Windows.Forms.MessageBox.Show("SQL-Statement Warteliste_Arbeitsplatz \n+ " + cmd.CommandText);

                                        //}
                                        else if (Überelement == "ordersinwork")
                                        {
                                            //SQL-Statement anpassen, item und timeneed mit aufnehmen
                                            cmd.CommandText = @"insert into Bearbeitung (Arbeitsplatz_FK, Teilenummer_FK, Menge, Zeitbedarf, Periode) values ('" + idle.Id + "','" + idle.Item + "','" + idle.Amount + "','" + idle.Timeneed + "','" + period + "')";
                                            System.Windows.Forms.MessageBox.Show("SQL-Statement Bearbeitung \n+ " + cmd.CommandText);
                                        }
                                        cmd.ExecuteNonQuery();
                                        break;
                                    case "completedorders":
                                        Überelement = "completedorders";
                                        if (reader.HasAttributes) //Attributsliste durchlaufen
                                        {
                                            while (reader.MoveToNextAttribute())
                                            {

                                            }
                                        }
                                        break;
                                    case "result":
                                        Überelement = "result";
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
