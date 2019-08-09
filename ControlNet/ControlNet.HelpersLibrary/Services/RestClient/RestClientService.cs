using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ControlNet.HelpersLibrary.Services.RestClient
{
    public class RestClientService : IRestClientService
    {
        #region Fields

        #endregion

        #region Constructors

        public RestClientService() { }

        #endregion

        #region Methods

        /// <summary>
        /// Sends GET-request to Read object.
        /// </summary>
        /// <returns></returns>
        public async Task SendDeleteRequestAsync()
        {
            //TODO: Implementation.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends POST-request to Create object.
        /// </summary>
        /// <returns></returns>
        public async Task SendGetRequestAsync()
        {
            //TODO: Implementation.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends POST-request to Update/Replace object.
        /// </summary>
        /// <returns></returns>
        public async Task SendPostRequestAsync()
        {
            //TODO: Implementation.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends DELETE-request to Delete object.
        /// </summary>
        /// <returns></returns>
        public async Task SendPutRequestAsync()
        {
            //TODO: Implementation.
            throw new NotImplementedException();
        }

        #endregion
    }
}
