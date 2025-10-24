(function () {
    $(function () {

        var _$bacSiDichVusTable = $('#BacSiDichVusTable');
        var _bacSiDichVusService = abp.services.app.bacSiDichVus;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.BacSiDichVu';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.BacSiDichVus.Create'),
            edit: abp.auth.hasPermission('Pages.BacSiDichVus.Edit'),
            'delete': abp.auth.hasPermission('Pages.BacSiDichVus.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BacSiDichVus/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BacSiDichVus/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBacSiDichVuModal'
        });       

		 var _viewBacSiDichVuModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BacSiDichVus/ViewbacSiDichVuModal',
            modalClass: 'ViewBacSiDichVuModal'
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

        var dataTable = _$bacSiDichVusTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bacSiDichVusService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BacSiDichVusTableFilter').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					dichVuTenFilter: $('#DichVuTenFilterId').val()
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
                                    _viewBacSiDichVuModal.open({ id: data.record.bacSiDichVu.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.bacSiDichVu.id });                                
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
                                    entityId: data.record.bacSiDichVu.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBacSiDichVu(data.record.bacSiDichVu);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "userName" ,
						 name: "userFk.name" 
					},
					{
						targets: 2,
						 data: "dichVuTen" ,
						 name: "dichVuFk.ten" 
					}
            ]
        });

        function getBacSiDichVus() {
            dataTable.ajax.reload();
        }

        function deleteBacSiDichVu(bacSiDichVu) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _bacSiDichVusService.delete({
                            id: bacSiDichVu.id
                        }).done(function () {
                            getBacSiDichVus(true);
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

        $('#CreateNewBacSiDichVuButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _bacSiDichVusService
                .getBacSiDichVusToExcel({
				filter : $('#BacSiDichVusTableFilter').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					dichVuTenFilter: $('#DichVuTenFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBacSiDichVuModalSaved', function () {
            getBacSiDichVus();
        });

		$('#GetBacSiDichVusButton').click(function (e) {
            e.preventDefault();
            getBacSiDichVus();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBacSiDichVus();
		  }
		});
    });
})();