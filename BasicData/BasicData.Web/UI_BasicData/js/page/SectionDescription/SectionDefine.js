var m_WorkingSectionName = '';
var m_OrganizationID = '';
var m_VariableName = '';
var m_Out_put = '';
var m_PulverizedCoalInput = '';
var m_Creator = '';
var m_Remarks = '';


$(function () {
    InitializeGrid();
});
////初始化页面的增删改查权限
//function initPageAuthority() {
//    $.ajax({
//        type: "POST",
//        url: "EquipmentAccountEdit.aspx/AuthorityControl",
//        data: "",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: false,//同步执行
//        success: function (msg) {
//            var authArray = msg.d;
//            //增加
//            if (authArray[1] == '0') {
//                $("#add").linkbutton('disable');

//            }
//            //修改
//            if (authArray[2] == '0') {
//                $("#edit").linkbutton('disable');
//                $("#saveEditDlg").linkbutton('disable');
//                var itemEl = $('#mm_edit')[0];
//                $("#RightMenu").menu('disableItem', itemEl);
//            }
//            //删除
//            if (authArray[3] == '0') {
//                $("#delete").linkbutton('disable');
//                var itemEl = $('#mm_delete')[0];
//                $("#RightMenu").menu('disableItem', itemEl);
//            }
//        }
//    });
//}

function loadGridData(myLoadType) {
    var m_OrganizationId = $("#organizationId").val();
    $.ajax({
        type: "POST",
        url: "SectionDefine.aspx/GetSectionDefineInfo",
        data: "{organizationId:'" + m_OrganizationId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (myLoadType == 'first') {
                m_MsgData = jQuery.parseJSON(msg.d);
                InitializeGrid(m_MsgData);
                }
            else if (myLoadType == 'last') {
                $('#SectionDefine_Info').datagrid('loadData', []);
                m_MsgData = jQuery.parseJSON(msg.d);
                $('#SectionDefine_Info').datagrid('loadData', m_MsgData['rows']);
            }
        },
        error: function () { $.messager.alert('警告', '获取数据失败！') }
    });
}

function InitializeGrid(myData) {
    $('#SectionDefine_Info').datagrid({
        data: myData,
        columns: [[
            { field: 'WorkingSectionItemID', title: '岗位项目ID', width: 80 },
            { field: 'WorkingSectionID', title: '工作岗位ID', width: 80},
            { field: 'WorkingSectionName', title: '岗位名称', width: 80 },
            { field: 'Type', title: '类别', width: 80 },
            { field: 'OrganizationID', title: '组织机构ID', width: 110 },
            { field: 'DisplayIndex', title: '显示顺序', width: 60 },
            { field: 'ElectricityQuantityId', title: '考核电量ID', width: 180 },
            { field: 'OutputId', title: '考核产量ID', width: 180 },
            { field: 'CoalWeightId', title: '考核耗煤量ID', width: 120 },
            { field: 'Creator', title: '修改人', width: 70 },
            { field: 'CreatedTime', title: '修改时间', width: 100 },
            { field: 'Enabled', title: '使能', width: 40 },
            { field: 'Remarks', title: '备注', width: 100 }
        ]],
        rownumbers: true,
        singleSelect: true,

    });
}
function onOrganisationTreeClick(node) {
    $('#productLineName').textbox('setText', node.text); //组织机构的名字
    $('#organizationId').val(node.OrganizationId);//用一个隐藏控件传递organizationId的值OrganizationId
}
////加载二电量VariableName
//m_OrganizationID = node.OrganizationId;




function QueryReportFun() {
    var organizationID = $('#organizationId').val();
    if (organizationID == "") {
        $.messager.alert('警告', '请选择生产线');
        return;
    }
    loadGridData('last');
    // loadGridData('first');
}

//m_WorkingSectionItemID = '';
//m_WorkingSectionID = '';
//m_WorkingSectionName = '';
//m_Type = '';
//m_OrganizationID = '';
//m_DisplayIndex = '';
//m_ElectricityQuantityId = '';
//m_OutputId = '';
//m_CoalWeightId = '';
//m_Creator = '';
//m_CreatedTime = '';
//m_Enabled = '';
//m_Remarks = '';

//var org;
function AddSectionDefine()
{
    var m_OrganizationId = $("#organizationId").val();
    if (m_OrganizationId == "" || m_OrganizationId == undefined) {
        $.messager.alert('警告', '请先选择生产线！');
        return;
    }
    $('#AddDialog').dialog('open');
    //清空数据
    //m_WorkingSectionName = $('#WorkingSectionName').textbox('getValue');
    //m_OrganizationID = $('#OrganizationID').textbox('setValue',OrganizationID_old);
    //m_ElectricityQuantityId = $('#ElectricityQuantityId').combotree('setValue', ElectricityQuantityId_old);
    //m_OutputId = $('#OutputId').combobox('setValue', OutputId_old);
    ////m_CoalWeightId = $('#CoalWeightId_edit').combotbox('setValue', CoalWeightId_old);
    //m_Creator = $('#Creator').textbox('setValue', Creator_old);
    //m_Remarks = $('#Remarks').val(Remarks_old);



    //加载OrganizationID
    var p = /clinker/;
    $.ajax({
        type: "POST",
        url: "SectionDefine.aspx/GetOrganizationIdInfo",
        data: "{organizationId:'" + m_OrganizationId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            m_MsgData = jQuery.parseJSON(msg.d);
            $('#OrganizationID').combobox({
                data: m_MsgData,
                valueField: 'OrganizationID',
                textField: 'Name',
                onSelect: function (record) {
                    //if (record.LevelType == "Factory") {
                    //    $.messager.alert('警告', '请选择机构生产线！');
                    //}
                    //else
                        if (p.test(record.OrganizationID)) {
                        $('#PulverizedCoalInputLabel').show();
                        $('#PulverizedCoalInputSelect').show();
                        loadPulverizedCoalInput(record.OrganizationID);

                        loadVariableNameData(record.OrganizationID);
                        loadOutputData(record.OrganizationID);
                    }
                    else {
                        loadVariableNameData(record.OrganizationID);
                        loadOutputData(record.OrganizationID);
                    }
                   
                }
            });      
        },
    });
}
function loadVariableNameData(s_OrganizationID)
{
    $.ajax({
        type: "POST",
        url: "SectionDefine.aspx/GetJsonofVariableNameTree",
        contentType: "application/json; charset=utf-8",
        data: '{organizationId:"' + s_OrganizationID + '"}',
        dataType: "json",
        success: function (msg) {
            if (msg.d == "[]") {
                alert("请选择组织机构！");
            }
            else {
                m_MsgData = jQuery.parseJSON(msg.d);
                $('#VariableName').combotree("loadData", m_MsgData);
                $('#VariableName').combotree("collapseAll");
            }
        },
        error: function () { $.messager.alert('警告', '电量类别数据获取失败！') }
    });
}
function loadOutputData(s_OrganizationID)
{
    $.ajax({
        type: "POST",
        url: "SectionDefine.aspx/GetJsonofOutputInfo",
        data: "{organizationId:'" + s_OrganizationID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            m_MsgData = jQuery.parseJSON(msg.d);
            $('#Out_put').combobox({
                data: m_MsgData,
                valueField: 'VariableId',
                textField: 'Out_put'
            });
        }
    });
}
function loadPulverizedCoalInput(s_OrganizationID)
{
    $.ajax({
        type: "POST",
        url: "SectionDefine.aspx/GetJsonofPulverizedCoalInfo",
        data: "{organizationId:'" + s_OrganizationID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            m_MsgData = jQuery.parseJSON(msg.d);
            $('#PulverizedCoalInput').combobox({
                data: m_MsgData,
                valueField: 'VariableId',
                textField: 'PulverizedCoalInput'
            });
        }
    });
}
function SaveSectionDefine() {
    //获取参数值
    m_WorkingSectionName = $('#WorkingSectionName').textbox('getValue');
    m_OrganizationID = $('#OrganizationID').combo('getValue');
    m_VariableName = $('#VariableName').combo('getValue');
    m_Out_put = $('#Out_put').combo('getValue');
    m_PulverizedCoalInput = $('#PulverizedCoalInput').combo('getValue');
    m_Creator = $('#Creator').textbox('getValue');
    m_Remarks = $('#Remarks').val();
    if (m_WorkingSectionName == '' || m_WorkingSectionName == undefined
        || m_VariableName == '' || m_VariableName == undefined
        || m_Out_put == '' || m_Out_put == undefined
        || m_Creator == '' || m_Creator == undefined      
        ) {
        $.messager.alert('提示', '请补充未填项！');
        return;
    }
    var m_Datas = "{WorkingSectionName:'" + m_WorkingSectionName + "',OrganizationID:'" + m_OrganizationID +
        "',VariableName:'" + m_VariableName + "',Out_put:'" + m_Out_put + "',PulverizedCoalInput:'" +
        m_PulverizedCoalInput + "',Creator:'" + m_Creator + "',Remarks:'" + m_Remarks + "'}";
    $.ajax({
        type: "POST",
        url: "SectionDefine.aspx/SaveSectionDefineInfo",
        data: m_Datas,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $.messager.alert('提示', msg.d);
            loadGridData("last");
            $('#AddDialog').dialog('close');
        },
        error: function () { $.messager.alert('警告', '保存数据失败！') }
    });
}
function RemoveSectionDefine() {
    var selectedRow = $('#SectionDefine_Info').datagrid('getSelected');
    var WorkingSectionItemID_old = selectedRow.WorkingSectionItemID;
    var WorkingSectionID_old = selectedRow.WorkingSectionID;
    if (selectedRow == undefined)
        $.messager.alert("提示", "请先选择一行数据");
    $.ajax({
        type: "POST",
        url: "SectionDefine.aspx/RemoveSectionDefineInfo",
        data: "{ WorkingSectionItemID_old:'" + WorkingSectionItemID_old + "',WorkingSectionID_old:'" + WorkingSectionID_old + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $.messager.alert('提示', msg.d);
            loadGridData("last");
        },
        error: function () { $.messager.alert('警告', '删除设备失败！') }
    });
}
function EditSectionDefine() {
    //编辑前的数据
    var selectedRow = $('#SectionDefine_Info').datagrid('getSelected');
    if (selectedRow == undefined)
        $.messager.alert("提示", "请先选择一行数据");

    var WorkingSectionName_old = selectedRow.WorkingSectionName;
    var OrganizationID_old=selectedRow.OrganizationID;
    var ElectricityQuantityId_old=selectedRow.ElectricityQuantityId;
    var OutputId_old=selectedRow.OutputId;
    //var CoalWeightId_old=selectedRow.CoalWeightId;
    var Creator_old=selectedRow.Creator;
    var Remarks_old=selectedRow.Remarks;

    //打开修改对话框
    $('#EditDialog').dialog('open');

    m_WorkingSectionName = $('#WorkingSectionName_edit').textbox('setValue', WorkingSectionName_old);//validatebox('getValue');
    //m_OrganizationID = $('#OrganizationID_edit').textbox('setValue',OrganizationID_old);
    
    //m_ElectricityQuantityId = $('#ElectricityQuantityId_edit').combotree('setValue', ElectricityQuantityId_old);
    //m_OutputId = $('#OutputId_edit').combobox('setValue', OutputId_old);
    //m_CoalWeightId = $('#CoalWeightId_edit').combotbox('setValue', CoalWeightId_old);
    m_Creator = $('#Creator_edit').textbox('setValue', Creator_old);
    m_Remarks = $('#Remarks_edit').val(Remarks_old);
   

    //m_WorkingSectionName = $('#WorkingSectionName').textbox('getValue');
    //m_OrganizationID = $('#OrganizationID').combo('getValue');
    //m_VariableName = $('#VariableName').combo('getValue');
    //m_Out_put = $('#Out_put').combo('getValue');
    //m_PulverizedCoalInput = $('#PulverizedCoalInput').combo('getValue');
    //m_Creator = $('#Creator').textbox('getValue');
    //m_Remarks = $('#Remarks').val();

    var m_OrganizationId = $("#organizationId").val();
    if (m_OrganizationId == '' || m_OrganizationId == undefined) {
        $.messager.alert("请选择组织机构！");
        return;
    }
    else {
        $.ajax({
            type: "POST",
            url: "SectionDefine.aspx/GetOrganizationIdInfo",
            data: "{organizationId:'" + m_OrganizationId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d == "[]") {
                    alert("请选择组织机构！");
                }
                else {
                    m_MsgData = jQuery.parseJSON(msg.d);
                    $('#OrganizationID_edit').combobox({
                        data: m_MsgData,
                        valueField: 'OrganizationID',
                        textField: 'Name'
                    });
                    $('#OrganizationID_edit').combobox('setValue', OrganizationID_old);
                }
            }
        });
        var EQStr = ElectricityQuantityId_old.split('_');
        var ElectricityQuantity = EQStr[0];
        $.ajax({
            type: "POST",
            url: "SectionDefine.aspx/GetJsonofVariableNameTree",
            contentType: "application/json; charset=utf-8",
            data: '{organizationId:"' + m_OrganizationId + '"}',
            dataType: "json",
            success: function (msg) {
                if (msg.d == "[]") {
                    alert("请选择组织机构！");
                }
                else {
                    m_MsgData = jQuery.parseJSON(msg.d);
                    $('#VariableName_edit').combotree({
                        data: m_MsgData,
                        valueField: 'VariableId',
                        textField: 'VariableName'
                    });
                    $('#VariableName_edit').combotree('setValue', ElectricityQuantity);
                    $('#VariableName_edit').combotree('tree').tree("collapseAll");
                }
            }
        });
        $.ajax({
            type: "POST",
            url: "SectionDefine.aspx/GetJsonofOutputInfo",
            data: "{organizationId:'" + m_OrganizationId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                m_MsgData = jQuery.parseJSON(msg.d);
                $('#Out_put_edit').combobox({
                    data: m_MsgData,
                    valueField: 'VariableId',
                    textField: 'Out_put'
                });
                $('#Out_put_edit').combobox('setValue', OutputId_old);
            }
        });
    }
}
function SaveEditSectionDefine() {
    var selectedRow = $('#SectionDefine_Info').datagrid('getSelected');
    var WorkingSectionItemID_old=selectedRow.WorkingSectionItemID;
    var WorkingSectionID_old=selectedRow.WorkingSectionID;
    //获取参数值
    m_WorkingSectionName = $('#WorkingSectionName_edit').textbox('getValue');
    m_OrganizationID = $('#OrganizationID_edit').combo('getValue');
    m_VariableName = $('#VariableName_edit').combo('getValue');
    m_Out_put = $('#Out_put_edit').combo('getValue');
    //m_PulverizedCoalInput = $('#PulverizedCoalInput').combo('getValue');
    m_Creator = $('#Creator_edit').textbox('getValue');
    m_Remarks = $('#Remarks_edit').val();
    if (m_WorkingSectionName == '' || m_WorkingSectionName == undefined
        || m_VariableName == '' || m_VariableName == undefined
        || m_Out_put == '' || m_Out_put == undefined
        || m_Creator == '' || m_Creator == undefined      
        ) {
        $.messager.alert('提示', '请补充未填项！');
        return;
    }

    var m_Datas = "{ WorkingSectionItemID_old:'" + WorkingSectionItemID_old + "',WorkingSectionID_old:'" + WorkingSectionID_old + "',WorkingSectionName:'" + m_WorkingSectionName + "',OrganizationID:'" + m_OrganizationID +
        "',VariableName:'" + m_VariableName + "',Out_put:'" + m_Out_put + "',Creator:'" + m_Creator + "',Remarks:'" + m_Remarks + "'}";
    $.ajax({
        type: "POST",
        url: "SectionDefine.aspx/SaveEditSectionDefine",
        data: m_Datas,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $.messager.alert('提示', msg.d);
            loadGridData("last");
            $('#EditDialog').dialog('close');
        },
        error: function () { $.messager.alert('警告', '保存数据失败！') }
    });
}
function onRowContextMenu(e, rowIndex, rowData) {
    e.preventDefault();
    $(this).datagrid('selectRow', rowIndex);
    $('#RightMenu').menu('show', {
        left: e.pageX,
        top: e.pageY
    });
}