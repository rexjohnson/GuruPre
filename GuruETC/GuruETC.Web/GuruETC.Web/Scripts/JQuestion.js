$(function () {
    $('input').iCheck({
        checkboxClass: 'icheckbox_futurico',
        radioClass: 'iradio_futurico',
        labelHover: true
    });
});


function getFirst() {
    $('#content_wrapper').height($('#dvQuestionPart2').height());

    $('#dvQuestionPart2').show();
    $('#content_wrapper').show();
    $('#dvQuestionPart1').hide();
    $('#dvQuestionPart3').hide();
    $('#dvQuestionPart6').hide();
    $('#dvQuestionPart7').hide();

    var patientConcernsCheckedItems = "";
    var duplicateDentalCheckedItems = "";
    var duplicateENTCheckedItems = "";
    var duplicateSleepCheckedItems = "";

    $('div#dvQuestionPart1 input:[type="checkbox"]:checked').each(function () {
        var getid = $(this).attr('value');
        patientConcernsCheckedItems += getid + "|";

        //duplicate dental check
        if ($(this).hasClass('duplicateDental')) {
            duplicateDentalCheckedItems += getid + "|";
        }
        //duplicate ENT check
        if ($(this).hasClass('duplicateENT')) {
            duplicateENTCheckedItems += getid + "|";
        }
        //duplicate Sleep check
        if ($(this).hasClass('duplicateSleep')) {
            duplicateSleepCheckedItems += getid + "|";
        }

    });

    patientConcernsCheckedItems = patientConcernsCheckedItems.substring(0, patientConcernsCheckedItems.length - 1);
    duplicateDentalCheckedItems = duplicateDentalCheckedItems.substring(0, duplicateDentalCheckedItems.length - 1);
    duplicateENTCheckedItems = duplicateENTCheckedItems.substring(0, duplicateENTCheckedItems.length - 1);
    duplicateSleepCheckedItems = duplicateSleepCheckedItems.substring(0, duplicateSleepCheckedItems.length - 1);



    var sendData = {
        hxConcerns: patientConcernsCheckedItems,
        duplicateDental: duplicateDentalCheckedItems,
        duplicateENT: duplicateENTCheckedItems,
        duplicateSleep: duplicateSleepCheckedItems
    };

    var sendData = JSON.stringify(sendData);

    $.ajax({
        url: '/Question/SavePatientHealthInfo',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: sendData,
        success: function (msg) {
            if (msg !== null) {
                var retval = Array();
                retval = msg.split('|');
                $('div#dvQuestionPart2 input:[type="checkbox"]').each(function () {
                    var getid = $(this).attr('value');
                    if ($.inArray(getid, retval) !== -1) {
                        $(this).prop('checked', true);
                    }
                });
            }
            $('input').iCheck({
                checkboxClass: 'icheckbox_futurico',
                radioClass: 'iradio_futurico',
                labelHover: true
            });
            $('#content_wrapper').hide();
        }
    });




    //    $('div#dvQuestionPart2 input:[type="checkbox"]').each(function () {
    //        var getid = $(this).attr('id');
    //        if ($.inArray(getid, AllAnswer) !== -1) {
    //            $('#' + getid).prop('checked', true);

    //        }
    //    });

    //    $.ajax({
    //        url: '/Question/GetRefreshInfo',
    //        type: 'POST',
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (msg) {
    //            $('input').iCheck({
    //                checkboxClass: 'icheckbox_futurico',
    //                radioClass: 'iradio_futurico',
    //                labelHover: true
    //            });
    //            $('#content_wrapper').hide();
    //        }
    //    });


}

function getBackSecond() {
    $('#dvQuestionPart1').show();
    $('#dvQuestionPart2').hide();
    $('#dvQuestionPart3').hide();
    $('#dvQuestionPart6').hide();
    $('#dvQuestionPart7').hide();
}

function getSecond() {
    $('#content_wrapper').height($('#dvQuestionPart3').height());
    $('#content_wrapper').show();
    $('#dvQuestionPart7').hide();
    $('#dvQuestionPart1').hide();
    $('#dvQuestionPart2').hide();
    $('#dvQuestionPart3').show();
    $('#dvQuestionPart6').hide();



    var hxMedicationsCheckedItems = "";
    var hxCategories = "";
    //    var hxPrescriptionMedications = "";

    $('div#dvQuestionPart2 input:[type="checkbox"]:checked').each(function () {
        var getid = $(this).attr('value');
        var dataitem = $(this).attr('name');
        hxMedicationsCheckedItems += getid + "|";
        hxCategories += dataitem + "|";
    });

    hxMedicationsCheckedItems = hxMedicationsCheckedItems.substring(0, hxMedicationsCheckedItems.length - 1);
    hxCategoriesCheckedItems = hxCategories.substring(0, hxCategories.length - 1);
    var sendData = {
        hxMedication: hxMedicationsCheckedItems,
        hxCategories: hxCategoriesCheckedItems
    };

    var sendData = JSON.stringify(sendData);

    $.ajax({
        url: '/Question/SavePatientMedicationInfo',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: sendData,
        success: function (msg) {

            if (msg.GHealth !== null) {
                var ghealth = [];
                ghealth = (msg.GHealth).split('|');
                $('div#dvQuestionPart3 input:[name="General Health"]').each(function () {
                    var getid = $(this).attr('value');

                    if ($.inArray(getid, ghealth) !== -1) {
                        $(this).prop('checked', true);

                    }

                });
                $('div#dvQuestionPart3 input:[name="Family Disease"]').each(function () {
                    var getid = $(this).attr('value');
                    if ($.inArray(getid, ghealth) !== -1) {
                        $(this).prop('checked', true);

                    }
                });

                $('div#dvQuestionPart3 input:[name="Disabilities"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, ghealth) !== -1) {
                        $(this).prop('checked', true);

                    }
                });
            }

            if (msg.Nutri !== null) {
                var gnutri = [];
                gnutri = (msg.Nutri).split('|');

                $('div#dvQuestionPart3 input:[name="Nutrition & Lifestyle"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, gnutri) !== -1) {
                        $(this).prop('checked', true);

                    }
                });
            }
            if (msg.Vital !== null) {
                var gvital = [];
                gvital = (msg.Vital).split('|');
                $('div#dvQuestionPart3 input:[name="Vital Measurements"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, gvital) !== -1) {
                        $(this).prop('checked', true);

                    }
                });
            }

            if (msg.Tbc !== null) {
                var gtbc = [];
                gtbc = (msg.Tbc).split('|');
                $('div#dvQuestionPart3 input:[name="Tobacco, Alcohol & Drugs"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, gtbc) !== -1) {
                        $(this).prop('checked', true);

                    }
                });
            }

            if (msg.Extra !== null) {
                var retl = msg.Extra;
                var retval = retl.split('|')
                $('#txtWeight').val(retval[0]);
                $('#txtHeight').val(retval[1]);
                $('#txtFeet').val(retval[2]);
                $('#drppacks').val(retval[3]);
                $('#drpcans').val(retval[4]);
                $('#inpformersmoker').val(retval[5]);
                $('#inpformertobacco').val(retval[6]);
                $('#hddiet').val(retval[7]);
            }

            $('input[type="radio"]').each(function () {
                var getfun = $('#hddiet').val();
                var getval = $(this).val();

                if (getval == retval[7]) {
                    $(this).attr('checked', true);

                }

            });

            $('input').iCheck({
                checkboxClass: 'icheckbox_futurico',
                radioClass: 'iradio_futurico',
                labelHover: true
            });
            $('input[dir="1"]').each(function () {
                var getfun = $(this).attr('onclick');
                var ischecked = $(this).attr('checked');
                if (ischecked == 'checked') {
                    $(this).parent().parent().find('div:eq(1)').show();
                }
                $(this).next('.iCheck-helper').attr('onclick', getfun);
            });


            $('#content_wrapper').hide();

            // Here to get the Result of General Health, nutrition page.
            // it can be return by a class where different properties like General Health, nutrition page in string form as like others also.

        }
    });




    //    $('div#dvQuestionPart3 input:[type="checkbox"]').each(function () {
    //        var getid = $(this).attr('id');
    //        if ($.inArray(getid, AllAnswer) !== -1) {
    //            $('#' + getid).prop('checked', true);
    //            //                            $('input').iCheck({
    //            //                                checkboxClass: 'icheckbox_futurico',
    //            //                                radioClass: 'iradio_futurico',
    //            //                                labelHover: true
    //            //                            });
    //        }
    //    });


    //    $.ajax({
    //        url: '/Question/GetPatientVitalInfo',
    //        type: 'POST',
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (msg) {

    //            /*get the checked question of patient */
    //            

    //            
    //        }
    //    });


}

function getBackThird() {
    $('#dvQuestionPart2').show();
    $('#dvQuestionPart1').hide();
    $('#dvQuestionPart3').hide();
    $('#dvQuestionPart6').hide();
    $('#dvQuestionPart7').hide();
}

function getThird() {
    $('#content_wrapper').height($('#dvQuestionPart6').height());
    $('#content_wrapper').show();
    $('#dvQuestionPart1').hide();
    $('#dvQuestionPart3').hide();
    $('#dvQuestionPart2').hide();
    $('#dvQuestionPart6').show();
    $('#dvQuestionPart7').hide();

    var hxGeneralCheckedItems = "";
    var hxVitalsLabsCheckedItems = "";
    var hxNutritionActivityCheckedItems = "";
    var hxTobaccoAlcoholCheckedItems = "";

    $('div#dvQuestionPart3 input:[name="General Health"]:checked').each(function () {
        hxGeneralCheckedItems += $(this).val() + "|";
    });
    $('div#dvQuestionPart3 input:[name="Family Disease"]:checked').each(function () {
        hxGeneralCheckedItems += $(this).val() + "|";
    });
    $('div#dvQuestionPart3 input:[name="Disabilities"]:checked').each(function () {
        hxGeneralCheckedItems += $(this).val() + "|";
    });

    $('div#dvQuestionPart3 input:[name="Nutrition & Lifestyle"]:checked').each(function () {
        hxNutritionActivityCheckedItems += $(this).val() + "|";
    });

    $('div#dvQuestionPart3 input:[name="Vital Measurements"]:checked').each(function () {
        hxVitalsLabsCheckedItems += $(this).val() + "|";
    });
    $('div#dvQuestionPart3 input:[name="Tobacco, Alcohol & Drugs"]:checked').each(function () {
        hxTobaccoAlcoholCheckedItems += $(this).val() + "|";
    });

    var diet = $("div#dvQuestionPart3 input[name='diet_rating']:checked").val();


    hxGeneralCheckedItems = hxGeneralCheckedItems.substring(0, hxGeneralCheckedItems.length - 1);
    hxVitalsLabsCheckedItems = hxVitalsLabsCheckedItems.substring(0, hxVitalsLabsCheckedItems.length - 1);
    hxNutritionActivityCheckedItems = hxNutritionActivityCheckedItems.substring(0, hxNutritionActivityCheckedItems.length - 1);
    hxTobaccoAlcoholCheckedItems = hxTobaccoAlcoholCheckedItems.substring(0, hxTobaccoAlcoholCheckedItems.length - 1);

    var weight = $('#txtWeight').val();
    var heightFeet = $('#txtHeight').val();
    var heightInches = $('#txtFeet').val();
    if ((heightFeet != '') && (heightInches)) {
        height = (parseInt(heightFeet) * 12) + parseInt(heightInches); //Get total height in inches
    }

    var formerSmokerQuitDate = $('#inpformersmoker').val();
    var formerChewerQuitDate = $('#inpformertobacco').val();
    var packs = $('#drpcans').val();
    var sendData = {
        hxGeneral: hxGeneralCheckedItems,
        hxutrition: hxNutritionActivityCheckedItems,
        hxVitalsLabs: hxVitalsLabsCheckedItems,
        hxTobacco: hxTobaccoAlcoholCheckedItems,
        wet: weight,
        hgfeet: heightFeet,
        hginc: heightInches,
        fsmoke: formerSmokerQuitDate,
        fchewer: formerChewerQuitDate,
        drate: diet
    };

    var sendData = JSON.stringify(sendData);

    $.ajax({
        url: '/Question/SavePatientGeneralLifestyle',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: sendData,
        success: function (msg) {
            if (msg.cardio !== null) {
                var Cardio = [];
                Cardio = (msg.cardio).split('|');
                $('div#dvQuestionPart6 input:[name="Cardiovascular"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, Cardio) !== -1) {
                        $(this).prop('checked', true);

                    }
                });
            }

            if (msg.sleep !== null) {
                var gsleep = [];
                gsleep = (msg.sleep).split('|');

                $('div#dvQuestionPart6 input:[name="Sleep"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, gsleep) !== -1) {
                        $(this).prop('checked', true);

                    }
                });
            }

            if (msg.ENT !== null) {
                var gENT = [];
                gENT = (msg.ENT).split('|');
                $('div#dvQuestionPart6 input:[name="ENT"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, gENT) !== -1) {

                        $(this).prop('checked', true);

                    }
                });
            }

            if (msg.DIS !== null) {
                var gdis = [];
                gdis = (msg.DIS).split('|');
                $('div#dvQuestionPart6 input:[name="Other Disease & Conditions"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, gdis) !== -1) {
                        $(this).prop('checked', true);

                    }
                });
            }

            if (msg.Cancer !== null) {
                var gCancer = [];
                gCancer = (msg.Cancer).split('|');
             
                $('div#dvQuestionPart6 input:[name="Cancer"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, gCancer) !== -1) {
                     
                        $(this).prop('checked', true);

                    }
                });
            }

            if (msg.GHealth !== null) {
                var gheal = [];
                gheal = (msg.GHealth).split('|');
                $('div#dvQuestionPart6 input:[name="Gender Health"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, gheal) !== -1) {
                        $(this).prop('checked', true);

                    }
                });
            }

            if (msg.Endo !== null) {
                var gEndo = [];
                gEndo = (msg.Endo).split('|');
                $('div#dvQuestionPart6 input:[name="Endocrine Disorders"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, gEndo) !== -1) {
                        $(this).prop('checked', true);

                    }
                });
            }

            $('input').iCheck({
                checkboxClass: 'icheckbox_futurico',
                radioClass: 'iradio_futurico',
                labelHover: true
            });

            $('input[dir="1"]').each(function () {
                var getfun = $(this).attr('onclick');
                var ischecked = $(this).attr('checked');
                if (ischecked == 'checked') {
                    $(this).parent().parent().find('div:eq(1)').show();
                }
                $(this).next('.iCheck-helper').attr('onclick', getfun);
            });


            $('#content_wrapper').hide();
        }
    });


    //    $('div#dvQuestionPart6 input:[type="checkbox"]').each(function () {
    //        var getid = $(this).attr('id');
    //        if ($.inArray(getid, AllAnswer) !== -1) {
    //            $('#' + getid).prop('checked', true);

    //        }

    //    });

    //    $.ajax({
    //        url: '/Question/GetRefreshInfo',
    //        type: 'POST',
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (msg) {
    //            $('input').iCheck({
    //                checkboxClass: 'icheckbox_futurico',
    //                radioClass: 'iradio_futurico',
    //                labelHover: true
    //            });
    //            $('#content_wrapper').hide();
    //        }
    //    });

    //  $('#content_wrapper').hide();
}
function getBackForth() {
    $('#dvQuestionPart2').hide();
    $('#dvQuestionPart1').hide();
    $('#dvQuestionPart3').show();
    $('#dvQuestionPart6').hide();
    $('#dvQuestionPart7').hide();

//    $('input[dir="1"]').each(function () {
//        var getfun = $(this).attr('onclick');
//        var ischecked = $(this).attr('checked');
//        if (ischecked == 'checked') {
//            $(this).parent().parent().find('div:eq(1)').show();
//        }
//        $(this).next('.iCheck-helper').attr('onclick', getfun);
//    });

//    $('input').iCheck({
//        checkboxClass: 'icheckbox_futurico',
//        radioClass: 'iradio_futurico',
//        labelHover: true
//    });
}

function getForth() {
    $('#content_wrapper').show();
    $('#dvQuestionPart1').hide();
    $('#dvQuestionPart3').hide();
    $('#dvQuestionPart2').hide();
    $('#dvQuestionPart6').hide();
    $('#dvQuestionPart7').show();


    var hxCardiovascularCheckedItems = "";
    var hxEndocrineCheckedItems = "";
    var hxCancerCheckedItems = "";
    var hxEntCheckedItems = "";
    var hxSleepCheckedItems = "";
    var hxOtherDiseasesCheckedItems = "";
    var hxGenderHealthCheckedItems = "";

    $('div#dvQuestionPart6 input:[name="Cardiovascular"]:checked').each(function () {
        hxCardiovascularCheckedItems += $(this).val() + "|";
    });

    $('div#dvQuestionPart6 input:[name="Sleep"]:checked').each(function () {
        hxSleepCheckedItems += $(this).val() + "|";
    });

    $('div#dvQuestionPart6 input:[name="Endocrine Disorders"]:checked').each(function () {
        hxEndocrineCheckedItems += $(this).val() + "|";
    });

    $('div#dvQuestionPart6 input:[name="Cancer"]:checked').each(function () {
        hxCancerCheckedItems += $(this).val() + "|";
    });

    $('div#dvQuestionPart6 input:[name="ENT"]:checked').each(function () {
        hxEntCheckedItems += $(this).val() + "|";
    });

    $('div#dvQuestionPart6 input:[name="Other Disease & Conditions"]:checked').each(function () {
        hxOtherDiseasesCheckedItems += $(this).val() + "|";
    });

    $('div#dvQuestionPart6 input:[name="Gender Health"]:checked').each(function () {
        hxGenderHealthCheckedItems += $(this).val() + "|";
    });

    hxCardiovascularCheckedItems = hxCardiovascularCheckedItems.substring(0, hxCardiovascularCheckedItems.length - 1);
    hxEndocrineCheckedItems = hxEndocrineCheckedItems.substring(0, hxEndocrineCheckedItems.length - 1);
    hxCancerCheckedItems = hxCancerCheckedItems.substring(0, hxCancerCheckedItems.length - 1);
    hxEntCheckedItems = hxEntCheckedItems.substring(0, hxEntCheckedItems.length - 1);
    hxSleepCheckedItems = hxSleepCheckedItems.substring(0, hxSleepCheckedItems.length - 1);
    hxOtherDiseasesCheckedItems = hxOtherDiseasesCheckedItems.substring(0, hxOtherDiseasesCheckedItems.length - 1);
    hxGenderHealthCheckedItems = hxGenderHealthCheckedItems.substring(0, hxGenderHealthCheckedItems.length - 1);

    var diabetes = $("div#dvQuestionPart6 input[name='diabetes_type']:checked").val();

    var sendData = {
        hxCardiovascular: hxCardiovascularCheckedItems,
        hxEndocrine: hxEndocrineCheckedItems,
        hxCancer: hxCancerCheckedItems,
        hxEnt: hxEntCheckedItems,
        hxSleep: hxSleepCheckedItems,
        hxOther: hxOtherDiseasesCheckedItems,
        hxGender: hxGenderHealthCheckedItems,
        diab: diabetes

    };

    var sendData = JSON.stringify(sendData);

    $.ajax({
        url: '/Question/SaveCardio',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: sendData,
        success: function (msg) {
            if (msg !== null) {
                var gdental = [];
                gdental = msg.split('|');
                $('div#dvQuestionPart7 input:[name="Dental Health"]').each(function () {
                    var getid = $(this).attr('value');
                    if (getid.indexOf('|') !== -1) {
                        var valsplit = getid.split('|');
                        getid = valsplit[1];
                    }
                    if ($.inArray(getid, gdental) !== -1) {
                        $(this).prop('checked', true);

                    }
                });
            }
            $('input').iCheck({
                checkboxClass: 'icheckbox_futurico',
                radioClass: 'iradio_futurico',
                labelHover: true
            });

            $('input[dir="1"]').each(function () {
                var getfun = $(this).attr('onclick');
                var ischecked = $(this).attr('checked');
                if (ischecked == 'checked') {
                    $(this).parent().parent().find('div:eq(1)').show();
                }
                $(this).next('.iCheck-helper').attr('onclick', getfun);
            });


            $('#content_wrapper').hide();
        }
    });



//    $('div#dvQuestionPart7 input:[type="checkbox"]').each(function () {
//        var getid = $(this).attr('id');
//        if ($.inArray(getid, AllAnswer) !== -1) {
//            $('#' + getid).prop('checked', true);

//        }

//    });


//    $.ajax({
//        url: '/Question/GetRefreshInfo',
//        type: 'POST',
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (msg) {
//            $('input').iCheck({
//                checkboxClass: 'icheckbox_futurico',
//                radioClass: 'iradio_futurico',
//                labelHover: true
//            });
//            $('#content_wrapper').hide();
//        }
//    });


    // $('#content_wrapper').hide();
}

function getBackFifth() {
    $('#dvQuestionPart2').hide();
    $('#dvQuestionPart1').hide();
    $('#dvQuestionPart3').hide();
    $('#dvQuestionPart6').show();
    $('#dvQuestionPart7').hide();
}

function getFinish() {
    $('#dvQuestionPart1').hide();
    $('#dvQuestionPart3').hide();
    $('#dvQuestionPart2').hide();
    $('#dvQuestionPart6').hide();
    $('#dvQuestionPart7').show();
    //  var getcheckedelem = [];

    var hxDentalCheckedItems = "";
    $('div#dvQuestionPart7 input:[name="Dental Health"]:checked').each(function () {
        hxDentalCheckedItems += $(this).val() + "|";
    });

    hxDentalCheckedItems = hxDentalCheckedItems.substring(0, hxDentalCheckedItems.length - 1);

    var sendData = {
        hxDental: hxDentalCheckedItems
    };

    var sendData = JSON.stringify(sendData);

    $.ajax({
        url: '/Question/SaveDental',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: sendData,
        success: function (msg) {
            window.location.href = '/Question/result';
        }
    });

    //    $('div#Wrapper input:[type="checkbox"]:checked').each(function () {
    //        var getid = $(this).attr('value');
    //        var catatt = $(this).attr('data-item');
    //        getcheckedelem.push({ value: getid, CatId: catatt });

    //    });
    //    var postData = JSON.stringify(getcheckedelem);
    //    $.ajax({
    //        url: '/Question/PostResult',
    //        type: 'POST',
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        data:postData,
    //        success: function (msg) {
    ////            $('input').iCheck({
    ////                checkboxClass: 'icheckbox_futurico',
    ////                radioClass: 'iradio_futurico',
    ////                labelHover: true
    ////            });
    //            $('#content_wrapper').hide();
    //        }
    //    });

}

function togglediv(id) {
    if (id == 'Addmeditation') {
        $('#Addmeditation').toggle();
        $('#AddSupliment').hide();
        $('#AddHospital').hide();
    }
    else if (id == 'AddSupliment') {
        $('#AddSupliment').toggle();
        $('#Addmeditation').hide();
        $('#AddHospital').hide();
    }
    else if (id == 'AddHospital') {
        $('#AddHospital').toggle();
        $('#AddSupliment').hide();
        $('#Addmeditation').hide();
    }

}

function savetd(id) {
    var date = new Date();
    var month = date.getMonth() + 1;
    var day = date.getDate();

    var output = date.getFullYear() + '/' + (('' + month).length < 2 ? '0' : '') + month + '/' + (('' + day).length < 2 ? '0' : '') + day;

    if (id == 'txtmeditation') {
        var getmed = $('#txtmeditation').val();
        if ($.trim(getmed) != '') {
            var obj = {
                key: "1",
                title: $.trim(getmed)
            };
            var json = JSON.stringify(obj);

            $.ajax({
                url: '/Question/SavePatientInfo',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: json,
                success: function (msg) {
                    var trId = "tr_" + msg;
                    var row = $('<tr id="' + trId + '"><td style="width:80%"><span>' + $.trim(getmed) + '</span> </td><td style="width:10%">' + output + ' </td><td style="width:10%"><a href="javascript:void(0);" id="' + msg + '" onclick=delpinfo("' + msg + '","meditation")><img src="/Images/cancel.png" alt="delete" /></a> </td></tr>');
                    $('#meditation').append($(row));
                    $('#Addmeditation').hide();
                    $('#txtmeditation').val('');
                }
            });
        }
    }
    else if (id == 'txtSupliment') {
        var getmed = $('#txtSupliment').val();
        if ($.trim(getmed) != '') {
            var obj = {
                key: "2",
                title: $.trim(getmed)
            };
            var json = JSON.stringify(obj);

            $.ajax({
                url: '/Question/SavePatientInfo',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: json,
                success: function (msg) {
                    var trId = "tr_" + msg;
                    var row = $('<tr id="' + trId + '"><td style="width:80%"><span>' + $.trim(getmed) + '</span> </td><td style="width:10%">' + output + ' </td><td style="width:10%"><a href="javascript:void(0);" id="' + msg + '" onclick=delpinfo("' + msg + '","Supliment")><img src="/Images/cancel.png" alt="delete" /></a> </td></tr>');
                    $('#Supliment').append($(row));
                    $('#AddSupliment').hide();
                    $('#txtSupliment').val('');
                }
            });
        }
    }
    else if (id == 'txtHospital') {
        var getmed = $('#txtHospital').val();
        if ($.trim(getmed) != '') {
            var obj = {
                key: "3",
                title: $.trim(getmed)
            };
            var json = JSON.stringify(obj);

            $.ajax({
                url: '/Question/SavePatientInfo',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: json,
                success: function (msg) {
                    var trId = "tr_" + msg;
                    var row = $('<tr id="' + trId + '"><td style="width:80%"><span>' + $.trim(getmed) + '</span> </td><td style="width:10%">' + output + ' </td><td style="width:10%"><a href="javascript:void(0);" id="' + msg + '" onclick=delpinfo("' + msg + '","Hospital")><img src="/Images/cancel.png" alt="delete" /></a> </td></tr>');
                    $('#Hospital').append($(row));
                    $('#AddHospital').hide();
                    $('#txtHospital').val('');
                }
            });
        }
    }
}

function delpinfo(id, frm) {
    if (frm == 'meditation') {
        var obj = {
            key: "1",
            delId: id
        };
        var json = JSON.stringify(obj);

        $.ajax({
            url: '/Question/DeletePatientInfo',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: json,
            success: function (msg) {
                var tdID = "tr_" + id;
                $("#meditation tr:[id='" + tdID + "']").remove();
            }
        });

    }
    else if (frm == 'Supliment') {

        var obj = {
            key: "2",
            delId: id
        };
        var json = JSON.stringify(obj);

        $.ajax({
            url: '/Question/DeletePatientInfo',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: json,
            success: function (msg) {
                var tdID = "tr_" + id;
                $("#Supliment tr:[id='" + tdID + "']").remove();
            }
        });
    }
    else if (frm == 'Hospital') {
        var obj = {
            key: "3",
            delId: id
        };
        var json = JSON.stringify(obj);

        $.ajax({
            url: '/Question/DeletePatientInfo',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: json,
            success: function (msg) {
                var tdID = "tr_" + id;
                $("#Hospital tr:[id='" + tdID + "']").remove();
            }
        });

    }
}

function csmoker() {

    $('#dvcsmoker').toggle();
}

function fsmoker() {
    $('#dvfsmoker').toggle();
}

function ctobacco() {
    $('#dvctobacco').toggle();
}

function ftobacco() {
    $('#dvftobacco').toggle();
}

function cDiabetes() {
    $('#dvcDiabetes').toggle();
}
