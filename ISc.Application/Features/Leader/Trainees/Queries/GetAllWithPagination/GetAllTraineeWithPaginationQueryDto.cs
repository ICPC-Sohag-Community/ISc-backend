﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Trainees.Queries.GetAllWithPagination
{
    public class GetAllTraineeWithPaginationQueryDto
    {
        public string Id { get; set; }
        public string FirstName {  get; set; }
        public string MiddleName {  get; set; }
        public string LastName {  get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CodeForceHandle { get; set; }
    }

    public enum GetAllTraineeWithPaginationQueryDtoColumn
    {
        Faculty,
        Grade,
        Gender
    }
}
