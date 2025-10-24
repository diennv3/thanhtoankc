(function () {
    $(function () {

        var _$nguoiThansTable = $('#NguoiThansTable');
        var _nguoiThansService = abp.services.app.nguoiThans;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.NguoiThan';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.NguoiThans.Create'),
            edit: abp.auth.hasPermission('Pages.NguoiThans.Edit'),
            'delete': abp.auth.hasPermission('Pages.NguoiThans.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiThans/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/NguoiThans/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditNguoiThanModal'
        });       

		 var _viewNguoiThanModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiThans/ViewnguoiThanModal',
            modalClass: 'ViewNguoiThanModal'
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

        var dataTable = _$nguoiThansTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _nguoiThansService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#NguoiThansTableFilter').val(),
					hoVaTenFilter: $('#HoVaTenFilterId').val(),
					minTuoiFilter: $('#MinTuoiFilterId').val(),
					maxTuoiFilter: $('#MaxTuoiFilterId').val(),
					gioiTinhFilter: $('#GioiTinhFilterId').val(),
					diaChiFilter: $('#DiaChiFilterId').val(),
					moiQuanHeFilter: $('#MoiQuanHeFilterId').val(),
					soDienThoaiFilter: $('#SoDienThoaiFilterId').val(),
					nguoiBenhHoVaTenFilter: $('#NguoiBenhHoVaTenFilterId').val()
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
                                    _viewNguoiThanModal.open({ id: data.record.nguoiThan.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.nguoiThan.id });                                
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
                                    entityId: data.record.nguoiThan.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteNguoiThan(data.record.nguoiThan);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "nguoiThan.hoVaTen",
						 name: "hoVaTen"   
					},
					{
						targets: 2,
						 data: "nguoiThan.tuoi",
						 name: "tuoi"   
					},
					{
						targets: 3,
						 data: "nguoiThan.gioiTinh",
						 name: "gioiTinh"   
					},
					{
						targets: 4,
						 data: "nguoiThan.diaChi",
						 name: "diaChi"   
					},
					{
						targets: 5,
						 data: "nguoiThan.moiQuanHe",
						 name: "moiQuanHe"   
					},
					{
						targets: 6,
						 data: "nguoiThan.soDienThoai",
						 name: "soDienThoai"   
					},
					{
						targets: 7,
						 data: "nguoiBenhHoVaTen" ,
						 name: "nguoiBenhFk.hoVaTen" 
					}
            ]
        });

        function getNguoiThans() {
            dataTable.ajax.reload();
        }

        function deleteNguoiThan(nguoiThan) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _nguoiThansService.delete({
                            id: nguoiThan.id
                        }).done(function () {
                            getNguoiThans(true);
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

        $('#CreateNewNguoiThanButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _nguoiThansService
                .getNguoiThansToExcel({
				filter : $('#NguoiThansTableFilter').val(),
					hoVaTenFilter: $('#HoVaTenFilterId').val(),
					minTuoiFilter: $('#MinTuoiFilterId').val(),
					maxTuoiFilter: $('#MaxTuoiFilterId').val(),
					gioiTinhFilter: $('#GioiTinhFilterId').val(),
					diaChiFilter: $('#DiaChiFilterId').val(),
					moiQuanHeFilter: $('#MoiQuanHeFilterId').val(),
					soDienThoaiFilter: $('#SoDienThoaiFilterId').val(),
					nguoiBenhHoVaTenFilter: $('#NguoiBenhHoVaTenFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditNguoiThanModalSaved', function () {
            getNguoiThans();
        });

		$('#GetNguoiThansButton').click(function (e) {
            e.preventDefault();
            getNguoiThans();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getNguoiThans();
		  }
		});
    });
})();