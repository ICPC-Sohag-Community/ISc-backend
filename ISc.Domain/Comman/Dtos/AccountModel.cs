using ISc.Domain.Models.IdentityModels;

namespace ISc.Domain.Comman.Dtos
{
    public class AccountModel<T> where T : class
    {
        public Account? Account { get; set; }
        public string? Password { get; set; }
        public T Member { get; set; }
    }
}
