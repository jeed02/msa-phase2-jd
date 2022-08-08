using System.Collections.Generic;
using backend.Models;

namespace backend.Data
{
    public interface ICharacterRepo
    {
        IEnumerable<Character> GetAllCharacters();
        Character GetCharacter(string name);
        Character AddCharacter(Character character);
        void UpdateCharacter(Character character);
        void DeleteCharacter(Character character);
        void SaveChanges();
        List<Character> GetTeam();
    }
}
