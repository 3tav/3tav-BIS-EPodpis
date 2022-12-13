using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace mSignAgent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            string user, pass, exportId, exportArgs, xmlPath;
            var showUi = false;

            if (args.Length == 0)
            {
                Application.Run(new Form2());
                return;
            }
            
            var method = string.Empty;
            var parm = string.Empty;

            //try
            //{
            //    method = args[0];
            //    arg = args[1];

            //    var f = new Form2();
            //    f.InitService();
            //    f.DispatchMethod(method, arg);
            //}
            //catch (Exception ex)
            //{
            //    //Console.WriteLine(ex.Message, string.Format("Napaka zagonu metode {0}", method));
            //    try
            //    {
            //        File.WriteAllText(@"C:\\temp\\msign_log.txt", ex.Message);
            //    }
            //    catch (Exception ex2)
            //    { 
            //        //
            //    }                
            //}


            if (args.Length == 2)
            {
                try
                {
                    method = args[0];
                    parm = args[1];
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Napaka pri branju argumentov!");
                    return;
                }
            }
          
            var fp = new Form2();

            //MessageBox.Show(string.Format("INIT: {0}; {1}; {2}; {3}", server, database, exportId, exportArgs));

            try
            {
                fp.Init(method, parm);
            }
            catch (Exception ex)             
            { 
            
            }
            

            //showUi = false;
            if (showUi)
            {
                fp.ShowDialog();
            }
            else
            {
                //MessageBox.Show(exportId, exportArgs);
                try
                {
                    fp.DispatchMethod(method, parm);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


            /*
            string message = "OK";
           
            try
            {
                var svc = new mSignAgentLib();
                svc.Init();

                //File.AppendText(@"C:\\3TAV\mSignAgent\\log.txt", "2");
                svc.RefreshMsignStatus();
            }
            catch (Exception ex)
            {
                message = ex.Message;
                
            }
            File.WriteAllText(@"C:\\3TAV\mSignAgent\\log.txt", message);
            */ 
        }
    }
}
