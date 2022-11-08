namespace API.Errors
{
    public class ApiResponse
    {
        // message = null x si no hay
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        // ?? returns the value of its left-hand operand if it isn't null; otherwise, it evaluates the right-hand operand and returns its result. 
        // si message el null => GetDefaultMessageForStatusCode()
        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        public int StatusCode { get; set; }
        public string Message { get; set; }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side.  Errors lead to anger.   Anger leads to hate.  Hate leads to career change.",
                _ => null
            };
        }
    }
}
