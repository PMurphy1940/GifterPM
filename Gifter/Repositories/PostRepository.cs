using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Gifter.Models;
using Gifter.Utils;

namespace Gifter.Repositories
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        public PostRepository(IConfiguration configuration) : base(configuration) { }

        /*public List<Post> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                SELECT " 
                                    + PostSqlCommandText 
                                    + "," 
                                    + UserProfileSqlCommandText 
                                    + " FROM Post p" 
                                    + AddUserToPost
                                    + " ORDER BY p.DateCreated";

                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        var aPost = DbModelBuilder.BuildPostModel(reader);
                        aPost.UserProfile = DbModelBuilder.BuildUserProfile(reader);
                        posts.Add(aPost);
                    }

                    reader.Close();

                    return posts;
                }
            }
        }*/

/*        public List<Post> GetAllWithComments()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT "
                                    + PostSqlCommandText
                                    + ","
                                    + UserProfileSqlCommandText
                                    + ","
                                    + CommentSqlCommandText
                                    + " FROM Post p"
                                    + AddUserToPost
                                    + AddComment
                                    + " ORDER BY p.DateCreated";

                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        var postId = DbUtils.GetInt(reader, "PostId");

                        var existingPost = posts.FirstOrDefault(p => p.Id == postId);
                        if (existingPost == null)
                        {
                            existingPost = DbModelBuilder.BuildPostModel(reader);
                            existingPost.UserProfile = DbModelBuilder.BuildUserProfile(reader);

                            posts.Add(existingPost);
                        }

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            existingPost.Comments.Add(DbModelBuilder.BuildCommentModel(reader));
                        }
                    }

                    reader.Close();

                    return posts;
                }
            }
        }*/
        public List<Post> GetAll(string q, bool profile, bool comments, string since)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                  
                    cmd.CommandText = @"
                                SELECT "
                                + PostSqlCommandText;

                        if (profile)
                        {
                            cmd.CommandText += ",";
                            cmd.CommandText += UserProfileSqlCommandText;
                        }
                        if (comments)
                        {
                            cmd.CommandText += ",";
                            cmd.CommandText += CommentSqlCommandText;
                        }                 
                    cmd.CommandText += " FROM Post p";
                    
                        if (profile)
                        {
                            cmd.CommandText += AddUserToPost;
                        }
                        if (comments)
                        {
                            cmd.CommandText += AddComment;
                        }
                        if (since != null)
                        {
                        cmd.CommandText += FromDate(cmd, since);
                        }
                    
                    cmd.CommandText += " ORDER BY p.DateCreated";
 

                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        var postId = DbUtils.GetInt(reader, "PostId");

                        var existingPost = posts.FirstOrDefault(p => p.Id == postId);
                        if (existingPost == null)
                        {
                            existingPost = DbModelBuilder.BuildPostModel(reader);
                            if (profile)
                                {
                                    existingPost.UserProfile = DbModelBuilder.BuildUserProfile(reader);
                                }
                            if (comments)
                            {
                                existingPost.Comments = new List<Comment>();
                            }

                            posts.Add(existingPost);
                        }
                        if (comments)
                        {
                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                            {
                                existingPost.Comments.Add(DbModelBuilder.BuildCommentModel(reader));
                            }
                        }
                    }

                    reader.Close();

                    return posts;
                }
            }
        }
        //Slim down GetById
        public Post GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT "
                                        + PostSqlCommandText
                                        + ","
                                        + UserProfileSqlCommandText
                                        + " FROM Post p"
                                        + AddUserToPost
                                        + " WHERE p.Id = @Id";


                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    Post post = null;
                    if (reader.Read())
                    {
                        post = DbModelBuilder.BuildPostModel(reader);
                        post.UserProfile = DbModelBuilder.BuildUserProfile(reader);
                        
                    }

                    reader.Close();

                    return post;
                }
            }
        }

        public Post GetByIdWithComments(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT "
                                    + PostSqlCommandText
                                    + ","
                                    + UserProfileSqlCommandText
                                    + ","
                                    + CommentSqlCommandText
                                    + " FROM Post p"
                                    + AddUserToPost
                                    + AddComment
                                    + " WHERE p.Id = @Id";


                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    Post post = null;
                    while (reader.Read())
                    {
                        if (post == null)
                        {
                            post = DbModelBuilder.BuildPostModel(reader);
                            post.UserProfile = DbModelBuilder.BuildUserProfile(reader);
                        };
                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            post.Comments.Add(DbModelBuilder.BuildCommentModel(reader));
                        }                        
                    }

                    reader.Close();

                    return post;
                }
            }
        }

        public List<Post> Search(string criterion, bool sortDescending)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var sql =
                        @"SELECT "
                            + PostSqlCommandText
                            + ","
                            + UserProfileSqlCommandText
                            + " FROM Post p"
                            + AddUserToPost
                            + " WHERE p.Title LIKE @Criterion OR p.Caption LIKE @Criterion";

                    if (sortDescending)
                    {
                        sql += " ORDER BY p.DateCreated DESC";
                    }
                    else
                    {
                        sql += " ORDER BY p.DateCreated";
                    }

                    cmd.CommandText = sql;
                    DbUtils.AddParameter(cmd, "@Criterion", $"%{criterion}%");
                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        var aPost = DbModelBuilder.BuildPostModel(reader);
                        aPost.UserProfile = DbModelBuilder.BuildUserProfile(reader);
                        posts.Add(aPost);                        
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public void Add(Post post)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Post (Title, Caption, DateCreated, ImageUrl, UserProfileId)
                        OUTPUT INSERTED.ID
                        VALUES (@Title, @Caption, @DateCreated, @ImageUrl, @UserProfileId)";

                    DbUtils.AddParameter(cmd, "@Title", post.Title);
                    DbUtils.AddParameter(cmd, "@Caption", post.Caption);
                    DbUtils.AddParameter(cmd, "@DateCreated", post.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", post.ImageUrl);
                    DbUtils.AddParameter(cmd, "@UserProfileId", post.UserProfileId);

                    post.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Post post)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Post
                           SET Title = @Title,
                               Caption = @Caption,
                               DateCreated = @DateCreated,
                               ImageUrl = @ImageUrl,
                               UserProfileId = @UserProfileId
                         WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Title", post.Title);
                    DbUtils.AddParameter(cmd, "@Caption", post.Caption);
                    DbUtils.AddParameter(cmd, "@DateCreated", post.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", post.ImageUrl);
                    DbUtils.AddParameter(cmd, "@UserProfileId", post.UserProfileId);
                    DbUtils.AddParameter(cmd, "@Id", post.Id);

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
                    cmd.CommandText = "DELETE FROM Post WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}