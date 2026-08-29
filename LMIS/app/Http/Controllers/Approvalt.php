<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\Approval_tree;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
class Approvalt extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {


        if(auth()->user()->is_admin == 1){
            $data['record'] = Approval_tree::where('isDeleted', 0)->orderBy('id', 'desc')->first();
//         echo '<pre>';   print_r($data);exit;
            return view('pages.approvals.tree.show',$data);
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
        //
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        //
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
    public function edit($id)
    {
        //
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
        $record = Approval_tree::find($id);
        $record->lp_master_data = ($request->lp_master_data)?$request->lp_master_data: 0;
        $record->exemption_rate = ($request->exemption_rate)?$request->exemption_rate: 0;
        $record->challan_fee = ($request->challan_fee)?$request->challan_fee: 0;
        $record->seller_profile = ($request->seller_profile)?$request->seller_profile: 0;
        $record->challan_form = ($request->challan_form)?$request->challan_form: 0;
        $record->land_form_seller = ($request->land_form_seller)?$request->land_form_seller: 0;
        $record->purchase_of_land = ($request->purchase_of_land)?$request->purchase_of_land: 0;
        $record->possession_certificate = ($request->possession_certificate)?$request->possession_certificate: 0;
        $record->pictorial_view = ($request->pictorial_view)?$request->pictorial_view: 0;
        $record->conveyance_deed = ($request->conveyance_deed)?$request->conveyance_deed: 0;
        $record->agreement = ($request->agreement)?$request->agreement: 0;
        $record->indemnity_bond = ($request->indemnity_bond)?$request->indemnity_bond: 0;
        $record->registry_document = ($request->registry_document)?$request->registry_document: 0;
        $record->exemption_form = ($request->exemption_form)?$request->exemption_form: 0;
        $record->affidavit_2 = ($request->affidavit_2)?$request->affidavit_2: 0;
        $record->intimation_application = ($request->intimation_application)?$request->intimation_application: 0;
        $record->intimation_letter = ($request->intimation_letter)?$request->intimation_letter: 0;
        $record->exemption_inventory = ($request->exemption_inventory)?$request->exemption_inventory: 0;

        $record->save();
        return redirect()->route('approval_tree.index')
            ->with('success', 'Approval Tree has been updated successfully.');
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
