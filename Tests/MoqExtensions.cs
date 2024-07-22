using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests
{
    public static class MoqExtensions
    {
        public static void SetSource<TEntity>(this Mock<DbSet<TEntity>> collectionMock, IQueryable<TEntity> source)
            where TEntity : class
        {
            collectionMock.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(source.Provider);
            collectionMock.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(source.Expression);
            collectionMock.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(source.ElementType);
            collectionMock.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(source.GetEnumerator());
        }
    }
}
