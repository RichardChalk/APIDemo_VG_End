using DeleteMe.Data;
using DeleteMe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeleteMe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]

    public class SuperHeroController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public SuperHeroController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //private static List<SuperHero> heroes = new List<SuperHero>
        //{
        //    new SuperHero
        //    {
        //        Id = 1, Name = "Spiderman", FirstName = "Peter",
        //        SurName="Parker", City="New York"},
        //    new SuperHero
        //    {
        //        Id = 2,
        //        Name = "Ironman", FirstName = "Tony", SurName="Stark",
        //        City="New York"},
        //};

        // READ ALL ///////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve ALL Superheroes from the database
        /// </summary>
        /// <returns>
        /// A full list of ALL Superheroes
        /// </returns>
        /// <remarks>
        /// Example end point: GET /SuperHero
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a full list of ALL Superheroes
        /// </response>

        [HttpGet]
        [Authorize(Roles = "Admin, User")]

        public async Task<ActionResult<List<SuperHero>>> GetAll()
        {
            //return Ok(heroes);
            return Ok(await _dbContext.SuperHeroes.ToListAsync());

        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<SuperHero>> GetOne(int id)
        {
            //var hero = heroes.Find(s => s.Id == id);
            var hero = _dbContext.SuperHeroes.Find(id);


            if (hero == null)
            {
                return BadRequest("Superhero not found");
            }
            return Ok(hero);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<SuperHero>> PostHero(SuperHero hero)
        {
            //heroes.Add(hero);
            //return Ok(heroes);
            _dbContext.SuperHeroes.Add(hero);
            await _dbContext.SaveChangesAsync();
            return Ok(await _dbContext.SuperHeroes.ToListAsync());

        }

        [HttpPut]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<SuperHero>> UpdateHero(SuperHero hero)
        {
            // OBS: PUT Uppdaterar HELA SuperHero (ALLA properties)
            //var heroToUpdate = heroes.Find(s => s.Id == hero.Id);
            var heroToUpdate = await _dbContext.SuperHeroes.FindAsync(hero.Id);


            if (heroToUpdate == null)
            {
                return BadRequest("Superhero not found");
            }

            heroToUpdate.Name = hero.Name;
            heroToUpdate.FirstName = hero.FirstName;
            heroToUpdate.SurName = hero.SurName;
            heroToUpdate.City = hero.City;

            //return Ok(heroes);
            await _dbContext.SaveChangesAsync();

            return Ok(await _dbContext.SuperHeroes.ToListAsync());

        }

        [HttpPatch]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<SuperHero>> PatchHero(JsonPatchDocument hero, int id)
        {
            // OBS: PATCH Uppdaterar SuperHero (VISSA properties)
            var heroToUpdate = await
                _dbContext.SuperHeroes.FindAsync(id);

            if (heroToUpdate == null)
            {
                return BadRequest("Superhero not found");
            }

            hero.ApplyTo(heroToUpdate);
            await _dbContext.SaveChangesAsync();

            return Ok(await _dbContext.SuperHeroes.ToListAsync());
        }


        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<SuperHero>> Delete(int id)
        {
            //var hero = heroes.Find(s => s.Id == id);
            var hero = await _dbContext.SuperHeroes.FindAsync(id);


            if (hero == null)
            {
                return BadRequest("Superhero not found");
            }

            //heroes.Remove(hero);
            //return Ok(heroes);

            _dbContext.SuperHeroes.Remove(hero);
            await _dbContext.SaveChangesAsync();
            return Ok(await _dbContext.SuperHeroes.ToListAsync());

        }
    }
}
