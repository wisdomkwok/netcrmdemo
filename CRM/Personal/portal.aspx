<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerPanel.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerPortal.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script src="../JS/FusionCharts/FusionCharts.js" type="text/javascript"></script>
    <script type="text/javascript">
        var manager;
        $(function () {
            manager = $("#portalMain").ligerPortal({
                draggable: false,
                rows: [
                    {
                        columns: [
                            {
                                width: '33%',
                                panels: [
                                    {
                                        title: '新闻',
                                        width: '99%',
                                        height: 200,
                                        content_id: 'c_news'
                                    }
                                ]
                            },
                        {
                            width: '33%',
                            panels: [
                                {
                                    title: '公告',
                                    width: '99%',
                                    height: 200,
                                    content_id: 'c_notice'
                                }
                            ]
                        },
                         {
                             width: '33%',
                             panels: [
                                 {
                                     title: '日程',
                                     width: '99%',
                                     height: 200,
                                     content_id: 'c_calendar'
                                 }
                             ]
                         }
                        ]
                    }
                    , {
                        columns: [{
                            width: '66%',
                            panels: [{
                                title: '统计年报',
                                width: '99.5%',
                                height: 200,
                                content_id: 'c_report'
                            }
                            ]
                        },
                        {
                            width: '33%',
                            panels: [{
                                title: '便签',
                                width: '99%',
                                height: 200,
                                content_id: 'c_note'
                            }
                            ]
                        }
                        ]
                    }
                ]
            });
            notice();
            todo();
            news();
            note();
            report();
        });
        function notice() {  //公告提醒
            $.getJSON("../data/public_notice.ashx?Action=noticeremind&rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>")
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/18.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='#' onclick=\"window.top.f_addTab('tabid34','公告','Personal/Message/notice.aspx')\">" + obj.Rows[i].notice_title + "</a></div></td><td width='80'>" + formatTimebytype(obj.Rows[i].notice_time, 'yyyy-MM-dd') + "</td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_notice"))
            });
        }
        function todo() {  //日程安排提醒
            $.getJSON("../data/Personal_Calendar.ashx?Action=Today&rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>")
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/31.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='#' onclick=\"window.top.f_addTab('tabid39','日程安排','personal/personal/Calendar.aspx')\">" + obj.Rows[i].Subject + "</a></div></td><td width='80'>" + formatTimebytype(obj.Rows[i].StartTime, 'yyyy-MM-dd') + "</td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_calendar"))
            });

        }
        function news() {  //新闻
            $.getJSON("../data/public_news.ashx?Action=newsremind&rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>")
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/10.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='#' onclick=\"window.top.f_addTab('tabid33','新闻','personal/message/news.aspx')\">" + obj.Rows[i].news_title + "</a></div></td><td width='80'>" + formatTimebytype(obj.Rows[i].news_time, 'yyyy-MM-dd') + "</td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_news"))
            });

        }
        function note() {  //便签
            $.getJSON("../data/Personal_notes.ashx?Action=notesremind&rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>")
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/35.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='#' onclick=\"window.top.f_addTab('tabid38','便签','personal/personal/notes.aspx')\">" + obj.Rows[i].note_content

                        + "</a></div></td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_note"))
            });

        }
        function report() {
            var d = new Date();
            var nowYear = +d.getFullYear();
            var sendtxt = "&Action=CRM_Reports_year&stype=%E5%AE%A2%E6%88%B7%E7%B1%BB%E5%9E%8B&stype_val=CustomerType&syear_val=" + nowYear + "&rnd=" + Math.random();
            $.ajax({
                url: "../data/Reports_CRM.ashx", type: "POST",
                data: sendtxt,
                dataType: 'json',              
                success: function (responseText) {                     
                    test(responseText);
                }
            });
            //test();

        }
        function test(GridData) {              
            var chart_myNext = new FusionCharts("../js/FusionCharts/FCF_MSColumn3DLineDY.swf", "myNext", "670", "200", "0", "0");
            var color = ['AFD8F8', 'F6BD0F', '8BBA00', 'FF8E46', '008E8E', 'D64646', '8E468E', '588526', 'B3AA00', '008ED6', '9D080D', 'A186BE', 'AFD8F8', 'F6BD0F', '8BBA00', 'FF8E46', '008E8E', 'D64646', '8E468E', '588526', 'B3AA00', '008ED6', '9D080D', 'A186BE', 'AFD8F8', 'F6BD0F', '8BBA00', 'FF8E46', '008E8E', 'D64646', '8E468E', '588526', 'B3AA00', '008ED6', '9D080D', 'A186BE', 'AFD8F8', 'F6BD0F', '8BBA00', 'FF8E46', '008E8E', 'D64646', '8E468E', '588526', 'B3AA00', '008ED6', '9D080D', 'A186BE', 'AFD8F8', 'F6BD0F', '8BBA00', 'FF8E46', '008E8E', 'D64646', '8E468E', '588526', 'B3AA00', '008ED6', '9D080D', 'A186BE'];
            var xmltext = "";
            xmltext += "<graph decimalPrecision='0' formatNumberScale='0'  baseFontSize='12'  yAxisMinValue='0' yAxisMaxValue='1' showValues='0'>";
            xmltext += "   <categories>";
            xmltext += "        <category name='一月' /><category name='二月' /><category name='三月' /><category name='四月' /><category name='五月' /><category name='六月' /><category name='七月' /><category name='八月' /><category name='九月' /><category name='十月' /><category name='十一月' /><category name='十二月' />";
            xmltext += "   </categories>";

            var mc1 = 0; mc2 = 0; mc3 = 0; mc4 = 0; mc5 = 0; mc6 = 0; mc7 = 0; mc8 = 0; mc9 = 0; mc10 = 0; mc11 = 0; mc12 = 0;
            for (var i = 0; i < GridData.Rows.length; i++) {
                var flowtype = GridData.Rows[i].items;
                if (GridData.Rows[i].items == "undefined")
                    flowtype = "未分类";
                var m1 = typeof (GridData.Rows[i].m1) != 'number' ? 0 : GridData.Rows[i].m1;
                var m2 = typeof (GridData.Rows[i].m2) != 'number' ? 0 : GridData.Rows[i].m2;
                var m3 = typeof (GridData.Rows[i].m3) != 'number' ? 0 : GridData.Rows[i].m3;
                var m4 = typeof (GridData.Rows[i].m4) != 'number' ? 0 : GridData.Rows[i].m4;
                var m5 = typeof (GridData.Rows[i].m5) != 'number' ? 0 : GridData.Rows[i].m5;
                var m6 = typeof (GridData.Rows[i].m6) != 'number' ? 0 : GridData.Rows[i].m6;
                var m7 = typeof (GridData.Rows[i].m7) != 'number' ? 0 : GridData.Rows[i].m7;
                var m8 = typeof (GridData.Rows[i].m8) != 'number' ? 0 : GridData.Rows[i].m8;
                var m9 = typeof (GridData.Rows[i].m9) != 'number' ? 0 : GridData.Rows[i].m9;
                var m10 = typeof (GridData.Rows[i].m10) != 'number' ? 0 : GridData.Rows[i].m10;
                var m11 = typeof (GridData.Rows[i].m11) != 'number' ? 0 : GridData.Rows[i].m11;
                var m12 = typeof (GridData.Rows[i].m12) != 'number' ? 0 : GridData.Rows[i].m12;
                m1 = (m1 == 0 ? 0.001 : m1); m2 = (m2 == 0 ? 0.001 : m2); m3 = (m3 == 0 ? 0.001 : m3); m4 = (m4 == 0 ? 0.001 : m4); m5 = (m5 == 0 ? 0.001 : m5); m6 = (m6 == 0 ? 0.001 : m6);
                m7 = (m7 == 0 ? 0.001 : m7); m8 = (m8 == 0 ? 0.001 : m8); m9 = (m9 == 0 ? 0.001 : m9); m10 = (m10 == 0 ? 0.001 : m10); m11 = (m11 == 0 ? 0.001 : m11); m12 = (m12 == 0 ? 0.001 : m12);
                xmltext += "   <dataset seriesName='" + flowtype + "' color='" + color[i] + "'><set value='" + m1 + "' /><set value='" + m2 + "' /><set value='" + m3 + "' /><set value='" + m4 + "' /><set value='" + m5 + "' /><set value='" + m6 + "' /><set value='" + m7 + "' /><set value='" + m8 + "' /><set value='" + m9 + "' /><set value='" + m10 + "' /><set value='" + m11 + "' /><set value='" + m12 + "' /></dataset>";
                mc1 += m1; mc2 += m2; mc3 += m3; mc4 += m4; mc5 += m5; mc6 += m6; mc7 += m7; mc8 += m8; mc9 += m9; mc10 += m10; mc11 += m11; mc12 += m12;
            }
            xmltext += "   <dataset seriesName='总计' color='ff00ff' parentYAxis='S'><set value='" + mc1 + "' /><set value='" + mc2 + "' /><set value='" + mc3 + "' /><set value='" + mc4 + "' /><set value='" + mc5 + "' /><set value='" + mc6 + "' /><set value='" + mc7 + "' /><set value='" + mc8 + "' /><set value='" + mc9 + "' /><set value='" + mc10 + "' /><set value='" + mc11 + "' /><set value='" + mc12 + "' /></dataset>";

            xmltext += "</graph>"
            chart_myNext.setDataXML(xmltext);
            chart_myNext.render("c_report")
        }
        //function test() {
        //    alert(JSON.stringify(manager.getPanels()));
        //}
    </script>
    <style type="text/css">
        .l-column-place { width: 99%; }
        .l-panel-place { width: 99%; }
        .l-portal .l-column .l-panel { margin-bottom: 0; }
    </style>
</head>
<body style="padding: 5px">

    <div style="width: 100%;" id="portalMain">
    </div>
    <%--<input type="button" value="test" onclick="test()" />--%>
</body>
</html>
