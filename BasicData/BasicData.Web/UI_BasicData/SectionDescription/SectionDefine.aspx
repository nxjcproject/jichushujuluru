<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SectionDefine.aspx.cs" Inherits="BasicData.Web.UI_BasicData.SectionDescription.SectionDefine" %>

<%@ Register Src="~/UI_WebUserControls/OrganizationSelector/OrganisationTree.ascx" TagPrefix="uc1" TagName="OrganisationTree" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <script type="text/javascript" src="/lib/ealib/extend/jquery.PrintArea.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/extend/jquery.jqprint.js" charset="utf-8"></script>

    <script type="text/javascript" src="/js/common/PrintFile.js" charset="utf-8"></script>

    <script type="text/javascript" src="../js/page/SectionDescription/SectionDefine.js" charset="utf-8"></script>
    <style type="text/css">
        .easyui-dialog td {
            padding: 5px;
            text-align: right;
        }
    </style>
</head>
<body>
    <div class="easyui-layout" data-options="fit:true,border:false" style="padding: 5px;">
        <div data-options="region:'west',border:false,collapsible:false" style="width: 240px;">
            <uc1:OrganisationTree runat="server" ID="OrganisationTree_ProductionLine" />
        </div>
         <div data-options="region:'center',border:false">
            <table id="SectionDefine_Info" class="easyui-datagrid" data-options="fit:true,border:false,toolbar:'#SectionDefine_ToolBar',onDblClickRow:EditSectionDefine,onRowContextMenu:onRowContextMenu"></table>
        </div>
    </div>
    <div id="SectionDefine_ToolBar">
        <div style="padding: 10px" id="PanelToolBar">
            <span>机构名称：</span><input id="productLineName" class="easyui-textbox" style="width: 180px;" readonly="true" />
            <a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true"
                onclick="QueryReportFun();">查询</a>
            <input id="organizationId" readonly="true" style="display: none;" />
        </div>
        <a href="#" class="easyui-linkbutton" id="edit" data-options="iconCls:'icon-edit',plain:true" onclick="javascript:EditSectionDefine()">编辑</a>
        <a href="#" class="easyui-linkbutton" id="add" data-options="iconCls:'icon-add',plain:true" onclick="javascript:AddSectionDefine()">增加岗位</a>
        <a href="#" class="easyui-linkbutton" id="delete" data-options="iconCls:'icon-remove',plain:true" onclick="javascript:RemoveSectionDefine()">删除岗位</a>
    </div>
 
    <%--添加设备对话框开始--%>
    <div id="AddDialog" class="easyui-dialog" title="添加岗位" style="width: 740px; height: 260px; background-color: rgb(248, 246, 246)"
        data-options="iconCls:'icon-save',resizable:false,modal:true,closed: true,buttons:'#buttons_Add'">
        <table style="padding: 10px;">
            <tr>
                <td>岗位名称:</td>
                <td>
                    <input id="WorkingSectionName" class="easyui-textbox" style="width: 120px;" data-options="required:true" />
                </td>
                <td>组织机构:</td>
                <td>
                    <select id="OrganizationID" class="easyui-combobox" style="width: 150px" data-options="panelHeight:'auto',required:true"></select></td>
                <td>电量类别:</td>
                <td>
                    <input id="VariableName" class="easyui-combotree" style="width: 150px;" data-options="panelHeight:'auto',valueField:'id',textField:'text',required:true"/></td>
            </tr>
            <tr>
                <td>产量类别:</td>
                <td>
                    <select id="Out_put" class="easyui-combobox" style="width: 120px;" data-option="panelHeight:'auto',required:true"></select></td>
                <td id="PulverizedCoalInputLabel"style="display: none;">煤粉消耗:</td>
                <td id="PulverizedCoalInputSelect"style="display: none;">
                    <select id="PulverizedCoalInput" class="easyui-combobox" style="width: 150px;" data-option="required:true"></select></td>
                <td>创建人:</td>
                <td>
                    <input id="Creator" class="easyui-textbox" style="width: 150px;" data-option="required:true" /></td>
            </tr>
            <tr>
                <td rowspan="2">备注：</td>
                <td colspan="5" rowspan="2" style="text-align: left">
                    <textarea id="Remarks" style="width: 500px"></textarea>
                </td>
            </tr>
        </table>
    </div>
    <%--添加设备对话框结束--%>
     <div id="buttons_Add">
        <table style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="javascript:SaveSectionDefine();">保存</a>
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="javascript:$('#AddDialog').dialog('close');">取消</a>
                </td>
            </tr>
        </table>
    </div>

   <%-- 编辑设备对话框开始--%>
    <div id="EditDialog" class="easyui-dialog" title="编辑设备信息" style="width: 770px; height: 330px; background-color: rgb(248, 246, 246)"
        data-options="iconCls:'icon-edit',resizable:false,modal:true,closed: true,buttons:'#buttons_Edit'">
        <%--<fieldset>--%>
       <%-- <legend>主机设备</legend>--%>
        <table style="padding: 10px;">
            <tr>
                <td>岗位名称:</td>
                <td>
                    <input id="WorkingSectionName_edit" class="easyui-textbox" style="width: 120px;" data-options="required:true" />
                </td>
                <td>组织机构:</td>
                <td>
                    <select id="OrganizationID_edit" class="easyui-combobox" style="width: 150px" data-options="panelHeight:'auto',required:true"></select></td>
                <td>电量类别:</td>
                <td>
                    <input id="VariableName_edit" class="easyui-combotree" style="width: 150px;" data-options="panelHeight:'auto',valueField:'id',textField:'text',required:true"/></td>
            </tr>
            <tr>
                <td>产量类别:</td>
                <td>
                    <select id="Out_put_edit" class="easyui-combobox" style="width: 120px;" data-option="panelHeight:'auto',required:true"></select></td>
                <td id="PulverizedCoalInputLabel_edit"style="display: none;">煤粉消耗:</td>
                <td id="PulverizedCoalInputSelect_edit"style="display: none;">
                    <select id="PulverizedCoalInput_edit" class="easyui-combobox" style="width: 150px;" data-option="required:true"></select></td>
                <td>创建人:</td>
                <td>
                    <input id="Creator_edit" class="easyui-textbox" style="width: 150px;" data-option="required:true" /></td>
            </tr>
            <tr>
                <td rowspan="2">备注：</td>
                <td colspan="5" rowspan="2" style="text-align: left">
                    <textarea id="Remarks_edit" style="width: 500px"></textarea>
                </td>
            </tr>
        </table>
        <%--</fieldset>--%>
    </div>
    <%--编辑设备对话框结束--%>
    <div id="buttons_Edit">
        <table cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <a href="#" id="saveEditDlg" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="javascript:SaveEditSectionDefine();">保存修改</a>
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="javascript:$('#EditDialog').dialog('close');">取消</a>
                </td>
            </tr>
        </table>
    </div>


        <!-- 右键菜单开始 -->
    <div id="RightMenu" class="easyui-menu" style="width: 120px;">
       <div id="mm_edit" onclick="EditSectionDefine()" data-options="iconCls:'icon-edit'">编辑</div>
        <div class="menu-sep"></div>
        <div id="mm_delete" onclick="RemoveSectionDefine()" data-options="iconCls:'icon-remove'">删除</div>
    </div>
    <!-- 右键菜单结束 -->
     <form id="form1" runat="server"> </form>
</body>
</html>
