<?php

use Illuminate\Support\Facades\Route;
use App\Http\Controllers\Land_Prdr;
use App\Http\Controllers\Seller;
use App\Http\Controllers\Land_fm;
use App\Http\Controllers\Purchs_L;
use App\Http\Controllers\Exption_r;
use App\Http\Controllers\Challan_f;
use App\Http\Controllers\Posession;
use App\Http\Controllers\Pictorial;
use App\Http\Controllers\Conveyance_d;
use App\Http\Controllers\Agreement_c;
use App\Http\Controllers\Indemnity_c;
use App\Http\Controllers\Registry_c;
use App\Http\Controllers\Exemption_f;
use App\Http\Controllers\Exemption_inventory_c;
use App\Http\Controllers\Affidavit_2_c;
use App\Http\Controllers\Undertaking_c;
use App\Http\Controllers\Int_app_c;
use App\Http\Controllers\Int_ltr_c;
use App\Http\Controllers\UserController;
use App\Http\Controllers\GlobalController;
use App\Http\Controllers\Approvalt;
use App\Http\Controllers\Approval_stages;
use App\Http\Controllers\Approval_setup;
use App\Http\Controllers\Challan_form;
use App\Http\Controllers\MY_Controller;
use App\Http\Controllers\HomeController;

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| contains the "web" middleware group. Now create something great!
|
*/

Route::get('/', function () {
    // ERP platform: there is no LIMS login page any more. A signed-in user (the ErpSso
    // middleware signs them in from the erp_sso cookie) goes straight to work; anyone
    // else goes to the single ERP login on the PMS host.
    if (Auth::check()) {
        return redirect()->route('home');
    }
    if (config('erp.enabled')) {
        return redirect()->away(config('erp.base_url') . config('erp.login_path'));
    }
    return view('auth/login');
});

// ERP platform endpoints (see App\Http\Controllers\ErpController)
Route::get('/erp/touch', [\App\Http\Controllers\ErpController::class, 'touch'])->name('erp.touch');
Route::post('/erp/verify', [\App\Http\Controllers\ErpController::class, 'verify'])->middleware('throttle:30,1')->name('erp.verify');
Route::middleware(['auth'])->group(function () {
//    Route::get('/dashboard', function () {
//          return redirect()->route('land_provider.index');
//    })->name('dashboard');
    // My Home — the landing workspace (same shape as PMS's My Home)
    Route::get('/home', [HomeController::class, 'index'])->name('home');
    Route::get('/dashboard', function () {
        return redirect()->route('home');
    })->name('land_provider');
    Route::resource('land_provider', Land_Prdr::class);
    Route::resource('seller_profile', Seller::class);
    Route::resource('land_form', Land_fm::class);
    Route::resource('purchase_of_land', Purchs_L::class);
    Route::resource('exemption_rate', Exption_r::class);
    Route::resource('challan_fee', Challan_f::class);
    Route::resource('possession_certificate', Posession::class);
    Route::resource('pictorial_view', Pictorial::class);
    Route::resource('conveyance', Conveyance_d::class);
    
    // Conveyance Report Download Routes
    Route::group(['prefix' => 'conveyance', 'as' => 'conveyance.'], function () {
        Route::get('{id}/download-conveyance', [Conveyance_d::class, 'downloadConveyance'])->name('download_conveyance');
        Route::get('{id}/download-agreement', [Conveyance_d::class, 'downloadSaleAgreement'])->name('download_agreement');
        Route::get('{id}/download-affidavit/{loCode?}', [Conveyance_d::class, 'downloadAffidavit'])->name('download_affidavit');
        Route::get('{id}/download-undertaking/{loCode?}', [Conveyance_d::class, 'downloadUndertaking'])->name('download_undertaking');
        Route::get('{id}/download-indemnity-bond/{loCode?}', [Conveyance_d::class, 'downloadIndemnityBond'])->name('download_indemnity_bond');
        Route::get('{id}/view-bundle', [Conveyance_d::class, 'viewDocumentBundle'])->name('view_bundle');
    });
    
    Route::resource('agreement', Agreement_c::class);
    Route::resource('indemnity_bond', Indemnity_c::class);
    Route::resource('registry_document', Registry_c::class);
    Route::resource('exemption_form', Exemption_f::class);
    Route::resource('exemption_inventory', Exemption_inventory_c::class);
    Route::resource('affidavit_2', Affidavit_2_c::class);
    Route::resource('undertaking', Undertaking_c::class);
    Route::resource('intimation_application', Int_app_c::class);
    Route::resource('intimation_letter', Int_ltr_c::class);
    Route::resource('users', UserController::class);
    Route::resource('approval_tree', Approvalt::class);
    Route::resource('approval_stage', Approval_stages::class);
    Route::resource('approval_setup', Approval_setup::class);
    Route::resource('challan_form', Challan_form::class);
//    Route::post('/approval_stage_delete', [Approval_stages::class, 'approval_stage_delete']);
//    Route::post('/approval_stage_delete/{id}', [Approval_stages::class, 'approval_stage_delete'])->name('approval_stage_delete');

    Route::get('/print_pld', [Land_Prdr::class, 'print_pld'])->name('print_pld');;
    Route::post('/get_seller_data', [GlobalController::class, 'get_seller_data']);
    Route::post('/get_land_form_data', [GlobalController::class, 'get_land_form_data']);
    Route::post('/get_land_form', [GlobalController::class, 'get_land_form']);
    Route::post('/get_purchase_of_land', [GlobalController::class, 'get_purchase_of_land']);
    Route::get('/get-land-rate/{doc_no}', [GlobalController::class, 'getLandRate']);
    Route::get('/get-lo-details/{doc_no}', [GlobalController::class, 'getLoDetails']);
    Route::get('/get-land-details/{doc_no}', [GlobalController::class, 'getLandDetails']);
    Route::get('/get-purchase_lo-details/{doc_no}', [GlobalController::class, 'getpurchaseLoDetails']);
    Route::get('/get-purchase-land-details/{doc_no}', [GlobalController::class, 'getpurchaseLandDetails']);
    Route::get('/get-land-form-details/{doc_no}', [Exemption_inventory_c::class, 'getLandFormDetails']);

    Route::post('/get_land_provider', [GlobalController::class, 'get_land_provider']);
    Route::post('/get_possession_record', [GlobalController::class, 'get_possession_record']);
    Route::post('/get_conveyance_deed', [GlobalController::class, 'get_conveyance_deed']);
    Route::post('/get_exemption_form', [GlobalController::class, 'get_exemption_form']);
    Route::post('/get_intimation_application', [GlobalController::class, 'get_intimation_application']);
    Route::post('/approval_status_update', [Approval_setup::class, 'approval_status_update'])->name('approval_status_update');
    Route::get('/approval_inbox/{id}', [Approval_setup::class, 'approval_inbox'])->name('approval_inbox');
    Route::get('/pending_documents/{id}', [Approval_setup::class, 'pending_documents'])->name('pending_documents');
    Route::get('/approved_documents/{id}', [Approval_setup::class, 'approved_documents'])->name('approved_documents');
    Route::get('/rejected_documents/{id}', [Approval_setup::class, 'rejected_documents'])->name('rejected_documents');
    Route::get('/approved_request/{id}/{text}', [Approval_setup::class, 'approved_request'])->name('approved_request');


    Route::post('/approval_document_history', [Approval_setup::class, 'approval_document_history'])->name('approval_document_history');
    Route::post('/approval_document_record', [Approval_setup::class, 'approval_document_record'])->name('approval_document_record');
    Route::post('/approved_docuement_view', [Approval_setup::class, 'approved_docuement_view'])->name('approved_docuement_view');

});
require __DIR__.'/auth.php';
