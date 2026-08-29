<?php

namespace App\Http\Controllers;

use App\Models\Affidavit_2;
use App\Models\Exemption_form;
use App\Models\Int_application;
use App\Models\Intimation_letter;
use App\Models\Intimation_letter_rows;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use App\Models\Land_provider;
use App\Models\Seller_profile;
use Illuminate\Http\Request;

class Int_ltr_c extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if(auth()->user()->intimation_letter_list == 1){
            $data['record'] = Intimation_letter::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            return view('pages.intimation.intimation_letter.show', $data);
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
        if(auth()->user()->intimation_letter_add == 1){
            $data['doc_no']  = Intimation_letter::latest('id')->value('id');
            $data['intimation_applicaion'] = Int_application::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            $data['exemption_form'] = Exemption_form::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            $data['affidavit'] = Affidavit_2::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            $data['land_provider'] = Land_provider::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            $data['sellers'] = Seller_profile::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();

//            echo '<pre>'; print_r($data['sellers']);exit;
            return view('pages.intimation.intimation_letter.add',$data);
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
            'date' => 'required',
            'application_no' => 'required',
//            'file_no' => 'required',
            'code_no' => 'required',
            'lo_code' => 'required',
//            'affidavit_no' => 'required',
        ]);
        $record = new Intimation_letter();
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->application_no = $request->application_no;
        $record->file_no = $request->file_no;
        $record->code_no = $request->code_no;
        $record->lo_code = $request->lo_code;
        $record->lo_name = $request->lo_name;
        $record->lo_address = $request->lo_address;
        $record->lo_father_name = $request->lo_father_name;
//        $record->purchaser = $request->purchaser;
//        $record->purchaser_address = $request->purchaser_address;
//        $record->purchaser_cnic = $request->purchaser_cnic;
        $record->district = $request->district;
        $record->tehsil = $request->tehsil;
        $record->lp_name = $request->lp_name;
        $record->lp_father_name = $request->lp_father_name;
        $record->affidavit_no = $request->affidavit_no;
        $record->createdBy =auth()->user()->id;

        $approval_check = Approval_setup_header::where('approval', 'Intimation Letter')->first();

        if($approval_check){
            $record->status = 1;
        }else{
            $record->status = 0;

        }




        $record->save();


        $lastid = $record->id;
        if($lastid){
            $line_items = $request->item_lines;
            foreach ($line_items as $line_item) {
                if ($line_item['purchaser_name']) {

                    $Exemption_form_rows = new Intimation_letter_rows();
                    $Exemption_form_rows->deed_id = $lastid;
                    $Exemption_form_rows->purchaser_name = $line_item['purchaser_name'];
                    $Exemption_form_rows->purchaser_address = $line_item['purchaser_address'];
                    $Exemption_form_rows->purchaser_cnic =$line_item['purchaser_cnic'];
                    $Exemption_form_rows->save();
                }
            }
        }
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
            return redirect()->route('intimation_letter.index')
                ->with('success','The Intimation Letter record sent for approval.');
        }else{
            return redirect()->route('intimation_letter.index')
                ->with('success','Intimation Letter has been created successfully.');
        }
//        return redirect()->route('intimation_letter.index')
//            ->with('success', 'Intimation Letter has been created successfully.');
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
    public function edit(Intimation_letter $intimation_letter)
    {
        if(auth()->user()->intimation_letter_edit == 1){
            $id = $intimation_letter->id;

            $data['intimation_applicaion'] = Int_application::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            $data['exemption_form'] = Exemption_form::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            $data['affidavit'] = Affidavit_2::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            $rows = Intimation_letter_rows::where('deed_id' , $id)->get();
            $intimation_letter['rows'] =  $rows->toArray();
            return view('pages.intimation.intimation_letter.edit',compact('intimation_letter'),$data);
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
            'date' => 'required',
            'application_no' => 'required',
//            'file_no' => 'required',
            'code_no' => 'required',
            'lo_code' => 'required',
//            'affidavit_no' => 'required',
        ]);
        $record = Intimation_letter::find($id);
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->application_no = $request->application_no;
        $record->file_no = $request->file_no;
        $record->code_no = $request->code_no;
        $record->lo_code = $request->lo_code;
        $record->lo_name = $request->lo_name;
        $record->lo_address = $request->lo_address;
        $record->lo_father_name = $request->lo_father_name;

        $record->district = $request->district;
        $record->tehsil = $request->tehsil;
        $record->lp_name = $request->lp_name;
        $record->lp_father_name = $request->lp_father_name;
        $record->affidavit_no = $request->affidavit_no;

        $record->save();

        if($id){
            $line_items = $request->item_lines;
            foreach ($line_items as $line_item) {
                if ($line_item['id']) {
                    $Intimation_letter_rows = Intimation_letter_rows::find($line_item['id']);

                    $Intimation_letter_rows->deed_id = $id;
                    $Intimation_letter_rows->purchaser_name = $line_item['purchaser_name'];
                    $Intimation_letter_rows->purchaser_address = $line_item['purchaser_address'];
                    $Intimation_letter_rows->purchaser_cnic =$line_item['purchaser_cnic'];
                    $Intimation_letter_rows->save();
                }
            }
        }
        return redirect()->route('intimation_letter.index')
            ->with('success', 'Intimation Letter has been Updated successfully.');
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

            $company = Intimation_letter::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('intimation_letter.index')
                ->with('success', 'Intimation Letter Has Been Deleted successfully');
        } else {
            return redirect()->route('intimation_letter.index')
                ->with('danger', 'Intimation Letter Not Found');
        }
    }
}
