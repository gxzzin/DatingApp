using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await _context.AppUsers.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<MemberDTO> GetMemberAsync(string userName)
        {
            return await _context.AppUsers.Where(x => x.UserName == userName)
                                          .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                                          .SingleOrDefaultAsync();
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await _context.AppUsers.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            return await _context.AppUsers.Include(x => x.Photos)
                                          .SingleOrDefaultAsync(x => x.UserName == userName.ToLower());
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.AppUsers
                                .Include(x => x.Photos)
                                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}