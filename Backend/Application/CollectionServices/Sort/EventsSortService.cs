using Entities.Enums;
using Entities.Models;
using System.Collections.Frozen;

namespace Application.CollectionServices.Sort
{
    public class EventsSortService : ISortService<EventBaseModel>
    {
        public FrozenDictionary<SortType, Func<EventBaseModel, object>> Functors
           => new Dictionary<SortType, Func<EventBaseModel, object>>()
           {
            { SortType.Default, model => model.Id },
            { SortType.ByDate, model => model.Date },
            { SortType.ByPrice, model => model.Price },
            { SortType.ByName, model => model.Name }
           }.ToFrozenDictionary();

        public IQueryable<EventBaseModel> Sort(
            IQueryable<EventBaseModel> collection,
            SortType sortType,
            SortOrder order = SortOrder.Ascending)
        {
            Func<EventBaseModel, object> functor = TryGetFunctor(sortType);

            IOrderedEnumerable<EventBaseModel> result = order == SortOrder.Ascending
                ? collection.OrderBy(functor)
                : collection.OrderByDescending(functor);

            return result.AsQueryable();
        }

        private Func<EventBaseModel, object> TryGetFunctor(SortType sortType)
        {
            if (Functors.TryGetValue(sortType, out var functor))
            {
                return functor;
            }

            return model => model.Id;
        }
    }
}
