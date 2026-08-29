<?php

namespace App\Http\Controllers;

use App\Models\Exemption_form;
use App\Models\Exemption_form_rows;
use App\Models\Purchase_of_land;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Http\Request;

class Exemption_f extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if(auth()->user()->exemption_form_list == 1){
            $data['record'] = Exemption_form::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            return view('pages.exemption.exemption_form.show', $data);
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
        if(auth()->user()->exemption_form_add == 1){
            $data['doc_no']  = Exemption_form::latest('doc_no')->value('doc_no');
            $data['purchase_of_land'] = Purchase_of_land::where('isDeleted',0)->where('status',0)->orderBy('id', 'desc')->get();
            return view('pages.exemption.exemption_form.add', $data);
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
            'base_doc_no' => 'required',
        ]);
        $record = new Exemption_form();
        $record->doc_no = $request->doc_no;
        $record->file_no = $request->base_doc_no;
        $record->date = $request->date;
        $record->base_doc_no = $request->base_doc_no;
        $record->file_no = $request->file_no;
        $record->lo_name = $request->lo_name;
        $record->lp_name = $request->lp_name;
        $record->so = $request->so;
        $record->reg_no = $request->reg_no;
        $record->mouza = $request->mouza;
        $record->reg_date = $request->reg_date;
//        $record->marla = $request->marla;
        $record->exemption_rate = $request->exemption_rate;
//        $record->sq_feet = $request->sq_feet;
        $record->total_files = $request->total_files;
//        $record->kanal = $request->kanal;
//        $record->khewat = $request->khewat;
//        $record->qatat = $request->qatat;
//        $record->khatooni = $request->khatooni;
        $record->file_security = $request->file_security;
        $record->balance = $request->balance;
        $record->transfer_of_decimals = $request->transfer_of_decimals;
        $record->createdBy =auth()->user()->id;




        $approval_check = Approval_setup_header::where('approval', 'Exemption Form')->first();

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
                if ($line_item['khewat_no']) {

                    $Exemption_form_rows = new Exemption_form_rows();
                    $Exemption_form_rows->deed_id = $lastid;
                    $Exemption_form_rows->khewat_no = $line_item['khewat_no'];
                    $Exemption_form_rows->khatooni_no = $line_item['khatooni_no'];
                    $Exemption_form_rows->qatat =$line_item['qatat'];
                    $Exemption_form_rows->kanal = $line_item['kanal'];
                    $Exemption_form_rows->marla = $line_item['marla'];
                    $Exemption_form_rows->sq_feet = $line_item['sq_feet'];
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

            return redirect()->route('exemption_form.index')
                ->with('success','The Exemption Form record sent for approval.');
        }else{
            return redirect()->route('exemption_form.index')
                ->with('success','Exemption Form has been created successfully.');
        }




//        return redirect()->route('exemption_form.index')
//            ->with('success', 'Exemption Form has been created successfully.');
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
    public function edit(Exemption_form $exemption_form)
    {
        if(auth()->user()->exemption_form_edit == 1){
            $id = $exemption_form->id;

            $rows = Exemption_form_rows::where('deed_id' , $id)->get();
            $exemption_form['rows'] =  $rows->toArray();
            $data['purchase_of_land'] = Purchase_of_land::where('isDeleted',0)->where('status',0)->orderBy('id', 'desc')->get();
            return view('pages.exemption.exemption_form.edit',compact('exemption_form'),$data);
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
            'base_doc_no' => 'required',
        ]);
        $record = Exemption_form::find($id);
        $record->doc_no = $request->doc_no;
        $record->file_no = $request->base_doc_no;
        $record->date = $request->date;
        $record->base_doc_no = $request->base_doc_no;
        $record->file_no = $request->file_no;
        $record->lo_name = $request->lo_name;
        $record->lp_name = $request->lp_name;
        $record->so = $request->so;
        $record->reg_no = $request->reg_no;
        $record->mouza = $request->mouza;
        $record->reg_date = $request->reg_date;
//        $record->marla = $request->marla;
        $record->exemption_rate = $request->exemption_rate;
//        $record->sq_feet = $request->sq_feet;
        $record->total_files = $request->total_files;
//        $record->kanal = $request->kanal;
//        $record->khewat = $request->khewat;
//        $record->qatat = $request->qatat;
//        $record->khatooni = $request->khatooni;
        $record->file_security = $request->file_security;
        $record->balance = $request->balance;
        $record->transfer_of_decimals = $request->transfer_of_decimals;

        $record->save();

        $line_items = $request->item_lines;
        foreach ($line_items as $line_item) {
            if ($line_item['khewat_no']) {

                if (isset($line_item['id'])) {
                    $Exemption_form_rows = Exemption_form_rows::find($line_item['id']);
                }else{
                    $Exemption_form_rows = new Exemption_form_rows();
                }


//                $Sellere_profile_land_row->deed_id = $id;
                $Exemption_form_rows->khewat_no = $line_item['khewat_no'];
                $Exemption_form_rows->khatooni_no = $line_item['khatooni_no'];
                $Exemption_form_rows->qatat =$line_item['qatat'];
                $Exemption_form_rows->kanal = $line_item['kanal'];
                $Exemption_form_rows->marla = $line_item['marla'];
                $Exemption_form_rows->sq_feet = $line_item['sq_feet'];
                $Exemption_form_rows->save();



            }
        }
        return redirect()->route('exemption_form.index')
            ->with('success', 'Exemption Form has been Updated successfully.');
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

            $company = Exemption_form::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('exemption_form.index')
                ->with('success', 'Exemption Form Has Been Deleted successfully');
        } else {
            return redirect()->route('exemption_form.index')
                ->with('danger', 'Exemption Form Not Found');
        }
    }
}
