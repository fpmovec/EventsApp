using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;

namespace Application.CollectionServices.Sort
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
            Func<EventCategory, object> functor = TryGetFunctor(sortType);

            IOrderedEnumerable<EventCategory> result = order == SortOrder.Ascending
                ? collection.OrderBy(functor)
                : collection.OrderByDescending(functor);

            return result.AsQueryable();
        }

        private Func<EventCategory, object> TryGetFunctor(SortType sortType)
        {
            if (Functors.TryGetValue(sortType, out var functor))
            {
                return functor;
            }

            return model => model.Id;
        }
    }
}
