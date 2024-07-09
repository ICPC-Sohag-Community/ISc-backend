﻿using ISc.Domain.Comman.Dtos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Application.Interfaces.Repos
{
    public interface ITraineeRepo:IActorQeueryRepo<Trainee>
    {
        Task DeleteAsync(Account account,Trainee trainee, bool isComplete);
    }
}
