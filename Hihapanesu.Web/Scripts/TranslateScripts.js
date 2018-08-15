function HihapanesuTranslate(input) {
    var dataObject = {
        Text: input,
        IsTest: false
    };

    var dataJson = JSON.stringify(dataObject);
    var resultData;

    var url = "../api/Translate/TranscribeAndGenerate";
    var jqXHR = $.ajax({
        type: "POST",
        url: url,
        data: dataJson,
        async: false,
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (data) {
        },
        failure: function (errmsg) {
            alert(errMsg);
        }
    });
    var responseText = jqXHR.responseText;
    var responseToString = JSON.parse(responseText);
    //console.log(responseText);

    return responseToString;
}

function SetHihapanesuValue(elementId, html) {
    var resultElem = document.getElementById(elementId);
    resultElem.innerHTML = html;
}