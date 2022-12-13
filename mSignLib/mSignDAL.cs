﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace mSignLib
{
    public class mSignDAL
    {
        private string _connString;
        private string _connStringDoc;

        public mSignDAL()
        {

            try
            {
                _connString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                _connStringDoc = ConfigurationManager.ConnectionStrings["connStringDoc"].ConnectionString;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Config error: {0}", ex.Message));
            }
        }

        public void Log(mSignLog log)
        {
            var spName = @"p_msign_log";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@url", log.Url);
                        cmd.Parameters.AddWithValue("@method", log.Method);
                        cmd.Parameters.AddWithValue("@request", log.Request);
                        cmd.Parameters.AddWithValue("@response", log.Response);
                        cmd.Parameters.AddWithValue("@result", log.Result);
                        cmd.Parameters.AddWithValue("@message", log.Message);
                        cmd.Parameters.AddWithValue("@duration", log.Duration);
                        cmd.Parameters.AddWithValue("@vir", log.Vir);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public DataTable GetZahteve()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_msign_zahteve", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public int CreateZahteva(mSignZahteva zahteva)
        {
            int id = -1;
            var spName = @"p_msign_zahteve_insert";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@opis", zahteva.Opis);
                        cmd.Parameters.AddWithValue("@datum", zahteva.Datum);
                        cmd.Parameters.AddWithValue("@status", zahteva.Status);
                        cmd.Parameters.AddWithValue("@doc_id", zahteva.docID);
                        cmd.Parameters.AddWithValue("@veza", zahteva.Veza);
                        cmd.Parameters.AddWithValue("@kljuc", zahteva.Kljuc);
                        cmd.Parameters.AddWithValue("@filePath", zahteva.FilePath);
                        cmd.Parameters.AddWithValue("@userId", zahteva.UserId);
                        id = (int)cmd.ExecuteScalar();
                    }
                }
            }

            return id;
        }

        public int CreateZahtevaKontakt(mSignZahteva zahteva)
        {
            int id = -1;
            var spName = @"p_msign_zahteve_insert";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@opis", zahteva.Opis);
                        cmd.Parameters.AddWithValue("@datum", zahteva.Datum);
                        cmd.Parameters.AddWithValue("@status", zahteva.Status);
                        cmd.Parameters.AddWithValue("@doc_id", zahteva.docID);
                        cmd.Parameters.AddWithValue("@veza", zahteva.Veza);
                        cmd.Parameters.AddWithValue("@kljuc", zahteva.Kljuc);
                        cmd.Parameters.AddWithValue("@filePath", zahteva.FilePath);
                        cmd.Parameters.AddWithValue("@telefon", zahteva.Telefon);
                        cmd.Parameters.AddWithValue("@email", zahteva.Email);
                        cmd.Parameters.AddWithValue("@userId", zahteva.UserId);
                        
                        id = (int)cmd.ExecuteScalar();
                    }
                }
            }

            return id;
        }
         

        public void UpdateZahteva(mSignZahteva zahteva)
        {

            var spName = @"p_msign_zahteve_update";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", zahteva.Id);
                        cmd.Parameters.AddWithValue("@Sifraobdelave", zahteva.SifraObdelave);
                        cmd.Parameters.AddWithValue("@opis", zahteva.Opis);
                        cmd.Parameters.AddWithValue("@datum", zahteva.Datum);
                        cmd.Parameters.AddWithValue("@status", zahteva.Status);
                        cmd.Parameters.AddWithValue("@msign_id", zahteva.mSignId);
                        cmd.Parameters.AddWithValue("@msign_documentId", zahteva.mSignDocumentId);
                        cmd.Parameters.AddWithValue("@msign_status", zahteva.mSignStatus);
                        cmd.Parameters.AddWithValue("@msign_msg", zahteva.mSignMsg);
                        cmd.Parameters.AddWithValue("@doc_id", zahteva.docID);
                        cmd.Parameters.AddWithValue("@doc_id_signed", zahteva.docIDSigned);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void UpdateMSignId(int id, int msignId)
        {
            var sql = @"update msign_zahteve set msign_id = @msignId where id = @id";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@msignId", msignId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void UpdateMSignShareDocument(int id, CreateSharedDocumentReturnDTO ret)
        {
            var sql = @"update msign_zahteve 
                            set msign_documentid = @msignDocumantId, 
                                msign_token = @msignToken, 
                                msign_expires = @msignExpires
                        where id = @id";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@msignDocumantId", ret.documentId);
                        cmd.Parameters.AddWithValue("@msignToken", ret.token);
                        cmd.Parameters.AddWithValue("@msignExpires", ret.expiresOn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // msign status
        public void UpdateMSignStatus(int id, int msignStatus, string msignMsg)
        {
            var sql = @"update msign_zahteve set msign_status = @msignStatus, msign_msg = @msignMsg where id = @id";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@msignStatus", msignStatus);
                        cmd.Parameters.AddWithValue("@msignMsg", msignMsg);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // procesni status
        public void UpdateZahtevaStatus(int id, int status, string msg)
        {
            var sql = @"update msign_zahteve set status = @status, opomba = @msg, spr_datetime = getdate() where id = @id";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@msg", msg);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // token za podpisovanje
        public string GetToken(int id)
        {
            string token = string.Empty;
            var sql = @"select msign_token from msign_zahteve where msign_id = @id";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            token = rdr.GetString(0);
                        }
                    }
                }
            }

            return token;
        }

        // pending documents
        public List<mSignZahteva> GetPendingDocs()
        {
            var zahteve = new List<mSignZahteva>();
            var sql = @"select id, msign_id, kljuc from msign_zahteve where status = 1 and isnull(msign_status, -1) <> 2 and msign_expires > GETDATE()";
            //var sql = @"select id, msign_id, kljuc from msign_zahteve where id = 703";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            zahteve.Add(new mSignZahteva() { Id = rdr.GetInt32(0), mSignId = rdr.GetInt32(1), Kljuc = rdr.GetInt32(2) });
                        }
                    }
                }
            }

            return zahteve;
        }

        // pending documents - custom
        public List<mSignZahteva> GetPendingDocsCustom()
        {
            var zahteve = new List<mSignZahteva>();
            var sql = "p_msign_zahteve_pending";
            //var sql = @"select id, msign_id, kljuc from msign_zahteve where id = 703";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            zahteve.Add(new mSignZahteva() { Id = rdr.GetInt32(0), mSignId = rdr.GetInt32(1), Kljuc = rdr.GetInt32(2) });
                        }
                    }
                }
            }

            return zahteve;
        }

        // pending documents
        public List<mSignZahteva> GetPendingDocs(string veza)
        {
            var zahteve = new List<mSignZahteva>();
            var sql = @"select id, msign_id, kljuc from msign_zahteve where veza = @veza and status = 1 and isnull(msign_status, -1) <> 2 and msign_expires > GETDATE()";
            //var sql = @"select id, msign_id, kljuc from msign_zahteve where id = 703";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@veza", veza);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            zahteve.Add(new mSignZahteva() { Id = rdr.GetInt32(0), mSignId = rdr.GetInt32(1), Kljuc = rdr.GetInt32(2) });
                        }
                    }
                }
            }

            return zahteve;
        }

        // DOC ID za zahtevo
        public int GetDocId(int id)
        {
            var docId = -1;
            var sql = @"select isnull(doc_id, -1) from msign_zahteve where id = @id";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            docId = rdr.GetInt32(0);
                        }
                    }
                }
            }

            return docId;
        }

        // Msign ID za povprasevanje
        public int GetMsignIdPovprasevanje(int sifraObdelave)
        {
            var msignId = -1;
            var sql = @"select top 1 isnull(msign_id, -1) from msign_zahteve where kljuc= @kljuc and veza ='web_PovprasevanjePrenos' order by id desc";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kljuc", sifraObdelave);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            msignId = rdr.GetInt32(0);
                        }
                    }
                }
            }

            return msignId;
        }

        // Msign ID za pogodbo
        public int GetMsignIdPogodba(int sifraObdelave)
        {
            var msignId = -1;
            var sql = @"select isnull(msign_id, -1) from msign_zahteve where kljuc= @kljuc and veza ='bis_pogodbe_gl'";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kljuc", sifraObdelave);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            msignId = rdr.GetInt32(0);
                        }
                    }
                }
            }

            return msignId;
        }



        // Msign ID za pogodbo
        public int GetMsignIdPogodbaLast(int sifraObdelave)
        {
            var msignId = -1;
            var sql = @"select top 1 isnull(msign_id, -1) from msign_zahteve where kljuc= @kljuc and veza ='bis_pogodbe_gl' and status = 1 order by msign_id desc";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kljuc", sifraObdelave);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            msignId = rdr.GetInt32(0);
                        }
                    }
                }
            }

            return msignId;
        }


        // DOC ID za zahtevo
        public DataTable GetPovprasevanjeMsignId(int msignId)
        {
            DataTable dt = new DataTable();
            var sql = @"select z.id, p.oznakaPogodbe, z.doc_id, p.sifraObdelave
                        from msign_zahteve z inner join web_PovprasevanjePrenos p on z.kljuc = p.Sifraobdelave and z.veza = 'web_PovprasevanjePrenos'
                        where z.msign_id = @msignId";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@msignId", msignId);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // DOC ID za zahtevo
        public DataTable GetPogodbaMsignId(int msignId)
        {
            DataTable dt = new DataTable();
            var sql = @"select z.id, p.oznaka_pogodbe, z.doc_id, p.stevilka_pogodbe
                        from msign_zahteve z inner join ebs_PogodbeKandidatiMenjavaPaketa p on z.kljuc = p.stevilka_pogodbe and z.veza = 'bis_pogodbe_gl'
                        where z.msign_id = @msignId";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@msignId", msignId);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // DOC ID za zahtevo
        public DataTable GetBisPogodbaMsignId(int msignId)
        {
            DataTable dt = new DataTable();
            var sql = @"select z.id, p.oznaka as oznakaPogodbe, z.doc_id, p.stevilka_pogodbe
                        from msign_zahteve z inner join bis_pogodbe_gl p on z.kljuc = p.stevilka_pogodbe and z.veza = 'bis_pogodbe_gl'
                        where z.msign_id = @msignId";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@msignId", msignId);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // procesni status
        public void UpdateZahtevaDocId(int id, int docId)
        {
            var sql = @"update msign_zahteve set doc_id = @docId, status = 2, spr_datetime = getdate() where id = @id";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@docId", docId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // procesni status
        public void AddPriloga(int idZahteve, int vrstaObrazca)
        {
            var sql = @"insert into msign_zahteve_priloge (id_zahteve, vrsta_obrazca) values (@id_zahteve, @vrsta_obrazca)";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_zahteve", idZahteve);
                    cmd.Parameters.AddWithValue("@vrsta_obrazca", vrstaObrazca);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Log(string method, string args, int userId, int id, int result, string message)
        {
            var spName = @"p_epodpis_log_insert";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@method", method);
                        cmd.Parameters.AddWithValue("@args", args);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@stevilka_pogodbe", id);
                        cmd.Parameters.AddWithValue("@result", result);
                        cmd.Parameters.AddWithValue("@message", message);                     
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Log(string method, string args, int userId, int id, int result, string message, int tip)
        {
            var spName = @"p_epodpis_log_insert";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@method", method);
                        cmd.Parameters.AddWithValue("@args", args);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@stevilka_pogodbe", id);
                        cmd.Parameters.AddWithValue("@result", result);
                        cmd.Parameters.AddWithValue("@message", message);
                        cmd.Parameters.AddWithValue("@tip", tip);                     
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Log(string method, string args, int userId, int id, int result, string message, int tip, string fileName)
        {
            var spName = @"p_epodpis_log_insert";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@method", method);
                        cmd.Parameters.AddWithValue("@args", args);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@stevilka_pogodbe", id);
                        cmd.Parameters.AddWithValue("@result", result);
                        cmd.Parameters.AddWithValue("@message", message);
                        cmd.Parameters.AddWithValue("@tip", tip);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

    }

    public class mSignLog
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int Result { get; set; }
        public string Message { get; set; }
        public string Vir { get; set; }
        public int Duration { get; set; }
        public int IdZahteva { get; set; }
    }

    public class mSignZahteva
    {
        public int Id { get; set; }
        public int SifraObdelave { get; set; }
        public string Opis { get; set; }
        public DateTime Datum { get; set; }
        public int Status { get; set; }
        public int mSignId { get; set; }
        public int mSignDocumentId { get; set; }
        public int mSignStatus { get; set; }
        public string mSignMsg { get; set; }
        public string FilePath { get; set; }
        public string FilePathSigned { get; set; }
        public int docIDSigned { get; set; }
        public int docID { get; set; }
        public string Veza { get; set; }
        public int Kljuc { get; set; }
        public int UserId { get; set; }
        public int RemoteSign { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
    }

    public class mSignZahtevaPriloga
    {
        public int Id { get; set; }
        public int IdZahteve { get; set; }
        public int VrstaObrazca { get; set; }
    }

    public enum LogType { Info = 0, Error = -1, Success = 1}
}
