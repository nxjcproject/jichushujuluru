$(function () {
    loadVaribleNameData();
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
function loadVaribleNameData() {
    $.ajax({
        type: "POST",
        url: "EnergyDataManualInput.aspx/GetVariableNameData",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var comboboxValue = jQuery.parseJSON(msg.d);
            $('#addVariableName').combobox({
                data: comboboxValue,
                valueField: 'VariableId',
                textField: 'VariableName'
            });
        }
    });
}

function loadGridData() {
    var dataToServer = {
        organizationId: publicData.organizationId
    };
    $.ajax({
        type: "POST",
        url: "EnergyDataManualInput.aspx/GetEnergyDataManualInputData",
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
                field: 'VariableName', title: '变量名称', width: '17%', align: 'center'
            },
            {
                field: 'Name', title: '组织机构名称', width: '12%', align: 'center'
            },
            {
                field: 'TimeStamp', title: '更新日期', width: '10%', align: 'center'
            },
            {
                field: 'DataValue', title: '录入值', width: '16%', align: 'center'
            },
            {
                field: 'UpdateCycle', title: '更新周期', width: '16%', align: 'center'
            },
            {
                field: 'Version', title: '版本', width: '8%', align: 'center'
            },
            {
                field: 'Remark', title: '备注', width: '20%', align: 'center'
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
    addData.organizationId = publicData.organizationId;
    addData.variableId = $('#addVariableName').combobox('getValue');
    addData.updateCycle = $('#addUpdateCycle').combobox('getValue');
    addData.dataValue = $('#addDataValue').numberbox('getValue');
    addData.timeStamp = $('#addTimeStamp').datebox('getValue');
    addData.remark = $('#addRemark').textbox('getText');

    if (addData.variableId != '' && addData.dataValue != '' && addData.timeStamp != '') {
        $.ajax({
            type: "POST",
            url: "EnergyDataManualInput.aspx/AddEnergyDataManualInputData",
            data: "{\"maddData\":'" + JSON.stringify(addData) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d == '1') {
                    alert("添加成功！");
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
//删除数据按钮
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
                deleteData(row["DataItemId"]);
            }
        });
    }
}
function deleteData(id) {
    //parent.$.messager.progress({ text: '数据加载中....' });
    $.ajax({
        type: "POST",
        url: "EnergyDataManualInput.aspx/DeleteEnergyDataManualInputData",
        data: "{id: '" + id + "'}",
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

        publicData.dataItemId = row['DataItemId'];
        $("#editVariableName").textbox("setText", row['VariableName']);
        $("#editUpdateCycle").textbox("setText", row['UpdateCycle']);
        $("#editDataValue").numberbox("setValue", row['DataValue']);
        $("#editTimeStamp").datebox("setValue", row['TimeStamp']);
        $("#editRemark").textbox("setText", row['Remark']);

        $('#editDialog').dialog('open');
    }
}
function saveEditDialog() {
    var editData = {};
    editData.dataItemId = publicData.dataItemId;
    editData.dataValue = $('#editDataValue').numberbox('getValue');
    editData.timeStamp = $('#editTimeStamp').datebox('getValue');
    editData.remark = $('#editRemark').textbox('getText');

    if (editData.dataValue != '' && editData.timeStamp != '') {
        $.ajax({
            type: "POST",
            url: "EnergyDataManualInput.aspx/EditEnergyDataManualInputData",
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