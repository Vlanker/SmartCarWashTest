namespace SmartCarWashTest.CRUD.WebApi.Infrastructure.Responses
{
    /// <summary>
    /// JSON Error response.
    /// </summary>
    /// <param name="Messages">Message.</param>
    public record JsonErrorResponse(string[] Messages)
    {
        /// <summary>
        /// Developer message.
        /// </summary>
        public object DeveloperMessage { get; set; } = null!;
    }
}