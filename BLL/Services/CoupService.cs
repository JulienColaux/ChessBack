using Common.DTOs;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CoupService
    {
        //INJECTION DE LA DAL

        private readonly CoupRepo _coupRepo;

        public CoupService(CoupRepo coupRepo)
        {
            _coupRepo = coupRepo;
        }

        //AJOUTER UN COUP

        public async Task<int> AddCoup(CoupSansId coupSansId)
        {
            if (coupSansId == null)
            {
                throw new ArgumentNullException(nameof(coupSansId), "Aucun coup fourni.");
            }
            if(coupSansId.Ordre < 0)
            {
                throw new ArgumentException("L'ordre du coup doit être positif.");
            }
            if(string.IsNullOrEmpty(coupSansId.Depart) || string.IsNullOrEmpty(coupSansId.Arrive))
            {
                throw new ArgumentException("Le départ et l'arrivée ne peuvent pas être vides.");
            }
            if (coupSansId.Depart == coupSansId.Arrive)
            {
                throw new ArgumentException("Le départ et l'arrivée ne peuvent pas être identique.");
            }

            try
            {
                return await _coupRepo.AddCoup(coupSansId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans BLL - AddCoup: {ex.Message}");
                throw new Exception("Erreur interne lors de l'ajout du coup.", ex);
            }
        }

        //RECUPERER LES COUPS D UNE LIGNE

        public async Task<List<CoupSansId>> GetAllCoupsFromLigne(int ligneId)
        {
            try
            {
                return await _coupRepo.GetAllCoupsFromLigne(ligneId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur dans BLL - GetAllCoupsFromLigne: " + ex.ToString());
                throw new Exception("Erreur interne lors de la recupération des coups de la ligne.", ex);
            }
        }
    }
}
