using BLL.Services;
using Common.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoupController : ControllerBase
    {
        //INJECTION DE LA BLL
        private readonly CoupService _coupService;

        public CoupController(CoupService coupService)
        {
            _coupService = coupService;
        }

        //AJOUTER UN COUP
        [HttpPost("add")]
        public async Task<IActionResult> AddCoup([FromBody] CoupSansId coup)
        {
            try
            {
                if (coup == null)
                {
                    return BadRequest(new { message = "Le corps de la requête est vide." });
                }

                int result = await _coupService.AddCoup(coup);
                return Ok(new {id = result, message = "Coup ajouté avec succès."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
            }
        }

        //RECUPERER LES COUP D UNE LIGNE

        [HttpGet("get")]
        public async Task<IActionResult> GetAllCoupsFromLigne(int ligneId)
        {
            try
            {
                if (ligneId <= 0)
                {
                    Console.WriteLine("ID de ligne invalide reçu: " + ligneId);
                    return BadRequest(new { message = "L'identifiant de ligne doit être un entier positif." });
                }

                List<CoupSansId> result = await _coupService.GetAllCoupsFromLigne(ligneId);

                if (result == null)
                {
                    return NotFound(new { message = "Aucun coup trouvé pour cette ligne." });
                }

                if (result[0] == null)
                {
                    return NotFound(new { message = "Aucun coup trouvé pour cette ligne." });
                }

                return Ok(new { result = result, message = "Coup récupéré avec succès" });
            }
            catch (ArgumentException ex) // Gestion des erreurs métier
            {
                return BadRequest(new { message = "Requête invalide.", details = ex.Message });
            }
            catch (SqlException ex) // Gestion des erreurs SQL
            {
                return StatusCode(500, new { message = "Erreur de base de données.", details = ex.Message });
            }
            catch (Exception ex) // Gestion des erreurs générales
            {
                return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
            }
        }
    }
}
