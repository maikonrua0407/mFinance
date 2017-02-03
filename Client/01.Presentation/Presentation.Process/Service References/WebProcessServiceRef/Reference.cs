﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Presentation.Process.WebProcessServiceRef {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RequestBase", Namespace="http://schemas.datacontract.org/2004/07/CommunicationMessages.Base.MessageBases")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Presentation.Process.WebProcessServiceRef.WebRequest))]
    public partial class RequestBase : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Utilities.Common.DatabaseConstant.Action ActionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClientIPField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClientMACField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Utilities.Common.ApplicationConstant.ClientType ClientTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Utilities.Common.ApplicationConstant.DonViSuDung CompanyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DeptCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Utilities.Common.DatabaseConstant.Function FunctionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MngDeptCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Utilities.Common.DatabaseConstant.Module ModuleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NgayLamViecField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OprDeptCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RequestDateTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RequestIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RequestSecurityKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SessionIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int UserIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Utilities.Common.BusinessConstant.LoaiNguoiSuDung UserTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int intMessageRequestField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long lngMessageRequestField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string strMessageRequestField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string xmlMessageRequestField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Utilities.Common.DatabaseConstant.Action Action {
            get {
                return this.ActionField;
            }
            set {
                if ((this.ActionField.Equals(value) != true)) {
                    this.ActionField = value;
                    this.RaisePropertyChanged("Action");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientIP {
            get {
                return this.ClientIPField;
            }
            set {
                if ((object.ReferenceEquals(this.ClientIPField, value) != true)) {
                    this.ClientIPField = value;
                    this.RaisePropertyChanged("ClientIP");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientMAC {
            get {
                return this.ClientMACField;
            }
            set {
                if ((object.ReferenceEquals(this.ClientMACField, value) != true)) {
                    this.ClientMACField = value;
                    this.RaisePropertyChanged("ClientMAC");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Utilities.Common.ApplicationConstant.ClientType ClientType {
            get {
                return this.ClientTypeField;
            }
            set {
                if ((this.ClientTypeField.Equals(value) != true)) {
                    this.ClientTypeField = value;
                    this.RaisePropertyChanged("ClientType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Utilities.Common.ApplicationConstant.DonViSuDung Company {
            get {
                return this.CompanyField;
            }
            set {
                if ((this.CompanyField.Equals(value) != true)) {
                    this.CompanyField = value;
                    this.RaisePropertyChanged("Company");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DeptCode {
            get {
                return this.DeptCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.DeptCodeField, value) != true)) {
                    this.DeptCodeField = value;
                    this.RaisePropertyChanged("DeptCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Utilities.Common.DatabaseConstant.Function Function {
            get {
                return this.FunctionField;
            }
            set {
                if ((this.FunctionField.Equals(value) != true)) {
                    this.FunctionField = value;
                    this.RaisePropertyChanged("Function");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MngDeptCode {
            get {
                return this.MngDeptCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.MngDeptCodeField, value) != true)) {
                    this.MngDeptCodeField = value;
                    this.RaisePropertyChanged("MngDeptCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Utilities.Common.DatabaseConstant.Module Module {
            get {
                return this.ModuleField;
            }
            set {
                if ((this.ModuleField.Equals(value) != true)) {
                    this.ModuleField = value;
                    this.RaisePropertyChanged("Module");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NgayLamViec {
            get {
                return this.NgayLamViecField;
            }
            set {
                if ((object.ReferenceEquals(this.NgayLamViecField, value) != true)) {
                    this.NgayLamViecField = value;
                    this.RaisePropertyChanged("NgayLamViec");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OprDeptCode {
            get {
                return this.OprDeptCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.OprDeptCodeField, value) != true)) {
                    this.OprDeptCodeField = value;
                    this.RaisePropertyChanged("OprDeptCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RequestDateTime {
            get {
                return this.RequestDateTimeField;
            }
            set {
                if ((object.ReferenceEquals(this.RequestDateTimeField, value) != true)) {
                    this.RequestDateTimeField = value;
                    this.RaisePropertyChanged("RequestDateTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RequestId {
            get {
                return this.RequestIdField;
            }
            set {
                if ((object.ReferenceEquals(this.RequestIdField, value) != true)) {
                    this.RequestIdField = value;
                    this.RaisePropertyChanged("RequestId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RequestSecurityKey {
            get {
                return this.RequestSecurityKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.RequestSecurityKeyField, value) != true)) {
                    this.RequestSecurityKeyField = value;
                    this.RaisePropertyChanged("RequestSecurityKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SessionId {
            get {
                return this.SessionIdField;
            }
            set {
                if ((object.ReferenceEquals(this.SessionIdField, value) != true)) {
                    this.SessionIdField = value;
                    this.RaisePropertyChanged("SessionId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int UserId {
            get {
                return this.UserIdField;
            }
            set {
                if ((this.UserIdField.Equals(value) != true)) {
                    this.UserIdField = value;
                    this.RaisePropertyChanged("UserId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserName {
            get {
                return this.UserNameField;
            }
            set {
                if ((object.ReferenceEquals(this.UserNameField, value) != true)) {
                    this.UserNameField = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Utilities.Common.BusinessConstant.LoaiNguoiSuDung UserType {
            get {
                return this.UserTypeField;
            }
            set {
                if ((this.UserTypeField.Equals(value) != true)) {
                    this.UserTypeField = value;
                    this.RaisePropertyChanged("UserType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int intMessageRequest {
            get {
                return this.intMessageRequestField;
            }
            set {
                if ((this.intMessageRequestField.Equals(value) != true)) {
                    this.intMessageRequestField = value;
                    this.RaisePropertyChanged("intMessageRequest");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long lngMessageRequest {
            get {
                return this.lngMessageRequestField;
            }
            set {
                if ((this.lngMessageRequestField.Equals(value) != true)) {
                    this.lngMessageRequestField = value;
                    this.RaisePropertyChanged("lngMessageRequest");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string strMessageRequest {
            get {
                return this.strMessageRequestField;
            }
            set {
                if ((object.ReferenceEquals(this.strMessageRequestField, value) != true)) {
                    this.strMessageRequestField = value;
                    this.RaisePropertyChanged("strMessageRequest");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string xmlMessageRequest {
            get {
                return this.xmlMessageRequestField;
            }
            set {
                if ((object.ReferenceEquals(this.xmlMessageRequestField, value) != true)) {
                    this.xmlMessageRequestField = value;
                    this.RaisePropertyChanged("xmlMessageRequest");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WebRequest", Namespace="http://schemas.datacontract.org/2004/07/CommunicationServices.WebProcess.Messages" +
        "")]
    [System.SerializableAttribute()]
    public partial class WebRequest : Presentation.Process.WebProcessServiceRef.RequestBase {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResponseBase", Namespace="http://schemas.datacontract.org/2004/07/CommunicationMessages.Base.MessageBases")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Presentation.Process.WebProcessServiceRef.WebResponse))]
    public partial class ResponseBase : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClientIPField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClientMACField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Utilities.Common.ApplicationConstant.ClientType ClientTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Utilities.Common.ApplicationConstant.DonViSuDung CompanyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DeptCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] ExceptionObjectField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ResponseDateTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Presentation.Process.WebProcessServiceRef.ResponseDetail[] ResponseDetailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ResponseIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ResponseMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] ResponseMessageDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ResponseSecurityKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Utilities.Common.ApplicationConstant.ResponseStatus ResponseStatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int intMessageResponseField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long lngMessageResponseField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string strMessageResponseField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string xmlMessageResponseField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientIP {
            get {
                return this.ClientIPField;
            }
            set {
                if ((object.ReferenceEquals(this.ClientIPField, value) != true)) {
                    this.ClientIPField = value;
                    this.RaisePropertyChanged("ClientIP");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientMAC {
            get {
                return this.ClientMACField;
            }
            set {
                if ((object.ReferenceEquals(this.ClientMACField, value) != true)) {
                    this.ClientMACField = value;
                    this.RaisePropertyChanged("ClientMAC");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Utilities.Common.ApplicationConstant.ClientType ClientType {
            get {
                return this.ClientTypeField;
            }
            set {
                if ((this.ClientTypeField.Equals(value) != true)) {
                    this.ClientTypeField = value;
                    this.RaisePropertyChanged("ClientType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Utilities.Common.ApplicationConstant.DonViSuDung Company {
            get {
                return this.CompanyField;
            }
            set {
                if ((this.CompanyField.Equals(value) != true)) {
                    this.CompanyField = value;
                    this.RaisePropertyChanged("Company");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DeptCode {
            get {
                return this.DeptCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.DeptCodeField, value) != true)) {
                    this.DeptCodeField = value;
                    this.RaisePropertyChanged("DeptCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] ExceptionObject {
            get {
                return this.ExceptionObjectField;
            }
            set {
                if ((object.ReferenceEquals(this.ExceptionObjectField, value) != true)) {
                    this.ExceptionObjectField = value;
                    this.RaisePropertyChanged("ExceptionObject");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ResponseDateTime {
            get {
                return this.ResponseDateTimeField;
            }
            set {
                if ((object.ReferenceEquals(this.ResponseDateTimeField, value) != true)) {
                    this.ResponseDateTimeField = value;
                    this.RaisePropertyChanged("ResponseDateTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Presentation.Process.WebProcessServiceRef.ResponseDetail[] ResponseDetail {
            get {
                return this.ResponseDetailField;
            }
            set {
                if ((object.ReferenceEquals(this.ResponseDetailField, value) != true)) {
                    this.ResponseDetailField = value;
                    this.RaisePropertyChanged("ResponseDetail");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ResponseId {
            get {
                return this.ResponseIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ResponseIdField, value) != true)) {
                    this.ResponseIdField = value;
                    this.RaisePropertyChanged("ResponseId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ResponseMessage {
            get {
                return this.ResponseMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.ResponseMessageField, value) != true)) {
                    this.ResponseMessageField = value;
                    this.RaisePropertyChanged("ResponseMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] ResponseMessageData {
            get {
                return this.ResponseMessageDataField;
            }
            set {
                if ((object.ReferenceEquals(this.ResponseMessageDataField, value) != true)) {
                    this.ResponseMessageDataField = value;
                    this.RaisePropertyChanged("ResponseMessageData");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ResponseSecurityKey {
            get {
                return this.ResponseSecurityKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.ResponseSecurityKeyField, value) != true)) {
                    this.ResponseSecurityKeyField = value;
                    this.RaisePropertyChanged("ResponseSecurityKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Utilities.Common.ApplicationConstant.ResponseStatus ResponseStatus {
            get {
                return this.ResponseStatusField;
            }
            set {
                if ((this.ResponseStatusField.Equals(value) != true)) {
                    this.ResponseStatusField = value;
                    this.RaisePropertyChanged("ResponseStatus");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserName {
            get {
                return this.UserNameField;
            }
            set {
                if ((object.ReferenceEquals(this.UserNameField, value) != true)) {
                    this.UserNameField = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int intMessageResponse {
            get {
                return this.intMessageResponseField;
            }
            set {
                if ((this.intMessageResponseField.Equals(value) != true)) {
                    this.intMessageResponseField = value;
                    this.RaisePropertyChanged("intMessageResponse");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long lngMessageResponse {
            get {
                return this.lngMessageResponseField;
            }
            set {
                if ((this.lngMessageResponseField.Equals(value) != true)) {
                    this.lngMessageResponseField = value;
                    this.RaisePropertyChanged("lngMessageResponse");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string strMessageResponse {
            get {
                return this.strMessageResponseField;
            }
            set {
                if ((object.ReferenceEquals(this.strMessageResponseField, value) != true)) {
                    this.strMessageResponseField = value;
                    this.RaisePropertyChanged("strMessageResponse");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string xmlMessageResponse {
            get {
                return this.xmlMessageResponseField;
            }
            set {
                if ((object.ReferenceEquals(this.xmlMessageResponseField, value) != true)) {
                    this.xmlMessageResponseField = value;
                    this.RaisePropertyChanged("xmlMessageResponse");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WebResponse", Namespace="http://schemas.datacontract.org/2004/07/CommunicationServices.WebProcess.Messages" +
        "")]
    [System.SerializableAttribute()]
    public partial class WebResponse : Presentation.Process.WebProcessServiceRef.ResponseBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SessionIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SessionId {
            get {
                return this.SessionIdField;
            }
            set {
                if ((object.ReferenceEquals(this.SessionIdField, value) != true)) {
                    this.SessionIdField = value;
                    this.RaisePropertyChanged("SessionId");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResponseDetail", Namespace="http://schemas.datacontract.org/2004/07/CommunicationMessages.Base.MessageBases")]
    [System.SerializableAttribute()]
    public partial class ResponseDetail : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DetailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ObjectField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OperationField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ResultField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Detail {
            get {
                return this.DetailField;
            }
            set {
                if ((object.ReferenceEquals(this.DetailField, value) != true)) {
                    this.DetailField = value;
                    this.RaisePropertyChanged("Detail");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Object {
            get {
                return this.ObjectField;
            }
            set {
                if ((object.ReferenceEquals(this.ObjectField, value) != true)) {
                    this.ObjectField = value;
                    this.RaisePropertyChanged("Object");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Operation {
            get {
                return this.OperationField;
            }
            set {
                if ((object.ReferenceEquals(this.OperationField, value) != true)) {
                    this.OperationField = value;
                    this.RaisePropertyChanged("Operation");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Result {
            get {
                return this.ResultField;
            }
            set {
                if ((object.ReferenceEquals(this.ResultField, value) != true)) {
                    this.ResultField = value;
                    this.RaisePropertyChanged("Result");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WebProcessServiceRef.IWebProcessService")]
    public interface IWebProcessService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebProcessService/returnHello", ReplyAction="http://tempuri.org/IWebProcessService/returnHelloResponse")]
        string returnHello();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebProcessService/MessageProcess", ReplyAction="http://tempuri.org/IWebProcessService/MessageProcessResponse")]
        long MessageProcess(ref string pv_strMessage);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebProcessService/RequestProcess", ReplyAction="http://tempuri.org/IWebProcessService/RequestProcessResponse")]
        Presentation.Process.WebProcessServiceRef.WebResponse RequestProcess(Presentation.Process.WebProcessServiceRef.WebRequest request);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWebProcessServiceChannel : Presentation.Process.WebProcessServiceRef.IWebProcessService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebProcessServiceClient : System.ServiceModel.ClientBase<Presentation.Process.WebProcessServiceRef.IWebProcessService>, Presentation.Process.WebProcessServiceRef.IWebProcessService {
        
        public WebProcessServiceClient() {
        }
        
        public WebProcessServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebProcessServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebProcessServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebProcessServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string returnHello() {
            return base.Channel.returnHello();
        }
        
        public long MessageProcess(ref string pv_strMessage) {
            return base.Channel.MessageProcess(ref pv_strMessage);
        }
        
        public Presentation.Process.WebProcessServiceRef.WebResponse RequestProcess(Presentation.Process.WebProcessServiceRef.WebRequest request) {
            return base.Channel.RequestProcess(request);
        }
    }
}