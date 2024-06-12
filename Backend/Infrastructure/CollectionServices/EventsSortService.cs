using Application.CollectionServices;
using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;

namespace Infrastructure.CollectionServices
{
    public class EventsSortService : ISortService<EventBaseModel>
    {
        public FrozenDictionary<EventsSortType, Func<EventBaseModel, object>> Functors
           => new Dictionary<EventsSortType, Func<EventBaseModel, object>>()
           {
            { EventsSortType.Default, model => model.Id },
            { EventsSortType.ByDate, model => model.Date },
            { EventsSortType.ByPrice, model => model.Price },
            { EventsSortType.ByName, model => model.Name }
           }.ToFrozenDictionary();

        public IQueryable<EventBaseModel> Sort(
            IQueryable<EventBaseModel> collection,
            EventsSortType sortType,
            SortOrder order = SortOrder.Ascending)
        {
            Func<EventBaseModel, object> functor = TryInvokeFunctor(sortType);

            IOrderedEnumerable<EventBaseModel> result = order == SortOrder.Ascending
                ? collection.OrderBy(functor)
                : collection.OrderByDescending(functor);

            return result.AsQueryable();
        }

        private Func<EventBaseModel, object> TryInvokeFunctor(EventsSortType? sortType)
        {
            if (Functors.TryGetValue(sortType, out var functor))
            {
                return functor;
            }

            throw new InvalidDataException();
        }
    }
}
