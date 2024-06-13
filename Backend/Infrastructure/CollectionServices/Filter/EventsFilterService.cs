using Application.CollectionServices;
using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;
using System.Linq.Expressions;

namespace Infrastructure.CollectionServices.Filter
{
    public class EventsFilterService : IFilterService<EventBaseModel>
    {
        public FrozenDictionary<FilterType, Func<EventBaseModel, object, bool>> Functors
            => new Dictionary<FilterType, Func<EventBaseModel, object, bool>>()
        {
            { FilterType.ByCategory, (model, value) => model.Category.Name == (string)value },
            { FilterType.ByMaxDate, (model, value) => model.Date <= (DateOnly)value },
            { FilterType.ByMinDate, (model, value) => model.Date >= (DateOnly)value },
            { FilterType.ByMinPrice, (model, value) => model.Price >= (double)value },
            { FilterType.ByMaxPrice, (model, value) => model.Price <= (double)value }
        }.ToFrozenDictionary();

        public IQueryable<EventBaseModel> Filter(IQueryable<EventBaseModel> collection, FilterType property, object filterValue)
        {
            Func<EventBaseModel, object, bool> functor = TryInvokeFunctor(property);
            Expression<Func<EventBaseModel, bool>> sortFunctor = model => functor(model, filterValue);

            return collection.Where(sortFunctor);
        }

        private Func<EventBaseModel, object, bool> TryInvokeFunctor(FilterType filterProperty)
        {
            if (Functors.TryGetValue(filterProperty, out var functor))
            {
                return functor;
            }

            throw new InvalidDataException();
        }
    }
}
