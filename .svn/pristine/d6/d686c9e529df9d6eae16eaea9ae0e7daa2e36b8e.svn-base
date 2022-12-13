using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using mSignLib;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace mSignTest
{
    public partial class Form1 : Form
    {
        private mSignService _svc;
        private mSignDAL _dal;
        public Form1()
        {
            InitializeComponent();
            try
            {
                _svc = new mSignService();
                _svc.Init();

                _dal = new mSignDAL();
                
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ret = string.Empty;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
               //ret = _svc.TestKlic();
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            Cursor.Current = Cursors.Default;

            textBox1.Text = ret;
        }

        private void btnGetDocument_Click(object sender, EventArgs e)
        {
            var status = -1;
            int.TryParse(inputStatus.Text, out status);
            Cursor.Current = Cursors.WaitCursor;
            if (status > 0) {
                GetDocuments(status); 
                return;
            }
            
            GetDocuments();
            Cursor.Current = Cursors.Default;
             
        }

        private void GetDocuments() {
            var ret = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                ret = _svc.GetDocuments();
                var r = JsonConvert.DeserializeObject<List<GetDocumentsDTO>>(ret);
                dataGridView1.DataSource = r;
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message;
                return;
            }
 
           
             textBox1.Text = JsonHelper.FormatJson(ret);
             Cursor.Current = Cursors.Default;
            
        }

        private void GetDocuments(int status) {
            var ret = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                ret = _svc.GetDocuments(status);
                var r = JsonConvert.DeserializeObject<List<GetDocumentsDTO>>(ret);
                dataGridView1.DataSource = r;
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }

             textBox1.Text = JsonHelper.FormatJson(ret);
             Cursor.Current = Cursors.Default;
        }


        private void GetDocument(int id)
        {
            var ret = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                ret = _svc.GetDocument(id);
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            Cursor.Current = Cursors.Default;
            textBox1.Text = JsonHelper.FormatJson(ret);
        }

        private void CreateDocument()
        {
            var ret = string.Empty;

            var filePath = inputFilePath.Text;
            var fileName = System.IO.Path.GetFileName(filePath);


            var bytes = File.ReadAllBytes(filePath);
            var fileBase64 = Convert.ToBase64String(bytes);


            var doc = new CreateDocumentDTO();
            doc.title = "plmb-test";            
            doc.status = (int)mSignDocumentStatus.ForSigning;
            doc.attachments = new List<Attachment>();

            var att = new Attachment() { fileName = fileName, content = fileBase64 };
            att.signatureTags = new List<SignatureTag>();
            att.signatureTags.Add(new SignatureTag() { tag = "{SIGNATURE1}", metadataValue = "test" });

            doc.attachments.Add(att);

            

            try
            {
                ret = _svc.CreateDocument(doc);
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                return;
            }

            textBox1.Text = JsonHelper.FormatJson(ret);

            try
            {
                inputDocID.Text = _svc.GetCreateDocumentId(ret).ToString();
            }
            catch (Exception ex)
            {
                textBox1.AppendText(string.Format("Parse error: {0}", ex.Message));            
            }                                                        
 
        }

        private void CreateSharedDocument()
        {
            var ret = string.Empty;

             

            var doc = new CreateSharedDocumentDTO();
            doc.signatureTagName = "{SIGNATURE1}";
            doc.documentId = Convert.ToInt32(inputDocID.Text);
            doc.email = inputEmail.Text;
            doc.mobile_number = inputGSM.Text;
             
            try
            {
                ret = _svc.CreateSharedDocument(doc);
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }

            textBox1.Text = JsonHelper.FormatJson(ret);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            var id = -1;
            int.TryParse(inputStatus.Text, out id);

            if (id > -1)
            {
                GetDocument(id);
                return;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var id = -1;
            int.TryParse(inputStatus.Text, out id);

            if (id > -1)
            {
                CreateDocument();
                return;
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            var id = -1;
            int.TryParse(inputDocID.Text, out id);

            if (id > -1)
            {
                CreateSharedDocument();
                return;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var data = (JObject)JsonConvert.DeserializeObject(textBox1.Text);
            var id = data["id"].Value<string>();
            //var id = data["documentId"].Value<string>();
            textBox1.Text = id;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                var data = (JObject)JsonConvert.DeserializeObject(textBox1.Text);

                var content = (string)data["attachments"][0]["content"];

                File.WriteAllBytes(@"C:\\temp\mSign\\signed.pdf", Convert.FromBase64String(content));

            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message;
            }
            // rabim za DOC bazo
            //byte[] bytes = System.Convert.FromBase64String(content);
        }

        private void btnPosljiVPodpis_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            /**********************************************************
            *   Branje inputa
            ************************************************************/
            var ret = string.Empty;
            var documentId = -1;

            var filePath = inputFilePath.Text;
            var fileName = System.IO.Path.GetFileName(filePath);


            var bytes = File.ReadAllBytes(filePath);
            var fileBase64 = Convert.ToBase64String(bytes);


            var doc = new CreateDocumentDTO();
            doc.title = "plmb-test";
            doc.status = (int)mSignDocumentStatus.ForSigning;
            doc.attachments = new List<Attachment>();

            var att = new Attachment() { fileName = fileName, content = fileBase64 };
            att.signatureTags = new List<SignatureTag>();
            att.signatureTags.Add(new SignatureTag() { tag = "{SIGNATURE1}", metadataValue = "test" });

            doc.attachments.Add(att);


            /**********************************************************
            *   Kreiraj zahtevo za podpis
            ************************************************************/
            _dal.CreateZahteva(new mSignZahteva { Opis = doc.title, FilePath = filePath, Datum = DateTime.Now, Veza = "test-povprasevanje", Kljuc = -1 });


            /**********************************************************
            *   Kreiraj dokument    
            ************************************************************/
            try
            {
                ret = _svc.CreateDocument(doc);
                documentId = _svc.GetCreateDocumentId(ret);
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                textBox1.Text = ret;
                return;
            }

            

            inputDocID.Text = documentId.ToString();


            /**********************************************************
            *   Kreiraj shared dokument    
            ************************************************************/
            var sharedDoc = new CreateSharedDocumentDTO();
            sharedDoc.signatureTagName = "{SIGNATURE1}";
            sharedDoc.documentId = Convert.ToInt32(inputDocID.Text);
            sharedDoc.email = inputEmail.Text;
            sharedDoc.mobile_number = inputGSM.Text;

            try
            {
                ret = _svc.CreateSharedDocument(sharedDoc);
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                textBox1.Text = ret;
                return;
            }
            Cursor.Current = Cursors.Default;
            textBox1.Text = string.Format("Poslano v podpis ID {0}", documentId);



            //var filename = string.Format("{0}{1}.xml", _fileDumpPath, string.Format("{0}_{1}", method, DateTime.Now.ToString("yyyyMMddHHmmssfff")));
        }

        private void btnGetSignedPDF_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var id = Convert.ToInt32(inputDocID.Text);
            var ret = string.Empty;

            try
            {
                ret = _svc.GetDocument(id);
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                textBox1.Text = ret;
                return;
            }

            var filename = string.Format("signed_{0}_{1}.pdf", id, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

            var data = (JObject)JsonConvert.DeserializeObject(ret);

            var content = (string)data["attachments"][0]["content"];
            var filepath = string.Format(@"C:\\temp\mSign\\{0}", filename);
            File.WriteAllBytes(filepath, Convert.FromBase64String(content));
            Cursor.Current = Cursors.Default;
            System.Diagnostics.Process.Start(filepath);
        }

        private void btnZahteve_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _dal.GetZahteve();
        }

        private void btnNovaZahteva_Click(object sender, EventArgs e)
        {
            _dal.CreateZahteva(new mSignZahteva { Opis = "eon-test", FilePath = inputFilePath.Text, Datum = DateTime.Now, Veza = "test-povprasevanje", Kljuc = -1 });
            dataGridView1.DataSource = _dal.GetZahteve();
        }

        private void btnZahtevaStorno_Click(object sender, EventArgs e)
        {

        }

        private void btnAddTag_Click(object sender, EventArgs e)
        {
            var inputFile = "C:\\temp\\plmb\\plmb_pogodba.pdf";
            var outputFile = "C:\\temp\\plmb\\plmb_pogodba_field.pdf";

            var fieldName = "{SIGNATURE1}";

            var x = Convert.ToInt32(inputRectX.Text);
            var y = Convert.ToInt32(inputRectY.Text);
            var ux = Convert.ToInt32(inputRectUX.Text);
            var uy = Convert.ToInt32(inputRectUY.Text);

            using (PdfStamper stamper = new PdfStamper(new PdfReader(inputFile), File.Create(outputFile)))
            {
                var r = new iTextSharp.text.Rectangle(x, y, ux, uy);

                Document doc = new Document(r);

                
                var c = new Chunk(fieldName);
                //stamper.AddAnnotation(c.get, 2);

                stamper.Close();
            }


           

            


            System.Diagnostics.Process.Start(outputFile);

        }


        /*
           var inputFile = "C:\\temp\\plmb\\plmb_pogodba.pdf";
            var outputFile = "C:\\temp\\plmb\\plmb_pogodba_field.pdf";

            var fieldName = "{SIGNATURE1}";

            var x = Convert.ToInt32(inputRectX.Text);
            var y = Convert.ToInt32(inputRectY.Text);
            var ux = Convert.ToInt32(inputRectUX.Text);
            var uy = Convert.ToInt32(inputRectUY.Text);

            using (PdfStamper stamper = new PdfStamper(new PdfReader(inputFile), File.Create(outputFile)))
            {
                TextField tf = new TextField(stamper.Writer, new iTextSharp.text.Rectangle(x, y, ux, uy), fieldName);

                //iTextSharp.text.pdf.PdfSignatu


                tf.Text = fieldName;
                tf.Options = TextField.READ_ONLY; 
               // tf.Visibility = 1;

                stamper.AddAnnotation(tf.GetTextField(), 2);
                stamper.Close();
            }

            System.Diagnostics.Process.Start(outputFile);
         */
        
    }
}
