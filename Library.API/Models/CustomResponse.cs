namespace Library.API.Models
{
    public class CustomResponse<Q>
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public Q Result { get; set; }
    }
}
