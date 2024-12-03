namespace RestService.models.reverse
{
    public record CancelCommand(int MerchantConfigurationId, ulong payGateTranId);
   
}