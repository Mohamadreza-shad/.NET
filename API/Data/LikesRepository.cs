using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<LikeUser> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams)
        {
            var users = _context.Users.AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if (likeParams.Predicate == "Liked")
            {
                likes = likes.Where(xx => xx.SourceUserId == likeParams.UserId);
                users = likes.Select(likes => likes.LikedUser);
            }

            if (likeParams.Predicate == "LikedBy")
            {
                likes = likes.Where(xx => xx.LikedUserId == likeParams.UserId);
                users = likes.Select(likes => likes.SourceUser);
            }

            var usersToReturn = users.Select(user => new LikeDto
            {
                Id = user.Id,
                City = user.City,
                Username = user.UserName,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(xx => xx.IsMain).Url,
                KnownAs = user.KnownAs
            });

            return await PagedList<LikeDto>.CreateAsync(usersToReturn
                ,likeParams.PageNumber,likeParams.PageSize); 
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(xx => xx.LikedUsers)
                .FirstOrDefaultAsync(xx => xx.Id == userId);
        }
    }
}