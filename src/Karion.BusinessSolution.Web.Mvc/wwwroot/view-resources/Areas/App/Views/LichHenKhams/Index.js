(function () {
    $(function () {
        function isEmptyOrSpaces(str) {
            return str === null || str.match(/^ *$/) !== null;
        }

        $("#testAPI").click(function () {
            abp.services.app.chiTietThanhToans.mBBankGetTransactionHistory(1);
        });
        
        var _BoPhanTiep = {
            "1": "Bác sĩ tiếp quản",
            "2": "Thu ngân tiếp quản",
            "3": "Thực hiện thanh toán",
            "4": "Hoàn tất thanh toán"
        };

        var _NghiepVuMoi = {
            "1": "Thu ngân tiếp quản",
            "3": "Nhập Tổng chi phí",
        };

        var _flag = (parseInt)($('#FlagFilterId').val());

        var _$lichHenKhamsTable = $('#LichHenKhamsTable');
        var _lichHenKhamsService = abp.services.app.lichHenKhams;
        var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.LichHenKham';

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.LichHenKhams.Create'),
            edit: abp.auth.hasPermission('Pages.LichHenKhams.Edit'),
            'delete': abp.auth.hasPermission('Pages.LichHenKhams.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LichHenKhams/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LichHenKhams/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditLichHenKhamModal',
            modalSize: 'modal-xl modal-dialog-scrollable'
        });
        var _faceDetectsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LichHenKhams/FacesDetect',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LichHenKhams/_faceDetectsModal.js',
            modalClass: 'FaceDetectsModalModal'
        });
        var _viewLichHenKhamModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LichHenKhams/ViewlichHenKhamModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LichHenKhams/_ViewLichHenKhamModal.js',
            modalClass: 'ViewLichHenKhamModal',
            modalSize: 'modal-xl modal-dialog-scrollable'
        });

        var _dichVuChiTietModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LichHenKhams/DanhSachDichVuModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LichHenKhams/_ListDichVuModal.js',
            modalClass: 'ListDichVuKhamModal'
        });

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();

        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$lichHenKhamsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _lichHenKhamsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#LichHenKhamsTableFilter').val(),
                        minNgayHenKhamFilter: getDateFilter($('#MinNgayHenKhamFilterId')),
                        maxNgayHenKhamFilter: getDateFilter($('#MaxNgayHenKhamFilterId')),
                        moTaTrieuChungFilter: $('#MoTaTrieuChungFilterId').val(),
                        isCoBHYTFilter: $('#IsCoBHYTFilterId').val(),
                        soTheBHYTFilter: $('#SoTheBHYTFilterId').val(),
                        noiDangKyKCBDauTienFilter: $('#NoiDangKyKCBDauTienFilterId').val(),
                        minBHYTValidDateFilter: getDateFilter($('#MinBHYTValidDateFilterId')),
                        maxBHYTValidDateFilter: getDateFilter($('#MaxBHYTValidDateFilterId')),
                        minPhuongThucThanhToanFilter: $('#MinPhuongThucThanhToanFilterId').val(),
                        maxPhuongThucThanhToanFilter: $('#MaxPhuongThucThanhToanFilterId').val(),
                        isDaKhamFilter: $('#IsDaKhamFilterId').val(),
                        isDaThanhToanFilter: $('#IsDaThanhToanFilterId').val(),
                        minTimeHoanThanhKhamFilter: getDateFilter($('#MinTimeHoanThanhKhamFilterId')),
                        maxTimeHoanThanhKhamFilter: getDateFilter($('#MaxTimeHoanThanhKhamFilterId')),
                        minTimeHoanThanhThanhToanFilter: getDateFilter($('#MinTimeHoanThanhThanhToanFilterId')),
                        maxTimeHoanThanhThanhToanFilter: getDateFilter($('#MaxTimeHoanThanhThanhToanFilterId')),
                        userNameFilter: $('#UserNameFilterId').val(),
                        userName2Filter: $('#UserName2FilterId').val(),
                        nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val(),
                        nguoiThanHoVaTenFilter: $('#NguoiThanHoVaTenFilterId').val(),
                        //dichVuTenFilter: $('#DichVuTenFilterId').val(),
                        chuyenKhoaTenFilter: $('#ChuyenKhoaTenFilterId').val(),
                        flagFilter: _flag
                    };
                }
            },
            columnDefs: [
                {
                    scrollX: true,
                    scrollY: '50vh',
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('ChiTietThanhToan'),
                                visible: function () {
                                    return _flag == 3
                                },
                                action: function (data) {
                                    _viewLichHenKhamModal.open({id: data.record.lichHenKham.id});
                                }
                            },
                            {
                                text: app.localize('ChinhSuaLichHenKham'),
                                visible: function () {
                                    return _permissions.edit && _flag == 1;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.lichHenKham.id});
                                }
                            },
                            {
                                text: app.localize('History'),
                                visible: function () {
                                    return entityHistoryIsEnabled();
                                },
                                action: function (data) {
                                    _entityTypeHistoryModal.open({
                                        entityTypeFullName: _entityTypeFullName,
                                        entityId: data.record.lichHenKham.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteLichHenKham(data.record.lichHenKham);
                                }
                            },
                            {
                                text: app.localize('HoanTatThanhToan'),
                                visible: function (data) {
                                    return _flag == 3 && data.record.lichHenKham.isDaThanhToan == false
                                },
                                action: function (data) {
                                    hoanTatThanhToan(data.record.lichHenKham);
                                }
                            },
                            {
                                text: app.localize('ThanhToanVienPhi'),
                                visible: function (data) {
                                    return _flag == 3 && data.record.lichHenKham.isDaThanhToan == false
                                },
                                action: function (data) {
                                    console.log(data);
                                    thanhToanVienPhi(data.record.lichHenKham);
                                }
                            }
                            ]
                    }
                },
                {
                    targets: 1,
                    data: "lichHenKham.tongChiPhi",
                    name: "tongChiPhi",
                    visible: _flag == 3 ? true : false,
                    render: function (data, meta, row) {
                        return addCommas(data);
                    }
                },
                {
                    targets: 2,
                    data: "lichHenKham.tongTienThanhToan",
                    name: "tongTienThanhToan",
                    visible: _flag == 3 ? true : false,
                    render: function (data, meta, row) {
                        return addCommas(data);
                    }                     
                },
                {
                    targets: 3,
                    data: "lichHenKham",
                    name: "tamUng",
                    visible: _flag == 3 ? true : false,
                    render: function (data, meta, row) {
                        
                        return addCommas(data.tongTienThanhToan > 0 ? (data.tongChiPhi-data.tongTienThanhToan) : 0);
                    }
                        
                },
                {
                    targets: 4,
                    //render: function (displayName, type, row, meta) {
                    //    if (row.lichHenKham.isDaThanhToan) {
                    //        if (row.lichHenKham.isDaKham) {
                    //            return '<label class="badge badge-success">Đã hoàn tất<span class="badge badge-light"></span></label>';
                    //        } else {
                    //            return '<button class="btn btn-secondary btn-sm btn-brand chuyen-tiep-benh-nhan" data-dakham="' + row.lichHenKham.isDaKham + '" data-index="' + row.lichHenKham.id + '">' + _BoPhanTiep["4"] + '</button>'
                    //        }
                    //    } else {
                    //        return '<button class="btn btn-secondary btn-sm btn-brand chuyen-tiep-benh-nhan" data-index="' + row.lichHenKham.id + '">' + _BoPhanTiep[_flag] + '</button>'
                    //    }
                    //},
                    render: function (displayName, type, row, meta) {
                        if (row.lichHenKham.isDaThanhToan) {
                            return '<label class="badge badge-success">Đã hoàn tất<span class="badge badge-light"></span></label>';
                        } else {
                            let btnThanhToan = '<button class="btn btn-secondary btn-sm btn-brand chuyen-tiep-benh-nhan" data-dakham="' + row.lichHenKham.isDaThanhToan + '" data-index="' + row.lichHenKham.id + '"><i class="fas fa-dollar-sign"></i>' + _NghiepVuMoi[_flag] + '</button>';
                            let btnQR = '';
                            // if (row.lichHenKham.tongChiPhi > 0)
                            //     btnQR += '<button class="btn btn-primary btn-sm btn-brand tao-qrthanhtoan" data-index="' + row.lichHenKham.id + '" title="QR Thanh Toán"><i class="fas fa-qrcode"></i> QR</button>';
                            // return btnThanhToan+btnQR
                             return btnThanhToan
                        }
                    }
                },
                {
                    targets: 5,
                    data: "lichHenKham.ngayHenKham",
                    name: "ngayHenKham",
                    render: function (ngayHenKham) {
                        if (ngayHenKham) {
                            return moment(ngayHenKham).format('L');
                        }
                        return "";
                    }
                },
                {
                    targets: 6,
                    data: "nguoiBenh.UserName",
                    name: "nguoiBenhFk.userName",
                    render: function (displayName, type, row, meta) {
                        if (row.isNhanDien) {
                            return row.nguoiBenhUserName + " <span class='badge badge-success'> <i class='fa fa-check-circle'></i> Nhận diện Ai</span>";
                        }
                        return row.nguoiBenhUserName;
                    }
                },
                {
                    targets: 7,
                    render: function (displayName, type, row, meta) {
                        if (_flag == 1) {
                            return '';
                        } else {
                            let selectedDichVu = [];
                            if (row.lichHenKham.chiDinhDichVuSerialize != "") {
                                var serialized = JSON.parse(row.lichHenKham.chiDinhDichVuSerialize);
                                if (serialized != null) {
                                    for (var i = 0; i < serialized.length; i++) {
                                        selectedDichVu[i] = {};
                                        selectedDichVu[i].id = serialized[i].id;
                                        selectedDichVu[i].gia = serialized[i].gia;
                                    }
                                }
                            }
                            return "<button class='btn btn-primary btn-sm btn-brand dich-vu-chi-tiet' selected-dichvu='" + JSON.stringify(selectedDichVu) + "' row-index='" + row.lichHenKham.id + "' chuyenKhoa-index='" + row.lichHenKham.chuyenKhoaId + "'>Các dịch vụ khám</button>"
                        }

                    },
                    visible: false
                },
                {
                    targets: 8,
                    data: "userName",
                    name: "bacSiFk.name"
                },
                {
                    targets: 9,
                    data: "chuyenKhoaTen",
                    name: "chuyenKhoaFk.ten"
                },
                {
                    targets: 10,
                    data: "lichHenKham.isDaKham",
                    name: "isDaKham",
                    render: function (isDaKham) {
                        if (isDaKham) {
                            return '<label class="badge badge-success" style="">Đã khám <span class="badge badge-light"></span></label>';
                        }
                        return '<label class="badge badge-danger" style="">Chưa khám</label>';
                    },
                    visible: false
                },
                {
                    targets: 11,
                    data: "lichHenKham",
                    name: "isDaThanhToan",
                    render: function (data) {
                        let daTT = '<label class="badge badge-success" style="">Đã TT <span class="badge badge-light"></span></label>';
                        let chuaTT = '<label class="badge badge-danger" style="">Chưa TT</label>';
                        let tamUng = '<label class="badge badge-warning" style="">Tạm ứng</label>';
                        if (data.isDaThanhToan) {
                            if(data.isTamUng){
                                return daTT+tamUng;
                            }
                            return daTT
                        }
                        else {
                            if(data.isTamUng){
                                return chuaTT+tamUng;
                            }
                            return chuaTT
                        }
                    }

                },
                {
                    targets: 12,
                    data: "userName2",
                    name: "thuNganFk.name",
                    visible: false
                },
                {
                    targets: 13,
                    data: "lichHenKham.moTaTrieuChung",
                    name: "moTaTrieuChung"
                },
                {
                    targets: 14,
                    data: "lichHenKham.isCoBHYT",
                    name: "isCoBHYT",
                    render: function (isCoBHYT) {
                        if (isCoBHYT) {
                            //return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
                            return '<label class="badge badge-success" style="">Có BHYT</label>';
                        }
                        //return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                        return '<label class="badge badge-dark" style="">Không BHYT</label>';
                    }

                },
                {
                    targets: 15,
                    data: "lichHenKham.soTheBHYT",
                    name: "soTheBHYT"
                },
                {
                    targets: 16,
                    data: "lichHenKham.noiDangKyKCBDauTien",
                    name: "noiDangKyKCBDauTien"
                },
                {
                    targets: 17,
                    data: "lichHenKham.bhytValidDate",
                    name: "bhytValidDate",
                    render: function (bhytValidDate) {
                        if (bhytValidDate) {
                            return moment(bhytValidDate).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 18,
                    data: "lichHenKham.phuongThucThanhToan",
                    name: "phuongThucThanhToan",
                    render: function (data, type, row, meta) {
                        if(row.lichHenKham.phuongThucThanhToan == 1){
                            return "Tiền mặt";
                        }
                        if(row.lichHenKham.phuongThucThanhToan == 2){
                            return "Chuyển khoản";
                        }
                    }
                },


                {
                    targets: 19,
                    data: "lichHenKham.timeHoanThanhKham",
                    name: "timeHoanThanhKham",
                    render: function (timeHoanThanhKham) {
                        if (timeHoanThanhKham) {
                            return moment(timeHoanThanhKham).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 20,
                    data: "lichHenKham.timeHoanThanhThanhToan",
                    name: "timeHoanThanhThanhToan",
                    render: function (timeHoanThanhThanhToan) {
                        if (timeHoanThanhThanhToan) {
                            return moment(timeHoanThanhThanhToan).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 21,
                    data: "nguoiThanHoVaTen",
                    name: "nguoiThanFk.hoVaTen"
                }
            ]
        });


        $(document).ready(function () {
            $(document).off("click", "#locFaceDetect");
            $(document).on("click", "#locFaceDetect", function () {
                _faceDetectsModal.open();
            })
        });

        function getLichHenKhams() {
            dataTable.ajax.reload();
        }

        function deleteLichHenKham(lichHenKham) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _lichHenKhamsService.delete({
                            id: lichHenKham.id
                        }).done(function () {
                            getLichHenKhams(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function hoanTatThanhToan(lichHenKham) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        console.log(lichHenKham)

                        _lichHenKhamsService.hoanTatThanhToan(lichHenKham.id).done(function (result) {
                            if(result == 200)
                                getLichHenKhams(true);
                            else
                                sweetAlert({
                                    text: app.localize("CoLoiXayRa")
                                })
                        });
                    }
                }
            );
        }
        function thanhToanVienPhi(lichHenKham) {
            sweetAlert({
                text: app.localize("NhapSoTienThanhToan"),
                content: {
                    element: "input",
                    attributes: {
                        placeholder: "Nhập số tiền thanh toán",
                        type: "number",
                    },
                },
                button: {
                    Text: app.localize("OK"),
                    closeModal: true
                }
            })
                .then(soTienThanhToan => {
                    if(soTienThanhToan != null){
                        _lichHenKhamsService.thanhToanVienPhi({
                            id : lichHenKham.id,
                            soTienThanhToan: soTienThanhToan
                        }).then(result => {
                            if (result == 200){
                                getLichHenKhams();
                            }
                            else {
                                swal(app.localize("Warning"), app.localize("CoLoiXayRa"), "warning"); 
                            }
                        })
                            
                    }
                })
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewLichHenKhamButton').click(function () {    
            _createOrEditModal.open();
        });
        $('#ktraThanhToanSignalr').click(function () {
           _lichHenKhamsService.testSignal();
        });
        
        

        var regNumber = /^(0|[1-9][0-9]*)$/

        // nghiệp vụ cũ
        //$(document).on('click', '.chuyen-tiep-benh-nhan', function () {
        //    let id = $(this).attr("data-index")
        //    let isDaKham = $(this).attr("data-dakham")
        //    if (_flag == 3 && isDaKham == 'false') {
        //        sweetAlert({
        //            text: app.localize("NhapTienThua"),
        //            content: "input",
        //            button: {
        //                Text: app.localize("HoanTien"),
        //                closeModal: true
        //            }
        //        }).then(tienThua => {
        //            if (!regNumber.test(tienThua))
        //                swal(app.localize("Warning"), app.localize("SoTienKhongHopLe"), "warning");
        //            else {
        //                _lichHenKhamsService.capNhatTienThua({
        //                    id: id,
        //                    tienThua: tienThua
        //                }).then(result => {
        //                    if (result == 200) {
        //                        getLichHenKhams();
        //                    } else {
        //                        swal(app.localize("Warning"), app.localize("CoLoiXayRa"), "warning");
        //                    }
        //                })
        //            }
        //        });
        //    } else {
        //        abp.message.confirm(
        //            '',
        //            app.localize('AreYouSure'),
        //            function (isConfirmed) {
        //                if (isConfirmed) {
        //                    _lichHenKhamsService.chuyenBenhNhan({
        //                        id: id,
        //                        flag: _flag
        //                    }).done(function (res) {
        //                        if (res === 200) {
        //                            getLichHenKhams()
        //                        } else {
        //                            alert(app.localize('CoLoiXayRa'));
        //                        }
        //                    })
        //                }
        //            }
        //        );
        //    }
        //});

        // nghiệp vụ rút gọn
        $(document).on('click', '.chuyen-tiep-benh-nhan', function () {
            let $self = $(this);
            let id = $self.attr("data-index");
            let isDaThanhToan = $(this).attr("data-thanhToan")

            if (_flag == 3) {
                if (isDaThanhToan == 'false' || isDaThanhToan == undefined) {
                    //nhap tien
                    sweetAlert({
                        text: app.localize("NhapSoTienThanhToan"),
                        content: {
                            element: "input",
                            attributes: {
                                placeholder: "Nhập số tiền thanh toán.",
                                type: "number",
                            },
                        },
                        button: {
                            Text: app.localize("OK"),
                            closeModal: true
                        }
                    }).then(tongTien => {   
                        if (tongTien != null) {
                            _lichHenKhamsService.chuyenBenhNhan({
                                id: id,
                                flag: _flag,
                                tongChiPhi: tongTien
                            }).then(result => {
                                if (result == 200) {
                                    getLichHenKhams();
                                } else {
                                    swal(app.localize("Warning"), app.localize("CoLoiXayRa"), "warning");
                                }
                            })
                        }
                    })
                } 
            } else {
                abp.message.confirm(
                    '',
                    app.localize('AreYouSure'),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            _lichHenKhamsService.chuyenBenhNhan({
                                id: id,
                                flag: _flag
                            }).done(function (res) {
                                if (res === 200) {
                                    getLichHenKhams()
                                } else {
                                    alert(app.localize('CoLoiXayRa'));
                                }
                            })
                        }
                    }
                );
            }
        });

        var QRContainer = document.createElement("div")
        QRContainer.className = "text-center"
        QRContainer.id = "qrcontainer"

        var _qroptions = {
            text: "",
            width: 256,
            height: 256,
            logo: "/Common/Images/techber.png"
        }

        $(document).on('click', '.tao-qrthanhtoan', function () {
            var $self = $(this);
            let id = $self.attr('data-index')

            _lichHenKhamsService.generator(id).done(function (result) {
                QRContainer.textContent  = "";
                if (!isEmptyOrSpaces(result)) {
                    _qroptions.text = result
                    new QRCode(QRContainer, _qroptions);
                    sweetAlert({
                        content: QRContainer,
                        button: {
                            Text: app.localize("OK"),
                            closeModal: true
                        }
                    })
                } else {
                    sweetAlert({
                        text: app.localize("CoLoiXayRa")
                    })
                }

            })
        })

        $(document).on('click', '.dich-vu-chi-tiet', function () {
            let $self = $(this);
            let chuyenKhoaId = $self.attr("chuyenKhoa-index");
            let id = $self.attr('row-index');
            let serializedDichVu = $self.attr('selected-dichvu')
            _dichVuChiTietModal.open({
                id: id,
                chuyenKhoaId: chuyenKhoaId,
                serializedDichVu: serializedDichVu,
                flag: _flag
            }, function (thongtin) {
                $self.attr('selected-dichvu', thongtin)
            });
        })

        $('#ExportToExcelButton').click(function () {
            _lichHenKhamsService
                .getLichHenKhamsToExcel({
                    filter: $('#LichHenKhamsTableFilter').val(),
                    minNgayHenKhamFilter: getDateFilter($('#MinNgayHenKhamFilterId')),
                    maxNgayHenKhamFilter: getDateFilter($('#MaxNgayHenKhamFilterId')),
                    moTaTrieuChungFilter: $('#MoTaTrieuChungFilterId').val(),
                    isCoBHYTFilter: $('#IsCoBHYTFilterId').val(),
                    soTheBHYTFilter: $('#SoTheBHYTFilterId').val(),
                    noiDangKyKCBDauTienFilter: $('#NoiDangKyKCBDauTienFilterId').val(),
                    minBHYTValidDateFilter: getDateFilter($('#MinBHYTValidDateFilterId')),
                    maxBHYTValidDateFilter: getDateFilter($('#MaxBHYTValidDateFilterId')),
                    minPhuongThucThanhToanFilter: $('#MinPhuongThucThanhToanFilterId').val(),
                    maxPhuongThucThanhToanFilter: $('#MaxPhuongThucThanhToanFilterId').val(),
                    isDaKhamFilter: $('#IsDaKhamFilterId').val(),
                    isDaThanhToanFilter: $('#IsDaThanhToanFilterId').val(),
                    minTimeHoanThanhKhamFilter: getDateFilter($('#MinTimeHoanThanhKhamFilterId')),
                    maxTimeHoanThanhKhamFilter: getDateFilter($('#MaxTimeHoanThanhKhamFilterId')),
                    minTimeHoanThanhThanhToanFilter: getDateFilter($('#MinTimeHoanThanhThanhToanFilterId')),
                    maxTimeHoanThanhThanhToanFilter: getDateFilter($('#MaxTimeHoanThanhThanhToanFilterId')),
                    userNameFilter: $('#UserNameFilterId').val(),
                    userName2Filter: $('#UserName2FilterId').val(),
                    nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val(),
                    nguoiThanHoVaTenFilter: $('#NguoiThanHoVaTenFilterId').val(),
                    dichVuTenFilter: $('#DichVuTenFilterId').val(),
                    chuyenKhoaTenFilter: $('#ChuyenKhoaTenFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditLichHenKhamModalSaved', function () {
            getLichHenKhams();
        });

        $('#GetLichHenKhamsButton').click(function (e) {
            e.preventDefault();
            getLichHenKhams();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getLichHenKhams();
            }
        });
    });
})();