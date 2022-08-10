using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using backend.Models;
using backend.Data;
using backend.Controllers;
using Moq;

namespace UnitTesting
{
    public class Tests
    {
        private CharacterController _controller;
        [SetUp]
        public void Setup()
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockRepo = new Mock<ICharacterRepo>();
            Character char1 = new Character() { Id = 154, name = "Beidou", vision = "Electro", weapon = "Claymore", constellation = "Victor Mare", birthday = "0000-02-14", rarity = 4 };
            Character char2 = new Character() { Id = 178, name = "Kujou Sara", vision = "Electro", weapon = "Bow", constellation = "Flabellum", birthday = "0000-07-14", rarity = 4 };
            Character char3 = new Character() { Id = 163, name = "Hu Tao", vision = "Pyro", weapon = "Polearm", constellation = "Papilio Charontis", birthday = "0000-07-15", rarity = 5 };
            Character char4 = new Character() { Id = 175, name = "Raiden Shogun", vision = "Electro", weapon = "Polearm", constellation = "Imperatrix Umbrosa", birthday = "0000-06-26", rarity = 5 };
            Character char5 = new Character() { Id = 195, name = "Yun Jin", vision = "Geo", weapon = "Polearm", constellation = "Opera Grandis", birthday = "0000-05-21", rarity = 4 };

            IList<Character> mockCharacters = new List<Character> { char1, char2, char3, char4, char5 };
            List<Character> team = new List<Character>();
            Random rand = new Random();
            int[] existingRandInt = new int[4];

            for (int i = 0; i < 4; i++)
            {
                int toSkip = rand.Next(1, mockCharacters.Count());
                while (existingRandInt.Contains(toSkip))
                {
                    toSkip = rand.Next(1, mockCharacters.Count());
                }
                existingRandInt[i] = toSkip;
                Character c = mockCharacters.Skip(toSkip).Take(1).First();
                team.Add(c);
            }
            
            // Set up to return all characters
            mockRepo.Setup(repo => repo.GetAllCharacters()).Returns(mockCharacters);

            //Set up to return character by name
            mockRepo.Setup(repo => repo.GetCharacter(It.IsAny<string>())).Returns((string c) => mockCharacters.Where(x => x.name == c).FirstOrDefault());

            //Set up to add character
            mockRepo.Setup(repo => repo.AddCharacter(It.IsAny<Character>())).Callback((Character c) => { 
                mockCharacters.Add(c);
            });

            //Set up delete character
            mockRepo.Setup(repo => repo.DeleteCharacter(It.IsAny<Character>())).Callback((Character c) => { mockCharacters.Remove(c); });

            //Set up update character
            mockRepo.Setup(repo => repo.UpdateCharacter(It.IsAny<Character>())).Callback((Character c) =>
            {
                Character existingChar = mockCharacters.Where(x => x.name == c.name).FirstOrDefault();
                if (existingChar != null)
                {
                    int index = mockCharacters.IndexOf(existingChar);
                    mockCharacters[index] = c;
                }
            });

            //Set up get randomised team
            mockRepo.Setup(repo => repo.GetTeam()).Returns(team);

            var client = new HttpClient();
            mockFactory.Setup(c => c.CreateClient("genshin")).Returns(client);

            IHttpClientFactory factory = mockFactory.Object;
            ICharacterRepo repo = mockRepo.Object;
            _controller = new CharacterController(factory, repo);

        }
        [Test(Description = "Testing GetCharacter() method for Hu Tao, Hu Tao's name has a space meaning it also tests string formatting")]
        public void GetCharacterHuTao()
        {
            Character HuTao = new Character() { Id = 163, name = "Hu Tao", vision = "Pyro", weapon = "Polearm", constellation = "Papilio Charontis", birthday = "0000-07-15", rarity = 5 };

            ActionResult<Character> actionResult = _controller.Get("Hu Tao");
            var result = actionResult.Result as OkObjectResult;
            var charResult = result.Value;


            //Check if instance of Character
            Assert.IsInstanceOf<Character>(charResult);

            //Check if Character fields match with Hu Tao
            if (charResult is Character)
            {
                Assert.AreEqual(HuTao.name, ((Character)charResult).name);
                Assert.AreEqual(HuTao.vision, ((Character)charResult).vision);
                Assert.AreEqual(HuTao.weapon, ((Character)charResult).weapon);
                Assert.AreEqual(HuTao.constellation, ((Character)charResult).constellation);
                Assert.AreEqual(HuTao.birthday, ((Character)charResult).birthday);
                Assert.AreEqual(HuTao.rarity, ((Character)charResult).rarity);
            }

        }

        [Test(Description = "Repo has 5 characters, check if GetAll() returns 5")]
        public void GetAllCharacters()
        {

            IEnumerable<Character> result = _controller.GetAll();

            //Check if count is 5, meaning endpoint got all characters
            Assert.AreEqual(5, result.Count());
        }

        [Test]
        public void CanAddCharacter()
        {
            Character shenhe = new Character() { name = "Shenhe", vision = "Cryo", weapon = "Polearm", constellation = "Crista Doloris", birthday = "0000-03-10", rarity = 5 };

            Assert.AreEqual(5, _controller.GetAll().Count());

            _controller.Create(shenhe);
            Assert.AreEqual(6, _controller.GetAll().Count());

            ActionResult<Character> c = _controller.Get("Shenhe");
            var result = c.Result as OkObjectResult;
            var value = result.Value;

            Assert.IsInstanceOf<Character>(value);
            if (value is Character)
            {
                Assert.AreEqual(shenhe.name, ((Character)value).name);
                Assert.AreEqual(shenhe.vision, ((Character)value).vision);
                Assert.AreEqual(shenhe.weapon, ((Character)value).weapon);
                Assert.AreEqual(shenhe.constellation, ((Character)value).constellation);
                Assert.AreEqual(shenhe.birthday, ((Character)value).birthday);
                Assert.AreEqual(shenhe.rarity, ((Character)value).rarity);
            }

        }

        [Test]
        public void CanDeleteCharacter()
        {
            Assert.AreEqual(5, _controller.GetAll().Count());
            _controller.Delete("Beidou");
            Assert.AreEqual(4, _controller.GetAll().Count());

        }

        [Test]
        public void CanUpdateCharacter()
        {
            Character updateChar = new Character() { Id = 195, name = "Yun Jin", vision = "Geo", weapon = "Claymore", constellation = "Opera Grandis", birthday = "0000-05-21", rarity = 4 };
            _controller.Update(updateChar.name, updateChar);
            ActionResult<Character> newChar = _controller.Get(updateChar.name);
            var result = newChar.Result as OkObjectResult;
            var value = result.Value;
            if (value is Character)
            {
                Assert.AreEqual(updateChar.name, ((Character)value).name);
                Assert.AreEqual(updateChar.vision, ((Character)value).vision);
                Assert.AreEqual(updateChar.weapon, ((Character)value).weapon);
                Assert.AreEqual(updateChar.constellation, ((Character)value).constellation);
                Assert.AreEqual(updateChar.birthday, ((Character)value).birthday);
                Assert.AreEqual(updateChar.rarity, ((Character)value).rarity);
            }

        }

        [Test]
        public void ReturnsTeamOf4()
        {
            List<Character> team = _controller.GetTeam();
            Assert.AreEqual(4, team.Count());
            Assert.That(team, Is.Unique);
        }


    }
}