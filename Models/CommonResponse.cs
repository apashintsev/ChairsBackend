namespace ChairsBackend.Models
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public T Payload { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public BaseResponse(T payload)
        {
            Success = true;
            Payload = payload;
            Errors = Enumerable.Empty<string>();
        }

        public void AddError(string message)
        {
            Success = false;
            if (Errors != null)
            {
                Errors = Errors.Concat(new[] { message });
            }
            else
            {
                Errors = new[] { message };
            }
        }

        public void AddErrors(IEnumerable<string> messages)
        {
            Success = false;
            if (Errors != null)
            {
                Errors = Errors.Concat(messages);
            }
            else
            {
                Errors = messages;
            }
        }
    }
}
