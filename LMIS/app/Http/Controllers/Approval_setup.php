<?php

namespace App\Http\Controllers;

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
use Illuminate\Http\Request;
use App\Models\Approval_stage;
use App\Models\Approval_tree;
use App\Models\User;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Land_provider;
use App\Models\Document_approval;
use App\Models\Document_approval_history;
use App\Models\Exemption_rate;
use App\Models\Challan_fee;
use App\Models\Exemption_form;
use App\Models\Affidavit_2;
use App\Models\Intimation_letter;
use App\Models\Int_application;
use App\Models\Exemption_inventory_approval;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;

class Approval_setup extends Controller
{

    public function index()
    {
        if(auth()->user()->is_admin == 1){
            $data['record'] = Approval_setup_header::where('isDeleted', 0)->orderBy('id','desc')->get();
            return view('pages.approvals.setup.show',$data);
        }else{
            return view('pages.authrization.show');


        }
    }
    public function create()
    {
        if(auth()->user()->is_admin == 1){
            $data['stages'] = Approval_stage::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            $data['users'] = User::where('isDeleted', 0)->where('is_admin', 0)->orderBy('id', 'desc')->get();

            $data['tree'] = Approval_tree::where('isDeleted', 0)->orderBy('id', 'desc')->first();
            $data['exists_approvals'] = Approval_setup_header::where('isDeleted', 0)->orderBy('id', 'desc')->get();

//                echo '<pre>';   print_r($data['exists_approvals']);exit;
            return view('pages.approvals.setup.add',$data);

        }else{
            return view('pages.authrization.show');
        }
    }
    public function store(Request $request)
    {
        $request->validate([
            'approval' => 'required',
            'stage' => 'required',
            'no_of_approvals' => 'required',
        ]);




        $record = new Approval_setup_header();
        $record->approval = $request->approval;
        $record->stage = $request->stage;
        $record->no_of_approvals = $request->no_of_approvals;

        $record->save();
        $lastid = $record->id;

        $line_items = $request->item_lines;
        $count = 1;
        foreach ($line_items as $line_item) {
            if ($line_item['user']) {

                $childRecord = new Approval_setup_line();
                $childRecord->main = $lastid;
                $childRecord->priority = $count;
                $childRecord->user =  $line_item['user'];
                $childRecord->designation =  $line_item['designation'];
                $childRecord->save();
                $count++;
            }
        }







        return redirect()->route('approval_setup.index')
            ->with('success', 'Approval Setup has been created successfully.');
    }
    public function show($id)
    {
        //
    }
    public function edit($id)
    {
            $data['approval_setup_header'] = Approval_setup_header::get_records(['approval_setup_headers.id' => $id]);



            if ($data['approval_setup_header']->main) {
                $id = $data['approval_setup_header']->main;

                $rows = Approval_setup_line::where('main', $id)->get();

                $data['approval_setup_header']->rows = $rows->toArray();
                $data['tree'] = Approval_tree::where('isDeleted', 0)->orderBy('id', 'desc')->first();

                $data['stages'] = Approval_stage::where('isDeleted', 0)->orderBy('id', 'desc')->get();
                $data['users'] = User::where('isDeleted', 0)->where('is_admin', 0)->orderBy('id', 'desc')->get();
            }

        return view('pages.approvals.setup.edit', $data);

    }

    public function approval_document_history(Request $request){
        $loginId = $request->id;
        $document_name = $request->approval;

        $record = Document_approval_history::get_recordss(['document_approval_histories.document_id' => $loginId,'document_name' => $document_name]);

        return response()->json(['message' => 'Document has been ','record' =>$record ], 200);

    }
    public function approval_document_record(Request $request){
        $loginId = $request->id;
        $document_name = $request->approval;

        $record = Document_approval::get_recordss(['document_approvals.document_id' => $loginId,'document_name' => $document_name]);

        return response()->json(['message' => 'Document has been ','record' =>$record ], 200);

    }
    public function approved_request($id,$table){

        $document_record = Document_approval::where('isDeleted', 0)
            ->where('document_id', $id)
            ->where('status', 2)
            ->where('document_name', $table)
            ->orderBy('id', 'asc')
            ->first();

        $document_record->priority = 1;
        $document_record->status = 0;
//        $insert_approval_history->remarks = $request->remarks;
        $document_record->save();





        $loginId = auth()->user()->id;

        $insert_approval_history = new Document_approval_history();
        $insert_approval_history->document_name = $table;
        $insert_approval_history->document_id =  $id;
        $insert_approval_history->approval_user_id =  $loginId;
        $insert_approval_history->status = 1;
//        $insert_approval_history->remarks = $request->remarks;
        $insert_approval_history->save();



        if($table == 'LP Master Data'){
            $header = Land_provider::find($id);
        }if($table == 'Exemption Rate'){
            $header = Exemption_rate::find($id);
        }if($table == 'Challan Fee'){
            $header = Challan_fee::find($id);
        }if($table == 'Seller Profile'){
            $header = Seller_profile::find($id);
        }if($table == 'Challan Form'){
            $header = Challan_form_header::find($id);
        }if($table == 'Land Form Seller'){
            $header = Land_form::find($id);
        }if($table == 'Purchase of Land'){
            $header = Purchase_of_land::find($id);
        }if($table == 'Possession Certificate'){
            $header = Possession_certificate::find($id);
        }if($table == 'Pictorial View'){
            $header = Pictorial_view::find($id);
        }if($table == 'Conveyance Deed'){
            $header = Conveyance::find($id);
        }if($table == 'Agreement'){
            $header = Agreement::find($id);
        }if($table == 'Indemnity Bond'){
            $header = Indemnity_bond::find($id);
        }if($table == 'Registry Document'){
            $header = Registry_document::find($id);
        }if($table == 'Exemption Form'){
            $header = Exemption_form::find($id);
        }if($table == 'Affidavit 2'){
            $header = Affidavit_2::find($id);
        }if($table == 'Intimation Application'){
            $header = Int_application::find($id);
        }if($table == 'Intimation Letter'){
            $header = Intimation_letter::find($id);
        }if($table == 'Exemption Inventory'){
            $header = Exemption_inventory_approval::find($id);
        }


        if($header['status'] == 2){
            $header->status = 1;
            $header->save();
        }

        return redirect()->route('rejected_documents', $id);

//        {{ route('rejected_documents', Auth::user()->id) }};



    }
    public function approval_inbox($id){
            $loginId = auth()->user()->id;

        $data['total_count'] = 0;
            $approval_check = Approval_setup_line::where('isDeleted', 0)->where('user', $loginId)->orderBy('id', 'desc')->get();

        if($approval_check){
            $data['lp_master_data_record_count'] = 0;
            $data['challan_fee_approvals_count'] = 0;
            $data['exemption_r_count'] = 0;
            $data['land_form_seller_count'] = 0;
            $data['seller_profile_count'] = 0;
            $data['challan_form_count'] = 0;
            $data['purchase_of_land_count'] = 0;
            $data['possession_certificate_count'] = 0;
            $data['pictorial_view_count'] = 0;
            $data['conveyance_deed_count'] = 0;
            $data['agreement_count'] = 0;
            $data['indemnity_bond_count'] = 0;
            $data['registry_document_count'] = 0;
            $data['exemption_form_count'] = 0;
            $data['affidavit_2_count'] = 0;
            $data['intimation_application_count'] = 0;
            $data['intimation_letter_count'] = 0;
            $data['exemption_inventory_count'] = 0;
            $data['total_count'] = 0;
            $data['lp_master_data'] = array();
            $data['exemption_r'] = array();
            $data['challan_fee'] = array();
            $data['seller_profile'] = array();
            $data['challan_form'] = array();
            $data['land_form_seller'] = array();
            $data['purchase_of_land'] = array();
            $data['possession_certificate'] = array();
            $data['pictorial_view'] = array();
            $data['conveyance_deed'] = array();
            $data['agreement'] = array();
            $data['indemnity_bond'] = array();
            $data['registry_document'] = array();
            $data['exemption_form'] = array();
            $data['affidavit_2'] = array();
            $data['intimation_application'] = array();
            $data['intimation_letter'] = array();
            $data['exemption_inventory'] = array();

            foreach($approval_check as $single_record){

                $data['Approval_setup_header'] = Approval_setup_header::where('isDeleted', 0)->where('id', $single_record->main)->orderBy('id','desc')->first();

                if($data['Approval_setup_header']['approval'] == 'LP Master Data'){
                    $data['lp_master_data'] = Land_provider::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'LP Master Data')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['lp_master_data'] = $data['lp_master_data']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['lp_master_data_approvals'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'LP Master Data')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['lp_master_data_record_count'] = $data['lp_master_data_approvals']->count();
                    $data['total_count'] += $data['lp_master_data_record_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Exemption Rate'){
                    $data['exemption_r'] = Exemption_rate::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Exemption Rate')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['exemption_r'] = $data['exemption_r']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['exemption_r_approvals'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Exemption Rate')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['exemption_r_count'] = $data['exemption_r_approvals']->count();
                    $data['total_count'] += $data['exemption_r_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Challan Fee'){
                    $data['challan_fee'] = Challan_fee::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Challan Fee')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['challan_fee'] = $data['challan_fee']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['challan_fee_approvals'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Challan Fee')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['challan_fee_approvals_count'] = $data['challan_fee_approvals']->count();
                    $data['total_count'] += $data['challan_fee_approvals_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Seller Profile'){
                    $data['seller_profile'] = Seller_profile::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Seller Profile')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['seller_profile'] = $data['seller_profile']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['seller_profile_approvals'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Seller Profile')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['seller_profile_count'] = $data['seller_profile_approvals']->count();
                    $data['total_count'] += $data['seller_profile_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Challan Form'){
                    $data['challan_form'] = Challan_form_header::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Challan Form')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['challan_form'] = $data['challan_form']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['challan_form_approvals'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Challan Form')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['challan_form_count'] = $data['challan_form_approvals']->count();
                    $data['total_count'] += $data['challan_form_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Land Form Seller'){
                    $data['land_form_seller'] = Land_form::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Land Form Seller')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['land_form_seller'] = $data['land_form_seller']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['challan_form_approvals_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Land Form Seller')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['land_form_seller_count'] = $data['challan_form_approvals_approval']->count();
                    $data['total_count'] += $data['land_form_seller_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Purchase of Land'){
                    $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Purchase of Land')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['purchase_of_land'] = $data['purchase_of_land']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['purchase_of_land_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Purchase of Land')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['purchase_of_land_count'] = $data['purchase_of_land_approval']->count();
                    $data['total_count'] += $data['purchase_of_land_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Possession Certificate'){
                    $data['possession_certificate'] = Possession_certificate::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Possession Certificate')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['possession_certificate'] = $data['possession_certificate']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['possession_certificate_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Possession Certificate')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['possession_certificate_count'] = $data['possession_certificate_approval']->count();
                    $data['total_count'] += $data['possession_certificate_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Pictorial View'){
                    $data['pictorial_view'] = Pictorial_view::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Pictorial View')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['pictorial_view'] = $data['pictorial_view']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['pictorial_view_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Pictorial View')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['pictorial_view_count'] = $data['pictorial_view_approval']->count();
                    $data['total_count'] += $data['pictorial_view_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Conveyance Deed'){
                    $data['conveyance_deed'] = Conveyance::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Conveyance Deed')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['conveyance_deed'] = $data['conveyance_deed']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['conveyance_deed_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Conveyance Deed')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['conveyance_deed_count'] = $data['conveyance_deed_approval']->count();
                    $data['total_count'] += $data['conveyance_deed_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Agreement'){
                    $data['agreement'] = Agreement::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Agreement')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['agreement'] = $data['agreement']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['agreement_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Agreement')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['agreement_count'] = $data['agreement_approval']->count();
                    $data['total_count'] += $data['agreement_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Registry Document'){
                    $data['registry_document'] = Registry_document::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Registry Document')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['registry_document'] = $data['registry_document']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['registry_document_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Registry Document')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['registry_document_count'] = $data['registry_document_approval']->count();
                    $data['total_count'] += $data['registry_document_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Exemption Form'){
                    $data['exemption_form'] = Exemption_form::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Exemption Form')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['exemption_form'] = $data['exemption_form']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['exemption_form_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Exemption Form')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['exemption_form_count'] = $data['exemption_form_approval']->count();
                    $data['total_count'] += $data['exemption_form_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Intimation Application'){
                    $data['intimation_application'] = Int_application::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Intimation Application')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['intimation_application'] = $data['intimation_application']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['intimation_application_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Intimation Application')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['intimation_application_count'] = $data['intimation_application_approval']->count();
                    $data['total_count'] += $data['intimation_application_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Intimation Letter'){
                    $data['intimation_letter'] = Intimation_letter::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Intimation Letter')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['intimation_letter'] = $data['intimation_letter']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['intimation_letter_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Intimation Letter')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['intimation_letter_count'] = $data['intimation_letter_approval']->count();
                    $data['total_count'] += $data['intimation_letter_count'] ;
                }
                if($data['Approval_setup_header']['approval'] == 'Exemption Inventory'){
                    $data['exemption_inventory'] = Exemption_inventory_approval::where('isDeleted', 0)
                        ->where('status', 1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $approval_documents = Document_approval::orderBy('id', 'desc')
                        ->where('status', '!=', 1)
                        ->where('document_name', 'Exemption Inventory')
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->get();
                    $documentIds = $approval_documents->pluck('document_id')->toArray();
// Filter out the records
                    $data['exemption_inventory'] = $data['exemption_inventory']->filter(function ($item) use ($documentIds) {
                        return in_array($item->id, $documentIds);
                    });
                    $data['exemption_inventory_approval'] = Document_approval::where('approval_user_id', $loginId)
                        ->where('document_name', 'Exemption Inventory')
                        ->where('status', '!=', 1)
                        ->where('isDeleted', 0)
                        ->where('approval_user_id',$loginId)
                        ->where('priority',1)
                        ->orderBy('id', 'desc')
                        ->get();
                    $data['exemption_inventory_count'] = $data['exemption_inventory_approval']->count();
                    $data['total_count'] += $data['exemption_inventory_count'] ;
                }
            }
        }

        session()->put('total_count_approval', $data['total_count']);



            return view('pages.approvals.setup.inbox',$data);

    }
    public function pending_documents($id){
            $loginId = auth()->user()->id;

        $data['total_count'] = 0;

            $data['lp_master_data_record_count'] = 0;
            $data['challan_fee_approvals_count'] = 0;
            $data['exemption_r_count'] = 0;
            $data['land_form_seller_count'] = 0;
            $data['seller_profile_count'] = 0;
            $data['challan_form_count'] = 0;
            $data['purchase_of_land_count'] = 0;
            $data['possession_certificate_count'] = 0;
            $data['pictorial_view_count'] = 0;
            $data['conveyance_deed_count'] = 0;
            $data['agreement_count'] = 0;
            $data['indemnity_bond_count'] = 0;
            $data['registry_document_count'] = 0;
            $data['exemption_form_count'] = 0;
            $data['affidavit_2_count'] = 0;
            $data['intimation_application_count'] = 0;
            $data['intimation_letter_count'] = 0;
            $data['exemption_inventory_count'] = 0;
            $data['total_count'] = 0;
            $data['lp_master_data'] = array();
            $data['exemption_r'] = array();
            $data['challan_fee'] = array();
            $data['seller_profile'] = array();
            $data['challan_form'] = array();
            $data['land_form_seller'] = array();
            $data['purchase_of_land'] = array();
            $data['possession_certificate'] = array();
            $data['pictorial_view'] = array();
            $data['conveyance_deed'] = array();
            $data['agreement'] = array();
            $data['indemnity_bond'] = array();
            $data['registry_document'] = array();
            $data['exemption_form'] = array();
            $data['affidavit_2'] = array();
            $data['intimation_application'] = array();
            $data['intimation_letter'] = array();
            $data['exemption_inventory'] = array();

            $data['lp_master_data'] = Land_provider::where('createdBy', $loginId)
                ->where('status', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
                $data['lp_master_data_record_count'] = $data['lp_master_data']->count();
                $data['total_count'] += $data['lp_master_data_record_count'] ;

            $data['exemption_r'] = Exemption_rate::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['exemption_r_count'] = $data['exemption_r']->count();
            $data['total_count'] += $data['exemption_r_count'] ;
            $data['challan_fee'] = Challan_fee::where('isDeleted', 0)
                    ->where('createdBy', $loginId)
                    ->where('status', 1)
                    ->where('isDeleted', 0)
                    ->orderBy('id', 'desc')
                    ->get();

            $data['challan_fee_approvals_count'] = $data['challan_fee']->count();
            $data['total_count'] += $data['challan_fee_approvals_count'] ;

            $data['seller_profile'] = Seller_profile::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['seller_profile_count'] = $data['seller_profile']->count();
            $data['total_count'] += $data['seller_profile_count'] ;

            $data['challan_form'] = Challan_form_header::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['challan_form_count'] = $data['challan_form']->count();
            $data['total_count'] += $data['challan_form_count'] ;

            $data['land_form_seller'] = Land_form::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['land_form_seller_count'] = $data['land_form_seller']->count();
            $data['total_count'] += $data['land_form_seller_count'] ;

            $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['purchase_of_land_count'] = $data['purchase_of_land']->count();
            $data['total_count'] += $data['purchase_of_land_count'] ;


            $data['possession_certificate'] = Possession_certificate::where('isDeleted', 0)
                ->where('status', 1)
                ->where('createdBy', $loginId)

                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['possession_certificate_count'] = $data['possession_certificate']->count();
            $data['total_count'] += $data['possession_certificate_count'] ;


            $data['pictorial_view'] = Pictorial_view::where('isDeleted', 0)
                ->where('status', 1)
                ->where('createdBy', $loginId)

                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['pictorial_view_count'] = $data['pictorial_view']->count();
            $data['total_count'] += $data['pictorial_view_count'] ;





            $data['conveyance_deed'] = Conveyance::where('isDeleted', 0)
                ->where('status', 1)
                ->where('createdBy', $loginId)

                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['conveyance_deed_count'] = $data['conveyance_deed']->count();
            $data['total_count'] += $data['conveyance_deed_count'] ;


            $data['agreement'] = Agreement::where('isDeleted', 0)
                ->where('status', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['agreement_count'] = $data['agreement']->count();
            $data['total_count'] += $data['agreement_count'] ;


            $data['registry_document'] = Registry_document::where('isDeleted', 0)
                ->where('status', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['registry_document_count'] = $data['registry_document']->count();
            $data['total_count'] += $data['registry_document_count'] ;


            $data['exemption_form'] = Exemption_form::where('isDeleted', 0)
                ->where('status', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['exemption_form_count'] = $data['exemption_form']->count();
            $data['total_count'] += $data['exemption_form_count'] ;



            $data['intimation_application'] = Int_application::where('isDeleted', 0)
                ->where('status', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['intimation_application_count'] = $data['intimation_application']->count();
            $data['total_count'] += $data['intimation_application_count'] ;





            $data['intimation_letter'] = Intimation_letter::where('isDeleted', 0)
                ->where('status', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['intimation_letter_count'] = $data['intimation_letter']->count();
            $data['total_count'] += $data['intimation_letter_count'] ;

            $data['exemption_inventory'] = Exemption_inventory_approval::where('isDeleted', 0)
                ->where('status', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['exemption_inventory_count'] = $data['exemption_inventory']->count();
            $data['total_count'] += $data['exemption_inventory_count'] ;



        session()->put('total_count_pending', $data['total_count']);



            return view('pages.approvals.setup.pending',$data);

    }
    public function approved_documents($id){
            $loginId = auth()->user()->id;

            $data['total_count'] = 0;
            $data['lp_master_data_record_count'] = 0;
            $data['challan_fee_approvals_count'] = 0;
            $data['exemption_r_count'] = 0;
            $data['land_form_seller_count'] = 0;
            $data['seller_profile_count'] = 0;
            $data['challan_form_count'] = 0;
            $data['purchase_of_land_count'] = 0;
            $data['possession_certificate_count'] = 0;
            $data['pictorial_view_count'] = 0;
            $data['conveyance_deed_count'] = 0;
            $data['agreement_count'] = 0;
            $data['indemnity_bond_count'] = 0;
            $data['registry_document_count'] = 0;
            $data['exemption_form_count'] = 0;
            $data['affidavit_2_count'] = 0;
            $data['intimation_application_count'] = 0;
            $data['intimation_letter_count'] = 0;
            $data['exemption_inventory_count'] = 0;
            $data['total_count'] = 0;
            $data['lp_master_data'] = array();
            $data['exemption_r'] = array();
            $data['challan_fee'] = array();
            $data['seller_profile'] = array();
            $data['challan_form'] = array();
            $data['land_form_seller'] = array();
            $data['purchase_of_land'] = array();
            $data['possession_certificate'] = array();
            $data['pictorial_view'] = array();
            $data['conveyance_deed'] = array();
            $data['agreement'] = array();
            $data['indemnity_bond'] = array();
            $data['registry_document'] = array();
            $data['exemption_form'] = array();
            $data['affidavit_2'] = array();
            $data['intimation_application'] = array();
            $data['intimation_letter'] = array();
            $data['exemption_inventory'] = array();

            $data['lp_master_data'] = Land_provider::where('createdBy', $loginId)
                ->where('status', 0)->where('view', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
                $data['lp_master_data_record_count'] = $data['lp_master_data']->count();
                $data['total_count'] += $data['lp_master_data_record_count'] ;

            $data['exemption_r'] = Exemption_rate::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 0)->where('view', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['exemption_r_count'] = $data['exemption_r']->count();
            $data['total_count'] += $data['exemption_r_count'] ;
            $data['challan_fee'] = Challan_fee::where('isDeleted', 0)
                    ->where('createdBy', $loginId)
                    ->where('status', 0)->where('view', 1)
                    ->where('isDeleted', 0)
                    ->orderBy('id', 'desc')
                    ->get();

            $data['challan_fee_approvals_count'] = $data['challan_fee']->count();
            $data['total_count'] += $data['challan_fee_approvals_count'] ;

            $data['seller_profile'] = Seller_profile::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 0)->where('view', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['seller_profile_count'] = $data['seller_profile']->count();
            $data['total_count'] += $data['seller_profile_count'] ;

            $data['challan_form'] = Challan_form_header::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 0)->where('view', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['challan_form_count'] = $data['challan_form']->count();
            $data['total_count'] += $data['challan_form_count'] ;

            $data['land_form_seller'] = Land_form::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 0)->where('view', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['land_form_seller_count'] = $data['land_form_seller']->count();
            $data['total_count'] += $data['land_form_seller_count'] ;

            $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 0)->where('view', 1)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['purchase_of_land_count'] = $data['purchase_of_land']->count();
            $data['total_count'] += $data['purchase_of_land_count'] ;


            $data['possession_certificate'] = Possession_certificate::where('isDeleted', 0)
                ->where('status', 0)->where('view', 1)
                ->where('createdBy', $loginId)

                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['possession_certificate_count'] = $data['possession_certificate']->count();
            $data['total_count'] += $data['possession_certificate_count'] ;


            $data['pictorial_view'] = Pictorial_view::where('isDeleted', 0)
                ->where('status', 0)->where('view', 1)
                ->where('createdBy', $loginId)

                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['pictorial_view_count'] = $data['pictorial_view']->count();
            $data['total_count'] += $data['pictorial_view_count'] ;





            $data['conveyance_deed'] = Conveyance::where('isDeleted', 0)
                ->where('status', 0)->where('view', 1)
                ->where('createdBy', $loginId)

                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['conveyance_deed_count'] = $data['conveyance_deed']->count();
            $data['total_count'] += $data['conveyance_deed_count'] ;


            $data['agreement'] = Agreement::where('isDeleted', 0)
                ->where('status', 0)->where('view', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['agreement_count'] = $data['agreement']->count();
            $data['total_count'] += $data['agreement_count'] ;


            $data['registry_document'] = Registry_document::where('isDeleted', 0)
                ->where('status', 0)->where('view', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['registry_document_count'] = $data['registry_document']->count();
            $data['total_count'] += $data['registry_document_count'] ;


            $data['exemption_form'] = Exemption_form::where('isDeleted', 0)
                ->where('status', 0)->where('view', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['exemption_form_count'] = $data['exemption_form']->count();
            $data['total_count'] += $data['exemption_form_count'] ;



            $data['intimation_application'] = Int_application::where('isDeleted', 0)
                ->where('status', 0)->where('view', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['intimation_application_count'] = $data['intimation_application']->count();
            $data['total_count'] += $data['intimation_application_count'] ;





            $data['intimation_letter'] = Intimation_letter::where('isDeleted', 0)
                ->where('status', 0)->where('view', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['intimation_letter_count'] = $data['intimation_letter']->count();
            $data['total_count'] += $data['intimation_letter_count'] ;

            $data['exemption_inventory'] = Exemption_inventory_approval::where('isDeleted', 0)
                ->where('status', 0)->where('view', 1)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['exemption_inventory_count'] = $data['exemption_inventory']->count();
            $data['total_count'] += $data['exemption_inventory_count'] ;




        session()->put('total_count_approved', $data['total_count']);



            return view('pages.approvals.setup.approved',$data);

    }
    public function rejected_documents($id){
            $loginId = auth()->user()->id;

            $data['total_count'] = 0;
            $data['lp_master_data_record_count'] = 0;
            $data['challan_fee_approvals_count'] = 0;
            $data['exemption_r_count'] = 0;
            $data['land_form_seller_count'] = 0;
            $data['seller_profile_count'] = 0;
            $data['challan_form_count'] = 0;
            $data['purchase_of_land_count'] = 0;
            $data['possession_certificate_count'] = 0;
            $data['pictorial_view_count'] = 0;
            $data['conveyance_deed_count'] = 0;
            $data['agreement_count'] = 0;
            $data['indemnity_bond_count'] = 0;
            $data['registry_document_count'] = 0;
            $data['exemption_form_count'] = 0;
            $data['affidavit_2_count'] = 0;
            $data['intimation_application_count'] = 0;
            $data['intimation_letter_count'] = 0;
            $data['exemption_inventory_count'] = 0;
            $data['total_count'] = 0;
            $data['lp_master_data'] = array();
            $data['exemption_r'] = array();
            $data['challan_fee'] = array();
            $data['seller_profile'] = array();
            $data['challan_form'] = array();
            $data['land_form_seller'] = array();
            $data['purchase_of_land'] = array();
            $data['possession_certificate'] = array();
            $data['pictorial_view'] = array();
            $data['conveyance_deed'] = array();
            $data['agreement'] = array();
            $data['indemnity_bond'] = array();
            $data['registry_document'] = array();
            $data['exemption_form'] = array();
            $data['affidavit_2'] = array();
            $data['intimation_application'] = array();
            $data['intimation_letter'] = array();
            $data['exemption_inventory'] = array();

            $data['lp_master_data'] = Land_provider::where('createdBy', $loginId)
                ->where('status', 2)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
                $data['lp_master_data_record_count'] = $data['lp_master_data']->count();
                $data['total_count'] += $data['lp_master_data_record_count'] ;

            $data['exemption_r'] = Exemption_rate::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 2)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['exemption_r_count'] = $data['exemption_r']->count();
            $data['total_count'] += $data['exemption_r_count'] ;
            $data['challan_fee'] = Challan_fee::where('isDeleted', 0)
                    ->where('createdBy', $loginId)
                    ->where('status', 2)
                    ->where('isDeleted', 0)
                    ->orderBy('id', 'desc')
                    ->get();

            $data['challan_fee_approvals_count'] = $data['challan_fee']->count();
            $data['total_count'] += $data['challan_fee_approvals_count'] ;

            $data['seller_profile'] = Seller_profile::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 2)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['seller_profile_count'] = $data['seller_profile']->count();
            $data['total_count'] += $data['seller_profile_count'] ;

            $data['challan_form'] = Challan_form_header::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 2)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['challan_form_count'] = $data['challan_form']->count();
            $data['total_count'] += $data['challan_form_count'] ;

            $data['land_form_seller'] = Land_form::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 2)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['land_form_seller_count'] = $data['land_form_seller']->count();
            $data['total_count'] += $data['land_form_seller_count'] ;

            $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)
                ->where('createdBy', $loginId)
                ->where('status', 2)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['purchase_of_land_count'] = $data['purchase_of_land']->count();
            $data['total_count'] += $data['purchase_of_land_count'] ;


            $data['possession_certificate'] = Possession_certificate::where('isDeleted', 0)
                ->where('status', 2)
                ->where('createdBy', $loginId)

                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['possession_certificate_count'] = $data['possession_certificate']->count();
            $data['total_count'] += $data['possession_certificate_count'] ;


            $data['pictorial_view'] = Pictorial_view::where('isDeleted', 0)
                ->where('status', 2)
                ->where('createdBy', $loginId)

                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['pictorial_view_count'] = $data['pictorial_view']->count();
            $data['total_count'] += $data['pictorial_view_count'] ;





            $data['conveyance_deed'] = Conveyance::where('isDeleted', 0)
                ->where('status', 2)
                ->where('createdBy', $loginId)

                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['conveyance_deed_count'] = $data['conveyance_deed']->count();
            $data['total_count'] += $data['conveyance_deed_count'] ;


            $data['agreement'] = Agreement::where('isDeleted', 0)
                ->where('status', 2)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['agreement_count'] = $data['agreement']->count();
            $data['total_count'] += $data['agreement_count'] ;


            $data['registry_document'] = Registry_document::where('isDeleted', 0)
                ->where('status', 2)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['registry_document_count'] = $data['registry_document']->count();
            $data['total_count'] += $data['registry_document_count'] ;


            $data['exemption_form'] = Exemption_form::where('isDeleted', 0)
                ->where('status', 2)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['exemption_form_count'] = $data['exemption_form']->count();
            $data['total_count'] += $data['exemption_form_count'] ;



            $data['intimation_application'] = Int_application::where('isDeleted', 0)
                ->where('status', 2)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();
            $data['intimation_application_count'] = $data['intimation_application']->count();
            $data['total_count'] += $data['intimation_application_count'] ;





            $data['intimation_letter'] = Intimation_letter::where('isDeleted', 0)
                ->where('status', 2)
                ->where('createdBy', $loginId)
                ->where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            $data['intimation_letter_count'] = $data['intimation_letter']->count();
            $data['total_count'] += $data['intimation_letter_count'] ;





        session()->put('total_count_rejected', $data['total_count']);



            return view('pages.approvals.setup.rejected',$data);

    }
    public function approval_status_update(Request $request)
    {
        $request->validate([
            'id' => 'required',
            'status' => 'required',
        ]);

        $loginId = auth()->user()->id;

        // Find the document record
        $document_record = Document_approval::where('isDeleted', 0)
            ->where('document_id', $request->id)
            ->where('document_name', $request->form)
            ->orderBy('id', 'desc')
            ->first();

        if (!$document_record) {
            return redirect()->route('approval_inbox', ['id' => $loginId])
                ->with('danger', 'Something went wrong');
        }

        // Insert approval history
//        $insert_approval_history = new Document_approval_history();
//        $insert_approval_history->document_name = $document_record->document_name;
//        $insert_approval_history->document_id = $request->id;
//        $insert_approval_history->approval_user_id = $loginId;
//        $insert_approval_history->status = $request->status;
//        $insert_approval_history->remarks = $request->remarks;
//        $insert_approval_history->save();


        if ($request->status == 2) {
            $notFirstApproval = Document_approval::where('isDeleted', 0)
                ->where('document_id', $request->id)
                ->where('document_name', $request->form)
//                ->where('approval_user_id', $loginId)
//                ->where('status','!=', 1)
                ->where('priority',  0)
                ->orderBy('id', 'desc')
                ->first();

            if (!$notFirstApproval) {
                $record1 = Document_approval::where('isDeleted', 0)
                    ->where('document_id', $request->id)
                    ->where('document_name', $request->form)
                    ->where('approval_user_id', $loginId)
                    ->orderBy('id', 'desc')
                    ->first();

                if ($record1) {

                    $documentTypes1 = [
                        'LP Master Data' => Land_provider::class,
                        'Exemption Rate' => Exemption_rate::class,
                        'Challan Fee' => Challan_fee::class,
                        'Seller Profile' => Seller_profile::class,
                        'Challan Form' => Challan_form_header::class,
                        'Land Form Seller' => Land_form::class,
                        'Purchase of Land' => Purchase_of_land::class,
                        'Possession Certificate' => Possession_certificate::class,
                        'Pictorial View' => Pictorial_view::class,
                        'Conveyance Deed' => Conveyance::class,
                        'Agreement' => Agreement::class,
                        'Indemnity Bond' => Indemnity_bond::class,
                        'Registry Document' => Registry_document::class,
                        'Exemption Form' => Exemption_form::class,
                        'Affidavit 2' => Affidavit_2::class,
                        'Intimation Application' => Int_application::class,
                        'Intimation Letter' => Intimation_letter::class,
                    ];

                    if (isset($documentTypes1[$request->form])) {
                        $document = $documentTypes1[$request->form]::find($request->id);

                        $document->status = 2;
                        $document->save();
//                        return redirect()->route('approval_inbox', ['id' => $loginId])
//                            ->with('success', 'Document has been Rejected');
                    }

                }
            }
        }





        // Update the document approval status and priority
        $row = Document_approval::where('isDeleted', 0)
            ->where('document_id', $request->id)
            ->where('document_name', $request->form)
            ->where('approval_user_id', $loginId)
            ->orderBy('id', 'desc')
            ->first();

        if ($row) {
            $priority = $request->status == 1 ? $row->priority - 1 : $row->priority + 1;
            $row->status = $request->status;
            $row->remarks = $request->remarks;
            $row->priority = $priority;
            $row->save();

            // Update the priorities of other documents
            if ($request->status == 1) {
                $nextDocs = Document_approval::where('document_id', $request->id)
                    ->where('document_name', $request->form)
                    ->where('id', '>', $row->id)
                    ->where('approval_user_id', '!=', $loginId)
                    ->orderBy('id', 'desc')
                    ->get();

                foreach ($nextDocs as $nextDoc) {
                    $nextDoc->priority = $nextDoc->priority - 1;
                    $nextDoc->save();
                }
            } elseif ($request->status == 2) {
                $nextDocs = Document_approval::where('document_id', $request->id)
                    ->where('document_name', $request->form)
                    ->where('id', '<', $row->id)
                    ->where('approval_user_id', '!=', $loginId)
                    ->orderBy('id', 'desc')
                    ->get();

                $lop = 1;
                foreach ($nextDocs as $nextDoc) {
                    $nextDoc->priority = $nextDoc->priority + 1;
                    if ($lop == 1) {
                        $nextDoc->status = 0;
                    }
                    $nextDoc->save();
                    $lop++;
                }
            }
        }

        // Finalize the document status if needed
        $row = Document_approval::where('isDeleted', 0)
            ->where('document_id', $request->id)
            ->where('document_name', $request->form)
            ->where('status', '!=', 1)
            ->orderBy('id', 'desc')
            ->first();

        if (!$row) {
            $documentTypes = [
                'LP Master Data' => Land_provider::class,
                'Exemption Rate' => Exemption_rate::class,
                'Challan Fee' => Challan_fee::class,
                'Seller Profile' => Seller_profile::class,
                'Challan Form' => Challan_form_header::class,
                'Land Form Seller' => Land_form::class,
                'Purchase of Land' => Purchase_of_land::class,
                'Possession Certificate' => Possession_certificate::class,
                'Pictorial View' => Pictorial_view::class,
                'Conveyance Deed' => Conveyance::class,
                'Agreement' => Agreement::class,
                'Indemnity Bond' => Indemnity_bond::class,
                'Registry Document' => Registry_document::class,
                'Exemption Form' => Exemption_form::class,
                'Affidavit 2' => Affidavit_2::class,
                'Intimation Application' => Int_application::class,
                'Intimation Letter' => Intimation_letter::class,
            ];

            if (isset($documentTypes[$request->form])) {
                $header = $documentTypes[$request->form]::find($request->id);
                $header->status = $request->status == 2 ? 2 : 0;
                $header->save();
            }
        }

        $statusMessage = $request->status == 1 ? 'Approved' : 'Rejected';

        return redirect()->route('approval_inbox', ['id' => $loginId])
            ->with('success', 'Document has been ' . $statusMessage);
    }

    public function approval_status_update_1(Request $request)
    {
        $request->validate([
            'id' => 'required',
            'status' => 'required',
        ]);

        $loginId = auth()->user()->id;








        $document_record = Document_approval::where('isDeleted', 0)
            ->where('document_id', $request->id)
            ->where('document_name', $request->form)
            ->orderBy('id', 'desc')
            ->first();

        if(!$document_record){
            return redirect()->route('approval_inbox', ['id' => $loginId])
                ->with('danger', 'Something went wrong');
        }



        $insert_approval_history = new Document_approval_history();
        $insert_approval_history->document_name = $document_record['document_name'];
        $insert_approval_history->document_id =  $request->id;
        $insert_approval_history->approval_user_id =  $loginId;
        $insert_approval_history->status = $request->status;
        $insert_approval_history->remarks = $request->remarks;
        $insert_approval_history->save();



//        if ($request->status == 2) {
//            $notFirstApproval = Document_approval::where('isDeleted', 0)
//                ->where('document_id', $request->id)
//                ->where('document_name', $request->form)
//                ->where('status', '>', 0)
//                ->orderBy('id', 'desc')
//                ->first();
//
//            if (!$notFirstApproval) {
//                $record1 = Document_approval::where('isDeleted', 0)
//                    ->where('document_id', $request->id)
//                    ->where('document_name', $request->form)
//                    ->where('approval_user_id', $loginId)
//                    ->orderBy('id', 'desc')
//                    ->first();
//
//                if ($record1) {
//
//                    if($request->form == 'LP Master Data'){
//                        $document = Land_provider::find($request->id);
//                    }if($request->form == 'Exemption Rate'){
//                        $document = Exemption_rate::find($request->id);
//                    }if($request->form == 'Challan Fee'){
//                        $document = Challan_fee::find($request->id);
//                    }if($request->form == 'Seller Profile'){
//                        $document = Seller_profile::find($request->id);
//                    }if($request->form == 'Challan Form'){
//                        $document = Challan_form_header::find($request->id);
//                    }if($request->form == 'Land Form Seller'){
//                        $document = Land_form::find($request->id);
//                    }if($request->form == 'Purchase of Land'){
//                        $document = Purchase_of_land::find($request->id);
//                    }if($request->form == 'Possession Certificate'){
//                        $document = Possession_certificate::find($request->id);
//                    }if($request->form == 'Pictorial View'){
//                        $document = Pictorial_view::find($request->id);
//                    }if($request->form == 'Conveyance Deed'){
//                        $document = Conveyance::find($request->id);
//                    }if($request->form == 'Agreement'){
//                        $document = Agreement::find($request->id);
//                    }if($request->form == 'Indemnity Bond'){
//                        $document = Indemnity_bond::find($request->id);
//                    }if($request->form == 'Registry Document'){
//                        $document = Registry_document::find($request->id);
//                    }if($request->form == 'Exemption Form'){
//                        $document = Exemption_form::find($request->id);
//                    }if($request->form == 'Affidavit 2'){
//                        $document = Affidavit_2::find($request->id);
//                    }if($request->form == 'Intimation Application'){
//                        $document = Int_application::find($request->id);
//                    }if($request->form == 'Intimation Letter'){
//                        $document = Intimation_letter::find($request->id);
//                    }
//                    $document->status = 2;
//                    $document->save();
//                    return redirect()->route('approval_inbox', ['id' => $loginId])
//                        ->with('success', 'Document has been Rejected');
//                }
//            }
//        }

        $row = Document_approval::where('isDeleted', 0)
            ->where('document_id', $request->id)
            ->where('document_name', $request->form)
            ->where('approval_user_id', $loginId)
            ->orderBy('id', 'desc')
            ->first();

        if ($row) {

            if ($request->status == 1) {
                $priority = $row->priority - 1;
            } else {
                $priority = $row->priority + 1;
            }

            $row->status = $request->status;
            $row->remarks = $request->remarks;
            $row->priority = $priority;
            $row->save();



            if ($request->status == 1) {
                $nextDoc = Document_approval::where('document_id', $request->id)
                    ->where('document_name', $request->form)
//                    ->where('priority','>', 0)
                    ->where('id', '>', $row->id)
                    ->where('approval_user_id', '!=', $loginId)
                    ->orderBy('id', 'desc')
                    ->get();

                if ($nextDoc) {
                    foreach($nextDoc as $nextsinglerow){

                        $nextDocpriority = $nextsinglerow->priority - 1;

                        $header = Document_approval::find($nextsinglerow->id);
                        $header->priority = $nextDocpriority;
                        $header->save();
                    }

                }
            }
            if ($request->status == 2) {

                $nextDoc = Document_approval::where('document_id', $request->id)
                    ->where('document_name', $request->form)
//                    ->where('priority', 0)
//                    ->where('status', 1)
                    ->where('approval_user_id', '!=', $loginId)
                    ->where('id', '<', $row->id)
                    ->orderBy('id', 'desc')
                    ->get();

                if ($nextDoc) {
                    $lop = 1;
//                    dd($nextDoc);
                    foreach($nextDoc as $nextsinglerow){

                        $nextDocpriority = $nextsinglerow->priority + 1;
                        $header = Document_approval::find($nextsinglerow->id);
                        if($lop == 1){
                            $header->status = 0;
                            $header->priority = $nextDocpriority;
                            $header->save();
                        }

                        $lop++;
                    }

                }
            }
            }



        $row = Document_approval::where('isDeleted', 0)
            ->where('document_id', $request->id)
            ->where('document_name', $request->form)
            ->where('status', '!=', 1)
//            ->where('approval_user_id', $loginId)
            ->orderBy('id', 'desc')
            ->first();

        if(!$row || $request->form == 2){

            if($request->form == 'LP Master Data'){
                $header = Land_provider::find($request->id);
            }if($request->form == 'Exemption Rate'){
                $header = Exemption_rate::find($request->id);
            }if($request->form == 'Challan Fee'){
                $header = Challan_fee::find($request->id);
            }if($request->form == 'Seller Profile'){
                $header = Seller_profile::find($request->id);
            }if($request->form == 'Challan Form'){
                $header = Challan_form_header::find($request->id);
            }if($request->form == 'Land Form Seller'){
                $header = Land_form::find($request->id);
            }if($request->form == 'Purchase of Land'){
                $header = Purchase_of_land::find($request->id);
            }if($request->form == 'Possession Certificate'){
                $header = Possession_certificate::find($request->id);
            }if($request->form == 'Pictorial View'){
                $header = Pictorial_view::find($request->id);
            }if($request->form == 'Conveyance Deed'){
                $header = Conveyance::find($request->id);
            }if($request->form == 'Agreement'){
                $header = Agreement::find($request->id);
            }if($request->form == 'Indemnity Bond'){
                $header = Indemnity_bond::find($request->id);
            }if($request->form == 'Registry Document'){
                $header = Registry_document::find($request->id);
            }if($request->form == 'Exemption Form'){
                $header = Exemption_form::find($request->id);
            }if($request->form == 'Affidavit 2'){
                $header = Affidavit_2::find($request->id);
            }if($request->form == 'Intimation Application'){
                $header = Int_application::find($request->id);
            }if($request->form == 'Intimation Letter'){
                $header = Intimation_letter::find($request->id);
            }
            if($request->status == 2){
                $header->status = $request->status;

            }else{
                $header->status = 0;

            }
            $header->save();
        }


        $statusMessage = $request->status == 1 ? 'Approved' : 'Rejected';

        return redirect()->route('approval_inbox', ['id' => $loginId])
            ->with('success', 'Document has been ' . $statusMessage);
    }


    public function update(Request $request, $id)
    {
        //
    }


    public function approved_docuement_view(Request $request)
    {
        $loginId = auth()->user()->id;

        $documentTypes = [
            'LP Master Data' => Land_provider::class,
            'Exemption Rate' => Exemption_rate::class,
            'Challan Fee' => Challan_fee::class,
            'Seller Profile' => Seller_profile::class,
            'Challan Form' => Challan_form_header::class,
            'Land Form Seller' => Land_form::class,
            'Purchase of Land' => Purchase_of_land::class,
            'Possession Certificate' => Possession_certificate::class,
            'Pictorial View' => Pictorial_view::class,
            'Conveyance Deed' => Conveyance::class,
            'Agreement' => Agreement::class,
            'Indemnity Bond' => Indemnity_bond::class,
            'Registry Document' => Registry_document::class,
            'Exemption Form' => Exemption_form::class,
            'Affidavit 2' => Affidavit_2::class,
            'Intimation Application' => Int_application::class,
            'Intimation Letter' => Intimation_letter::class,
        ];

        if (isset($documentTypes[$request->elementText])) {
            $documentClass = $documentTypes[$request->elementText];
            $headers = $documentClass::where('createdBy', $loginId)->get();

            foreach ($headers as $header) {
                $header->view = 2;
                $header->save();
            }

            return response()->json(['success' => 'Document status updated successfully']);
        }

        return response()->json(['error' => 'Invalid document type'], 400);
    }
    public function destroy($id)
    {
        if ($id) {

            $header = Approval_setup_header::find($id);
            $header->isDeleted = 1;
            $header->delete();


            $childs = Approval_setup_line::where('main', $id)->get();

            if ($childs->count() > 0) {
                // $child found
                foreach ($childs as $child) {
//                    $child->isDeleted = 1;
                    $child->delete();
                    // Process each record
                }
            }




            return redirect()->route('approval_setup.index')
                ->with('success', 'Approval Setup Has Been Deleted successfully');
        } else {
            return redirect()->route('approval_setup.index')
                ->with('danger', 'Approval Setup Not Found');
        }
    }
    public function edit_old(Approval_setup_header $approval_setup_header)
    {











        $header = Approval_setup_header::find(1);
//        dd($header);
//        print_r($approval_setup_header );exit;
//            if($approval_setup_header->id){
//                $id = $approval_setup_header->id;
//                $rows = Approval_setup_line::where('main' , $id)->get();
//                $approval_setup_header['rows'] =  $rows->toArray();
//            }
        $data['stages'] = Approval_stage::where('isDeleted', 0)->orderBy('id', 'desc')->get();
        $data['users'] = User::where('isDeleted', 0)->where('is_admin', 0)->orderBy('id', 'desc')->get();

        $data['tree'] = Approval_tree::where('isDeleted', 0)->orderBy('id', 'desc')->first();

        return view('pages.approvals.setup.edit',compact('approval_setup_header'),$data);
    }
    public function approval_inbox_old($id){
        $loginId = auth()->user()->id;
        $data['total_count'] = 0;
        $approval_check = Approval_setup_line::where('isDeleted', 0)->where('user', $loginId)->orderBy('id', 'desc')->first;
//     echo '<pre>';   print_r($approval_check);exit;
        if($approval_check){






            $data['Approval_setup_header'] = Approval_setup_header::where('isDeleted', 0)->where('id', $approval_check->main)->orderBy('id','desc')->get();

            echo '<pre>'; print_r($data['Approval_setup_header']);exit;




//            $data['lp_master_data'] = Land_provider::where('isDeleted', 0)->orderBy('id','desc')->get();
            $data['lp_master_data'] = Land_provider::where('isDeleted', 0)
                ->where('status', 0)->where('view', 1)
                ->orderBy('id', 'desc')
                ->get();

            $approval_documents = Document_approval::orderBy('id', 'desc')
                ->where('status', 1)
                ->get();

// Get an array of document IDs where status is not equal to 1
            $documentIds = $approval_documents->pluck('document_id')->toArray();

// Filter out the records
            $data['lp_master_data'] = $data['lp_master_data']->filter(function ($item) use ($documentIds) {
                return !in_array($item->id, $documentIds);
            });



            $data['lp_master_data_approvals'] = Document_approval::where('approval_user_id', $loginId)
                ->where('document_name', 'LP Master Data')
                ->where('status', '!=', 1)
                ->orderBy('id', 'desc')
                ->get();
            $data['lp_master_data_record_count'] = $data['lp_master_data_approvals']->count();
            $data['total_count'] += $data['lp_master_data_record_count'] ;
        }else{
            $data['lp_master_data'] = array();
//            $data['lp_master_data'] = Land_provider::where('isDeleted', 0)->orderBy('id','desc')->get();
            $data['lp_master_data_record_count'] = 0;
        }

//        print_r($data['total_count']);exit;
        return view('pages.approvals.setup.inbox',$data);

    }
    public function approval_status_update_old(Request $request){

        $request->validate([
            'id' => 'required',
            'status' => 'required',
        ]);

        $loginId =  auth()->user()->id;

//        $table = str_replace('Approval','',$request->form);
//
//        $approval_head = Approval_setup_header::where('isDeleted', 0)->where('approval', $table)->orderBy('id', 'desc')->first();
//        $approval_row = Approval_setup_line::where('isDeleted', 0)->where('main', $approval_head->id)->where('user', $loginId)->orderBy('id', 'desc')->first();

//      echo '<pre>';  print_r($table);exit;
//        ->where('status','!=',  0)



        if($request->status == 2){
            $notFirstApproval = Document_approval::where('isDeleted', 0)->where('document_id', $request->id)->where('status',  '>', 0)->orderBy('id','desc')->first();
            if(!$notFirstApproval){
                $record1 = Document_approval::where('isDeleted', 0)->where('document_id', $request->id)->where('approval_user_id', $loginId)->orderBy('id','desc')->first();

                $record1->id = $request->id;
                $record1->status = $request->status;
                $record1->isDeleted = 1;
                $record1->remarks = $request->remarks;
                $record1->save();
                return redirect()->route('approval_inbox', ['id' => $loginId])
                    ->with('success', 'Document has been Rejected ');
            }

        }

        $row = Document_approval::where('isDeleted', 0)->where('document_id', $request->id)->where('approval_user_id', $loginId)->orderBy('id','desc')->first();

        if($request->status == 1){
            $priority = $row->priority - 1;
        }
        if($request->status == 2){
            $priority = $row->priority +1;

        }

        $record = Document_approval::find($row->id);
        $record->id = $request->id;
        $record->status = $request->status;
        $record->remarks = $request->remarks;
        $record->priority = $priority;
        $record->save();




        $nextDoc = Document_approval::where('document_id', $request->id)->where('priority', 2)->where('approval_user_id', '!=', $loginId)->orderBy('id','desc')->first();


        if($request->status == 1){
            $nextDocpriority = $nextDoc->priority - 1;
        }
        if($request->status == 2){
            $nextDocpriority = $nextDoc->priority + 1;

        }

        if($nextDoc){
            $nextDoc->priority = $nextDocpriority;
            $nextDoc->save();
        }





//        $record = Document_approval::find($request->id);




//        if($request->status == 1){
//
//            if($table == 'LP Master Data '){
//                $getlp_current_status = Land_provider::where('isDeleted', 0)->where('approval', $table)->orderBy('id', 'desc')->first();
//
//                $record1 = new Land_provider();
//            }
//
//            $record1->status = $request->status;
//            $record1->save();
//
//        }



        if($request->status == 1){
            $s = 'Approved';
        }else{
            $s = 'Reject';
        }

        return redirect()->route('approval_inbox', ['id' => $loginId])
            ->with('success', 'Document has been '.$s);

    }

}
