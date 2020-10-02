using Gifter.Models;
using System.Collections.Generic;

namespace Gifter.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        void Delete(int id);
        List<Post> GetAll(string q, bool profile, bool comments, string since);
       // List<Post> GetAllWithComments();
        Post GetById(int id);
        Post GetByIdWithComments(int id);
       // List<Post> GetDynamic(bool profile, bool comments);
        List<Post> Search(string criterion, bool sortDescending);
        void Update(Post post);
    }
}