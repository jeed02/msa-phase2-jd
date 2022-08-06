using System;
using backend.Models;
namespace backend.Services;

public static class CharacterService
{
	static List<Character> Characters { get; }

	static CharacterService()
	{
		Characters = new List<Character> {};
	}

	public static List<Character> GetAll() => Characters;

	public static Character? Get(string name) => Characters.FirstOrDefault(c => c.name == name);

    public static void Add(Character c)
    {
        Characters.Add(c);
    }

    public static void Delete(string name)
    {
        var c = Get(name);
        if (c is null)
            return;

        Characters.Remove(c);
    }

    public static void Update(Character c)
    {
        var index = Characters.FindIndex(p => p.name == c.name);
        if (index == -1)
            return;

        Characters[index] = c;
    }
}
