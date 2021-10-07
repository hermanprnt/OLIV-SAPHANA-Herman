//Freeze table js
var freezeTable = {
    init: function () {
        $(document).find('.fixed-columns').each(function (i, elem) {
            freezeTable.fixColumns(elem);
        });
    },

    fixColumns: function (table, columns) {
        var $table = $(table);
        $table.removeClass('fixed-columns');
        $table.css('overflow-y', 'hidden');
        $table.css('max-height', '446px');
        var $fixedColumns = $table.clone().attr('id', $table.attr('id') + '-fixed').insertBefore($table).addClass('fixed-columns-fixed');

        $fixedColumns.find('*').each(function (i, elem) {
            if ($(this).attr('id') !== undefined) {
                $table.find("[id='" + $(this).attr("id") + "']").attr('id', $(this).attr('id'));
            }
            if ($(this).attr('name') !== undefined) {
                $table.find("[name='" + $(this).attr("name") + "']").attr('name', $(this).attr('name'));
            }
        });

        if (columns !== undefined) {
            $fixedColumns.find('tr').each(function (x, elem) {
                $(elem).find('th,td').each(function (i, elem) {
                    if (i >= columns) {
                        $(elem).remove();
                    }
                });
            });
        } else {
            $fixedColumns.find('tr').each(function (x, elem) {
                $(elem).find('th,td').each(function (i, elem) {
                    if (!$(elem).hasClass('fixed-column')) {
                        $(elem).remove();
                    } else {
                        /*
                        * Updated by : alira.salman
                        * Updated dt : 11.08.2015
                        * Description :
                        * To maintain freeze-column size when window resized. remember to give id
                        * both freezing table and related freeze column.
                        */
                        var $baseColumn = $table.find("[id='" + $(elem).attr('id') + "']");
                        $(elem).width($baseColumn.width());

                        //$(elem).width($(elem).width());
                    }
                });
            });
        }

        $fixedColumns.find('tr').each(function (i, elem) {
            $(this).height($table.find('tr:eq(' + i + ')').height());
        });
    }
};


//Preloader table function
function showLoading(page) {
    var preloader = page.find("#preloader")
    var wait_div = page.find("#wait")
    preloader.show();
    wait_div.show();
}

function hideLoading(page) {
    var preloader = page.find("#preloader")
    var wait_div = page.find("#wait")
    preloader.hide();
    wait_div.hide();
}

function hideColumn(btn) {

    if ($("._hide").is(":visible")) {
        $("._hide").hide();
        if (btn != undefined) $(btn).html("Show Detail");
        $(document).find('.fixed-columns').each(function (i, elem) {
            freezeTable.fixColumns(elem);
        });
        $.initFixedTable();
        $.resetFixedTable();
    } else {
        $("._hide").show();
        if (btn != undefined) $(btn).html("Hide Detail");
        $(document).find('.fixed-columns').each(function (i, elem) {
            freezeTable.fixColumns(elem);
        });
        $.initFixedTable();
        $.resetFixedTable();
    }
}

function toogleTableMode(mode, table) {
    var $table = $("#" + table);

    if (mode === "show") {
        $table.find(".toogle-column").each(function (index, element) {
            $(element).show();
        });
    }
    else {
        $table.find(".toogle-column").each(function (index, element) {
            $(element).hide();
        });
    }
}

function toogleTable(button, table) {
    var $table = $("#" + table);
    var $button = $(button);
    var mode = $button.data("mode");

    if (mode === "show") {
        $button.data("mode", "hide");
        $button.html("Sembunyi Detil");
    }
    else {
        $button.data("mode", "show");
        $button.html("Tampil Detil");
    }

    toogleTableMode(mode, table);

    if ($table.find("td#blank-col")) {
        var $blankCol = $table.find("td#blank-col");
        var spanColumn = parseInt($blankCol.attr("colspan")) || 0;
        var toogleColumn = parseInt($blankCol.data("tooglecolumn")) || 0;

        if (mode === "show") {
            spanColumn = spanColumn + toogleColumn;
            $blankCol.attr("colspan", spanColumn);
        }
        else {
            spanColumn = spanColumn - toogleColumn;
            $blankCol.attr("colspan", spanColumn);
        }
    }

    if ($table.hasClass("afixed-table")) {
        $.remakeFixedTable();
    }
}