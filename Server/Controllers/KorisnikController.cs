using System;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Server.DTO;
using Server.Helpers;
using Server.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Server.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    /// Kontroler za model Korisnik. 
    /// Sadrži implementaciju http zahteva vezanih za korisnike aplikacije.
    public class KorisnikController : ControllerBase
    {
        private KorisnikService _korisnikService;
        private ITokenService _tokenService;

        public KorisnikController(KorisnikService korisnikService, ITokenService tokenService)
        {
            _korisnikService = korisnikService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [AdminOnly]
        /// Dohvata sve korisnike.
        public ActionResult<List<Korisnik>> Get()
        {
            // Console.Write(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value);
            // Console.Write(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "tip").Value);
            return _korisnikService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetKorisnik")]
        [AdminOnly]
        /// Dohvata odredjenog korisnika na osnovu njegovog id-ja.
        public ActionResult<Korisnik> Get(string id)
        {
            var korisnik = _korisnikService.Get(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return korisnik;
        }

        [HttpPost]
        /// Beleži novog korisnika u bazi.
        public ActionResult<Korisnik> Create(Korisnik korisnik)
        {
            _korisnikService.Create(korisnik);
            
            return CreatedAtRoute("GetKorisnik", new { id = korisnik.Id.ToString() }, korisnik);
        }

        [HttpPost("register")]
        /// Vrši se registracija novog korisnika. 
        /// @param register - informacije potrebne za registraciju.
        public ActionResult<string> Register(RegisterDto register)
        {
            var korisnik = _korisnikService.Register(register.Username, register.Password, register.Email, register.EmailObavestenja); 

            var token = _tokenService.CreateToken(korisnik);

            return token;
        }

        [HttpPost("login")]
        /// Vrši se logovanje postojećeg korisnika uz proveru username-a i šifre.
        /// @param login - informacije potrebne za logovanje. 
        public ActionResult<string> Login(LoginDto login)
        {
            var korisnik = _korisnikService.GetByUsername(login.Username);
            if(korisnik == null)
                return Unauthorized("Invalid username");

            using var hmac= new HMACSHA512(korisnik.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            for(int i=0; i< computeHash.Length;i++)
            {
                if(computeHash[i] != korisnik.PasswordHash[i])
                    return Unauthorized("Wrong password");
            }

            var token = _tokenService.CreateToken(korisnik);

            return token;
        }

        [HttpPut("{id:length(24)}")]
        [AdminOnly]
        /// Menja postojeću vrednost u bazi.
        /// @param newValForKorisnik - nova vrednost koja treba da se nadje u bazi. 
        /// @param id - id postojeće vrednosti koja se menja.
        public IActionResult Update(string id, Korisnik newValForKorisnik)
        {
            var korisnik = _korisnikService.Get(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            _korisnikService.Update(id, newValForKorisnik);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [AdminOnly]
        /// Briše postojeće izvodjenje iz baze na osnovu id-ja.
        public IActionResult Delete(string id)
        {
            var korisnik = _korisnikService.Get(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            _korisnikService.Remove(korisnik.Id);

            return NoContent();
        }

        [HttpPost("emailObavestenja/{username}")]
        /// Menja vrednost atributa EmailObavestenja na suprotnu od postavljene.
        public IActionResult PromeniEmailObavestenja(string username)
        {
            _korisnikService.UpdateObavestenja(username);
            return NoContent();
        }
    }
}