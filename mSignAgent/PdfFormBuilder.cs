using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml;

namespace mSignAgent
{
    public class PdfFormBuilder
    {
        private XmlDocument _doc = null;
        public bool Debug = false;
        public string Log = "";

        protected void SetAcroField(PdfStamper stamper, string name, string value)
        {
            String[] values = stamper.AcroFields.GetAppearanceStates(name);

            stamper.AcroFields.SetField(name, value);
        }

        protected void FillFormFromXml(PdfStamper stamper, XmlDocument _doc, bool freeTextFlattening)
        {

            var arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
            var arialBaseFont = BaseFont.CreateFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


            var acroFields = stamper.AcroFields.Fields.ToList();


            stamper.AcroFields.AddSubstitutionFont(arialBaseFont);


            if (Debug)
            {
                foreach (KeyValuePair<string, AcroFields.Item> entry in acroFields)
                {
                    String[] values = stamper.AcroFields.GetAppearanceStates(entry.Key);

                    Log = Log + string.Format("acro_key: {0} type: {1} values {2} \n", entry.Key, entry.Value.GetType().ToString(), string.Join(";", values));
                }
            }

            foreach (XmlNode node in _doc.DocumentElement.ChildNodes)
            {
                string attr = node.Attributes["type"].InnerText;

                if (attr == "AcroFields")
                {
                    if (node.Attributes["name"] != null && node.Attributes["value"] != null)
                    {
                        if (Debug) Log = Log + string.Format("node: {0} value: {1} \n", node.Attributes["name"].InnerText, node.Attributes["value"].InnerText);

                        SetAcroField(stamper, node.Attributes["name"].InnerText, node.Attributes["value"].InnerText);
                    }
                }
            }

            stamper.FormFlattening = freeTextFlattening;
        }


        public void Export(string pdfPath, string xmlSettings, out MemoryStream outputPdfStream, bool formFlattening, bool freeTextFlattening)
        {
            outputPdfStream = new MemoryStream();

            using (Stream _inputPdfStream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {

                //Opens the unmodified PDF for reading
                var reader = new PdfReader(_inputPdfStream);
                //Creates a stamper to put an image on the original pdf
                var stamper = new PdfStamper(reader, outputPdfStream) { FormFlattening = formFlattening, FreeTextFlattening = freeTextFlattening };

                if (string.IsNullOrEmpty(xmlSettings)) xmlSettings = "<settings></settings>";
                LoadXmlConfiguration(xmlSettings);

                FillFormFromXml(stamper, _doc, freeTextFlattening);

                stamper.Writer.CloseStream = false;
                stamper.Close();
            }
        }

        public void LoadXmlConfiguration(string xml)
        {
            _doc = new XmlDocument();

            _doc.LoadXml(xml);
        }

        public bool MergePDFs(FileInfo[] fileNames, string targetPdf)
        {
            bool merged = true;
            using (var stream = new FileStream(targetPdf, FileMode.Create))
            {
                var document = new Document();
                var pdf = new PdfCopy(document, stream);
                PdfReader reader = null;
                try
                {
                    document.Open();
                    foreach (var file in fileNames)
                    {
                        reader = new PdfReader(file.FullName);
                        pdf.AddDocument(reader);
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    merged = false;
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    if (document != null)
                    {
                        document.Close();
                    }
                }
            }
            return merged;
        }

        public bool MergePDFs(List<string> fileNames, string targetPdf)
        {
            bool merged = true;
            using (var stream = new FileStream(targetPdf, FileMode.Create))
            {
                var document = new Document();
                var pdf = new PdfCopy(document, stream);
                PdfReader reader = null;
                try
                {
                    document.Open();
                    foreach (var file in fileNames)
                    {
                        reader = new PdfReader(file);
                        pdf.AddDocument(reader);
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    merged = false;
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    if (document != null)
                    {
                        document.Close();
                    }
                }
            }
            return merged;
        }
    }

}
