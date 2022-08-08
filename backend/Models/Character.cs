using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace backend.Models
{
	public class Character
	{
		public int Id { get; set; }
		public string name { get; set; }
		public string? vision { get; set; }
		public string? weapon { get; set; }
		public string? constellation { get; set; }
		public string? birthday { get; set; }
		public int rarity { get; set; }

	}

	
}
