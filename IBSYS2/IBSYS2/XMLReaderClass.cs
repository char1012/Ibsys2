using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.OleDb;
using System.Windows.Forms;


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
            public int Amount { get; set; }



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

                        XmlDocument doc = new XmlDocument();
                        doc.Load(filename);
                        XmlNode data = doc.DocumentElement;

                        List<Artikel> artikelliste = new List<Artikel>();
                        Artikel art = null;
                        List<Order> orderliste = new List<Order>();
                        Order ord = null;
                        List<Idletime> idleliste = new List<Idletime>();
                        Idletime idle = null;
                        int period = 0;
                        double lagerbestand = 0.0;


                        //Aktuelle Periode auslesen aus XML-Dokument
                        foreach (XmlNode node in data.SelectNodes("/results"))
                        {
                            period = Convert.ToInt32(node.Attributes["period"].InnerText);
                        }

                        art = new Artikel();
                        artikelliste.Add(art);
                        //Lagerbestand auslesen
                        foreach (XmlNode node in data.SelectNodes("/results/warehousestock"))
                        {
                            // Das minus 1 wegen dem zusätzlichen Childnode "Totalstockvalue"...
                            for (int i = 0; i < node.ChildNodes.Count; i++)
                            {
                                try
                                {
                                    if (node.ChildNodes[i].Name == "totalstockvalue")
                                    {
                                        lagerbestand = Convert.ToDouble(node.ChildNodes[i].InnerText);
                                        MessageBox.Show("Lagerbestand " + lagerbestand);
                                    }
                                    else
                                    {
                                        art.Id = Convert.ToInt32(node.ChildNodes[i].Attributes["id"].InnerText);
                                        art.Amount = Convert.ToInt32(node.ChildNodes[i].Attributes["amount"].InnerText);
                                        art.Startamount = Convert.ToInt32(node.ChildNodes[i].Attributes["startamount"].InnerText);
                                        art.Pct = Convert.ToDecimal(node.ChildNodes[i].Attributes["pct"].InnerText);
                                        art.Price = Convert.ToDecimal(node.ChildNodes[i].Attributes["price"].InnerText);
                                        art.Stockvalue = Convert.ToDecimal(node.ChildNodes[i].Attributes["stockvalue"].InnerText);
                                        cmd.CommandText = @"insert into Lager (Teilenummer_FK, Bestand, Prozent, Teilewert, Lagerwert, Periode) values ('" + art.Id + "','" + art.Amount + "','" + art.Pct + "','" + art.Price + "','" + art.Stockvalue + "','" + period + "')";
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                                }
                            }
                        }

                        ord = new Order();
                        orderliste.Add(ord);
                        foreach (XmlNode node in data.SelectNodes("/results/inwardstockmovement"))
                        {
                            for (int i = 0; i < node.ChildNodes.Count-5; i++)
                            {
                                ord.Orderperiod = Convert.ToInt32(node.ChildNodes[i].Attributes["orderperiod"].InnerText);
                                ord.Id = Convert.ToInt32(node.ChildNodes[i].Attributes["id"].InnerText);
                                ord.Mode = Convert.ToInt32(node.ChildNodes[i].Attributes["mode"].InnerText);
                                ord.Article = Convert.ToInt32(node.ChildNodes[i].Attributes["article"].InnerText);
                                ord.Amount = Convert.ToInt32(node.ChildNodes[i].Attributes["amount"].InnerText);
                                ord.Time = Convert.ToInt32(node.ChildNodes[i].Attributes["time"].InnerText);
                                ord.Materialcosts = Convert.ToDecimal(node.ChildNodes[i].Attributes["materialcosts"].InnerText);
                                ord.Ordercosts = Convert.ToDecimal(node.ChildNodes[i].Attributes["ordercosts"].InnerText);
                                ord.Entirecosts = Convert.ToDecimal(node.ChildNodes[i].Attributes["entirecosts"].InnerText);
                                ord.Piececosts = Convert.ToDecimal(node.ChildNodes[i].Attributes["piececosts"].InnerText);

                                try
                                {
                                    cmd.CommandText = @"insert into Bestellung (Teilenummer_FK, Menge, Modus_FK, Bestellperiode, Eingegangen, Lieferzeit, Materialkosten, Lieferkosten, Gesamtkosten, Stückkosten) values ('" + ord.Article + "','" + ord.Amount + "','" + ord.Mode + "','" + period + "'" + ",True,'" + ord.Time + "','" + ord.Materialcosts + "','" + ord.Ordercosts + "','" + ord.Entirecosts + "','" + ord.Piececosts + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                                }
                            }
                        }


                        foreach (XmlNode node in data.SelectNodes("/results/futureinwardstockmovement"))
                        {
                            for (int i = 0; i < node.ChildNodes.Count; i++)
                            {
                                ord.Orderperiod = Convert.ToInt32(node.ChildNodes[i].Attributes["orderperiod"].InnerText);
                                ord.Id = Convert.ToInt32(node.ChildNodes[i].Attributes["id"].InnerText);
                                ord.Mode = Convert.ToInt32(node.ChildNodes[i].Attributes["mode"].InnerText);
                                ord.Article = Convert.ToInt32(node.ChildNodes[i].Attributes["article"].InnerText);
                                ord.Amount = Convert.ToInt32(node.ChildNodes[i].Attributes["amount"].InnerText);

                                try
                                {
                                    cmd.CommandText = @"insert into Bestellung (Teilenummer_FK, Menge, Modus_FK, Bestellperiode, Eingegangen) values ('" + ord.Article + "','" + ord.Amount + "','" + ord.Mode + "','" + period + "'" + ",False)";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                                }
                            }
                        }

                        // ------------------------------------------------------------------------

                        idle = new Idletime();
                        idleliste.Add(idle);
                        foreach (XmlNode node in data.SelectNodes("/results/idletimecosts"))
                        {
                            for (int i = 0; i < node.ChildNodes.Count-1; i++)
                            {
                                idle.Id = Convert.ToInt32(node.ChildNodes[i].Attributes["id"].InnerText);
                                idle.Setupevents = Convert.ToInt32(node.ChildNodes[i].Attributes["setupevents"].InnerText);
                                idle.Idletimes = Convert.ToInt32(node.ChildNodes[i].Attributes["idletime"].InnerText);
                                idle.Wageidletimecosts = Convert.ToDecimal(node.ChildNodes[i].Attributes["wageidletimecosts"].InnerText);
                                idle.Wagecosts = Convert.ToDecimal(node.ChildNodes[i].Attributes["wagecosts"].InnerText);
                                idle.Machineidletimecosts = Convert.ToDecimal(node.ChildNodes[i].Attributes["machineidletimecosts"].InnerText);

                                try
                                {
                                    cmd.CommandText = @"insert into Leerzeitenkosten (Arbeitsplatz_FK, Rüstvorgänge, Leerzeit_min, Lohnleerkosten, Lohnkosten, Maschinenstillstandskosten, Periode) values ('" + idle.Id + "','" + idle.Setupevents + "','" + idle.Idletimes + "','" + idle.Wageidletimecosts + "','" + idle.Wagecosts + "','" + idle.Machineidletimecosts + "','" + period + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                                }
                            }
                        }

                        foreach (XmlNode node in data.SelectNodes("/results/waitingliststock/missingpart"))
                        {
                            int missingpart_id = Convert.ToInt32(node.Attributes["id"].InnerText);
                            for (int i = 0; i < node.ChildNodes.Count; i++)
                            {
                                int wl_period = Convert.ToInt32(node.ChildNodes[i].Attributes["period"].InnerText);
                                int order = Convert.ToInt32(node.ChildNodes[i].Attributes["order"].InnerText);
                                int item = Convert.ToInt32(node.ChildNodes[i].Attributes["item"].InnerText);
                                int wl_amount = Convert.ToInt32(node.ChildNodes[i].Attributes["amount"].InnerText);

                                try
                                {
                                    cmd.CommandText = @"insert into Warteliste_Material (Fehlteil_Teilenummer_FK, Erzeugnis_Teilenummer_FK, Menge, Periode) values ('" + missingpart_id + "','" + item + "','" + wl_amount + "','" + wl_period + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                                }
                            }
                        }

                        // Durchlauf der Arbeitsplätze sowie der Wartelisten der Arbeitsplätze
                        foreach (XmlNode node in data.SelectNodes("/results/waitinglistworkstations/workplace"))
                        {
                            int workplace_id = Convert.ToInt32(node.Attributes["id"].InnerText);
                            // Durchlauf der Arbeitsplatzspezifischen Wartelisten
                            for (int i = 0; i < node.ChildNodes.Count; i++)
                            {
                                int wl_period = Convert.ToInt32(node.ChildNodes[i].Attributes["period"].InnerText);
                                int item = Convert.ToInt32(node.ChildNodes[i].Attributes["item"].InnerText);
                                int amount = Convert.ToInt32(node.ChildNodes[i].Attributes["amount"].InnerText);
                                int timeneed = Convert.ToInt32(node.ChildNodes[i].Attributes["timeneed"].InnerText);

                                try
                                {
                                    cmd.CommandText = @"insert into Warteliste_Arbeitsplatz (Arbeitsplatz_FK, Teilenummer_FK, Menge, Zeitbedarf, Periode) values ('" + workplace_id + "','" + item + "','" + amount + "','" + timeneed + "','" + wl_period + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                                }
                            }
                        }

                        foreach (XmlNode node in data.SelectNodes("/results/ordersinwork"))
                        {
                            
                            for (int i = 0; i < node.ChildNodes.Count; i++)
                            {
                                int wp_ordersinwork = Convert.ToInt32(node.ChildNodes[i].Attributes["id"].InnerText);
                                int wl_period = Convert.ToInt32(node.ChildNodes[i].Attributes["period"].InnerText);
                                //int order = Convert.ToInt32(node.ChildNodes[i].Attributes["order"].InnerText);
                                int item = Convert.ToInt32(node.ChildNodes[i].Attributes["item"].InnerText);
                                int amount = Convert.ToInt32(node.ChildNodes[i].Attributes["amount"].InnerText);
                                int timeneed = Convert.ToInt32(node.ChildNodes[i].Attributes["timeneed"].InnerText);

                                try
                                {
                                    cmd.CommandText = @"insert into Bearbeitung (Arbeitsplatz_FK, Teilenummer_FK, Menge, Zeitbedarf, Periode) values ('" + wp_ordersinwork + "','" + item + "','" + amount + "','" + timeneed + "','" + wl_period + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                                }
                            }
                        }

                        //Deklaration oberhalb der For-Schleifen für gemeinsames Schreiben in DB
                        string eff_current = "0";
                        string eff_average = "0";
                        double sellw_current = 0.0;
                        double sellw_average = 0;
                        double sellw_all = 0;
                        string del_current = "0";
                        string dell_average = "0";
                        double idletime_current = 0;
                        double idletime_average = 0;
                        double itc_current = 0;
                        double itc_average = 0;
                        double sv_current = 0;
                        double sv_average = 0;
                        double scosts_current = 0;
                        double scosts_average = 0;
                        double ns_prof_current = 0;
                        double ns_prof_average = 0;
                        double ds_prof_current = 0;
                        double ds_prof_average = 0;
                        double mps_prof_current = 0;
                        double mps_prof_average = 0;
                        double sum_current = 0;
                        double sum_average = 0.0;



                        //Auslesen der einzelnen Elemente unter <result/>
                        foreach (XmlNode node in data.SelectNodes("/results//result/general"))
                        {


                            for (int i = 0; i < node.ChildNodes.Count; i++ )
                            {
                                if (node.ChildNodes[i].Name == "effiency")
                                {
                                    eff_current = Convert.ToString(node.ChildNodes[i].Attributes["current"].InnerText);
                                    eff_average = Convert.ToString(node.ChildNodes[i].Attributes["average"].InnerText);
                                }
                                if (node.ChildNodes[i].Name == "sellwish")
                                {
                                    sellw_current = Convert.ToDouble(node.ChildNodes[i].Attributes["current"].InnerText);
                                    sellw_average = Convert.ToDouble(node.ChildNodes[i].Attributes["average"].InnerText);
                                    sellw_all = Convert.ToDouble(node.ChildNodes[i].Attributes["all"].InnerText);
                                }
                                if (node.ChildNodes[i].Name == "deliveryreliability")
                                {
                                    del_current = Convert.ToString(node.ChildNodes[i].Attributes["current"].InnerText);
                                    dell_average = Convert.ToString(node.ChildNodes[i].Attributes["average"].InnerText);
                                }
                                if (node.ChildNodes[i].Name == "idletime")
                                {
                                    idletime_current = Convert.ToDouble(node.ChildNodes[i].Attributes["current"].InnerText);
                                    idletime_average = Convert.ToDouble(node.ChildNodes[i].Attributes["average"].InnerText);
                                }
                                if (node.ChildNodes[i].Name == "idletimecosts")
                                {
                                    itc_current = Convert.ToDouble(node.ChildNodes[i].Attributes["current"].InnerText);
                                    itc_average = Convert.ToDouble(node.ChildNodes[i].Attributes["average"].InnerText);
                                }
                                if (node.ChildNodes[i].Name == "storevalue")
                                {
                                    sv_current = Convert.ToDouble(node.ChildNodes[i].Attributes["current"].InnerText);
                                    sv_average = Convert.ToDouble(node.ChildNodes[i].Attributes["average"].InnerText);
                                }
                                if (node.ChildNodes[i].Name == "storagecosts")
                                {
                                    scosts_current = Convert.ToDouble(node.ChildNodes[i].Attributes["current"].InnerText);
                                    scosts_average = Convert.ToDouble(node.ChildNodes[i].Attributes["average"].InnerText);
                                }

                                 
                            }

                        
                        }

                        foreach (XmlNode node in data.SelectNodes("/results//result/normalsale"))
                        {
                            for (int i = 0; i < node.ChildNodes.Count; i++)
                            {
                                if (node.ChildNodes[i].Name == "profit")
                                {
                                    ns_prof_current = Convert.ToDouble(node.ChildNodes[i].Attributes["current"].InnerText);
                                    ns_prof_average = Convert.ToDouble(node.ChildNodes[i].Attributes["average"].InnerText);
                                }
                            }
                        }

                        foreach (XmlNode node in data.SelectNodes("/results//result/directsale"))
                        {
                            for (int i = 0; i < node.ChildNodes.Count; i++)
                            {
                                if (node.ChildNodes[i].Name == "profit")
                                {
                                    ds_prof_current = Convert.ToDouble(node.ChildNodes[i].Attributes["current"].InnerText);
                                    ds_prof_average = Convert.ToDouble(node.ChildNodes[i].Attributes["average"].InnerText);
                                }
                            }
                        }
                        foreach (XmlNode node in data.SelectNodes("/results//result/marketplacesale"))
                        {
                            for (int i = 0; i < node.ChildNodes.Count; i++)
                            {
                                if (node.ChildNodes[i].Name == "profit")
                                {
                                    mps_prof_current = Convert.ToDouble(node.ChildNodes[i].Attributes["current"].InnerText);
                                    mps_prof_average = Convert.ToDouble(node.ChildNodes[i].Attributes["average"].InnerText);
                                }
                            }
                        }
                        foreach (XmlNode node in data.SelectNodes("/results//result/summary"))
                        {
                            for (int i = 0; i < node.ChildNodes.Count; i++)
                            {
                                if (node.ChildNodes[i].Name == "profit")
                                {
                                    sum_current = Convert.ToDouble(node.ChildNodes[i].Attributes["current"].InnerText);
                                    sum_average = Convert.ToDouble(node.ChildNodes[i].Attributes["average"].InnerText);
                                }
                            }
                        }
                        try
                        {
                            //MessageBox.Show("insert into Informationen (Periode, Eff_Current, EFF_Average, Sellwish_Current, Sellwish_Average, Sellwish_All, Del_reliability_Current, Del_reliability_Average, Idletime_Current, Idletime_Average, IdletimeCosts_Current , IdletimeCosts_Average, Storevalue_Current, Storevalue_Average, Storacecosts_Current, Storagecosts_Average, Normalsale_Current, Normalsale_Average, Directsale_Current, Directsale_Average, MPSale_Current, MPSale_Average, Summary_Current, Summary_Average) values ('" + period + "','" + eff_current + "','" + eff_average + "','" + sellw_current + "','" + sellw_average + "','" + sellw_all + "','" + del_current + "','" + dell_average + "','" + idletime_current + "','" + idletime_average + "','" + itc_current + "','" + itc_average + "','" + sv_current + "','" + sv_average + "','" + scosts_current + "','" + scosts_average + "','" + ns_prof_current + "','" + ns_prof_average + "','" + ds_prof_current + "','" + ds_prof_average + "','" + mps_prof_current + "','" + mps_prof_average + "','" + sum_current + "','" + sum_average + "')");
                            cmd.CommandText = @"insert into Informationen (Periode, Eff_Current, EFF_Average, Sellwish_Current, Sellwish_Average, Sellwish_All, Del_reliability_Current, Del_reliability_Average, Idletime_Current, Idletime_Average, IdletimeCosts_Current , IdletimeCosts_Average, Durchschnitt_Storevalue_aktuell, Durchschnitt_Storevalue_Gesamt, Storacecosts_Current, Storagecosts_Average, Normalsale_Current, Normalsale_Average, Directsale_Current, Directsale_Average, MPSale_Current, MPSale_Average, Summary_Current, Summary_Average, Aktueller_Lagerbestand) values ('" + period + "','" + eff_current + "','" + eff_average + "','" + sellw_current + "','" + sellw_average + "','" + sellw_all + "','" + del_current + "','" + dell_average + "','" + idletime_current + "','" + idletime_average + "','" + itc_current + "','" + itc_average + "','" + sv_current + "','" + sv_average + "','" + scosts_current + "','" + scosts_average + "','" + ns_prof_current + "','" + ns_prof_average + "','" + ds_prof_current + "','" + ds_prof_average + "','" + mps_prof_current + "','" + mps_prof_average + "','" + sum_current + "','" + sum_average + "','" + lagerbestand + "')";
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Exception : \n" + ex);
                    }

                    System.Windows.Forms.MessageBox.Show("Die Dateien wurden erfolgreich importiert, vielen Dank für ihre Geduld.","XML-Datensatz eingelesen");
                }
            }
        }
