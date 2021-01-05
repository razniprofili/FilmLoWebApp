using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class popmoviesview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE view PopularMovies as
                                   SELECT u.Id, m.MovieJMDBApiId, m.Name
                                   FROM WatchedMovie ss right join [User] u 
                                   ON (u.Id=ss.UserId) left join MovieDetailsJMDBApi m 
                                   ON (m.MovieJMDBApiId = ss.MovieJMDBApiId)
                                   WHERE ss.Rating = 5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP view PopularMovies");
        }
    }
}
