using Common.DTOs;
using DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class LigneRepo
    {

        //INJECTION DU SERVICE DBHELPER

        private readonly DBHelper _dbHelper;

        public LigneRepo(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        //AJOUT D UNE LIGNE

        public async Task<int> AddLine(LigneSansId ligne)
        {
            try
            {
                using(SqlConnection conn = _dbHelper.GetConnection())
                {
                    await conn.OpenAsync();

                    string query = "INSERT INTO Ligne (nom, description) OUTPUT INSERTED.id VALUES (@Nom, @Description)";

                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nom", ligne.Name);
                        cmd.Parameters.AddWithValue("@Description", ligne.Description ?? (object)DBNull.Value);  // si description n'est pas nul, on lui donne description mais si cest null on lui donne DBNull.value

                        int newId = (int) await cmd.ExecuteScalarAsync();
                        return newId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout de la ligne: {ex.Message}");
                return -1;
            }
        }

        //RECUPERER TOUTE LES LIGNES

        public async Task<List<Ligne>> GetAllLignes()
        {
            try
            {
                using (SqlConnection conn = _dbHelper.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = "SELECT * FROM Ligne";

                    using( SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        List<Ligne> lignes = new List<Ligne>();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                lignes.Add(new Ligne
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Name = reader.GetString(reader.GetOrdinal("nom")),
                                    Description = reader.GetString(reader.GetOrdinal("description"))
                                });
                            }
                        }
                        return lignes;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Erreur au niveau de la récupération des données. (DAL)", ex);
            }
        }

    }
}
