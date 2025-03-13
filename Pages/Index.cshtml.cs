using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace learningapp.Pages;

public class IndexModel : PageModel
{
    public List<Course> Courses = new List<Course>();
    private readonly ILogger<IndexModel> _logger;
    private IConfiguration _configuration;
    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void OnGet()
    {

        string connectionString = _configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
        var sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();

        var sqlcommand = new SqlCommand(
        "SELECT CourseID,CourseName,Rating FROM Course;", sqlConnection);
        using (SqlDataReader sqlDatareader = sqlcommand.ExecuteReader())
        {
            while (sqlDatareader.Read())
            {
                Courses.Add(new Course()
                {
                    CourseID = sqlDatareader["CourseID"] != DBNull.Value ? Convert.ToInt32(sqlDatareader["CourseID"]) : 0,
                    CourseName = sqlDatareader["CourseName"]?.ToString(),
                    Rating = sqlDatareader["Rating"] != DBNull.Value ? Convert.ToDecimal(sqlDatareader["Rating"]) : 0.0m
                });
            }
        }
    }
}
