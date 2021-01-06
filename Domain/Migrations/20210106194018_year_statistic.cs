using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class year_statistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE view YearStatistic as
                                   SELECT u.Id AS userId, 
                                          right(ss.WatchingDate, 5) AS watchingYear, 
                                          COUNT(*) AS totalMovies
                                   FROM WatchedMovie ss left join [User] u 
                                   ON (u.Id=ss.UserId) left join MovieDetailsJMDBApi m 
                                   ON (m.MovieJMDBApiId = ss.MovieJMDBApiId)
                                   GROUP BY right(ss.WatchingDate, 5), u.Id
                                   HAVING COUNT(*) > 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP view YearStatistic");
        }
    }
}
