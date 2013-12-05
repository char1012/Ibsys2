using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBSYS2
{
    class Sprachen
    {
        //DEUTSCH
        // Brotkrumenleiste
        public static String DE_LBL_STARTSEITE = "Startseite";
        public static String DE_LBL_SICHERHEITSBESTAND = "Sicherheitsbestand";
        public static String DE_LBL_PRODUKTION = "Produktion";
        public static String DE_LBL_KAPATITAETSPLAN = "Kapazitätsplan"; //Umlaut
        public static String DE_LBL_KAUFTEILEDISPOSITION = "Kaufteildisposition";
        public static String DE_LBL_ERGEBNIS = "Ergebnis";

        //ImportProgress
        //Buttons
        public static String DE_BTN_IP_BERECHNUNG_STARTEN = "Berechnung starten";
        public static String DE_BTN_IP_SPRUNG = "Sprung";
        public static String DE_BTN_IP_DATEI_AUSWAEHLEN = "Datei auswählen";

        //Groupbox
        public static String DE_IP_GROUPBOX1 = "Import der XML-Datei und Prognose - Zusätzliche Lieferanfragen (optional)";

        //Labels
        public static String DE_LBL_IP_SCHRITT1 = "1. Schritt:";
        public static String DE_LBL_IP_SCHRITT2 = "2. Schritt:";
        public static String DE_LBL_IP_SCHRITT3 = "3. Schritt:";
        public static String DE_LBL_IP_OPTIONAL = "(optional)";
        public static String DE_LBL_IP_AKTUELLE_PERIODE = "Aktuelle Periode";
        public static String DE_LBL_IP_PERIODEX = "Periode X";
        public static String DE_LBL_IP_PERIODEX1 = "Periode X+1";
        public static String DE_LBL_IP_PERIODEX2 = "Periode X+2";

        //InfoBox
        public static String DE_IP_INFO = "Wählen Sie als erstes die aktuelle Periode aus und betätigen Sie anschließend die bereitgestellte Schaltfläche zum Import der XML-Datei. \nIm Anschluss geben Sie bitte ihre Prognosen für die kommenden Perioden ein. \nAnschließend können Sie mit der Bearbeitung fortfahren.";
        public static String DE_IP_INFO_SCHRITT1 = "Wählen Sie als erstes die aktuelle Periode aus und betätigen Sie anschließend die bereitgestellte Schaltfläche zum Import der XML-Datei.";
        public static String DE_IP_INFO_SCHRITT2 = "Geben Sie nun Ihre Prognose für die nächsten Perioden an.";

        //ComboBox
        public static String DE_CB_IP_PERIODE_AUSWAEHLEN = "Wählen Sie die zu bearbeitende Periode aus";

        //Kaufteiledisposition
        //Groupboxen
        public static String DE_KD_GROUPBOX1 = "Kaufteildisposition";

        //Labels
        public static String DE_LBL_KP_MENGE = "Menge";
        public static String DE_LBL_KP_BESTELLART = "Bestellart";

        //ToolTip
        public static String DE_KD_INFO = " ";



        //Sicherheitsbestand
        //Buttons
        public static String DE_BTN_ETEILEBERECHNEN = "E-Teile Berechnen";
        public static String DE_BTN_CONTINUE = "Weiter";
        public static String DE_BTN_BACK = "Zurück"; //Umlaut

        //Groupboxen
        public static String DE_GROUPBOX1 = "P-Teile - Sicherheitsbestand";
        public static String DE_GROUPBOX3 = "E-Teile - Sicherheitsbestand";
        public static String DE_GROUPBOX2 = "Produktion";

        //Labels
        public static String DE_LABEL4 = "Die hier berechneten Werte können von Ihnen überschrieben werden.";
        public static String DE_LABLE9 = "* E-Teile werden zur Weiterberechnung summiert";

        //Tooltip
        public static String DE_INFOP = "Bitte den Sicherheitsbestand eingeben, welcher für die P-Teile gehalten werden soll.";
        public static String DE_INFOE = "- Diese Felder der Sicherheitsbestände für die E-Teile ist vor Berechnung der P-Teile nicht pflegbar. \n" + "- Das Ergbenis der Sicherheitsbestände der E-Teile wird vom System berechnet, können aber nach Bedarf händisch nachgefplegt werden. \n" + "- Um fortzufahren auf 'Fortfahren' klicken.";
        

        //ENGLISH
        // Brotkrumenleiste
        public static String EN_LBL_STARTSEITE = "Homepage";
        public static String EN_LBL_SICHERHEITSBESTAND = "Safty Stock";
        public static String EN_LBL_PRODUKTION = "Production";
        public static String EN_LBL_KAPATITAETSPLAN = "Capacity Plan";
        public static String EN_LBL_KAUFTEILEDISPOSITION = "Purchased parts dispostion";
        public static String EN_LBL_ERGEBNIS = "Result";

        //ImportProgress
        //Buttons
        public static String EN_BTN_IP_BERECHNUNG_STARTEN = "Calculation Start";
        public static String EN_BTN_IP_SPRUNG = "Jump";
        public static String EN_BTN_IP_DATEI_AUSWAEHLEN = "Data select";

        //Groupbox
        public static String EN_IP_GROUPBOX1 = "Import of the XML-file and forecast - extra supply query (optional)";

        //Labels
        public static String EN_LBL_IP_SCHRITT1 = "1. Step:";
        public static String EN_LBL_IP_SCHRITT2 = "2. Step:";
        public static String EN_LBL_IP_SCHRITT3 = "3. Step:";
        public static String EN_LBL_IP_OPTIONAL = "(optional)";
        public static String EN_LBL_IP_AKTUELLE_PERIODE = "current period";
        public static String EN_LBL_IP_PERIODEX = "Period X";
        public static String EN_LBL_IP_PERIODEX1 = "Period X+1";
        public static String EN_LBL_IP_PERIODEX2 = "Period X+2";

        //InfoBox
        public static String EN_IP_INFO = "First of all, select the current period, and then press the provided button to import the XML file. \nAfter that please enter their forecasts for the coming periods. \nYou can then continue editing";
        public static String EN_IP_INFO_SCHRITT1 = "First, select the current period, and then press the provided button to import the XML file.";
        public static String EN_IP_INFO_SCHRITT2 = "Now enter the forecasts for the next period.";

        //ComboBox
        public static String EN_CB_IP_PERIODE_AUSWAEHLEN = "Select the period to edit";

        //Kaufteiledisposition
        //Groupboxen
        public static String EN_KD_GROUPBOX1 = "Disposition of bought-out components";

        //Labels
        public static String EN_LBL_KP_MENGE = "Amount";
        public static String EN_LBL_KP_BESTELLART = "Order type";

        //ToolTip
        public static String EN_KD_INFO = " ";

        //Sicherheitsbestand
        //Buttons
        public static String EN_BTN_ETEILEBERECHNEN = "E-Items calculate";
        public static String EN_BTN_CONTINUE = "Continue";
        public static String EN_BTN_BACK = "Back";


        //Groupboxen
        public static String EN_GROUPBOX1 = "P-Items - Safty Stock";
        public static String EN_GROUPBOX3 = "E-Items - Safty Stock";
        public static String EN_GROUPBOX2 = "Production";

        //Labels
        public static String EN_LABEL4 = "You can overwrite the calculated values.";
        public static String EN_LABLE9 = "* E-Items are summed for further calcualtion.";

        //Tooltip
        public static String EN_INFOP = "Please enter the Safty-Stock for the P-Items.";
        public static String EN_INFOE = "- These fields of safety stocks for the E-Items is not maintainable before calculation of the P-Items. \n "+" - The result showing the security holdings of the E-Items is calculated by the system, but can be changed manually as needed. \n "+" - to continue on 'Continue' button.";
            

    }
}
