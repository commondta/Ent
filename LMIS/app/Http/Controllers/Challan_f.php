<?php

namespace App\Http\Controllers;

use App\Models\Challan_fee;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;

use Illuminate\Http\Request;

class Challan_f extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if(auth()->user()->challan_fee_list == 1){

            $data['record'] = Challan_fee::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            return view('pages.purchasing_of_land.challan_fee.show', $data);
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
        if(auth()->user()->challan_fee_add == 1){
            $data['sr_code']  = Challan_fee::latest('id')->value('id');
            return view('pages.purchasing_of_land.challan_fee/add',$data);
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
            'sr_code' => 'required',
            'category' => 'required',
            'amount' => 'required',
        ]);
        $record = new Challan_fee();
        $record->sr_code = $request->sr_code;
        $record->category = $request->category;
        $record->amount = $request->amount;
        $record->createdBy =auth()->user()->id;

        $approval_check = Approval_setup_header::where('approval', 'Challan Fee')->first();

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

            return redirect()->route('challan_fee.index')
                ->with('success','The Challan Fee record sent for approval.');
        }else{
            return redirect()->route('challan_fee.index')
                ->with('success','Challan Fee has been created successfully.');
        }



//        return redirect()->route('challan_fee.index')
//            ->with('success', 'Challan Fee has been created successfully.');
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
    public function edit(Challan_fee $challan_fee)
    {
        if(auth()->user()->challan_fee_edit == 1){
            return view('pages.purchasing_of_land.challan_fee.edit', compact('challan_fee'));
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
            'sr_code' => 'required',
            'category' => 'required',
            'amount' => 'required',
        ]);
        $record = Challan_fee::find($id);
        $record->sr_code = $request->sr_code;
        $record->category = $request->category;
        $record->amount = $request->amount;

        $record->save();
        return redirect()->route('challan_fee.index')
            ->with('success', 'Challan Fee has been updated successfully.');
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

            $company = Challan_fee::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('challan_fee.index')
                ->with('success', 'Challan Fee Has Been Deleted successfully');
        } else {
            return redirect()->route('challan_fee.index')
                ->with('danger', 'Challan Fee Not Found');
        }
    }
}
