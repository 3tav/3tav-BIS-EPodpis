using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Mail;
using System.Web;
using System.Configuration;
using System.Net;
using System.Net.Mime;
using System.Security;
using System.Security.Cryptography.X509Certificates;


namespace mSignAgent
{
    public partial class Form2 : Form
    {
        private DispatchService _ds;
        private DbPovprasevanja _dal;

        private string _parm;
        

        public Form2()
        {
            InitializeComponent();
            InitService();
            InitGUI();
        }

        public void InitService()
        {
            InitService(string.Empty);
        }

        public void InitService(string method)
        {
            _ds = new DispatchService();
            _ds.Init();

            _dal = new DbPovprasevanja();
             
        }

        public void InitService(string method, string parm)
        {
            _ds = new DispatchService();
            _ds.Init();
            _parm = parm;           
        }

        public void Init(string method, string parm)
        {
            InitService(method, parm);            
        }

        public void InitGUI()
        {
            var obrazci = _dal.GetVrsteObrazcev();
            inputVrstaObrazca.DisplayMember = "SifraObrazca";
            inputVrstaObrazca.ValueMember = "SifraObrazca";
            inputVrstaObrazca.DataSource = obrazci;
            
        }

        public void DispatchMethod(string method, string arg)
        {
            var result = 0;
            var message = string.Empty;

            var id = -1;
            var userId = -1;
            var remote = true;
            var flatten = false;
            var filePath = string.Empty;
            var telefon = string.Empty;
            var email = string.Empty;
            var url = string.Empty;
            var datoteke = string.Empty;

            string[] args;

            

            if (_ds.Podjetje == Podjetja.PLMB) {
                _ds.DispatchMethod(method, arg);
                return;
            }

            // TODO premakni v method dispatcher
            var svc = new mSignAgentLib();

            try
            {
                /****************************************************
                *   PDF OPERATIONS
                ****************************************************/
                if (method == Methods.CreatePDF)
                {
                    args = arg.Split(';');

                    try
                    {
                        id = Convert.ToInt32(args[0]);
                        flatten = Convert.ToBoolean(args[1]);
                        userId = Convert.ToInt32(args[2]);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [10]", ex.Message));
                    }


                    svc.PreviewMerged(id, flatten);
                    result = 1;
                }

                if (method == Methods.MergePDF)
                {
                    args = arg.Split(';');

                    try
                    {
                        id = Convert.ToInt32(args[0]);
                        datoteke = args[1];
                        userId = Convert.ToInt32(args[2]);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [10]", ex.Message));
                    }


                    svc.Merge(id, datoteke, userId);
                    result = 1;
                }

                if (method == Methods.ImportPDF)
                {
                    args = arg.Split(';');

                    try
                    {
                        id = Convert.ToInt32(args[0]);
                        datoteke = args[1];
                        userId = Convert.ToInt32(args[2]);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [10]", ex.Message));
                    }


                    svc.Import(id, datoteke, userId);
                    result = 1;
                }

                /****************************************************
                *   MSIGN METHODS
                ****************************************************/
                if (method == Methods.MsignSign)
                {
                    args = arg.Split(';');

                    try
                    {
                        id = Convert.ToInt32(args[0]);
                        remote = Convert.ToBoolean(args[1]);
                        userId = Convert.ToInt32(args[2]);

                        telefon = args[3];
                        email = args[4];


                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [20]", ex.Message));
                    }

                    svc.PogodbaPodpisi(id, remote, userId, telefon, email);
                    result = 1;
                }

                
                if (method == Methods.MsignSignFile)
                {
                    // pošlji pogodbo v podpis (statični PDF, brez generiranja)
                    args = arg.Split(';');

                    try
                    {
                        id = Convert.ToInt32(args[0]);
                        remote = Convert.ToBoolean(args[1]);
                        userId = Convert.ToInt32(args[2]);
                        filePath = args[3];
                        telefon = args[4];
                        email = args[5];
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [30]", ex.Message));
                    }

                    svc.PogodbaPodpisi(id, remote, userId, filePath, telefon, email);
                    result = 1;
                }


                if (method == Methods.MsignStatusRefreshPogodba)
                {
                    // osveževanje statusa za posamezno
                    args = arg.Split(';');

                    try
                    {
                        id = Convert.ToInt32(args[0]);
                        userId = Convert.ToInt32(args[1]);


                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [40]", ex.Message));
                    }

                    svc.RefreshMsignStatusPogodbe(id, userId);
                    result = 1;
                }

                if (method == Methods.MsignStatusRefreshPogodbe)
                {
                   
                    // osveževanje statusa masovno                                       
                    try
                    {
                        userId = -99; // zaganja sistem
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [40]", ex.Message));
                    }

                    svc.RefreshMsignStatusPogodbe(userId);
                    result = 1;
                }

                /****************************************************
                *   SIGNPRO METHODS
                ****************************************************/
                if (method == Methods.SignProSignFile)
                {
                    args = arg.Split(';');

                    try
                    {
                        id = Convert.ToInt32(args[0]);
                        userId = Convert.ToInt32(args[1]);
                        filePath = args[2];
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [50]", ex.Message));
                    }

                    svc.SignProSignFile(id, userId, filePath);
                    result = 1;
                }

                if (method == Methods.SignProImportSigned)
                {
                    args = arg.Split(';');

                    try
                    {
                        id = Convert.ToInt32(args[0]);
                        userId = Convert.ToInt32(args[1]);
                        filePath = args[2];

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [40]", ex.Message));
                    }

                    svc.SignProImportSigned(id, userId, filePath);
                    result = 1;
                }

                /****************************************************
                *   MISC / HELPER METHODS
                ****************************************************/
                if (method == Methods.GdprOpen)
                {
                    args = arg.Split(';');

                    try
                    {
                        id = Convert.ToInt32(args[0]);
                        userId = Convert.ToInt32(args[1]);
                        //filePath = args[2];

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [40]", ex.Message));
                    }

                    
                    svc.GdprOpen(id, userId);
                    result = 1;
                }

                if (method == Methods.URLOpen)
                {
                    args = arg.Split(';');

                    try
                    {
                        url = args[0];
                        userId = Convert.ToInt32(args[1]);                        

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [40]", ex.Message));
                    }


                    svc.GdprOpen(id, userId);
                    result = 1;
                }

                if (method == Methods.EmailSendSigned)
                {
                    args = arg.Split(';');

                    try
                    {
                        id = Convert.ToInt32(args[0]);                        
                        email = args[1];
                        userId = Convert.ToInt32(args[2]);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Argument exception: {0} [40]", ex.Message));
                    }

                    
                    svc.PosljiMailPodpisano(id, email);
                    result = 1;
                }

                
             
            }
            catch (Exception ex)
            {
                result = -1;
                message = ex.Message;
            }

            try
            {
                // job avtomatike ne logiraj če uspeh
                if (method == Methods.MsignStatusRefreshPogodbe && result == 1)
                    return;

                svc.Log(method, arg, userId, id, result, message);
            }
            catch (Exception ex)
            {
                // log silent fail?
            }
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sifraObrazca = inputVrstaObrazca.SelectedValue.ToString();

            var obrazec = _dal.GetObrazecInfo(sifraObrazca);


            inputTemplate.Text = obrazec.Rows[0]["DokumentPdf"].ToString();
            inputProcedura.Text = obrazec.Rows[0]["StoredProcedura"].ToString();
            inputFileName.Text = obrazec.Rows[0]["IzhodniPdf"].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id = -1;
            int.TryParse(inputID.Text.Trim(), out id);
            if (id == -1)
                return;


            var svc = new mSignAgentLib();
            try
            {
                svc.Init();
                svc.RefreshMsignStatusPogodbe(id, -99);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnGenerirajPDF_Click(object sender, EventArgs e)
        {
            var svc = new mSignAgentLib();
            var sifraObrazca = inputVrstaObrazca.SelectedValue.ToString();
            int id = -1;
            int.TryParse(inputID.Text.Trim(), out id);
            if (id == -1)
                return;

            var dir = @"c:\\temp\\";

            try
            {                
                svc.CreatePDF(sifraObrazca, 1, id, inputFileName.Text, dir);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
           // 
        }

        public void GenerirajPDF(int stevilkaPogodbe) {
            //var svc = new mSignAgentLib();
            //svc.CreatePDF(sifraObrazca, 1, id, inputFileName.Text, dir);

            // dobi obrazce za pogodbo

            // generiraj vse

            // merge 

            // prikaži
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            int id = -1;
            int.TryParse(inputID.Text.Trim(), out id);
            if (id == -1)
                return;

            var svc = new mSignAgentLib();
            svc.PreviewMerged(id);
        }

        private void btnPosljiVPodpis_Click(object sender, EventArgs e)
        {
            int id = -1;
            int.TryParse(inputID.Text.Trim(), out id);
            if (id == -1)
                return;

            var svc = new mSignAgentLibPLMB();
            try
            {
                svc.Init();
                svc.PogodbaPodpisi(id, true, -99, "+38631402559", "simon.rusjan@gmail.com");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //svc.PogodbaPodpisi(id, true, 16674);
        }

        private void btnCMD_Click(object sender, EventArgs e)
        {
            var cmd = inputCMD.Text;
            cmd = cmd.Replace("\"", "");
            
            try
            {
                var args = cmd.Split(' ');

                InitService();
                DispatchMethod(args[0], args[1]);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";
            dialog.FilterIndex = 1;
            dialog.Multiselect = false;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            inputFilePath.Text = dialog.FileName;

        }

        private void btnPosljiVPodpisFile_Click(object sender, EventArgs e)
        {
            int id = -1;
            int.TryParse(inputID.Text.Trim(), out id);
            if (id == -1)
                return;

            var svc = new mSignAgentLib();
            svc.PogodbaPodpisi(id, true, -99, inputFilePath.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id = -1;
            int.TryParse(inputID.Text.Trim(), out id);
            if (id == -1)
                return;

            var svc = new mSignAgentLib();
            svc.PogodbaPodpisi(id, true, -99, inputFilePath.Text);
        }

        private void btnTestEmail1_Click(object sender1, EventArgs e)
        {

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress("simon.rusjan@gmail.com", "Simon Rusjan"));
            msg.From = new MailAddress("e-podpis@plinarna-maribor.si", "E podpis PLMB");
            msg.Subject = "EPodpis Test Mail";
            msg.Body = "Test mail";
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("epodpis", "Zim@2022");
            client.Port = 25; // You can use Port 25 if 587 is blocked (mine is!)
            client.Host = "10.108.12.53";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.EnableSsl = true;

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            try
            {
                client.Send(msg);
                 
            }
            catch (Exception ex)
            {
                //lblText.Text = ex.ToString();
                MessageBox.Show(ex.Message);
            }
            
            /*
            int id = -1;
            int.TryParse(inputID.Text.Trim(), out id);
            if (id == -1)
                return;

            try
            {
                var svc = new mSignAgentLib();
                svc.Init();
                svc.MailVPodpisTest(id, true);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }
             */ 
        }

        private void btnSignPro_Click(object sender, EventArgs e)
        {
            var svc = new WacomSignProLib.SignProService();

            svc.Sign(inputFilePath.Text);

        }

        private void btnTestEmail2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                var svc = new mSignAgentLib();
                svc.RefreshMsignStatusCustom();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
         
            
        }

       
    }
}
