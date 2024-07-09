using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Application.Interfaces
{
    public interface IAuthServices
    {
        string GenerateToken(Account user,ICollection<string> roles, bool rembemerMe = false);
    }
}
