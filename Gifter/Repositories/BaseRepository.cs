using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Gifter.Repositories
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        protected SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }
        //Sql command text presets
        protected static string PostSqlCommandText => @" p.Id AS PostId, 
                                                        p.Title, 
                                                        p.Caption, 
                                                        p.DateCreated AS PostDateCreated, 
                                                        p.ImageUrl AS PostImageUrl, 
                                                        p.UserProfileId";
        protected static string UserProfileSqlCommandText => @" up.id AS ProfileId,
                                                                up.Name, 
                                                                up.Bio, 
                                                                up.Email, 
                                                                up.DateCreated AS UserProfileDateCreated, 
                                                                up.ImageUrl AS UserProfileImageUrl";
        protected static string CommentSqlCommandText => @" c.Id AS CommentId, 
                                                            c.Message, 
                                                            c.UserProfileId AS CommentUserProfileId,
                                                            c.PostId AS CommentPostId";
        protected static string AddUserToPost => " LEFT JOIN UserProfile up ON p.UserProfileId = up.id";
        protected static string AddComment => " LEFT JOIN Comment c on c.PostId = p.id";
    }
}