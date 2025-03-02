using BLL.Services;
using Common.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LigneController : ControllerBase
    {
        //INJECTION DE LA BLL
        private readonly LigneService _ligneService;

        public LigneController(LigneService ligneService)
        {
            _ligneService = ligneService;
        }

        //AJOUTER UNE LIGNE
        [HttpPost("add")]

        public async Task<IActionResult> AddLigne([FromBody] LigneSansId ligne)
        {
            try
            {
                if (ligne == null)
                {
                    return BadRequest(new { message = "Le corps de la requête est vide." });
                }
                int result = await _ligneService.AddLigne(ligne);
                return Ok(new {id = result, message = "Ligne ajouté avec succès."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur interne du serveur", details = ex.Message });
            }
        }

        //RECUPERER TOUTE LES LIGNES
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Ligne>>> GetAllLignes()
        {
            try
            {
                List<Ligne> lignes = await _ligneService.GetAllLignes();
                return Ok(lignes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur interne du serveur (API)", details = ex.Message });
            }
        }
    }
}
