using TestTask.model;

namespace TestTask.service
{
    internal interface IUserService
    {
        public User getById(Guid id);

        public List<User> getByDomain(string domain, int page = 1, int limit = 10);

        public List<User> getAllByTagAndDomain(string tag, string domain);
    }
}
