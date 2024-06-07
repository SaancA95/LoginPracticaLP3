using Microsoft.AspNetCore.Mvc;
using PracticasLP3.Data;
using PracticasLP3.ViewsModels;
using PracticasLP3.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PracticasLP3.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public AccesoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Registrarse(UsuarioVM modelo)
        {
            if(modelo.Clave!= modelo.ConfirmarClave) {

                ViewData["Mensaje"] = "las claves no cuadran pendejo";
                return View();
            }

            Usuario usuario = new Usuario()
            {
                Nombre = modelo.Nombre,
                Correo = modelo.Correo,
                Clave = modelo.Clave,
            };

            await _appDBContext.Usuarios.AddAsync(usuario);
            await _appDBContext.SaveChangesAsync();

            if (usuario.Id!=0)
            {
                return RedirectToAction("Login","Acceso");
            }

            ViewData["Mensaje"] = "No se pudo crear el user pendejo";

            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity!.IsAuthenticated) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM modelo)
        {
            Usuario? usuario_encontrado = await _appDBContext.Usuarios
                                            .Where(u =>
                                            u.Correo == modelo.Correo &&
                                            u.Clave== modelo.Clave
                                            ).FirstOrDefaultAsync();

            if(usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias jsjs";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,usuario_encontrado.Nombre)
                
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");



           
        }




        }

}
