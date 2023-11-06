using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.extension;
using TestTask.model;
using TestTask.repository;

namespace TestTask.service
{
    internal class StreamLoader : IStreamLoader
    {
        private AppDbContext _appDbContext;

        public StreamLoader(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        // Предположим, входные данные не вмещаются в оперативную память компьютера и не могут быть прочтены за раз
        // В таком случае можно использовать поток и считывать данные построчно, затем сохранять каждую строку в базу данных
        // Эта идея и реализована в методе ниже. Также, так как размер входных данных заранее неизвестен
        // Я решил очищать контекст базы данных в конце каждого цикла для избежания проблем с потреблением памяти
        public int Load(Stream stream, Encoding encoding, char lineSplitter, char columnSplitter)
        {
            _appDbContext.Users.RemoveRange(_appDbContext.Users.ToList());
            _appDbContext.Tags.RemoveRange(_appDbContext.Tags.ToList());
            _appDbContext.SaveChanges();

            int processed = 0;

            using (var reader = new StreamReader(stream, encoding))
            {
                foreach (var line in reader.ReadLines(lineSplitter))
                {
                    var columns = line.Split(columnSplitter);

                    var user = new User()
                    {
                        UserId = Guid.Parse(columns[0]),
                        Name = columns[1],
                        Domain = columns[2]
                    };

                    var tag = new Tag()
                    {
                        TagId = Guid.Parse(columns[3]),
                        Value = columns[4],
                        Domain = columns[5]
                    };


                    if (!_appDbContext.Tags.Any(x => x.TagId == tag.TagId))
                    {
                        _appDbContext.Tags.Add(tag);
                    }
                    else
                    {
                        _appDbContext.Entry(tag).State = EntityState.Unchanged;
                    }

                    if (!_appDbContext.Users.Any(x => x.UserId == user.UserId))
                    {
                        _appDbContext.Users.Add(user);
                        user.Tags.Add(tag);
                    }
                    else
                    {
                        user.Tags.Add(tag);
                        _appDbContext.Entry(user).State = EntityState.Modified;
                    }

                    _appDbContext.SaveChanges();
                    _appDbContext.ChangeTracker.Clear();
                    processed++;
                }

            }

            return processed;
        }
    }
}
