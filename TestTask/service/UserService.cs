using Microsoft.EntityFrameworkCore;
using TestTask.model;
using TestTask.repository;

namespace TestTask.service
{
    internal class UserService : IUserService
    {

        private AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        // Реализация поиска пользователя с использованием EF API
        public User getById(Guid id)
        {
            return _appDbContext.Users.Include(x => x.Tags).FirstOrDefault(x => x.UserId.Equals(id));
        }

        // Реализация поиска пользователя с использованием EF API
        public List<User> getByDomain(string domain, int page = 1, int limit = 10)
        {
            page--;
            if (page < 0)
            {
                page = 0;
            }

            var skip = page * limit;

            return _appDbContext.Users
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Where(x => x.Domain.Equals(domain))
                .Skip(skip)
                .Take(limit)
                .Include(x => x.Tags)
                .ToList();
        }

        // Реализация поиска пользователя с использованием EF API
        public List<User> getAllByTagAndDomain(string tag, string domain)
        {
            return _appDbContext.Users
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Where(x => x.Domain.Equals(domain))
                .Include(x => x.Tags)
                .Where(x => x.Tags.Select(x => x.Value).Contains(tag))
                .ToList();
        }
    }
}
