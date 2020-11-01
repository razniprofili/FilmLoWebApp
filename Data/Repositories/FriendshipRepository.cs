using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class FriendshipRepository : GenericRepository<Friendship>
    {
        public FriendshipRepository(DbContext context) : base(context)
        {

        }
    }
}
