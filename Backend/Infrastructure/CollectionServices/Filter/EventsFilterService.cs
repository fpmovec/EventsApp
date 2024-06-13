using Application.CollectionServices;
using Application.Models;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
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
            { FilterType.ByMaxPrice, (model, value) => model.Price <= (double)value },
            { FilterType.ByPlace, (model, value) => model.Place == (string)value    }
        }.ToFrozenDictionary();

        public IQueryable<EventBaseModel> Filter(IQueryable<EventBaseModel> collection, FilterType property, object filterValue)
        {
            Func<EventBaseModel, object, bool> functor = TryInvokeFunctor(property);
            Func<EventBaseModel, bool> sortFunctor = model => functor(model, filterValue);

            var events = collection
                .Include(e => e.Image)
                .Include(e => e.Category)
                .ToList()
                .Where(sortFunctor)
                .AsQueryable();

            return events;
        }

        public IQueryable<EventBaseModel> FilterWithManyOptions(IQueryable<EventBaseModel> collection, List<FilterOption> filterOptions)
        {
            IQueryable<EventBaseModel> source = collection
                .Include(e => e.Image)
                .Include(e => e.Category);

            if (filterOptions is null && filterOptions?.Count == 0)
                return source;

            foreach (FilterOption filterOption in filterOptions)
            {
                if (filterOption.Value is not null)
                {
                    Func<EventBaseModel, object, bool> functor = TryInvokeFunctor(filterOption.FilterType);
                    Func<EventBaseModel, bool> sortFunctor = model => functor(model, filterOption.Value);

                    source = source.ToList().Where(sortFunctor).AsQueryable();
                }
                
            }

            return source;
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
