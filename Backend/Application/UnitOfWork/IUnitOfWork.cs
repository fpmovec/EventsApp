namespace Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
