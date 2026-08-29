<?php

namespace App\Http\Controllers;

use App\Models\Registry_document;
use App\Models\Seller_profile;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Http\Request;
use App\Models\Purchase_of_land;


class Registry_c extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if(auth()->user()->registry_document_list == 1){
            $data['record'] = Registry_document::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            return view('pages.registry.document.show', $data);
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
        if(auth()->user()->registry_document_add == 1){
            $data['doc_no']  = Registry_document::latest('id')->value('id') ?? 0;
             $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();            

            return view('pages.registry.document.add', $data);
        }else{
            return view('pages.authrization.show');
        }
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
            'doc_no' => 'required',
            'base_doc_no' => 'required',
            'registry_no' => 'required',
            'registry_date' => 'required',
            'mutation_no' => 'required',
            'mutation_date' => 'required',
            'land_challan_no' => 'required',
            'land_challan_date' => 'required',
            'land_challan_amount' => 'required',
            'newspaper_challan_no' => 'required',
            'newspaper_challan_date' => 'required', 
            'newspaper_challan_amount' => 'required',

        ]);
        $record = new Registry_document();
        $record->doc_no = $request->doc_no;
        $record->base_doc_no = $request->base_doc_no;
        $record->date = $request->date;
        $record->registry_no = $request->registry_no;
        $record->registry_date = $request->registry_date;
        $record->mutation_no = $request->mutation_no;
        $record->mutation_date = $request->mutation_date;
        $record->land_challan_no = $request->land_challan_no;
        $record->land_challan_date = $request->land_challan_date;
        $record->land_challan_amount = $request->land_challan_amount;
        $record->newspaper_challan_no = $request->newspaper_challan_no;
        $record->newspaper_challan_date = $request->newspaper_challan_date;
        $record->newspaper_challan_amount = $request->newspaper_challan_amount;

        $record->createdBy =auth()->user()->id;

        $indemnity_bond = $request->file('indemnity_bond');
        if($indemnity_bond){
            $imageName = time() . '_' . uniqid() . '.' .  $indemnity_bond->getClientOriginalExtension();
            if( $indemnity_bond->move(public_path('assets/uploads'), $imageName)){
                $record->indemnity_bond = $imageName;
            }
        } 
        $agreement = $request->file('agreement');
        if($agreement){
            $imageName1 = time() . '_' . uniqid() . '.' .  $agreement->getClientOriginalExtension();
            if( $agreement->move(public_path('assets/uploads'), $imageName1)){
                $record->agreement = $imageName1;
            }
        }
        $undertaking = $request->file('undertaking');
        if($undertaking){
            $imageName2 = time() . '_' . uniqid() . '.' .  $undertaking->getClientOriginalExtension();
            if( $undertaking->move(public_path('assets/uploads'), $imageName2)){
                $record->undertaking = $imageName2;
            }
        }
        $registry = $request->file('registry');
        if($registry){
            $imageName3 = time() . '_' . uniqid() . '.' .  $registry->getClientOriginalExtension();
            if( $registry->move(public_path('assets/uploads'), $imageName3)){
                $record->registry = $imageName3;
            }
        }
        $mutation = $request->file('mutation');
        if($mutation){
            $imageName4 = time() . '_' . uniqid() . '.' .  $mutation->getClientOriginalExtension();
            if( $mutation->move(public_path('assets/uploads'), $imageName4)){
                $record->mutation = $imageName4;
            }
        }
        $afidavit = $request->file('afidavit');
        if($afidavit){
            $imageName5 = time() . '_' . uniqid() . '.' .  $afidavit->getClientOriginalExtension();
            if( $afidavit->move(public_path('assets/uploads'), $imageName5)){
                $record->afidavit = $imageName5;
            }
        }


        $approval_check = Approval_setup_header::where('approval', 'Registry Document')->first();

        if($approval_check){
            $record->status = 1;
        }else{
            $record->status = 0;

        }




        $record->save();


        $lastid = $record->id;

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

            return redirect()->route('registry_document.index')
                ->with('success','The Registry Document record sent for approval.');
        }else{
            return redirect()->route('registry_document.index')
                ->with('success','Registry Document has been created successfully.');
        }





//        return redirect()->route('registry_document.index')
//            ->with('success', 'Registry Document Reciving  has been added successfully.');
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
    public function edit(Registry_document $registry_document)
    {
        if(auth()->user()->registry_document_edit == 1){
            $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.registry.document.edit',compact('registry_document'),$data);
        }else{
            return view('pages.authrization.show');
        }

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
            'doc_no' => 'required',
            'base_doc_no' => 'required',
            'registry_no' => 'required',
            'registry_date' => 'required',
            'mutation_no' => 'required',
            'mutation_date' => 'required',
             'land_challan_no' => 'required',
            'land_challan_date' => 'required',
            'land_challan_amount' => 'required',
            'newspaper_challan_no' => 'required',
            'newspaper_challan_date' => 'required', 
            'newspaper_challan_amount' => 'required',
        ]);
        $record = Registry_document::find($id);
        $record->doc_no = $request->doc_no;
        $record->base_doc_no = $request->base_doc_no;
        $record->date = $request->date;
        $record->registry_no = $request->registry_no;
        $record->registry_date = $request->registry_date;
        $record->mutation_no = $request->mutation_no;
        $record->mutation_date = $request->mutation_date;
        $record->land_challan_no = $request->land_challan_no;
        $record->land_challan_date = $request->land_challan_date;
        $record->land_challan_amount = $request->land_challan_amount;
        $record->newspaper_challan_no = $request->newspaper_challan_no;
        $record->newspaper_challan_date = $request->newspaper_challan_date;
        $record->newspaper_challan_amount = $request->newspaper_challan_amount;
            

        $indemnity_bond = $request->file('indemnity_bond');
        if($indemnity_bond){
            $imageName = time() . '_' . uniqid() . '.' .  $indemnity_bond->getClientOriginalExtension();
            if( $indemnity_bond->move(public_path('assets/uploads'), $imageName)){
                $record->indemnity_bond = $imageName;
            }
        }
        $agreement = $request->file('agreement');
        if($agreement){
            $imageName1 = time() . '_' . uniqid() . '.' .  $agreement->getClientOriginalExtension();
            if( $agreement->move(public_path('assets/uploads'), $imageName1)){
                $record->agreement = $imageName1;
            }
        }
        $undertaking = $request->file('undertaking');
        if($undertaking){
            $imageName2 = time() . '_' . uniqid() . '.' .  $undertaking->getClientOriginalExtension();
            if( $undertaking->move(public_path('assets/uploads'), $imageName2)){
                $record->undertaking = $imageName2;
            }
        }
        $registry = $request->file('registry');
        if($registry){
            $imageName3 = time() . '_' . uniqid() . '.' .  $registry->getClientOriginalExtension();
            if( $registry->move(public_path('assets/uploads'), $imageName3)){
                $record->registry = $imageName3;
            }
        }
        $mutation = $request->file('mutation');
        if($mutation){
            $imageName4 = time() . '_' . uniqid() . '.' .  $mutation->getClientOriginalExtension();
            if( $mutation->move(public_path('assets/uploads'), $imageName4)){
                $record->mutation = $imageName4;
            }
        }
        $afidavit = $request->file('afidavit');
        if($afidavit){
            $imageName5 = time() . '_' . uniqid() . '.' .  $afidavit->getClientOriginalExtension();
            if( $afidavit->move(public_path('assets/uploads'), $imageName5)){
                $record->afidavit = $imageName5;
            }
        }
        $record->save();
        return redirect()->route('registry_document.index')
            ->with('success', 'Registry Document Reciving  has been Updated successfully.');
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy($id)
    {
        if ($id) {

            $company = Registry_document::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('registry_document.index')
                ->with('success', 'Registry Document Has Been Deleted successfully');
        } else {
            return redirect()->route('registry_document.index')
                ->with('danger', 'Registry Document Not Found');
        }
    }
}
