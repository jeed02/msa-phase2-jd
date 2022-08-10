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

            IEnumerable<Character> mockCharacters = new List<Character> { char1, char2, char3, char4, char5};

            // Set up to return all characters
            mockRepo.Setup(repo => repo.GetAllCharacters()).Returns(mockCharacters);

            //Set up to return character by name
            mockRepo.Setup(repo => repo.GetCharacter(It.IsAny<string>())).Returns((string c) => mockCharacters.Where(x => x.name == c).First());

            var client = new HttpClient();
            mockFactory.Setup(c => c.CreateClient("genshin")).Returns(client);

            IHttpClientFactory factory = mockFactory.Object;
            ICharacterRepo repo = mockRepo.Object;
            _controller = new CharacterController(factory, repo);

        }
        [Test]
        public void GetCharacter()
        {
            Character HuTao = new Character() { Id = 163, name = "Hu Tao", vision = "Pyro", weapon = "Polearm", constellation = "Papilio Charontis", birthday = "0000-07-15", rarity = 5 };

            ActionResult<Character> actionResult = _controller.Get("Beidou");
            var result = actionResult.Result as OkObjectResult;

            TestContext.Write(result.Value);
            Assert.IsInstanceOf<Character>(result.Value);
            
        }

        [Test(Description ="Repo has 5 characters, check if GetAll() returns 5")]
        public void GetAllCharacters()
        {
    
            IEnumerable<Character> result = _controller.GetAll();
            
            Assert.AreEqual(5, result.Count());
        }

        
    }
}