using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Data;
using SuperHeroAPI.Models;

namespace SuperHeroAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SuperHeroController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public SuperHeroController(ApplicationDbContext context)
		{
			_context = context;
		}


		[HttpGet]
		public async Task<ActionResult<List<SuperHero>>> GetAllHeros()
		{
			var heros = await _context.SuperHeros.ToListAsync();
			return Ok(heros);

		}


		[HttpGet]
		[Route("{id}")]
		public async Task<ActionResult<List<SuperHero>>> GetHero(int id)
		{
			var hero = await _context.SuperHeros.FindAsync(id);
			if (hero == null)
			{
				return NotFound("Hero Not Found");
			}
			return Ok(hero);

		}



		[HttpPost] 
		public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
		{
			_context.SuperHeros.Add(hero);
			await _context.SaveChangesAsync();
			return Ok(await _context.SuperHeros.ToListAsync());

		}


		
		[HttpPut("{id}")]
		public async Task<ActionResult<List<SuperHero>>> UpdateHero(int id, SuperHero updatedHero)
		{
			var dbHero = await _context.SuperHeros.FindAsync(id);
			if (dbHero == null)
			{
				return NotFound("Hero Not Found");
			}

			dbHero.Name = updatedHero.Name;
			dbHero.FirstName = updatedHero.FirstName;
			dbHero.LastName = updatedHero.LastName;
			dbHero.Place = updatedHero.Place;

			await _context.SaveChangesAsync();

			return Ok(await _context.SuperHeros.ToListAsync());
		}




		[HttpDelete]
		public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
		{
			var hero = await _context.SuperHeros.FindAsync(id);
			if (hero == null)
			{
				return NotFound();
			}

			_context.SuperHeros.Remove(hero);
			await _context.SaveChangesAsync();

			return Ok(await _context.SuperHeros.ToListAsync());//return NoContent()
		}



        [HttpGet("search")]
        public async Task<ActionResult<List<SuperHero>>> SearchHeroes([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty");
            }

            var heroes = await _context.SuperHeros
                .Where(hero => hero.Name.Contains(query) || hero.FirstName.Contains(query) || hero.LastName.Contains(query) || hero.Place.Contains(query))
                .ToListAsync();

            if (!heroes.Any())
            {
                return NotFound("No heroes found matching the search query");
            }

            return Ok(heroes);
        }







    }
}
