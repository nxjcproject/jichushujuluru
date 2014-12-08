<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BasicData.Web.UI_BasicData.PeakValleyFlat.List" %>

<%@ Register Src="~/UI_WebUserControls/OrganizationSelector/OrganisationTree.ascx" TagPrefix="uc1" TagName="OrganisationTree" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/js/common/jquery.utility.js" charset="utf-8"></script>

    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <script type="text/javascript" src="../js/page/PeakValleyFlat/List.js" charset="utf-8"></script>

    <title>峰谷平列表</title>
</head>
<body>
    <div class="easyui-layout" data-options="fit:true,border:false" style="padding: 5px;">
        <div data-options="region:'west',border:false,collapsible:false" style="width: 230px;">
            <uc1:OrganisationTree runat="server" ID="OrganisationTree" />
        </div>
        <div data-options="region:'center',border:false,collapsible:false" style="padding-left: 10px;">
            <table>
                <tr>
                    <td>产线名称:</td>
                    <td><input id="organizationName" class="easyui-textbox" style="width:100px"/></td>
                    <td>启用时间:</td>
                    <td>
                        <input id="selectedDate" style="width: 100px;" />
                    </td>
                    <td><a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true" onclick="loadGridData('first')">查询</a></td>
                </tr>
            </table>
            <table id="dg" class="easyui-datagrid" title="" style="width: 100%; height: auto">
            </table>
        </div>
    </div>

    <div id="tb" style="padding: 5px; height: auto">
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="addItem()">添加</a>
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="deleteItem()">删除</a>
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="editItem()">编辑</a>
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true" onclick="loadGridData('first')">刷新</a>
    </div>

    <div id="editDialog" class="easyui-dialog" title="编辑" data-options="iconCls:'icon-save',closed:true,buttons:'#editDialog-buttons'" style="width: 400px; height: 200px; padding: 10px">
        <table>
            <tr>
                <td>启用时间</td>
                <td>
                    <input id="startUsing" style="width: 100px;" required="required" readonly="true" />
                </td>
            </tr>
            <tr>
                <td>停用时间</td>
                <td>
                    <input id="endUsing" style="width: 100px;" />
                </td>
            </tr>
            <tr>
                <td>启用标志</td>
                <td>
                    <input id="radioTrue" type="radio" name="radiobutton" value="True" />启用
                    <input id="radioFalse" type="radio" name="radiobutton" value="False" />停用
                </td>
            </tr>
        </table>
    </div>
    <div id="editDialog-buttons">
        <a href="javascript:void(0)" class="easyui-linkbutton" onclick="saveEditDialog()">保存</a>
        <a href="javascript:void(0)" class="easyui-linkbutton" onclick="javascript:$('#editDialog').dialog('close')">取消</a>
    </div>
</body>
</html>
