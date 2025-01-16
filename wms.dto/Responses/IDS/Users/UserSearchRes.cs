using wms.dto.Common;

namespace wms.dto.Responses
{
    public class UserSearchRes : BaseRes
    {
        public int TotalRecord { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }

        /// <summary>
        /// 1: Male | 2: Female
        /// </summary>
        public int Gender { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }
    }
}
