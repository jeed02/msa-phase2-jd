using System;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;


namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CharacterController : ControllerBase
{
	private readonly HttpClient _client;

	public CharacterController(IHttpClientFactory clientFactory) {
		if (clientFactory is null)
		{
			throw new ArgumentNullException(nameof(clientFactory));
		}
		_client = clientFactory.CreateClient("genshin");
	}

	[HttpGet]
	[Route("raw")]
	[ProducesResponseType(200)]
	public async Task<IActionResult> GetAllCharacters()
	{
		var res = await _client.GetAsync("");
		var content = await res.Content.ReadAsStringAsync();
		string[] allCharacters = JsonSerializer.Deserialize<string[]>(content);

		foreach(string n in allCharacters)
        {
			var c = await _client.GetAsync("/characters/" + n);
			var charInfo = await c.Content.ReadAsStringAsync();
			Character character = JsonSerializer.Deserialize<Character>(charInfo);
			CharacterService.Add(character);
		}


		return Ok(allCharacters);
	}

	//GET all
	[HttpGet]
	public ActionResult<List<Character>> GetAll() =>
		CharacterService.GetAll();

    //GET by name
    [HttpGet("{name}")]
	public ActionResult<Character> Get(string name)
    {
		var character = CharacterService.Get(name);

		if (character == null)
			return NotFound();

		return character;
    }

	//POST
	[HttpPost]
	public IActionResult Create(Character c)
	{
		CharacterService.Add(c);
		return CreatedAtAction(nameof(Create), c);
	}

	//PUT
	[HttpPut("{name}")]
	public IActionResult Update(string name, Character c)
	{
		if (name != c.name)
			return BadRequest();

		var existingCharacter = CharacterService.Get(name);
		if (existingCharacter is null)
			return NotFound();

		CharacterService.Update(c);

		return NoContent();
	}

	//DELETE
	[HttpDelete("{name}")]
	public IActionResult Delete(string name)
	{
		var character = CharacterService.Get(name);

		if (character is null)
			return NotFound();

		CharacterService.Delete(name);

		return NoContent();
	}

}
