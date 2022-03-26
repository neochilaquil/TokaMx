namespace TEST_DEV_SHN_24_Marzo_2022.Models
{
    public class ErrorModel
    {
        public string? Error { get; set; }
        public string? MENSAJEERROR { get; set; }
        public int? Id { get; set; }
        public int? IdError { get; set; }
        public List<Exception>? LstError { get; set; }
        public bool? IfExist { get; set; }
    }
}
