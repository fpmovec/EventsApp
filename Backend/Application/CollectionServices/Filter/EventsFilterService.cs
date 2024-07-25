using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CollectionServices.Filter
{
    public class EventsFilterService : IFilterService<EventBaseModel>
    {
        public FrozenDictionary<FilterType, Func<EventBaseModel, object, bool>> Functors
            => new Dictionary<FilterType, Func<EventBaseModel, object, bool>>()
        {
            { FilterType.ByCategory, (model, value) => model.Category.Name == (string)value },
            { FilterType.ByMaxDate, (model, value) => model.Date <= (DateTime)value },
            { FilterType.ByMinDate, (model, value) => model.Date >= (DateTime)value },
            { FilterType.ByMinPrice, (model, value) => model.Price >= (double)value },
            { FilterType.ByMaxPrice, (model, value) => model.Price <= (double)value },
            { FilterType.ByPlace, (model, value) => model.Place == (string)value    },
            { FilterType.ByName, (model, value) => model.Name.Contains((string)value,
                StringComparison.InvariantCultureIgnoreCase) }
        }.ToFrozenDictionary();

        public IQueryable<EventBaseModel> Filter(IQueryable<EventBaseModel> collection, FilterType property, object filterValue)
        {
            Func<EventBaseModel, object, bool> functor = TryGetFunctor(property);
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

            if (filterOptions is null || filterOptions?.Count == 0)
                return source;

            foreach (FilterOption filterOption in filterOptions)
            {
                if (filterOption.Value is not null)
                {
                    Func<EventBaseModel, object, bool> functor = TryGetFunctor(filterOption.FilterType);
                    Func<EventBaseModel, bool> sortFunctor = model => functor(model, filterOption.Value);

                    source = source.ToList().Where(sortFunctor).AsQueryable();
                }
            }

            return source;
        }

        private Func<EventBaseModel, object, bool> TryGetFunctor(FilterType filterProperty)
        {
            if (Functors.TryGetValue(filterProperty, out var functor))
            {
                return functor;
            }

            return (model, value) => true is true;
        }
    }
}
