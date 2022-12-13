using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace mSignAgent
{
    public class PDFService
    {

        private string _connStringId;
        private string _connString;
        public string ConnString { get { return _connString; } }
        public string ConnStringId { get { return _connStringId; } }

        private List<string> _sqlCommands = new List<string>();

        public void Init(string connStringId)
        {
            _connStringId = connStringId;
            _connString = ConfigurationManager.ConnectionStrings[connStringId].ToString();
        }

        public byte[] CreatePDF(string sifraObrazca, int varianta, int id, string fileName)
        {

            byte[] pdfObrazec = null;

            PdfFormBuilder pdfForm = new PdfFormBuilder();
            pdfForm.Debug = false;

            var seznamObrazcev = DobiNastavitveZaObrazec(_connString, sifraObrazca, varianta, id, 1, fileName, true);
            //var seznamObrazcev = DobiNastavitveZaObrazec(_connString, "EN-DIGITAL", 0, id, 1, fileName, true);
            if (seznamObrazcev.Count == 0)
                throw new Exception("Ni nastavitev za obrazec!");

            foreach (ObrazecPdf obrazec in seznamObrazcev)
            {
                var outputPdfStream = new MemoryStream();
                pdfForm.Export(obrazec.PotPDF, obrazec.XmlNastavitev, out outputPdfStream, true, false);
                pdfObrazec = outputPdfStream.ToArray();
            }

            return pdfObrazec;
        }

        private void NapolniPdfPogodbe(int varianta, int sifraObdelave, int leadID, string oznaka)
        {
            //pripravimo PDF
            PdfFormBuilder pdfForm = new PdfFormBuilder();
            pdfForm.Debug = false;

            var extension = ".pdf";
            var fname = string.Format("{0}{1}", oznaka, extension);
            //List<ObrazecPdf> seznamObrazcev = Helpers.DobiNastavitveZaObrazec(_connString, "RWE-SI-DB-TS", varianta, leadID, (int)VrstaEnergenta.NiDolocen, true);
            List<ObrazecPdf> seznamObrazcev = DobiNastavitveZaObrazec(_connString, "RWE-SI-DB-TS", varianta, leadID, 1, fname, true);

            //// če izbrani big bang artikli -> dodaj big bang obrazec
            //if (VsebujeBigBangPostavke())
            //{
            //    seznamObrazcev.AddRange(Helpers.DobiNastavitveZaObrazec(_connString, "RWE-SI-EA", 50, sifraObdelave, true));
            //    rowEmso.Visible = true;
            //}





            foreach (ObrazecPdf obrazec in seznamObrazcev)
            {
                MemoryStream outputPdfStream = new MemoryStream();

                pdfForm.Export(obrazec.PotPDF, obrazec.XmlNastavitev, out outputPdfStream, true, false);

                obrazec.outputPdfStream = outputPdfStream;


                //                                
                var opis = string.Empty;
                long vrsteObrazcev = 0;
                int tip = 22;
                var contentType = "application/pdf";

                var fpath = fname;

                // shrani v bazo
                //var fs = outputPdfStream;
                //var data = new Byte[fs.Length];
                //fs.Read(data, 0, (int)fs.Length);
                //fs.Close();
                byte[] data = outputPdfStream.ToArray();

                //int docId = -1;

                //try
                //{
                //    docId = dbp.InsertDoc(fname, fpath, extension, data, userId, contentType);
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(string.Format("{0} [{1}]", ex.Message, 10));
                //}

                //try
                //{
                //    dbp.InsertDocLink(sifraObdelave, docId, fname, opis, oznaka, (int)tip, vrsteObrazcev, userId);
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(string.Format("{0} [{1}]", ex.Message, 20));
                //}
            }


        }

        public List<ObrazecPdf> DobiNastavitveZaObrazec(string _connString, string SifraObrazca, int Varinata, int Sifra, int Energenti, string nazivPDF, bool generateXML)
        {
            List<ObrazecPdf> result = new List<ObrazecPdf>();

            string xmlNastavitev = null;
            string StoredProcedura = null;
            SqlConnection myConnection = new SqlConnection(_connString);
            string ukaz1 = "SELECT ID, SifraObrazca, Varianta, DokumentPdf, StoredProcedura, IzhodniPdf, isnull(Energent, -1) as Energent  FROM web_ObrazciPdf where SifraObrazca = @SifraObrazca and Varianta = @Varianta order by vrstni_red";
            SqlDataAdapter myCommand = new SqlDataAdapter(ukaz1, myConnection);
            myCommand.SelectCommand.Parameters.Add("@SifraObrazca", SqlDbType.VarChar, 512).Value = SifraObrazca;
            myCommand.SelectCommand.Parameters.Add("@Varianta", SqlDbType.Int, 4).Value = Varinata;

            DataSet ds = new DataSet();
            myCommand.Fill(ds);
            foreach (DataTable table in ds.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    StoredProcedura = row["StoredProcedura"].ToString();
                    int vrstaEnergenta = Convert.ToInt32(row["Energent"]);

                    if (generateXML)
                        xmlNastavitev = DobiNastavitveXmlZaObrazec(_connString, StoredProcedura, Sifra);

                    //if (CheckVrstaEnergenta(Energenti, vrstaEnergenta) || vrstaEnergenta == -1)
                    result.Add(new ObrazecPdf() { IzbranaVrstaStoritve = int.Parse(row["Varianta"].ToString()), PotPDF = row["DokumentPdf"].ToString(), NazivPDF = nazivPDF, XmlNastavitev = xmlNastavitev, LeadId = Sifra });
                }
            }
            return result;
        }

        public string DobiNastavitveXmlZaObrazec(string _connString, string storedProcedure, int Sifra)
        {
            string result = null;
            try
            {
                using (var con = new SqlConnection(_connString))
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = storedProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int, 4).Value = Sifra;

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result = reader.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public List<ObrazecPdf> DobiNastavitveZaObrazec(string _connString, string SifraObrazca, int varianta, int Sifra, bool generateXML)
        {
            List<ObrazecPdf> result = new List<ObrazecPdf>();

            string xmlNastavitev = null;
            string StoredProcedura = null;
            SqlConnection myConnection = new SqlConnection(_connString);
            string ukaz1 = "SELECT ID, SifraObrazca, Varianta, DokumentPdf, StoredProcedura, IzhodniPdf FROM web_ObrazciPdf where SifraObrazca = @SifraObrazca and Varianta = @Varianta";
            SqlDataAdapter myCommand = new SqlDataAdapter(ukaz1, myConnection);
            myCommand.SelectCommand.Parameters.Add("@SifraObrazca", SqlDbType.VarChar, 512).Value = SifraObrazca;



            var variantaTemp = (varianta == 3 ? varianta : 1);

            myCommand.SelectCommand.Parameters.Add("@Varianta", SqlDbType.Int, 4).Value = variantaTemp; // Fiksirano Simon 8.10.2020

            DataSet ds = new DataSet();
            myCommand.Fill(ds);
            foreach (DataTable table in ds.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    StoredProcedura = row["StoredProcedura"].ToString();

                    if (generateXML)
                        xmlNastavitev = DobiNastavitveXmlZaObrazec(_connString, StoredProcedura, Sifra);

                    // če dokument vezan na id akcije: dodaj v template
                    var potPDF = row["DokumentPdf"].ToString();
                    if (varianta > 1000)
                    { // za posamezne pakete el.en.
                        potPDF = potPDF.Replace(".pdf", string.Format("_{0}.pdf", varianta));
                    }

                    result.Add(new ObrazecPdf() { IzbranaVrstaStoritve = int.Parse(row["Varianta"].ToString()), PotPDF = potPDF, NazivPDF = row["IzhodniPdf"].ToString(), XmlNastavitev = xmlNastavitev, LeadId = Sifra });
                }
            }
            return result;
        }




    }

    public class ObrazecPdf
    {
        public MemoryStream outputPdfStream { get; set; }
        public string PotPDF { get; set; }
        public string NazivPDF { get; set; }
        public string XmlNastavitev { get; set; }
        public int IzbranaVrstaStoritve { get; set; }
        public int LeadId { get; set; }
        public int Varianta { get; set; }

        public void Export()
        {
            try
            {
                //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                //outputPdfStream.Position = 0;
                //response.ClearContent();
                //response.ClearHeaders();
                //response.Buffer = true;
                //response.ContentType = "application/pdf";
                //response.AddHeader("Content-Length", outputPdfStream.Length.ToString());
                //response.AddHeader("Content-Disposition",
                //    "attachment; filename=" + NazivPDF + "; size=" + outputPdfStream.Length.ToString());
                //response.OutputStream.Write(outputPdfStream.GetBuffer(), 0, outputPdfStream.GetBuffer().Length);
                //response.Flush();
                //response.End();

            }
            catch (Exception ex)
            {
                // 
            }
        }
    }
}
