$(function () {
    InitializePage();  
    publicData.organizationId = $.getUrlParam('organizationId');

    loadGridData('first');
});

var publicData = {
    organizationId: "",
    editIndex: "",
    editRow: {}
}


function InitializePage() {
    $('#endUsing').datebox({
        required: "true",
        formatter: $.dateFormatter,
        parser: $.dateParser
    });
    $('#selectedDate').datetimespinner({
        required: "true",
        value: $.dateFormatter(new Date()),
        selections:[[0,4],[5,7],[8,10]],
        formatter: $.dateFormatter,
        parser: $.dateParser
    });
}

function loadGridData(myLoadType) {
    var selectedDate = $('#selectedDate').datetimespinner('getValue');
    $.ajax({
        type: "POST",
        url: "List.aspx/GetPVFList",
        data: "{organizationId: '123', startUsing: '" + selectedDate + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (myLoadType == 'first') {
                myLoadType = 'last';
                m_MsgData = jQuery.parseJSON(msg.d);
                InitializeGrid(m_MsgData);
            }
            else if (myLoadType == 'last') {
                m_MsgData = jQuery.parseJSON(msg.d);
                $('#dg').datagrid('loadData', m_MsgData);
            }
        }
    });
}
function InitializeGrid(myData) {
    $('#dg').datagrid({
        data: myData,
        iconCls: 'icon-edit', singleSelect: true, rownumbers: true, striped: true, toolbar: '#tb',
        columns: [[
            {
                field: 'StartUsing', title: '启用时间', width: '30%', align: 'center',
                formatter: function (value) {
                    return value.substring(0, 10);
                }
            },
            {
                field: 'EndUsing', title: '停用时间', width: '30%', align: 'center',
                formatter: function (value) {
                    return value.substring(0, 10);
                }
            },
            {
                field: 'Flag', title: '启用标志', width: '20%', align: 'center',
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
                field: 'action', title: '操作', width: '14%', align: 'center',
                formatter: function (value, row, index) {
                    var s = '<a href="Detail.aspx?keyid=' + row['KeyID'] + '">详细</a> ';
                    return s;
                }
            }
        ]]
    });
}

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
        var start = row['StartUsing'].substring(0, 10);
        $('#startUsing').val(start);
        if (row['EndUsing'] != "") {
            var end = row['EndUsing'].substring(0, 10);
            $('#endUsing').datebox('setValue', end);
        }
        if (row['Flag'] == 'true') {
            $("#radioTrue").attr("checked", "checked");
        }
        else {
            $("#radioFalse").attr("checked", "checked");
        }
        $('#editDialog').dialog('open');
    }
}
function saveEditDialog() {
    var endUsing = $('#endUsing').datebox('getValue');
    var flag = $("input[name='radiobutton']:checked").val();
    $('#dg').datagrid('updateRow', {
        index: publicData.editIndex,
        row: {
            EndUsing: endUsing,
            Flag: flag
        }
    });
    var id = publicData.editRow["ID"];
    $('#editDialog').dialog('close');
    editData(id, endUsing, flag);
}
function editData(id, endUsing, flag) {
    //parent.$.messager.progress({ text: '数据加载中....' });
    $.ajax({
        type: "POST",
        url: "List.aspx/UpdatePVFList",
        data: "{id:'" + id + "',endUsing: '" + endUsing + "',flag:'" + flag + "'}",
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
}

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
                deleteData(row["KeyID"]);
            }
        });
    }
}
function deleteData(keyId) {
    //parent.$.messager.progress({ text: '数据加载中....' });
    $.ajax({
        type: "POST",
        url: "List.aspx/DeletePVFList",
        data: "{keyId: '" + keyId + "'}",
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