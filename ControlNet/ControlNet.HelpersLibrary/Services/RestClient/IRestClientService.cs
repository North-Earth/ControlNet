using System.Threading.Tasks;

namespace ControlNet.HelpersLibrary.Services.RestClient
{
    public interface IRestClientService
    {

        #region Methods

        /// <summary>
        /// Sends GET-request to Read object.
        /// </summary>
        /// <returns></returns>
        Task SendGetRequestAsync();

        /// <summary>
        /// Sends POST-request to Create object.
        /// </summary>
        /// <returns></returns>
        Task SendPostRequestAsync();

        /// <summary>
        /// Sends POST-request to Update/Replace object.
        /// </summary>
        /// <returns></returns>
        Task SendPutRequestAsync();


        /// <summary>
        /// Sends DELETE-request to Delete object.
        /// </summary>
        /// <returns></returns>
        Task SendDeleteRequestAsync();

        #endregion
    }
}