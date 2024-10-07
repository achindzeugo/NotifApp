using Microsoft.AspNetCore.Mvc;
using NotifApps.Models;
using System.Threading.Tasks;

namespace NotifApps.Controllers
{
    public class UtilisateursController : Controller
    {
        private readonly NotifdbContext _context;

        public UtilisateursController(NotifdbContext context)
        {
            _context = context;
        }

        // GET: Utilisateurs/Utilisateur
        public IActionResult Utilisateur()
        {
            return View();
        }

        // POST: Utilisateurs/Utilisateur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Utilisateur([Bind("Nom,Email,Service")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utilisateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirection vers l'index
            }
            return View(utilisateur);
        }

        // Méthode Index pour afficher la liste des utilisateurs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilisateurs.ToListAsync());
        }
    }
}
