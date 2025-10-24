(function () {
    $(function () {

        var _$nguoiBenhsTable = $('#NguoiBenhsTable');
        var _nguoiBenhsService = abp.services.app.nguoiBenhs;
        var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.NguoiBenh';

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.NguoiBenhs.Create'),
            edit: abp.auth.hasPermission('Pages.NguoiBenhs.Edit'),
            viewNguoiThan: abp.auth.hasPermission('Pages.NguoiThans'),
            'delete': abp.auth.hasPermission('Pages.NguoiBenhs.Delete')
        };
        var _updateImageProfileModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiBenhs/UpdateImageProfileModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/NguoiBenhs/_UpdateImageProfileModal.js',
            modalClass: 'UpdateImageProfileModal'
        });
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiBenhs/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/NguoiBenhs/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditNguoiBenhModal'
        });
        var _viewNguoiThanModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiThans/ViewDanhSachNguoiThanModal',
            modalClass: 'ViewDanhSachNguoiThanModal',
            modalSize: 'modal-full modal-dialog-scrollable',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/NguoiThans/_ViewListNguoiThanModal.js',
        });
        var _viewNguoiBenhModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiBenhs/ViewnguoiBenhModal',
            modalClass: 'ViewNguoiBenhModal'
        });

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        function getDateFilter(element) {
            var picker = element.data("DateTimePicker");
            if (!picker || !picker.date()) {
                return null;
            }
            return picker.date().format("YYYY-MM-DDT00:00:00Z");
        }

        function renderWorkDays(mask) {
            if (!mask) return "";
            var arr = [];
            mask = parseInt(mask);
            if (mask & 2) arr.push('T2');
            if (mask & 4) arr.push('T3');
            if (mask & 8) arr.push('T4');
            if (mask & 16) arr.push('T5');
            if (mask & 32) arr.push('T6');
            if (mask & 64) arr.push('T7');
            if (mask & 1) arr.push('CN');
            return arr.join(', ');
        }

        // Helper: lấy giá trị int từ selector, trả về -1 nếu không có (tức là "không lọc")
        function getIntFilterOrDefault(selector) {
            var $el = $(selector);
            if ($el.length === 0) return -1;
            var v = $el.val();
            if (v === undefined || v === null || v === "") return -1;
            var i = parseInt(v);
            return isNaN(i) ? -1 : i;
        }

        var dataTable = _$nguoiBenhsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _nguoiBenhsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#NguoiBenhsTableFilter').val(),
                        hoVaTenFilter: $('#HoVaTenFilterId').val(),
                        tuoiFilter: $('#TuoiFilterId').val(),
                        gioiTinhFilter: $('#GioiTinhFilterId').val(),
                        diaChiFilter: $('#DiaChiFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
                        minAccessFailedCountFilter: $('#MinAccessFailedCountFilterId').val(),
                        maxAccessFailedCountFilter: $('#MaxAccessFailedCountFilterId').val(),
                        phoneNumberFilter: $('#PhoneNumberFilterId').val(),
                        emailAddressFilter: $('#EmailAddressFilterId').val(),
                        emailConfirmationCodeFilter: $('#EmailConfirmationCodeFilterId').val(),
                        isActiveFilter: $('#IsActiveFilterId').val(),
                        isEmailConfirmedFilter: $('#IsEmailConfirmedFilterId').val(),
                        isNhanVienFilter: getIntFilterOrDefault('#IsNhanVienFilterId'),
                        isPhoneNumberConfirmedFilter: $('#IsPhoneNumberConfirmedFilterId').val(),
                        passwordResetCodeFilter: $('#PasswordResetCodeFilterId').val(),
                        profilePictureFilter: $('#ProfilePictureFilterId').val(),
                        passwordFilter: $('#PasswordFilterId').val(),
                        tokenFilter: $('#TokenFilterId').val(),
                        minTokenExpireFilter: getDateFilter($('#MinTokenExpireFilterId')),
                        maxTokenExpireFilter: getDateFilter($('#MaxTokenExpireFilterId'))
                    };
                }
            },
            columns: [
                {
                    data: null,
                    orderable: false,
                    width: 120,
                    render: function (data, type, row) {
                        var id = row.nguoiBenh.id;
                        var html = '<div class="btn-group">';
                        html += '<button class="btn btn-brand dropdown-toggle" data-toggle="dropdown"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>';
                        html += '<div class="dropdown-menu">';
                        html += '<a class="dropdown-item" href="javascript:;" onclick="_viewNguoiBenh(' + id + ')">' + app.localize('View') + '</a>';
                        if (_permissions.edit) html += '<a class="dropdown-item" href="javascript:;" onclick="_editNguoiBenh(' + id + ')">' + app.localize('Edit') + '</a>';
                        if (_permissions.edit) html += '<a class="dropdown-item" href="javascript:;" onclick="_updateImage(' + id + ')">Cập nhật ảnh</a>';
                        if (_permissions.viewNguoiThan) html += '<a class="dropdown-item" href="javascript:;" onclick="_viewNguoiThan(' + id + ')">' + app.localize('ViewNguoiThan') + '</a>';
                        if (entityHistoryIsEnabled()) html += '<a class="dropdown-item" href="javascript:;" onclick="_history(' + id + ')">' + app.localize('History') + '</a>';
                        if (_permissions['delete']) html += '<a class="dropdown-item text-danger" href="javascript:;" onclick="_deleteNguoiBenh(' + id + ')">' + app.localize('Delete') + '</a>';
                        html += '</div></div>';
                        return html;
                    }
                },
                { data: "nguoiBenh.hoVaTen", name: "hoVaTen" },
                {
                    data: null,
                    name: "ngaySinh",
                    render: function (data, type, row) {
                        return (row.nguoiBenh.ngaySinh || "") + "/" + (row.nguoiBenh.thangSinh || "") + "/" + (row.nguoiBenh.namSinh || "");
                    }
                },
                { data: "nguoiBenh.gioiTinh", name: "gioiTinh" },
                { data: "nguoiBenh.diaChi", name: "diaChi" },
                { data: "nguoiBenh.userName", name: "userName" },
                // Assigned shift name (from backend projection)
                {
                    data: "nguoiBenh.assignedShiftName",
                    name: "assignedShiftName",
                    render: function (val) { return val ? val : ""; }
                },
                // Assigned shift time
                {
                    data: null,
                    orderable: false,
                    render: function (data, type, row) {
                        var s = row.nguoiBenh.assignedShiftStart;
                        var e = row.nguoiBenh.assignedShiftEnd;
                        if (s && e) {
                            var sShort = (s.length > 5) ? s.substring(0, 5) : s;
                            var eShort = (e.length > 5) ? e.substring(0, 5) : e;
                            return sShort + " - " + eShort;
                        }
                        return "";
                    }
                },
                {
                    data: "nguoiBenh.assignedShiftWorkDaysMask",
                    name: "assignedShiftWorkDaysMask",
                    render: function (mask) { return renderWorkDays(mask); }
                },
                { data: "nguoiBenh.accessFailedCount", name: "accessFailedCount" },
                { data: "nguoiBenh.phoneNumber", name: "phoneNumber" },
                { data: "nguoiBenh.emailAddress", name: "emailAddress" },
                { data: "nguoiBenh.emailConfirmationCode", name: "emailConfirmationCode" },
                {
                    data: "nguoiBenh.isActive",
                    name: "isActive",
                    render: function (isActive) {
                        if (isActive) return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    }
                },
                {
                    data: "nguoiBenh.isEmailConfirmed",
                    name: "isEmailConfirmed",
                    render: function (v) { if (v) return '<div class="text-center"><i class="fa fa-check kt--font-success"></i></div>'; return '<div class="text-center"><i class="fa fa-times-circle"></i></div>'; }
                },
                {
                    data: "nguoiBenh.isPhoneNumberConfirmed",
                    name: "isPhoneNumberConfirmed",
                    render: function (v) { if (v) return '<div class="text-center"><i class="fa fa-check kt--font-success"></i></div>'; return '<div class="text-center"><i class="fa fa-times-circle"></i></div>'; }
                },
                { data: "nguoiBenh.passwordResetCode", name: "passwordResetCode" },
                { data: "nguoiBenh.profilePicture", name: "profilePicture" },
                { data: "nguoiBenh.password", name: "password" },
                { data: "nguoiBenh.token", name: "token" },
                {
                    data: "nguoiBenh.tokenExpire",
                    name: "tokenExpire",
                    render: function (tokenExpire) {
                        if (tokenExpire) return moment(tokenExpire).format('L');
                        return "";
                    }
                }
            ]
        });

        window._viewNguoiBenh = function (id) {
            _viewNguoiBenhModal.open({ id: id });
        };
        window._editNguoiBenh = function (id) {
            _createOrEditModal.open({ id: id });
        };
        window._updateImage = function (id) {
            _updateImageProfileModal.open({ id: id });
        };
        window._viewNguoiThan = function (id) {
            _viewNguoiThanModal.open({ id: id });
        };
        window._history = function (id) {
            _entityTypeHistoryModal.open({ entityTypeFullName: _entityTypeFullName, entityId: id });
        };
        window._deleteNguoiBenh = function (id) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _nguoiBenhsService.delete({ id: id }).done(function () {
                        getNguoiBenhs(true);
                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                    });
                }
            });
        };

        function getNguoiBenhs() {
            dataTable.ajax.reload();
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

        $('#CreateNewNguoiBenhButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _nguoiBenhsService
                .getNguoiBenhsToExcel({
                    filter: $('#NguoiBenhsTableFilter').val(),
                    hoVaTenFilter: $('#HoVaTenFilterId').val(),
                    tuoiFilter: $('#TuoiFilterId').val(),
                    gioiTinhFilter: $('#GioiTinhFilterId').val(),
                    diaChiFilter: $('#DiaChiFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    minAccessFailedCountFilter: $('#MinAccessFailedCountFilterId').val(),
                    maxAccessFailedCountFilter: $('#MaxAccessFailedCountFilterId').val(),
                    phoneNumberFilter: $('#PhoneNumberFilterId').val(),
                    emailAddressFilter: $('#EmailAddressFilterId').val(),
                    emailConfirmationCodeFilter: $('#EmailConfirmationCodeFilterId').val(),
                    isActiveFilter: $('#IsActiveFilterId').val(),
                    isEmailConfirmedFilter: $('#IsEmailConfirmedFilterId').val(),
                    isNhanVienFilter: getIntFilterOrDefault('#IsNhanVienFilterId'),
                    isPhoneNumberConfirmedFilter: $('#IsPhoneNumberConfirmedFilterId').val(),
                    passwordResetCodeFilter: $('#PasswordResetCodeFilterId').val(),
                    profilePictureFilter: $('#ProfilePictureFilterId').val(),
                    passwordFilter: $('#PasswordFilterId').val(),
                    tokenFilter: $('#TokenFilterId').val(),
                    minTokenExpireFilter: getDateFilter($('#MinTokenExpireFilterId')),
                    maxTokenExpireFilter: getDateFilter($('#MaxTokenExpireFilterId'))
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditNguoiBenhModalSaved', function () {
            getNguoiBenhs();
        });

        $('#GetNguoiBenhsButton').click(function (e) {
            e.preventDefault();
            getNguoiBenhs();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getNguoiBenhs();
            }
        });
    });
})();