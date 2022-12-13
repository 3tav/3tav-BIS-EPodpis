using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mSignAgent
{
    interface ImsignAgentLib
    {
        void Init();

        void Log(string method, string args, int userId, int id, int result, string message);
        void Log(string method, string args, int userId, int id, int result, string message, int tip, string fileName);

        void PreviewMerged(int stevilkaPogodbe);
        void PreviewMerged(int stevilkaPogodbe, bool flatten);
        void Merge(int stevilkaPogodbe, string datoteke, int userId);
        void Import(int stevilkaPogodbe, string filePath, int userId);


        void PogodbaPodpisi(int stevilkaPogodbe, bool remote, int userId);
        void PogodbaPodpisi(int stevilkaPogodbe, bool remote, int userId, string telefon, string email);
        void PogodbaPodpisi(int stevilkaPogodbe, bool remote, int userId, string filePath);

        void RefreshMsignStatusPogodbe(int stevilkaPogodbe, int userId);
        void RefreshMsignStatusPogodbe(int userId);
    }
}
