using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBSYS2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace IBSYS2
    {
        public class Warehouse //Lager?
        {
            public int Id { get; set; }
            public int Artikel_Id { get; set; }
            public int Bestand { get; set; }
            public int Prozent { get; set; }
            public int Teilewert { get; set; }
            public int Lagerwert { get; set; }
            public int Periode { get; set; }

            public Warehouse(int id, int artikel_Id, int bestand, int prozent, int teilewert, int lagerwert, int periode)
            {
                Id = id;
                Artikel_Id = artikel_Id;
                Bestand = bestand;
                Prozent = prozent;
                Teilewert = teilewert;
                Lagerwert = lagerwert;
                Periode = Periode;
            }

            public Warehouse()
            {

            }

        }
        public class Lieferart
        {
            public int Id { get; set; }
            public int Nummer { get; set; }
            public String Bezeichnung { get; set; }

            public Lieferart(int id, int nummer, string bezeichnung)
            {
                Id = id;
                Nummer = nummer;
                Bezeichnung = bezeichnung;
            }

            public Lieferart()
            {

            }

        }
        public class Bearbeitung
        {
            public int Id { get; set; }
            public int Arbeitsplatz_FK { get; set; }
            public int Teilenummer_FK { get; set; }
            public int Menge { get; set; }
            public int Zeitbedarf { get; set; } //Typ Time?
            public int Periode { get; set; }

            public Bearbeitung(int id, int arbeitsplatz_FK, int teilenummer_FK, int menge, int zeitbedarf, int periode)
            {
                Id = id;
                Arbeitsplatz_FK = arbeitsplatz_FK;
                Teilenummer_FK = teilenummer_FK;
                Menge = menge;
                Zeitbedarf = zeitbedarf;
                Periode = periode;
            }

            public Bearbeitung()
            {

            }

        }
        public class Leerzeitkosten
        {
            public int Id { get; set; }
            public int Arbeitsplatz_FK { get; set; }
            public int Rüstvorgang { get; set; }
            public int Leerzeit_min { get; set; } //Typ Time?
            public int Lohnleerkosten { get; set; }
            public int Lohnkosten { get; set; }
            public int Machinenstillstandskosten { get; set; }
            public int Periode { get; set; }

            public Leerzeitkosten(int id, int arbeitsplatz_FK, int rüstvorgang, int leerzeit_min, int lohnleerkosten, int lohnkosten, int machinenstillstandskosten, int periode)
            {
                Id = id;
                Arbeitsplatz_FK = arbeitsplatz_FK;
                Rüstvorgang = rüstvorgang;
                Leerzeit_min = leerzeit_min;
                Lohnleerkosten = lohnleerkosten;
                Lohnkosten = lohnkosten;
                Machinenstillstandskosten = machinenstillstandskosten;
                Periode = periode;
            }

            public Leerzeitkosten()
            {

            }

        }
        public class Arbeitsplatz
        {
            public int Id { get; set; }
            public int Arbeitsplatznummer { get; set; }
            public string Bezeichung { get; set; }
            public int Lohn_Schicht_1 { get; set; }
            public int Lohn_Schicht_2 { get; set; }
            public int Lohn_Schicht_3 { get; set; }
            public int Lohn_Überstunden { get; set; }
            public int Variable_Maschinenkosten { get; set; }
            public int Fixe_Maschinenkosten { get; set; }

            public Arbeitsplatz(int id, int arbeitsplatznummer, string bezeichung, int lohn_Schicht_1, int lohn_Schicht_2, int lohn_Schicht_3, int lohn_Überstunden, int variable_Maschinenkosten, int fixe_Maschinenkosten)
            {
                Id = id;
                Arbeitsplatznummer = arbeitsplatznummer;
                Bezeichung = bezeichung;
                Lohn_Schicht_1 = lohn_Schicht_1;
                Lohn_Schicht_2 = lohn_Schicht_2;
                Lohn_Schicht_3 = lohn_Schicht_3;
                Lohn_Überstunden = lohn_Überstunden;
                Variable_Maschinenkosten = variable_Maschinenkosten;
                Fixe_Maschinenkosten = fixe_Maschinenkosten;
            }

            public Arbeitsplatz()
            {

            }

        }
    }

}
