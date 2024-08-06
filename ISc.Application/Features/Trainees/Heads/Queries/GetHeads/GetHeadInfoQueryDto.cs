﻿using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Trainees.Heads.Queries.GetHeads
{
    public class GetHeadInfoQueryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public College College { get; set; }
        public string? PhotoUrl { get; set; }
        public string? FacebookLink { get; set; }
        public string CodeForceHandle { get; set; }
    }
}
