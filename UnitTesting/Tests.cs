using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using System;
using backend.Data;
using backend.Models;
using backend.Controllers;
using Moq;

namespace UnitTesting
{
    public class Tests
    {
        private CharacterController _characterController;

        [SetUp]
        public void Setup()
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockRepo = new Mock<ICharacterRepo>();

            _characterController = new CharacterController(mockFactory.Object, mockRepo.Object);
        }

        [Test]
        public void GetReturnsTeamOf4()
        {
            var result =  _characterController.GetTeam();
            var actionResult = result.Value;

            
            
            Assert.That(actionResult, Has.Count.EqualTo(4));
        }
    }
}