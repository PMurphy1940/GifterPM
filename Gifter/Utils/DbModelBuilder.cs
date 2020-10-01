using Gifter.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gifter.Utils
{
    public static class DbModelBuilder
    {
        public static Post BuildPostModel(SqlDataReader reader)
        {
            Post post = new Post()
            {
                Id = DbUtils.GetInt(reader, "PostId"),
                Title = DbUtils.GetString(reader, "Title"),
                Caption = DbUtils.GetString(reader, "Caption"),
                DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                UserProfile = new UserProfile(),
                Comments = new List<Comment>()
            };

            return post;
        }
        public static UserProfile BuildUserProfile(SqlDataReader reader)
        {
            var profile = new UserProfile
            {
                Id = DbUtils.GetInt(reader, "ProfileId"),
                Name = DbUtils.GetString(reader, "Name"),
                Email = DbUtils.GetString(reader, "Email"),
                ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                Bio = DbUtils.GetString(reader, "Bio")
            };

            return profile;
        }

        public static Comment BuildCommentModel(SqlDataReader reader)
        {
            var comment = new Comment()
            {
                Id = DbUtils.GetInt(reader, "CommentId"),
                Message = DbUtils.GetString(reader, "Message"),
                PostId = DbUtils.GetInt(reader, "CommentPostId"),
                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
            };
            return comment;
        }
    }
}
