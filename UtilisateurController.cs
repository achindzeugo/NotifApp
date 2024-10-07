using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotifApps.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NotifApps.Controllers
{
    public class GroupController : Controller
    {
        private readonly NotifdbContext _context;

        public GroupController(NotifdbContext context)
        {
            _context = context;
        }

        // GET: Group/Create
        public IActionResult GroupCreate()
        {
            var utilisateurs = _context.Utilisateurs.Select(u => new
            {
                u.UtilisateurId,
                u.Nom,
                u.Service
            }).ToList();

            ViewBag.Utilisateurs = new SelectList(utilisateurs, "UtilisateurId", "Nom");
            ViewBag.UtilisateursWithService = utilisateurs.Select(u => new SelectListItem
            {
                Value = u.UtilisateurId.ToString(),
                Text = u.Nom,
                Group = new SelectListGroup { Name = u.Service }
            }).ToList();

            return View();
        }

        // POST: Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupCreate([Bind("GroupNom,SelectedUtilisateurs")] Group group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(group);
                await _context.SaveChangesAsync();

                // Ajouter les utilisateurs sélectionnés au groupe
                foreach (var utilisateurId in group.SelectedUtilisateurs)
                {
                    var groupUtilisateur = new GroupUtilisateur
                    {
                        GroupId = group.GroupId,
                        UtilisateurId = utilisateurId
                    };
                    _context.GroupUtilisateurs.Add(groupUtilisateur);
                    Console.WriteLine($"Ajout de l'utilisateur {utilisateurId} au groupe {group.GroupId}");
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(GroupList));
            }

            var utilisateurs = _context.Utilisateurs.Select(u => new
            {
                u.UtilisateurId,
                u.Nom,
                u.Service
            }).ToList();

            ViewBag.Utilisateurs = new SelectList(utilisateurs, "UtilisateurId", "Nom");
            ViewBag.UtilisateursWithService = utilisateurs.Select(u => new SelectListItem
            {
                Value = u.UtilisateurId.ToString(),
                Text = u.Nom,
                Group = new SelectListGroup { Name = u.Service }
            }).ToList();

            return View(group);
        }

        // GET: Group
        public async Task<IActionResult> GroupList()
        {
            return View(await _context.Groups.ToListAsync());
        }

        // GET: Group/Edit/5
        public async Task<IActionResult> GroupEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.GroupUtilisateurs)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            var utilisateurs = _context.Utilisateurs.Select(u => new
            {
                u.UtilisateurId,
                u.Nom,
                u.Service
            }).ToList();

            ViewBag.Utilisateurs = new SelectList(utilisateurs, "UtilisateurId", "Nom");
            ViewBag.UtilisateursWithService = utilisateurs.Select(u => new SelectListItem
            {
                Value = u.UtilisateurId.ToString(),
                Text = u.Nom,
                Group = new SelectListGroup { Name = u.Service }
            }).ToList();

            group.SelectedUtilisateurs = group.GroupUtilisateurs.Select(gu => gu.UtilisateurId).ToList();

            return View(group);
        }

        // POST: Group/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupEdit(int id, [Bind("GroupId,GroupNom,SelectedUtilisateurs")] Group group)
        {
            if (id != group.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();

                    // Mettre à jour les utilisateurs du groupe
                    var existingGroupUtilisateurs = _context.GroupUtilisateurs.Where(gu => gu.GroupId == group.GroupId).ToList();
                    _context.GroupUtilisateurs.RemoveRange(existingGroupUtilisateurs);

                    foreach (var utilisateurId in group.SelectedUtilisateurs)
                    {
                        var groupUtilisateur = new GroupUtilisateur
                        {
                            GroupId = group.GroupId,
                            UtilisateurId = utilisateurId
                        };
                        _context.GroupUtilisateurs.Add(groupUtilisateur);
                        Console.WriteLine($"Ajout de l'utilisateur {utilisateurId} au groupe {group.GroupId}");
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.GroupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GroupList));
            }

            var utilisateurs = _context.Utilisateurs.Select(u => new
            {
                u.UtilisateurId,
                u.Nom,
                u.Service
            }).ToList();

            ViewBag.Utilisateurs = new SelectList(utilisateurs, "UtilisateurId", "Nom");
            ViewBag.UtilisateursWithService = utilisateurs.Select(u => new SelectListItem
            {
                Value = u.UtilisateurId.ToString(),
                Text = u.Nom,
                Group = new SelectListGroup { Name = u.Service }
            }).ToList();

            return View(group);
        }

        // GET: Group/Delete/5
        public async Task<IActionResult> GroupDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("GroupDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupDeleteConfirmed(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GroupList));
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }
    }
}
