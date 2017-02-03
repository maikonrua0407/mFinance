<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLichTraNoCT.ascx.cs" Inherits="Presentation.WebClient.Modules.TDVM.LichTraNo.ucLichTraNoCT" %>
<div class="row">
    <div class="col-md-12">
        <div class="panel panel-blue">
            <div class="panel-heading">
                @ViewBag.Title
            </div>           
            <div class="panel-body grid-table">
                <div class="row mbm">
                    <div class="col-md-4">                       
                            <a class="btn btn-info" href="@Url.Action("Add", controller)">
                                <span class="glyphicon glyphicon-plus"></span>
                                @BackendMessage.AddNew
                            </a>
                    
                            <button type="button" class="btn btn-danger" onclick="customer.deleteCustomers()">
                                <span class="glyphicon glyphicon-remove"></span>
                                @BackendMessage.Delete
                            </button>
                        
                    </div>
                    <div class="col-md-8">
                       
                            <button type="button" class="btn btn-warning pull-right ml3" onclick="customer.export()">
                                <span class="fa fa-cloud-download"></span>
                                @BackendMessage.ExportExcel
                            </button>
                     
                            <button type="button" class="btn btn-pink pull-right ml3" onclick="customer.importClick()">
                                <span class="fa fa-cloud-upload"></span>
                                @BackendMessage.ImportExcel
                            </button>
                      
                        <div class="btn-group pull-right">
                            <button type="button" class="btn btn-default">@BackendMessage.FileTemplates</button>
                            <button type="button" data-toggle="dropdown" class="btn btn-default dropdown-toggle">
                                <span class="caret"></span>
                                <span class="sr-only">Toggle Dropdown</span>
                            </button>
                            <ul role="menu" class="dropdown-menu">
                                <li>
                                    <a href="@Url.Content("~/Content/FileTemplate/Customers.csv")">@BackendMessage.CsvFileTemplate</a>

                                </li>
                                <li>
                                    <a href="@Url.Content("~/Content/FileTemplate/Customers.xls")">@BackendMessage.ExcelFileTemplate</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div id="divList">
                    @Html.Partial("BaseView/Customer/_list")
                </div>
            </div>
        </div>
    </div>
</div>