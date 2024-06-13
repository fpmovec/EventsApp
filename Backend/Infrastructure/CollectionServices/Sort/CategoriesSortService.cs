using Application.CollectionServices;
using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;
using System.Linq;

namespace Infrastructure.CollectionServices.Sort
{
    public class CategoriesSortService : ISortService<EventCategory>
    {
        public FrozenDictionary<SortType, Func<EventCategory, object>> Functors
          => new Dictionary<SortType, Func<EventCategory, object>>
          {
            { SortType.Default, model => model.Id },
            { SortType.ByName, model => model.Name },
          }.ToFrozenDictionary();

        public IQueryable<EventCategory> Sort(IQueryable<EventCategory> collection, SortType sortType, SortOrder order)
        {
            Func<EventCategory, object> functor = TryInvokeFunctor(sortType);

            IOrderedEnumerable<EventCategory> result = order == SortOrder.Ascending
                ? collection.OrderBy(functor)
                : collection.OrderByDescending(functor);

            return result.AsQueryable();
        }

        private Func<EventCategory, object> TryInvokeFunctor(SortType sortType)
        {
            if (Functors.TryGetValue(sortType, out var functor))
            {
                return functor;
            }

            throw new InvalidDataException();
        }
    }
}
