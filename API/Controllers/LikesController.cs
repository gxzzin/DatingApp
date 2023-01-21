using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork _uof;

        public LikesController(IUnitOfWork uof)
        {
            _uof = uof;
        }


        [HttpPost("{userName}")]
        public async Task<ActionResult> AddLike(string userName)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _uof.UserRepository.GetUserByUserNameAsync(userName);
            var sourceUser = await _uof.LikeRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.UserName == userName) return BadRequest("You cannot like yourself.");

            var userLike = await _uof.LikeRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);
            if(await _uof.Complete()) return Ok();

            return BadRequest("Faild to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLike([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _uof.LikeRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }

}