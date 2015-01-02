﻿var m_levelCode;
var m_productType;
$(function () {
    //var m_UserName = $('#HiddenField_UserName').val();
    //loadGridData('first');
    InitializeGrid('');
    //loadGridData('first');//
});

function loadGridData(myLoadType) {
    //parent.$.messager.progress({ text: '数据加载中....' });
    var m_OrganizationId = $('#organizationId').val();
    //alert(m_OrganizationId);
    var m_MsgData;
    $.ajax({
        type: "POST",
        url: "EnergyPredit.aspx/GetReportData",
        data: "{OrganizationId:'"+m_OrganizationId+"',leveCode:'"+m_levelCode+"'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (myLoadType == 'first') {
                m_MsgData = jQuery.parseJSON(msg.d);
                InitializeGrid(m_MsgData);
            }
            else if (myLoadType == 'last') {
                m_MsgData = jQuery.parseJSON(msg.d);
                $('#gridMain_ReportTemplate').datagrid('loadData', m_MsgData['rows']);
            }
        },
        error: handleError
    });
}

function handleError() {
    $('#gridMain_ReportTemplate').datagrid('loadData', []);
    $.messager.alert('失败', '获取数据失败');
}

function InitializeGrid(myData) {
    $('#gridMain_ReportTemplate').datagrid({
        title: '',
        data: "",
        dataType: "json",
        striped: true,//设置为 true，则把行条纹化。（即奇偶行使用不同背景色）
        //idField: "field",//指示哪个字段是标识字段。
        //frozenColumns: [[m_IdAndNameColumn[1]]],
        columns: [[
            	{ field: 'IndicatorName', title: '指标项目名称', width: 100 },
                { field: 'January', title: '一月', width: 100 },
                { field: 'February', title: '二月', width: 100 },
                { field: 'March', title: '三月', width: 100 },
                { field: 'April', title: '四月', width: 100 },
                { field: 'May', title: '五月', width: 100 },
                { field: 'June', title: '六月', width: 100 },
                { field: 'July', title: '七月', width: 100 },
                { field: 'August', title: '八月', width: 100 },
                { field: 'September', title: '九月', width: 100 },
                { field: 'October', title: '十月', width: 100 },
                { field: 'November', title: '十一月', width: 100 },
                { field: 'December', title: '十二月', width: 100 },
                { field: 'Totals', title: '年度', width: 100 },
                { field: 'Remarks', title: '备注', width: 100 },
        ]],
        //loadMsg: '',   //设置本身的提示消息为空 则就不会提示了的。这个设置很关键的
        rownumbers: true,
        //pagination: true,
        singleSelect: true,
        //onClickCell: onClickCell,
        //idField: m_IdAndNameColumn[0].field,
        //pageSize: 20,
        //pageList: [20, 50, 100, 500],

        toolbar: '#toolbar_ReportTemplate'
    });

    //for(
}

function ExportFileFun() {
    var m_FunctionName = "ExcelStream";
    var m_Parameter1 = "Parameter1";
    var m_Parameter2 = "Parameter2";

    var form = $("<form id = 'ExportFile'>");   //定义一个form表单
    form.attr('style', 'display:none');   //在form表单中添加查询参数
    form.attr('target', '');
    form.attr('method', 'post');
    form.attr('action', "report_CementMilMonthlyEnergyConsumption.aspx");

    var input_Method = $('<input>');
    input_Method.attr('type', 'hidden');
    input_Method.attr('name', 'myFunctionName');
    input_Method.attr('value', m_FunctionName);
    var input_Data1 = $('<input>');
    input_Data1.attr('type', 'hidden');
    input_Data1.attr('name', 'myParameter1');
    input_Data1.attr('value', m_Parameter1);
    var input_Data2 = $('<input>');
    input_Data2.attr('type', 'hidden');
    input_Data2.attr('name', 'myParameter2');
    input_Data2.attr('value', m_Parameter2);

    $('body').append(form);  //将表单放置在web中 
    form.append(input_Method);   //将查询参数控件提交到表单上
    form.append(input_Data1);   //将查询参数控件提交到表单上
    form.append(input_Data2);   //将查询参数控件提交到表单上
    form.submit();
    //释放生成的资源
    form.remove();

    /*
    var m_Parmaters = { "myFunctionName": m_FunctionName, "myParameter1": m_Parameter1, "myParameter2": m_Parameter2 };
    $.ajax({
        type: "POST",
        url: "Report_Example.aspx",
        data: m_Parmater,                       //'myFunctionName=' + m_FunctionName + '&myParameter1=' + m_Parameter1 + '&myParameter2=' + m_Parameter2,
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == "1") {
                alert("导出成功!");
            }
            else{
                alert(msg.d);
            }
        }
    });
    */
}
function RefreshFun() {
    loadGridData('last');
}
function PrintFileFun() {
    $.ajax({
        type: "POST",
        url: "report_CementMilMonthlyEnergyConsumption.aspx/PrintFile",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            PrintHtml(msg.d);
        }
    });


}

function QueryReportFun() {
    var organizationID = $('#organizationId').val();
    //var levelCode = $('#organizationId').levelCode();
    //var datetime = $('#datetime').datetimespinner('getValue');
    if (organizationID == "") {
        $.messager.alert('警告', '请选择生产线');
        return;
    }
    if (m_productType.trim() != '水泥磨' && m_productType.trim() != '熟料' && m_productType.trim() != '') {
        $.messager.alert('警告', '请选择水泥磨，熟料生产线或分厂及以上节点');
        return;
    }
    loadGridData('last');
   // loadGridData('first');
}

function onOrganisationTreeClick(node) {
    $('#productLineName').textbox('setText', node.text); //组织机构的名字
    $('#organizationId').val(node.OrganizationId);//用一个隐藏控件传递organizationId的值OrganizationId
    m_levelCode = node.id;//
    m_productType = node.OrganizationType;
    //alert("Id:" + node.OrganizationId+"leveCode:"+node.id);
}

// datetime spinner
function formatter2(date) {
    if (!date) { return ''; }
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    return y + '-' + (m < 10 ? ('0' + m) : m);
}
function parser2(s) {
    if (!s) { return null; }
    var ss = s.split('-');
    var y = parseInt(ss[0], 10);
    var m = parseInt(ss[1], 10);
    if (!isNaN(y) && !isNaN(m)) {
        return new Date(y, m - 1, 1);
    } else {
        return new Date();
    }
}

//$(function () {
//    $('.combo-arrow').click(function () {
//        $('.calendar-title > span').click();
//        $('.calendar-menu-month').click(function () {
//            $("tr.calendar-first > .calendar-last").click();
//        });
//    });
//});
