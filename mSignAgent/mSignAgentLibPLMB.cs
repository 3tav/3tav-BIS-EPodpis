using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using mSignLib;
using System.Configuration;
using WacomSignProLib;



namespace mSignAgent
{
    public class mSignAgentLibPLMB : ImsignAgentLib
    {
        private string _basePath;
        private string _fileNameMerged;
        private string _emailFrom;
        private string _emailSubject;
        private string _emailSubjectPodpisano;
        private string _emailSubjectPregled;
        private string _msignDocumentName;

        private DbPovprasevanja _dal;

        public void Init() {
            _basePath = ConfigurationManager.AppSettings["BasePath"].ToString();
            _fileNameMerged = ConfigurationManager.AppSettings["FileNameMerged"].ToString();
            _emailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
            _emailSubject = ConfigurationManager.AppSettings["MailSubject"].ToString();
            _emailSubjectPodpisano = ConfigurationManager.AppSettings["MailSubjectPodpisano"].ToString();
            _emailSubjectPregled = ConfigurationManager.AppSettings["MailSubjectPregled"].ToString();
            _msignDocumentName = ConfigurationManager.AppSettings["MsignDocumentName"].ToString();
            _dal = new DbPovprasevanja();
        }

        public  void RefreshMsignStatus()
        {
           
            try
            {
                var msignService = new mSignService();
                var dal = new DbPovprasevanja();
                var msignDAL = new mSignDAL();
                //int sifraObdelave = Convert.ToInt32(Session[SessionKeys.SifraObdelave].ToString());

                msignService.Init();


                var pendingDocs = msignDAL.GetPendingDocs();
                if (pendingDocs.Count == 0)
                    return;

                var signedDocs = msignService.GetDocumentsDTO(msignService.GetDocuments(2));
                

                foreach (var p in pendingDocs)
                {
                    if (signedDocs.Exists(s => s.id == p.mSignId))
                    {
                        msignDAL.UpdateMSignStatus(p.Id, (int)Status.Podpisan, "Signed");
                        var sifraObdelave = p.Kljuc;

                        // prenesi dokumente
                        PrenesiPodpisane(sifraObdelave);

                         
                        try
                        {

                            // merge                
                            var filesFilter = string.Format("{0}_*.pdf", sifraObdelave);
                            var targetDir = String.Format("{0}Obrazci\\temp\\",_basePath);
                            var di = new DirectoryInfo(targetDir);
                            var files = new List<string>();
                            var attachments = new List<System.Net.Mail.Attachment>();
                            // združi vse PDF
                          

                            var dt = _dal.GetDocSeznamPovprasevanje(sifraObdelave);


                            int zap = 1;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                var docId = Convert.ToInt32(dt.Rows[i]["doc_id"].ToString());
                                var doc = _dal.GetDatoteka(docId);

                                var filePath = string.Format("{0}Obrazci\\temp\\{1}_{2}_{3}.pdf", _basePath, sifraObdelave, zap.ToString().PadLeft(2, '0'),  Environment.TickCount);
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }

                                File.WriteAllBytes(filePath, doc.Datoteka);
                                files.Add(filePath);
                                attachments.Add(new System.Net.Mail.Attachment(filePath));
                                zap++;
                            }

                            var k = dal.GetPovprasevanjeKontakt(sifraObdelave);
                            var paketEnergija = Convert.ToInt32(k.Rows[0]["PaketEnergija"]);

                            var filePathPogoji = string.Empty;
                            var filePathCenik = string.Empty;
                            var filePathBon = string.Empty;
                            var templatePath = string.Empty;  
                            if (paketEnergija > 1000)
                            {
                                // mail za podpisano pogodbo
                                templatePath = string.Format("{0}Obrazci\\email\\mail_podpisano_template.html", _basePath); 

                                // dodaj splošne pogoje                
                                filePathPogoji = string.Format("{0}Obrazci\\ece_splosni_pogoji_{1}.pdf", _basePath, k.Rows[0]["PaketEnergija"].ToString());
                                if (File.Exists(filePathPogoji))
                                {
                                    attachments.Add(new System.Net.Mail.Attachment(filePathPogoji));
                                }

                                // cenik
                                filePathCenik = string.Format("{0}Obrazci\\ece_cenik_{1}.pdf", _basePath, k.Rows[0]["PaketEnergija"].ToString());
                                if (File.Exists(filePathCenik))
                                {
                                    attachments.Add(new System.Net.Mail.Attachment(filePathCenik));
                                }

                                // bon
                                filePathBon = string.Format("{0}Obrazci\\ece_shop_bon.pdf", _basePath);
                                if (File.Exists(filePathBon))
                                {
                                    attachments.Add(new System.Net.Mail.Attachment(filePathBon));
                                }

                            }
                            else
                            { 
                                // poslovni
                                templatePath = string.Format("{0}Obrazci\\email\\mail_podpisano_template_pos.html", _basePath); 

                                // dodaj splošne pogoje                
                                filePathPogoji = string.Format("{0}Obrazci\\ece_splosni_pogoji_pos.pdf", _basePath);
                                if (File.Exists(filePathPogoji))
                                {
                                    attachments.Add(new System.Net.Mail.Attachment(filePathPogoji));
                                }
                            }


                            var mailTo = k.Rows[0]["Eposta"].ToString();
                            var subject = string.Format("Pogodba št. {0} je podpisana", sifraObdelave);
                            
                            var template = File.ReadAllText(templatePath);

                            string napaka;
                            var m = new EmailSender();
                            //System.Net.Mail.Attachment attachment;
                            //attachment = new System.Net.Mail.Attachment(fileNameMerged);
                            var att = new List<MailInlineAttachment>();
                            att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}Obrazci\\email\\ece_logo.png", _basePath), Tag = "ece_logo" });
                            m.PosljiEmailMime("epogodba@ece.si", mailTo, template, subject, att, attachments, out napaka);



                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

         
        }


        public void RefreshMsignStatusCustom()
        {

            try
            {
                var msignService = new mSignService();
                var dal = new DbPovprasevanja();
                var msignDAL = new mSignDAL();
                //int sifraObdelave = Convert.ToInt32(Session[SessionKeys.SifraObdelave].ToString());

                msignService.Init();


                var pendingDocs = msignDAL.GetPendingDocsCustom();
                if (pendingDocs.Count == 0)
                    return;

                var signedDocs = msignService.GetDocumentsDTO(msignService.GetDocuments(2));


                foreach (var p in pendingDocs)
                {
                    if (signedDocs.Exists(s => s.id == p.mSignId))
                    {
                        msignDAL.UpdateMSignStatus(p.Id, (int)Status.Podpisan, "Signed");
                        var sifraObdelave = p.Kljuc;

                        // prenesi dokumente
                        PrenesiPodpisanePogodba(sifraObdelave);

                        /*
                        try
                        {

                            // merge                
                            var filesFilter = string.Format("{0}_*.pdf", sifraObdelave);
                            var targetDir = String.Format("{0}Obrazci\\temp\\", _basePath);
                            var di = new DirectoryInfo(targetDir);
                            var files = new List<string>();
                            var attachments = new List<System.Net.Mail.Attachment>();
                            // združi vse PDF


                            var dt = _dal.GetDocSeznamPovprasevanje(sifraObdelave);


                            int zap = 1;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                var docId = Convert.ToInt32(dt.Rows[i]["doc_id"].ToString());
                                var doc = _dal.GetDatoteka(docId);

                                var filePath = string.Format("{0}Obrazci\\temp\\{1}_{2}_{3}.pdf", _basePath, sifraObdelave, zap.ToString().PadLeft(2, '0'), Environment.TickCount);
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }

                                File.WriteAllBytes(filePath, doc.Datoteka);
                                files.Add(filePath);
                                attachments.Add(new System.Net.Mail.Attachment(filePath));
                                zap++;
                            }

                            var k = dal.GetPovprasevanjeKontakt(sifraObdelave);
                            var paketEnergija = Convert.ToInt32(k.Rows[0]["PaketEnergija"]);

                            var filePathPogoji = string.Empty;
                            var filePathCenik = string.Empty;
                            var filePathBon = string.Empty;
                            var templatePath = string.Empty;
                            if (paketEnergija > 1000)
                            {
                                // mail za podpisano pogodbo
                                templatePath = string.Format("{0}Obrazci\\email\\mail_podpisano_template.html", _basePath);

                                // dodaj splošne pogoje                
                                filePathPogoji = string.Format("{0}Obrazci\\ece_splosni_pogoji_{1}.pdf", _basePath, k.Rows[0]["PaketEnergija"].ToString());
                                if (File.Exists(filePathPogoji))
                                {
                                    attachments.Add(new System.Net.Mail.Attachment(filePathPogoji));
                                }

                                // cenik
                                filePathCenik = string.Format("{0}Obrazci\\ece_cenik_{1}.pdf", _basePath, k.Rows[0]["PaketEnergija"].ToString());
                                if (File.Exists(filePathCenik))
                                {
                                    attachments.Add(new System.Net.Mail.Attachment(filePathCenik));
                                }

                                // bon
                                filePathBon = string.Format("{0}Obrazci\\ece_shop_bon.pdf", _basePath);
                                if (File.Exists(filePathBon))
                                {
                                    attachments.Add(new System.Net.Mail.Attachment(filePathBon));
                                }

                            }
                            else
                            {
                                // poslovni
                                templatePath = string.Format("{0}Obrazci\\email\\mail_podpisano_template_pos.html", _basePath);

                                // dodaj splošne pogoje                
                                filePathPogoji = string.Format("{0}Obrazci\\ece_splosni_pogoji_pos.pdf", _basePath);
                                if (File.Exists(filePathPogoji))
                                {
                                    attachments.Add(new System.Net.Mail.Attachment(filePathPogoji));
                                }
                            }


                            var mailTo = k.Rows[0]["Eposta"].ToString();
                            var subject = string.Format("Pogodba št. {0} je podpisana", sifraObdelave);

                            var template = File.ReadAllText(templatePath);

                            string napaka;
                            var m = new EmailSender();
                            //System.Net.Mail.Attachment attachment;
                            //attachment = new System.Net.Mail.Attachment(fileNameMerged);
                            var att = new List<MailInlineAttachment>();
                            att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}Obrazci\\email\\ece_logo.png", _basePath), Tag = "ece_logo" });
                            m.PosljiEmailMime("epogodba@ece.si", mailTo, template, subject, att, attachments, out napaka);
                          


                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        */
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }


        public void RefreshMsignStatusPogodbe(int stevilkaPogodbe, int userId)
        {

            try
            {
                var msignService = new mSignService();
                var dal = new DbPovprasevanja();
                var msignDAL = new mSignDAL();
                //int sifraObdelave = Convert.ToInt32(Session[SessionKeys.SifraObdelave].ToString());

                msignService.Init();


                var pendingDocs = msignDAL.GetPendingDocs(Tables.BisPogodbeGl);
                if (pendingDocs.Count == 0)
                    return;

                var signedDocs = msignService.GetDocumentsDTO(msignService.GetDocuments(2));


                foreach (var p in pendingDocs)
                {
                    if (signedDocs.Exists(s => s.id == p.mSignId))
                    {
                        if (stevilkaPogodbe == p.Kljuc)
                        {
                            msignDAL.UpdateMSignStatus(p.Id, (int)Status.Podpisan, "Signed");
                            PrenesiPodpisanePogodba(stevilkaPogodbe);

                            try
                            {

                                // merge                
                                var filesFilter = string.Format("{0}_*.pdf", stevilkaPogodbe);
                                var targetDir = String.Format("{0}Obrazci\\temp\\", _basePath);
                                var di = new DirectoryInfo(targetDir);
                                var files = new List<string>();
                                var attachments = new List<System.Net.Mail.Attachment>();
                                // združi vse PDF


                                var dt = dal.GetDocSeznamPogodba(stevilkaPogodbe);


                                int zap = 1;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    var docId = Convert.ToInt32(dt.Rows[i]["doc_id"].ToString());
                                    var doc = dal.GetDatoteka(docId);

                                    var filePath = string.Format("{0}temp\\{1}_{2}_{3}.pdf", _basePath, stevilkaPogodbe, zap.ToString().PadLeft(2, '0'), Environment.TickCount);
                                    if (File.Exists(filePath))
                                    {
                                        File.Delete(filePath);
                                    }

                                    File.WriteAllBytes(filePath, doc.Datoteka);
                                    files.Add(filePath);
                                    attachments.Add(new System.Net.Mail.Attachment(filePath));
                                    zap++;
                                }

                                var k = dal.GetMsignZahtevaKontakt(p.Id);
                                //var paketEnergija = Convert.ToInt32(k.Rows[0]["PaketEnergija"]);
                                //var paketEnergija = 2000;

                                var filePathPogoji = string.Empty;
                                var filePathCenik = string.Empty;
                                var filePathBon = string.Empty;
                                var templatePath = string.Empty;

                                // dodaj splošne pogoje                
                                //filePathPogoji = string.Format("{0}ece_splosni_pogoji_{1}.pdf", _basePath, k.Rows[0]["PaketEnergija"].ToString());
                                //if (File.Exists(filePathPogoji))
                                //{
                                //    attachments.Add(new System.Net.Mail.Attachment(filePathPogoji));
                                //}

                                //// cenik
                                //filePathCenik = string.Format("{0}ece_cenik_{1}.pdf", _basePath, k.Rows[0]["PaketEnergija"].ToString());
                                //if (File.Exists(filePathCenik))
                                //{
                                //    attachments.Add(new System.Net.Mail.Attachment(filePathCenik));
                                //}


                                // mail za podpisano pogodbo
                                templatePath = string.Format("{0}email\\mail_podpisano_template.html", _basePath);

                                var mailTo = k.Rows[0]["Eposta"].ToString();
                                //var mailTo = "simon@3tav.si";
                                //var subject = string.Format("Pogodba št. {0} je podpisana", stevilkaPogodbe);
                                var subject = string.Format(_emailSubjectPodpisano, stevilkaPogodbe);

                                var template = File.ReadAllText(templatePath);

                                string napaka;
                                var m = new EmailSender();
                                //System.Net.Mail.Attachment attachment;
                                //attachment = new System.Net.Mail.Attachment(fileNameMerged);
                                var att = new List<MailInlineAttachment>();
                                //att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}\\email\\ece_logo.png", _basePath), Tag = "ece_logo" });
                                
                                var mailErrors = string.Empty;
                                try
                                {
                                    m.PosljiEmailMime(_emailFrom, mailTo, template, subject, att, attachments, out napaka);
                                }
                                catch (Exception ex)
                                {
                                    mailErrors += ex.Message;
                                }

                                // dodaten mail na prodaja
                                //mailTo = "prodaja@ece.si";
                                ////mailTo = "simon.rusjan@gmail.com";

                                //try
                                //{

                                //    var stKupca = k.Rows[0]["StKupca"].ToString();
                                //    var nazivKupca = k.Rows[0]["NazivKupca"].ToString();
                                //    var nazivSkrbnika = k.Rows[0]["NazivSkrbnika"].ToString();
                                //    var mailProdajaVsebina = string.Format("<h3>E-Podpis pogodbe</h3>Številka pogodbe: {0}<br>Kupec: {1} {2}<br>Skrbnik: {3}", stevilkaPogodbe, stKupca, nazivKupca, nazivSkrbnika);

                                //    m.PosljiEmail("prodaja@ece.si", mailTo, mailProdajaVsebina, subject, out napaka);
                                //}
                                //catch (Exception ex)
                                //{
                                //    mailErrors += ex.Message;
                                //}

                                if (!string.IsNullOrEmpty(mailErrors)) {
                                    throw new Exception(mailErrors);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public void RefreshMsignStatusPogodbe(int userId)
        {

            try
            {
                var msignService = new mSignService();
                var dal = new DbPovprasevanja();
                var msignDAL = new mSignDAL();
                //int sifraObdelave = Convert.ToInt32(Session[SessionKeys.SifraObdelave].ToString());

                msignService.Init();


                var pendingDocs = msignDAL.GetPendingDocs(Tables.BisPogodbeGl);
                if (pendingDocs.Count == 0)
                    return;

                var signedDocs = msignService.GetDocumentsDTO(msignService.GetDocuments(2));


                foreach (var p in pendingDocs)
                {
                    if (signedDocs.Exists(s => s.id == p.mSignId))
                    {
                        var stevilkaPogodbe = p.Kljuc;
                         
                        msignDAL.UpdateMSignStatus(p.Id, (int)Status.Podpisan, "Signed");
                        PrenesiPodpisanePogodba(stevilkaPogodbe);

                        try
                        {

                            // merge                
                            var filesFilter = string.Format("{0}_*.pdf", stevilkaPogodbe);
                            var targetDir = String.Format("{0}Obrazci\\temp\\", _basePath);
                            var di = new DirectoryInfo(targetDir);
                            var files = new List<string>();
                            var attachments = new List<System.Net.Mail.Attachment>();
                            // združi vse PDF


                            var dt = dal.GetDocSeznamPogodba(stevilkaPogodbe);


                            int zap = 1;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                var docId = Convert.ToInt32(dt.Rows[i]["doc_id"].ToString());
                                var doc = dal.GetDatoteka(docId);

                                var filePath = string.Format("{0}temp\\{1}_{2}_{3}.pdf", _basePath, stevilkaPogodbe, zap.ToString().PadLeft(2, '0'), Environment.TickCount);
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }

                                File.WriteAllBytes(filePath, doc.Datoteka);
                                files.Add(filePath);
                                attachments.Add(new System.Net.Mail.Attachment(filePath));
                                zap++;
                            }

                            var k = dal.GetMsignZahtevaKontakt(p.Id);
                            //var paketEnergija = Convert.ToInt32(k.Rows[0]["PaketEnergija"]);
                            //var paketEnergija = 2000;

                            var filePathPogoji = string.Empty;
                            var filePathCenik = string.Empty;
                            var filePathBon = string.Empty;
                            var templatePath = string.Empty;


                            // dodaj splošne pogoje                
                            filePathPogoji = string.Format("{0}ece_splosni_pogoji_{1}.pdf", _basePath, k.Rows[0]["PaketEnergija"].ToString());
                            if (File.Exists(filePathPogoji))
                            {
                                attachments.Add(new System.Net.Mail.Attachment(filePathPogoji));
                            }

                            // cenik
                            filePathCenik = string.Format("{0}ece_cenik_{1}.pdf", _basePath, k.Rows[0]["PaketEnergija"].ToString());
                            if (File.Exists(filePathCenik))
                            {
                                attachments.Add(new System.Net.Mail.Attachment(filePathCenik));
                            }

                            // mail za podpisano pogodbo
                            templatePath = string.Format("{0}email\\mail_podpisano_template.html", _basePath);

                            var mailTo = k.Rows[0]["Eposta"].ToString();
                            //var mailTo = "simon@3tav.si";
                            //var subject = string.Format("Pogodba št. {0} je podpisana", stevilkaPogodbe);
                            var subject = string.Format("ECE d.o.o. elektronsko podpisani dokumenti št. {0}", stevilkaPogodbe);

                            var template = File.ReadAllText(templatePath);

                            string napaka;
                            var m = new EmailSender();
                            //System.Net.Mail.Attachment attachment;
                            //attachment = new System.Net.Mail.Attachment(fileNameMerged);
                            var att = new List<MailInlineAttachment>();
                            att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}\\email\\ece_logo.png", _basePath), Tag = "ece_logo" });

                            var mailErrors = string.Empty;
                            try
                            {
                                m.PosljiEmailMime("epogodba@ece.si", mailTo, template, subject, att, attachments, out napaka);                                
                            }
                            catch (Exception ex)
                            {
                                mailErrors += ex.Message;
                            }

                            // dodaten mail na prodaja
                            mailTo = "prodaja@ece.si";
                            //mailTo = "simon.rusjan@gmail.com";
                            
                            try
                            {

                                var stKupca = k.Rows[0]["StKupca"].ToString();
                                var nazivKupca = k.Rows[0]["NazivKupca"].ToString();
                                var nazivSkrbnika = k.Rows[0]["NazivSkrbnika"].ToString();
                                //var mailProdajaVsebina = string.Format("<h3>E-Podpis pogodbe</h3>Številka pogodbe: {0}<br>Kupec: {1} {2}", stevilkaPogodbe, stKupca, nazivKupca);
                                var mailProdajaVsebina = string.Format("<h3>E-Podpis pogodbe</h3>Številka pogodbe: {0}<br>Kupec: {1} {2}<br>Skrbnik: {3}", stevilkaPogodbe, stKupca, nazivKupca, nazivSkrbnika);
                                m.PosljiEmail("epogodba@ece.si", mailTo, mailProdajaVsebina, subject, out napaka);
                            }
                            catch (Exception ex)
                            {
                                mailErrors += ex.Message;
                            }

                            if (!string.IsNullOrEmpty(mailErrors))
                            {
                                throw new Exception(mailErrors);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void PrenesiPodpisane(int sifraObdelave)
        {

            var msignDal = new mSignDAL();
            var msignService = new mSignService();

            msignService.Init();
            var id = msignDal.GetMsignIdPovprasevanje(sifraObdelave);
            var ret = string.Empty;



            // če ne obstaja shrani v DOC
            var dt = msignDal.GetPovprasevanjeMsignId(id);
            if (dt == null)
                return;

            var zahtevaId = Convert.ToInt32(dt.Rows[0]["id"]);
            var docId = Convert.ToInt32(dt.Rows[0]["doc_id"]);
            var oznaka = dt.Rows[0]["oznakaPogodbe"].ToString();

            if (!(docId > 0))
            {
                try
                {
                    ret = msignService.GetDocument(id);
                }
                catch (Exception ex)
                {
                    //SetErrorText(ex.Message);
                    return;
                }

                var filename = string.Format("signed_{0}_{1}.pdf", id, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                var stevilo = GetObrazciCount(sifraObdelave);

                var attachments = msignService.GetSignedDocumentAttachments(ret, stevilo);

                var dbp = new DbPovprasevanja();

                // prenesi podpisane dok
                foreach (var a in attachments)
                {
                    var opis = string.Empty;

                    int tip = Convert.ToInt32(a.fileName.Substring(a.fileName.LastIndexOf("_") + 1, a.fileName.IndexOf(".") - a.fileName.LastIndexOf("_") - 1));
                    int vrsteObrazcev = 0;

                    var contentType = "application/pdf";

                    try
                    {
                        // doc
                        docId = dbp.InsertDoc(a.fileName, string.Empty, "pdf", System.Convert.FromBase64String(a.content), -99, contentType);
                        // link
                        dbp.InsertDocLink(sifraObdelave, docId, a.fileName, opis, oznaka, tip, vrsteObrazcev, -99);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("{0} [{1}]", ex.Message, 10));
                    }
                }

                try
                {
                    // zaključi zahtevo
                    msignDal.UpdateZahtevaDocId(zahtevaId, docId);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} [{1}]", ex.Message, 20));
                }
            }


        }


        private void PrenesiPodpisanePogodba(int stevilkaPogodbe)
        {

            var msignDal = new mSignDAL();
            var msignService = new mSignService();

            msignService.Init();
            var id = msignDal.GetMsignIdPogodbaLast(stevilkaPogodbe);
            var ret = string.Empty;



            // če ne obstaja shrani v DOC
            var dt = msignDal.GetBisPogodbaMsignId(id);
            if (dt == null)
                return;

            var zahtevaId = Convert.ToInt32(dt.Rows[0]["id"]);
            var docId = Convert.ToInt32(dt.Rows[0]["doc_id"]);
            var oznaka = dt.Rows[0]["oznakaPogodbe"].ToString();

            if (!(docId > 0))
            {
                try
                {
                    ret = msignService.GetDocument(id);
                }
                catch (Exception ex)
                {
                    //SetErrorText(ex.Message);
                    return;
                }

                var filename = string.Format("signed_{0}_{1}.pdf", id, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                var stevilo = GetObrazciCountPogodba(stevilkaPogodbe);

                var attachments = msignService.GetSignedDocumentAttachments(ret, stevilo);

                var dbp = new DbPovprasevanja();

                // prenesi podpisane dok
                foreach (var a in attachments)
                {
                    var opis = string.Empty;

                    int tip = Convert.ToInt32(a.fileName.Substring(a.fileName.LastIndexOf("_") + 1, a.fileName.IndexOf(".") - a.fileName.LastIndexOf("_") - 1));
                    int vrsteObrazcev = 0;

                    var contentType = "application/pdf";

                    try
                    {
                        // doc
                        docId = dbp.InsertDoc(a.fileName, string.Empty, "pdf", System.Convert.FromBase64String(a.content), -99, contentType);
                        // link
                        dbp.InsertDocLinkPogodba(stevilkaPogodbe, docId, a.fileName, opis, oznaka, tip, vrsteObrazcev, -99);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("{0} [{1}]", ex.Message, 10));
                    }
                }

                try
                {
                    // zaključi zahtevo
                    msignDal.UpdateZahtevaDocId(zahtevaId, docId);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} [{1}]", ex.Message, 20));
                }
            }


        }

        private void PrenesiPodpisanePogodbaMail(int stevilkaPogodbe)
        {

            var msignDal = new mSignDAL();
            var msignService = new mSignService();

            msignService.Init();
            var id = msignDal.GetMsignIdPogodbaLast(stevilkaPogodbe);
            var ret = string.Empty;



            // če ne obstaja shrani v DOC
            var dt = msignDal.GetBisPogodbaMsignId(id);
            if (dt == null)
                return;

            var zahtevaId = Convert.ToInt32(dt.Rows[0]["id"]);
            var docId = Convert.ToInt32(dt.Rows[0]["doc_id"]);
            var oznaka = dt.Rows[0]["oznakaPogodbe"].ToString();

            if (!(docId > 0))
            {
                try
                {
                    ret = msignService.GetDocument(id);
                }
                catch (Exception ex)
                {
                    //SetErrorText(ex.Message);
                    return;
                }

                var filename = string.Format("signed_{0}_{1}.pdf", id, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                var stevilo = GetObrazciCount(stevilkaPogodbe);

                var attachments = msignService.GetSignedDocumentAttachments(ret, stevilo);

                var dbp = new DbPovprasevanja();

                // prenesi podpisane dok
                foreach (var a in attachments)
                {
                    var opis = string.Empty;

                    int tip = Convert.ToInt32(a.fileName.Substring(a.fileName.LastIndexOf("_") + 1, a.fileName.IndexOf(".") - a.fileName.LastIndexOf("_") - 1));
                    int vrsteObrazcev = 0;

                    var contentType = "application/pdf";

                    try
                    {
                        // doc
                        docId = dbp.InsertDoc(a.fileName, string.Empty, "pdf", System.Convert.FromBase64String(a.content), -99, contentType);
                        // link
                        dbp.InsertDocLinkPogodba(stevilkaPogodbe, docId, a.fileName, opis, oznaka, tip, vrsteObrazcev, -99);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("{0} [{1}]", ex.Message, 10));
                    }
                }

                try
                {
                    // zaključi zahtevo
                    msignDal.UpdateZahtevaDocId(zahtevaId, docId);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} [{1}]", ex.Message, 20));
                }
            }


        }


        private int GetObrazciCount(int sifraObdelave)
        {
            int stevilo = 0;

            var dal = new DbPovprasevanja();
            var obrazci = dal.GetPovprasevanjeObrazci(Convert.ToInt32(sifraObdelave));
            for (var i = 0; i < obrazci.Columns.Count; i++)
            {
                if (obrazci.Rows[0][i] != DBNull.Value)
                {
                    stevilo++;
                }
            }

            return stevilo;
        }

        private int GetObrazciCountPogodba(int stevilkaPogodbe)
        {
            int stevilo = 0;

            var dal = new DbPovprasevanja();
            var obrazci = dal.GetPogodbaObrazci(Convert.ToInt32(stevilkaPogodbe));
            for (var i = 0; i < obrazci.Columns.Count; i++)
            {
                if (obrazci.Rows[0][i] != DBNull.Value)
                {
                    stevilo++;
                }
            }

            return stevilo;
        }

        public void CreatePDF(string sifraObrazca, int varianta, int id, string naziv, string targetDir)
        {
            var pdf = new PDFService();
            //var targetDir = @"c:\\temp\\pdf_digital\\";


            if (!Directory.Exists(targetDir))
                throw new Exception(string.Format("Pot za izvoz {0} ne obstaja!", targetDir));

            
            pdf.Init(Settings.ConnectionString);
                        
            var targetFile = string.Format("{0}{1}", targetDir, string.Format(naziv, id));
            byte[] bytes = null;

            try
            {
                bytes = pdf.CreatePDF(sifraObrazca, varianta, id, naziv);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 20));
            }

            try
            {
                File.WriteAllBytes(targetFile, bytes);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 30));
            }

            System.Diagnostics.Process.Start(targetFile);
                
            
        }

        public void PreviewMerged(int stevilkaPogodbe)
        {
            PreviewMerged(stevilkaPogodbe, false);
        }

        public void GeneratePDFMerged(int stevilkaPogodbe)
        {
            var pdfService = new PDFService();
            _dal = new DbPovprasevanja();

            bool flatten = true;

            //var tempPath = @"c:\\temp\\pdf\\";
            var tempPath = string.Format("{0}\\epodpis_pdf\\", Path.GetTempPath());
            if (Directory.Exists(tempPath) == false)
                Directory.CreateDirectory(tempPath);

            var dal = new DbPovprasevanja();
            var obrazciKode = new List<string>();

            var pogodbaVarianta = (new DbPovprasevanja()).GetPogodbaVarianta(stevilkaPogodbe);
            var obrazci = dal.GetPogodbaObrazci(stevilkaPogodbe);
            if (obrazci.Rows.Count == 0)
            {
                return;
            }

            for (var i = 0; i < obrazci.Columns.Count; i++)
            {
                if (obrazci.Rows[0][i] != DBNull.Value)
                {
                    obrazciKode.Add(obrazci.Rows[0][i].ToString());
                }
            }

            // merge                
            var filesFilter = string.Format("{0}_*.pdf", stevilkaPogodbe);
            var targetDir = tempPath;
            var di = new DirectoryInfo(targetDir);

            var filesDelete = di.GetFiles(filesFilter);
            foreach (var fd in filesDelete)
            {
                File.Delete(fd.FullName);
            }


            int zap = 1;
            foreach (var o in obrazciKode)
            {
                int varianta = 1;
                if (o == "ECE-ELEKTRIKA")
                    varianta = pogodbaVarianta;

                var dataSource = pdfService.DobiNastavitveZaObrazec(_dal.GetConnectionString(), o, varianta, stevilkaPogodbe, true);
                var pdf = dataSource[0];
                if (pdf != null)
                {
                    var pdfForm = new PdfFormBuilder();
                    var outputPdfStream = new MemoryStream();
                    pdfForm.Export(pdf.PotPDF, pdf.XmlNastavitev, out outputPdfStream, flatten, flatten);

                    var filePath = string.Format("{0}{1}_{2}_{3}.pdf", tempPath, stevilkaPogodbe, zap.ToString().PadLeft(2, '0'), o);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    File.WriteAllBytes(filePath, outputPdfStream.ToArray());
                    zap++;
                }
            }
            var nazivPDF = string.Format("ece-{0}-zdruzen.pdf", stevilkaPogodbe);
            var fileNameMerged = string.Format("{0}{1}", tempPath, nazivPDF);
            if (File.Exists(fileNameMerged))
            {
                File.Delete(fileNameMerged);
            }

            var files = di.GetFiles(filesFilter);

            var pdfFormBuilder = new PdfFormBuilder();
            var mergeResult = pdfFormBuilder.MergePDFs(files, fileNameMerged);
            if (!mergeResult)
                throw new Exception("Napaka pri združevanju obrazcev!");
        }

        public void PreviewMerged(int stevilkaPogodbe, bool flatten)
        {
             
                var pdfService = new PDFService();
                _dal = new DbPovprasevanja();

                //var tempPath = @"c:\\temp\\pdf\\";
                var tempPath = string.Format("{0}\\epodpis_pdf\\", Path.GetTempPath());
                if (Directory.Exists(tempPath) == false)
                    Directory.CreateDirectory(tempPath);

                var dal = new DbPovprasevanja();
                var obrazciKode = new List<string>();

                var pogodbaVarianta = (new DbPovprasevanja()).GetPogodbaVarianta(stevilkaPogodbe);
                var obrazci = dal.GetPogodbaObrazci(stevilkaPogodbe);
                if (obrazci.Rows.Count == 0)
                {
                    return;
                }

                for (var i = 0; i < obrazci.Columns.Count; i++)
                {
                    if (obrazci.Rows[0][i] != DBNull.Value)
                    {
                        obrazciKode.Add(obrazci.Rows[0][i].ToString());
                    }
                }

                // merge                
                var filesFilter = string.Format("{0}_*.pdf", stevilkaPogodbe);
                var targetDir = tempPath;
                var di = new DirectoryInfo(targetDir);

                var filesDelete = di.GetFiles(filesFilter);
                foreach (var fd in filesDelete) {
                    File.Delete(fd.FullName);
                }


                int zap = 1;
                foreach (var o in obrazciKode)
                {
                    int varianta = 1;
                    if (o == "ECE-ELEKTRIKA")
                        varianta = pogodbaVarianta;

                    var dataSource = pdfService.DobiNastavitveZaObrazec(_dal.GetConnectionString(), o, varianta, stevilkaPogodbe, true);
                    var pdf = dataSource[0];
                    if (pdf != null)
                    {
                        var pdfForm = new PdfFormBuilder();
                        var outputPdfStream = new MemoryStream();
                        pdfForm.Export(pdf.PotPDF, pdf.XmlNastavitev, out outputPdfStream, flatten, flatten);

                        // TODO handle za poslovne
                        var tipObrazca = dal.GetObrazecSifra(o, 1);

                        var filePath = string.Format("{0}{1}_{2}_{3}.pdf", tempPath, stevilkaPogodbe, zap.ToString().PadLeft(2, '0'), tipObrazca);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        File.WriteAllBytes(filePath, outputPdfStream.ToArray());
                        zap++;
                    }
                }
                var nazivPDF = string.Format("ece-{0}-zdruzen.pdf", stevilkaPogodbe);
                var fileNameMerged = string.Format("{0}{1}", tempPath, nazivPDF);
                if (File.Exists(fileNameMerged))
                {
                    File.Delete(fileNameMerged);
                }

                var files = di.GetFiles(filesFilter);

                var pdfFormBuilder = new PdfFormBuilder();
                var mergeResult = pdfFormBuilder.MergePDFs(files, fileNameMerged);
                if (!mergeResult)
                    throw new Exception("Napaka pri združevanju obrazcev!");

                System.Diagnostics.Process.Start(fileNameMerged);
        }

        public void Merge(int stevilkaPogodbe, string datoteke, int userId)
        {

            var pdfService = new PDFService();
            

            //var tempPath = @"c:\\temp\\pdf\\";
            var tempPath = string.Format("{0}\\epodpis_pdf\\merge\\", Path.GetTempPath());
            if (Directory.Exists(tempPath) == false)
                Directory.CreateDirectory(tempPath);


            // briši če že obstaja 
            var filesFilter = string.Format("{0}_*.pdf", stevilkaPogodbe);
            var targetDir = tempPath;
            var di = new DirectoryInfo(targetDir);

            var filesDelete = di.GetFiles(filesFilter);
            foreach (var fd in filesDelete)
            {
                File.Delete(fd.FullName);
            }


            var dal = new DbPovprasevanja();

            var datotekeArray = datoteke.Split(',');

            
            var i = 1;
            foreach (var d in datotekeArray) {

                //var id = Convert.ToInt32(d);
                int id = -1;
                if (!int.TryParse(d, out id))
                    continue;

                var doc = dal.GetDatoteka(id);

                var filePath = string.Format("{0}{1}_0{2}.pdf", tempPath, stevilkaPogodbe, i);

                File.WriteAllBytes(filePath, doc.Datoteka);
                i++;
            }


            var nazivPDF = string.Format("ece-{0}-sodo-{1}.pdf", stevilkaPogodbe, Environment.TickCount);
            var fileNameMerged = string.Format("{0}{1}", tempPath, nazivPDF);
            if (File.Exists(fileNameMerged))
            {
                File.Delete(fileNameMerged);
            }

            var files = di.GetFiles(filesFilter);

            var pdfFormBuilder = new PdfFormBuilder();
            var mergeResult = pdfFormBuilder.MergePDFs(files, fileNameMerged);
            if (!mergeResult)
                throw new Exception("Napaka pri združevanju obrazcev!");


            // združeno datoteko v DOC
            // prenesi podpisane dok
            var opis = "Združena datoteka";

            int tip = 13; // sodo obrazci
            int vrsteObrazcev = 0;

            var contentType = "application/pdf";

            try
            {
                // doc
                var docId = dal.InsertDoc(nazivPDF, string.Empty, "pdf", System.IO.File.ReadAllBytes(fileNameMerged), userId, contentType);
                // link
                dal.InsertDocLinkPogodba(stevilkaPogodbe, docId, nazivPDF, opis, string.Empty, tip, vrsteObrazcev, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 10));
            }

            // združevanje ok, nastavi status            
            foreach (var d in datotekeArray)
            {

                //var id = Convert.ToInt32(d);
                int id = -1;
                if (!int.TryParse(d, out id))
                    continue;

                // soft delete na merge-anih
                try
                {
                    dal.UpdateDocLinkStatus(id, -1);
                }
                catch (Exception ex)
                { 
                    //
                }
            }
        }

        public void Import(int stevilkaPogodbe, string filePath, int userId)
        {
                       
            var opis = "PDF Uvoz";
            int tip = 13; // sodo obrazci
            int vrsteObrazcev = 0;
            var contentType = "application/pdf";

            var nazivPDF = Path.GetFileName(filePath);

            try
            {
                var dal = new DbPovprasevanja();

                // doc
                var docId = dal.InsertDoc(nazivPDF, string.Empty, "pdf", System.IO.File.ReadAllBytes(filePath), userId, contentType);
                // link
                dal.InsertDocLinkPogodba(stevilkaPogodbe, docId, nazivPDF, opis, string.Empty, tip, vrsteObrazcev, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 10));
            }


 
        }

        public void PogodbaPodpisi(int stevilkaPogodbe, bool remote, int userId)
        {
            

                var dal = new DbPovprasevanja();
                var pdfService = new PDFService();

                var kontakt = dal.GetPogodbaKontakt(stevilkaPogodbe, remote);
                var status = dal.GetPogodbaPodpisStatus(stevilkaPogodbe);
                var nazivKupca = kontakt.Rows[0]["NazivKupca"].ToString();
                var varianta = dal.GetPogodbaVarianta(stevilkaPogodbe);
                
                if (status == (int)Status.ZaPodpis)
                    return;

                if (status != (int)Status.Nova)
                    throw new Exception("Status dokumenta ni ustrezen, podpis ni možen!");
                

                var msignDal = new mSignDAL();
                var msignService = new mSignService();

                msignService.Init();


                // obrazci
                List<ObrazecPdf> seznamObrazcev = new List<ObrazecPdf>();
                var obrazci = dal.GetPogodbaObrazci(stevilkaPogodbe);
                for (var i = 0; i < obrazci.Columns.Count; i++)
                {
                    if (obrazci.Rows[0][i] != DBNull.Value)
                    {
                        var o = new ObrazecPdf { IzbranaVrstaStoritve = i, NazivPDF = obrazci.Rows[0][i].ToString() };

                        if (i == 0)
                        {
                            o.Varianta = varianta;
                        }

                        seznamObrazcev.Add(o);
                    }
                }
                
                
                var doc = new CreateDocumentDTO();

                doc.title = string.Format("ece_bis_{0}", stevilkaPogodbe);
                doc.status = (int)mSignDocumentStatus.ForSigning;
                doc.attachments = new List<Attachment>();


                //var obrazci = (List<ObrazecPdf>)Session[SessionKeys.ObrazciSeznam];
                //var seznamObrazcev = new List<ObrazecPdf>();

                foreach (var o in seznamObrazcev)
                {

                    List<ObrazecPdf> dataSource = pdfService.DobiNastavitveZaObrazec(dal.GetConnectionString(), o.NazivPDF, o.Varianta, stevilkaPogodbe, true);
                    ObrazecPdf pdf = dataSource[0];

                    var pdfForm = new PdfFormBuilder();
                    var outputPdfStream = new MemoryStream();
                    pdfForm.Export(pdf.PotPDF, pdf.XmlNastavitev, out outputPdfStream, true, true);

                    var bytes = outputPdfStream.ToArray();
                    var fileBase64 = Convert.ToBase64String(bytes);

                    var att = new Attachment() { fileName = string.Format(doc.title + "_{0}.pdf", o.IzbranaVrstaStoritve), content = fileBase64 };
                    att.signatureTags = new List<SignatureTag>();
                    //att.signatureTags.Add(new SignatureTag() { tag = "{signature1}", metadataValue = "test" });
                    att.signatureTags.Add(new SignatureTag() { tag = "{SIGNATURE1}", metadataValue = nazivKupca });

                    doc.attachments.Add(att);

                }


                /**********************************************************
                *   Kreiraj zahtevo za podpis
                ************************************************************/
                int zahtevaId = msignDal.CreateZahteva(new mSignZahteva { Opis = doc.title, FilePath = seznamObrazcev[0].NazivPDF, Datum = DateTime.Now, Veza = "bis_pogodbe_gl", Kljuc = stevilkaPogodbe, RemoteSign = (remote ? 1 : 0), UserId = userId });


                /**********************************************************
                *   Kreiraj dokument    
                ************************************************************/
                var ret = string.Empty;
                var documentId = -1;

                try
                {
                    ret = msignService.CreateDocument(doc);
                    documentId = msignService.GetCreateDocumentId(ret);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} [{1}]", ex.Message, 15));
                }

                msignDal.UpdateMSignId(zahtevaId, documentId);


                /**********************************************************
                *   Kreiraj shared dokument    
                ************************************************************/


                var sharedDoc = new CreateSharedDocumentDTO();
                sharedDoc.signatureTagName = "{SIGNATURE1}";
                sharedDoc.documentId = documentId;
                sharedDoc.email = kontakt.Rows[0]["Eposta"].ToString();
                sharedDoc.mobile_number = kontakt.Rows[0]["Telefon"].ToString();
                var telefonSuffix = sharedDoc.mobile_number.Substring(sharedDoc.mobile_number.Length - 3);
                var emailTo = sharedDoc.email;
                var paketEn = Convert.ToInt32(kontakt.Rows[0]["PaketEnergija"].ToString());
                var splosniPogoji = kontakt.Rows[0]["SplosniPogoji"].ToString();
                var gdprUrl = kontakt.Rows[0]["GdprURL"].ToString();
            
                var url = string.Empty;
                
                try
                {
                    // brez emaila
                    ret = msignService.CreateSharedDocument(sharedDoc, false);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} [{1}]", ex.Message, 25));
                }

                try
                {
                    var r = msignService.GetCreateSharedDocumentDTO(ret);
                    msignDal.UpdateMSignShareDocument(zahtevaId, r);


                    msignDal.UpdateZahtevaStatus(zahtevaId, 1, "OK");

                    //pnlSign.Visible = false;
                    //pnlSignRemote.Visible = false;
                    //pnlRefresh.Visible = true;

                    //var url = string.Format("https://3tav.msign.si/document/sign?t={0}&lng=sl", r.token);

                    url = msignService.GetSignURL(r.token);
                    //if (remote == false)
                    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", string.Format("window.open('{0}');", url), true);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} [{1}]", ex.Message, 35));
                    
                }
                /*****************************************************************
                *   MAILING 
                ******************************************************************/
                if (remote == true)
                {
                    try
                    {

                        //var templatePath = MapPath(string.Format(string.Format("~/Obrazci/email/mail_vpodpis_template_{0}.html", paketEn)));
                        var templatePath = string.Format("{0}email\\mail_vpodpis_template.html", _basePath);
                        var template = File.ReadAllText(templatePath);
                        //var subject = string.Format("Pogodba št. {0} je predana v podpis", stevilkaPogodbe);
                        var subject = string.Format("ECE d.o.o. elektronski podpis pogodbene dokumentacije št. {0}", stevilkaPogodbe);
                        var mailFrom = "epogodba@ece.si";

                        string napaka;
                        var m = new EmailSender();

                        template = template.Replace("{{tel_st_suffix}}", telefonSuffix);
                        template = template.Replace("{{podpis_url}}", url);
                        template = template.Replace("{{splosni_pogoji_url}}", splosniPogoji);
                        template = template.Replace("{{gdpr_url}}", gdprUrl);

                        var att = new List<MailInlineAttachment>();
                        att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}{1}", _basePath, "email\\ece_logo.png"), Tag = "ece_logo" });

                        m.PosljiEmailMime(mailFrom, emailTo, template, subject, att, out napaka);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("{0} [{1}]", ex.Message, 45));
                    }
                }
             
        }

        public void MailVPodpisTest(int stevilkaPogodbe, bool remote) {
            var dal = new DbPovprasevanja();
            //var kontakt = dal.GetPogodbaKontakt(stevilkaPogodbe, remote);
            var url = string.Format("https://3tav.msign.si/document/sign?t={0}&lng=sl", "TEST");
            var splosniPogoji = "Splošni pogoji TEST";// kontakt.Rows[0]["SplosniPogoji"].ToString();
            var telefonSuffix = "TEST";
            var emailTo = "simon.rusjan@gmail.com";
            //var emailTo = "simon@3tav.si";

            try
            {

                //var templatePath = MapPath(string.Format(string.Format("~/Obrazci/email/mail_vpodpis_template_{0}.html", paketEn)));
                var templatePath = string.Format("{0}email\\mail_vpodpis_template.html", _basePath);
                var template = File.ReadAllText(templatePath);
                //var subject = string.Format("Pogodba št. {0} je predana v podpis", stevilkaPogodbe);
                var subject = string.Format("ECE d.o.o. elektronski podpis pogodbene dokumentacije št. {0}", stevilkaPogodbe);
                var mailFrom = "epogodba@ece.si";

                string napaka;
                var m = new EmailSender();

                template = template.Replace("{{tel_st_suffix}}", telefonSuffix);
                template = template.Replace("{{podpis_url}}", url);
                template = template.Replace("{{splosni_pogoji_url}}", splosniPogoji);

                var att = new List<MailInlineAttachment>();
                att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}{1}", _basePath, "email\\ece_logo.png"), Tag = "ece_logo" });

                var ret = m.PosljiEmailMime(mailFrom, emailTo, template, subject, att, out napaka);
                if (ret == 0)
                    throw new Exception(napaka);


            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 45));
            }
        
        }


        public void PogodbaPodpisi(int stevilkaPogodbe, bool remote, int userId, string telefon, string email)
        {
            /*!!! Default Metoda!*/

            var dal = new DbPovprasevanja();
            var pdfService = new PDFService();

            var kontakt = dal.GetPogodbaKontakt(stevilkaPogodbe, remote);
            var status = dal.GetPogodbaPodpisStatus(stevilkaPogodbe);
            var nazivKupca = kontakt.Rows[0]["NazivKupca"].ToString();
            var varianta = dal.GetPogodbaVarianta(stevilkaPogodbe);
          

            if (status == (int)Status.ZaPodpis)
                return;

            if (status != (int)Status.Nova)
                throw new Exception("Status dokumenta ni ustrezen, podpis ni možen!");


            var msignDal = new mSignDAL();
            var msignService = new mSignService();

            msignService.Init();


            // obrazci
            List<ObrazecPdf> seznamObrazcev = new List<ObrazecPdf>();
            var obrazci = dal.GetPogodbaObrazci(stevilkaPogodbe);
            for (var i = 0; i < obrazci.Columns.Count; i++)
            {
                if (obrazci.Rows[0][i] != DBNull.Value)
                {
                    var o = new ObrazecPdf { IzbranaVrstaStoritve = i, NazivPDF = obrazci.Rows[0][i].ToString() };

                    if (i == 0)
                    {
                        o.Varianta = varianta;
                    }

                    seznamObrazcev.Add(o);
                }
            }


            var doc = new CreateDocumentDTO();

            doc.title = string.Format(_msignDocumentName, stevilkaPogodbe);
            doc.status = (int)mSignDocumentStatus.ForSigning;
            doc.attachments = new List<Attachment>();


            //var obrazci = (List<ObrazecPdf>)Session[SessionKeys.ObrazciSeznam];
            //var seznamObrazcev = new List<ObrazecPdf>();

            foreach (var o in seznamObrazcev)
            {

                List<ObrazecPdf> dataSource = pdfService.DobiNastavitveZaObrazec(dal.GetConnectionString(), o.NazivPDF, o.Varianta, stevilkaPogodbe, true);
                ObrazecPdf pdf = dataSource[0];

                var pdfForm = new PdfFormBuilder();
                var outputPdfStream = new MemoryStream();
                pdfForm.Export(pdf.PotPDF, pdf.XmlNastavitev, out outputPdfStream, true, true);

                var bytes = outputPdfStream.ToArray();
                var fileBase64 = Convert.ToBase64String(bytes);

                var att = new Attachment() { fileName = string.Format(doc.title + "_{0}.pdf", o.IzbranaVrstaStoritve), content = fileBase64 };
                att.signatureTags = new List<SignatureTag>();
                //att.signatureTags.Add(new SignatureTag() { tag = "{signature1}", metadataValue = "test" });
                //att.signatureTags.Add(new SignatureTag() { tag = "{signature1}", metadataValue = "Andrej Grapulin" });
                att.signatureTags.Add(new SignatureTag() { tag = "{signature2}", metadataValue = nazivKupca });

                doc.attachments.Add(att);

            }


            /**********************************************************
            *   Kreiraj zahtevo za podpis
            ************************************************************/
            int zahtevaId = msignDal.CreateZahtevaKontakt(new mSignZahteva { Opis = doc.title, FilePath = seznamObrazcev[0].NazivPDF, Datum = DateTime.Now, Veza = "bis_pogodbe_gl", Kljuc = stevilkaPogodbe, RemoteSign = (remote ? 1 : 0), UserId = userId, Telefon = telefon, Email = email });


            /**********************************************************
            *   Kreiraj dokument    
            ************************************************************/
            var ret = string.Empty;
            var documentId = -1;

            try
            {
                ret = msignService.CreateDocument(doc);
                documentId = msignService.GetCreateDocumentId(ret);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 15));
            }

            msignDal.UpdateMSignId(zahtevaId, documentId);


            /**********************************************************
            *   Kreiraj shared dokument    
            ************************************************************/


            var sharedDoc = new CreateSharedDocumentDTO();
            sharedDoc.signatureTagName = "{signature2}";
            sharedDoc.documentId = documentId;
            sharedDoc.email = email; // kontakt.Rows[0]["Eposta"].ToString();
            sharedDoc.mobile_number = telefon; //kontakt.Rows[0]["Telefon"].ToString();
            var telefonSuffix = sharedDoc.mobile_number.Substring(sharedDoc.mobile_number.Length - 3);
            var emailTo = sharedDoc.email;
            var paketEn = Convert.ToInt32(kontakt.Rows[0]["PaketEnergija"].ToString());
            var splosniPogoji = kontakt.Rows[0]["SplosniPogoji"].ToString();
            var gdprUrl = kontakt.Rows[0]["GdprURL"].ToString();
            var url = string.Empty;
            var splosniPogojiDodHTML = string.Empty;

            try
            {
                // brez emaila
                ret = msignService.CreateSharedDocument(sharedDoc, false);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 25));
            }

            try
            {
                var r = msignService.GetCreateSharedDocumentDTO(ret);
                msignDal.UpdateMSignShareDocument(zahtevaId, r);

                msignDal.UpdateZahtevaStatus(zahtevaId, 1, "OK");
              
                url = msignService.GetSignURL(r.token);
              
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 35));

            }
            /*****************************************************************
            *   MAILING 
            ******************************************************************/
            if (remote == true)
            {
                try
                {
                    
                    var templatePath = string.Format("{0}email\\mail_vpodpis_template.html", _basePath);
                    var template = File.ReadAllText(templatePath);                    
                    var subject = string.Format(_emailSubject, stevilkaPogodbe);
                    var mailFrom = _emailFrom;

                    string napaka;
                    var m = new EmailSender();

                    template = template.Replace("{{tel_st_suffix}}", telefonSuffix);
                    template = template.Replace("{{podpis_url}}", url);
                    template = template.Replace("{{splosni_pogoji_url}}", splosniPogoji);
                    template = template.Replace("{{gdpr_url}}", gdprUrl);


                    if (!string.IsNullOrEmpty(splosniPogojiDodHTML))
                    {
                        template = template.Replace("{{splosni_pogoji_url_dod}}", splosniPogojiDodHTML);
                    }
                    else
                    {
                        template = template.Replace("{{splosni_pogoji_url_dod}}", string.Empty);
                    }

                    var att = new List<MailInlineAttachment>();
                    //att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}{1}", _basePath, "email\\ece_logo.png"), Tag = "ece_logo" });

                    m.PosljiEmailMime(mailFrom, emailTo, template, subject, att, out napaka);
                    if (!string.IsNullOrEmpty(napaka)) {
                        throw new Exception(string.Format("Mail error: {0}", napaka));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} [{1}]", ex.Message, 45));
                }
            }

        }

        public void PogodbaVPregled(int stevilkaPogodbe, bool remote, int userId, string telefon, string email)
        {
            /*!!! Samo mail v pregled, brez Msigna! */

            var dal = new DbPovprasevanja();
            var pdfService = new PDFService();

            var kontakt = dal.GetPogodbaKontakt(stevilkaPogodbe, remote, userId);
            var status = dal.GetPogodbaPodpisStatus(stevilkaPogodbe);
            var nazivKupca = kontakt.Rows[0]["NazivKupca"].ToString();
            var emailUporabnika = kontakt.Rows[0]["emailUporabnika"].ToString();
            var nazivUporabnika = kontakt.Rows[0]["nazivUporabnika"].ToString();

            var varianta = dal.GetPogodbaVarianta(stevilkaPogodbe);
           
            var msignDal = new mSignDAL();
            var msignService = new mSignService();

            msignService.Init();


            // obrazci
            List<ObrazecPdf> seznamObrazcev = new List<ObrazecPdf>();
            var obrazci = dal.GetPogodbaObrazci(stevilkaPogodbe);
            for (var i = 0; i < obrazci.Columns.Count; i++)
            {
                if (obrazci.Rows[0][i] != DBNull.Value)
                {
                    var o = new ObrazecPdf { IzbranaVrstaStoritve = i, NazivPDF = obrazci.Rows[0][i].ToString() };

                    if (i == 0)
                    {
                        o.Varianta = varianta;
                    }

                    seznamObrazcev.Add(o);
                }
            }


            var doc = new CreateDocumentDTO();

            doc.title = string.Format(_msignDocumentName, stevilkaPogodbe);
            

            foreach (var o in seznamObrazcev)
            {

                List<ObrazecPdf> dataSource = pdfService.DobiNastavitveZaObrazec(dal.GetConnectionString(), o.NazivPDF, o.Varianta, stevilkaPogodbe, true);
                ObrazecPdf pdf = dataSource[0];

                var pdfForm = new PdfFormBuilder();
                var outputPdfStream = new MemoryStream();
                pdfForm.Export(pdf.PotPDF, pdf.XmlNastavitev, out outputPdfStream, true, true);

                var bytes = outputPdfStream.ToArray();
                var fileBase64 = Convert.ToBase64String(bytes);

                var att = new Attachment() { fileName = string.Format(doc.title + "_{0}.pdf", o.IzbranaVrstaStoritve), content = fileBase64 };
                att.signatureTags = new List<SignatureTag>();
                //att.signatureTags.Add(new SignatureTag() { tag = "{signature1}", metadataValue = "test" });
                //att.signatureTags.Add(new SignatureTag() { tag = "{signature1}", metadataValue = "Andrej Grapulin" });
                att.signatureTags.Add(new SignatureTag() { tag = "{signature2}", metadataValue = nazivKupca });

                doc.attachments.Add(att);

            }


            /**********************************************************
            *   Kreiraj zahtevo za podpis
            ************************************************************/
            //int zahtevaId = msignDal.CreateZahtevaKontakt(new mSignZahteva { Opis = doc.title, FilePath = seznamObrazcev[0].NazivPDF, Datum = DateTime.Now, Veza = "bis_pogodbe_gl", Kljuc = stevilkaPogodbe, RemoteSign = (remote ? 1 : 0), UserId = userId, Telefon = telefon, Email = email });


            /**********************************************************
            *   Kreiraj dokument    
            ************************************************************/
            //var ret = string.Empty;
            //var documentId = -1;

            //try
            //{
            //    ret = msignService.CreateDocument(doc);
            //    documentId = msignService.GetCreateDocumentId(ret);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(string.Format("{0} [{1}]", ex.Message, 15));
            //}

            //msignDal.UpdateMSignId(zahtevaId, documentId);


            /**********************************************************
            *   Kreiraj shared dokument    
            ************************************************************/


            var sharedDoc = new CreateSharedDocumentDTO();
            //sharedDoc.signatureTagName = "{signature2}";
            //sharedDoc.documentId = documentId;
            sharedDoc.email = email; // kontakt.Rows[0]["Eposta"].ToString();
            sharedDoc.mobile_number = telefon; //kontakt.Rows[0]["Telefon"].ToString();
            var telefonSuffix = sharedDoc.mobile_number.Substring(sharedDoc.mobile_number.Length - 3);
            var emailTo = sharedDoc.email;
            
            var paketEn = Convert.ToInt32(kontakt.Rows[0]["PaketEnergija"].ToString());
            var splosniPogoji = kontakt.Rows[0]["SplosniPogoji"].ToString();
            var gdprUrl = kontakt.Rows[0]["GdprURL"].ToString();
            var url = string.Empty;
            var splosniPogojiDodHTML = string.Empty;

            //try
            //{
            //    // brez emaila
            //    ret = msignService.CreateSharedDocument(sharedDoc, false);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(string.Format("{0} [{1}]", ex.Message, 25));
            //}

            //try
            //{
            //    var r = msignService.GetCreateSharedDocumentDTO(ret);
            //    msignDal.UpdateMSignShareDocument(zahtevaId, r);

            //    msignDal.UpdateZahtevaStatus(zahtevaId, 1, "OK");

            //    url = msignService.GetSignURL(r.token);

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(string.Format("{0} [{1}]", ex.Message, 35));

            //}
            /*****************************************************************
            *   MAILING 
            ******************************************************************/
            if (remote == true)
            {
                try
                {

                    var templatePath = string.Format("{0}email\\mail_vpregled_template.html", _basePath);
                    var template = File.ReadAllText(templatePath);
                    var subject = string.Format(_emailSubject, stevilkaPogodbe);
                    var mailFrom = _emailFrom;

                    string napaka;
                    var m = new EmailSender();

                    template = template.Replace("{{tel_st_suffix}}", telefonSuffix);
                    template = template.Replace("{{podpis_url}}", url);
                    template = template.Replace("{{splosni_pogoji_url}}", splosniPogoji);
                    template = template.Replace("{{gdpr_url}}", gdprUrl);
                    template = template.Replace("{{uporabnik_naziv}}", nazivUporabnika);
                    


                    if (!string.IsNullOrEmpty(splosniPogojiDodHTML))
                    {
                        template = template.Replace("{{splosni_pogoji_url_dod}}", splosniPogojiDodHTML);
                    }
                    else
                    {
                        template = template.Replace("{{splosni_pogoji_url_dod}}", string.Empty);
                    }

                    var att = new List<MailInlineAttachment>();
                    //att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}{1}", _basePath, "email\\ece_logo.png"), Tag = "ece_logo" });
                    
                    // pošliji mail potrjevalcu in v vednost uporabniku
                    m.PosljiEmailMime(mailFrom, emailTo, emailUporabnika, template, subject, att, out napaka);
                    if (!string.IsNullOrEmpty(napaka))
                    {
                        throw new Exception(string.Format("Mail error: {0}", napaka));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} [{1}]", ex.Message, 45));
                }
            }

        }

        public void PogodbaPodpisi(int stevilkaPogodbe, bool remote, int userId, string filePath)
        {
            // verzija kjer gre v podpis statični fajl (se ne generira sproti)

            var dal = new DbPovprasevanja();
            var pdfService = new PDFService();

            var kontakt = dal.GetPogodbaKontakt(stevilkaPogodbe, remote);
            var status = dal.GetPogodbaPodpisStatus(stevilkaPogodbe);
            var nazivKupca = kontakt.Rows[0]["NazivKupca"].ToString();
            var varianta = dal.GetPogodbaVarianta(stevilkaPogodbe);

            if (status == (int)Status.ZaPodpis)
                return;

            if (status != (int)Status.Nova)
                throw new Exception("Status dokumenta ni ustrezen, podpis ni možen!");


            var msignDal = new mSignDAL();
            var msignService = new mSignService();

            msignService.Init();


           
            var doc = new CreateDocumentDTO();

            doc.title = string.Format("ece_bis_{0}", stevilkaPogodbe);
            doc.status = (int)mSignDocumentStatus.ForSigning;
            doc.attachments = new List<Attachment>();


            var bytes = File.ReadAllBytes(filePath);
            var fileBase64 = Convert.ToBase64String(bytes);

            var attachment = new Attachment() { fileName = string.Format(doc.title + "_{0}.pdf", 99), content = fileBase64 };
            attachment.signatureTags = new List<SignatureTag>();
            //att.signatureTags.Add(new SignatureTag() { tag = "{signature1}", metadataValue = "test" });
            attachment.signatureTags.Add(new SignatureTag() { tag = "{SIGNATURE1}", metadataValue = nazivKupca });

            doc.attachments.Add(attachment);

             


            /**********************************************************
            *   Kreiraj zahtevo za podpis
            ************************************************************/
            int zahtevaId = msignDal.CreateZahteva(new mSignZahteva { Opis = doc.title, FilePath = Path.GetFileName(filePath), Datum = DateTime.Now, Veza = "bis_pogodbe_gl", Kljuc = stevilkaPogodbe, RemoteSign = (remote ? 1 : 0), UserId = userId });


            /**********************************************************
            *   Kreiraj dokument    
            ************************************************************/
            var ret = string.Empty;
            var documentId = -1;

            try
            {
                ret = msignService.CreateDocument(doc);
                documentId = msignService.GetCreateDocumentId(ret);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 15));
            }

            msignDal.UpdateMSignId(zahtevaId, documentId);


            /**********************************************************
            *   Kreiraj shared dokument    
            ************************************************************/


            var sharedDoc = new CreateSharedDocumentDTO();
            sharedDoc.signatureTagName = "{SIGNATURE1}";
            sharedDoc.documentId = documentId;
            sharedDoc.email = kontakt.Rows[0]["Eposta"].ToString();
            sharedDoc.mobile_number = kontakt.Rows[0]["Telefon"].ToString();
            var telefonSuffix = sharedDoc.mobile_number.Substring(sharedDoc.mobile_number.Length - 3);
            var emailTo = sharedDoc.email;
            var paketEn = Convert.ToInt32(kontakt.Rows[0]["PaketEnergija"].ToString());
            var splosniPogoji = kontakt.Rows[0]["SplosniPogoji"].ToString();
            var url = string.Empty;

            // začasni dodatni splošni pogoji            
            var splosniPogojiDod = string.Empty;
            var splosniPogojiDodHTML = string.Empty;
//            switch (paketEn) { 
//                case 445236: 
//                case 445237:
//                    splosniPogojiDod = "https://www.ece.si/app/uploads/2022/01/Splo%C5%A1ni-pogoji-EEG-SOCA_Jan-2022.pdf";
//                    break;
//                case 445240:
//                case 445241:
//                    splosniPogojiDod = "https://www.ece.si/app/uploads/2022/01/Splo%C5%A1ni-pogoji-EEG-SAVA_Jan-2022.pdf";
//                    break;
//                case 445242:
//                case 445243:
//                    splosniPogojiDod = "https://www.ece.si/app/uploads/2022/01/Splo%C5%A1ni-pogoji-EEG-DRAVA_Jan-2022.pdf";
//                    break;
//                case 445244:
//                case 445245:
//                    splosniPogojiDod = "https://www.ece.si/app/uploads/2022/01/Splo%C5%A1ni-pogoji-EEG-TC_Jan-2022.pdf";
//                    break;                                    
//            }

//            var splosniPogojiDodHTML = string.Empty;
//            if (!string.IsNullOrEmpty(splosniPogojiDod)) {
//                splosniPogojiDodHTML = @"<p style=""margin: 0; font-size: 16px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 19px; margin-top: 0; margin-bottom: 0;""><span style=""font-size: 16px;font-weight: 700"">Od 1.3.2022 za izbrani paket veljajo &nbsp;<a href=""{{splosni_pogoji_url_dod}}"" style=""color: #00599e;"">NOVI POGOJI</a>.</span></p>
//                                        <p style=""margin: 0; font-size: 16px; line-height: 1.2; word-break: break-word; mso-line-height-alt: 19px; margin-top: 0; margin-bottom: 0;"">&nbsp;</p>";
//                splosniPogojiDodHTML = splosniPogojiDodHTML.Replace("{{splosni_pogoji_url_dod}}", splosniPogojiDod);
//            }

                


            try
            {
                // brez emaila
                ret = msignService.CreateSharedDocument(sharedDoc, false);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 25));
            }

            try
            {
                var r = msignService.GetCreateSharedDocumentDTO(ret);
                msignDal.UpdateMSignShareDocument(zahtevaId, r);


                msignDal.UpdateZahtevaStatus(zahtevaId, 1, "OK");

                //pnlSign.Visible = false;
                //pnlSignRemote.Visible = false;
                //pnlRefresh.Visible = true;

                //var url = string.Format("https://3tav.msign.si/document/sign?t={0}&lng=sl", r.token);

                url = msignService.GetSignURL(r.token);
                //if (remote == false)
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", string.Format("window.open('{0}');", url), true);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 35));

            }
            /*****************************************************************
            *   MAILING 
            ******************************************************************/
            if (remote == true)
            {
                try
                {

                    //var templatePath = MapPath(string.Format(string.Format("~/Obrazci/email/mail_vpodpis_template_{0}.html", paketEn)));
                    var templatePath = string.Format("{0}email\\mail_vpodpis_template.html", _basePath);
                    var template = File.ReadAllText(templatePath);
                    var subject = string.Format("ECE d.o.o. elektronski podpis pogodbene dokumentacije št. {0}", stevilkaPogodbe);
                    //var subject = string.Format("Pogodba št. {0} je predana v podpis", stevilkaPogodbe);

                    var mailFrom = "epogodba@ece.si";

                    string napaka;
                    var m = new EmailSender();

                    template = template.Replace("{{tel_st_suffix}}", telefonSuffix);
                    template = template.Replace("{{podpis_url}}", url);
                    template = template.Replace("{{splosni_pogoji_url}}", splosniPogoji);

                    if (!string.IsNullOrEmpty(splosniPogojiDodHTML))
                    {
                        template = template.Replace("{{splosni_pogoji_url_dod}}", splosniPogojiDodHTML);
                    }
                    else
                    {
                        template = template.Replace("{{splosni_pogoji_url_dod}}", string.Empty);
                    }

                    

                    var att = new List<MailInlineAttachment>();
                    att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}{1}", _basePath, "email\\ece_logo.png"), Tag = "ece_logo" });

                    m.PosljiEmailMime(mailFrom, emailTo, template, subject, att, out napaka);

                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} [{1}]", ex.Message, 45));
                }
            }

        }

        public void PogodbaPodpisi(int stevilkaPogodbe, bool remote, int userId, string filePath, string telefon, string email )
        {
            // verzija kjer gre v podpis statični fajl (se ne generira sproti)

            var dal = new DbPovprasevanja();
            var pdfService = new PDFService();

            var kontakt = dal.GetPogodbaKontakt(stevilkaPogodbe, remote);
            var status = dal.GetPogodbaPodpisStatus(stevilkaPogodbe);
            var nazivKupca = kontakt.Rows[0]["NazivKupca"].ToString();
            var varianta = dal.GetPogodbaVarianta(stevilkaPogodbe);
            var gdprUrl = kontakt.Rows[0]["GdprURL"].ToString();

            if (status == (int)Status.ZaPodpis)
                return;

            if (status != (int)Status.Nova)
                throw new Exception("Status dokumenta ni ustrezen, podpis ni možen!");


            var msignDal = new mSignDAL();
            var msignService = new mSignService();

            msignService.Init();



            var doc = new CreateDocumentDTO();

            doc.title = string.Format("ece_bis_{0}", stevilkaPogodbe);
            doc.status = (int)mSignDocumentStatus.ForSigning;
            doc.attachments = new List<Attachment>();


            var bytes = File.ReadAllBytes(filePath);
            var fileBase64 = Convert.ToBase64String(bytes);

            var attachment = new Attachment() { fileName = string.Format(doc.title + "_{0}.pdf", 99), content = fileBase64 };
            attachment.signatureTags = new List<SignatureTag>();
            //att.signatureTags.Add(new SignatureTag() { tag = "{signature1}", metadataValue = "test" });
            attachment.signatureTags.Add(new SignatureTag() { tag = "{SIGNATURE1}", metadataValue = nazivKupca });

            doc.attachments.Add(attachment);




            /**********************************************************
            *   Kreiraj zahtevo za podpis
            ************************************************************/
            int zahtevaId = msignDal.CreateZahteva(new mSignZahteva { Opis = doc.title, FilePath = Path.GetFileName(filePath), Datum = DateTime.Now, Veza = "bis_pogodbe_gl", Kljuc = stevilkaPogodbe, RemoteSign = (remote ? 1 : 0), UserId = userId });


            /**********************************************************
            *   Kreiraj dokument    
            ************************************************************/
            var ret = string.Empty;
            var documentId = -1;

            try
            {
                ret = msignService.CreateDocument(doc);
                documentId = msignService.GetCreateDocumentId(ret);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 15));
            }

            msignDal.UpdateMSignId(zahtevaId, documentId);


            /**********************************************************
            *   Kreiraj shared dokument    
            ************************************************************/


            var sharedDoc = new CreateSharedDocumentDTO();
            sharedDoc.signatureTagName = "{SIGNATURE1}";
            sharedDoc.documentId = documentId;
            sharedDoc.email = email;//kontakt.Rows[0]["Eposta"].ToString();
            sharedDoc.mobile_number = telefon;//kontakt.Rows[0]["Telefon"].ToString();
            var telefonSuffix = sharedDoc.mobile_number.Substring(sharedDoc.mobile_number.Length - 3);
            var emailTo = sharedDoc.email;
            var paketEn = Convert.ToInt32(kontakt.Rows[0]["PaketEnergija"].ToString());
            var splosniPogoji = kontakt.Rows[0]["SplosniPogoji"].ToString();
            var url = string.Empty;

            try
            {
                // brez emaila
                ret = msignService.CreateSharedDocument(sharedDoc, false);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 25));
            }

            try
            {
                var r = msignService.GetCreateSharedDocumentDTO(ret);
                msignDal.UpdateMSignShareDocument(zahtevaId, r);


                msignDal.UpdateZahtevaStatus(zahtevaId, 1, "OK");

                //pnlSign.Visible = false;
                //pnlSignRemote.Visible = false;
                //pnlRefresh.Visible = true;

                //var url = string.Format("https://3tav.msign.si/document/sign?t={0}&lng=sl", r.token);

                url = msignService.GetSignURL(r.token);
                //if (remote == false)
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", string.Format("window.open('{0}');", url), true);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} [{1}]", ex.Message, 35));

            }
            /*****************************************************************
            *   MAILING 
            ******************************************************************/
            if (remote == true)
            {
                try
                {

                    //var templatePath = MapPath(string.Format(string.Format("~/Obrazci/email/mail_vpodpis_template_{0}.html", paketEn)));
                    var templatePath = string.Format("{0}email\\mail_vpodpis_template.html", _basePath);

                    // probaj nekajkrat zaradi prekinjanja povezave s share
                    if (!File.Exists(templatePath)) {
                        var retry = 3;
                        for (int i = 0; i < retry; i++)
                        {
                            System.Threading.Thread.Sleep(500);
                            if (File.Exists(templatePath)) {
                                break;
                            }
                        }                        
                    }

                    var template = File.ReadAllText(templatePath);
                    //var subject = string.Format("Pogodba št. {0} je predana v podpis", stevilkaPogodbe);
                    var subject = string.Format("ECE d.o.o. elektronski podpis pogodbene dokumentacije št. {0}", stevilkaPogodbe);
                    var mailFrom = "epogodba@ece.si";

                    string napaka;
                    var m = new EmailSender();

                    template = template.Replace("{{tel_st_suffix}}", telefonSuffix);
                    template = template.Replace("{{podpis_url}}", url);
                    template = template.Replace("{{splosni_pogoji_url}}", splosniPogoji);
                    template = template.Replace("{{gdpr_url}}", gdprUrl);

                    var att = new List<MailInlineAttachment>();
                    att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}{1}", _basePath, "email\\ece_logo.png"), Tag = "ece_logo" });

                    m.PosljiEmailMime(mailFrom, emailTo, template, subject, att, out napaka);

                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} [{1}]", ex.Message, 45));
                }
            }

        }

        public void PosljiMailPodpisano(int stevilkaPogodbe, string emailTo) 
        { 
            try
            {
                //MailVPodpisTest(stevilkaPogodbe, true);
                //return;

                var dal = new DbPovprasevanja();

                // merge                
                var filesFilter = string.Format("{0}_*.pdf", stevilkaPogodbe);
                var targetDir = String.Format("{0}Obrazci\\temp\\", _basePath);
                var di = new DirectoryInfo(targetDir);
                var files = new List<string>();
                var attachments = new List<System.Net.Mail.Attachment>();
                // združi vse PDF


                var dt = dal.GetDocSeznamPogodba(stevilkaPogodbe);


                int zap = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var docId = Convert.ToInt32(dt.Rows[i]["doc_id"].ToString());
                    var doc = dal.GetDatoteka(docId);

                    var filePath = string.Format("{0}temp\\{1}_{2}_{3}.pdf", _basePath, stevilkaPogodbe, zap.ToString().PadLeft(2, '0'), Environment.TickCount);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    File.WriteAllBytes(filePath, doc.Datoteka);
                    files.Add(filePath);
                    attachments.Add(new System.Net.Mail.Attachment(filePath));
                    zap++;
                }

                var k = dal.GetPogodbaKontakt(stevilkaPogodbe, true);

                //var paketEnergija = Convert.ToInt32(k.Rows[0]["PaketEnergija"]);
                //var paketEnergija = 2000;

                var filePathPogoji = string.Empty;
                var filePathCenik = string.Empty;
                var filePathBon = string.Empty;
                var templatePath = string.Empty;

                // dodaj splošne pogoje                
                filePathPogoji = string.Format("{0}ece_splosni_pogoji_{1}.pdf", _basePath, k.Rows[0]["PaketEnergija"].ToString());
                if (File.Exists(filePathPogoji))
                {
                    attachments.Add(new System.Net.Mail.Attachment(filePathPogoji));
                }

                // cenik
                filePathCenik = string.Format("{0}ece_cenik_{1}.pdf", _basePath, k.Rows[0]["PaketEnergija"].ToString());
                if (File.Exists(filePathCenik))
                {
                    attachments.Add(new System.Net.Mail.Attachment(filePathCenik));
                }


                // mail za podpisano pogodbo
                templatePath = string.Format("{0}email\\mail_podpisano_tablica_template.html", _basePath);

                var mailTo = emailTo;// preko parametr  k.Rows[0]["Eposta"].ToString();
                //var mailTo = "simon@3tav.si";
                //var subject = string.Format("Pogodba št. {0} je podpisana", stevilkaPogodbe);
                var subject = string.Format("ECE d.o.o. elektronsko podpisani dokumenti št. {0}", stevilkaPogodbe);

                var template = File.ReadAllText(templatePath);

                string napaka;
                var m = new EmailSender();
                //System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(fileNameMerged);
                var att = new List<MailInlineAttachment>();
                att.Add(new MailInlineAttachment() { FileName = "ece_logo.png", FilePath = string.Format("{0}\\email\\ece_logo.png", _basePath), Tag = "ece_logo" });
                                
                var mailErrors = string.Empty;
                try
                {
                    m.PosljiEmailMime("epogodba@ece.si", mailTo, template, subject, att, attachments, out napaka);
                    if (string.IsNullOrEmpty(napaka)) {
                        mailErrors += napaka;
                    }

                }
                catch (Exception ex)
                {
                    mailErrors += ex.Message;
                }

                // dodaten mail na prodaja
                //mailTo = "prodaja@ece.si";
                //mailTo = "simon.rusjan@gmail.com";

                //try
                //{

                //    var stKupca = k.Rows[0]["StKupca"].ToString();
                //    var nazivKupca = k.Rows[0]["NazivKupca"].ToString();
                //    var mailProdajaVsebina = string.Format("<h3>E-Podpis pogodbe</h3>Številka pogodbe: {0}<br>Kupec: {1} {2}", stevilkaPogodbe, stKupca, nazivKupca);

                //    m.PosljiEmail("prodaja@ece.si", mailTo, mailProdajaVsebina, subject, out napaka);
                //}
                //catch (Exception ex)
                //{
                //    mailErrors += ex.Message;
                //}

                if (!string.IsNullOrEmpty(mailErrors)) {
                    throw new Exception(mailErrors);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }        
        }

        public void GdprOpen(int stevilkaPogodbe, int userId) {
            var dal = new DbPovprasevanja();
            var kontakt = dal.GetPogodbaKontakt(stevilkaPogodbe, true);            
            var gdprUrl = kontakt.Rows[0]["GdprURL"].ToString();

            System.Diagnostics.Process.Start(gdprUrl);
        }

        public void UrlOpen(string url, int userId)
        {            
            System.Diagnostics.Process.Start(url);
        }

        public void Log(string method, string args, int userId, int id, int result, string message) {            
            var dal = new mSignDAL();
            dal.Log(method, args, userId, id, result, message);
        }

        public void Log(string method, string args, int userId, int id, int result, string message, int tip, string fileName)
        {           
            var dal = new mSignDAL();
            dal.Log(method, args, userId, id, result, message, tip, fileName);
        }
    }
}
