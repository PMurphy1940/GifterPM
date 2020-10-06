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
        /// <summary>
        /// Builds Post object from data reader object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Instance of a Post object</returns>
        public static Post BuildPostModel(SqlDataReader reader)
        {
            Post post = new Post()
            {
                Id = DbUtils.GetInt(reader, "PostId"),
                Title = DbUtils.GetString(reader, "Title"),
                Caption = DbUtils.GetString(reader, "Caption"),
                DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                UserProfileId = DbUtils.GetInt(reader, "UserProfileId")                
            };

            return post;
        }

        /// <summary>
        /// Builds UserProfile object from data reader object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Instance of a UserProfile object</returns>
        public static UserProfile BuildUserProfile(SqlDataReader reader)
        {
            var profile = new UserProfile
            {
                Id = DbUtils.GetInt(reader, "ProfileId"),
                FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                Name = DbUtils.GetString(reader, "Name"),
                Email = DbUtils.GetString(reader, "Email"),
                ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                Bio = DbUtils.GetString(reader, "Bio")
            };

            return profile;
        }

        /// <summary>
        /// Builds Comment object from data reader object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Instance of a Comment object</returns>
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
