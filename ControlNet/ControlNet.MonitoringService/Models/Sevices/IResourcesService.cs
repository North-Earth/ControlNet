namespace ControlNet.MonitoringService.Models.Sevices
{
    public interface IResourcesService
    {
        string GetResource(string resourceKey);
        void SetResource(string resourceKey, string resourceValue);
    }
}