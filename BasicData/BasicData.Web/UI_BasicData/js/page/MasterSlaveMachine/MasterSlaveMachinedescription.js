var MasterMachineOpType;               //主机设备操作类型,1添加,2修改
var SlaveMachineOpType;                //从机设备操作类型,1添加,2修改
var MasterOrganizationId;              //主机组织机构代码,用于接收Web控件传递来的信息
var SlaveOrganizationId;               //从机组织机构代码,用于接收Web控件传递来的信息
var CurrentMachineEditFoucs            //当前编辑的是主机还是从机1表示主机，2表示从机
$(function () {
    LoadDcsOrganizationData('first');
    InitializeMaterMachineGrid({ "rows": [], "total": 0 });
    InitializeSlaveMachineGrid({ "rows": [], "total": 0 });
    LoadMasterMachineDialog();
    LoadSlaveMachineDialog();
    LoadSelectDcsTagsDialog();
});

function LoadMaterMachineData(myLoadType, myDcsId) {
    $.ajax({
        type: "POST",
        url: "MasterSlaveMachinedescription.aspx/GetMasterMachineInfo",
        data: "{myDcsId:'" + myDcsId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (myLoadType == 'first') {
                InitializeMaterMachineGrid(m_MsgData);
            }
            else if (myLoadType == 'last') {
                $('#grid_MasterMachineInfo').datagrid('loadData', m_MsgData);
            }
        }
    });
}
//////////////////////////////////初始化基础数据//////////////////////////////////////////
function InitializeMaterMachineGrid(myData) {
    $('#grid_MasterMachineInfo').datagrid({
        title: '',
        data: myData,
        dataType: "json",
        striped: true,
        //loadMsg: '',   //设置本身的提示消息为空 则就不会提示了的。这个设置很关键的
        rownumbers: true,
        singleSelect: true,
        idField: 'Id',
        fit:true, 
        columns: [[{
            width: 120,
            title: '设备组织机构',
            field: 'OrganizationId',
            hidden: true
        }, {
            width: 120,
            title: 'DCS名称',
            field: 'OrganizationName'
        }, {
            width: '120',
            title: '变量名',
            field: 'VariableName'
        }, {
            width: 150,
            title: '变量描述',
            field: 'VariableDescription'
        }, {
            width: 110,
            title: '记录主机停机信息',
            field: 'Record'
        }, {
            width: 50,
            title: '有效值',
            field: 'ValidValues'
        }, {
            width: 200,
            title: '备注',
            field: 'Remarks'
        }, {
            width: 60,
            title: '从机ID',
            field: 'KeyId',
            hidden: true
        }, {
            width: 70,
            title: '操作',
            field: 'Op',
            formatter: function (value, row, index) {
                var str = '';
                str = '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_edit.png" title="编辑" onclick="EditMasterMachineFun(\'' + row.Id + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_delete.png" title="删除" onclick="DeleteMasterMachineFun(\'' + row.Id + '\',\'' + row.OrganizationId + '\',\'' + row.VariableDescription + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/folder/folder_go.png" title="添加从机" onclick="AddSlaveMachineFun(\'' + row.Id + '\',\'' + row.VariableDescription + '\');"/>';
                return str;
            }
        }]],
        toolbar: '#toolbar_MasterMachineInfo',
        onDblClickRow: function (rowIndex, rowData) {
            $('#HiddenField_MasterMachineId').attr('value', rowData.Id);
            
            LoadSlaveMachineData('last', rowData.Id);                //刷新从机列表
            $('#Text_SelectMasterMachine').attr('value', rowData.VariableDescription);
        }
    });
}

function LoadSlaveMachineData(myLoadType,myKeyId) {
    $.ajax({
        type: "POST",
        url: "MasterSlaveMachinedescription.aspx/GetSlaveMachineInfo",
        data: "{myKeyId:'" + myKeyId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (myLoadType == 'first') {
                InitializeSlaveMachineGrid(m_MsgData);
            }
            else if (myLoadType == 'last') {
                $('#grid_SlaveMachineInfo').datagrid('loadData', m_MsgData);
            }
        }
    });
}
//////////////////////////////////初始化基础数据//////////////////////////////////////////
function InitializeSlaveMachineGrid(myData) {
    $('#grid_SlaveMachineInfo').datagrid({
        title: '',
        data: myData,
        dataType: "json",
        striped: true,
        //loadMsg: '',   //设置本身的提示消息为空 则就不会提示了的。这个设置很关键的
        rownumbers: true,
        singleSelect: true,
        idField: 'Id',
        columns: [[{
            width: 120,
            title: '设备组织机构',
            field: 'OrganizationId',
            hidden: true
        }, {
            width: 120,
            title: 'DCS名称',
            field: 'OrganizationName'
        }, {
            width: 120,
            title: '主机ID',
            field: 'KeyId',
            hidden: true
        }, {
            width: '120',
            title: '主机名',
            field: 'KeyName',
            hidden: true
        }, {
            width: 120,
            title: '变量名',
            field: 'VariableName'
        }, {
            width: 150,
            title: '变量描述',
            field: 'VariableDescription'
        }, {
            width: 60,
            title: '有效值',
            field: 'ValidValues'
        }, {
            width: 70,
            title: '允许时间',
            field: 'TimeDelay'
        }, {
            width: 150,
            title: '备注',
            field: 'Remarks'
        }, {
            width: 60,
            title: '操作',
            field: 'Op',
            formatter: function (value, row, index) {
                var str = '';
                str = '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_edit.png" title="编辑" onclick="EditSlaveMachineFun(\'' + row.Id + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_delete.png" title="删除" onclick="DeleteSlaveMachineFun(\'' + row.Id + '\',\'' + row.KeyId + '\');"/>';
                return str;
            }
        }]],
        toolbar: '#toolbar_SlaveMachineInfo'
    });
}

////////////////////////////////////////////////初始化Combotree///////////////////////////////////////////
function LoadDcsOrganizationData(myLoadType) {
    $.ajax({
        type: "POST",
        url: "MasterSlaveMachinedescription.aspx/GetDcsOrganization",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (myLoadType == 'first') {
                InitializeComboTree(m_MsgData);
            }
            else if (myLoadType == 'last') {
                $('#ComboTree_MasterDcs').combotree('loadData', m_MsgData);
                $('#ComboTree_SlaveDcs').combotree('loadData', m_MsgData);
                $('#Combobox_DCSF').combotree('loadData', m_MsgData);
            }
        }
    });
}
//////////////////////////////////初始化基础数据//////////////////////////////////////////
function InitializeComboTree(myData) {
    $('#Combobox_DCSF').combotree({
        data: myData,
        dataType: "json",
        valueField: 'id',
        textField: 'text',
        required: false,
        panelHeight: 'auto',
        editable: false
    });
}
////////////////////////////////////初始化dialog////////////////////////////
function LoadMasterMachineDialog() {
    //loading 用户dialog
    $('#dlg_AddMasterMachine').dialog({
        title: '主机设备信息',
        width: 650,
        height: 360,
        closed: true,
        cache: false,
        modal: true,
        buttons: "#buttons_AddMasterMachine"
    });
}

function LoadSlaveMachineDialog() {
    //loading 用户dialog
    $('#dlg_AddSlaveMachine').dialog({
        title: '从机设备信息',
        width: 650,
        height: 360,
        closed: true,
        cache: false,
        modal: true,
        buttons: "#buttons_AddSlaveMachine"
    });
}
function LoadSelectDcsTagsDialog() {
    $('#dlg_SelectDcsTags').dialog({
        title: '选择DCS标签',
        width: 750,
        height: 460,
        closed: true,
        cache: false,
        modal: true
    });
}
//////////////获得Tag信息，调用Web控件时必须定义该方法才能有效的传递双击事件
function GetTagInfo(myRowData, DcsOrganizationId) {
    //alert(myRowData.VariableName + "," + myRowData.VariableDescription + "," + myRowData.TableName + "," + myRowData.FieldName);
    if (CurrentMachineEditFoucs == 1) {
        $('#TextBox_MasterVariableName').textbox('setText', myRowData.VariableName);
        $('#TextBox_MasterVariableDescription').textbox('setText', myRowData.VariableDescription);
        MasterOrganizationId = DcsOrganizationId;
    }
    else if (CurrentMachineEditFoucs == 2) {
        $('#TextBox_SlaveVariableName').textbox('setText', myRowData.VariableName);
        $('#TextBox_SlaveVariableDescription').textbox('setText', myRowData.VariableDescription);
        SlaveOrganizationId = DcsOrganizationId;
    }
}
function GetDcsTagsFun(myCurrentMachineEditFoucs) {
    CurrentMachineEditFoucs = myCurrentMachineEditFoucs;
    $('#dlg_SelectDcsTags').dialog('open');
}
/////////////////////////////查询主设备/////////////////////////
function QueryMasterMachineInfoFun() {
    var m_SelectDcs = $('#Combobox_DCSF').combotree("getValue");
    if (m_SelectDcs != "" && m_SelectDcs != null && m_SelectDcs != undefined) {
        if (m_SelectDcs.length == 7) {
            LoadMaterMachineData('last', m_SelectDcs);
        }
        else {
            alert('必须选择DCS节点!');
        }
    }
}
function RefreshMasterMachineFun() {
    QueryMasterMachineInfoFun();
}
//////////////////////////////增加主机/////////////////////////////////
function AddMasterMachineFun() {
    
    $('#HiddenField_MasterMachineId').attr('value', "");
    $('#TextBox_MasterVariableName').textbox('setText', '');
    $('#TextBox_MasterVariableDescription').textbox('setText', '');
    $('#TextBox_MasterRemark').attr('value', '');
    //$('#Combobox_AddDepartmentIndex').combotree('setValue', data[0].OrganizationId);
    $('#Checkbox_MasterRecord').attr('checked', true);
    $('#Radio_MasterValidValueOff').attr('checked', 'checked');
    MasterOrganizationId = "";
    MasterMachineOpType = 0;

    LoadSlaveMachineData('last', "");                       //刷新从机列表
    $('#Text_SelectMasterMachine').attr('value', '');

    $('#dlg_AddMasterMachine').dialog('open');
}
function EditMasterMachineFun(myId) {
    $.ajax({
        type: "POST",
        url: "MasterSlaveMachinedescription.aspx/GetMasterMachineInfobyId",
        data: "{myId:'" + myId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = jQuery.parseJSON(msg.d)['rows'];
            if (data) {
                $('#HiddenField_MasterMachineId').attr('value', data[0].Id);
                $('#TextBox_MasterVariableName').textbox('setText', data[0].VariableName);
                $('#TextBox_MasterVariableDescription').textbox('setText', data[0].VariableDescription);
                $('#TextBox_MasterRemark').attr('value', data[0].Remarks);
                MasterOrganizationId = data[0].OrganizationId;
                if (data[0].Record.toLocaleLowerCase() == 'true') {
                    $('#Checkbox_MasterRecord').attr('checked', true);
                }
                else {
                    $('#Checkbox_MasterRecord').attr('checked', false);
                }
                if (data[0].ValidValues.toLocaleLowerCase() == 'true') {
                    $('#Radio_MasterValidValueOn').attr('checked', true);
                }
                else {
                    $('#Radio_MasterValidValueOff').attr('checked', true);
                }
                LoadSlaveMachineData('last', data[0].Id);                     //刷新从机列表
                $('#Text_SelectMasterMachine').attr('value', data[0].VariableDescription);
            }
        }
    });

    MasterMachineOpType = 1;
    $('#dlg_AddMasterMachine').dialog('open');

}

function SaveMasterMachine() {
    var m_MasterMachineId = $('#HiddenField_MasterMachineId').val();
    var m_OrganizationId = MasterOrganizationId;                              //$('#TextBox_MasterVariableDescription').textbox('getValue');
    var m_VariableName = $('#TextBox_MasterVariableName').textbox('getText');
    var m_VariableDescription = $('#TextBox_MasterVariableDescription').textbox('getText');
    var m_ValidValues = $("input[name='SelectRadio_MasterValidValues']:checked").val();
    var m_Remarks = $('#TextBox_MasterRemark').val();
    var m_Record = $('#Checkbox_MasterRecord').attr('checked');
    if (m_Record == 'checked') {
        m_Record = 1;
    }
    else {
        m_Record = 0;
    }
    var Valid_VariableName = $('#TextBox_MasterVariableName').textbox('isValid');
    if (!Valid_VariableName) {
        alert('请填写变量名!');
    }
    else if (m_OrganizationId == "" || m_OrganizationId == null || m_OrganizationId == undefined) {
        alert('请选择所属DCS!');
    }
    else if (m_OrganizationId.length != 7) {
        alert('请选择到DCS节点!');
    }
    else {
        if (MasterMachineOpType == 0) {              //添加主机设备

            $.ajax({
                type: "POST",
                url: "MasterSlaveMachinedescription.aspx/AddMasterMachineInfo",
                data: "{myOrganizationId:'" + m_OrganizationId + "',myVariableName:'" + m_VariableName + "',myVariableDescription:'" + m_VariableDescription
                        + "',myRecord:'" + m_Record + "',myValidValues:'" + m_ValidValues + "',myRemarks:'" + m_Remarks + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == '1') {
                        LoadMaterMachineData('last', m_OrganizationId);         //刷新主机列表
                        $('#dlg_AddMasterMachine').dialog('close');
                        alert('添加成功!');
                    }
                    else if (m_Msg == '0') {
                        alert('添加失败!');
                    }
                    else if (m_Msg == '-1') {
                        alert('数据库错误!');
                    }
                    else {
                        alert(m_Msg);
                    }
                }
            });
        }
        else if (MasterMachineOpType == 1) {                                     //修改主机设备
            $.ajax({
                type: "POST",
                url: "MasterSlaveMachinedescription.aspx/ModifyMasterMachineInfo",
                data: "{myId:'" + m_MasterMachineId + "',myOrganizationId:'" + m_OrganizationId + "',myVariableName:'" + m_VariableName + "',myVariableDescription:'" + m_VariableDescription
                        + "',myRecord:'" + m_Record + "',myValidValues:'" + m_ValidValues + "',myRemarks:'" + m_Remarks + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == '1') {
                        LoadMaterMachineData('last', m_OrganizationId);                      //刷新列表
                        $('#dlg_AddMasterMachine').dialog('close');
                        alert('修改成功!');
                    }
                    else if (m_Msg == '0') {
                        alert('修改失败!');
                    }
                    else if (m_Msg == '-1') {
                        alert('数据库错误!');
                    }
                    else {
                        alert(m_Msg);
                    }
                }
            });
        }
    }
}

////////////////////////////////删除主机///////////////////////////////
function DeleteMasterMachineFun(myId, myOrganizationId, myVariableDescription) {
    $('#HiddenField_MasterMachineId').attr('value', myId);
    LoadSlaveMachineData('last', myId);                     //刷新从机列表
    $('#Text_SelectMasterMachine').attr('value', myVariableDescription);
    parent.$.messager.confirm('询问', '您确定要删除该主机以及属于该主机的所有从机?', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "MasterSlaveMachinedescription.aspx/DeleteMasterMachineInfo",
                data: "{myId:'" + myId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == "1") {
                        $('#HiddenField_MasterMachineId').attr('value', '');
                        LoadMaterMachineData('last', myOrganizationId);         //刷新主机列表
                        LoadSlaveMachineData('last', myId);                     //刷新从机列表
                        $('#Text_SelectMasterMachine').attr('value', "");
                        alert("删除成功!");
                    }
                    else if (m_Msg == "-1") {
                        alert("数据库错误!");
                    }
                    else if (m_Msg == "0") {
                        alert("该主机已被删除!");
                    }
                    else {
                        alert(m_Msg);
                    }
                }
            });

        }
    });
}

//////////////////////////////////添加从机////////////////////////////////////////
function AddSlaveMachineFun(myKeyId, myVariableDescription) {
    $('#HiddenField_MasterMachineId').attr('value', myKeyId);
    $('#HiddenField_SlaveMachineId').attr('value', "");
    $('#TextBox_SlaveVariableName').textbox('setText', '');
    $('#TextBox_SlaveVariableDescription').textbox('setText', '');
    $('#TextBox_SlaveRemark').attr('value', '');
    //$('#Text_TimeDelay').timespinner('setValue', '00:05');
    $('#Text_TimeDelay').numberspinner('setValue', 1);
    $('#Radio_SlaveValidValueOff').attr('checked', 'checked');
    SlaveOrganizationId = "";
    SlaveMachineOpType = 0;
    LoadSlaveMachineData('last', myKeyId);                     //刷新从机列表
    $('#Text_SelectMasterMachine').attr('value', myVariableDescription);

    $('#dlg_AddSlaveMachine').dialog('open');
}
function EditSlaveMachineFun(myId) {
    $.ajax({
        type: "POST",
        url: "MasterSlaveMachinedescription.aspx/GetSlaveMachineInfobyId",
        data: "{myId:'" + myId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = jQuery.parseJSON(msg.d)['rows'];
            if (data) {
                $('#HiddenField_SlaveMachineId').attr('value', data[0].Id);
                $('#TextBox_SlaveVariableName').textbox('setText', data[0].VariableName);
                $('#TextBox_SlaveVariableDescription').textbox('setText', data[0].VariableDescription);
                $('#Text_TimeDelay').numberspinner('setValue', data[0].TimeDelay);
                $('#TextBox_SlaveRemark').attr('value', data[0].Remarks);
                if (data[0].ValidValues.toLocaleLowerCase() == 'true') {
                    $('#Radio_SlaveValidValueOn').attr('checked', true);
                }
                else {
                    $('#Radio_SlaveValidValueOff').attr('checked', true);
                }
                SlaveOrganizationId = data[0].OrganizationId;
            }
        }
    });

    SlaveMachineOpType = 1;
    $('#dlg_AddSlaveMachine').dialog('open');

}

function SaveSlaveMachine() {
    var m_SlaveMachineId = $('#HiddenField_SlaveMachineId').val();
    var m_OrganizationId = SlaveOrganizationId;                              //$('#TextBox_MasterVariableDescription').textbox('getValue');
    var m_VariableName = $('#TextBox_SlaveVariableName').textbox('getText');
    var m_VariableDescription = $('#TextBox_SlaveVariableDescription').textbox('getText');
    var m_KeyId = $('#HiddenField_MasterMachineId').val();
    var m_ValidValues = $("input[name='SelectRadio_SlaveValidValues']:checked").val();
    var m_TimeDelay = $('#Text_TimeDelay').numberspinner('getValue');
    var m_Remarks = $('#TextBox_SlaveRemark').val();

    var Valid_VariableName = $('#TextBox_SlaveVariableName').textbox('isValid');
    if (!Valid_VariableName) {
        alert('请填写变量名!');
    }
    else if (m_OrganizationId == "" || m_OrganizationId == null || m_OrganizationId == undefined) {
        alert('请选择所属DCS!');
    }
    else if (m_OrganizationId.length != 7) {
        alert('请选择到DCS节点!');
    }
    else {
        if (SlaveMachineOpType == 0) {              //添加主机设备
            $.ajax({
                type: "POST",
                url: "MasterSlaveMachinedescription.aspx/AddSlaveMachineInfo",
                data: "{myOrganizationId:'" + m_OrganizationId + "',myKeyId:'" + m_KeyId + "',myVariableName:'" + m_VariableName + "',myVariableDescription:'" + m_VariableDescription
                        + "',myValidValues:'" + m_ValidValues + "',myTimeDelay:'" + m_TimeDelay + "',myRemarks:'" + m_Remarks + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == '1') {
                        LoadSlaveMachineData('last', m_KeyId);                       //刷新从机列表
                        $('#dlg_AddSlaveMachine').dialog('close');
                        alert('添加成功!');
                    }
                    else if (m_Msg == '0') {
                        alert('添加失败!');
                    }
                    else if (m_Msg == '-1') {
                        alert('数据库错误!');
                    }
                    else {
                        alert(m_Msg);
                    }
                }
            });
        }
        else if (SlaveMachineOpType == 1) {                                     //修改主机设备
            $.ajax({
                type: "POST",
                url: "MasterSlaveMachinedescription.aspx/ModifySlaveMachineInfo",
                data: "{myId:'" + m_SlaveMachineId + "',myOrganizationId:'" + m_OrganizationId + "',myKeyId:'" + m_KeyId + "',myVariableName:'" + m_VariableName
                        + "',myVariableDescription:'" + m_VariableDescription + "',myValidValues:'" + m_ValidValues + "',myTimeDelay:'" + m_TimeDelay + "',myRemarks:'" + m_Remarks + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == '1') {
                        LoadSlaveMachineData('last', m_KeyId);                     //刷新从机列表
                        $('#dlg_AddSlaveMachine').dialog('close');
                        alert('修改成功!');
                    }
                    else if (m_Msg == '0') {
                        alert('修改失败!');
                    }
                    else if (m_Msg == '-1') {
                        alert('数据库错误!');
                    }
                    else {
                        alert(m_Msg);
                    }
                }
            });
        }
    }
}

////////////////////////////////删除从机///////////////////////////////
function DeleteSlaveMachineFun(myId, myOrganizationId) {
    parent.$.messager.confirm('询问', '您确定要删除该从机?', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "MasterSlaveMachinedescription.aspx/DeleteSlaveMachineInfo",
                data: "{myId:'" + myId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == "1") {
                        $('#HiddenField_SlaveMachineId').attr('value', '');
                        LoadSlaveMachineData('last', myOrganizationId);                     //刷新从机列表
                        alert("删除成功!");
                    }
                    else if (m_Msg == "-1") {
                        alert("数据库错误!");
                    }
                    else if (m_Msg == "0") {
                        alert("该从机已被删除!");
                    }
                    else {
                        alert(m_Msg);
                    }
                }
            });

        }
    });
}

function RemoveAllSlaveMachineFun() {
    var m_KeyId = $('#HiddenField_MasterMachineId').val();
    if (m_KeyId != "" && m_KeyId != null && m_KeyId != undefined) {
        parent.$.messager.confirm('询问', '您确定要删除所有从机?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: "MasterSlaveMachinedescription.aspx/DeleteAllSlaveMachineInfoByKeyId",
                    data: "{myKeyId:'" + m_KeyId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var m_Msg = msg.d;
                        if (m_Msg == "1") {
                            $('#HiddenField_SlaveMachineId').attr('value', '');
                            LoadSlaveMachineData('last', m_KeyId);                     //刷新从机列表
                            alert("删除成功!");
                        }
                        else if (m_Msg == "-1") {
                            alert("数据库错误!");
                        }
                        else if (m_Msg == "0") {
                            alert("该所有从机已被删除!");
                        }
                        else {
                            alert(m_Msg);
                        }
                    }
                });

            }
        });
    }
    else {
        alert("没有选择任何从机!");
    }
}