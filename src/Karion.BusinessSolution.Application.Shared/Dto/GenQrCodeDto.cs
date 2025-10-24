namespace Karion.BusinessSolution.Dto
{
    public class QRInputDto
    {
        public int amount { get; set; }
        public string bankAccount { get; set; }
        public string bankCode { get; set; }
        public string noiDung { get; set; }
    }

    public class GeneratorInput
    {
        public int amount { get; set; }
        public string bankAccount { get; set; }
        public string bankCode { get; set; }
        public string noiDung { get; set; }
        public string token { get; set; }
    }

    public class DataGeneratorOutput
    {
        public bool status { get; set; }
        public string qrCodeString { get; set; }
        public string messenger { get; set; }
    }
    public class GeneratorOutput
    {
        public int errorCode { get; set; }
        public DataGeneratorOutput data { get; set; }
    }
}