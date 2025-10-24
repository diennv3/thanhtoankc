(function () {
    $(function () {

        var _$bacSiChuyenKhoasTable = $('#BacSiChuyenKhoasTable');
        var _bacSiChuyenKhoasService = abp.services.app.bacSiChuyenKhoas;
        var _thongTinBacSiMoRong = abp.services.app.thongTinBacSiMoRongs;
        var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.BacSiChuyenKhoa';

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.BacSiChuyenKhoas.Create'),
            edit: abp.auth.hasPermission('Pages.BacSiChuyenKhoas.Edit'),
            'delete': abp.auth.hasPermission('Pages.BacSiChuyenKhoas.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BacSiChuyenKhoas/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BacSiChuyenKhoas/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBacSiChuyenKhoaModal'
        });

        var _capNhatAnhBacSiModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BacSiChuyenKhoas/CapNhatAnhBacSiModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BacSiChuyenKhoas/_CapNhatAnhBacSiModal.js',
            modalClass: 'CapNhatAnhBacSiModal'
        });
        
        var _viewBacSiChuyenKhoaModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BacSiChuyenKhoas/ViewbacSiChuyenKhoaModal',
            modalClass: 'ViewBacSiChuyenKhoaModal'
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

        var dataTable = _$bacSiChuyenKhoasTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bacSiChuyenKhoasService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#BacSiChuyenKhoasTableFilter').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
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
                                text: app.localize("CapNhatAnhBacSi"),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _capNhatAnhBacSiModal.open({thongTinBacSiMoRongId: data.record.thongTinBacSiMoRong.id})
                                }
                            },
                            // {
                            //     text: app.localize('View'),
                            //     action: function (data) {
                            //         _viewBacSiChuyenKhoaModal.open({id: data.record.bacSiChuyenKhoa.id});
                            //     }
                            // },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    console.log(data);
                                    _createOrEditModal.open({
                                        id: data.record.bacSiChuyenKhoa.id
                                    });
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
                                        entityId: data.record.bacSiChuyenKhoa.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteBacSiChuyenKhoa(data.record.bacSiChuyenKhoa, data.record.thongTinBacSiMoRong);
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
                    data: "thongTinBacSiMoRong.chucDanh",
                    name: "thongTinBacSiMoRong.chucDanh"
                },
                {
                    targets: 3,
                    data: "userName",
                    name: "userFk.name"
                },
                {
                    targets: 4,
                    //data: "thongTinBacSiMoRong.image",
                    render: function (image, type, row, meta) {
                        console.log(row);
                        var $container = $("<span/>");
                        if (row.thongTinBacSiMoRong.image != null) {
                            var profilePictureUrl = "BacSiChuyenKhoas/GetProfilePictureById?id=" + row.thongTinBacSiMoRong.image;
                            var $link = $("<a/>").attr("href", profilePictureUrl).attr("target", "_blank");
                            var $img = $("<img width='34' height='34' />")
                                .addClass("img-circle")
                                .attr("src", profilePictureUrl);

                            $link.append($img);
                            $container.append($link);
                        }

                        $container.append(image);
                        return $container[0].outerHTML;
                    }
                },
                {
                    targets: 5,
                    data: "thongTinBacSiMoRong.tieuSu",
                    name: "tieuSu",
                    render: function (displayName, type, row, meta) {
                        let btnTieuSu = '<button class="btn btn-primary btn-sm btn-brand xemTieuSu" data-index="' + row.thongTinBacSiMoRong.tieuSu + '">Xem Tiểu sử</button>';
                        return btnTieuSu
                    }
                },
            ]
        });

        var TieuSuContainer = document.createElement("div");
        TieuSuContainer.className = "text-center";
        TieuSuContainer.id = "tieusucontainer";
        $(document).on('click', '.xemTieuSu', function () {
            var $self = $(this);
            let noidung = $self.attr('data-index');
            TieuSuContainer.textContent = noidung;
            sweetAlert({
                content: TieuSuContainer,
                button: {
                    Text: app.localize("OK"),
                    closeModal: true
                }
            });
        });
        
        function getBacSiChuyenKhoas() {
            dataTable.ajax.reload();
        }

        function deleteBacSiChuyenKhoa(bacSiChuyenKhoa, thongTinBacSiMoRong) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _thongTinBacSiMoRong.delete({
                            id:thongTinBacSiMoRong.id
                        }).done(function () {
                            _bacSiChuyenKhoasService.delete({
                                id: bacSiChuyenKhoa.id
                            }).done(function () {
                                getBacSiChuyenKhoas(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
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

        $('#CreateNewBacSiChuyenKhoaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _bacSiChuyenKhoasService
                .getBacSiChuyenKhoasToExcel({
                    filter: $('#BacSiChuyenKhoasTableFilter').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    chuyenKhoaTenFilter: $('#ChuyenKhoaTenFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBacSiChuyenKhoaModalSaved', function () {
            getBacSiChuyenKhoas();
        });

        $('#GetBacSiChuyenKhoasButton').click(function (e) {
            e.preventDefault();
            getBacSiChuyenKhoas();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getBacSiChuyenKhoas();
            }
        });
        
        function getBacSiChuyenKhoas() {
            dataTable.ajax.reload(null, false);
        }
        abp.event.on('LoadLaiTrang', function () {
            getBacSiChuyenKhoas();
        });

    });
})();