﻿using ISc.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
    public class CampModel:ISoftEntity
    {
        [Key]
        public string Name { get; set; }
    }
}
