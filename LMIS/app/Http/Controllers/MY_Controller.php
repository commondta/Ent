<?php

namespace App\Http\Controllers;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Session;
use App\Models\Land_provider;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use App\Models\Agreement;
use App\Models\Challan_form_header;
use App\Models\Conveyance;
use App\Models\Indemnity_bond;
use App\Models\Land_form;
use App\Models\Pictorial_view;
use App\Models\Possession_certificate;
use App\Models\Purchase_of_land;
use App\Models\Registry_document;
use App\Models\Seller_profile;
use App\Models\Approval_stage;
use App\Models\Approval_tree;
use App\Models\User;
use App\Models\Document_approval_history;
use App\Models\Exemption_rate;
use App\Models\Challan_fee;
use App\Models\Exemption_form;
use App\Models\Affidavit_2;
use App\Models\Intimation_letter;
use App\Models\Int_application;
//use Auth;
class MY_Controller extends Controller
{
    public $permissionInfo;
    public function __construct()
    {
        $this->middleware('web'); // Use appropriate middleware
        $this->middleware('auth');
//        $this->get_user();
    }



    public function get_user()
    {
        $loginId = session()->get('user');

        print_r($loginId);exit;
        $data['total_count_approval'] = 0;
        $data['total_count_pending'] = 0;
        $approval_check = Approval_setup_line::where('isDeleted', 0)->where('user', $loginId)->orderBy('id', 'desc')->get();
        if($approval_check){
            $data['total_count_approval'] = 0;
            foreach($approval_check as $single_record){

                $data['Approval_setup_header'] = Approval_setup_header::where('isDeleted', 0)->where('id', $single_record->main)->orderBy('id','desc')->first();

                if($data['Approval_setup_header']['approval'] == 'LP Master Data'){
                    $data['lp_master_data_approvals'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'LP Master Data')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['lp_master_data_record_count'] = $data['lp_master_data_approvals']->total();
                    $data['total_count_approval'] += $data['lp_master_data_record_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Exemption Rate'){

                    $data['exemption_r_approvals'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Exemption Rate')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['exemption_r_count'] = $data['exemption_r_approvals']->total();
                    $data['total_count_approval'] += $data['exemption_r_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Challan Fee'){
                    $data['challan_fee_approvals'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Challan Fee')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['challan_fee_approvals_count'] = $data['challan_fee_approvals']->total();
                    $data['total_count_approval'] += $data['challan_fee_approvals_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Seller Profile'){
                    $data['seller_profile_approvals'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Seller Profile')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['seller_profile_count'] = $data['seller_profile_approvals']->total();
                    $data['total_count_approval'] += $data['seller_profile_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Challan Form'){

                    $data['challan_form_approvals'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Challan Form')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['challan_form_count'] = $data['challan_form_approvals']->total();
                    $data['total_count_approval'] += $data['challan_form_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Land Form Seller'){

                    $data['challan_form_approvals_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Land Form Seller')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['land_form_seller_count'] = $data['challan_form_approvals_approval']->total();
                    $data['total_count_approval'] += $data['land_form_seller_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Purchase of Land'){

                    $data['purchase_of_land_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Purchase of Land')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['purchase_of_land_count'] = $data['purchase_of_land_approval']->total();
                    $data['total_count_approval'] += $data['purchase_of_land_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Possession Certificate'){

                    $data['possession_certificate_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Possession Certificate')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['possession_certificate_count'] = $data['possession_certificate_approval']->total();
                    $data['total_count_approval'] += $data['possession_certificate_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Pictorial View'){

                    $data['pictorial_view_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Pictorial View')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['pictorial_view_count'] = $data['pictorial_view_approval']->total();
                    $data['total_count_approval'] += $data['pictorial_view_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Conveyance Deed'){

                    $data['conveyance_deed_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Conveyance Deed')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['conveyance_deed_count'] = $data['conveyance_deed_approval']->total();
                    $data['total_count_approval'] += $data['conveyance_deed_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Agreement'){

                    $data['agreement_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Agreement')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['agreement_count'] = $data['agreement_approval']->total();
                    $data['total_count_approval'] += $data['agreement_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Registry Document'){
                    $data['registry_document_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Registry Document')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['registry_document_count'] = $data['registry_document_approval']->total();
                    $data['total_count_approval'] += $data['registry_document_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Exemption Form'){
                    $data['exemption_form_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Exemption Form')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['exemption_form_count'] = $data['exemption_form_approval']->total();
                    $data['total_count_approval'] += $data['exemption_form_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Intimation Application'){

                    $data['intimation_application_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Intimation Application')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['intimation_application_count'] = $data['intimation_application_approval']->total();
                    $data['total_count_approval'] += $data['intimation_application_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Intimation Letter'){

                    $data['intimation_letter_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Intimation Letter')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['intimation_letter_count'] = $data['intimation_letter_approval']->total();
                    $data['total_count_approval'] += $data['intimation_letter_count'] ;
                }

            }


        }
        session()->put('total_count_approval', $data['total_count_approval']);

        $data['lp_master_data'] = Land_provider::where('createdBy', $loginId)
            ->where('status', 1)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();
        $data['lp_master_data_record_count'] = $data['lp_master_data']->total();
        $data['total_count_pending'] += $data['lp_master_data_record_count'] ;

        $data['exemption_r'] = Exemption_rate::where('isDeleted', 0)
            ->where('createdBy', $loginId)
            ->where('status', 1)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();

        $data['exemption_r_count'] = $data['exemption_r']->total();
        $data['total_count_pending'] += $data['exemption_r_count'] ;
        $data['challan_fee'] = Challan_fee::where('isDeleted', 0)
            ->where('createdBy', $loginId)
            ->where('status', 1)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();

        $data['challan_fee_approvals_count'] = $data['challan_fee']->total();
        $data['total_count_pending'] += $data['challan_fee_approvals_count'] ;

        $data['seller_profile'] = Seller_profile::where('isDeleted', 0)
            ->where('createdBy', $loginId)
            ->where('status', 1)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();

        $data['seller_profile_count'] = $data['seller_profile']->total();
        $data['total_count_pending'] += $data['seller_profile_count'] ;

        $data['challan_form'] = Challan_form_header::where('isDeleted', 0)
            ->where('createdBy', $loginId)
            ->where('status', 1)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();

        $data['challan_form_count'] = $data['challan_form']->total();
        $data['total_count_pending'] += $data['challan_form_count'] ;

        $data['land_form_seller'] = Land_form::where('isDeleted', 0)
            ->where('createdBy', $loginId)
            ->where('status', 1)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();

        $data['land_form_seller_count'] = $data['land_form_seller']->total();
        $data['total_count_pending'] += $data['land_form_seller_count'] ;

        $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)
            ->where('createdBy', $loginId)
            ->where('status', 1)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();
        $data['purchase_of_land_count'] = $data['purchase_of_land']->total();
        $data['total_count_pending'] += $data['purchase_of_land_count'] ;


        $data['possession_certificate'] = Possession_certificate::where('isDeleted', 0)
            ->where('status', 1)
            ->where('createdBy', $loginId)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();

        $data['possession_certificate_count'] = $data['possession_certificate']->total();
        $data['total_count_pending'] += $data['possession_certificate_count'] ;


        $data['pictorial_view'] = Pictorial_view::where('isDeleted', 0)
            ->where('status', 1)
            ->where('createdBy', $loginId)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();
        $data['pictorial_view_count'] = $data['pictorial_view']->total();
        $data['total_count_pending'] += $data['pictorial_view_count'] ;





        $data['conveyance_deed'] = Conveyance::where('isDeleted', 0)
            ->where('status', 1)
            ->where('createdBy', $loginId)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();

        $data['conveyance_deed_count'] = $data['conveyance_deed']->total();
        $data['total_count_pending'] += $data['conveyance_deed_count'] ;


        $data['agreement'] = Agreement::where('isDeleted', 0)
            ->where('status', 1)
            ->where('createdBy', $loginId)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();

        $data['agreement_count'] = $data['agreement']->total();
        $data['total_count_pending'] += $data['agreement_count'] ;


        $data['registry_document'] = Registry_document::where('isDeleted', 0)
            ->where('status', 1)
            ->where('createdBy', $loginId)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();
        $data['registry_document_count'] = $data['registry_document']->total();
        $data['total_count_pending'] += $data['registry_document_count'] ;


        $data['exemption_form'] = Exemption_form::where('isDeleted', 0)
            ->where('status', 1)
            ->where('createdBy', $loginId)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();
        $data['exemption_form_count'] = $data['exemption_form']->total();
        $data['total_count_pending'] += $data['exemption_form_count'] ;



        $data['intimation_application'] = Int_application::where('isDeleted', 0)
            ->where('status', 1)
            ->where('createdBy', $loginId)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();
        $data['intimation_application_count'] = $data['intimation_application']->total();
        $data['total_count_pending'] += $data['intimation_application_count'] ;





        $data['intimation_letter'] = Intimation_letter::where('isDeleted', 0)
            ->where('status', 1)
            ->where('createdBy', $loginId)
            ->where('isDeleted', 0)
            ->orderBy('id', 'desc')
            ->get();

        $data['intimation_letter_count'] = $data['intimation_letter']->total();
        $data['total_count_pending'] += $data['intimation_letter_count'] ;





        session()->put('total_count_pending', $data['total_count_pending']);









    }

}
