using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likeRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likeRepository)
        {
            _likeRepository = likeRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();

            var likedUser = await _userRepository.GetUserByUsernameAsync(username);

            var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("You can not like yourself");

            var likeUser = await _likeRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (likeUser != null) return BadRequest("You already like this user");

            likeUser = new LikeUser
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(likeUser);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like the user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikeParams likeParams)
        {
            likeParams.UserId = User.GetUserId();
            var usersToReturn = await _likeRepository.GetUserLikes(likeParams);

            Response.AddPaginationHeader(usersToReturn.CurrentPage,
                usersToReturn.PageSize, usersToReturn.TotalCount, usersToReturn.TotalPages);

            return Ok(usersToReturn);
        }

    }
}