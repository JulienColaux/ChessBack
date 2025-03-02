using Common.DTOs;
using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CoupRepo
    {
        //INJECTION DU SERVICE DBHELPER

        private readonly DBHelper _dbHelper;
        public CoupRepo(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }


        //AJOUT D UN COUP

        public async Task<int>AddCoup( CoupSansId coup)
        {
            try
            {
                using(SqlConnection conn = _dbHelper.GetConnection())
                {
                    await conn.OpenAsync();

                    string query = "INSERT INTO Coup(depart, arrive, piece, ligne_id, parent_coup_id, ordre, description) OUTPUT INSERTED.id VALUES (@Depart, @Arrive, @Piece, @IdLigne, @ParentCoupId, @Ordre, @Description)";

                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Depart", coup.Depart);
                        cmd.Parameters.AddWithValue("@Arrive", coup.Arrive);
                        cmd.Parameters.AddWithValue("@Piece", coup.Piece);
                        cmd.Parameters.AddWithValue("@IdLigne", coup.IdLigne);
                        cmd.Parameters.AddWithValue("@ParentCoupId", (object) coup.ParentCoupId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Ordre", coup.Ordre);
                        cmd.Parameters.AddWithValue("@Description",(object) coup.Description ?? DBNull.Value);


                        int newId = (int)await cmd.ExecuteScalarAsync();
                        return newId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout du coup: {ex.Message}");
                return -1;
            }
        }



        //RECUPERER LES COUPS D UNE LIGNE

        public async Task<List<CoupSansId>> GetAllCoupsFromLigne(int ligneId)
        {
            try
            {
                List<CoupSansId> coups = new List<CoupSansId>();

                using (SqlConnection conn = _dbHelper.GetConnection())
                {
                    await conn.OpenAsync();

                    string query = "SELECT * FROM Coup WHERE ligne_id = @LigneId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LigneId", ligneId);

                        using(SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while(await reader.ReadAsync())
                            {
                                coups.Add(new CoupSansId
                                {
                                    Depart = reader["depart"] != DBNull.Value ? reader["depart"].ToString() : "Inconnu",
                                    Arrive = reader["arrive"] != DBNull.Value ? reader["arrive"].ToString() : "Inconnu",
                                    Piece = reader["piece"] != DBNull.Value && reader["piece"].ToString().Length > 0  ? reader["piece"].ToString()[0] : '?', //vu que c est un char il faut chopper uniquement la premiere lettre avec [0] et la valeur par default doit etre un seul charactère
                                    IdLigne = reader["ligne_id"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("ligne_id")) : -1,
                                    ParentCoupId = reader["parent_coup_id"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("parent_coup_id")) : -1,
                                    Ordre = reader["ordre"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("ordre")) : -1,
                                    Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : "Inconnu"
                                });
                            }
                        }

                    }
                return  coups;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur l'hors de la recupération des coups de la ligne.", ex);
            }
        }
    }
}
