!function ($) {
    "use strict";

    $.progressbox = {
        show: function (title, text) {
            if ($("body #progress-spinner").length === 0) {
                $("body").append(
                    "<div id='progress-spinner' class='modal fade bs-example-modal-lg' data-backdrop='static' data-keyboard='false'>" +
                    "   <div class='modal-dialog modal-lg' style='width:300px'>" +
                    "       <div class='modal-content'>" +
                    "           <div class='modal-header'>" +
                    "               <h4 id='progress-title'></h4>" +
                    "           </div>" +
                    "           <div class='modal-body'>" +
                    "               <div class='text-center'>" +
                    "                   <p id='progress-text'></p>" +
                    "                   <img src='/Content/Bootstrap/img/loading.gif' />" +
                    "               </div>" +
                    "           </div>" +
                    "       </div>" +
                    "   </div>" +
                    "</div>"
                );
            }

            $('#progress-spinner .modal-backdrop').not(':eq(0)').not(':eq(1)').remove();

            $("#progress-title").html(title);
            $("#progress-text").html(text);
            $("#progress-spinner").modal();
        },

        hide: function () {
            $("#progress-spinner").modal("hide");

            //$('#progress-spinner').on('shown.bs.modal', function (e) {
           //     $("#progress-spinner").modal('hide');
           // })
        }
    }

}(window.jQuery);
