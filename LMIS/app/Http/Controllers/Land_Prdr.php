<?php

namespace App\Http\Controllers;

use App\Models\Land_provider;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Support\Str;
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
use App\Models\Document_approval_history;
use App\Models\Exemption_rate;
use App\Models\Challan_fee;
use App\Models\Exemption_form;
use App\Models\Affidavit_2;
use App\Models\Intimation_letter;
use App\Models\Int_application;

class Land_Prdr extends MY_Controller
{

    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if(auth()->user()->lp_master_data_list == 1){

            $data['record'] = Land_provider::where('isDeleted', 0)
                ->where('status', 0)
                ->orderBy('id', 'desc')
                ->get();
            return view('pages.purchasing_of_land.land_provider.show', $data);
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
        $data['doc_num']  = (int) Land_provider::latest('id')->value('id') ?? 0;
        $data['lp_code']  = (int) Land_provider::latest('id')->value('id') ?? 0;
        return view('pages.purchasing_of_land.land_provider.add',$data);
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
            'lp_cod' => 'required',
            'doc_no' => 'required|unique:land_providers,doc_no',
            'lp_name' => 'required',
            'relationship' => 'required',
            'lp_cnic' => 'required|digits:13',
            'contact_no' => 'required',
            'address' => 'required',
            'ntn_no' => 'required',
            'father_name' => 'required',
            'security_deposited' => 'required',
//            'file' => 'required|mimes:pdf,xlx,csv|max:2048',
        ]);

//
        $status = 0;
//        $approval_check = Approval_setup_header::where('approval', 'LP Master Data')->first();
//
//        if($approval_check){
//            $data['total_approvals'] = Approval_setup_line::where('isDeleted', 0)->where('main', $approval_check->id)->count();
//
//            $status = $data['total_approvals'];
//
//
//
//
////            $approval_check = Document_approval::where('document_name',  $approval_check->approval)->first();
//
//        }


        /*
            Write Code Here for
            Store $fileName name in DATABASE from HERE
        */
        $userid = auth()->user()->id;

        $land_provider = new land_provider;
        $land_provider->lp_cod = $request->lp_cod;
        $land_provider->doc_no = $request->doc_no;
        $land_provider->lp_name = $request->lp_name;
        $land_provider->relationship = $request->relationship;
        $land_provider->lp_cnic = $request->lp_cnic;
        $land_provider->contact_no = $request->contact_no;
        $land_provider->address = $request->address;
        $land_provider->tem_address = $request->tem_address;
        $land_provider->ntn_no = $request->ntn_no;
        $land_provider->father_name = $request->father_name;
        $land_provider->security_deposited = $request->security_deposited;
        $land_provider->createdBy =auth()->user()->id;

        if ($request->hasFile('attachments')) {
            $image = $request->file('attachments');
            $imageName = 'profile_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $land_provider->attachments = $imageName;
        }
        if ($request->hasFile('cnic_front_attachments')) {
            $image = $request->file('cnic_front_attachments');
            $imageName = 'cnic_front_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $land_provider->cnic_front_attachments = $imageName;
        }
        if ($request->hasFile('cnic_back_attachments')) {
            $image = $request->file('cnic_back_attachments');
            $imageName = 'cnic_back_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $land_provider->cnic_back_attachments = $imageName;
        }

        $approval_check = Approval_setup_header::where('isDeleted', 0)->where('approval', 'LP Master Data')->first();

        if($approval_check){
            $land_provider->status = 1;
        }else{
            $land_provider->status = 0;

        }

        $land_provider->save();
        $lastid = $land_provider->id;

        if($approval_check){
            $count = 1;
            $Approval_setup_lines = Approval_setup_line::where('isDeleted', 0)->where('main', $approval_check->id)->get();
            foreach($Approval_setup_lines as $Approval_setup_line){
                $document_approval = new Document_approval();
                $document_approval->document_name = $approval_check->approval;
                $document_approval->document_id = $lastid;
                $document_approval->priority = $count;
                $document_approval->approval_user_id = $Approval_setup_line->user;
                $document_approval->status = $Approval_setup_line->status;
                $document_approval->remarks = '';
                $document_approval->save();
                $count++;
            }









            return redirect()->route('land_provider.index')
                ->with('success','The land provider record sent for approval.');
        }else{
            return redirect()->route('land_provider.index')
                ->with('success','Land Provider has been created successfully.');
        }
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {

    }
    public function print_pld(){

        $data['record'] = Land_provider::where('isDeleted', 0)
            ->where('status', 0)
            ->orderBy('id', 'desc')
            ->get();

        $approval_documents = Document_approval::orderBy('id', 'desc')
            ->where('status', '!=', 1)
            ->get();

// Get an array of document IDs where status is not equal to 1
        $documentIds = $approval_documents->pluck('document_id')->toArray();

// Filter out the records
        $data['record'] = $data['record']->filter(function ($item) use ($documentIds) {
            return !in_array($item->id, $documentIds);
        });




//        $data['record'] = Land_provider::where('isDeleted',0)->orderBy('id','desc')->get();
        return view('pages.purchasing_of_land.land_provider.print_pld', $data);
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit(Land_provider $Land_provider)
    {
//        echo '<pre>';print_r($Land_provider);exit;

        return view('pages.purchasing_of_land.land_provider.edit',compact('Land_provider'));

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
            'lp_cod' => 'required',
            'doc_no' => 'required',
            'lp_name' => 'required',
            'relationship' => 'required',
            'lp_cnic' => 'required',
            'contact_no' => 'required',
            'address' => 'required',
            'ntn_no' => 'required',
            'father_name' => 'required',
            'security_deposited' => 'required',
        ]);
       // dd($request->all());
        $land_provider = Land_provider::find($id);
        $land_provider->lp_cod = $request->lp_cod;
        $land_provider->doc_no = $request->doc_no;
        $land_provider->lp_name = $request->lp_name;
        $land_provider->relationship = $request->relationship;
        $land_provider->lp_cnic = $request->lp_cnic;
        $land_provider->contact_no = $request->contact_no;
        $land_provider->address = $request->address;
        $land_provider->tem_address = $request->tem_address;
        $land_provider->ntn_no = $request->ntn_no;
        $land_provider->father_name = $request->father_name;
        $land_provider->security_deposited = $request->security_deposited;
        if ($request->hasFile('attachments')) {
            $image = $request->file('attachments');
            $imageName = 'profile_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $land_provider->attachments = $imageName;
        }
        if ($request->hasFile('cnic_front_attachments')) {
            $image = $request->file('cnic_front_attachments');
            $imageName = 'cnic_front_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $land_provider->cnic_front_attachments = $imageName;
        }
        if ($request->hasFile('cnic_back_attachments')) {
            $image = $request->file('cnic_back_attachments');
            $imageName = 'cnic_back_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $land_provider->cnic_back_attachments = $imageName;
        }
        $land_provider->save();
        return redirect()->route('land_provider.index')
            ->with('success','Land Provider has been Updated successfully.');
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy($id)
    {
        if($id){

            $company = Land_provider::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('land_provider.index')
                ->with('success','Land Provider Has Been Deleted successfully');
        }else{
            return redirect()->route('land_provider.index')
                ->with('danger','Land Record Provider Not Found');
        }
    }
}
