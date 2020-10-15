using System;
using Microsoft.AspNetCore.Mvc;
using Gifter.Repositories;
using Gifter.Models;
using Microsoft.AspNetCore.Authorization;

namespace Gifter.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _UserProfileRepository;
        public UserProfileController(IUserProfileRepository UserProfileRepository)
        {
            _UserProfileRepository = UserProfileRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_UserProfileRepository.GetAll());
        }

        [HttpGet("{firebaseUserId}")]
        public IActionResult GetByFirebaseUserId(string firebaseUserId)
        {
            var userProfile = _UserProfileRepository.GetByFirebaseUserId(firebaseUserId);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _UserProfileRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("getwithposts/{id}")]
        public IActionResult GetWithPosts(int id)
        {
            var user = _UserProfileRepository.GetByIdWithPosts(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public IActionResult Post(UserProfile profile)
        {
            _UserProfileRepository.Add(profile);
            return CreatedAtAction("Get", new { id = profile.Id }, profile);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UserProfile profile)
        {
            if (id != profile.Id)
            {
                return BadRequest();
            }
            _UserProfileRepository.Update(profile);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _UserProfileRepository.Delete(id);
            return NoContent();
        }
    }
}
