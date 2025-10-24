//
// var app = app || {};
// (function () {
//
//     //Ktra xem SignalR đã xác định hay chưa
//     if (!signalR) {
//         return;
//     }
//
//     //Tạo namespaces
//     app.signalr = app.signalr || {};
//     app.signalr.ttkcHubs = app.signalr.ttkcHubs || {};
//
//     var thanhToanKhongChamHub = null;
//
//     //Cấu hình để kết nối
//     function configureConnectionTtkc(connection) {
//         //Cài common hub
//         app.signalr.ttkcHubs.thongBao = connection;
//         thanhToanKhongChamHub = connection;
//         console.log(21,'thuan');
//         let reconnectTime = 5000; //5 giây
//         let tries = 1;
//         let maxTries = 8;
//         function tryReconnectTtkc() {
//             if (tries > maxTries) {
//                 return;
//             } else {
//                 connection.start()
//                     .then(function () {
//                         reconnectTime = 5000;
//                         tries = 1;
//                         console.log('Đã tái kết nối tới SigalR server');
//                     }).catch(function () {
//                         tries += 1;
//                         reconnectTime *= 2;
//                         setTimeout(function () {
//                             tryReconnectTtkc();
//                         }, reconnectTime);
//                     })
//             }
//         }
//
//         //Tái kết nối nếu hub bị ngắt kết nối
//         connection.onclose(function (e) {
//             console.log(46,'thuan');
//             if (e) {
//                 abp.log.debug('Kết nối ThanhToanKhongChamHub bị đóng bởi lỗi: ' + e);
//             }
//             else {
//                 abp.log.debug("ThanhToanKhongChamHub đã ngắt kết nối");
//             }
//
//             if (!abp.signalr.autoConnectTtkc) {
//                 return;
//             }
//             tryReconnectTtkc();
//         });
//
//         //Hàm đăng ký xử lí thông báo....
//         registerThanhToanKhongChamHubEvents(connection);
//     }
//
//     //Kết nối tới máy chủ
//     abp.signalr.connectTtkc = function () {
//         //Bắt đầu kết nối
//         startConnectionTtkc(abp.appPath + 'signalr-ttkc', configureConnectionTtkc)
//             .then(function (connection) {
//                 console.log(connection)
//                 abp.log.debug('Đã kết nối tới SignalR server');
//                 //abp.event.trigger('app.ThanhToanKhongChamHub.connected');
//
//                 //Gọi phương thức đăng ký trên hub
//                 //connection.invoke('register').then(function () {
//                 //    abp.log.debug('Registered to the SignalR server!')
//                 //});
//             }).catch(function (error) {
//                 abp.log.debug(error.message);
//             });
//     };
//
//     function startConnectionTtkc(url, configureConnection) {
//         if (abp.signalr.remoteServiceBaseUrl) {
//             url = abp.signalr.remoteServiceBaseUrl + url;
//         }
//
//         if (abp.signalr.qs) {
//             url += '?' + abp.signalr.qs;
//         }
//         console.log(89, url);
//         return function start(transport) {
//
//             var connection = new signalR.HubConnectionBuilder()
//                 .withUrl(url, transport)
//                 .build();
//
//             if (configureConnection && typeof configureConnection === 'function') {
//                 configureConnectionTtkc(connection);
//             }
//
//             return connection.start()
//                 .then(function () {
//                     return connection;
//                 })
//                 .catch(function (error) {
//                     abp.log.debug('Cannot start the connection using ' + signalR.HttpTransportType[transport] + ' transport. ' + error.message);
//                     if (transport !== signalR.HttpTransportType.LongPolling) {
//                         return start(transport + 1);
//                     }
//                     return Promise.reject(error);
//                 });
//         }(signalR.HttpTransportType.WebSockets)
//     }
//
//     abp.signalr.startConnectionTtkc = startConnectionTtkc;
//
//     if (abp.signalr.autoConnectTtkc === undefined) {
//         abp.signalr.autoConnectTtkc = true;
//     }
//
//     if (abp.signalr.autoConnectTtkc) {
//         abp.signalr.connectTtkc();
//     }
//
//     function registerThanhToanKhongChamHubEvents(connection) {
//         console.log(125,'thuan');
//         connection.on('test', function (message) {
//             console.log(128, "mess: " + message )
//         });
//     }
//
//     //app.thongBao.sendMessage = function (messageData, callback) {
//     //    if (thanhToanKhongChamHub.connection.connectionState !== signalR.HubConnectionState.Connected) {
//     //        callback && callback();
//     //        abp.notify.warn(app.localize('ChatIsNotConnectedWarning'));
//     //        return;
//     //    }
//
//     //    thanhToanKhongChamHub.invoke('sendMessage', messageData).then(function (result) {
//     //        if (result) {
//     //            abp.notify.warn(result);
//     //        }
//
//     //        callback && callback();
//     //    });
//     //};
// })();