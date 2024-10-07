using Microsoft.AspNetCore.Mvc;
using NotifApps.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace NotifApps.Controllers
{
    public class UtilisateursController : Controller
    {
        private readonly NotifdbContext _context;

        public UtilisateursController(NotifdbContext context)
        {
            _context = context;
        }

        // GET: Utilisateurs/UtilisateurCreate 
        public IActionResult Utilisateur()
        {
            return View();
        }

        // POST: Utilisateurs/UtilisateurCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Utilisateur([Bind("Nom,Email,Service")] Utilisateur utilisateur)
        {
           
                _context.Add(utilisateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            return View(utilisateur);
        }
    }
}
