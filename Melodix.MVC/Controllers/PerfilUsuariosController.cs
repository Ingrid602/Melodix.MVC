﻿using Melodix.APIConsumer;
using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Melodix.MVC.Controllers
{
    public class PerfilUsuariosController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public PerfilUsuariosController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PerfilUsuarios
        public async Task<ActionResult> Index(string searchString)
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
                return RedirectToAction("Login", "Account");

            var lista = _context.PerfilUsuarios
          .Include(p => p.Usuario)
          .Where(p => p.PerfilUsuarioId != usuario.Id)
          .ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                lista = lista
                    .Where(p => p.Usuario != null && p.Usuario.Nombre.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.CurrentFilter = searchString;

            return View(lista);
        }

        // GET: PerfilUsuarios/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var data = _context.PerfilUsuarios
         .Include(p => p.Usuario)
        .FirstOrDefault(p => p.PerfilUsuarioId == id);

            if (data == null)
                return NotFound();

            //Obtener solo las playlist publicas
            var playlistsPublicas = _context.Playlists
       .Where(pl => pl.UsuarioId == id && pl.EsPrivada == false)
       .ToList();

            ViewBag.PlaylistsPublicas = playlistsPublicas;

            var usuarioActual = await _userManager.GetUserAsync(User);
            ViewBag.EsMiPerfil = (usuarioActual?.Id == id);

            return View(data);
        }

        // GET: PerfilUsuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PerfilUsuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerfilUsuario data, IFormFile nuevaFoto)
        {
            try
            {
                // Guardar imagen si se subió
                if (nuevaFoto != null && nuevaFoto.Length > 0)
                {
                    var carpetaUsuarios = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "usuarios");

                    if (!Directory.Exists(carpetaUsuarios))
                    {
                        Directory.CreateDirectory(carpetaUsuarios);
                    }

                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(nuevaFoto.FileName);
                    var rutaGuardado = Path.Combine(carpetaUsuarios, nombreArchivo);

                    using (var stream = new FileStream(rutaGuardado, FileMode.Create))
                    {
                        await nuevaFoto.CopyToAsync(stream);
                    }

                    data.FotoUrl = "/img/usuarios/" + nombreArchivo;
                }

                Crud<PerfilUsuario>.Create(data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }


        // GET: PerfilUsuarios/Edit/5
        public ActionResult Edit(string id)
        {
            var data = Crud<PerfilUsuario>.GetById(id);
            return View(data);
        }

        // POST: PerfilUsuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, PerfilUsuario data, IFormFile nuevaFoto)
        {
            try
            {
                var perfilExistente = Crud<PerfilUsuario>.GetById(id);
                if (perfilExistente == null)
                {
                    return NotFound();
                }

                // Actualizar campos editables
                perfilExistente.Bio = data.Bio;
                perfilExistente.FechaNacimiento = data.FechaNacimiento;
                perfilExistente.Pais = data.Pais;

                // Si se subió nueva imagen
                // Si se subió nueva imagen
                if (nuevaFoto != null && nuevaFoto.Length > 0)
                {
                    // Ruta de carpeta
                    var carpetaUsuarios = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "usuarios");

                    // Crear la carpeta si no existe
                    if (!Directory.Exists(carpetaUsuarios))
                    {
                        Directory.CreateDirectory(carpetaUsuarios);
                    }

                    // Elimina imagen anterior si existe
                    if (!string.IsNullOrEmpty(perfilExistente.FotoUrl))
                    {
                        var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", perfilExistente.FotoUrl.TrimStart('/'));
                        if (System.IO.File.Exists(rutaAnterior))
                        {
                            System.IO.File.Delete(rutaAnterior);
                        }
                    }

                    // Guardar nueva imagen
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(nuevaFoto.FileName);
                    var rutaGuardado = Path.Combine(carpetaUsuarios, nombreArchivo);

                    using (var stream = new FileStream(rutaGuardado, FileMode.Create))
                    {
                        await nuevaFoto.CopyToAsync(stream);
                    }

                    // Guardar solo la URL relativa
                    perfilExistente.FotoUrl = "/img/usuarios/" + nombreArchivo;
                }


                Crud<PerfilUsuario>.Update(id, perfilExistente); // Usamos el Update con string
                return RedirectToAction("Details", new { id = perfilExistente.PerfilUsuarioId });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: PerfilUsuarios/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    var data = Crud<PerfilUsuario>.GetById(id);
        //    return View(data);
            
        //}

        //// POST: PerfilUsuarios/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, PerfilUsuario data)
        //{

        //    try
        //    {
        //        Crud<PerfilUsuario>.Delete(id);
        //        return RedirectToAction(nameof(IndexAsync));
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        return View(data);
        //    }
        //}
    }
}
