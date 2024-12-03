namespace RestService.models.settle
{
    public class CancelResultVm : IResponseVm
    {
        public int ResCode { get; set; }
        public string ResMessage { get; set; }
    }

}