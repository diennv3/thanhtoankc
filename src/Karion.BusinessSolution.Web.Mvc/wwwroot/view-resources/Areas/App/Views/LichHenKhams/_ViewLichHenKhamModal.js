(function () {
    app.modals.ViewLichHenKhamModal = function () {
        function isEmptyOrSpaces(str) {
            return str === null || str.match(/^ *$/) !== null;
        }
        var _lichHenKhamsService = abp.services.app.lichHenKhams;
        var _chiTietThanhToanService = abp.services.app.chiTietThanhToans;
        
        var QRContainer = document.createElement("div");
        QRContainer.className = "text-center";
        QRContainer.id = "qrcontainer";

        var _qroptions = {
            text: "",
            width: 256,
            height: 256,
            logo: "/Common/Images/techber.png"
        };
       
        $(document).off('click', '.tao-qrthanhtoan');
        $(document).on('click', '.tao-qrthanhtoan', function () {
            var $self = $(this);
            let lichHenKhamId = $self.attr('data-lhk');
            let chiTietThanhToanId = $self.attr('data-cttt');
            _lichHenKhamsService.generator(lichHenKhamId, chiTietThanhToanId
            ).done(function (result) {
                QRContainer.textContent  = "";
                if (!isEmptyOrSpaces(result)) {
                    _qroptions.text = result;
                    new QRCode(QRContainer, _qroptions);
                    sweetAlert({
                        content: QRContainer,
                        button: {
                            text:"OK",
                            closeModal: true
                        }
                    })
                } else {
                    sweetAlert({
                        text: "CoLoiXayRa"
                    })
                }

            })
        });

        $('#kiemTraThanhToanNganHang').click(function () {
            var lichHenKhamId = $(this).attr('lichhenkhamid');
            _chiTietThanhToanService.kiemTraThanhToanNganHang({
                lichHenKhamId : lichHenKhamId
            });
        });

        //Bắt đầu sử dụng Signal
        var app = app || {};
        (function () {

            //Ktra xem SignalR đã xác định hay chưa
            if (!signalR) {
                return;
            }

            //Tạo namespaces
            app.signalr = app.signalr || {};
            app.signalr.ttkcHubs = app.signalr.ttkcHubs || {};

            var thanhToanKhongChamHub = null;

            //Cấu hình để kết nối
            function configureConnectionTtkc(connection) {
                //Cài common hub
                app.signalr.ttkcHubs.thongBao = connection;
                thanhToanKhongChamHub = connection;
                let reconnectTime = 5000; //5 giây
                let tries = 1;
                let maxTries = 8;
                function tryReconnectTtkc() {
                    if (tries > maxTries) {
                        return;
                    } else {
                        connection.start()
                            .then(function () {
                                reconnectTime = 5000;
                                tries = 1;
                                console.log('Đã tái kết nối tới SigalR server');
                            }).catch(function () {
                            tries += 1;
                            reconnectTime *= 2;
                            setTimeout(function () {
                                tryReconnectTtkc();
                            }, reconnectTime);
                        })
                    }
                }

                //Tái kết nối nếu hub bị ngắt kết nối
                connection.onclose(function (e) {
                    if (e) {
                        abp.log.debug('Kết nối ThanhToanKhongChamHub bị đóng bởi lỗi: ' + e);
                    }
                    else {
                        abp.log.debug("ThanhToanKhongChamHub đã ngắt kết nối");
                    }

                    if (!abp.signalr.autoConnectTtkc) {
                        return;
                    }
                    tryReconnectTtkc();
                });

                //Hàm đăng ký xử lí thông báo....
                registerThanhToanKhongChamHubEvents(connection);
            }

            //Kết nối tới máy chủ
            abp.signalr.connectTtkc = function () {
                //Bắt đầu kết nối
                startConnectionTtkc(abp.appPath + 'signalr-ttkc', configureConnectionTtkc)
                    .then(function (connection) {
                        abp.log.debug('Đã kết nối tới SignalR ThanhToanKhongCham');
                        abp.log.debug(connection);
                    }).catch(function (error) {
                    abp.log.debug(error.message);
                });
            };

            function startConnectionTtkc(url, configureConnection) {
                if (abp.signalr.remoteServiceBaseUrl) {
                    url = abp.signalr.remoteServiceBaseUrl + url;
                }

                if (abp.signalr.qs) {
                    url += '?' + abp.signalr.qs;
                }
                return function start(transport) {

                    var connection = new signalR.HubConnectionBuilder()
                        .withUrl(url, transport)
                        .build();

                    if (configureConnection && typeof configureConnection === 'function') {
                        configureConnectionTtkc(connection);
                    }

                    return connection.start()
                        .then(function () {
                            return connection;
                        })
                        .catch(function (error) {
                            abp.log.debug('Cannot start the connection using ' + signalR.HttpTransportType[transport] + ' transport. ' + error.message);
                            if (transport !== signalR.HttpTransportType.LongPolling) {
                                return start(transport + 1);
                            }
                            return Promise.reject(error);
                        });
                }(signalR.HttpTransportType.WebSockets)
            }

            abp.signalr.startConnectionTtkc = startConnectionTtkc;

            if (abp.signalr.autoConnectTtkc === undefined) {
                abp.signalr.autoConnectTtkc = true;
            }

            if (abp.signalr.autoConnectTtkc) {
                 abp.signalr.connectTtkc();
            }
            
            function registerThanhToanKhongChamHubEvents(connection) {
                connection.on('thongbao', function (response) {
                    console.log(response);
                    if(response.isUpdateTrangThaiThanhToan){
                        $("#"+response.chiTietThanhToanId+"")
                            .replaceWith('<button class="btn btn-success btn-sm btn-brand" disabled>Đã Thanh toán</button>');
                        abp.notify.success(response.message, 'Thông báo');
                    }
                    else {
                        abp.notify.warn(response.message, 'Thông báo');
                    }
                });
            }
            
        })();
    };
})(jQuery); 