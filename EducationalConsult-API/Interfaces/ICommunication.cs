namespace EducationalConsultAPI.Interfaces {
    public interface ICommunication {
        bool SendEmail(string to, string subject, string body, bool isHtml = true);
    }
}