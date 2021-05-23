using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Server.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private KorisnikService _korisnikService;

        public KorisnikController(KorisnikService korisnikService)
        {
            _korisnikService = korisnikService;
        }

        [HttpGet]
        public ActionResult<List<Korisnik>> Get() =>
            _korisnikService.Get();

        [HttpGet("{id:length(24)}", Name = "GetKorisnik")]
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
        public ActionResult<Korisnik> register(Register register)
        {
            using var hmac = new HMACSHA512();

            var korisnik = new Korisnik {
                Username = register.Username, 
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt = hmac.Key, 
                Email = register.Password, 
                Tip = register.Tip
            };

            _korisnikService.Create(korisnik);

            return korisnik;

        }

        [HttpPost("login")]
        public ActionResult<Korisnik> login(Login login)
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

            return korisnik;
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

    }
}