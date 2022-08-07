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
	private readonly ICharacterRepo _repository;

	public CharacterController(IHttpClientFactory clientFactory, ICharacterRepo repository) {
		if (clientFactory is null)
		{
			throw new ArgumentNullException(nameof(clientFactory));
		}
		_client = clientFactory.CreateClient("genshin");
		_repository = repository;
	}

	// GET all genshin characters from API and store into database
	[HttpGet]
	[Route("GetAPI")]
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
			_repository.AddCharacter(character);
		}


		return Ok(allCharacters);
	}

	//GET all
	[HttpGet]
	public ActionResult<IEnumerable<Character>> GetAll()
    {
		IEnumerable<Character> characters = _repository.GetAllCharacters();
		return Ok(characters);
	}
		

    //GET by name
    [HttpGet("{name}")]
	public ActionResult<Character> Get(string name)
    {
		Character character = _repository.GetCharacter(name);

		if (character == null)
			return NotFound();

		return character;
    }

	//POST
	[HttpPost]
	public IActionResult Create(Character c)
	{
		Character AddedCharacter = _repository.AddCharacter(c);
		return CreatedAtAction(nameof(Create), AddedCharacter);
	}

	//PUT
	[HttpPut("{name}")]
	public IActionResult Update(string name, Character c)
	{
		if (name != c.name)
			return BadRequest();

		Character existingCharacter = _repository.GetCharacter(name);
		if (existingCharacter is null)
			return NotFound();

		_repository.UpdateCharacter(c);

		return NoContent();
	}

	//DELETE
	[HttpDelete("{name}")]
	public IActionResult Delete(string name)
	{
		Character existingCharacter = _repository.GetCharacter(name);

		if (existingCharacter is null)
			return NotFound();

		_repository.DeleteCharacter(existingCharacter);

		return NoContent();
	}

}
