(function () {
    $(function () {

        var _$bankCodesTable = $('#BankCodesTable');
        var _bankCodesService = abp.services.app.bankCodes;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.BankCode';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.BankCodes.Create'),
            edit: abp.auth.hasPermission('Pages.BankCodes.Edit'),
            'delete': abp.auth.hasPermission('Pages.BankCodes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BankCodes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BankCodes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBankCodeModal'
        });       

		 var _viewBankCodeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BankCodes/ViewbankCodeModal',
            modalClass: 'ViewBankCodeModal'
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

        var dataTable = _$bankCodesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bankCodesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BankCodesTableFilter').val(),
					codeFilter: $('#CodeFilterId').val(),
					bankNameFilter: $('#BankNameFilterId').val()
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
                                    _viewBankCodeModal.open({ id: data.record.bankCode.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.bankCode.id });                                
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
                                    entityId: data.record.bankCode.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBankCode(data.record.bankCode);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "bankCode.code",
						 name: "code"   
					},
					{
						targets: 2,
						 data: "bankCode.bankName",
						 name: "bankName"   
					}
            ]
        });

        function getBankCodes() {
            dataTable.ajax.reload();
        }

        function deleteBankCode(bankCode) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _bankCodesService.delete({
                            id: bankCode.id
                        }).done(function () {
                            getBankCodes(true);
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

        $('#CreateNewBankCodeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _bankCodesService
                .getBankCodesToExcel({
				filter : $('#BankCodesTableFilter').val(),
					codeFilter: $('#CodeFilterId').val(),
					bankNameFilter: $('#BankNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBankCodeModalSaved', function () {
            getBankCodes();
        });

		$('#GetBankCodesButton').click(function (e) {
            e.preventDefault();
            getBankCodes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBankCodes();
		  }
		});
    });
})();