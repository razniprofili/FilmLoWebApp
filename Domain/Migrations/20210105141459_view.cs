using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class view : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE view WatchedMoviestStats as
                                   SELECT s.Id userId, 
                                   COUNT(ss.UserId) totalCount, 
                                   ISNULL(sum(m.Duration), 0)  totalTime,
                                   ISNULL(ROUND(AVG(CAST(ss.Rating AS FLOAT)), 2), 0) averageRate
                                   FROM WatchedMovie ss right join [User] s 
                                   ON (s.Id=ss.UserId) left join MovieDetailsJMDBApi m 
                                   ON (m.MovieJMDBApiId = ss.MovieJMDBApiId)
                                   GROUP BY s.Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP view WatchedMoviestStats");
        }
    }
}
