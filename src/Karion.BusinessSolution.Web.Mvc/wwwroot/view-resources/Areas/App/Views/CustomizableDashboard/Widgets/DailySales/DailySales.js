$(function () {
    var _tenantDashboardService = abp.services.app.tenantDashboard;
    var _widgetBase = app.widgetBase.create();
    var _$Container = $('.DailySalesContainer');
    var _baocaoService = abp.services.app.lichHenKhams;

    //== Daily Sales chart.
    //** Based on Chartjs plugin - http://www.chartjs.org/
    var initDailySales = function (data) {
        var dayLabels = [];
        for (var day = 1; day <= data.length; day++) {
            dayLabels.push("Ngày " + day);
        }

        var chartData = {
            labels: dayLabels,
            datasets: [
                {
                    //label: 'Dataset 1',
                    backgroundColor: '#34bfa3',
                    data: data
                }
            ]
        };

        for (var i = 0; i < _$Container.length; i++) {
            var chartContainer = $(_$Container[i]).find('#m_chart_daily_sales');

            new Chart(chartContainer, {
                type: 'bar',
                data: chartData,
                options: {
                    title: {
                        display: true,
                        text: "Báo cáo doanh thu hàng tháng"
                    },
                    tooltips: {
                        intersect: false,
                        mode: 'nearest',
                        xPadding: 10,
                        yPadding: 10,
                        caretPadding: 10
                    },
                    legend: {
                        display: false
                    },
                    responsive: true,
                    maintainAspectRatio: false,
                    barRadius: 4,
                    scales: {
                        xAxes: [{
                            //display: true,
                            //gridLines: true,
                            stacked: true
                        }],
                        yAxes: [{
                            //display: true,
                            stacked: true,
                            //gridLines: false
                        }]
                    },
                    layout: {
                        padding: {
                            left: 0,
                            right: 0,
                            top: 0,
                            bottom: 0
                        }
                    }
                }
            });
        }
    };

    //var getDailySales = function () {
    //    abp.ui.setBusy(_$Container);
    //    _tenantDashboardService
    //        .getDailySales()
    //        .done(function (result) {
    //            initDailySales(result.dailySales);
    //        }).always(function () {
    //            abp.ui.clearBusy(_$Container);
    //        });
    //};

    var getBaoCaoDailySales = function () {
        let now = new Date();
        let year = now.getFullYear();
        let month = now.getMonth() + 1;
        let daysInMonth = new Date(year, month, 0).getDate();
        let dataChart = [];

        abp.ui.setBusy(_$Container);
        _baocaoService.getBaoCao({
            thang: month,
            nam: year
        }).done(function (result) {
            console.log(result)
            for (let i = 0; i < daysInMonth; i++) {
                dataChart[i] = 0;
            }

            result.duLieuTho.forEach(function (item) {
                dataChart[new Date(item.date).getDate()-1] = item.doanhthu
            })

            initDailySales(dataChart);
        }).always(function () {
            abp.ui.clearBusy(_$Container);
        })
    }


    _widgetBase.runDelayed(getBaoCaoDailySales);

    $('#DashboardTabList a[data-toggle="pill"]').on('shown.bs.tab', function (e) {
        _widgetBase.runDelayed(getBaoCaoDailySales);
    });

    abp.event.on('app.dashboardFilters.DateRangePicker.OnDateChange', function (_selectedDates) {
        _widgetBase.runDelayed(getBaoCaoDailySales);
    });

    var chart = document.getElementById("m_chart_daily_sales");
    chart.height = 300;
});