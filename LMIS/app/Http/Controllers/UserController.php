<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\User;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Support\Facades\Hash;


class UserController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
       if(auth()->user()->is_admin == 1){
           $data['record'] = User::where(array('isDeleted'=>0,'is_admin' => 0))->orderBy('id','desc')->get();
//      echo '<pre>';  print_r($data);exit;
           return view('pages.users.show', $data);
       }else{
           return view('pages.authrization.show');

       }


    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        return view('pages.users.add');

    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        $request->validate([
            'name' => 'required',
            'email' => 'required',
            'password' => 'required',
        ]);
        $record = new User();
        $record->name = $request->name;
        $record->email = $request->email;
        $record->designation = $request->designation;
        $record->password = Hash::make($request->password);

        $record->lp_master_data_list = ($request->lp_master_data_list)?$request->lp_master_data_list: 0;
        $record->lp_master_data_add = ($request->lp_master_data_add)?$request->lp_master_data_add: 0;
        $record->lp_master_data_edit = ($request->lp_master_data_edit)?$request->lp_master_data_edit: 0;
        $record->lp_master_data_delete = ($request->lp_master_data_delete)?$request->lp_master_data_delete: 0;
        $record->lp_master_data_print = ($request->lp_master_data_print)?$request->lp_master_data_print: 0;
        $record->exemption_rate_list = ($request->exemption_rate_list)?$request->exemption_rate_list: 0;
        $record->exemption_rate_add = ($request->exemption_rate_add)?$request->exemption_rate_add: 0;
        $record->exemption_rate_edit = ($request->exemption_rate_edit)?$request->exemption_rate_edit: 0;
        $record->exemption_rate_delete = ($request->exemption_rate_delete)?$request->exemption_rate_delete: 0;
        $record->exemption_rate_print = ($request->exemption_rate_print)?$request->exemption_rate_print: 0;
        $record->challan_fee_list = ($request->challan_fee_list)?$request->challan_fee_list: 0;
        $record->challan_fee_add = ($request->challan_fee_add)?$request->challan_fee_add: 0;
        $record->challan_fee_edit = ($request->challan_fee_edit)?$request->challan_fee_edit: 0;
        $record->challan_fee_delete = ($request->challan_fee_delete)?$request->challan_fee_delete: 0;
        $record->challan_fee_print = ($request->challan_fee_print)?$request->challan_fee_print: 0;
        $record->seller_profile_list = ($request->seller_profile_list)?$request->seller_profile_list: 0;
        $record->seller_profile_add = ($request->seller_profile_add)?$request->seller_profile_add: 0;
        $record->seller_profile_edit = ($request->seller_profile_edit)?$request->seller_profile_edit: 0;
        $record->seller_profile_delete = ($request->seller_profile_delete)?$request->seller_profile_delete: 0;
        $record->seller_profile_print = ($request->seller_profile_print)?$request->seller_profile_print: 0;
        $record->challan_form_list = ($request->challan_form_list)?$request->challan_form_list: 0;
        $record->challan_form_add = ($request->challan_form_add)?$request->challan_form_add: 0;
        $record->challan_form_edit = ($request->challan_form_edit)?$request->challan_form_edit: 0;
        $record->challan_form_delete = ($request->challan_form_delete)?$request->challan_form_delete: 0;
        $record->challan_form_print = ($request->challan_form_print)?$request->challan_form_print: 0;
        $record->land_form_seller_list = ($request->land_form_seller_list)?$request->land_form_seller_list: 0;
        $record->land_form_seller_add = ($request->land_form_seller_add)?$request->land_form_seller_add: 0;
        $record->land_form_seller_edit = ($request->land_form_seller_edit)?$request->land_form_seller_edit: 0;
        $record->land_form_seller_delete = ($request->land_form_seller_delete)?$request->land_form_seller_delete: 0;
        $record->land_form_seller_print = ($request->land_form_seller_print)?$request->land_form_seller_print: 0;
        $record->purchase_of_land_list = ($request->purchase_of_land_list)?$request->purchase_of_land_list: 0;
        $record->purchase_of_land_add = ($request->purchase_of_land_add)?$request->purchase_of_land_add: 0;
        $record->purchase_of_land_edit = ($request->purchase_of_land_edit)?$request->purchase_of_land_edit: 0;
        $record->purchase_of_land_delete = ($request->purchase_of_land_delete)?$request->purchase_of_land_delete: 0;
        $record->purchase_of_land_print = ($request->purchase_of_land_print)?$request->purchase_of_land_print: 0;
        $record->possession_certificate_list = ($request->possession_certificate_list)?$request->possession_certificate_list: 0;
        $record->possession_certificate_add = ($request->possession_certificate_add)?$request->possession_certificate_add: 0;
        $record->possession_certificate_edit = ($request->possession_certificate_edit)?$request->possession_certificate_edit: 0;
        $record->possession_certificate_delete = ($request->possession_certificate_delete)?$request->possession_certificate_delete: 0;
        $record->possession_certificate_print = ($request->possession_certificate_print)?$request->possession_certificate_print: 0;
        $record->pictorial_view_list = ($request->pictorial_view_list)?$request->pictorial_view_list: 0;
        $record->pictorial_view_add = ($request->pictorial_view_add)?$request->pictorial_view_add: 0;
        $record->pictorial_view_edit = ($request->pictorial_view_edit)?$request->pictorial_view_edit: 0;
        $record->pictorial_view_delete = ($request->pictorial_view_delete)?$request->pictorial_view_delete: 0;
        $record->pictorial_view_print = ($request->pictorial_view_print)?$request->pictorial_view_print: 0;
        $record->conveyance_deed_list = ($request->conveyance_deed_list)?$request->conveyance_deed_list: 0;
        $record->conveyance_deed_add = ($request->conveyance_deed_add)?$request->conveyance_deed_add: 0;
        $record->conveyance_deed_edit = ($request->conveyance_deed_edit)?$request->conveyance_deed_edit: 0;
        $record->conveyance_deed_delete = ($request->conveyance_deed_delete)?$request->conveyance_deed_delete: 0;
        $record->conveyance_deed_print = ($request->conveyance_deed_print)?$request->conveyance_deed_print: 0;
        $record->agreement_list = ($request->agreement_list)?$request->agreement_list: 0;
        $record->agreement_add = ($request->agreement_add)?$request->agreement_add: 0;
        $record->agreement_edit = ($request->agreement_edit)?$request->agreement_edit: 0;
        $record->agreement_delete = ($request->agreement_delete)?$request->agreement_delete: 0;
        $record->agreement_print = ($request->agreement_print)?$request->agreement_print: 0;
        $record->indemnity_bond_list = ($request->indemnity_bond_list)?$request->indemnity_bond_list: 0;
        $record->indemnity_bond_add = ($request->indemnity_bond_add)?$request->indemnity_bond_add: 0;
        $record->indemnity_bond_edit = ($request->indemnity_bond_edit)?$request->indemnity_bond_edit: 0;
        $record->indemnity_bond_delete = ($request->indemnity_bond_delete)?$request->indemnity_bond_delete: 0;
        $record->indemnity_bond_print = ($request->indemnity_bond_print)?$request->indemnity_bond_print: 0;
        $record->registry_document_list = ($request->registry_document_list)?$request->registry_document_list: 0;
        $record->registry_document_add = ($request->registry_document_add)?$request->registry_document_add: 0;
        $record->registry_document_edit = ($request->registry_document_edit)?$request->registry_document_edit: 0;
        $record->registry_document_delete = ($request->registry_document_delete)?$request->registry_document_delete: 0;
        $record->registry_document_print = ($request->registry_document_print)?$request->registry_document_print: 0;
        $record->exemption_form_list = ($request->exemption_form_list)?$request->exemption_form_list: 0;
        $record->exemption_form_add = ($request->exemption_form_add)?$request->exemption_form_add: 0;
        $record->exemption_form_edit = ($request->exemption_form_edit)?$request->exemption_form_edit: 0;
        $record->exemption_form_delete = ($request->exemption_form_delete)?$request->exemption_form_delete: 0;
        $record->exemption_form_print = ($request->exemption_form_print)?$request->exemption_form_print: 0;
        $record->affidavit_2_list = ($request->affidavit_2_list)?$request->affidavit_2_list: 0;
        $record->affidavit_2_add = ($request->affidavit_2_add)?$request->affidavit_2_add: 0;
        $record->affidavit_2_edit = ($request->affidavit_2_edit)?$request->affidavit_2_edit: 0;
        $record->affidavit_2_delete = ($request->affidavit_2_delete)?$request->affidavit_2_delete: 0;
        $record->affidavit_2_print = ($request->affidavit_2_print)?$request->affidavit_2_print: 0;
        $record->intimation_application_list = ($request->intimation_application_list)?$request->intimation_application_list: 0;
        $record->intimation_application_add = ($request->intimation_application_add)?$request->intimation_application_add: 0;
        $record->intimation_application_edit = ($request->intimation_application_edit)?$request->intimation_application_edit: 0;
        $record->intimation_application_delete = ($request->intimation_application_delete)?$request->intimation_application_delete: 0;
        $record->intimation_application_print = ($request->intimation_application_print)?$request->intimation_application_print: 0;
        $record->intimation_letter_list = ($request->intimation_letter_list)?$request->intimation_letter_list: 0;
        $record->intimation_letter_add = ($request->intimation_letter_add)?$request->intimation_letter_add: 0;
        $record->intimation_letter_edit = ($request->intimation_letter_edit)?$request->intimation_letter_edit: 0;
        $record->intimation_letter_delete = ($request->intimation_letter_delete)?$request->intimation_letter_delete: 0;
        $record->intimation_letter_print = ($request->intimation_letter_print)?$request->intimation_letter_print: 0;
        $record->exemption_inventory_list = ($request->exemption_inventory_list)?$request->exemption_inventory_list: 0;
        $record->exemption_inventory_add = ($request->exemption_inventory_add)?$request->exemption_inventory_add: 0;
        $record->exemption_inventory_edit = ($request->exemption_inventory_edit)?$request->exemption_inventory_edit: 0;
        $record->exemption_inventory_delete = ($request->exemption_inventory_delete)?$request->exemption_inventory_delete: 0;
        $record->exemption_inventory_print = ($request->exemption_inventory_print)?$request->exemption_inventory_print: 0;





        $record->save();
        return redirect()->route('users.index')
            ->with('success', 'New User has been added successfully.');
    }


    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        //
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
//    public function edit($id)
//    {
//        //
//    }
    public function edit(User $User)
    {

//        $data['record'] = User::where('isDeleted', 0)->orderBy('id', 'desc')->get();
//        echo '<pre>'; print_r($User);exit;
        return view('pages.users.edit', compact('User'));

    }
    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, $id)
    {
        $request->validate([
            'name' => 'required',
            'email' => 'required',
        ]);
        $record = User::find($id);

//        $record = new User();
        $record->name = $request->name;
        $record->email = $request->email;
        $record->designation = $request->designation;

        if($request->password){
            $record->password = Hash::make($request->password);
        }

        $record->lp_master_data_list = ($request->lp_master_data_list)?$request->lp_master_data_list: 0;
        $record->lp_master_data_add = ($request->lp_master_data_add)?$request->lp_master_data_add: 0;
        $record->lp_master_data_edit = ($request->lp_master_data_edit)?$request->lp_master_data_edit: 0;
        $record->lp_master_data_delete = ($request->lp_master_data_delete)?$request->lp_master_data_delete: 0;
        $record->lp_master_data_print = ($request->lp_master_data_print)?$request->lp_master_data_print: 0;
        $record->exemption_rate_list = ($request->exemption_rate_list)?$request->exemption_rate_list: 0;
        $record->exemption_rate_add = ($request->exemption_rate_add)?$request->exemption_rate_add: 0;
        $record->exemption_rate_edit = ($request->exemption_rate_edit)?$request->exemption_rate_edit: 0;
        $record->exemption_rate_delete = ($request->exemption_rate_delete)?$request->exemption_rate_delete: 0;
        $record->exemption_rate_print = ($request->exemption_rate_print)?$request->exemption_rate_print: 0;
        $record->challan_fee_list = ($request->challan_fee_list)?$request->challan_fee_list: 0;
        $record->challan_fee_add = ($request->challan_fee_add)?$request->challan_fee_add: 0;
        $record->challan_fee_edit = ($request->challan_fee_edit)?$request->challan_fee_edit: 0;
        $record->challan_fee_delete = ($request->challan_fee_delete)?$request->challan_fee_delete: 0;
        $record->challan_fee_print = ($request->challan_fee_print)?$request->challan_fee_print: 0;
        $record->seller_profile_list = ($request->seller_profile_list)?$request->seller_profile_list: 0;
        $record->seller_profile_add = ($request->seller_profile_add)?$request->seller_profile_add: 0;
        $record->seller_profile_edit = ($request->seller_profile_edit)?$request->seller_profile_edit: 0;
        $record->seller_profile_delete = ($request->seller_profile_delete)?$request->seller_profile_delete: 0;
        $record->seller_profile_print = ($request->seller_profile_print)?$request->seller_profile_print: 0;
        $record->challan_form_list = ($request->challan_form_list)?$request->challan_form_list: 0;
        $record->challan_form_add = ($request->challan_form_add)?$request->challan_form_add: 0;
        $record->challan_form_edit = ($request->challan_form_edit)?$request->challan_form_edit: 0;
        $record->challan_form_delete = ($request->challan_form_delete)?$request->challan_form_delete: 0;
        $record->challan_form_print = ($request->challan_form_print)?$request->challan_form_print: 0;
        $record->land_form_seller_list = ($request->land_form_seller_list)?$request->land_form_seller_list: 0;
        $record->land_form_seller_add = ($request->land_form_seller_add)?$request->land_form_seller_add: 0;
        $record->land_form_seller_edit = ($request->land_form_seller_edit)?$request->land_form_seller_edit: 0;
        $record->land_form_seller_delete = ($request->land_form_seller_delete)?$request->land_form_seller_delete: 0;
        $record->land_form_seller_print = ($request->land_form_seller_print)?$request->land_form_seller_print: 0;
        $record->purchase_of_land_list = ($request->purchase_of_land_list)?$request->purchase_of_land_list: 0;
        $record->purchase_of_land_add = ($request->purchase_of_land_add)?$request->purchase_of_land_add: 0;
        $record->purchase_of_land_edit = ($request->purchase_of_land_edit)?$request->purchase_of_land_edit: 0;
        $record->purchase_of_land_delete = ($request->purchase_of_land_delete)?$request->purchase_of_land_delete: 0;
        $record->purchase_of_land_print = ($request->purchase_of_land_print)?$request->purchase_of_land_print: 0;
        $record->possession_certificate_list = ($request->possession_certificate_list)?$request->possession_certificate_list: 0;
        $record->possession_certificate_add = ($request->possession_certificate_add)?$request->possession_certificate_add: 0;
        $record->possession_certificate_edit = ($request->possession_certificate_edit)?$request->possession_certificate_edit: 0;
        $record->possession_certificate_delete = ($request->possession_certificate_delete)?$request->possession_certificate_delete: 0;
        $record->possession_certificate_print = ($request->possession_certificate_print)?$request->possession_certificate_print: 0;
        $record->pictorial_view_list = ($request->pictorial_view_list)?$request->pictorial_view_list: 0;
        $record->pictorial_view_add = ($request->pictorial_view_add)?$request->pictorial_view_add: 0;
        $record->pictorial_view_edit = ($request->pictorial_view_edit)?$request->pictorial_view_edit: 0;
        $record->pictorial_view_delete = ($request->pictorial_view_delete)?$request->pictorial_view_delete: 0;
        $record->pictorial_view_print = ($request->pictorial_view_print)?$request->pictorial_view_print: 0;
        $record->conveyance_deed_list = ($request->conveyance_deed_list)?$request->conveyance_deed_list: 0;
        $record->conveyance_deed_add = ($request->conveyance_deed_add)?$request->conveyance_deed_add: 0;
        $record->conveyance_deed_edit = ($request->conveyance_deed_edit)?$request->conveyance_deed_edit: 0;
        $record->conveyance_deed_delete = ($request->conveyance_deed_delete)?$request->conveyance_deed_delete: 0;
        $record->conveyance_deed_print = ($request->conveyance_deed_print)?$request->conveyance_deed_print: 0;
        $record->agreement_list = ($request->agreement_list)?$request->agreement_list: 0;
        $record->agreement_add = ($request->agreement_add)?$request->agreement_add: 0;
        $record->agreement_edit = ($request->agreement_edit)?$request->agreement_edit: 0;
        $record->agreement_delete = ($request->agreement_delete)?$request->agreement_delete: 0;
        $record->agreement_print = ($request->agreement_print)?$request->agreement_print: 0;
        $record->indemnity_bond_list = ($request->indemnity_bond_list)?$request->indemnity_bond_list: 0;
        $record->indemnity_bond_add = ($request->indemnity_bond_add)?$request->indemnity_bond_add: 0;
        $record->indemnity_bond_edit = ($request->indemnity_bond_edit)?$request->indemnity_bond_edit: 0;
        $record->indemnity_bond_delete = ($request->indemnity_bond_delete)?$request->indemnity_bond_delete: 0;
        $record->indemnity_bond_print = ($request->indemnity_bond_print)?$request->indemnity_bond_print: 0;
        $record->registry_document_list = ($request->registry_document_list)?$request->registry_document_list: 0;
        $record->registry_document_add = ($request->registry_document_add)?$request->registry_document_add: 0;
        $record->registry_document_edit = ($request->registry_document_edit)?$request->registry_document_edit: 0;
        $record->registry_document_delete = ($request->registry_document_delete)?$request->registry_document_delete: 0;
        $record->registry_document_print = ($request->registry_document_print)?$request->registry_document_print: 0;
        $record->exemption_form_list = ($request->exemption_form_list)?$request->exemption_form_list: 0;
        $record->exemption_form_add = ($request->exemption_form_add)?$request->exemption_form_add: 0;
        $record->exemption_form_edit = ($request->exemption_form_edit)?$request->exemption_form_edit: 0;
        $record->exemption_form_delete = ($request->exemption_form_delete)?$request->exemption_form_delete: 0;
        $record->exemption_form_print = ($request->exemption_form_print)?$request->exemption_form_print: 0;
        $record->affidavit_2_list = ($request->affidavit_2_list)?$request->affidavit_2_list: 0;
        $record->affidavit_2_add = ($request->affidavit_2_add)?$request->affidavit_2_add: 0;
        $record->affidavit_2_edit = ($request->affidavit_2_edit)?$request->affidavit_2_edit: 0;
        $record->affidavit_2_delete = ($request->affidavit_2_delete)?$request->affidavit_2_delete: 0;
        $record->affidavit_2_print = ($request->affidavit_2_print)?$request->affidavit_2_print: 0;
        $record->intimation_application_list = ($request->intimation_application_list)?$request->intimation_application_list: 0;
        $record->intimation_application_add = ($request->intimation_application_add)?$request->intimation_application_add: 0;
        $record->intimation_application_edit = ($request->intimation_application_edit)?$request->intimation_application_edit: 0;
        $record->intimation_application_delete = ($request->intimation_application_delete)?$request->intimation_application_delete: 0;
        $record->intimation_application_print = ($request->intimation_application_print)?$request->intimation_application_print: 0;
        $record->intimation_letter_list = ($request->intimation_letter_list)?$request->intimation_letter_list: 0;
        $record->intimation_letter_add = ($request->intimation_letter_add)?$request->intimation_letter_add: 0;
        $record->intimation_letter_edit = ($request->intimation_letter_edit)?$request->intimation_letter_edit: 0;
        $record->intimation_letter_delete = ($request->intimation_letter_delete)?$request->intimation_letter_delete: 0;
        $record->intimation_letter_print = ($request->intimation_letter_print)?$request->intimation_letter_print: 0;
        $record->exemption_inventory_list = ($request->exemption_inventory_list)?$request->exemption_inventory_list: 0;
        $record->exemption_inventory_add = ($request->exemption_inventory_add)?$request->exemption_inventory_add: 0;
        $record->exemption_inventory_edit = ($request->exemption_inventory_edit)?$request->exemption_inventory_edit: 0;
        $record->exemption_inventory_delete = ($request->exemption_inventory_delete)?$request->exemption_inventory_delete: 0;
        $record->exemption_inventory_print = ($request->exemption_inventory_print)?$request->exemption_inventory_print: 0;





        $record->save();
        return redirect()->route('users.index')
            ->with('success', 'User has been updated successfully.');
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy($id)
    {
        //
    }
}
