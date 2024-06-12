using Application.UnitOfWork;

namespace Infrastructure.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork, IDisposable
    {
        public Task CompleteAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
