<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="BasicData.Web.UI_BasicData.MachineHaltReasons.Edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>停机原因维护</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css"/>
	<link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css"/>
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css"/>
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css"/>

	<script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/js/common/jquery.utility.js"></script>
	<script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>
    <!--[if lt IE 8 ]><script type="text/javascript" src="/js/common/json2.min.js"></script><![endif]-->
</head>
<body>
    <div id="wrapper" class="easyui-panel" style="width:98%;height:auto;padding:2px;">
	    <div class="easyui-panel" style="width:100%;padding:5px;margin-bottom:5px;">
            <a id="save" href="javascript:void(0)" class="easyui-linkbutton c4 easyui-tooltip tooltip-f" data-options="plain:true,iconCls:'icon-ok'" title="保存。" onclick="saveMachineHaltReasons()">保存</a>
	    </div>

	    <table id="tgMachineHaltReasonsEditor" class="easyui-treegrid" title="停机原因维护" style="width:100%;height:450px;"
			    data-options="
				    iconCls: 'icon-edit',
				    rownumbers: true,
				    animate: true,
				    collapsible: true,
				    fitColumns: true,
				    idField: 'MachineHaltReasonID',
				    treeField: 'ReasonText',
				    onContextMenu: onContextMenu,
                    onClickRow: clickRow,
				    onDblClickRow: dblClickRow
			    ">
		    <thead>
			    <tr>
                    <th data-options="field:'ReasonItemID',width:100">错误标识</th>         <%--, hidden:true--%>
                    <th data-options="field:'MachineHaltReasonID',width:50">错误码</th>
				    <th data-options="field:'ReasonText',width:100,editor:'text'">停机原因</th>
                    <th data-options="field:'Remarks',width:100,editor:'text'">备注</th>
                    <th data-options="field:'Enabled',width:80,editor:'text'">是否可用</th>
			    </tr>
		    </thead>
	    </table>
	    <div class="easyui-panel" style="width:100%;padding:5px;margin-top:5px;">
            <a id="addOneReason" href="javascript:void(0)" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-add'" onclick="appendRoot()">添加一级停机原因</a>
	    </div>

         <!-- 右键菜单开始 -->
	    <div id="mm" class="easyui-menu" style="width:120px;">
		    <div id="add" onclick="append()" data-options="iconCls:'icon-add'">添加</div>
		    <div id="remove" onclick="removeIt()" data-options="iconCls:'icon-remove'">删除</div>
		    <div class="menu-sep"></div>
            <div id="addReason" onclick="appendRoot()" data-options="iconCls:'icon-add'">添加一级停机原因</div>
            <div class="menu-sep"></div>
		    <div onclick="collapse()">收起</div>
		    <div onclick="expand()">展开</div>
	    </div>
        <!-- 右键菜单结束 -->
    </div>

    <script type="text/javascript">
        //////////////////////////////////////////////////////////////////////
        //初始化页面的增删改查权限
        function initPageAuthority() {
            $.ajax({
                type: "POST",
                url: "Edit.aspx/AuthorityControl",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//同步执行
                success: function (msg) {
                    var authArray = msg.d;
                    //增加
                    if (authArray[1] == '0') {
                        var itemEl = $('#add')[0];
                        $("#mm").menu('disableItem', itemEl);
                        itemEl = $('#addReason')[0];
                        $("#mm").menu('disableItem', itemEl);
                        $("#addOneReason").linkbutton('disable');
                    }
                    //修改
                    if (authArray[2] == '0') {                     
                        $("#save").linkbutton('disable');
                    }
                    //删除
                    if (authArray[3] == '0') {
                        var itemEl = $('#remove')[0];
                        $("#mm").menu('disableItem', itemEl);
                    }
                }
            });
        }
        // 右键菜单
        function onContextMenu(e, row) {
            e.preventDefault();
            $(this).treegrid('select', row.MachineHaltReasonID);
            $('#mm').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        }

        // 添加根节点
        function appendRoot() {
            GetReasonRow("RootNode");
        }
 
        //添加子节点
        function append() {
            GetReasonRow("ChildNode");
        }
        //创建设备故障原因ID
        function GetReasonRow(Flag) {
            var queryUrl = 'Edit.aspx/GetReasonItemID';
            $.ajax({
                type: "POST",
                url: queryUrl,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg != null && m_Msg != undefined && m_Msg != "") {
                        if (Flag == "RootNode") {           //添加根节点
                            var levelCode = getAppendRootLevelCode();
                            $('#tgMachineHaltReasonsEditor').treegrid('append', {
                                data: [{
                                    ReasonItemID: m_Msg,
                                    MachineHaltReasonID: levelCode,
                                    ReasonText: '新增错误原因',
                                    Enabled: 'True'
                                }]
                            })
                        }
                        else if (Flag == "ChildNode")
                        {
                            var node = $('#tgMachineHaltReasonsEditor').treegrid('getSelected');
                            if (node.MachineHaltReasonID.length == 7) {
                                $.messager.alert('错误', '最多只能有三级停机原因！');
                                return;
                            }
                            var levelCode = getAppendLevelCode(node.MachineHaltReasonID);
                            $('#tgMachineHaltReasonsEditor').treegrid('append', {
                                parent: node.MachineHaltReasonID,
                                data: [{
                                    ReasonItemID: m_Msg,
                                    MachineHaltReasonID: levelCode,
                                    ReasonText: '新增错误原因',
                                    Enabled: 'True'
                                }]
                            })
                        }
                    }
                    else {
                        $.messager.alert('错误', '无法创建新设备故障原因！');
                    }
                },
                error: function () {
                    $.messager.alert('错误', '无法创建新设备故障原因！');
                }
            });
        }
        // 删除节点
        function removeIt() {
            var node = $('#tgMachineHaltReasonsEditor').treegrid('getSelected');
            if (node) {
                $('#tgMachineHaltReasonsEditor').treegrid('remove', node.MachineHaltReasonID);
            }
        }

        // 收起节点
        function collapse() {
            var node = $('#tgMachineHaltReasonsEditor').treegrid('getSelected');
            if (node) {
                $('#tgMachineHaltReasonsEditor').treegrid('collapse', node.MachineHaltReasonID);
            }
        }

        // 展开节点
        function expand() {
            var node = $('#tgMachineHaltReasonsEditor').treegrid('getSelected');
            if (node) {
                $('#tgMachineHaltReasonsEditor').treegrid('expand', node.MachineHaltReasonID);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////

        // 生成根节点ID
        function getAppendRootLevelCode() {
            var rows = $('#tgMachineHaltReasonsEditor').treegrid('getRoots');
            if (rows.length == 0) {
                return 'E01';
            }
            else {
                var maxCode = 0;
                for (var i = 0; i < rows.length; i++) {
                    var temp = rows[i].MachineHaltReasonID;
                    if (temp.length != 3)
                        continue;
                    var p = parseInt(temp.substring(1, temp.length));
                    if (p > maxCode)
                        maxCode = p;
                }
                maxCode = maxCode + 1;
                if (maxCode.toString().length % 2 == 1)
                    return 'E0' + maxCode;
                else
                    return 'E' + maxCode;
            }
        }

        // 生成子节点ID
        function getAppendLevelCode(parentId) {
            var rows = $('#tgMachineHaltReasonsEditor').treegrid('getChildren', parentId);

            if (rows.length == 0) {
                return parentId + '01';
            }
            else {
                var maxCode = 0;
                for (var i = 0; i < rows.length; i++) {
                    var temp = rows[i].MachineHaltReasonID;
                    if (temp.length != parentId.length + 2)
                        continue;
                    var p = parseInt(temp.substring(1, temp.length));
                    if (p > maxCode)
                        maxCode = p;
                }
                maxCode = maxCode + 1;
                if (maxCode.toString().length % 2 == 1)
                    return 'E0' + maxCode;
                else
                    return 'E' + maxCode;
            }
        }

        ///////////////////////////////////////////////////////////

        // 当前编辑行ID
        var editingId;

        // 编辑
        function edit() {
            if (editingId != undefined) {
                $('#tgMachineHaltReasonsEditor').treegrid('select', editingId);
                return;
            }
            var row = $('#tgMachineHaltReasonsEditor').treegrid('getSelected');
            if (row) {
                editingId = row.MachineHaltReasonID;
                $('#tgMachineHaltReasonsEditor').treegrid('beginEdit', editingId);
            }
        }

        // 保存
        function save() {
            if (editingId != undefined) {
                var t = $('#tgMachineHaltReasonsEditor');
                t.treegrid('endEdit', editingId);
                editingId = undefined;
            }
        }

        // 取消编辑
        function cancel() {
            if (editingId != undefined) {
                $('#tgMachineHaltReasonsEditor').treegrid('cancelEdit', editingId);
                editingId = undefined;
            }
        }

        // tg单击处理
        function clickRow(row) {
            save();
        }

        // tg双击处理
        function dblClickRow(row) {
            // 首先进入编辑模式
            edit();
        }

        /////////////////////////////////////////////////////////////

        // 获取所有停机原因
        function loadMachineHaltReasons() {
            var queryUrl = 'Edit.aspx/GetMachineHaltReasonsWithTreeGridFormat';

            var win = $.messager.progress({
                title: '请稍后',
                msg: '数据载入中...'
            });

            $.ajax({
                type: "POST",
                url: queryUrl,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    initializeMachineHaltReasonsEditor(jQuery.parseJSON(msg.d));
                    $.messager.progress('close');
                },
                error: function () {
                    $.messager.progress('close');
                    $.messager.alert('错误', '数据载入失败！');
                }
            });
        }

        // 初始停机原因编辑器
        function initializeMachineHaltReasonsEditor(jsonData) {
            $('#tgMachineHaltReasonsEditor').treegrid({
                data: jsonData,
                dataType: "json"
            });
        }

        ///////////////////////////////////////////////////////////////////////


        // 保存停机原因
        function saveMachineHaltReasons() {
            var queryUrl = 'Edit.aspx/SaveMachineHaltReasons';
            save();
            var dataToSend = '{"json":\'' + JSON.stringify($('#tgMachineHaltReasonsEditor').treegrid('getData')) + '\'}';
            //var m_OrganizationId = "";
            //var dataToSend = '{organizationId: "' + m_OrganizationId + '"}';

            $.ajax({
                type: "POST",
                url: queryUrl,
                data: dataToSend,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $.messager.alert('信息', '停机原因保存成功！', 'info');
                }
            });
        }


        $(document).ready(function () {
            loadMachineHaltReasons();
            initPageAuthority();
        });

    </script>
    <form id="form1" runat="server"></form>
</body>
</html>
