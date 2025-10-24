(function () {
    $(function () {

        var _$thongTinDonViesTable = $('#ThongTinDonViesTable');
        var _thongTinDonViesService = abp.services.app.thongTinDonVies;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.ThongTinDonVi';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ThongTinDonVies.Create'),
            edit: abp.auth.hasPermission('Pages.ThongTinDonVies.Edit'),
            'delete': abp.auth.hasPermission('Pages.ThongTinDonVies.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ThongTinDonVies/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ThongTinDonVies/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditThongTinDonViModal'
        });       

		 var _viewThongTinDonViModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ThongTinDonVies/ViewthongTinDonViModal',
            modalClass: 'ViewThongTinDonViModal'
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

        var dataTable = _$thongTinDonViesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _thongTinDonViesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ThongTinDonViesTableFilter').val(),
					keyFilter: $('#KeyFilterId').val(),
					valueFilter: $('#ValueFilterId').val()
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
                                    _viewThongTinDonViModal.open({ id: data.record.thongTinDonVi.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.thongTinDonVi.id });                                
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
                                    entityId: data.record.thongTinDonVi.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteThongTinDonVi(data.record.thongTinDonVi);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "thongTinDonVi.key",
						 name: "key"   
					},
					{
						targets: 2,
						 data: "thongTinDonVi.value",
						 name: "value"   
					}
            ]
        });

        function getThongTinDonVies() {
            dataTable.ajax.reload();
        }

        function deleteThongTinDonVi(thongTinDonVi) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _thongTinDonViesService.delete({
                            id: thongTinDonVi.id
                        }).done(function () {
                            getThongTinDonVies(true);
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

        $('#CreateNewThongTinDonViButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _thongTinDonViesService
                .getThongTinDonViesToExcel({
				filter : $('#ThongTinDonViesTableFilter').val(),
					keyFilter: $('#KeyFilterId').val(),
					valueFilter: $('#ValueFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditThongTinDonViModalSaved', function () {
            getThongTinDonVies();
        });

		$('#GetThongTinDonViesButton').click(function (e) {
            e.preventDefault();
            getThongTinDonVies();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getThongTinDonVies();
		  }
		});
    });
})();