namespace LeaveManagementSystem.Infrastructure.Exceptions
{
    public class BadRequestException : Exception
    {
        public int StatusCode { get; }

        public BadRequestException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
