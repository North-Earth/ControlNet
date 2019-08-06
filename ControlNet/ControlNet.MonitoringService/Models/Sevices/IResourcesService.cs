namespace ControlNet.MonitoringService.Models.Sevices
{
    public interface IResourcesService
    {
        #region Methods

        string GetResource(string resourceKey);

        void SetResource(string resourceKey, string resourceValue);

        #endregion
    }
}