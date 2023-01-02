namespace Services
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public int Code { get; set; }
        public string ErrorMessage { get; set; } = String.Empty;
    }
}
