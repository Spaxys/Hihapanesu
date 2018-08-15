function HihapanesuTranslate(input) {
    debugger;
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
            debugger;
            //resultData = data.responseText
        },
        failure: function (errmsg) {
            debugger;
            alert(errMsg);
        }
    });
    var responseText = jqXHR.responseText;
    console.log(responseText);

    return responseText;

    //$.ajax({
    //    type: "POST",
    //    method: "POST",
    //    url: "api/Translate/TranscribeAndGenerate",
    //    data: dataObject,
    //    contentType: "application/json; charset=utf-8",
    //    success: function (data) {
    //        debugger;
    //        resultData = data
    //    },
    //    failure: function (errmsg) {
    //        debugger;
    //        alert(errMsg);
    //    }
    //});

    //$.ajax({
    //    type: "POST",
    //    method: "POST",
    //    url: "api/Translate/TranscribeAndGenerate",
    //    data: dataObject,
    //    contentType: "application/json; charset=utf-8",
    //    success: function (data) {
    //        debugger;
    //        resultData = data
    //    },
    //    failure: function (errmsg) {
    //        debugger;
    //        alert(errMsg);
    //    }
    //});

    //$.ajax({
    //    method: "POST",
    //    url: url,
    //    data: dataObject,
    //    success: function (data) {
    //        console.log(data);
    //        debugger;
    //        resultData = data;
    //    },
    //    failure: function (errmsg) {
    //        debugger;
    //        alert(errMsg);
    //    }
    //});

    //jQuery.post('api/Translate/TranscribeAndGenerate', dataJson, function (data) {
    //    resultData = data
    //}, 'json');


    //Vanilla Js ajax version
    //var xhttp = new XMLHttpRequest();


    //return resultData;

    //var boldResult = "<b>" + input + "</b>";
    //return boldResult;
}

function SetHihapanesuValue(elementId, html) {
    debugger;
    var resultElem = document.getElementById(elementId);
    resultElem.innerHTML = html;
}