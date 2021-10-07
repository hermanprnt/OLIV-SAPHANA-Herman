var STR_PAD_LEFT = 1;
var STR_PAD_RIGHT = 2;
var STR_PAD_BOTH = 3;
var STR_BG_COLOR_READONLY = "DarkGray";

function pad(str, len, pad, dir) {
    if (typeof (len) == "undefined") { var len = 0; }
    if (typeof (pad) == "undefined") { var pad = ' '; }
    if (typeof (dir) == "undefined") { var dir = STR_PAD_RIGHT; }

    if (len + 1 >= str.length) {

        switch (dir) {

            case STR_PAD_LEFT:
                str = Array(len + 1 - str.length).join(pad) + str;
                break;

            case STR_PAD_BOTH:
                var right = Math.ceil((padlen = len - str.length) / 2);
                var left = padlen - right;
                str = Array(left + 1).join(pad) + str + Array(right + 1).join(pad);
                break;

            default:
                str = str + Array(len + 1 - str.length).join(pad);
                break;

        } // switch
    }

    return str;
}

function handleAjaxResult(returnResult, processName, funcSuccess, funcError, isNotShowSuccessMesg, isMesgSuccessNotBlocking, isMesgErrorNotBlocking) {
    if (typeof returnResult.Redirect != "undefined") {
        window.location = returnResult.Redirect;
        return;
    }

    if (typeof returnResult.Result != "undefined" && typeof returnResult.ErrMesgs != "undefined") {
        if (returnResult.Result == returnResult.ValueSuccess) {
            if (!isNotShowSuccessMesg) {
                var mesg = "Process " + processName + " finish successfully";
                if (typeof returnResult.SuccMesgs != "undefined" && returnResult.SuccMesgs != null) {
                    for (i = 0; i < returnResult.SuccMesgs.length; i++) {
                        mesg += "<br/>" + returnResult.SuccMesgs[i];
                    }
                }
                if (typeof returnResult.ProcessId != "undefined" && returnResult.ProcessId != null) {
                    //mesg += "<br/> ProcessId = " + returnResult.ProcessId;
                    mesg += ("<br/> ProcessId = <a href='#' onclick='openNewWindow(\"/LoggingMonitoringDetail?process_id=" + returnResult.ProcessId + "\")'>" + returnResult.ProcessId + "</a>");
                }
                $.msgBox({
                    title: "INFORMATION",
                    content: mesg,
                    success: function (result) {
                        if (!isMesgSuccessNotBlocking) {
                            if (funcSuccess && (typeof funcSuccess == "function")) {
                                funcSuccess(returnResult);
                            }
                        }
                    }
                });
            }

            if (isMesgSuccessNotBlocking || isNotShowSuccessMesg) {
                if (funcSuccess && (typeof funcSuccess == "function")) {
                    funcSuccess(returnResult);
                }
            }
        } else {
            if (returnResult.ErrMesgs != null) {
                var errMesg = "Process " + processName + " finish with error : <br/>";
                if (typeof returnResult.ProcessId != "undefined" && returnResult.ProcessId != null) {
                    //errMesg += "ProcessId = " + returnResult.ProcessId + "<br/>";
                    errMesg += ("ProcessId = <a href='#' onclick='openNewWindow(\"/LogMonitoringDtl?processId=" + returnResult.ProcessId + "\")'>" + returnResult.ProcessId + "</a><br/>");
                }
                for (i = 0; i < returnResult.ErrMesgs.length; i++) {
                    errMesg += returnResult.ErrMesgs[i] + "<br/>";
                }
                $.msgBox({
                    title: "ERROR",
                    type: "error",
                    content: errMesg,
                    success: function (result) {
                        if (!isMesgErrorNotBlocking) {
                            if (funcError && (typeof funcError == "function")) {
                                funcError(returnResult);
                            }
                        }
                    }
                });
            }

            if (isMesgErrorNotBlocking) {
                if (funcError && (typeof funcError == "function")) {
                    funcError(returnResult);
                }
            }
        }
    } else {
        alert(returnResult);
        window.location = window.location.origin;
    }
}

function handleAjaxResponseError(returnResult, processName, funcError, isMesgErrorNotBlocking) {
    $.msgBox({
        title: "ERROR",
        //type: "error",
        content: "Process " + processName + " finish with error : <br/> Ajax Request Error : <br/> " + JSON.stringify(returnResult),
        success: function (result) {
            if (!isMesgErrorNotBlocking) {
                if (funcError && (typeof funcError == "function")) {
                    funcError(returnResult);
                }
            }
        }
    });

    if (isMesgErrorNotBlocking) {
        if (funcError && (typeof funcError == "function")) {
            funcError(result);
        }
    }
}

function handleDownloadResponseError(responseHtml, url) {
    window.location = window.location.origin;
}

function showErrMesg(mesg, funcSuccess, title) {
    $.msgBox({
        title: title ? title : "ERROR",
        content: mesg,
        type: "error",
        success: function (result) {
            if (funcSuccess && (typeof funcSuccess == "function")) {
                funcSuccess(result);
            }
        }
    });
}

function showInfMesg(mesg, funcSuccess, title) {
    $.msgBox({
        title: title ? title : "INFORMATION",
        content: mesg,
        type: "info",
        success: function (result) {
            if (funcSuccess && (typeof funcSuccess == "function")) {
                funcSuccess(result);
            }
        }
    });
}

function showConfirmMesg(mesg, funcSuccess, title) {
    $.msgBox({
        title: title ? title : "Are You Sure",
        content: mesg,
        type: "confirm",
        buttons: [{ value: "Yes" }, { value: "No"}],
        success: function (result) {
            if (result == "Yes") {
                if (funcSuccess && (typeof funcSuccess == "function")) {
                    funcSuccess(result);
                }
            }
        }
    });
}

function popUpProgressShow() {
    $.blockUI({
        message: '</br></br><div style="font-weight: bold;font-size: 110%;font-family: Sans-Serif;">Loading</div><div style="font-weight: bold;font-size: 110%;font-family: Sans-Serif;">Please wait...</div></br></br><img src="/Content/Images/loading.gif" /></br></br>',
        css: {
            padding: 0,
            margin: 0,
            width: '22%',
            top: '33%',
            left: '39%',
            border: '1px solid black',
            backgroundColor: '#FFFFFF',
            cursor: 'wait'
        },
        overlayCSS: {
            backgroundColor: '#CCCCCC',
            opacity: 0.6,
            cursor: 'wait'
        },
        baseZ: 99990
    });
}

function popUpProgressHide() {
    $.unblockUI();
}