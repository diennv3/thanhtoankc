(function () {
    $(function () {

        var _$chiTietThanhToansTable = $('#ChiTietThanhToansTable');
        var _chiTietThanhToansService = abp.services.app.chiTietThanhToans;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.ChiTietThanhToan';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ChiTietThanhToans.Create'),
            edit: abp.auth.hasPermission('Pages.ChiTietThanhToans.Edit'),
            'delete': abp.auth.hasPermission('Pages.ChiTietThanhToans.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChiTietThanhToans/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChiTietThanhToans/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditChiTietThanhToanModal'
        });       

		 var _viewChiTietThanhToanModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChiTietThanhToans/ViewchiTietThanhToanModal',
            modalClass: 'ViewChiTietThanhToanModal'
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

        var dataTable = _$chiTietThanhToansTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chiTietThanhToansService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ChiTietThanhToansTableFilter').val(),
					minSoTienThanhToanFilter: $('#MinSoTienThanhToanFilterId').val(),
					maxSoTienThanhToanFilter: $('#MaxSoTienThanhToanFilterId').val(),
					minLoaiThanhToanFilter: $('#MinLoaiThanhToanFilterId').val(),
					maxLoaiThanhToanFilter: $('#MaxLoaiThanhToanFilterId').val(),
					minNgayThanhToanFilter:  getDateFilter($('#MinNgayThanhToanFilterId')),
					maxNgayThanhToanFilter:  getDateFilter($('#MaxNgayThanhToanFilterId')),
					lichHenKhamMoTaTrieuChungFilter: $('#LichHenKhamMoTaTrieuChungFilterId').val(),
					nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
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
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewChiTietThanhToanModal.open({ id: data.record.chiTietThanhToan.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.chiTietThanhToan.id });                                
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
                                    entityId: data.record.chiTietThanhToan.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteChiTietThanhToan(data.record.chiTietThanhToan);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "chiTietThanhToan.soTienThanhToan",
						 name: "soTienThanhToan"   
					},
					{
						targets: 2,
						 data: "chiTietThanhToan.loaiThanhToan",
						 name: "loaiThanhToan"   
					},
					{
						targets: 3,
						 data: "chiTietThanhToan.ngayThanhToan",
						 name: "ngayThanhToan" ,
					render: function (ngayThanhToan) {
						if (ngayThanhToan) {
							return moment(ngayThanhToan).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 4,
						 data: "lichHenKhamMoTaTrieuChung" ,
						 name: "lichHenKhamFk.moTaTrieuChung" 
					},
					{
						targets: 5,
						 data: "nguoiBenhUserName" ,
						 name: "nguoiBenhFk.userName" 
					}
            ]
        });

        function getChiTietThanhToans() {
            dataTable.ajax.reload();
        }

        function deleteChiTietThanhToan(chiTietThanhToan) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _chiTietThanhToansService.delete({
                            id: chiTietThanhToan.id
                        }).done(function () {
                            getChiTietThanhToans(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
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

        $('#CreateNewChiTietThanhToanButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _chiTietThanhToansService
                .getChiTietThanhToansToExcel({
				filter : $('#ChiTietThanhToansTableFilter').val(),
					minSoTienThanhToanFilter: $('#MinSoTienThanhToanFilterId').val(),
					maxSoTienThanhToanFilter: $('#MaxSoTienThanhToanFilterId').val(),
					minLoaiThanhToanFilter: $('#MinLoaiThanhToanFilterId').val(),
					maxLoaiThanhToanFilter: $('#MaxLoaiThanhToanFilterId').val(),
					minNgayThanhToanFilter:  getDateFilter($('#MinNgayThanhToanFilterId')),
					maxNgayThanhToanFilter:  getDateFilter($('#MaxNgayThanhToanFilterId')),
					lichHenKhamMoTaTrieuChungFilter: $('#LichHenKhamMoTaTrieuChungFilterId').val(),
					nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditChiTietThanhToanModalSaved', function () {
            getChiTietThanhToans();
        });

		$('#GetChiTietThanhToansButton').click(function (e) {
            e.preventDefault();
            getChiTietThanhToans();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getChiTietThanhToans();
		  }
		});
    });
})();