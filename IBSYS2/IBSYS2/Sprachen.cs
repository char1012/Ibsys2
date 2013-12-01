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
        public static String DE_LBL_SICHERHEITSBESTAND = "Sicherheitsbe.";
        public static String DE_LBL_AUFTRAEGE = "Aufträge"; //Umlaut
        public static String DE_LBL_KAPATITAETSPLAN = "Kapazitätsplan"; //Umlaut
        public static String DE_LBL_KAUFTEILEDISPOSITION = "Kaufteildisposition";
        public static String DE_LBL_ERGEBNIS = "Ergebnis";

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
        public static String EN_LBL_AUFTRAEGE = "Order";
        public static String EN_LBL_KAPATITAETSPLAN = "Capacity Plan";
        public static String EN_LBL_KAUFTEILEDISPOSITION = "Purchased parts dispostion";
        public static String EN_LBL_ERGEBNIS = "Result";

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
