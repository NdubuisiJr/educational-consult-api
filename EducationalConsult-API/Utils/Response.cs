namespace EducationalConsultAPI.Utils {
    public class Response<T> where T: new() {
        public Response(int status, string message, T data = default) {
            Status = status;
            Message = message;
            Data = data;
        }

        public int Status { get; }
        public string Message { get; }
        public T Data { get; }
    }
}
