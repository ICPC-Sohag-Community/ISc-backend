namespace ISc.Application.Dtos.CodeForce
{
    public class CodeForceRequestDto<T>
    {
        public T Data { get; set; }
        public bool IsSohag { get; set; }
        public string? ApiKey { get; set; }
        public string? ApiSecret { get; set; }
        public CodeForceRequestDto()
        {
            IsSohag = true;
        }
        public CodeForceRequestDto(T data) : base()
        {
            Data = data;
        }
    }
}
