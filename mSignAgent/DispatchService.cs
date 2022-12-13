using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace mSignAgent
{
    
    public class DispatchService
    {
        private string _podjetje;
        public string Podjetje { get { return _podjetje; } }

       // Dictionary<string, IPerun3Service> _mapper;
        public void Init()
        {
            _podjetje = ConfigurationManager.AppSettings["Podjetje"].ToString();
        }

        public void Init(string podjetje)
        {
            _podjetje = podjetje;
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

            var svc = GetMsignService(_podjetje);
           
            try
            {
                svc.Init();
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
        private ImsignAgentLib GetMsignService(string podjetje) {
            ImsignAgentLib svc = null;

            if (podjetje == Podjetja.PLMB)
            {
                svc = new mSignAgentLibPLMB();
            }

            if (podjetje == Podjetja.ECE)
            {
                svc = new mSignAgentLibECE();
            }
            
            return svc;
        }
    }
}
