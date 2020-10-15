using Gifter.Models;
using Gifter.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gifter.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT  Id, 
                                                [Name], 
                                                Email, 
                                                ImageUrl, 
                                                Bio, 
                                                DateCreated
                                        FROM UserProfile
                                        ";
                    var reader = cmd.ExecuteReader();
                    var users = new List<UserProfile>();
                    while (reader.Read())
                    {
                        users.Add(new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            Bio = DbUtils.GetString(reader, "Bio"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
                        }
                        );
                    }
                    reader.Close();
                    return users;

                }
            }
        }

        public UserProfile GetById(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT  Id, 
                                                [Name], 
                                                Email, 
                                                ImageUrl, 
                                                Bio, 
                                                DateCreated
                                        FROM UserProfile
                                        WHERE Id = @id
                                        ";
                    cmd.Parameters.AddWithValue("@id", Id);
                    var reader = cmd.ExecuteReader();
                    UserProfile user = null;
                    if (reader.Read())
                    {
                        user = new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            Bio = DbUtils.GetString(reader, "Bio"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
                        };
                        
                    }
                    reader.Close();
                    return user;

                }
            }
        }
        public UserProfile GetByIdWithPosts(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT  up.Id, 
                                                [Name], 
                                                Email, 
                                                up.ImageUrl AS UserImageUrl, 
                                                Bio, 
                                                up.DateCreated AS UserProfileCreateDate,
                                                
                                                p.Id AS PostId,
                                                p.Title,
                                                p.ImageUrl AS PostImageUrl,
                                                p.Caption,
                                                p.DateCreated AS PostDateCreated,

                                                c.Id AS CommentId,
                                                c.Message,
                                                c.UserProfileId AS CommentUserId
                                        FROM UserProfile up
                                        LEFT JOIN Post p ON p.UserProfileId = up.Id
                                        LEFT JOIN Comment c ON c.PostId = p.Id
                                        WHERE up.Id = @id
                                        ";
                    cmd.Parameters.AddWithValue("@id", Id);
                    var reader = cmd.ExecuteReader();
                    UserProfile user = null;
                    while (reader.Read())
                    {
                        if (user == null)
                        {
                            user = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                ImageUrl = DbUtils.GetString(reader, "UserImageUrl"),
                                Bio = DbUtils.GetString(reader, "Bio"),
                                DateCreated = DbUtils.GetDateTime(reader, "UserProfileCreateDate"),
                                Posts = new List<Post>(),
                                
                            };
                        var postId = DbUtils.GetInt(reader, "PostId");
                        var existingPost = user.Posts.FirstOrDefault(p => p.Id == postId);
                        if (existingPost == null)
                            {
                                existingPost = new Post()
                                {
                                    Id = postId,
                                    Title = DbUtils.GetString(reader, "Title"),
                                    Caption = DbUtils.GetString(reader, "Caption"),
                                    ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                                    DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                                    Comments = new List<Comment>()
                                };
         
                                user.Posts.Add(existingPost);
                            }

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                            {
                                existingPost.Comments.Add(new Comment()
                                {
                                    Id = DbUtils.GetInt(reader, "CommentId"),
                                    PostId = postId,
                                    Message = DbUtils.GetString(reader, "Message"),
                                    UserProfileId = DbUtils.GetInt(reader, "CommentUserId")
                                });
                            }
                        }

                    }
                    reader.Close();
                    return user;

                }
            }
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using(var conn = Connection)
            {
                conn.Open();
                using( var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT"
                                        + UserProfileSqlCommandText;
                    cmd.CommandText += "From UserProfile up" +
                        "               WHERE up.firebaseUserId LIKE @firebaseUserId";

                    DbUtils.AddParameter(cmd, "@FirebaseUserId", firebaseUserId);

                    var reader = cmd.ExecuteReader();

                    UserProfile user = null;

                    if (reader.Read())
                    {
                        user = DbModelBuilder.BuildUserProfile(reader);
                    }

                    reader.Close();

                    return user;
                }
            }

        }
        public void Add(UserProfile profile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO UserProfile (FirebaseUserId, Name, Email, ImageUrl, Bio, DateCreated)
                                        OUTPUT INSERTED.Id
                                        VALUES (@FirebaseUserId, @Name, @Email, @ImageUrl, @Bio, @DateCreated)
                                        ";
                    DbUtils.AddParameter(cmd, "@FirebaseUserId", profile.FirebaseUserId);
                    DbUtils.AddParameter(cmd, "@Name", profile.Name);
                    DbUtils.AddParameter(cmd, "@Email", profile.Email);
                    DbUtils.AddParameter(cmd, "@ImageUrl", profile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@Bio", profile.Bio);
                    DbUtils.AddParameter(cmd, "@profile.Name", profile.DateCreated);

                    profile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void Update(UserProfile profile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        UPDATE UserProfile
                                            SET Name = @Name,
                                                Email = @Email,
                                                ImageUrl = @ImageUrl,
                                                Bio = @Bio, 
                                                DateCreated = @DateCreated
                                        WHERE Id = @id
                                        ";
                    DbUtils.AddParameter(cmd, "@Name", profile.Name);
                    DbUtils.AddParameter(cmd, "@Email", profile.Email);
                    DbUtils.AddParameter(cmd, "@ImageUrl", profile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@Bio", profile.Bio);
                    DbUtils.AddParameter(cmd, "@profile.Name", profile.DateCreated);
                    DbUtils.AddParameter(cmd, "@id", profile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
