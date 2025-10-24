(function ($) {
    app.modals.CreateOrEditLichHenKhamModal = function () {

        var _lichHenKhamsService = abp.services.app.lichHenKhams;

        var _modalManager;
        var _$lichHenKhamInformationForm = null;

        var _LichHenKhamuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LichHenKhams/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LichHenKhams/_LichHenKhamUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });
        var _LichHenKhamnguoiBenhLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LichHenKhams/NguoiBenhLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LichHenKhams/_LichHenKhamNguoiBenhLookupTableModal.js',
            modalClass: 'NguoiBenhLookupTableModal'
        });
        var _LichHenKhamnguoiThanLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LichHenKhams/NguoiThanLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LichHenKhams/_LichHenKhamNguoiThanLookupTableModal.js',
            modalClass: 'NguoiThanLookupTableModal'
        });
        var _LichHenKhamdichVuLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LichHenKhams/DichVuLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LichHenKhams/_LichHenKhamDichVuLookupTableModal.js',
            modalClass: 'DichVuLookupTableModal'
        });
        var _LichHenKhamchuyenKhoaLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LichHenKhams/ChuyenKhoaLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LichHenKhams/_LichHenKhamChuyenKhoaLookupTableModal.js',
            modalClass: 'ChuyenKhoaLookupTableModal'
        });
        
        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'DD/MM/YYYY'
            });

            _$lichHenKhamInformationForm = _modalManager.getModal().find('form[name=LichHenKhamInformationsForm]');
            _$lichHenKhamInformationForm.validate();
        };

        $('#OpenUserLookupTableButton').click(function () {

            var lichHenKham = _$lichHenKhamInformationForm.serializeFormToObject();

            _LichHenKhamuserLookupTableModal.open({
                id: lichHenKham.bacSiId,
                displayName: lichHenKham.userName
            }, function (data) {
                _$lichHenKhamInformationForm.find('input[name=userName]').val(data.displayName);
                _$lichHenKhamInformationForm.find('input[name=bacSiId]').val(data.id);
            });
        });
		
        $('#ClearUserNameButton').click(function () {
            _$lichHenKhamInformationForm.find('input[name=userName]').val('');
            _$lichHenKhamInformationForm.find('input[name=bacSiId]').val('');
        });

        $('#OpenUser2LookupTableButton').click(function () {

            var lichHenKham = _$lichHenKhamInformationForm.serializeFormToObject();

            _LichHenKhamuserLookupTableModal.open({
                id: lichHenKham.thuNganId,
                displayName: lichHenKham.userName2
            }, function (data) {
                _$lichHenKhamInformationForm.find('input[name=userName2]').val(data.displayName);
                _$lichHenKhamInformationForm.find('input[name=thuNganId]').val(data.id);
            });
        });

        $('#ClearUserName2Button').click(function () {
            _$lichHenKhamInformationForm.find('input[name=userName2]').val('');
            _$lichHenKhamInformationForm.find('input[name=thuNganId]').val('');
        });

        $('#OpenNguoiBenhLookupTableButton').click(function () {

            var lichHenKham = _$lichHenKhamInformationForm.serializeFormToObject();

            _LichHenKhamnguoiBenhLookupTableModal.open({
                id: lichHenKham.nguoiBenhId,
                displayName: lichHenKham.nguoiBenhUserName
            }, function (data) {
                _$lichHenKhamInformationForm.find('input[name=nguoiBenhUserName]').val(data.displayName);
                _$lichHenKhamInformationForm.find('input[name=nguoiBenhId]').val(data.id);
            });
        });

        $('#ClearNguoiBenhUserNameButton').click(function () {
            _$lichHenKhamInformationForm.find('input[name=nguoiBenhUserName]').val('');
            _$lichHenKhamInformationForm.find('input[name=nguoiBenhId]').val('');
        });

        $('#OpenNguoiThanLookupTableButton').click(function () {

            var lichHenKham = _$lichHenKhamInformationForm.serializeFormToObject();

            _LichHenKhamnguoiThanLookupTableModal.open({
                id: lichHenKham.nguoiThanId,
                displayName: lichHenKham.nguoiThanHoVaTen
            }, function (data) {
                _$lichHenKhamInformationForm.find('input[name=nguoiThanHoVaTen]').val(data.displayName);
                _$lichHenKhamInformationForm.find('input[name=nguoiThanId]').val(data.id);
            });
        });

        $('#ClearNguoiThanHoVaTenButton').click(function () {
            _$lichHenKhamInformationForm.find('input[name=nguoiThanHoVaTen]').val('');
            _$lichHenKhamInformationForm.find('input[name=nguoiThanId]').val('');
        });

        $('#OpenDichVuLookupTableButton').click(function () {

            var lichHenKham = _$lichHenKhamInformationForm.serializeFormToObject();

            _LichHenKhamdichVuLookupTableModal.open({
                id: lichHenKham.dichVuId,
                displayName: lichHenKham.dichVuTen
            }, function (data) {
                _$lichHenKhamInformationForm.find('input[name=dichVuTen]').val(data.displayName);
                _$lichHenKhamInformationForm.find('input[name=dichVuId]').val(data.id);
            });
        });

        $('#ClearDichVuTenButton').click(function () {
            _$lichHenKhamInformationForm.find('input[name=dichVuTen]').val('');
            _$lichHenKhamInformationForm.find('input[name=dichVuId]').val('');
        });
        
        $('#OpenChuyenKhoaLookupTableButton').click(function () {

            var lichHenKham = _$lichHenKhamInformationForm.serializeFormToObject();

            _LichHenKhamchuyenKhoaLookupTableModal.open({
                id: lichHenKham.chuyenKhoaId,
                displayName: lichHenKham.chuyenKhoaTen
            }, function (data) {
                _$lichHenKhamInformationForm.find('input[name=chuyenKhoaTen]').val(data.displayName);
                _$lichHenKhamInformationForm.find('input[name=chuyenKhoaId]').val(data.id);
                getLichKham()
            });
        });

        $("#LichHenKham_NgayHenKham").on("dp.change", function () {
            getLichKham()
        });
        
        function getLichKham(){
            var chuyenKhoaId = $("#LichHenKham_ChuyenKhoaId").val();
            if(chuyenKhoaId === '' || chuyenKhoaId === -1){
                return;
            }
            else {
                var ngayHenKham = moment($('#LichHenKham_NgayHenKham').val(), "DD/MM/YYYY").format("MM/DD/YYYY");
                if ( chuyenKhoaId === '' || chuyenKhoaId === -1) {
                    abp.message.error( app.localize('ChonChuyenKhoaLaBatBuoc'));
                    return;
                }
                if ( ngayHenKham === '') {
                    abp.message.error(app.localize('{0}IsRequired', app.localize('ChonNgayHenKham')));
                    return;
                }
                _modalManager.setBusy(true);
                _lichHenKhamsService.getKhungKham({
                    ngayHenKham : ngayHenKham,
                    chuyenKhoaId : chuyenKhoaId
                }).done(function (data) {
                    let GioKhamSang = moment(ngayHenKham + ' ' + data.gioBatDauLamViecSang);
                    let GioKhamChieu = moment(ngayHenKham + ' ' + data.gioBatDauLamViecChieu);
                    let GioBatDauLamViecSang = moment(ngayHenKham + ' ' + data.gioBatDauLamViecSang);
                    let GioKetThucLamViecSang = moment(ngayHenKham + ' ' + data.gioKetThucLamViecSang);
                    let GioBatDauLamViecChieu = moment(ngayHenKham + ' ' + data.gioBatDauLamViecChieu);
                    let GioKetThucLamViecChieu = moment(ngayHenKham + ' ' + data.gioKetThucLamViecChieu);
                    let ThoiGianLamViecSang = GioKetThucLamViecSang.diff(GioBatDauLamViecSang, 'minutes');
                    let ThoiGianLamViecChieu = GioKetThucLamViecChieu.diff(GioBatDauLamViecChieu, 'minutes');
                    var hienthi = '';
                    var dem = [];

                    //Thuật toán đếm phân tán 

                    for (let khungKham = 1; khungKham <= ThoiGianLamViecSang / data.khamSession; khungKham++)
                    {
                        dem[khungKham] = 0; //đánh dấu là chưa trùng
                    }
                    for(let khungKham = (ThoiGianLamViecSang/data.khamSession)+1; khungKham <= (ThoiGianLamViecChieu + ThoiGianLamViecSang)/data.khamSession; khungKham++){
                        dem[khungKham] = 0;
                    }
                    $.each(data.lichHenKham,function (key, value) {
                        dem[value.khungKham] = 1; //đánh dấu là trùng
                    });
                    for (let khungKham = 1; khungKham <= ThoiGianLamViecSang / data.khamSession; khungKham++) {
                        var a = moment(moment().format("MM/DD/YYYY HH:mm"));
                        var b = moment(GioKhamSang.format("MM/DD/YYYY HH:mm"));
                        var c = b.diff(a, 'minutes');
                        if(c > 0 ){
                            //bắt đầu so sánh đã trùng hay chưa

                            if (dem[khungKham] == 1) {
                                {
                                    hienthi += '<div class="col-2">\n' +
                                        '     <input type="radio" class="btn-check col-1" name="khungKham" id="' + khungKham + '" value="' + khungKham + '" autocomplete="off" disabled hidden>\n' +
                                        '     <label class="btn btn-danger col-10" for="' + khungKham + '">' + GioKhamSang.format('LT') + '</label>\n' +
                                        '</div>';
                                }
                            } else {
                                hienthi += '<div class="col-2">\n' +
                                    '     <input type="radio" class="btn-check col-1" name="khungKham" id="' + khungKham + '" value="' + khungKham + '" autocomplete="off" hidden >\n' +
                                    '     <label class="btn btn-success col-10" for="' + khungKham + '">' + GioKhamSang.format('LT') + '</label>\n' +
                                    '</div>';
                            }
                        }
                        else {
                            hienthi += '<div class="col-2">\n' +
                                '     <input type="radio" class="btn-check col-1" name="khungKham" id="' + khungKham + '" value="' + khungKham + '" autocomplete="off" disabled hidden>\n' +
                                '     <label class="btn btn-secondary col-10" for="' + khungKham + '" disabled="">' + GioKhamSang.format('LT') + '</label>\n' +
                                '</div>';
                        }

                        GioKhamSang = GioKhamSang.add(data.khamSession, "minutes");
                    }
                    for(let khungKham = (ThoiGianLamViecSang/data.khamSession)+1; khungKham <= (ThoiGianLamViecChieu + ThoiGianLamViecSang)/data.khamSession; khungKham++){
                        var a = moment(moment().format("MM/DD/YYYY HH:mm"));
                        var b = moment(GioKhamChieu.format("MM/DD/YYYY HH:mm"));
                        var c = b.diff(a, 'minutes');
                        if(c > 0){
                            if(dem[khungKham] == 1){
                                // hienthi += '<div class="col-'+(12/(60/data.khamSession))+'">\n'
                                hienthi += '<div class="col-2">\n' +
                                    '     <input type="radio" class="btn-check col-1" name="khungKham" id="'+ khungKham+'" value="'+ khungKham+'" autocomplete="off" disabled hidden>\n' +
                                    '     <label class="btn btn-danger col-10" for="'+ khungKham+'">'+ GioKhamChieu.format('LT')+'</label>\n' +
                                    '</div>';
                            }
                            else{
                                hienthi += '<div class="col-2">\n' +
                                    '     <input type="radio" class="btn-check col-1" name="khungKham" id="'+ khungKham+'" value="'+ khungKham+'" autocomplete="off" hidden>\n' +
                                    '     <label class="btn btn-success col-10" for="'+ khungKham+'">'+ GioKhamChieu.format('LT') +'</label>\n' +
                                    '</div>';
                            }
                        }
                        else {
                            hienthi += '<div class="col-2">\n' +
                                '     <input type="radio" class="btn-check col-1" name="khungKham" id="'+ khungKham+'" value="' + khungKham + '" autocomplete="off" disabled hidden>\n' +
                                '     <label class="btn btn-secondary col-10" for="'+ khungKham+'">' + GioKhamChieu.format('LT') + '</label>\n' +
                                '</div>';
                        }
                        GioKhamChieu = GioKhamChieu.add(data.khamSession, "minutes")
                    }
                    $('#hienKhungKham').html(hienthi);
                    _modalManager.setBusy(false);

                })
            }
                

        }
       $("#LichHenKham_IsCoBHYT").on("change", function (){
           if($("#LichHenKham_IsCoBHYT").val() === "false"){
               $("#aaa").attr("hidden","hidden");   
           }
           if($("#LichHenKham_IsCoBHYT").val() === "true"){
               $("#aaa").removeAttr("hidden");
           }
       });
        
        $('#ClearChuyenKhoaTenButton').click(function () {
            _$lichHenKhamInformationForm.find('input[name=chuyenKhoaTen]').val('');
            _$lichHenKhamInformationForm.find('input[name=chuyenKhoaId]').val('');
        });
       
        this.save = function () {
            if (!_$lichHenKhamInformationForm.valid()) {
                return;
            }
            if ($("input[type='radio']:checked").val() === null || $("input[type='radio']:checked").val() === undefined) {
                abp.message.error(app.localize('ChuaChonGioKham'));
                return;
            }
            if ($('#LichHenKham_BacSiId').prop('required') && $('#LichHenKham_BacSiId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#LichHenKham_ThuNganId').prop('required') && $('#LichHenKham_ThuNganId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#LichHenKham_NguoiBenhId').prop('required') || $('#LichHenKham_NguoiBenhId').val() == '') {
                abp.message.error(app.localize('ChuaChonNguoiBenh'));
                return;
            }
            if ($('#LichHenKham_NguoiThanId').prop('required') && $('#LichHenKham_NguoiThanId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('NguoiThan')));
                return;
            }
            if ($('#LichHenKham_DichVuId').prop('required') && $('#LichHenKham_DichVuId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DichVu')));
                return;
            }
            if ($('#LichHenKham_ChuyenKhoaId').prop('required') && $('#LichHenKham_ChuyenKhoaId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ChuyenKhoa')));
                return;
            }

            var lichHenKham = _$lichHenKhamInformationForm.serializeFormToObject();
            console.log(lichHenKham);

            _modalManager.setBusy(true);
            _lichHenKhamsService.createOrEdit(
                lichHenKham
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditLichHenKhamModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);