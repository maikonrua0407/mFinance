using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DataModel.EntityFramework;
using CommunicationMessages.Base.MessageBases;
using CommunicationServices.ZAMainApp.Messages;
using CommunicationContracts.Base.ContractBases;

namespace CommunicationServices.ZAMainApp
{
    [ServiceContract]
    public interface IZAMainAppService : IServiceBase
    {
        [OperationContract]
        string returnHello();

        [OperationContract]
        SessionResponse getSession(SessionRequest request);

        [OperationContract]
        LoginResponse doLogin(LoginRequest request);

        [OperationContract]
        LoginResponse doLoginWithSession(LoginRequest request);

        [OperationContract]
        LogoutResponse doLogout(LogoutRequest request);

        [OperationContract]
        SessionResponse doCheckSession(SessionRequest request);
    }
}
