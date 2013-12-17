using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBSYS2
{
    class Sprachen
    {
        /*--------------------------------------------------------------------------------------------------------*/
        /*---------------------DEUTSCH----------------------------------------------------------------------------*/
        /*--------------------------------------------------------------------------------------------------------*/
        // Brotkrumenleiste
        public static String DE_LBL_STARTSEITE = "Startseite";
        public static String DE_LBL_SICHERHEITSBESTAND = "Sicherheitsbestand";
        public static String DE_LBL_PRODUKTION = "Produktion";
        public static String DE_LBL_PRODUKTIONSREIHENFOLGE = "Produktionsreihenfolge";
        public static String DE_LBL_KAPATITAETSPLAN = "Kapazitätsplan"; //Umlaut
        public static String DE_LBL_KAUFTEILEDISPOSITION = "Kaufteildisposition";
        public static String DE_LBL_ERGEBNIS = "Ergebnis";

        /*---------Begruessungsseite----------*/
        //Button
        public static String DE_BTN_CLEAR = "Datenbank leeren";
        public static String DE_MSG_INFO1 = "Sind Sie sicher, dass Sie die Anwendung schließen möchten?\nDadurch werden alle Änderungen verworfen.";
        public static String DE_MSG_INFO2 = "Anwendung schließen";

        /*---------ImportProgress----------*/
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

        /*---------Kaufteiledisposition----------*/
        //Groupboxen
        public static String DE_KD_GROUPBOX1 = "Kaufteildisposition";

        //Labels
        public static String DE_LBL_KD_MENGE = "Menge";
        public static String DE_LBL_KD_BESTELLART = "Bestellart";

        //ToolTip
        public static String DE_KD_INFO = " ";

        /*---------Kapazitätsplan----------*/
        //Button
        public static String DE_BTN_DEFAULT = "Berechnung wiederherstellen";

        //Groupbox
        public static String DE_KP_GROUPBOX1 = "Kapazitätsplan";

        //Label
        public static String DE_LBL_KD_INFO = "Die hier berechneten Werte können von Ihnen überschrieben werden.";
        public static String DE_LBL_KD_KBEDARF = "Kapazitätsbedarf";
        public static String DE_LBL_KD_UEBERSTUNDENP = "Überstunden/Periode";
        public static String DE_LBL_KD_UEBERSTUNDENT = "UeberstundenTag";
        public static String DE_LBL_KD_SCHICHTEN = "Schichten";

        //ToolTip
        public static String DE_KP_INFO = "- Der berechnete Kapazitätsbedarf ist nicht änderbar. " +
                "Sie können jedoch für jeden Arbeitsplatz die Überstunden pro Periode " +
                "\n   und die Anzahl der Schichten anpassen. " +
                "Eine Änderung bei Überstunden/Periode bewirkt eine Neuberechnung von Überstunden/Tag. " +
                "\n- Wenn in der Zeile Schichten eine rote 3 angezeigt wird, " +
                "bedeutet dies, dass mehr als drei Schichten benötigt werden.\n   In diesem Fall sollten Sie " +
                "ihre Produktionsmengen anpassen.\n- Den Arbeitsplatz 5 gibt es nicht.";

        /*---------Produktion----------*/
        //Button
        public static String DE_BTN_ETEILE = "E-Teile";

        //Groupbox
        public static String DE_PR_GROUPBOX1 = "Produktion planen";

        //ToolTip
        public static String DE_PR_INFO = "Sie können die hier berechneten Produktionswerte überschreiben. \nUm die Produktion der E-Teile zu überprüfen, klicken Sie auf den Button E-Teile.";
        /*---------Produktion E-Teile----------*/
        public static String DE_PRE_GB_ETEILE = "Produktion der Eigenprodukte";

        /*---------Sicherheitsbestand----------*/
        //Buttons
        public static String DE_BTN_ETEILEBERECHNEN = "E-Teile berechnen";
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
        public static String DE_INFOE = "- Diese Felder der Sicherheitsbestände für die E-Teile ist vor Berechnung der P-Teile nicht pflegbar.\n" + "- Das Ergbenis der Sicherheitsbestände der E-Teile wird vom System berechnet, können aber nach Bedarf händisch nachgefplegt werden.\n" + "- Um fortzufahren auf 'Fortfahren' klicken.";

        /*---------Direktverkäufe----------*/
        //Groupbox
        public static String DE_DV_GROUPBOX1 = "Direktverkäufe";

        //Labels
        public static String DE_DV_LABEL4 = "Menge";
        public static String DE_DV_LABEL5 = "Preis";
        public static String DE_DV_LABEL6 = "Strafbetrag";

        /*---------Ergebnis----------*/
        //Buttons
        public static String DE_BTN_XML_EXPORT = "XML export";

        //Groupbox
        public static String DE_ER_GROUPBOX2 = "Einkaufsaufträge";
        public static String DE_ER_GROUPBOX3 = "Produktionsaufträge";
        public static String DE_ER_GROUPBOX4 = "Produktionskapazitäten";

        /*--------------------------------------------------------------------------------------------------------*/
        /*---------------------ENGLISH----------------------------------------------------------------------------*/
        /*--------------------------------------------------------------------------------------------------------*/
        // Brotkrumenleiste
        public static String EN_LBL_STARTSEITE = "Homepage";
        public static String EN_LBL_SICHERHEITSBESTAND = "Safty Stock";
        public static String EN_LBL_PRODUKTIONSREIHENFOLGE = "Production sequence";
        public static String EN_LBL_PRODUKTION = "Production";
        public static String EN_LBL_KAPATITAETSPLAN = "Capacity Plan";
        public static String EN_LBL_KAUFTEILEDISPOSITION = "Purchased parts disposition";
        public static String EN_LBL_ERGEBNIS = "Result";

        /*---------Begruessungsseite----------*/
        //Button
        public static String EN_BTN_CLEAR = "Empty database";
        public static String EN_MSG_INFO1 = "Are you sure to close the application?\nBy that all changes are discarded.";
        public static String EN_MSG_INFO2 = "Close application";

        /*---------ImportProgress----------*/
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
        public static String EN_LBL_IP_AKTUELLE_PERIODE = "Current period";
        public static String EN_LBL_IP_PERIODEX = "Period X";
        public static String EN_LBL_IP_PERIODEX1 = "Period X+1";
        public static String EN_LBL_IP_PERIODEX2 = "Period X+2";

        //InfoBox                          Wählen Sie als erstes die aktuelle Periode aus und betätigen Sie anschließend die bereitgestellte Schaltfläche zum Import der XML-Datei. \nIm Anschluss geben Sie bitte ihre Prognosen für die kommenden Perioden ein. \nAnschließend können Sie mit der Bearbeitung fortfahren.
        public static String EN_IP_INFO = "First of all, select the current period, and then press the provided button to import the XML file.                                                                         \nAfter that please enter their forecasts for the coming periods.             \nYou can then continue editing                          ";
        public static String EN_IP_INFO_SCHRITT1 = "First, select the current period, and then press the provided button to import the XML file.                                                                                 ";
        public static String EN_IP_INFO_SCHRITT2 = "Now enter the forecasts for the next period.                           ";

        //ComboBox
        public static String EN_CB_IP_PERIODE_AUSWAEHLEN = "Select the period to edit";

        /*---------Kaufteiledisposition----------*/
        //Groupboxen
        public static String EN_KD_GROUPBOX1 = "Disposition of bought-out components";

        //Labels
        public static String EN_LBL_KD_MENGE = "Amount";
        public static String EN_LBL_KD_BESTELLART = "Order type";

        //ToolTip
        public static String EN_KD_INFO = " ";

        /*---------Kapazitätsplan----------*/
        //Button
        public static String EN_BTN_DEFAULT = "Restor calculation";

        //Groupbox
        public static String EN_KP_GROUPBOX1 = "Capacity Plan";

        //Label
        public static String EN_LBL_KD_INFO = "The calculated values can be override";
        public static String EN_LBL_KD_KBEDARF = "Capacity requirements";
        public static String EN_LBL_KD_UEBERSTUNDENP = "Overtime/Period";
        public static String EN_LBL_KD_UEBERSTUNDENT = "Overtime/Day";
        public static String EN_LBL_KD_SCHICHTEN = "Shift";

        //ToolTip
        public static String EN_KP_INFO = "- The calculated capacity requirements can not be changed. " +
            "You can customize for each workplace the overtime per period " + "/n and the number of shifts." +
                "A change in Overtime/Period causes a recalculation of Overtime/Day." +
                "\n- If a red line appears in the 3. shift, " +
                "this means that more than three shifts are required.\n   In this case, " +
                " you should adjust their production amounts.\n- The workplace 5 does not exist.";

        /*---------Produktion----------*/
        //Button
        public static String EN_BTN_ETEILE = "E-Parts";

        //Groupbox
        public static String EN_PR_GROUPBOX1 = "Production planning";

        //ToolTip
        public static String EN_PR_INFO = "Here you can overwrite the calculated production values. \nTo check the production of the E-Parts, click on the button 'E-Parts'.                     ";
        /*---------Produktion E-Teile----------*/
        public static String EN_PRE_GB_ETEILE = "Production of own products";


        /*---------Sicherheitsbestand----------*/
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
        public static String EN_INFOP = "Please enter the Safty-Stock for the P-Items.                                       ";
        public static String EN_INFOE = "- These fields of safety stocks for the E-Items is not maintainable before calculation of the P-Items.\n " + " - The result showing the security holdings of the E-Items is calculated by the system, but can be changed manually as needed.                \n " + " - to continue click on 'Continue' button.";
        
        /*---------Direktverkäufe----------*/
        //Groupbox
        public static String EN_DV_GROUPBOX1 = "Direct sales";

        //Labels
        public static String EN_DV_LABEL4 = "Amount";
        public static String EN_DV_LABEL5 = "Price";
        public static String EN_DV_LABEL6 = "Penalty";
        
        /*---------Ergebnis----------*/
        //Buttons
        public static String EN_BTN_XML_EXPORT = "XML export";

        //Groupbox
        public static String EN_ER_GROUPBOX2 = "Purchase orders";
        public static String EN_ER_GROUPBOX3 = "Production orders";
        public static String EN_ER_GROUPBOX4 = "Production capacity";
    }
}
