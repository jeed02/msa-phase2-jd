using System;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace backend.Data
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
            Character c = _dbCharacter.Characters.FirstOrDefault(e => e.name.ToLower() == character.name.ToLower());
            if (c != null)
            {
                _dbCharacter.Characters.Remove(c);
                _dbCharacter.SaveChanges();
            }

        }

        public IEnumerable<Character> GetAllCharacters()
        {
            IEnumerable<Character> characters = _dbCharacter.Characters.ToList();
            return characters;
        }

        public Character GetCharacter(string name)
        {
            Character c = _dbCharacter.Characters.FirstOrDefault(e => e.name.ToLower() == name.ToLower());
            return c;
        }

        public void SaveChanges()
        {
            _dbCharacter.SaveChanges();
        }

        public void UpdateCharacter(Character character)
        {
            Character c = _dbCharacter.Characters.FirstOrDefault(e => e.name.ToLower() == character.name.ToLower());
            if (c != null)
            {
                c = character;
                _dbCharacter.SaveChanges();
            }

        }

        public List<Character> GetTeam()
        {
            Random rand = new Random();
            List<Character> team = new List<Character>();
            int[] existingRandInt = new int[4];
            
            for(int i=0; i<4; i++)
            {
                int toSkip = rand.Next(1, _dbCharacter.Characters.Count());
                while (existingRandInt.Contains(toSkip))
                {
                    toSkip = rand.Next(1, _dbCharacter.Characters.Count());
                }
                existingRandInt[i] = toSkip;
                Character c = _dbCharacter.Characters.Skip(toSkip).Take(1).First();
                team.Add(c);
            }
            
            return team;
        }
    }


}

