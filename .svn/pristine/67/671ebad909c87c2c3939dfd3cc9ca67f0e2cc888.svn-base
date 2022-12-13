using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WacomSignProLib
{
    public class SignProService
    {
        private string _signProPath = "C:\\Program Files (x86)\\Wacom sign pro PDF\\Sign Pro PDF.exe";
        public void Init() { 
            
        }

        public void Sign(string fileName)
        {
            //var command = GetCommandJSON(fileName);

            // samo odpri program, ni licence za uporabo API
            Run(_signProPath); 

            
        }

       

        public void Run(string command) {          
            System.Diagnostics.Process.Start(_signProPath);            
        }

        public string GetCommandApiFile(string fileName, string apiFile) {
            var command = string.Format(@" -apiFile ""{0}""", apiFile);

            return command;
        }

        public string GetCommandJSON(string fileName)
        {
            var json = GetJSONString(fileName);
             
            var base64JSON = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));

            var command = string.Format(@" -api signpro:{0}", base64JSON);

            return command;
        }

        public string GetJSONString(string fileName) {
           // string output = JsonConvert.SerializeObject(product);

            var outputFile = fileName.Replace(".pdf", "_signpro.pdf");
            /*
            string output = string.Format(@"{
              ""file"": {
                ""input"": {
                  ""filesystem"": ""C:\\Users\\simon\\Desktop\\ece\\SignPro\\ece-903200-predogled.pdf"",
                  ""http_get"": null
                },
                ""output"": {
                  ""filesystem"": ""C:\\Users\\simon\\Desktop\\ece\\SignPro\\ece-903200-predogled_signpro.pdf"",
                  ""http_post"": null
                },
                ""authentication"": {
                  ""pdf_user_password"": ""pass"",
                  ""http_user"": null,
                  ""http_password"": null
                }
              },
              ""configuration"": {
                ""show_annotate"": false,
                ""show_manual_signature"": true,
                ""error_handler_url"": null,
                ""process_text_tags"": false
              }    
            }", fileName, outputFile);
            */

            string output = string.Format(@"{{
              ""file"": {{
                ""input"": {{
                  ""filesystem"": ""{0}"",
                  ""http_get"": null
                }},
                ""output"": {{
                  ""filesystem"": ""{1}"",
                  ""http_post"": null
                }},
                ""authentication"": {{
                  ""pdf_user_password"": ""pass"",
                  ""http_user"": null,
                  ""http_password"": null
                }}
              }},
              ""configuration"": {{
                ""show_annotate"": false,
                ""show_manual_signature"": true,
                ""error_handler_url"": null,
                ""process_text_tags"": false
              }}    
            }}", fileName, outputFile);

            return output;

        }

    }
}
