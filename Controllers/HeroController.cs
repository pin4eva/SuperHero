
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHero.Data;
using SuperHero.Models;

namespace SuperHero.Controllers
{
    [ApiController]
    [Route("api/heros")]
    public class HeroController : ControllerBase
    {
        private readonly ILogger<Hero> logger;
        private readonly DataContext db;

        public HeroController(DataContext dataContext, ILogger<Hero> _logger)
        {
            db = dataContext;
            logger = _logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Hero>>> GetAll()
        {
            var heros = await db.Heros.ToListAsync();
            return Ok(heros);

        }

        // Get Single Hero
        [HttpGet("{id}")]
        public async Task<ActionResult<Hero>> GetHero(int id)
        {
            // var hero = heros.FirstOrDefault(hero => hero.Id == id);
            var hero = await db.Heros.FindAsync(id);
            if (hero is null) return NotFound("Invalid hero Id");

            return Ok(hero);
        }

        // create Hero
        [HttpPost]
        public async Task<ActionResult<Hero>> CreateHero(Hero newHero)
        {
            // newHero.Id = heros.Max((hero) => hero.Id) + 1;

            db.Heros.Add(newHero);
            await db.SaveChangesAsync();

            return newHero;
        }

        // Update Hero
        [HttpPut]
        public async Task<ActionResult<Hero>> UpdateHero(Hero newHero)
        {
            logger.LogInformation("hero", newHero);
            Hero? hero = await db.Heros.FindAsync(newHero.Id);
            if (hero == null) return NotFound("Invalid hero ID");
            hero.Name = newHero.Name;
            hero.Place = newHero.Place;

            await db.SaveChangesAsync();
            return Ok(newHero);
        }

        // Delete Hero
        [HttpDelete("{id}")]
        public async Task<ActionResult<Hero>> DeleteHero(int id)
        {
            Hero? hero = await db.Heros.FindAsync(id);
            if (hero is null) return NotFound("Invalid hero Id");
            db.Heros.Remove(hero);
            await db.SaveChangesAsync();
            return Ok(hero);
        }
    }
}
