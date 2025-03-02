using Common.DTOs;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LigneService
    {
        //INJECTION DE LA DAL

        private readonly LigneRepo _ligneRepo;

        public LigneService(LigneRepo ligneRepo)
        {
            _ligneRepo = ligneRepo;
        }

        //AJOUTER UNE LIGNE

        public async Task<int> AddLigne(LigneSansId ligne)
        {
            if (ligne == null)
            {
                throw new ArgumentNullException(nameof(ligne), "Aucun coup fourni.");
            }

            try
            {
                return await _ligneRepo.AddLine(ligne);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout de la ligne : {ex.Message}");
                throw new Exception("Erreur interne lors de l'ajout de la ligne.", ex);
            }
        }

        //RECUPERER TOUTE LES LIGNES

        public async Task<List<Ligne>> GetAllLignes()
        {
            try
            {
                return await _ligneRepo.GetAllLignes();
            }
            catch(Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des données. (BLL)", ex);
            }
        }


    }
}
