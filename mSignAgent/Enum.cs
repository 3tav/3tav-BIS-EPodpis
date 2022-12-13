﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mSignAgent
{
    /*
    public enum Status { Neobdelan = 0, Napaka = -1, Uspeh = 1, VObdelavi = 2, Zadrzana = 10, Evidencno = 100 }
    public enum TipZahteve { NovaPoD = 0, PrviPriklop = 1, RednaMenjava = 3, OdpovedOdjemalca = 4 }
    */

    public static class Methods
    {
        public const string PodatkiInit = "INIT";
        /* methods on PDF templates*/
        public const string CreatePDF = "PDF-CREATE";
        public const string MergePDF = "PDF-MERGE";
        public const string ImportPDF = "PDF-IMPORT";

        /* Msign API */
        public const string MsignSign = "MSIGN-SIGN";
        public const string MsignSignFile = "MSIGN-SIGN-FILE";
        public const string MsignStatusRefresh = "MSIGN-STATUS-REFRESH";
        public const string MsignGetDocument = "MSIGN-GET-DOCUMENT";
        public const string MsignStatusRefreshPogodba = "MSIGN-STATUS-REFRESH-POGODBA"; // za posamezno pogodbo
        public const string MsignStatusRefreshPogodbe = "MSIGN-STATUS-REFRESH-POGODBE"; // masovno
        public const string MsignVPregled = "MSIGN-VPREGLED"; // pošlji v pregled
        
        /* Email */
        public const string EmailSendSign = "EMAIL-SEND-SIGN";
        public const string EmailSendSigned = "EMAIL-SEND-SIGNED";

        /* SignPro */
        public const string SignProSignFile = "SIGNPRO-SIGN-FILE";
        public const string SignProImportSigned = "SIGNPRO-IMPORT-SIGNED";

        /* Database */
        public const string ExecuteSP = "EXECUTE-SP";

        /* odpri GDPR obrazec */
        public const string GdprOpen = "GDPR-OPEN";

        /* odpri splošen URL */
        public const string URLOpen = "URL-OPEN";
        
    }

    public static class Settings
    {
        public const string ConnectionString = "connString";
        public const string ConnectionStringDOC = "connStringDoc";
        
    }

    public static class Tables
    {
        public const string BisPogodbeGl= "bis_pogodbe_gl";
        public const string WebPovprasevanja = "web_PovprasevanjePrenos";

    }

    public static class Podjetja
    {
        public const string ECE = "ECE";
        public const string PLMB = "PM";

    }

}
