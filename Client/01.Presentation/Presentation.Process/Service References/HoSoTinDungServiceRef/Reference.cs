﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Presentation.Process.HoSoTinDungServiceRef {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="HoSoTinDungServiceRef.IHoSoTinDungService")]
    public interface IHoSoTinDungService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHoSoTinDungService/returnHello", ReplyAction="http://tempuri.org/IHoSoTinDungService/returnHelloResponse")]
        string returnHello();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IHoSoTinDungServiceChannel : Presentation.Process.HoSoTinDungServiceRef.IHoSoTinDungService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class HoSoTinDungServiceClient : System.ServiceModel.ClientBase<Presentation.Process.HoSoTinDungServiceRef.IHoSoTinDungService>, Presentation.Process.HoSoTinDungServiceRef.IHoSoTinDungService {
        
        public HoSoTinDungServiceClient() {
        }
        
        public HoSoTinDungServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public HoSoTinDungServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HoSoTinDungServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HoSoTinDungServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string returnHello() {
            return base.Channel.returnHello();
        }
    }
}
