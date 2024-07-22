using Application.CollectionServices;
using Entities.AppSettings;
using Entities.Enums;
using Entities.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;

namespace Tests.RepositoryTests
{
    public class CategoriesRepositoryTests
    {
        private readonly Mock<EventContext> _contextMock = new(new DbContextOptions<EventContext>());
        private readonly Mock<IFilterService<EventCategory>> _filterMock = new();
        private readonly Mock<ISortService<EventCategory>> _sortMock = new();


        [Theory]
        [InlineData("Festival")]
        [InlineData("Concert")]
        [InlineData("Conference")]
        public async Task Get_Category_By_Existing_Name(string name)
        {
            var dbSetMock = new Mock<DbSet<EventCategory>>();
            var fakeCategories = GetFakeCategories().AsQueryable().BuildMock();

            dbSetMock.SetSource(fakeCategories);

            _contextMock.Setup(c => c.Categories).Returns(dbSetMock.Object);

            CategoryRepository repo = new(
                _contextMock.Object,
                Mock.Of<ILogger<CategoryRepository>>(),
                Options.Create(new AppSettings()),
                _filterMock.Object,
                _sortMock.Object);

            var c = dbSetMock.Object.ToList();
            EventCategory? category = await repo.GetCategoryByNameAsync(name, default);
            Assert.NotNull(category);
        }

        [Theory]
        [InlineData("New category")]
        [InlineData("Nonexisting")]
        [InlineData("Undefined")]
        public async Task Get_Category_By_Non_Existing_Name(string name)
        {
            var dbSetMock = new Mock<DbSet<EventCategory>>();
            var fakeCategories = GetFakeCategories().AsQueryable().BuildMock();

            dbSetMock.SetSource(fakeCategories);

            _contextMock.Setup(c => c.Categories).Returns(dbSetMock.Object);

            CategoryRepository repo = new(
                _contextMock.Object,
                Mock.Of<ILogger<CategoryRepository>>(),
                Options.Create(new AppSettings()),
                _filterMock.Object,
                _sortMock.Object);

            EventCategory? category = await repo.GetCategoryByNameAsync(name, default);
            Assert.Null(category);
        }


        [Fact]
        public async Task Get_All_Categories()
        {
            var dbSetMock = new Mock<DbSet<EventCategory>>();
            var fakeCategories = GetFakeCategories().AsQueryable().BuildMock();

            dbSetMock.SetSource(fakeCategories);

            _filterMock.Setup(f => f.FilterWithManyOptions(It.IsAny<IQueryable<EventCategory>>(), It.IsAny<List<FilterOption>>()))
                .Returns(dbSetMock.Object);

            _sortMock.Setup(s => s.Sort(It.IsAny<IQueryable<EventCategory>>(), It.IsAny<SortType>(), It.IsAny<SortOrder>()))
                .Returns(dbSetMock.Object);

            _contextMock.Setup(c => c.Categories).Returns(dbSetMock.Object);

            CategoryRepository repo = new(
                _contextMock.Object,
                Mock.Of<ILogger<CategoryRepository>>(),
                Options.Create(new AppSettings()),
                _filterMock.Object,
                _sortMock.Object);

            var categories = await repo.GetAllCategoriesAsync();

            Assert.True(categories is not null);
            Assert.Equal(5, categories.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task Get_Existing_Category_By_Id(int id)
        {
            var dbSetMock = new Mock<DbSet<EventCategory>>();
            var fakeCategories = GetFakeCategories().AsQueryable();

            dbSetMock.SetSource(fakeCategories);
            dbSetMock.Setup(m => m.FindAsync(It.IsAny<int>()))
                .ReturnsAsync(dbSetMock.Object.FirstOrDefault(c => c.Id == id));
            _contextMock.Setup(c => c.Categories).Returns(dbSetMock.Object);

            CategoryRepository repo = new(
                _contextMock.Object,
                Mock.Of<ILogger<CategoryRepository>>(),
                Options.Create(new AppSettings()),
                _filterMock.Object,
                _sortMock.Object);

            var category = await repo.GetByIdAsync(id, default);

            Assert.True(category is not null);
            Assert.Equal(id, category.Id);
        }

        private List<EventCategory> GetFakeCategories()
            => [
                new() { Id = 1, Name = "Concert" },
                new() { Id = 2, Name = "Festival" },
                new() { Id = 3, Name = "Conference" },
                new() { Id = 4, Name = "OpenAir" },
                new() { Id = 5, Name = "Meetup" }
               ];
    }
}
