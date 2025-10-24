(function () {
    $(function () {

        var _$dichVusTable = $('#DichVusTable');
        var _dichVusService = abp.services.app.dichVus;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.DichVu';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DichVus.Create'),
            edit: abp.auth.hasPermission('Pages.DichVus.Edit'),
            'delete': abp.auth.hasPermission('Pages.DichVus.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DichVus/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DichVus/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDichVuModal'
        });       

		 var _viewDichVuModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DichVus/ViewdichVuModal',
            modalClass: 'ViewDichVuModal'
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

        var dataTable = _$dichVusTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _dichVusService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#DichVusTableFilter').val(),
					tenFilter: $('#TenFilterId').val(),
					moTaFilter: $('#MoTaFilterId').val(),
					chuyenKhoaTenFilter: $('#ChuyenKhoaTenFilterId').val()
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
                                    _viewDichVuModal.open({ id: data.record.dichVu.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.dichVu.id });                                
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
                                    entityId: data.record.dichVu.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteDichVu(data.record.dichVu);
                            }
                        }]
                    }
                },
                {
                    targets: 1,
                    data: "chuyenKhoaTen",
                    name: "chuyenKhoaFk.ten"
                },
                {
                    targets: 2,
                    data: "dichVu.ten",
                    name: "ten"
                    
                },
                {
                    targets: 3,
                    data: "giaDichVu.gia",
                    name: "giaDichVu.gia",
                    render: function (data, meta, row) {
                        return addCommas(data);
                    }
                },
                {
                    targets: 4,
                    data: "dichVu.moTa",
                    name: "moTa"
                }
                
            ]
        });

        function getDichVus() {
            dataTable.ajax.reload();
        }

        function deleteDichVu(dichVu) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _dichVusService.delete({
                            id: dichVu.id
                        }).done(function () {
                            getDichVus(true);
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

        $('#CreateNewDichVuButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _dichVusService
                .getDichVusToExcel({
				filter : $('#DichVusTableFilter').val(),
					tenFilter: $('#TenFilterId').val(),
					moTaFilter: $('#MoTaFilterId').val(),
					chuyenKhoaTenFilter: $('#ChuyenKhoaTenFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDichVuModalSaved', function () {
            getDichVus();
        });

		$('#GetDichVusButton').click(function (e) {
            e.preventDefault();
            getDichVus();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getDichVus();
		  }
		});
    });
})();