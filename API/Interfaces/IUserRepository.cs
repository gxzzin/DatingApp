using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
         void Update (AppUser user);

         Task<bool> SaveAllAsync();

         Task<IEnumerable<AppUser>> GetUsersAsync();

         Task<AppUser> GetUserById(int id);

         Task<AppUser> GetUserByUserNameAsync(string userName);

         Task<IEnumerable<MemberDTO>> GetMembersAsync();

         Task<MemberDTO> GetMemberAsync(string userName);
    }
}