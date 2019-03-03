using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class UserResponse
    {
        public UserResponse(User user)
        {
            this.User = user;
            this.Succeeded = true;
        }

        public User User { get; }
        public bool Succeeded { get; }
    }
}
