using System;
using System.Collections.Generic;
using System.Linq;
using Gifter.Models;
using Gifter.Repositories;

namespace Gifter.Tests.Mocks
{
    class InMemoryUserProfileRepository : IUserProfileRepository
    {
        private readonly List<UserProfile> _data;

        public List<UserProfile> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryUserProfileRepository(List<UserProfile> startingData)
        {
            _data = startingData;
        }

        public void Add(UserProfile profile)
        {
            var lastProfile = _data.Last();
            profile.Id = profile.Id + 1;
            _data.Add(profile);
        }

        public void Delete(int id)
        {
            var profileTodelete = _data.FirstOrDefault(p => p.Id == id);
            if (profileTodelete == null)
            {
                return;
            }

            _data.Remove(profileTodelete);
        }

        public List<UserProfile> GetAll()
        {
            return _data;
        }

        public UserProfile GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public void Update(UserProfile profile)
        {
            var currentProfile = _data.FirstOrDefault(p => p.Id == profile.Id);
            if (currentProfile == null)
            {
                return;
            }

            currentProfile.Name = profile.Name;
            currentProfile.Email = profile.Email;
            currentProfile.DateCreated = profile.DateCreated;
            currentProfile.ImageUrl = profile.ImageUrl;
            currentProfile.DateCreated = profile.DateCreated;
            currentProfile.Bio = profile.Bio;
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            throw new NotImplementedException();
        }
        public UserProfile GetByIdWithPosts(int Id)
        {
            throw new NotImplementedException();
        }

    }
}