namespace WebClient.Services;

public interface IEmployeeApiService
{
    Task<List<EmployeeApiModel>> GetEmployees();
}
