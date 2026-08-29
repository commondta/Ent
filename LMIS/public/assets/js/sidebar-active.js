/**
 * Created by Umer-Khan on 07/08/2023.
 */
$(document).ready(function () {
    //debugger;
    var current_url = window.location.href;
    if (/\/home(\/|\?|#|$)/.test(window.location.pathname)){
        $('.home-nav').addClass("active");
    }
    else if (current_url.indexOf("dashboard") > 0){
        $('.lp-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("land_provider") > 0){
        $('.lp_master-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("exemption_rate") > 0){
        $('.exemption_rate-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("challan_fee") > 0){
        $('.challan_fee-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("seller_profile") > 0){
        $('.seller_profile-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("seller_profile") > 0){
        $('.seller_profile-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("land_form") > 0){
        $('.land_form-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("purchase_of_land") > 0){
        $('.purchase_of_land-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("possession_certificate") > 0){
        $('.possession_certificate-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("pictorial_view") > 0){
        $('.pictorial_view-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("conveyance") > 0){
        $('.conveyance-nav,.registry').addClass("active");
    }
    else if (current_url.indexOf("agreement") > 0){
        $('.agreement-nav,.registry').addClass("active");
    }
    else if (current_url.indexOf("indemnity_bond") > 0){
        $('.indemnity_bond-nav,.registry').addClass("active");
    }
    else if (current_url.indexOf("registry_document") > 0){
        $('.registry_document-nav,.registry').addClass("active");
    }
    else if (current_url.indexOf("exemption_inventory") > 0){
        $('.exemption_inventory-nav,.purchasing_of_land').addClass("active");
    }
    else if (current_url.indexOf("exemption_form") > 0){
        $('.exemption_form-nav,.exemption').addClass("active");
    }
    else if (current_url.indexOf("affidavit_2") > 0){
        $('.affidavit_2-nav,.exemption').addClass("active");
    }
    else if (current_url.indexOf("intimation_application") > 0){
        $('.intimation_application-nav,.intimation').addClass("active");

    }
    else if (current_url.indexOf("intimation_letter") > 0){
        $('.intimation_letter-nav,.intimation').addClass("active");
    }





});