(function ($) {
    app.modals.ListDichVuKhamModal = function () {

        function isEmptyOrSpaces(str) {
            return str === null || str.match(/^ *$/) !== null;
        }

        var _flag = $('#FlagId').val();

        var _qroptions = {
            text: "",
            width: 256,
            height: 256,
            logo: "/Common/Images/techber.png"
        }

        var _modalManager;

        var _lichHenKhamService = abp.services.app.lichHenKhams;
        var _$dichVuKhamTable = $('#danhSachDichVuId');


        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        let chuyenKhoaId = parseInt($('#chuyenKhoaId').val());
        let listDichVu = JSON.parse($('#serializedDichVuId').val())

        var dataTable = _$dichVuKhamTable.DataTable({
            //scrollX: true,
            //scrollY: '50vh',
            paging: false,
            serverSide: true,
            processing: true,
            scrollResize: true,
            info: false,
            //responsive: false,
            //sort: false,
            select: {
                style: 'multi',
                selector: 'td:first-child'
            },
            listAction: {
                ajaxFunction: _lichHenKhamService.getDichVuChuyenKhoaVaGiaDichVu,
                inputFilter: function () {
                    return {
                        chuyenKhoaId: chuyenKhoaId
                    }
                }
            },
            columnDefs: [
                {
                    width: 60,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: "",
                    className: _flag == 2 ? "select-checkbox" : ""
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: "tenDichVu"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 2,
                    data: "moTa"
                },
                {
                    targets: 3,
                    autoWidth: false,
                    orderable: false,
                    render: function (displayName, type, row, meta) {
                        if (_flag != 3)
                            return row.gia;
                        else {
                            let kq = listDichVu.find(x => x.id == row.id)
                            if (kq == undefined) {
                                return row.gia
                            }else
                                return kq.gia
                        }
                    }
                }
            ],
            rowCallback: function (row, data) {
                if (listDichVu.map(x => x.id).includes(data.id)) {
                    this.api().row(row).select();
                } else if(_flag == 3) {
                    $(row).addClass('d-none');
                }
            },
            initComplete: function (settings, json) {
                var sum = 0;
                sum = listDichVu.map(x => x.gia)
                    .reduce( (a, b) => a + b ,0 );
                $('#tong-tien-thanh-toan').html(sum)
            }
        })

        //dataTable.row('.bo-di').remove().draw(false)

        dataTable.on("click", "th.select-checkbox", function () {
            if ($("th.select-checkbox").hasClass("selected")) {
                dataTable.rows().deselect();
                $("th.select-checkbox").removeClass("selected");
            } else {
                dataTable.rows().select();
                $("th.select-checkbox").addClass("selected");
            }
        }).on("select deselect", function () {
            ("Some selection or deselection going on")
            if (dataTable.rows({
                selected: true
            }).count() !== dataTable.rows().count()) {
                $("th.select-checkbox").removeClass("selected");
            } else {
                $("th.select-checkbox").addClass("selected");
            }
        });

        dataTable.on('user-select', function (e, dt, type, cell, originalEvent) {
            if ($('#FlagId').val() != 2) {
                e.preventDefault();
            }

        });

        $("#btn-tao-thanh-toan").on("click", function () {
            var self = $(this);
            _lichHenKhamService.generator(self.attr("data-target")).done(function (result) {
                if (!isEmptyOrSpaces(result)) {
                    _qroptions.text = result
                    $('#btn-thanh-toan-wrapper').remove();
                    new QRCode(document.getElementById("qrcontainer"), _qroptions);
                }
            })
        })

        if ($("#qrcontainer").attr('data-container') != undefined && !isEmptyOrSpaces($("#qrcontainer").attr('data-container'))) {
            _qroptions.text = $("#qrcontainer").attr('data-container')
            new QRCode(document.getElementById("qrcontainer"), _qroptions);
        }

        function getDichVus() {
            dataTable.ajax.reload();
        }

        $('.btn.save-button').on('click', function () {
            let rowId = $('#rowId').val();
            let data = dataTable.rows({ selected: true }).data();
            let listSelected = [];
            let result = []
            for (var i = 0; i < data.length; i++) {
                result[i] = data[i];
                listSelected[i] = {}
                listSelected[i].id = data[i].id;
                listSelected[i].gia = data[i].gia;
            }

            _lichHenKhamService.capNhatDichVuLichHenKham({
                id: rowId,
                dichVuSerialized: JSON.stringify(result)
            }).then(ketqua => {
                if (ketqua == 200) {
                    _modalManager.setResult(JSON.stringify(listSelected));
                    _modalManager.close();
                } else {
                    alert(app.localize("CoLoiXayRa"))
                }
            })
        })

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDichVus();
            }
        });

    }
})(jQuery);