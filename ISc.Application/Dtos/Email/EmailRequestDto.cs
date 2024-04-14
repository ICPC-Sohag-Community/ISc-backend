namespace ISc.Application.Dtos.Email
{
    public class EmailRequestDto
    {
        public string To { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public object BodyData { get; set; }
        public string From { get; set; }
    }
}
