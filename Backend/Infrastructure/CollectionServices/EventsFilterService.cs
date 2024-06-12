using Application.CollectionServices;
using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;
using System.Linq.Expressions;

namespace Infrastructure.CollectionServices
{
    public class EventsFilterService : IFilterService<EventBaseModel>
    {
        public FrozenDictionary<EventFilterType, Func<EventBaseModel, object, bool>> Functors
            => new Dictionary<EventFilterType, Func<EventBaseModel, object, bool>>()
        {
            { EventFilterType.ByCategory, (model, value) => model.Category.Name == (string)value },
            { EventFilterType.ByMaxDate, (model, value) => model.Date <= (DateOnly)value },
            { EventFilterType.ByMinDate, (model, value) => model.Date >= (DateOnly)value },
            { EventFilterType.ByMinPrice, (model, value) => model.Price >= (double)value },
            { EventFilterType.ByMaxPrice, (model, value) => model.Price <= (double)value }
        }.ToFrozenDictionary();

        public IQueryable<EventBaseModel> Filter(IQueryable<EventBaseModel> collection, EventFilterType? property, object filterValue)
        {
            Func<EventBaseModel, object, bool> functor = TryInvokeFunctor(property);
            Expression<Func<EventBaseModel, bool>> sortFunctor =  model => functor(model, filterValue);

            return collection.Where(sortFunctor);
        }

        private Func<EventBaseModel, object, bool> TryInvokeFunctor(EventFilterType filterProperty)
        {
            if (Functors.TryGetValue(filterProperty, out var functor))
            {
                return functor;
            }

            throw new InvalidDataException();
        }
    }
}
