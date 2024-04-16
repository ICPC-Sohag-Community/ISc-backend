﻿using ISc.Domain.Comman.Enums;
using ISc.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
	public class NewRegisteration:Auditable,ISoftEntity
	{
		public string FirstName { get; set; }
		public string MiddelName { get; set; }
		public string LastName { get; set; }
		public string NationalId { get; set; }
		public DateOnly BirthDate { get; set; }
		public int Grade { get; set; }
		public string College { get; set; }
		public Gender Gender { get; set; }
		public string CodeForceHandle { get; set; }
		public string? FacebookHandle { get; set; }
		public string? VjudgeHandle { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string? ImageUrl { get; set; }
        public int CampId { get; set; }
        public string? Comment { get; set; }
        public bool HasLaptop { get; set; }
	}
}
