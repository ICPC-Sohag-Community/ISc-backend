using ISc.Domain.Models.IdentityModels;

namespace ISc.Domain.Comman.Dtos
{
    public class AccountModel<T>
    {
        public Account? Account { get; set; }
        public string? Password { get; set; }
        public T Member { get; set; }
    }
}
