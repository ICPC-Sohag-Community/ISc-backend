using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Presistance;
using ISc.Presistance.Repos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISC.UnitTests
{
    internal class UnitOfWorkTest : UnitOfWork
    {
        public UnitOfWorkTest(InMemoryDbcontext context, UserManager<Account> userManager, IStuffArchiveRepo stuffRepo, IMediaServices mediaServices) : base(context, userManager, stuffRepo, mediaServices)
        {
        }
    }
}
