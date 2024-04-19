namespace ISc.Application.Interfaces.Repos
{
    public interface IUnitOFWork:IDisposable
    {

        Task<int> SaveAsync();
    }
}
