<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SET_DEPT_BONUSPERCENT.aspx.cs"
    Inherits="GoldNet.JXKP.Bonus.Set.SET_DEPT_BONUSPERCENT" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../lib/datatables/jquery.dataTables.css" />
    <link rel="stylesheet" href="../../lib/datatables/fixedColumns.dataTables.min.css" />
    <script type="text/javascript" src="../../lib/datatables/jquery-1.12.4.min.js"></script>
    <script type="text/javascript" src="../../lib/datatables/jquery.dataTables.js"></script>
    <script type="text/javascript" src="../../lib/datatables/dataTables.fixedColumns.min.js"></script>
    <link rel="stylesheet" href="../../lib/datatables/bootstrap.min.css"  />
    <script type="text/javascript" src="../../lib/datatables/bootstrap.min.js"></script>
    <link rel="stylesheet" href="../../lib/datatables/jquery-ui.css" />
    <script type="text/javascript" src="../../lib/datatables/jquery-ui.js"></script>
    <link rel="stylesheet" href="../../lib/datatables/all.min.css" />
    <script type="text/javascript" src="../../lib/datatables/jquery.ui.datepicker-zh-CN.js"></script>

    <style type="text/css">
        /* 保持表头自适应宽度 */
        table.dataTable thead th {
            white-space: nowrap;
        }

        /* 防止列文字换行 */
        table.dataTable td {
            white-space: nowrap;
        }

        
        .inline-block > *
        {
            display: inline-block;
            align-items: center; /* 垂直居中对齐 */
            margin: 0 5px;
            height:25px;
        }
        
        .inline-block 
        {
            width:100%;
            display: inline-block; /* 设置为行内块级元素 */
            background-color: #D4E1F2; /* 设置背景色 */
            color: Black; /* 设置文字颜色 */
            align-items: center; /* 垂直居中对齐 */
            
        }
        
        .border-example {
            border: 1px solid #8DB2E3; /* 设置边框宽度为2px，样式为实线，颜色为蓝色 */
            padding:0px 5px 5px 5px;
        }
        
        .custom-button {
            display: inline-block; /* 使按钮成为行内块级元素 */
            width: 50px; /* 设置按钮宽度 */
            height: 25px; /* 设置按钮高度 */
            background-color: #7AB153; /* 按钮背景色 */
            color: white; /* 按钮文字颜色 */
            font-weight: bold; /* 鼠标悬浮时加粗 */
            border: none; /* 去掉默认边框 */
            border-radius: 5px; /* 边角圆润 */
            text-align: center; /* 文字居中 */
            line-height: 25px; /* 行高与按钮高度相同以实现垂直居中 */
            font-size: 12px; /* 字体大小 */
            cursor: pointer; /* 鼠标悬停时显示为手型 */
            transition: background-color 0.3s; /* 背景色过渡效果 */
        }
        
        .custom-button-del {
            display: inline-block; /* 使按钮成为行内块级元素 */
            width: 70px; /* 设置按钮宽度 */
            height: 25px; /* 设置按钮高度 */
            background-color: #F63803; /* 按钮背景色 */
            color: white; /* 按钮文字颜色 */
            font-weight: bold; /* 鼠标悬浮时加粗 */
            border: none; /* 去掉默认边框 */
            border-radius: 5px; /* 边角圆润 */
            text-align: center; /* 文字居中 */
            line-height: 25px; /* 行高与按钮高度相同以实现垂直居中 */
            font-size: 12px; /* 字体大小 */
            cursor: pointer; /* 鼠标悬停时显示为手型 */
            transition: background-color 0.3s; /* 背景色过渡效果 */
        }

        .custom-button:hover 
        {
            background-color: #239243; /* 悬停时改变背景色 */
        }
        
        .custom-button-del:hover 
        {
            background-color: #C13803; /* 悬停时改变背景色 */
        }
        
        /* 固定列样式 */
        .fixed-column 
        {
            position: sticky;
            left: 0;
            background: white;
            z-index: 1;
        }
        
        .back
        {
            
            border: 1px solid red;
            padding: 1px; /* 内边距 */
            margin: 1px; /* 外边距 */
        }
        
        .dataTables_wrapper
        {
            border: 1px solid yellow;
        }
        
        .table-container 
        {
            position:fixed;
            top:50px;
            bottom:0;
            left:0;
            right:0;
            margin: 1px;
            border: 1px solid green;
        }
        
        body
        {
            height:100%;     
        }
    </style>
</head>
<body>
<div class="back">
        <div class="inline-block border-example">
            <label for="datePicker">选择日期:</label>
            <input type="text" id="datePicker" name="date"/>
            <button id="searchBtn" class="custom-button">查询</button>
            <label>全选:</label>
            <input type="checkbox" id="selectAll" style="margin:0px;padding:0px;position:relative;top:8px;"/>
            <button id="deleteSelectedBtn" class="custom-button-del">批量删除</button>
            <label for="">页内搜索:</label>
            <input type="text"  id="custom-search-box"/>
        </div>

        <div id="containerDiv" class="table-container">
            <table id="example" class="display" style="width: 100%;">
                <thead>
                    <tr></tr>
                </thead>
            </table>
        </div>
    
    <script type="text/javascript">
        $(document).ready(function () {

            // 设置一个标志位，防止多次触发
            var resizeTimeout;

            // 计算 scrollY 的高度
            function calculateScrollY() {
                var containerHeight = $('#containerDiv').height(); // 获取容器的高度
                var tableOffsetTop = $('#example').offset().top; // 获取表格相对于页面顶部的偏移量
                var availableHeight = containerHeight - tableOffsetTop; // 计算可用高度
                return availableHeight + 'px'; // 返回带有 'px' 单位的值
            }

            // 窗口调整大小时重新计算 scrollY 的高度
            $(window).on('resize', function () {
                // 清除之前的 timeout，防止多次触发
                if (resizeTimeout) {
                    clearTimeout(resizeTimeout);
                }

                // 使用 setTimeout 模拟实时更新
                resizeTimeout = requestAnimationFrame(function () {
                    var newScrollY = calculateScrollY(); // 重新计算高度
                    table.settings()[0].oScroll.sY = newScrollY; // 更新 DataTables 的 scrollY 设置
                    table.draw(false); // 重新绘制表格但不重置分页
                }); // 延迟时间可以根据需要调整
            });

            // 初始化jQuery UI日期选择器
            $("#datePicker").datepicker({
                dateFormat: "yymmdd",
                showButtonPanel: true,           // 显示按钮面板
                changeMonth: true,               // 允许选择月份
                changeYear: true,                // 允许选择年份
                monthNames: ["一月", "二月", "三月", "四月", "五月", "六月",
                             "七月", "八月", "九月", "十月", "十一月", "十二月"]
            });

            function loadTable(date) {
                $.ajax({
                    url: "SET_DEPT_BONUSPERCENT.aspx?action=getData&date=" + (date || ""),
                    method: "GET",
                    success: function (response) {
                        $('#example').DataTable({
                            "paging": false,
                            "destroy": true,
                            "columns": response.columns,
                            "data": response.data,
                            "scrollX": true,
                            "scrollY": calculateScrollY(),
                            "scrollCollapse": true,
                            "columnDefs": [
                            {
                                "targets": 3, // 固定首列
                                "className": 'fixed-column'
                            }
                            ],
                            "language": {
                                "url": "../../lib/datatables/Chinese.json"
                            },
                            "dom": 'lrtip'
                        });

                        // 在 DataTable 初始化之后绑定编辑和删除按钮的事件
                        bindActionButtons();
                    }
                });
            }

            // 初始化表格
            loadTable();

            // 自定义搜索框绑定到 DataTables 的搜索功能
            $('#custom-search-box').on('keyup', function () {
                $('#example').DataTable().search(this.value).draw();
            });

            // 绑定查询按钮点击事件
            $("#searchBtn").on("click", function () {
                var selectedDate = $("#datePicker").val();
                if (selectedDate) {
                    loadTable(selectedDate);
                } else {
                    alert("请选择一个日期！");
                }
            });

            // 绑定全选/取消全选功能
            $('#selectAll').on('click', function () {
                var rows = $('#example').DataTable().rows({ 'search': 'applied' }).nodes();
                $('input[type="checkbox"]', rows).prop('checked', this.checked);
            });

            // 批量删除功能
            $('#deleteSelectedBtn').on('click', function () {
                var selectedIds = [];
                $('#example tbody input.row-select:checked').each(function () {
                    var data = $('#example').DataTable().row($(this).parents('tr')).data();
                    selectedIds.push(data.ID);
                });

                if (selectedIds.length > 0) {
                    if (confirm('确定要删除选中的行吗？')) {
                        $.ajax({
                            url: 'SET_DEPT_BONUSPERCENT.aspx?action=deleteRows',
                            method: 'POST',
                            data: { ids: selectedIds.join(",") },
                            success: function (response) {
                                if (response.success) {
                                    loadTable($("#datePicker").val());
                                } else {
                                    alert('删除失败');
                                }
                            }
                        });
                    }
                } else {
                    alert('请先选择要删除的行。');
                }
            });

            // 绑定编辑和删除按钮事件
            function bindActionButtons() {

                // 取消之前绑定的事件，防止重复绑定
                $('#example tbody').off('click', '.delete-btn');
                $('#example tbody').off('click', '.edit-btn');

                // 删除行事件
                $('#example tbody').on('click', '.delete-btn', function () {
                    var data = $('#example').DataTable().row($(this).parents('tr')).data();
                    if (confirm('确定要删除此行吗？')) {
                        $.ajax({
                            url: 'SET_DEPT_BONUSPERCENT.aspx?action=deleteRow',
                            method: 'POST',
                            data: { id: data.ID },
                            success: function (response) {
                                if (response.success) {
                                    loadTable($("#datePicker").val());
                                } else {
                                    alert('删除失败');
                                }
                            }
                        });
                    }
                });

                // 编辑行事件
                $('#example tbody').on('click', '.edit-btn', function () {
                    var data = $('#example').DataTable().row($(this).parents('tr')).data();
                    $('#deptCode').val(data.Dept_Code);
                    $('#deptName').val(data.科室名称);
                    $('#YZB').val(data.药占比);
                    $('#editModal').modal('show');
                });
            }

            // 保存更改事件
            $('#saveBtn').click(function () {
                var formData = $('#editForm').serialize();
                $.post('SET_DEPT_BONUSPERCENT.aspx?action=saveRow', formData, function (response) {
                    if (response.success) {
                        $('#editModal').modal('hide');
                        loadTable($("#datePicker").val());
                    } else {
                        alert('保存失败');
                    }
                }, 'json');

            });
        });
    </script>
    <!-- Bootstrap Modal -->
    <div id="editModal" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title">编辑用户信息</h4>
                </div>
                <div class="modal-body">
                    <form id="editForm">
                    <div class="form-group">
                        <label for="deptName">科室名称</label>
                        <input type="text" class="form-control" id="deptName" name="name" required />
                    </div>
                    <div class="form-group">
                        <label for="YZB">药占比</label>
                        <input type="number" class="form-control" id="YZB" name="yzb" required />
                    </div>
                    <input type="hidden" id="deptCode" name="id" />
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    <button type="button" class="btn btn-primary" id="saveBtn">保存更改</button>
                </div>
            </div>
        </div>
    </div>
</div>
</body>
</html>
