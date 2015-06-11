﻿$(function () {
    //InitializePage();
    loadGridData();
});

var publicData = {
    organizationId: "",
    editIndex: "",
    editRow: {}
}

function onOrganisationTreeClick(node) {
    publicData.organizationId = node.OrganizationId;
    $('#organizationName').textbox('setText', node.text);
}

function loadGridData() {
    var dataToServer = {
        variableName:$('#variableName').val()
    };
    $.ajax({
        type: "POST",
        url: "EnergyDataManualInputContrast.aspx/GetEnergyDataManualInputContrastData",
        data: JSON.stringify(dataToServer),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            m_MsgData = jQuery.parseJSON(msg.d);
            InitializeGrid(m_MsgData);
        }
    });
}
function InitializeGrid(myData) {
    $('#dg').datagrid({
        data: myData,
        iconCls: 'icon-edit', singleSelect: true, rownumbers: true, striped: true, toolbar: '#tb',
        columns: [[
            {
                field: 'VariableName', title: '变量名称', width: '20%', align: 'center'
            },
            {
                field: 'Type', title: '变量类别', width: '12%', align: 'center', hidden: true
            },
            {
                field: 'Enabled', title: '启用标志', width: '10%', align: 'center',
                formatter: function (value) {
                    if (value == true || value == "True")
                        return "启用";
                    else if (value == false || value == "False")
                        return "禁用";
                    else
                        return "";
                }
            },
            {
                field: 'Creator', title: '创建人', width: '18%', align: 'center'
            },
            {
                field: 'CreateTime', title: '创建时间', width: '16%', align: 'center'
            },
            {
                field: 'Remark', title: '备注', width: '35%', align: 'center'
            }
        ]]
    });
}
//添加按钮
function addItem() {
    $('#addDialog').dialog('open');
}
//添加对话框保存按钮
function saveAddDialog() {
    var addData = {};
    addData.variableId = $('#addVariableId').textbox('getText');
    addData.variableName = $('#addVariableName').textbox('getText');
    //addData.type = $('#addType').combobox('getValue');
    addData.enabled = $("input[name='radiobutton']:checked").val();
    addData.creator = $('#addCreator').textbox('getText');
    addData.createTime = $('#addCreateTime').datetimebox('getValue');
    addData.remark = $('#addRemark').textbox('getText');

    if (addData.variableId != '' && addData.variableName != '' && addData.creator != '' && addData.createTime != '') {
        $.ajax({
            type: "POST",
            url: "EnergyDataManualInputContrast.aspx/AddEnergyDataManualInputContrastData",
            data: "{\"maddData\":'" + JSON.stringify(addData) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d == '1') {
                    alert("添加成功！");
                }
                else if (msg.d == '-2') {
                    alert("ID值重复，添加失败！");
                }
                else {
                    alert("添加失败！");
                }
            }
        });

        $('#addDialog').dialog('close');
        loadGridData();
    }
    else {
        alert('请输入必填项！');
    }
}
//删除数据
function deleteItem() {
    var row = $("#dg").datagrid('getSelected');
    if (row == null) {
        alert('请选中一行数据！');
    }
    else {
        var index = $("#dg").datagrid('getRowIndex', row);
        $.messager.defaults = { ok: "是", cancel: "否" };
        $.messager.confirm('提示', '确定要删除选中行？', function (r) {
            if (r) {
                $('#dg').datagrid('deleteRow', index);
                deleteData(row['VariableId']);
            }
        });
    }
}
//删除按钮
function deleteData(id) {
    //parent.$.messager.progress({ text: '数据加载中....' });
    $.ajax({
        type: "POST",
        url: "EnergyDataManualInputContrast.aspx/DeleteEnergyDataManualInputContrastData",
        data: "{variableId: '" + id + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == '1') {
                alert("删除成功！");
            }
            else {
                alert("删除失败！");
            }
        }
    });
}
//编辑按钮
function editItem() {
    var row = $("#dg").datagrid('getSelected');
    publicData.editRow = row;
    if (row == null) {
        alert('请选中一行数据！');
    }
    else {
        var index = $("#dg").datagrid('getRowIndex', row);
        publicData.editIndex = index;
        $('#dg').datagrid('selectRow', index);
        var row = $('#dg').datagrid('getSelected');

        publicData.variableId = row['VariableId'];
        $("#editVariableName").textbox("setText", row['VariableName']);
        $("#editCreator").textbox("setText", row['Creator']);
        $("#editCreateTime").datetimebox('setValue', row['CreateTime']);
        $("#editRemark").textbox("setText", row['Remark']);
        if (row['Enabled'] == 'True') {
            $("#editradioTrue").attr("checked", "checked");
        }
        else {
            $("#editradioFalse").attr("checked", "checked");
        }
        $('#editDialog').dialog('open');
    }
}
function saveEditDialog() {
    var editData = {};
    editData.variableId = publicData.variableId;
    editData.variableName = $('#editVariableName').textbox('getText');
    editData.enabled = $("input[name='editradiobutton']:checked").val();
    editData.creator = $('#editCreator').textbox('getText');
    editData.createTime = $('#editCreateTime').datetimebox('getValue');
    editData.remark = $('#editRemark').textbox('getText');

    if (editData.variableName != '' && editData.creator != '' && editData.createTime != '') {
        $.ajax({
            type: "POST",
            url: "EnergyDataManualInputContrast.aspx/EditEnergyDataManualInputContrastData",
            data: "{\"editData\":'" + JSON.stringify(editData) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d == '1') {
                    alert("修改成功！");
                }
                else {
                    alert("修改失败！");
                }
            }
        });

        $('#editDialog').dialog('close');
        loadGridData();
    }
    else {
        alert('请输入必填项！');
    }
}