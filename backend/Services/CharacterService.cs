using System;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace backend.Services
{
    public class CharacterService : ICharacterRepo
    {
        private readonly CharacterDb _dbCharacter;
        
        public CharacterService(CharacterDb characterDb) { _dbCharacter = characterDb; }

        public Character AddCharacter(Character character)
        {
            _dbCharacter.Add(character);
            _dbCharacter.SaveChanges();
            return character;
        }

        public void DeleteCharacter(Character character)
        {
            Character c = _dbCharacter.Characters.FirstOrDefault(e => e.name == character.name);
            if (c != null)
            {
                _dbCharacter.Characters.Remove(c);
                _dbCharacter.SaveChanges();
            }
                
        }

        public IEnumerable<Character> GetAllCharacters()
        {
            IEnumerable<Character> characters = _dbCharacter.Characters.ToList<Character>();
            return characters;
        }

        public Character GetCharacter(string name)
        {
            Character c = _dbCharacter.Characters.FirstOrDefault(e => e.name == name);
            return c;
        }

        public void SaveChanges()
        {
            _dbCharacter.SaveChanges();
        }

        public void UpdateCharacter(Character character)
        {
            Character c = _dbCharacter.Characters.FirstOrDefault(e => e.name == character.name);
            if(c != null)
            {
                c = character;
                _dbCharacter.SaveChanges();
            }
            
        }
    }


}

