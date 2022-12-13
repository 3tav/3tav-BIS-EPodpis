﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace mSignAgent
{
    public class DbPovprasevanja
    {
        private string _connString;
        private string _connStringDoc;

        public DbPovprasevanja()
            : this(ConfigurationManager.ConnectionStrings[ConnectionStrings.Default].ConnectionString, ConfigurationManager.ConnectionStrings[ConnectionStrings.Documents].ConnectionString)
        {
            // default konstruktor bere iz configa            
        }

        public DbPovprasevanja(string connString, string connStringDoc)
        {
            _connString = connString;
            _connStringDoc = connStringDoc;
        }

        public string GetConnectionString(){
            return _connString;
        }

        /*
        public int SavePovprasevanje(string oznakaPogodbe, int vrsta_osebe, string OIme, string OPriimek, )
        {
            int sifraObdelave = -1;
            
            using (var conn = new SqlConnection(_connString))
            {

                conn.Open();

                // v zadnji minuti pogodba ne sme biti vnešena (preprečitev podvojenih)
                var count = 0;
                var sql = @"select isnull(count(*),0)
                            from web_PoslovniPartner
                            where OznakaPogodbe = @oznakaPogodbe and
                                    DATEDIFF(second, VpisDatetime, getdate()) < 60";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("oznakaPogodbe", oznakaPogodbe);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (count == 0)
                {
                    var tran = conn.BeginTransaction();

                    //naseldnja sifra
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = "select max(isnull(sifraObdelave, 100000)) + 1 as stevec from web_PoslovniPartner";
                        sifraObdelave = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // insert web_PoslovniPartner
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                                ImePP,nazivPP,naziv1,
                                                                                naslovpp,krajpp,HisnaStevilka,
                                                                                postnastevilka,StatusDDV,DdvStevilka,
                                                                                UgodnostGorenje,UgodnostZaposleni, UserId,
                                                                                KodaProdajalca, OznakaPogodbe, PaketEnergija, 
                                                                                KolicinaOrion, KolicinaLuna, NacinPlacilaEn,
                                                                                NacinPlacilaLED, TipRacuna, SifraAgenta, 
                                                                                DatumSklepaPogodbe, PogodbaDatoteka, KolicinaProksima, 
                                                                                KolicinaSirius, KolicinaCoDetektor, KolicinaAkcije,
                                                                                Obrok1, Obrok2, Obrok3,
                                                                                NacinPlacilaCO, TipAkcijeBB, TipAkcijeDiners, 
                                                                                KolicinaBPEL, MeseciBPEL, KolicinaLumen,
                                                                                dovoliObvescanje, StatusPogodba, StatusPogodbaNepopolna,
                                                                                ZkIzpis, SoglasjeLastnika, SpremembaLastnika, 
                                                                                SpremembaOsebnihPodatkov)
                                            VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                    @Oime,@Opriimek,@Onaziv,
                                                    @Onaslov,@krajpp,@hs,
                                                    @kraj_id,@StatusDDV,@DDVstevilka,
                                                    @UgodnostGorenje,@UgodnostZaposleni, @UserId,
                                                    @KodaProdajalca, @oznakaPogodbe,@PaketEnergija, 
                                                    @KolicinaOrion, @KolicinaLuna, @NacinPlacilaEn,
                                                    @NacinPlacilaLED, @TipRacuna, @SifraAgenta, 
                                                    @DatumSklepaPogodbe, @PogodbaDatoteka, @KolicinaProksima, 
                                                    @KolicinaSirius, @KolicinaCoDetektor, @KolicinaAkcije,
                                                    @Obrok1, @Obrok2, @Obrok3,
                                                    @NacinPlacilaCO, @TipAkcijeBB, @TipAkcijeDiners, 
                                                    @KolicinaBPEL, @MeseciBPEL, @KolicinaLumen,
                                                    @dovoliObvescanje, @StatusPogodba, @StatusPogodbaNepopolna,
                                                    @ZkIzpis, @SoglasjeLastnika, @SpremembaLastnika, 
                                                    @SpremembaOsebnihPodatkov )";

                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                        cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = vrsta_osebe;
                        cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = OIme.Text.ToUpper();
                        cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = OPriimek.Text.ToUpper();
                        cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";//ONaziv.Text;
                        cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = ONaslov.Text.ToUpper();
                        cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = OKraj.Text.ToUpper();
                        cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = OHs.Text.ToUpper();
                        cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = Dlist_OPosta.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@StatusDDV", SqlDbType.Char, 1).Value = Zavezanec;
                        cmd.Parameters.Add("@DDVstevilka", SqlDbType.Char, 15).Value = PDavcna.Text.ToUpper();// ODavcna.Text;
                        cmd.Parameters.Add("@UgodnostGorenje", SqlDbType.NChar, 7).Value = ' ';// UgodnostGorenje.Text;
                        cmd.Parameters.Add("@UgodnostZaposleni", SqlDbType.NChar, 5).Value = "";// UgodnostZaposleni.Text;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = userID;
                        cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = sifraAkviziterja.ToUpper(); //kodaProdajalca;
                        cmd.Parameters.Add("@oznakaPogodbe", SqlDbType.VarChar, 50).Value = oznakaPogodbe.ToUpper();
                        cmd.Parameters.Add("@PaketEnergija", SqlDbType.Int).Value = DlistPaketiEnergija.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@KolicinaOrion", SqlDbType.Int).Value = kolicinaOrion;
                        cmd.Parameters.Add("@KolicinaLuna", SqlDbType.Int).Value = kolicinaLuna;
                        cmd.Parameters.Add("@KolicinaProksima", SqlDbType.Int).Value = kolicinaProksima;
                        cmd.Parameters.Add("@KolicinaCoDetektor", SqlDbType.Int).Value = kolicinaCoDetektor;
                        cmd.Parameters.Add("@KolicinaSirius", SqlDbType.Int).Value = kolicinaSirius;
                        cmd.Parameters.Add("@NacinPlacilaEn", SqlDbType.Int).Value = nacinPlacilaEn;
                        cmd.Parameters.Add("@NacinPlacilaLED", SqlDbType.Int).Value = nacinPlacilaLed;
                        cmd.Parameters.Add("@TipRacuna", SqlDbType.Int).Value = oblikaRacuna;
                        cmd.Parameters.Add("@SifraAgenta", SqlDbType.VarChar).Value = SifraProdAgenta.Text;
                        cmd.Parameters.Add("@PogodbaDatoteka", SqlDbType.VarChar).Value = fileName;
                        cmd.Parameters.Add("@KolicinaAkcije", SqlDbType.Int).Value = kolicinaAkcije;
                        cmd.Parameters.Add("@Obrok1", SqlDbType.Int).Value = obrok1;
                        cmd.Parameters.Add("@Obrok2", SqlDbType.Int).Value = obrok2;
                        cmd.Parameters.Add("@Obrok3", SqlDbType.Int).Value = obrok3;

                        cmd.Parameters.Add("@NacinPlacilaCO", SqlDbType.Int).Value = nacinPlacilaCO;
                        cmd.Parameters.Add("@TipAkcijeBB", SqlDbType.Int).Value = tipAkcijeBB;
                        cmd.Parameters.Add("@TipAkcijeDiners", SqlDbType.Int).Value = tipAkcijeDiners;

                        cmd.Parameters.Add("@KolicinaBPEL", SqlDbType.Int).Value = kolicinaBPEL;
                        cmd.Parameters.Add("@MeseciBPEL", SqlDbType.VarChar, 50).Value = meseciBPEL;

                        cmd.Parameters.Add("@KolicinaLumen", SqlDbType.Int).Value = kolicinaLumen;



                        if (datumSklPogodbe.HasValue)
                        {
                            cmd.Parameters.Add("@DatumSklepaPogodbe", SqlDbType.DateTime).Value = datumSklPogodbe;
                        }
                        else
                        {
                            cmd.Parameters.Add("@DatumSklepaPogodbe", SqlDbType.DateTime).Value = DBNull.Value;
                        }

                        cmd.Parameters.Add("@dovoliObvescanje", SqlDbType.Int).Value = dovoliObvescanje;
                        cmd.Parameters.Add("@statusPogodba", SqlDbType.Int).Value = statusPogodba;
                        cmd.Parameters.Add("@statusPogodbaNepopolna", SqlDbType.Int).Value = statusPogodbaNepopolna;
                        cmd.Parameters.Add("@zkIzpis", SqlDbType.Int).Value = zkIzpis;
                        cmd.Parameters.Add("@soglasjeLastnika", SqlDbType.Int).Value = soglasjeLastnika;
                        cmd.Parameters.Add("@spremembaLastnika", SqlDbType.Int).Value = spremembaLastnika;
                        cmd.Parameters.Add("@spremembaOsebnihPodatkov", SqlDbType.Int).Value = spremembaOsebnihPodatkov;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [10]", ex.Message));
                        }
                    }

                    //dodatno partner
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_PartnerDodatno(SifraObdelave,Placnik,Naslovnik,
                                                                                    Telefon,Eposta,PooblastiloPrekinitev,
                                                                                    PooblastiloZbiranje,ZamenjalDobavitelja,ImeKontakt,
                                                                                    PriimekKontakt,Opomba, userId)
                                                    VALUES (@SifraObdelave,@placnik,@naslovnik,
                                                            @telefon,@eposta,@PooblastiloPrekinitev,
                                                            @PooblastiloZbiranje,@ZamenjalDobavitelja,@ImeKontakt,
                                                            @PriimekKontakt,@opomba, @userId) ";
                        cmd.Parameters.Add("@placnik", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@naslovnik", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@telefon", SqlDbType.NChar, 30).Value = Dlist_TelS.Text + '/' + Telefon.Text;
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@eposta", SqlDbType.NChar, 30).Value = Eposta.Text;
                        cmd.Parameters.Add("@PooblastiloPrekinitev", SqlDbType.Int, 4).Value = PoobPrekinitev;
                        cmd.Parameters.Add("@PooblastiloZbiranje", SqlDbType.Int, 4).Value = PoobZbiranje;
                        cmd.Parameters.Add("@ZamenjalDobavitelja", SqlDbType.Int, 4).Value = 0;////DList_Zamenjal.SelectedItem.Value;
                        cmd.Parameters.Add("@ImeKontakt", SqlDbType.NChar, 30).Value = NIme.Text.ToUpper(); //ImeKontakt.Text.ToUpper();
                        cmd.Parameters.Add("@PriimekKontakt", SqlDbType.NChar, 30).Value = NPriimek.Text.ToUpper(); // PriimekKontakt.Text.ToUpper();
                        cmd.Parameters.Add("@Opomba", SqlDbType.NText).Value = Opomba.Value;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = userID;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [20]", ex.Message));
                        }
                    }

                    //plačnik in nosilec sta po novem ista 
                    //if (EnakPlacnik.Checked == false)
                    if (true)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                        ImePP,nazivPP,naziv1,
                                                                        naslovpp,krajpp,HisnaStevilka,
                                                                        postnastevilka,StatusDDV,DdvStevilka,
                                                                        userId, KodaProdajalca) 
                                        VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                @Oime,@Opriimek,@Onaziv,
                                                @Onaslov,@krajpp,@hs,
                                                @kraj_id,@StatusDDV,@DDVstevilka,
                                                @userId, @KodaProdajalca)";
                            cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                            cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                            cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 2;
                            cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = PIme.Text.ToUpper();
                            cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = PPriimek.Text.ToUpper();
                            cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";// PNaziv.Text;
                            cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = PNaslov.Text.ToUpper();
                            cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = PKraj.Text.ToUpper();
                            cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = PHs.Text.ToUpper();
                            cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = Dlist_PPosta.SelectedItem.Value.ToUpper();
                            cmd.Parameters.Add("@StatusDDV", SqlDbType.Char, 1).Value = ZavezanecP;
                            cmd.Parameters.Add("@DDVstevilka", SqlDbType.Char, 15).Value = PDavcna.Text.ToUpper();
                            cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = userID;
                            cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = kodaProdajalca.ToUpper();

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw new Exception(string.Format("{0} [30]", ex.Message));
                            }
                        }
                    }

                    //naslovnik
                    //if (EnakNaslovnik.Checked == false)
                    if (true)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                                    ImePP,nazivPP,naziv1,
                                                                                    naslovpp,krajpp,HisnaStevilka,
                                                                                    postnastevilka, userId, KodaProdajalca) 
                                                VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                        @Oime,@Opriimek,@Onaziv,
                                                        @Onaslov,@krajpp,@hs,
                                                        @kraj_id, @userId, @KodaProdajalca)";
                            cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                            cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                            cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 3;
                            cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = NIme.Text.ToUpper();
                            cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = NPriimek.Text.ToUpper();
                            cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";//NNaziv.Text;
                            cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = NNaslov.Text.ToUpper();
                            cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = NKraj.Text.ToUpper();
                            cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = NHs.Text.ToUpper();
                            cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = Dlist_NPosta.SelectedItem.Value.ToUpper();
                            cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = userID;
                            cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = kodaProdajalca.ToUpper();

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw new Exception(string.Format("{0} [40]", ex.Message));
                            }
                        }
                    }

                    //OM
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_OM(StevilkaOM,SifraObdelave,IdSodo,
                                                        IdDobaviteljEl,NazivOM,NaslovOM,
                                                        HisnaStevilka,PostnaStevilka,KrajOM,
                                                        Obracun,Poraba1,Poraba2,
                                                        ObracunskaMoc, UserId) 
                                        VALUES (@StevilkaOM,@SifraObdelave,@IdSodo,
                                                @IdDobaviteljEl,@nazivom,@naslovom,
                                                @hs,@posta,@krajom,
                                                @Obracun,@Poraba1,@Poraba2,
                                                @moc, @UserId)";
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@IdSodo", SqlDbType.Int, 4).Value = Dlist_Sodo.SelectedItem.Value;
                        cmd.Parameters.Add("@IdDobaviteljEl", SqlDbType.Int, 4).Value = Dlist_Dobavitelj.SelectedItem.Value;
                        cmd.Parameters.Add("@nazivom", SqlDbType.NChar, 100).Value = OmIme.Text.ToUpper();
                        cmd.Parameters.Add("@naslovom", SqlDbType.NChar, 100).Value = OmNaslov.Text.ToUpper();
                        cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = OmHS.Text.ToUpper();
                        cmd.Parameters.Add("@krajom", SqlDbType.NChar, 50).Value = OmKraj.Text.ToUpper();
                        cmd.Parameters.Add("@posta", SqlDbType.NChar, 10).Value = Dlist_OmPosta.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@Obracun", SqlDbType.Int, 4).Value = Dlist_Obracun.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@Poraba1", SqlDbType.NChar, 10).Value = string.Empty;
                        cmd.Parameters.Add("@Poraba2", SqlDbType.NChar, 10).Value = string.Empty;
                        cmd.Parameters.Add("@moc", SqlDbType.NChar, 10).Value = Dlist_Obracunskamoc.SelectedItem.Value;
                        cmd.Parameters.Add("@StevilkaOM", SqlDbType.NChar, 20).Value = StevilkaOM.Text;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = userID;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [50]", ex.Message));
                        }
                    }

                    tran.Commit();

                }
            }
        
            return sifraObdelave;
        
        }
        */
        public int InsertDoc(string name, string srcPath, string extension, byte[] data, int userId, string contentType)
        {
            int id = -1;
            using (var conn = new SqlConnection(_connStringDoc))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "p_doc_datoteke_insert";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ime", name);
                    cmd.Parameters.AddWithValue("@pot_izvor", srcPath);
                    cmd.Parameters.AddWithValue("@koncnica", extension);
                    cmd.Parameters.AddWithValue("@datoteka", data);
                    cmd.Parameters.AddWithValue("@vpis_uporabnik", userId);
                    cmd.Parameters.AddWithValue("@vir", "J");
                    cmd.Parameters.AddWithValue("@contentType", contentType);
                    id = (int)cmd.ExecuteScalar();
                }
            }

            return id;
        }


        public void UpdateDoc(int docId, string name, string srcPath, string extension, byte[] data, int userId, string contentType)
        {

            using (var conn = new SqlConnection(_connStringDoc))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "p_doc_datoteke_update";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@doc_id", docId);
                    cmd.Parameters.AddWithValue("@ime", name);
                    cmd.Parameters.AddWithValue("@pot_izvor", srcPath);
                    cmd.Parameters.AddWithValue("@koncnica", extension);
                    cmd.Parameters.AddWithValue("@datoteka", data);
                    cmd.Parameters.AddWithValue("@spr_uporabnik", userId);
                    cmd.Parameters.AddWithValue("@vir", "J");
                    cmd.Parameters.AddWithValue("@contentType", contentType);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertDocLink(string kljucTabela, string kljucPolje, int? kljucVrednost, int docId, string ime, string opis, string oznaka, int bisTipDokumenta, long vrsteObrazcev, int userId)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "p_ins_doc_datoteke_link";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@kljuc_tabela", kljucTabela);
                    cmd.Parameters.AddWithValue("@kljuc_polje", kljucPolje);
                    if (kljucVrednost.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@kljuc_vrednost", kljucVrednost.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@kljuc_vrednost", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("@doc_id", docId);
                    cmd.Parameters.AddWithValue("@ime", ime);
                    cmd.Parameters.AddWithValue("@opis", opis);
                    cmd.Parameters.AddWithValue("@bis_tip_dokumenta", bisTipDokumenta);
                    cmd.Parameters.AddWithValue("@vrste_obrazcev", vrsteObrazcev);
                    cmd.Parameters.AddWithValue("@vpis_uporabnik", userId);
                    cmd.Parameters.AddWithValue("@oznaka", oznaka);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDocLink(DocDatotekaLink l, int userId)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "p_upd_doc_datoteke_link";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@kljuc_tabela", l.KljucTabela);
                    cmd.Parameters.AddWithValue("@kljuc_polje", l.KljucPolje);
                    if (l.KljucVrednost.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@kljuc_vrednost", l.KljucVrednost);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@kljuc_vrednost", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("@doc_id", l.DocId);
                    cmd.Parameters.AddWithValue("@ime", l.Ime);
                    cmd.Parameters.AddWithValue("@opis", l.Opis);
                    cmd.Parameters.AddWithValue("@bis_tip_dokumenta", l.BisTip);
                    cmd.Parameters.AddWithValue("@vrste_obrazcev", l.VrsteObrazcev);
                    cmd.Parameters.AddWithValue("@spr_uporabnik", userId);
                    cmd.Parameters.AddWithValue("@oznaka", l.Oznaka);
                    cmd.Parameters.AddWithValue("@link_id", l.LinkId);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void InsertDocLink(int? kljucVrednost, int docId, string ime, string opis, string oznaka, int bisTipDokumenta, long vrsteObrazcev, int userId)
        {
            InsertDocLink("web_PovprasevanjePrenos", "SifraObdelave", kljucVrednost, docId, ime, opis, oznaka, bisTipDokumenta, vrsteObrazcev, userId);
        }

        public void InsertDocLinkPogodba(int? kljucVrednost, int docId, string ime, string opis, string oznaka, int bisTipDokumenta, long vrsteObrazcev, int userId)
        {
            InsertDocLink("bis_pogodbe_gl", "stevilka_pogodbe", kljucVrednost, docId, ime, opis, oznaka, bisTipDokumenta, vrsteObrazcev, userId);
        }

        public void SaveDocLink(int sifraObdelave, int docId, int tipDokumenta, long vrste_obrazcev, int userId)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "p_ins_web_PovprasevanjePrenos_doc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                    cmd.Parameters.AddWithValue("@doc_id", docId);
                    cmd.Parameters.AddWithValue("@bis_tip_dokumenta", tipDokumenta);
                    cmd.Parameters.AddWithValue("@vrste_obrazcev", vrste_obrazcev);
                    cmd.Parameters.AddWithValue("@vpis_uporabnik", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Byte[] GetDocumentBytes(int docId)
        {
            Byte[] content = null;
            using (var conn = new SqlConnection(_connStringDoc))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "p_get_document_content";
                    cmd.Parameters.AddWithValue("@doc_id", docId);
                    content = cmd.ExecuteScalar() as byte[];
                }
            }
            return content;
        }

        public DocDatoteka GetDatoteka(int docId)
        {
            DocDatoteka doc = null;
            using (var conn = new SqlConnection(_connStringDoc))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "p_get_document_meta";
                    cmd.Parameters.AddWithValue("@doc_id", docId);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            doc = new DocDatoteka();
                            doc.Ime = rdr.GetString(0);
                            doc.Datoteka = (byte[])rdr[1];
                            doc.ContentType = rdr.GetString(2);
                        }
                    }
                }
            }
            return doc;
        }

        public DocDatotekaLink GetDatotekaLink(int linkId)
        {
            DocDatotekaLink l = null;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "p_get_doc_datoteke_link";
                    cmd.Parameters.AddWithValue("@link_id", linkId);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            l = new DocDatotekaLink();
                            l.LinkId = rdr.GetInt32(0);
                            l.Ime = rdr.GetString(1);
                            l.Oznaka = rdr.GetString(2);
                            l.Opis = rdr.GetString(3);
                            l.Status = rdr.GetInt32(4);
                            l.BisTip = rdr.GetInt32(5);
                            l.BisTipNaziv = rdr.GetString(6);
                            l.KljucTabela = rdr.GetString(7);

                            if (rdr.IsDBNull(8))
                            {

                            }
                            else
                            {
                                l.KljucVrednost = rdr.GetInt32(8);
                            }

                            l.VpisDatetime = rdr.GetDateTime(9);
                            l.VpisUporabnik = rdr.GetInt32(10);
                            l.VpisUporabnikNaziv = rdr.GetString(11);
                            l.DocId = rdr.GetInt32(12);
                            l.VrsteObrazcev = rdr.GetInt64(13);

                            l.Pog = rdr.GetInt32(14);
                            l.Rac = rdr.GetInt32(15);
                            l.O51 = rdr.GetInt32(16);
                            l.ODB = rdr.GetInt32(17);
                            l.OMP = rdr.GetInt32(18);
                            l.OEP = rdr.GetInt32(19);

                            l.O11 = rdr.GetInt32(20);
                            l.O21 = rdr.GetInt32(21);
                            l.O11_1 = rdr.GetInt32(22);
                            l.O11_1_21 = rdr.GetInt32(23);
                            l.O31 = rdr.GetInt32(24);
                            l.O71 = rdr.GetInt32(25);

                            l.ANE_ugod = rdr.GetInt32(26);
                            l.ANE_dod_produkt = rdr.GetInt32(27);
                            /*dodal LT 20180508*/
                            l.O10_2 = rdr.GetInt32(28);
                            l.O12_2 = rdr.GetInt32(29);
                            l.BB_ANE = rdr.GetInt32(30);
                            l.DI_ANE = rdr.GetInt32(31);
                            l.ZK = rdr.GetInt32(32);
                            l.SKL_DED = rdr.GetInt32(33);
                            l.POG_NAJ_KUP = rdr.GetInt32(34);
                            l.PRIM_ZAP = rdr.GetInt32(35);
                            l.ZAHT_MENJ_DOB = rdr.GetInt32(36);
                            l.POOB_POS_MER_POD = rdr.GetInt32(37);
                            l.V_LAST_ODK = rdr.GetInt32(38);
                            l.POOB = rdr.GetInt32(39);
                            l.V_SPR_JAK_OMEJ_PRIK = rdr.GetInt32(40);
                            l.V_SPR_POD = rdr.GetInt32(41);
                            l.DOK_LAST = rdr.GetInt32(42);
                            l.V_SPR_POD_OM_PLIN = rdr.GetInt32(43);
                            l.PR_DOL_MES_PAV = rdr.GetInt32(44);
                            l.V_SRAC = rdr.GetInt32(45);
                            l.PR_POG_DOD_BL = rdr.GetInt32(46);

                        }
                    }
                }
            }
            return l;
        }

        public void DeleteDatotekaLink(int linkId, int userId)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_del_doc_datoteke_link", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@linkId", linkId);
                    cmd.Parameters.AddWithValue("@spr_uporabnik", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDatotekaLinkNepovezani(int userId)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_del_doc_datoteke_link_nepovezani", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userid", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetDocSeznamUser(int? userId)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_doc_seznam", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (userId.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@userId", DBNull.Value);
                        }
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetDocSeznamPovprasevanje(int sifraObdelave)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_doc_seznam_povp", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetDocSeznamPogodba(int stevilkaPogodbe)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_doc_seznam_pogodba", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@stevilkaPogodbe", stevilkaPogodbe);
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetDocSeznamUserEdit(int? userId)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_doc_seznam_edit", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (userId.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@userId", DBNull.Value);
                        }
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetDocSeznamUserEdit(int? userId, DateTime? datumOd, DateTime? datumDo, string sifraAgenta, string stPogodbe, string podjetje)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_doc_seznam_edit", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (userId.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@userId", DBNull.Value);
                        }


                        var parmDatumOd = new SqlParameter("@datumOd", SqlDbType.DateTime);
                        if (datumOd.HasValue)
                        {
                            parmDatumOd.Value = datumOd.Value;
                        }
                        else
                        {
                            parmDatumOd.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(parmDatumOd);

                        var parmDatumDo = new SqlParameter("@datumDo", SqlDbType.DateTime);
                        if (datumDo.HasValue)
                        {
                            parmDatumDo.Value = datumDo.Value;
                        }
                        else
                        {
                            parmDatumDo.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(parmDatumDo);

                        var sifraAgentaParm = new SqlParameter("@stAgenta", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(sifraAgenta))
                        {
                            sifraAgentaParm.Value = sifraAgenta;
                        }
                        else
                        {
                            sifraAgentaParm.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(sifraAgentaParm);

                        var stPogodbeParm = new SqlParameter("@stPogodbe", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(stPogodbe))
                        {
                            stPogodbeParm.Value = stPogodbe;
                        }
                        else
                        {
                            stPogodbeParm.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(stPogodbeParm);

                        var podjetjeParm = new SqlParameter("@podjetje", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(podjetje))
                        {
                            podjetjeParm.Value = podjetje;
                        }
                        else
                        {
                            podjetjeParm.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(podjetjeParm);

                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
        public DataTable GetKupciDavcna(string davcna)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_podatki_davcna", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@davcna", davcna);
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetKupecDavcna(string davcna)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_podatki_davcna_zadnji", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@davcna", davcna);
                        cmd.Parameters.AddWithValue("@userId", DBNull.Value);
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetKupecDavcna(string davcna, int userId)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_podatki_davcna_zadnji", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@davcna", davcna);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }


        public int InsertPovprasevanje(WebPovprasevanje p)
        {
            // database operations
            int sifraObdelave = -1;

            using (var conn = new SqlConnection(_connString))
            {

                conn.Open();

                // v zadnji minuti pogodba ne sme biti vnešena (preprečitev podvojenih)
                var count = 0;
                var sql = @"select isnull(count(*),0)
                                from web_PoslovniPartner
                                where OznakaPogodbe = @oznakaPogodbe and
                                        DATEDIFF(second, VpisDatetime, getdate()) < 60";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("oznakaPogodbe", p.OznakaPogodbe);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (count == 0)
                {
                    var tran = conn.BeginTransaction();

                    //naseldnja sifra
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = "select max(isnull(sifraObdelave, 100000)) + 1 as stevec from web_PoslovniPartner";
                        sifraObdelave = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // insert web_PoslovniPartner
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                                    ImePP,nazivPP,naziv1,
                                                                                    naslovpp,krajpp,HisnaStevilka,
                                                                                    postnastevilka,StatusDDV,DdvStevilka,
                                                                                    UgodnostGorenje,UgodnostZaposleni, UserId,
                                                                                    KodaProdajalca, OznakaPogodbe, PaketEnergija, 
                                                                                    KolicinaOrion, KolicinaLuna, NacinPlacilaEn,
                                                                                    NacinPlacilaLED, TipRacuna, SifraAgenta, 
                                                                                    DatumSklepaPogodbe, PogodbaDatoteka, KolicinaProksima, 
                                                                                    KolicinaSirius, KolicinaCoDetektor, KolicinaAkcije,
                                                                                    Obrok1, Obrok2, Obrok3,
                                                                                    NacinPlacilaCO, TipAkcijeBB, TipAkcijeDiners, 
                                                                                    KolicinaBPEL, MeseciBPEL, KolicinaLumen, PromoKoda,
                                                                                    dovoliObvescanje, StatusPogodba, StatusPogodbaNepopolna,
                                                                                    ZkIzpis, SoglasjeLastnika, SpremembaLastnika, 
                                                                                    SpremembaOsebnihPodatkov, DopolnitevStanjeStevca, SoglasjePlacnika,
                                                                                    SoglasjeSolastnika, StatusDostave)
                                                VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                        @Oime,@Opriimek,@Onaziv,
                                                        @Onaslov,@krajpp,@hs,
                                                        @kraj_id,@StatusDDV,@DDVstevilka,
                                                        @UgodnostGorenje,@UgodnostZaposleni, @UserId,
                                                        @KodaProdajalca, @oznakaPogodbe,@PaketEnergija, 
                                                        @KolicinaOrion, @KolicinaLuna, @NacinPlacilaEn,
                                                        @NacinPlacilaLED, @TipRacuna, @SifraAgenta, 
                                                        @DatumSklepaPogodbe, @PogodbaDatoteka, @KolicinaProksima, 
                                                        @KolicinaSirius, @KolicinaCoDetektor, @KolicinaAkcije,
                                                        @Obrok1, @Obrok2, @Obrok3,
                                                        @NacinPlacilaCO, @TipAkcijeBB, @TipAkcijeDiners, 
                                                        @KolicinaBPEL, @MeseciBPEL, @KolicinaLumen, @PromoKoda,
                                                        @dovoliObvescanje, @StatusPogodba, @StatusPogodbaNepopolna,
                                                        @ZkIzpis, @SoglasjeLastnika, @SpremembaLastnika, 
                                                        @SpremembaOsebnihPodatkov, @DopolnitevStanjeStevca, @SoglasjePlacnika,
                                                        @SoglasjeSolastnika, @StatusDostave )";

                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                        cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 1; // p.VrstaOsebe;
                        cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = p.Ime;// OIme.Text.ToUpper();
                        cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = p.Priimek; // OPriimek.Text.ToUpper();
                        cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";//ONaziv.Text;
                        cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = p.Naslov;// ONaslov.Text.ToUpper();
                        cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = p.Kraj;// OKraj.Text.ToUpper();
                        cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.Hs; // OHs.Text.ToUpper();
                        cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = p.Posta;// Dlist_OPosta.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@StatusDDV", SqlDbType.Char, 1).Value = p.Zavezanec; //zavezanec
                        cmd.Parameters.Add("@DDVstevilka", SqlDbType.Char, 15).Value = p.PDavcna; // PDavcna.Text.ToUpper();// ODavcna.Text;
                        cmd.Parameters.Add("@UgodnostGorenje", SqlDbType.NChar, 7).Value = string.Empty;// UgodnostGorenje.Text;
                        cmd.Parameters.Add("@UgodnostZaposleni", SqlDbType.NChar, 5).Value = string.Empty;// UgodnostZaposleni.Text;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;
                        cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = p.SifraAgenta; //sifraAkviziterja.ToUpper(); //kodaProdajalca;
                        cmd.Parameters.Add("@oznakaPogodbe", SqlDbType.VarChar, 50).Value = p.OznakaPogodbe;// oznakaPogodbe.ToUpper();
                        cmd.Parameters.Add("@PaketEnergija", SqlDbType.Int).Value = p.PaketEnergija;// DlistPaketiEnergija.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@KolicinaOrion", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaOrion;
                        cmd.Parameters.Add("@KolicinaLuna", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaLuna;
                        cmd.Parameters.Add("@KolicinaProksima", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaProksima;
                        cmd.Parameters.Add("@KolicinaCoDetektor", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaCODetektor;
                        cmd.Parameters.Add("@KolicinaSirius", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaSirius;
                        cmd.Parameters.Add("@NacinPlacilaEn", SqlDbType.Int).Value = p.NacinPlacilaEn;
                        cmd.Parameters.Add("@NacinPlacilaLED", SqlDbType.Int).Value = p.NacinPlacilaLED;// nacinPlacilaLed;
                        cmd.Parameters.Add("@TipRacuna", SqlDbType.Int).Value = p.OblikaRacuna;
                        cmd.Parameters.Add("@SifraAgenta", SqlDbType.VarChar).Value = p.SifraAgenta; // SifraProdAgenta.Text;
                        cmd.Parameters.Add("@PogodbaDatoteka", SqlDbType.VarChar).Value = string.Empty; // fileName;
                        cmd.Parameters.Add("@KolicinaAkcije", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaAkcije;
                        cmd.Parameters.Add("@Obrok1", SqlDbType.Int).Value = p.DodatneStoritve.Obrok1;
                        cmd.Parameters.Add("@Obrok2", SqlDbType.Int).Value = p.DodatneStoritve.Obrok2;
                        cmd.Parameters.Add("@Obrok3", SqlDbType.Int).Value = p.DodatneStoritve.Obrok3;

                        cmd.Parameters.Add("@NacinPlacilaCO", SqlDbType.Int).Value = p.DodatneStoritve.NacinPlacilaCO;
                        cmd.Parameters.Add("@TipAkcijeBB", SqlDbType.Int).Value = p.DodatneStoritve.TipAkcijeBB;
                        cmd.Parameters.Add("@TipAkcijeDiners", SqlDbType.Int).Value = p.DodatneStoritve.TipAkcijeDiners;

                        cmd.Parameters.Add("@KolicinaBPEL", SqlDbType.Int).Value = p.DodatneStoritve.BPELKolicina;
                        cmd.Parameters.Add("@MeseciBPEL", SqlDbType.VarChar, 50).Value = p.DodatneStoritve.BPELMeseci;

                        cmd.Parameters.Add("@KolicinaLumen", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaLumen;
                        cmd.Parameters.Add("@PromoKoda", SqlDbType.VarChar, 50).Value = p.DodatneStoritve.PromoKoda;

                        cmd.Parameters.Add("@StatusDostave", SqlDbType.VarChar, 50).Value = p.DodatneStoritve.StatusDostave;

                        if (p.DatumSklPogodbe.HasValue)
                        {
                            cmd.Parameters.Add("@DatumSklepaPogodbe", SqlDbType.DateTime).Value = p.DatumSklPogodbe.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@DatumSklepaPogodbe", SqlDbType.DateTime).Value = DBNull.Value;
                        }

                        cmd.Parameters.Add("@dovoliObvescanje", SqlDbType.Int).Value = p.DovoliObvescanje;
                        cmd.Parameters.Add("@statusPogodba", SqlDbType.Int).Value = p.StatusPogodbe;
                        cmd.Parameters.Add("@statusPogodbaNepopolna", SqlDbType.Int).Value = p.StatusPogodbaNepopolna;
                        cmd.Parameters.Add("@zkIzpis", SqlDbType.Int).Value = p.DopolnitevZkIzpis;
                        cmd.Parameters.Add("@soglasjeLastnika", SqlDbType.Int).Value = p.DopolnitevSoglasjeLastnika;
                        cmd.Parameters.Add("@spremembaLastnika", SqlDbType.Int).Value = p.DopolnitevSpremembaLastnika;
                        cmd.Parameters.Add("@spremembaOsebnihPodatkov", SqlDbType.Int).Value = p.DopolnitevSpremembaOsebnihPodatkov;

                        //dodal BA 16.3.2017
                        cmd.Parameters.Add("@DopolnitevStanjeStevca", SqlDbType.Int).Value = p.DopolnitevStanjeStevca;
                        cmd.Parameters.Add("@SoglasjePlacnika", SqlDbType.Int).Value = p.DopolnitevSoglasjePlacnika;
                        cmd.Parameters.Add("@SoglasjeSolastnika", SqlDbType.Int).Value = p.DopolnitevSoglasjeSolastnika;


                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [10]", ex.Message));
                        }
                    }

                    //dodatno partner
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_PartnerDodatno(SifraObdelave,Placnik,Naslovnik,
                                                                                        Telefon,Eposta,PooblastiloPrekinitev,
                                                                                        PooblastiloZbiranje,ZamenjalDobavitelja,ImeKontakt,
                                                                                        PriimekKontakt,Opomba, userId)
                                                        VALUES (@SifraObdelave,@placnik,@naslovnik,
                                                                @telefon,@eposta,@PooblastiloPrekinitev,
                                                                @PooblastiloZbiranje,@ZamenjalDobavitelja,@ImeKontakt,
                                                                @PriimekKontakt,@opomba, @userId) ";
                        cmd.Parameters.Add("@placnik", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@naslovnik", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@telefon", SqlDbType.NChar, 30).Value = p.Telefon; //Dlist_TelS.Text + '/' + Telefon.Text;
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@eposta", SqlDbType.NChar, 30).Value = p.Email;// Eposta.Text;
                        cmd.Parameters.Add("@PooblastiloPrekinitev", SqlDbType.Int, 4).Value = 1;
                        cmd.Parameters.Add("@PooblastiloZbiranje", SqlDbType.Int, 4).Value = 1;
                        cmd.Parameters.Add("@ZamenjalDobavitelja", SqlDbType.Int, 4).Value = 0;////DList_Zamenjal.SelectedItem.Value;
                        cmd.Parameters.Add("@ImeKontakt", SqlDbType.NChar, 30).Value = p.NIme;//NIme.Text.ToUpper(); //ImeKontakt.Text.ToUpper();
                        cmd.Parameters.Add("@PriimekKontakt", SqlDbType.NChar, 30).Value = p.NPriimek; // NPriimek.Text.ToUpper(); // PriimekKontakt.Text.ToUpper();
                        cmd.Parameters.Add("@Opomba", SqlDbType.NText).Value = p.Opomba;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [20]", ex.Message));
                        }
                    }

                    //plačnik in nosilec sta po novem ista 
                    //if (EnakPlacnik.Checked == false)
                    if (true)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                            ImePP,nazivPP,naziv1,
                                                                            naslovpp,krajpp,HisnaStevilka,
                                                                            postnastevilka,StatusDDV,DdvStevilka,
                                                                            userId, KodaProdajalca) 
                                            VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                    @Oime,@Opriimek,@Onaziv,
                                                    @Onaslov,@krajpp,@hs,
                                                    @kraj_id,@StatusDDV,@DDVstevilka,
                                                    @userId, @KodaProdajalca)";
                            cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                            cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                            cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 2;
                            cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = p.PIme;// PIme.Text.ToUpper();
                            cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = p.PPriimek; // PPriimek.Text.ToUpper();
                            cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";// PNaziv.Text;
                            cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = p.PNaslov; //PNaslov.Text.ToUpper();
                            cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = p.PKraj;// PKraj.Text.ToUpper();
                            cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.PHs; // PHs.Text.ToUpper();
                            cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = p.Posta;// Dlist_PPosta.SelectedItem.Value.ToUpper();
                            cmd.Parameters.Add("@StatusDDV", SqlDbType.Char, 1).Value = p.Zavezanec; //ZavezanecP;
                            cmd.Parameters.Add("@DDVstevilka", SqlDbType.Char, 15).Value = p.PDavcna;// PDavcna.Text.ToUpper();
                            cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;
                            cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = p.KodaProdajalca; // kodaProdajalca.ToUpper();

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw new Exception(string.Format("{0} [30]", ex.Message));
                            }
                        }
                    }

                    //naslovnik
                    //if (EnakNaslovnik.Checked == false)
                    if (true)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                                        ImePP,nazivPP,naziv1,
                                                                                        naslovpp,krajpp,HisnaStevilka,
                                                                                        postnastevilka, userId, KodaProdajalca) 
                                                    VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                            @Oime,@Opriimek,@Onaziv,
                                                            @Onaslov,@krajpp,@hs,
                                                            @kraj_id, @userId, @KodaProdajalca)";
                            cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                            cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                            cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 3;
                            cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = p.NIme; //NIme.Text.ToUpper();
                            cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = p.NPriimek; // NPriimek.Text.ToUpper();
                            cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";//NNaziv.Text;
                            cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = p.NNaslov; // NNaslov.Text.ToUpper();
                            cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = p.NKraj; // NKraj.Text.ToUpper();
                            cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.NHs;// NHs.Text.ToUpper();
                            cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = p.NPosta;// Dlist_NPosta.SelectedItem.Value.ToUpper();
                            cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;
                            cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = p.KodaProdajalca;// kodaProdajalca.ToUpper();

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw new Exception(string.Format("{0} [40]", ex.Message));
                            }
                        }
                    }

                    //OM
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_OM(StevilkaOM,SifraObdelave,IdSodo,
                                                            IdDobaviteljEl,NazivOM,NaslovOM,
                                                            HisnaStevilka,PostnaStevilka,KrajOM,
                                                            Obracun,Poraba1,Poraba2,
                                                            ObracunskaMoc, UserId) 
                                            VALUES (@StevilkaOM,@SifraObdelave,@IdSodo,
                                                    @IdDobaviteljEl,@nazivom,@naslovom,
                                                    @hs,@posta,@krajom,
                                                    @Obracun,@Poraba1,@Poraba2,
                                                    @moc, @UserId)";
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@IdSodo", SqlDbType.Int, 4).Value = p.IdSodo;// Dlist_Sodo.SelectedItem.Value;
                        cmd.Parameters.Add("@IdDobaviteljEl", SqlDbType.Int, 4).Value = p.Dobavitelj;// Dlist_Dobavitelj.SelectedItem.Value;
                        cmd.Parameters.Add("@nazivom", SqlDbType.NChar, 100).Value = p.OmIme;// OmIme.Text.ToUpper();
                        cmd.Parameters.Add("@naslovom", SqlDbType.NChar, 100).Value = p.OmNaslov; // OmNaslov.Text.ToUpper();
                        cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.OmHS; // OmHS.Text.ToUpper();
                        cmd.Parameters.Add("@krajom", SqlDbType.NChar, 50).Value = p.OmKraj; // OmKraj.Text.ToUpper();
                        cmd.Parameters.Add("@posta", SqlDbType.NChar, 10).Value = p.OmPosta; // Dlist_OmPosta.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@Obracun", SqlDbType.Int, 4).Value = p.TipObracuna; // Dlist_Obracun.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@Poraba1", SqlDbType.NChar, 10).Value = string.Empty;
                        cmd.Parameters.Add("@Poraba2", SqlDbType.NChar, 10).Value = string.Empty;
                        cmd.Parameters.Add("@moc", SqlDbType.NChar, 10).Value = p.Obracunskamoc;// Dlist_Obracunskamoc.SelectedItem.Value;
                        cmd.Parameters.Add("@StevilkaOM", SqlDbType.NChar, 20).Value = p.StevilkaOM;// StevilkaOM.Text;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId; // userID;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [50]", ex.Message));
                        }
                    }

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        //                            cmd.CommandText = @"update doc_datoteke_link set kljuc_vrednost = @SifraObdelave 
                        //                                                where zapis = @link_id";
                        //                            cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        //                            cmd.Parameters.Add("@link_id", SqlDbType.Int, 4).Value = GetSelectedKey(dgDatoteke);

                        cmd.CommandText = @"update doc_datoteke_link 
                                                set kljuc_vrednost = @SifraObdelave,
                                                    spr_datetime = getdate(),
                                                    spr_uporabnik = @userId,
                                                    status = 1     
                                                where oznaka = @oznaka
                                                and kljuc_vrednost is null";
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@oznaka", SqlDbType.VarChar, 100).Value = p.OznakaPogodbe;
                        cmd.Parameters.Add("@userId", SqlDbType.Int, 4).Value = p.UserId;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [60]", ex.Message));
                        }
                    }


                    tran.Commit();
                }
            }

            return sifraObdelave;


        }

        public int InsertPovprasevanje(WebPovprasevanje p, int leadId, string datoteke)
        {
            // database operations
            int sifraObdelave = -1;

            using (var conn = new SqlConnection(_connString))
            {

                conn.Open();

                // v zadnji minuti pogodba ne sme biti vnešena (preprečitev podvojenih)
                var count = 0;
                var sql = @"select isnull(count(*),0)
                                from web_PoslovniPartner
                                where OznakaPogodbe = @oznakaPogodbe and
                                        DATEDIFF(second, VpisDatetime, getdate()) < 60";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("oznakaPogodbe", p.OznakaPogodbe);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (count == 0)
                {
                    var tran = conn.BeginTransaction();

                    //naseldnja sifra
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = "select max(isnull(sifraObdelave, 100000)) + 1 as stevec from web_PoslovniPartner";
                        sifraObdelave = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // insert web_PoslovniPartner
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                                    ImePP,nazivPP,naziv1,
                                                                                    naslovpp,krajpp,HisnaStevilka,
                                                                                    postnastevilka,StatusDDV,DdvStevilka,
                                                                                    UgodnostGorenje,UgodnostZaposleni, UserId,
                                                                                    KodaProdajalca, OznakaPogodbe, PaketEnergija, 
                                                                                    KolicinaOrion, KolicinaLuna, NacinPlacilaEn,
                                                                                    NacinPlacilaLED, TipRacuna, SifraAgenta, 
                                                                                    DatumSklepaPogodbe, PogodbaDatoteka, KolicinaProksima, 
                                                                                    KolicinaSirius, KolicinaCoDetektor, KolicinaAkcije,
                                                                                    Obrok1, Obrok2, Obrok3,
                                                                                    NacinPlacilaCO, TipAkcijeBB, TipAkcijeDiners, 
                                                                                    KolicinaBPEL, MeseciBPEL, KolicinaLumen, PromoKoda,
                                                                                    dovoliObvescanje, StatusPogodba, StatusPogodbaNepopolna,
                                                                                    ZkIzpis, SoglasjeLastnika, SpremembaLastnika, 
                                                                                    SpremembaOsebnihPodatkov, DopolnitevStanjeStevca, SoglasjePlacnika,
                                                                                    SoglasjeSolastnika, StatusDostave, lead_id)
                                                VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                        @Oime,@Opriimek,@Onaziv,
                                                        @Onaslov,@krajpp,@hs,
                                                        @kraj_id,@StatusDDV,@DDVstevilka,
                                                        @UgodnostGorenje,@UgodnostZaposleni, @UserId,
                                                        @KodaProdajalca, @oznakaPogodbe,@PaketEnergija, 
                                                        @KolicinaOrion, @KolicinaLuna, @NacinPlacilaEn,
                                                        @NacinPlacilaLED, @TipRacuna, @SifraAgenta, 
                                                        @DatumSklepaPogodbe, @PogodbaDatoteka, @KolicinaProksima, 
                                                        @KolicinaSirius, @KolicinaCoDetektor, @KolicinaAkcije,
                                                        @Obrok1, @Obrok2, @Obrok3,
                                                        @NacinPlacilaCO, @TipAkcijeBB, @TipAkcijeDiners, 
                                                        @KolicinaBPEL, @MeseciBPEL, @KolicinaLumen, @PromoKoda,
                                                        @dovoliObvescanje, @StatusPogodba, @StatusPogodbaNepopolna,
                                                        @ZkIzpis, @SoglasjeLastnika, @SpremembaLastnika, 
                                                        @SpremembaOsebnihPodatkov, @DopolnitevStanjeStevca, @SoglasjePlacnika,
                                                        @SoglasjeSolastnika, @StatusDostave, @leadId )";

                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                        cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 1; // p.VrstaOsebe;
                        cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = p.Ime;// OIme.Text.ToUpper();
                        cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = p.Priimek; // OPriimek.Text.ToUpper();
                        cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";//ONaziv.Text;
                        cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = p.Naslov;// ONaslov.Text.ToUpper();
                        cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = p.Kraj;// OKraj.Text.ToUpper();
                        cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.Hs; // OHs.Text.ToUpper();
                        cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = p.Posta;// Dlist_OPosta.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@StatusDDV", SqlDbType.Char, 1).Value = p.Zavezanec; //zavezanec
                        cmd.Parameters.Add("@DDVstevilka", SqlDbType.Char, 15).Value = p.PDavcna; // PDavcna.Text.ToUpper();// ODavcna.Text;
                        cmd.Parameters.Add("@UgodnostGorenje", SqlDbType.NChar, 7).Value = string.Empty;// UgodnostGorenje.Text;
                        cmd.Parameters.Add("@UgodnostZaposleni", SqlDbType.NChar, 5).Value = string.Empty;// UgodnostZaposleni.Text;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;
                        cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = p.SifraAgenta; //sifraAkviziterja.ToUpper(); //kodaProdajalca;
                        cmd.Parameters.Add("@oznakaPogodbe", SqlDbType.VarChar, 50).Value = p.OznakaPogodbe;// oznakaPogodbe.ToUpper();
                        cmd.Parameters.Add("@PaketEnergija", SqlDbType.Int).Value = p.PaketEnergija;// DlistPaketiEnergija.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@KolicinaOrion", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaOrion;
                        cmd.Parameters.Add("@KolicinaLuna", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaLuna;
                        cmd.Parameters.Add("@KolicinaProksima", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaProksima;
                        cmd.Parameters.Add("@KolicinaCoDetektor", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaCODetektor;
                        cmd.Parameters.Add("@KolicinaSirius", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaSirius;
                        cmd.Parameters.Add("@NacinPlacilaEn", SqlDbType.Int).Value = p.NacinPlacilaEn;
                        cmd.Parameters.Add("@NacinPlacilaLED", SqlDbType.Int).Value = p.NacinPlacilaLED;// nacinPlacilaLed;
                        cmd.Parameters.Add("@TipRacuna", SqlDbType.Int).Value = p.OblikaRacuna;
                        cmd.Parameters.Add("@SifraAgenta", SqlDbType.VarChar).Value = p.SifraAgenta; // SifraProdAgenta.Text;
                        cmd.Parameters.Add("@PogodbaDatoteka", SqlDbType.VarChar).Value = string.Empty; // fileName;
                        cmd.Parameters.Add("@KolicinaAkcije", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaAkcije;
                        cmd.Parameters.Add("@Obrok1", SqlDbType.Int).Value = p.DodatneStoritve.Obrok1;
                        cmd.Parameters.Add("@Obrok2", SqlDbType.Int).Value = p.DodatneStoritve.Obrok2;
                        cmd.Parameters.Add("@Obrok3", SqlDbType.Int).Value = p.DodatneStoritve.Obrok3;

                        cmd.Parameters.Add("@NacinPlacilaCO", SqlDbType.Int).Value = p.DodatneStoritve.NacinPlacilaCO;
                        cmd.Parameters.Add("@TipAkcijeBB", SqlDbType.Int).Value = p.DodatneStoritve.TipAkcijeBB;
                        cmd.Parameters.Add("@TipAkcijeDiners", SqlDbType.Int).Value = p.DodatneStoritve.TipAkcijeDiners;

                        cmd.Parameters.Add("@KolicinaBPEL", SqlDbType.Int).Value = p.DodatneStoritve.BPELKolicina;
                        cmd.Parameters.Add("@MeseciBPEL", SqlDbType.VarChar, 50).Value = p.DodatneStoritve.BPELMeseci;

                        cmd.Parameters.Add("@KolicinaLumen", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaLumen;
                        cmd.Parameters.Add("@PromoKoda", SqlDbType.VarChar, 50).Value = p.DodatneStoritve.PromoKoda;

                        cmd.Parameters.Add("@StatusDostave", SqlDbType.VarChar, 50).Value = p.DodatneStoritve.StatusDostave;

                        if (p.DatumSklPogodbe.HasValue)
                        {
                            cmd.Parameters.Add("@DatumSklepaPogodbe", SqlDbType.DateTime).Value = p.DatumSklPogodbe.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@DatumSklepaPogodbe", SqlDbType.DateTime).Value = DBNull.Value;
                        }

                        cmd.Parameters.Add("@dovoliObvescanje", SqlDbType.Int).Value = p.DovoliObvescanje;
                        cmd.Parameters.Add("@statusPogodba", SqlDbType.Int).Value = p.StatusPogodbe;
                        cmd.Parameters.Add("@statusPogodbaNepopolna", SqlDbType.Int).Value = p.StatusPogodbaNepopolna;
                        cmd.Parameters.Add("@zkIzpis", SqlDbType.Int).Value = p.DopolnitevZkIzpis;
                        cmd.Parameters.Add("@soglasjeLastnika", SqlDbType.Int).Value = p.DopolnitevSoglasjeLastnika;
                        cmd.Parameters.Add("@spremembaLastnika", SqlDbType.Int).Value = p.DopolnitevSpremembaLastnika;
                        cmd.Parameters.Add("@spremembaOsebnihPodatkov", SqlDbType.Int).Value = p.DopolnitevSpremembaOsebnihPodatkov;

                        //dodal BA 16.3.2017
                        cmd.Parameters.Add("@DopolnitevStanjeStevca", SqlDbType.Int).Value = p.DopolnitevStanjeStevca;
                        cmd.Parameters.Add("@SoglasjePlacnika", SqlDbType.Int).Value = p.DopolnitevSoglasjePlacnika;
                        cmd.Parameters.Add("@SoglasjeSolastnika", SqlDbType.Int).Value = p.DopolnitevSoglasjeSolastnika;

                        cmd.Parameters.Add("@leadId", SqlDbType.Int).Value = leadId;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [10]", ex.Message));
                        }
                    }

                    //dodatno partner
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_PartnerDodatno(SifraObdelave,Placnik,Naslovnik,
                                                                                        Telefon,Eposta,PooblastiloPrekinitev,
                                                                                        PooblastiloZbiranje,ZamenjalDobavitelja,ImeKontakt,
                                                                                        PriimekKontakt,Opomba, userId)
                                                        VALUES (@SifraObdelave,@placnik,@naslovnik,
                                                                @telefon,@eposta,@PooblastiloPrekinitev,
                                                                @PooblastiloZbiranje,@ZamenjalDobavitelja,@ImeKontakt,
                                                                @PriimekKontakt,@opomba, @userId) ";
                        cmd.Parameters.Add("@placnik", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@naslovnik", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@telefon", SqlDbType.NChar, 30).Value = p.Telefon; //Dlist_TelS.Text + '/' + Telefon.Text;
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@eposta", SqlDbType.NChar, 30).Value = p.Email;// Eposta.Text;
                        cmd.Parameters.Add("@PooblastiloPrekinitev", SqlDbType.Int, 4).Value = 1;
                        cmd.Parameters.Add("@PooblastiloZbiranje", SqlDbType.Int, 4).Value = 1;
                        cmd.Parameters.Add("@ZamenjalDobavitelja", SqlDbType.Int, 4).Value = 0;////DList_Zamenjal.SelectedItem.Value;
                        cmd.Parameters.Add("@ImeKontakt", SqlDbType.NChar, 30).Value = p.NIme;//NIme.Text.ToUpper(); //ImeKontakt.Text.ToUpper();
                        cmd.Parameters.Add("@PriimekKontakt", SqlDbType.NChar, 30).Value = p.NPriimek; // NPriimek.Text.ToUpper(); // PriimekKontakt.Text.ToUpper();
                        cmd.Parameters.Add("@Opomba", SqlDbType.NText).Value = p.Opomba;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [20]", ex.Message));
                        }
                    }

                    //plačnik in nosilec sta po novem ista 
                    //if (EnakPlacnik.Checked == false)
                    if (true)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                            ImePP,nazivPP,naziv1,
                                                                            naslovpp,krajpp,HisnaStevilka,
                                                                            postnastevilka,StatusDDV,DdvStevilka,
                                                                            userId, KodaProdajalca) 
                                            VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                    @Oime,@Opriimek,@Onaziv,
                                                    @Onaslov,@krajpp,@hs,
                                                    @kraj_id,@StatusDDV,@DDVstevilka,
                                                    @userId, @KodaProdajalca)";
                            cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                            cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                            cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 2;
                            cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = p.PIme;// PIme.Text.ToUpper();
                            cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = p.PPriimek; // PPriimek.Text.ToUpper();
                            cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";// PNaziv.Text;
                            cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = p.PNaslov; //PNaslov.Text.ToUpper();
                            cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = p.PKraj;// PKraj.Text.ToUpper();
                            cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.PHs; // PHs.Text.ToUpper();
                            cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = p.Posta;// Dlist_PPosta.SelectedItem.Value.ToUpper();
                            cmd.Parameters.Add("@StatusDDV", SqlDbType.Char, 1).Value = p.Zavezanec; //ZavezanecP;
                            cmd.Parameters.Add("@DDVstevilka", SqlDbType.Char, 15).Value = p.PDavcna;// PDavcna.Text.ToUpper();
                            cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;
                            cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = p.KodaProdajalca; // kodaProdajalca.ToUpper();

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw new Exception(string.Format("{0} [30]", ex.Message));
                            }
                        }
                    }

                    //naslovnik
                    //if (EnakNaslovnik.Checked == false)
                    if (true)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                                        ImePP,nazivPP,naziv1,
                                                                                        naslovpp,krajpp,HisnaStevilka,
                                                                                        postnastevilka, userId, KodaProdajalca) 
                                                    VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                            @Oime,@Opriimek,@Onaziv,
                                                            @Onaslov,@krajpp,@hs,
                                                            @kraj_id, @userId, @KodaProdajalca)";
                            cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                            cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                            cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 3;
                            cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = p.NIme; //NIme.Text.ToUpper();
                            cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = p.NPriimek; // NPriimek.Text.ToUpper();
                            cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";//NNaziv.Text;
                            cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = p.NNaslov; // NNaslov.Text.ToUpper();
                            cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = p.NKraj; // NKraj.Text.ToUpper();
                            cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.NHs;// NHs.Text.ToUpper();
                            cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = p.NPosta;// Dlist_NPosta.SelectedItem.Value.ToUpper();
                            cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;
                            cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = p.KodaProdajalca;// kodaProdajalca.ToUpper();

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw new Exception(string.Format("{0} [40]", ex.Message));
                            }
                        }
                    }

                    //OM
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_OM(StevilkaOM,SifraObdelave,IdSodo,
                                                            IdDobaviteljEl,NazivOM,NaslovOM,
                                                            HisnaStevilka,PostnaStevilka,KrajOM,
                                                            Obracun,Poraba1,Poraba2,
                                                            ObracunskaMoc, UserId) 
                                            VALUES (@StevilkaOM,@SifraObdelave,@IdSodo,
                                                    @IdDobaviteljEl,@nazivom,@naslovom,
                                                    @hs,@posta,@krajom,
                                                    @Obracun,@Poraba1,@Poraba2,
                                                    @moc, @UserId)";
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@IdSodo", SqlDbType.Int, 4).Value = p.IdSodo;// Dlist_Sodo.SelectedItem.Value;
                        cmd.Parameters.Add("@IdDobaviteljEl", SqlDbType.Int, 4).Value = p.Dobavitelj;// Dlist_Dobavitelj.SelectedItem.Value;
                        cmd.Parameters.Add("@nazivom", SqlDbType.NChar, 100).Value = p.OmIme;// OmIme.Text.ToUpper();
                        cmd.Parameters.Add("@naslovom", SqlDbType.NChar, 100).Value = p.OmNaslov; // OmNaslov.Text.ToUpper();
                        cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.OmHS; // OmHS.Text.ToUpper();
                        cmd.Parameters.Add("@krajom", SqlDbType.NChar, 50).Value = p.OmKraj; // OmKraj.Text.ToUpper();
                        cmd.Parameters.Add("@posta", SqlDbType.NChar, 10).Value = p.OmPosta; // Dlist_OmPosta.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@Obracun", SqlDbType.Int, 4).Value = p.TipObracuna; // Dlist_Obracun.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@Poraba1", SqlDbType.NChar, 10).Value = string.Empty;
                        cmd.Parameters.Add("@Poraba2", SqlDbType.NChar, 10).Value = string.Empty;
                        cmd.Parameters.Add("@moc", SqlDbType.NChar, 10).Value = p.Obracunskamoc;// Dlist_Obracunskamoc.SelectedItem.Value;
                        cmd.Parameters.Add("@StevilkaOM", SqlDbType.NChar, 20).Value = p.StevilkaOM;// StevilkaOM.Text;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId; // userID;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [50]", ex.Message));
                        }
                    }

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        //                            cmd.CommandText = @"update doc_datoteke_link set kljuc_vrednost = @SifraObdelave 
                        //                                                where zapis = @link_id";
                        //                            cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        //                            cmd.Parameters.Add("@link_id", SqlDbType.Int, 4).Value = GetSelectedKey(dgDatoteke);

                        cmd.CommandText = string.Format(@"update doc_datoteke_link 
                                                set kljuc_vrednost = @SifraObdelave,
                                                    spr_datetime = getdate(),
                                                    spr_uporabnik = @userId,
                                                    status = 1     
                                                where zapis in ({0})
                                                and kljuc_vrednost is null", datoteke);
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@oznaka", SqlDbType.VarChar, 100).Value = p.OznakaPogodbe;
                        cmd.Parameters.Add("@userId", SqlDbType.Int, 4).Value = p.UserId;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(string.Format("{0} [60]", ex.Message));
                        }
                    }


                    tran.Commit();
                }
            }

            return sifraObdelave;


        }

        public int InsertPovprasevanje(WebPovprasevanje p, int leadId, string datoteke, SqlConnection conn, SqlTransaction tran)
        {
            // database operations
            int sifraObdelave = -1;


            int count = 0;
            // v zadnji minuti pogodba ne sme biti vnešena (preprečitev podvojenih)
            // se kontrolira v krovni proceduri
            //                var count = 0;
            //                var sql = @"select isnull(count(*),0)
            //                                from web_PoslovniPartner
            //                                where OznakaPogodbe = @oznakaPogodbe and
            //                                        DATEDIFF(second, VpisDatetime, getdate()) < 60";
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.CommandText = sql;
            //                    cmd.Parameters.AddWithValue("oznakaPogodbe", p.OznakaPogodbe);
            //                    count = Convert.ToInt32(cmd.ExecuteScalar());
            //                }


            if (count == 0)
            {

                //naseldnja sifra
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = "select max(isnull(sifraObdelave, 100000)) + 1 as stevec from web_PoslovniPartner";
                    sifraObdelave = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // insert web_PoslovniPartner
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                                    ImePP,nazivPP,naziv1,
                                                                                    naslovpp,krajpp,HisnaStevilka,
                                                                                    postnastevilka,StatusDDV,DdvStevilka,
                                                                                    UgodnostGorenje,UgodnostZaposleni, UserId,
                                                                                    KodaProdajalca, OznakaPogodbe, PaketEnergija, 
                                                                                    KolicinaOrion, KolicinaLuna, NacinPlacilaEn,
                                                                                    NacinPlacilaLED, TipRacuna, SifraAgenta, 
                                                                                    DatumSklepaPogodbe, PogodbaDatoteka, KolicinaProksima, 
                                                                                    KolicinaSirius, KolicinaCoDetektor, KolicinaAkcije,
                                                                                    Obrok1, Obrok2, Obrok3,
                                                                                    NacinPlacilaCO, TipAkcijeBB, TipAkcijeDiners, 
                                                                                    KolicinaBPEL, MeseciBPEL, KolicinaLumen, PromoKoda,
                                                                                    dovoliObvescanje, StatusPogodba, StatusPogodbaNepopolna,
                                                                                    ZkIzpis, SoglasjeLastnika, SpremembaLastnika, 
                                                                                    SpremembaOsebnihPodatkov, DopolnitevStanjeStevca, SoglasjePlacnika,
                                                                                    SoglasjeSolastnika, StatusDostave, lead_id)
                                                VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                        @Oime,@Opriimek,@Onaziv,
                                                        @Onaslov,@krajpp,@hs,
                                                        @kraj_id,@StatusDDV,@DDVstevilka,
                                                        @UgodnostGorenje,@UgodnostZaposleni, @UserId,
                                                        @KodaProdajalca, @oznakaPogodbe,@PaketEnergija, 
                                                        @KolicinaOrion, @KolicinaLuna, @NacinPlacilaEn,
                                                        @NacinPlacilaLED, @TipRacuna, @SifraAgenta, 
                                                        @DatumSklepaPogodbe, @PogodbaDatoteka, @KolicinaProksima, 
                                                        @KolicinaSirius, @KolicinaCoDetektor, @KolicinaAkcije,
                                                        @Obrok1, @Obrok2, @Obrok3,
                                                        @NacinPlacilaCO, @TipAkcijeBB, @TipAkcijeDiners, 
                                                        @KolicinaBPEL, @MeseciBPEL, @KolicinaLumen, @PromoKoda,
                                                        @dovoliObvescanje, @StatusPogodba, @StatusPogodbaNepopolna,
                                                        @ZkIzpis, @SoglasjeLastnika, @SpremembaLastnika, 
                                                        @SpremembaOsebnihPodatkov, @DopolnitevStanjeStevca, @SoglasjePlacnika,
                                                        @SoglasjeSolastnika, @StatusDostave, @leadId )";

                    cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                    cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                    cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 1; // p.VrstaOsebe;
                    cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = p.Ime;// OIme.Text.ToUpper();
                    cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = p.Priimek; // OPriimek.Text.ToUpper();
                    cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";//ONaziv.Text;
                    cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = p.Naslov;// ONaslov.Text.ToUpper();
                    cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = p.Kraj;// OKraj.Text.ToUpper();
                    cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.Hs; // OHs.Text.ToUpper();
                    cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = p.Posta;// Dlist_OPosta.SelectedItem.Value.ToUpper();
                    cmd.Parameters.Add("@StatusDDV", SqlDbType.Char, 1).Value = p.Zavezanec; //zavezanec
                    cmd.Parameters.Add("@DDVstevilka", SqlDbType.Char, 15).Value = p.PDavcna; // PDavcna.Text.ToUpper();// ODavcna.Text;
                    cmd.Parameters.Add("@UgodnostGorenje", SqlDbType.NChar, 7).Value = string.Empty;// UgodnostGorenje.Text;
                    cmd.Parameters.Add("@UgodnostZaposleni", SqlDbType.NChar, 5).Value = string.Empty;// UgodnostZaposleni.Text;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;
                    cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = p.SifraAgenta; //sifraAkviziterja.ToUpper(); //kodaProdajalca;
                    cmd.Parameters.Add("@oznakaPogodbe", SqlDbType.VarChar, 50).Value = p.OznakaPogodbe;// oznakaPogodbe.ToUpper();
                    cmd.Parameters.Add("@PaketEnergija", SqlDbType.Int).Value = p.PaketEnergija;// DlistPaketiEnergija.SelectedItem.Value.ToUpper();
                    cmd.Parameters.Add("@KolicinaOrion", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaOrion;
                    cmd.Parameters.Add("@KolicinaLuna", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaLuna;
                    cmd.Parameters.Add("@KolicinaProksima", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaProksima;
                    cmd.Parameters.Add("@KolicinaCoDetektor", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaCODetektor;
                    cmd.Parameters.Add("@KolicinaSirius", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaSirius;
                    cmd.Parameters.Add("@NacinPlacilaEn", SqlDbType.Int).Value = p.NacinPlacilaEn;
                    cmd.Parameters.Add("@NacinPlacilaLED", SqlDbType.Int).Value = p.NacinPlacilaLED;// nacinPlacilaLed;
                    cmd.Parameters.Add("@TipRacuna", SqlDbType.Int).Value = p.OblikaRacuna;
                    cmd.Parameters.Add("@SifraAgenta", SqlDbType.VarChar).Value = p.SifraAgenta; // SifraProdAgenta.Text;
                    cmd.Parameters.Add("@PogodbaDatoteka", SqlDbType.VarChar).Value = string.Empty; // fileName;
                    cmd.Parameters.Add("@KolicinaAkcije", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaAkcije;
                    cmd.Parameters.Add("@Obrok1", SqlDbType.Int).Value = p.DodatneStoritve.Obrok1;
                    cmd.Parameters.Add("@Obrok2", SqlDbType.Int).Value = p.DodatneStoritve.Obrok2;
                    cmd.Parameters.Add("@Obrok3", SqlDbType.Int).Value = p.DodatneStoritve.Obrok3;

                    cmd.Parameters.Add("@NacinPlacilaCO", SqlDbType.Int).Value = p.DodatneStoritve.NacinPlacilaCO;
                    cmd.Parameters.Add("@TipAkcijeBB", SqlDbType.Int).Value = p.DodatneStoritve.TipAkcijeBB;
                    cmd.Parameters.Add("@TipAkcijeDiners", SqlDbType.Int).Value = p.DodatneStoritve.TipAkcijeDiners;

                    cmd.Parameters.Add("@KolicinaBPEL", SqlDbType.Int).Value = p.DodatneStoritve.BPELKolicina;
                    cmd.Parameters.Add("@MeseciBPEL", SqlDbType.VarChar, 50).Value = p.DodatneStoritve.BPELMeseci;

                    cmd.Parameters.Add("@KolicinaLumen", SqlDbType.Int).Value = p.DodatneStoritve.KolicinaLumen;
                    cmd.Parameters.Add("@PromoKoda", SqlDbType.VarChar, 50).Value = p.DodatneStoritve.PromoKoda;

                    cmd.Parameters.Add("@StatusDostave", SqlDbType.VarChar, 50).Value = p.DodatneStoritve.StatusDostave;

                    if (p.DatumSklPogodbe.HasValue)
                    {
                        cmd.Parameters.Add("@DatumSklepaPogodbe", SqlDbType.DateTime).Value = p.DatumSklPogodbe.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("@DatumSklepaPogodbe", SqlDbType.DateTime).Value = DBNull.Value;
                    }

                    cmd.Parameters.Add("@dovoliObvescanje", SqlDbType.Int).Value = p.DovoliObvescanje;
                    cmd.Parameters.Add("@statusPogodba", SqlDbType.Int).Value = p.StatusPogodbe;
                    cmd.Parameters.Add("@statusPogodbaNepopolna", SqlDbType.Int).Value = p.StatusPogodbaNepopolna;
                    cmd.Parameters.Add("@zkIzpis", SqlDbType.Int).Value = p.DopolnitevZkIzpis;
                    cmd.Parameters.Add("@soglasjeLastnika", SqlDbType.Int).Value = p.DopolnitevSoglasjeLastnika;
                    cmd.Parameters.Add("@spremembaLastnika", SqlDbType.Int).Value = p.DopolnitevSpremembaLastnika;
                    cmd.Parameters.Add("@spremembaOsebnihPodatkov", SqlDbType.Int).Value = p.DopolnitevSpremembaOsebnihPodatkov;

                    //dodal BA 16.3.2017
                    cmd.Parameters.Add("@DopolnitevStanjeStevca", SqlDbType.Int).Value = p.DopolnitevStanjeStevca;
                    cmd.Parameters.Add("@SoglasjePlacnika", SqlDbType.Int).Value = p.DopolnitevSoglasjePlacnika;
                    cmd.Parameters.Add("@SoglasjeSolastnika", SqlDbType.Int).Value = p.DopolnitevSoglasjeSolastnika;

                    cmd.Parameters.Add("@leadId", SqlDbType.Int).Value = leadId;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw new Exception(string.Format("{0} [10]", ex.Message));
                    }
                }

                //dodatno partner
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = @"INSERT INTO web_PartnerDodatno(SifraObdelave,Placnik,Naslovnik,
                                                                                        Telefon,Eposta,PooblastiloPrekinitev,
                                                                                        PooblastiloZbiranje,ZamenjalDobavitelja,ImeKontakt,
                                                                                        PriimekKontakt,Opomba, userId)
                                                        VALUES (@SifraObdelave,@placnik,@naslovnik,
                                                                @telefon,@eposta,@PooblastiloPrekinitev,
                                                                @PooblastiloZbiranje,@ZamenjalDobavitelja,@ImeKontakt,
                                                                @PriimekKontakt,@opomba, @userId) ";
                    cmd.Parameters.Add("@placnik", SqlDbType.Int, 4).Value = sifraObdelave;
                    cmd.Parameters.Add("@naslovnik", SqlDbType.Int, 4).Value = sifraObdelave;
                    cmd.Parameters.Add("@telefon", SqlDbType.NChar, 30).Value = p.Telefon; //Dlist_TelS.Text + '/' + Telefon.Text;
                    cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                    cmd.Parameters.Add("@eposta", SqlDbType.NChar, 30).Value = p.Email;// Eposta.Text;
                    cmd.Parameters.Add("@PooblastiloPrekinitev", SqlDbType.Int, 4).Value = 1;
                    cmd.Parameters.Add("@PooblastiloZbiranje", SqlDbType.Int, 4).Value = 1;
                    cmd.Parameters.Add("@ZamenjalDobavitelja", SqlDbType.Int, 4).Value = 0;////DList_Zamenjal.SelectedItem.Value;
                    cmd.Parameters.Add("@ImeKontakt", SqlDbType.NChar, 30).Value = p.NIme;//NIme.Text.ToUpper(); //ImeKontakt.Text.ToUpper();
                    cmd.Parameters.Add("@PriimekKontakt", SqlDbType.NChar, 30).Value = p.NPriimek; // NPriimek.Text.ToUpper(); // PriimekKontakt.Text.ToUpper();
                    cmd.Parameters.Add("@Opomba", SqlDbType.NText).Value = p.Opomba;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw new Exception(string.Format("{0} [20]", ex.Message));
                    }
                }

                //plačnik in nosilec sta po novem ista 
                //if (EnakPlacnik.Checked == false)
                if (true)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                            ImePP,nazivPP,naziv1,
                                                                            naslovpp,krajpp,HisnaStevilka,
                                                                            postnastevilka,StatusDDV,DdvStevilka,
                                                                            userId, KodaProdajalca) 
                                            VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                    @Oime,@Opriimek,@Onaziv,
                                                    @Onaslov,@krajpp,@hs,
                                                    @kraj_id,@StatusDDV,@DDVstevilka,
                                                    @userId, @KodaProdajalca)";
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                        cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 2;
                        cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = p.PIme;// PIme.Text.ToUpper();
                        cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = p.PPriimek; // PPriimek.Text.ToUpper();
                        cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";// PNaziv.Text;
                        cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = p.PNaslov; //PNaslov.Text.ToUpper();
                        cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = p.PKraj;// PKraj.Text.ToUpper();
                        cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.PHs; // PHs.Text.ToUpper();
                        cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = p.Posta;// Dlist_PPosta.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@StatusDDV", SqlDbType.Char, 1).Value = p.Zavezanec; //ZavezanecP;
                        cmd.Parameters.Add("@DDVstevilka", SqlDbType.Char, 15).Value = p.PDavcna;// PDavcna.Text.ToUpper();
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;
                        cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = p.KodaProdajalca; // kodaProdajalca.ToUpper();

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(string.Format("{0} [30]", ex.Message));
                        }
                    }
                }

                //naslovnik
                //if (EnakNaslovnik.Checked == false)
                if (true)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"INSERT INTO web_PoslovniPartner(SifraObdelave,status,vrstaosebe,
                                                                                        ImePP,nazivPP,naziv1,
                                                                                        naslovpp,krajpp,HisnaStevilka,
                                                                                        postnastevilka, userId, KodaProdajalca) 
                                                    VALUES (@SifraObdelave,@status,@VrstaOsebe,
                                                            @Oime,@Opriimek,@Onaziv,
                                                            @Onaslov,@krajpp,@hs,
                                                            @kraj_id, @userId, @KodaProdajalca)";
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@status", SqlDbType.Int, 4).Value = 1;
                        cmd.Parameters.Add("@VrstaOsebe", SqlDbType.Int, 4).Value = 3;
                        cmd.Parameters.Add("@Oime", SqlDbType.NChar, 100).Value = p.NIme; //NIme.Text.ToUpper();
                        cmd.Parameters.Add("@Opriimek", SqlDbType.NChar, 100).Value = p.NPriimek; // NPriimek.Text.ToUpper();
                        cmd.Parameters.Add("@Onaziv", SqlDbType.NChar, 100).Value = "";//NNaziv.Text;
                        cmd.Parameters.Add("@Onaslov", SqlDbType.NChar, 100).Value = p.NNaslov; // NNaslov.Text.ToUpper();
                        cmd.Parameters.Add("@krajpp", SqlDbType.NChar, 50).Value = p.NKraj; // NKraj.Text.ToUpper();
                        cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.NHs;// NHs.Text.ToUpper();
                        cmd.Parameters.Add("@kraj_id", SqlDbType.NChar, 10).Value = p.NPosta;// Dlist_NPosta.SelectedItem.Value.ToUpper();
                        cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId;
                        cmd.Parameters.Add("@KodaProdajalca", SqlDbType.VarChar, 50).Value = p.KodaProdajalca;// kodaProdajalca.ToUpper();

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(string.Format("{0} [40]", ex.Message));
                        }
                    }
                }

                //OM
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = @"INSERT INTO web_OM(StevilkaOM,SifraObdelave,IdSodo,
                                                            IdDobaviteljEl,NazivOM,NaslovOM,
                                                            HisnaStevilka,PostnaStevilka,KrajOM,
                                                            Obracun,Poraba1,Poraba2,
                                                            ObracunskaMoc, UserId) 
                                            VALUES (@StevilkaOM,@SifraObdelave,@IdSodo,
                                                    @IdDobaviteljEl,@nazivom,@naslovom,
                                                    @hs,@posta,@krajom,
                                                    @Obracun,@Poraba1,@Poraba2,
                                                    @moc, @UserId)";
                    cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                    cmd.Parameters.Add("@IdSodo", SqlDbType.Int, 4).Value = p.IdSodo;// Dlist_Sodo.SelectedItem.Value;
                    cmd.Parameters.Add("@IdDobaviteljEl", SqlDbType.Int, 4).Value = p.Dobavitelj;// Dlist_Dobavitelj.SelectedItem.Value;
                    cmd.Parameters.Add("@nazivom", SqlDbType.NChar, 100).Value = p.OmIme;// OmIme.Text.ToUpper();
                    cmd.Parameters.Add("@naslovom", SqlDbType.NChar, 100).Value = p.OmNaslov; // OmNaslov.Text.ToUpper();
                    cmd.Parameters.Add("@hs", SqlDbType.NChar, 6).Value = p.OmHS; // OmHS.Text.ToUpper();
                    cmd.Parameters.Add("@krajom", SqlDbType.NChar, 50).Value = p.OmKraj; // OmKraj.Text.ToUpper();
                    cmd.Parameters.Add("@posta", SqlDbType.NChar, 10).Value = p.OmPosta; // Dlist_OmPosta.SelectedItem.Value.ToUpper();
                    cmd.Parameters.Add("@Obracun", SqlDbType.Int, 4).Value = p.TipObracuna; // Dlist_Obracun.SelectedItem.Value.ToUpper();
                    cmd.Parameters.Add("@Poraba1", SqlDbType.NChar, 10).Value = string.Empty;
                    cmd.Parameters.Add("@Poraba2", SqlDbType.NChar, 10).Value = string.Empty;
                    cmd.Parameters.Add("@moc", SqlDbType.NChar, 10).Value = p.Obracunskamoc;// Dlist_Obracunskamoc.SelectedItem.Value;
                    cmd.Parameters.Add("@StevilkaOM", SqlDbType.NChar, 20).Value = p.StevilkaOM;// StevilkaOM.Text;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = p.UserId; // userID;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw new Exception(string.Format("{0} [50]", ex.Message));
                    }
                }

                if (datoteke.Length > 0)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        //                            cmd.CommandText = @"update doc_datoteke_link set kljuc_vrednost = @SifraObdelave 
                        //                                                where zapis = @link_id";
                        //                            cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        //                            cmd.Parameters.Add("@link_id", SqlDbType.Int, 4).Value = GetSelectedKey(dgDatoteke);

                        cmd.CommandText = string.Format(@"update doc_datoteke_link 
                                                set kljuc_vrednost = @SifraObdelave,
                                                    spr_datetime = getdate(),
                                                    spr_uporabnik = @userId,
                                                    status = 1     
                                                where zapis in ({0})
                                                and kljuc_vrednost is null", datoteke);
                        cmd.Parameters.Add("@SifraObdelave", SqlDbType.Int, 4).Value = sifraObdelave;
                        cmd.Parameters.Add("@oznaka", SqlDbType.VarChar, 100).Value = p.OznakaPogodbe;
                        cmd.Parameters.Add("@userId", SqlDbType.Int, 4).Value = p.UserId;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(string.Format("{0} [60]", ex.Message));
                        }
                    }
                }

                //tran.Commit();
            }


            return sifraObdelave;


        }


        public bool CheckExternalId(string externalId)
        {
            int count = 0;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select isnull(count(*), 0) from ebs_Prijava where externalid = @externalId";
                    cmd.Parameters.AddWithValue("@externalId", externalId);
                    count = (Int32)cmd.ExecuteScalar();
                }
            }

            if (count > 0)
                return true;

            return false;

        }

        public bool CheckSifraAgenta(string sifraAgenta)
        {
            int count = 0;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select isnull(count(*), 0) from web_dsf_akviziterji where sifra_akviziterja = @sifraAgenta";
                    cmd.Parameters.AddWithValue("@sifraAgenta", sifraAgenta);
                    count = (Int32)cmd.ExecuteScalar();
                }
            }

            if (count > 0)
                return true;

            return false;

        }

        public bool CheckOznakaPogodbe(string oznakaPogodbe)
        {
            // kontrola če že obstaja         
            var count = 0;
            var sql = @"select isnull(count(*),0)
                        from web_PoslovniPartner
                        where OznakaPogodbe = @oznakaPogodbe";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("oznakaPogodbe", oznakaPogodbe);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            if (count > 0)
                return true;

            return false;
        }

        public bool CheckPermissionVnosDokumentovIndex(int userId)
        {
            return CheckPermission("VNOS_DOKUMENTOV", "INDEX_DOSTOP", userId);
        }

        public bool CheckPermission(string oznaka, string menuFunkcija, int userId)
        {

            var count = 0;

            try
            {
                var sql = @"select isnull(count(*),0)
                        from web_prog_tvdw_okna_pravice
                        where oznaka = @oznaka and
                              menu_funkcija = @menu_funkcija and
                              uporabnik = @uporabnik and
                              pravice = 1";
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("oznaka", oznaka);
                        cmd.Parameters.AddWithValue("menu_funkcija", menuFunkcija);
                        cmd.Parameters.AddWithValue("uporabnik", userId);
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }

            return (count > 0);
        }
        public DataSet GetSeznamPoste()
        {
            return GetDataSet(@"SELECT rtrim(postna_stevilka) as postna_stevilka, rtrim(postna_stevilka)+' '+rtrim(Kraj) as krajp FROM posta order by kraj");
        }

        public DataSet GetSeznamSodo()
        {

            return GetDataSet(@"SELECT IdSodo,cast(IdSodo as varchar(20)) + ' - ' + naziv as naziv FROM web_sodo");
        }

        public DataSet GetSeznamDobaviteljiEl()
        {
            return GetDataSet(@"SELECT IdDobaviteljEl,naziv FROM web_DobaviteljEl order  by naziv");
        }

        public DataSet GetSeznamSkrbnikiPogodb()
        {
            return GetDataSet(@"SELECT userid, naziv from web_skrbniki_pogodb order by naziv");
        }


        public DataSet GetSeznamTelSkupine()
        {
            return GetDataSet(@"SELECT id,sifra FROM web_TelSkupine");
        }

        public DataSet GetSeznamAkcijeElEn()
        {
            return GetDataSet(@"select -1 as id, '' as naziv union all select id, naziv from web_crm_akcije where tip = 0 and status = 0");
        }

        public DataSet GetSeznamAkviziterjiAktivni()
        {
            return GetDataSet(@"select sifra_akviziterja, naziv + ' ' + sifra_akviziterja  as naziv from web_dsf_akviziterji where status = 0");
        }

        public DataSet GetSeznamPlinCDK()
        {
            return GetDataSet(@"select OdjemnaSkupinaPlinId, Naziv from Bis_OdjemneSkupinePlin");
        }

        public DataTable GetVrsteObrazcev()
        {
            return GetDataTable(@"select SifraObrazca from web_ObrazciPdf");
        }

        public DataTable GetObrazecInfo(string sifraObrazca)
        {
            return GetDataTable(string.Format(@"select ID, SifraObrazca, Varianta, 
                                DokumentPdf, StoredProcedura, IzhodniPdf, 
                                status, prikaz, Energent, vrstni_red from web_ObrazciPdf where sifraObrazca = '{0}'", sifraObrazca));
        }

        public DataSet GetSeznamAkcijeElEn(string dsf)
        {
            return GetDataSet(string.Format(@" select -1 as id, '' as naziv, null as  dsf 
                                                union all 
                                                select a.id, a.naziv, d.dsf 
                                                from web_crm_akcije a inner join web_crm_akcije_dsf d on a.id = d.id_crm_akcije
                                                where a.tip = 0 and 
                                                a.status = 0    
                                                and d.dsf = '{0}' order by id", dsf));
        }

        public DataSet GetDobaviteljiEnergent(int vrstaEnergenta)
        {
            var ds = new DataSet();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("p_get_dobavitelji_en", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_energenta", vrstaEnergenta);
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }
        public DataTable GetDobaviteljiEnergentDt(int vrstaEnergenta)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("p_get_dobavitelji_en", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_energenta", vrstaEnergenta);
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }


        public DataSet GetPredmetiDelaKategorije()
        {
            return GetDataSetProcedure("p_get_predmeti_dela_kategorije");
        }

        public DataTable GetDobaviteljiEnergentPosta(int vrstaEnergenta, string posta)
        {
            var ds = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_dobavitelji_en_posta", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_energenta", vrstaEnergenta);
                        cmd.Parameters.AddWithValue("@posta", string.IsNullOrEmpty(posta) ? (object)DBNull.Value : posta);
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public DataTable GetPovprasevanjeKontakt(int sifraObdelave)
        {
            var ds = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_povprasevanje_kontakt", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public DataTable GetPovprasevanjeKontakt(int sifraObdelave, bool remote)
        {
            var ds = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_povprasevanje_kontakt", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                        cmd.Parameters.AddWithValue("@remote", (remote ? 1 : 0));
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public DataTable GetPogodbaKontakt(int stevilkaPogodbe, bool remote)
        {
            var ds = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_epodpis_get_pogodba_kontakt", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@stevilkaPogodbe", stevilkaPogodbe);
                        cmd.Parameters.AddWithValue("@remote", (remote ? 1 : 0));
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public DataTable GetPogodbaKontakt(int stevilkaPogodbe, bool remote, int userId)
        {
            var ds = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_epodpis_get_pogodba_kontakt", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@stevilkaPogodbe", stevilkaPogodbe);
                        cmd.Parameters.AddWithValue("@remote", (remote ? 1 : 0));
                        cmd.Parameters.AddWithValue("@userId", userId);
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public DataTable GetMsignZahtevaKontakt(int idZahteve)
        {
            var ds = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_epodpis_get_zahteva_kontakt", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idZahteve", idZahteve);                        
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public DataTable GetPovprasevanjeUI(int sifraObdelave)
        {
            var ds = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_povprasevanje_ui", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public int GetPovprasevanjePogodbaVarianta(int sifraObdelave)
        {
            int varianta = 1;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_povprasevanje_varianta", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                varianta = rdr.GetInt32(0);
                            }
                        }

                    }
                }
            }

            return varianta;
        }


       

        public int GetPovprasevanjeStatus(int sifraObdelave)
        {
            int status = 1;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("select status from web_PovprasevanjePrenos where Sifraobdelave = @sifraobdelave", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                status = rdr.GetInt32(0);
                            }
                        }
                    }
                }
            }

            return status;
        }

        public int GetPogodbaPodpisStatus(int stevilkaPogodbe)
        {
            int status = 1;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_epodpis_get_pogodba_status", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@stevilkaPogodbe", stevilkaPogodbe);
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                status = rdr.GetInt32(0);
                            }
                        }

                    }
                }
            }

            return status;
        }


        public string GetPogodbaOznakaOpis(string oznaka)
        {
            string opis = string.Empty;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_pogodba_opis", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@oznaka", oznaka);
                        opis = (string)cmd.ExecuteScalar();
                    }
                }
            }

            return opis;
        }


        public int GetDobaviteljEnergentPosta(int vrstaEnergenta, string posta)
        {
            int id = -1;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_dobavitelj_en_posta", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_energenta", vrstaEnergenta);
                        cmd.Parameters.AddWithValue("@posta", posta);
                        id = (int)cmd.ExecuteScalar();
                    }
                }
            }

            return id;
        }


        // procesni status
        public void UpdatePovprasevanjeStatus(int sifraObdelave, int status)
        {
            var sql = @"update web_PovprasevanjePrenos set status = @status, spr_datetime = getdate() where sifraObdelave = @sifraObdelave";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // procesni status
        public void UpdateDocLinkStatus(int docId, int status)
        {
            var sql = @"update doc_datoteke_link set status = @status, spr_datetime = getdate() where doc_id = @docId";
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@docId", docId);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataSet GetDataSet(string sql)
        {
            return GetDataSet(_connString, sql);
        }

        public DataSet GetDataSet(string connString, string sql)
        {
            var ds = new DataSet();
            using (var conn = new SqlConnection(_connString))
            {
                using (var cmd = new SqlDataAdapter(sql, conn))
                {
                    cmd.Fill(ds);
                }
            }
            return ds;
        }


        public DataTable GetDataTable(string sql)
        {
            return GetDataTable(_connString, sql);
        }

        public DataTable GetDataTable(string connString, string sql)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                using (var cmd = new SqlDataAdapter(sql, conn))
                {
                    cmd.Fill(dt);
                }
            }
            return dt;
        }


        public DataSet GetDataSetProcedure(string spName)
        {
            var ds = new DataSet();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public DataSet GetDataSetProcedure(string spName, string parmName, int parmValue)
        {
            var ds = new DataSet();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue(parmName, parmValue);
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public DataSet GetPovprasevanjaIndex(int userId)
        {
            var ds = new DataSet();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_povprasevanja_akviziter_zadnja", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userId", userId);
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public int GetObrazecVarianta(int sifraObdelave)
        {
            int varianta = 1;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("select isnull(obrazec_varianta, 1) from web_PovprasevanjePrenos where sifraObdelave = @sifraObdelave", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                        varianta = (int)cmd.ExecuteScalar();
                    }
                }
            }

            return varianta;
        }

        public DataTable GetPovprasevanjeObrazci(int sifraObdelave)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_get_povprasevanje_obrazci", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }


        public DataTable GetPogodbaObrazci(int stevilkaPogodbe)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_epodpis_get_pogodba_obrazci", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@stevilkaPogodbe", stevilkaPogodbe);
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public int GetObrazecSifra(string obrazecKoda, int varianta)
        {
            int id = -1;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("select isnull(id, -1) from web_obrazciPdf where sifraObrazca = @obrazecKoda and varianta = @varianta", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {                        
                        cmd.Parameters.AddWithValue("@obrazecKoda", obrazecKoda);
                        cmd.Parameters.AddWithValue("@varianta", varianta);
                        id = (Int32)cmd.ExecuteScalar();
                    }
                }
            }

            return id;
        }

        public int GetPogodbaVarianta(int stevilkaPogodbe)
        {
            int varianta = 1;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("p_epodpis_get_pogodba_varianta", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@stevilkaPogodbe", stevilkaPogodbe);
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                varianta = rdr.GetInt32(0);
                            }
                        }

                    }
                }
            }

            return varianta;
        }

        public int GetPovprasevanjeObrazciInt(int sifraObdelave)
        {
            var obrazci = 0;
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("select vrste_obrazcev from web_PovprasevanjePrenos where  sifraObdelave = @sifraObdelave", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@sifraObdelave", sifraObdelave);
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                obrazci = rdr.GetInt32(0);
                            }
                        }
                    }
                }
            }

            return obrazci;
        }

        public DateTime? GetPerunDatumPreklopa()
        {
            DateTime? datumPreklopa = null;

            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("select dbo.f_get_perun_datum_preklopa(getdate())", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                datumPreklopa = rdr.GetDateTime(0);
                            }
                        }
                    }
                }
            }

            return datumPreklopa;
        }
    }

    public enum BisDokumentiTipi { Pogodba = 1, Obrazci = 13, PogodbaPlin = 20, ObrazciPlin = 21 }
    public enum NivoDostopa { User = 10, SuperUser = 20, Admin = 50 }



    // user vidi samo svoje dokumente
    // superuser vidi svoje v okviru podjetja
    // admin vidi vse

    public class DocDatoteka
    {
        public int Id;
        public string Ime;
        public string Opis;
        public Byte[] Datoteka;
        public string ContentType;

    }

    public class DocDatotekaLink
    {
        public int LinkId;
        public int DocId;

        public string Ime;
        public string Opis;
        public int BisTip;
        public string BisTipNaziv;
        public long VrsteObrazcev;
        public string Oznaka;

        public int? KljucVrednost;
        public string KljucTabela = "web_poslovniPartner";
        public string KljucPolje = "SifraObdelave";

        public int Status;
        public int StatusNaziv;

        /* pogodbe */
        public int Pog;
        public int Rac;
        public int O51;
        public int ODB;
        public int OMP;
        public int OEP;
        public int ANE_ugod;
        public int ANE_dod_produkt;

        /* obrazci */
        public int O11;
        public int O21;
        public int O11_1;
        public int O11_1_21;
        public int O31;
        public int O71;
        public int O12_2;
        public int O10_2;
        /*Dodal LT*/
        public int BB_ANE;
        public int DI_ANE;
        public int ZK;
        public int SKL_DED;
        public int POG_NAJ_KUP;
        public int PRIM_ZAP;
        public int ZAHT_MENJ_DOB;
        public int POOB_POS_MER_POD;
        public int V_LAST_ODK;
        public int POOB;
        public int V_SPR_JAK_OMEJ_PRIK;
        public int V_SPR_POD;
        public int DOK_LAST;
        public int V_SPR_POD_OM_PLIN;
        public int PR_DOL_MES_PAV;
        public int V_SRAC;
        public int PR_POG_DOD_BL;

        public DateTime VpisDatetime;
        public int VpisUporabnik;
        public string VpisUporabnikNaziv;



    }

    public class WebPovprasevanje
    {

        public int SifraObdelave;
        public string OznakaPogodbe;

        public string OznakaPogodbeTakeover;

        public string Ime;
        public string Priimek;
        public string Naslov;
        public string Hs;
        public string Kraj;
        public string Posta;
        public string PostaKraj;
        public string Telefon;
        public string Email;
        public string Opomba;
        public string KodaProdajalca;
        public string SifraAgenta;
        public DateTime? DatumSklPogodbe;
        public DateTime? DatumDostPogodbe;

        public int UserId;
        public int EaStatus;
        public string PIme;
        public string PPriimek;
        public string PNaslov;
        public string PHs;
        public string PKraj;
        public string PDavcna;
        public string PPosta;
        public string PPostaKraj;

        public int VrstaOsebe;
        public int Zavezanec;
        public int StatusPogodbe;

        public string NIme;
        public string NPriimek;
        public string NNaslov;
        public string NHs;
        public string NKraj;
        public string NPosta;
        public string NPostaKraj;

        public string OmIme;
        public string OmPriimek;
        public string OmNaslov;
        public string OmHS;
        public string OmKraj;
        public string OmPostaKraj;
        public string OmPosta;

        public string StevilkaOM;
        public int? Dobavitelj;
        public int? Obracunskamoc;
        public int? TipObracuna;
        public int? IdSodo;
        public int? TelS;
        public int? VrstaStoritve;
        public bool? StatusPogodbaNepopolna;
        public bool? DopolnitevZkIzpis;
        public bool? DopolnitevSoglasjeLastnika;
        public bool? DopolnitevSpremembaLastnika;
        public bool? DopolnitevSpremembaOsebnihPodatkov;
        //dodal BA 16.3.2017
        public bool? DopolnitevSoglasjeSolastnika;
        public bool? DopolnitevSoglasjePlacnika;
        public bool? DopolnitevStanjeStevca;

        // Dodano Simon 12.6.2018
        public bool? DopolnitevBBAneks;
        public bool? DopolnitevDinersAneks;
        public bool? DopolnitevSoglasjeTrajnik;
        public int? DopolnitevSkrbnik;
        public DateTime? DopolnitevDatumPodpis;
        public string DopolnitevPridobitevPis;
        public string PredhodniPaket;
        public string PredhodniCenaVT;
        public string PredhodniCenaMT;
        public string PredhodniCenaET;
        public int ZadrzanaPogodba;
        public Int64 GDPRKanali;
        public string SEPAIban;
        public string Smm;
        public string SEPABankaNaziv;
        public string SEPABankaBic;

        public bool? DovoliObvescanje;
        public int? OblikaRacuna;
        public int? NacinPlacilaEn;
        public int? PaketEnergija;
        public int? NacinPlacilaLED;
        public int RazlogNeopravljen;

        public string TelefonPrefix
        {
            get
            {
                var prefix = string.Empty;
                if (Telefon.Length > 0)
                {
                    if (Telefon.IndexOf('/') > 0)
                    {
                        prefix = Telefon.Substring(0, Telefon.IndexOf('/'));
                    }
                }
                return prefix;
            }
        }

        public string TelefonSuffix
        {
            get
            {
                var suffix = string.Empty;
                if (Telefon.Length > 0)
                {
                    if (Telefon.IndexOf('/') > 0)
                    {

                        suffix = Telefon.Substring(Telefon.IndexOf('/') + 1, Telefon.Length - Telefon.IndexOf('/') - 1);
                    }
                }
                return suffix;
            }
        }

        public WebPovprasevanjeDod DodatneStoritve;

        public WebPovprasevanje()
        {
            DodatneStoritve = new WebPovprasevanjeDod();
        }

        public void KopirajPlacnik()
        {
            // placnik = nosilec
            this.PIme = this.Ime;
            this.PPriimek = this.Priimek;
            this.PNaslov = this.Naslov;
            this.PKraj = this.Kraj;
            this.PHs = this.Hs;
            this.PPosta = this.Posta;
        }


    }

    public class WebPovprasevanjeDod
    {
        public int KolicinaOrion;
        public int KolicinaLuna;
        public int KolicinaProksima;
        public int KolicinaSirius;
        public int KolicinaCODetektor;
        public int KolicinaAkcije;
        public int Obrok1;
        public int Obrok2;
        public int Obrok3;
        public int NacinPlacilaCO;
        public int NacinPlacilaLED;
        public int TipAkcijeBB;
        public int TipAkcijeDiners;
        public int BPELKolicina;
        public string BPELMeseci;
        public int KolicinaLumen;
        public string PromoKoda;
        public int StatusDostave;
    }
    public static class ConnectionStrings
    {
        public static string Default = "connString";
        public static string Application = "connStringApp";
        public static string Documents = "connStringDoc";
    }

    public enum Status { Preklic = -100, Storno = -1, Nova = 0, ZaPodpis = 1, Podpisan = 2, Nepopolna = 20, Zakljucen = 100, Prenesen = 200 }
}
