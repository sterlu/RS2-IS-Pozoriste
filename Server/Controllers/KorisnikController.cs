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
        public ActionResult<List<Korisnik>> Get()
        {
            // Console.Write(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value);
            // Console.Write(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "tip").Value);
            return _korisnikService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetKorisnik")]
        [AdminOnly]
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
        public ActionResult<Korisnik> Create(Korisnik korisnik)
        {
            _korisnikService.Create(korisnik);
            
            return CreatedAtRoute("GetKorisnik", new { id = korisnik.Id.ToString() }, korisnik);
        }

        [HttpPost("register")]
        public ActionResult<string> Register(RegisterDto register)
        {
            using var hmac = new HMACSHA512();

            var korisnik = new Korisnik {
                Username = register.Username, 
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt = hmac.Key, 
                Email = register.Email, 
                Tip = "korisnik"
            };

            _korisnikService.Create(korisnik);

            var token = _tokenService.CreateToken(korisnik);

            return token;
        }

        [HttpPost("login")]
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

        [HttpGet("mailingLista")]
        public void PosaljiObavestenje(string obavestenje)
        {
            var mailingLista = _korisnikService.MailingList();
            
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("matfpozoriste@gmail.com", "pozoriste123"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("email"),
                Subject = "Nova predstava",
                Body = obavestenje,
                IsBodyHtml = false
            };
            foreach(Korisnik korisnik in mailingLista)
            {
                mailMessage.To.Add(korisnik.Email);  
            }
            smtpClient.Send(mailMessage);

        }

    }
}