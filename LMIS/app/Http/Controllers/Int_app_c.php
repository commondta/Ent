<?php

namespace App\Http\Controllers;

use App\Models\Int_application;
use App\Models\Seller_profile;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Http\Request;

class Int_app_c extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if(auth()->user()->intimation_application_list == 1){
            $data['record'] = Int_application::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            return view('pages.intimation.intimation_application.show', $data);
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
        if(auth()->user()->intimation_application_add == 1){
            $data['doc_no']  = Int_application::latest('id')->value('id');
            $data['seller_profile'] = Seller_profile::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            return view('pages.intimation.intimation_application.add',$data);
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
            'file_no' => 'required',
            'doc_no' => 'required',
            'date' => 'required',
            'lo_code' => 'required',
            'lo_name' => 'required',
            'lo_cnic' => 'required',
            'lo_father_name' => 'required',
            'code_no' => 'required',
        ]);
        $record = new Int_application();
        $record->file_no = $request->file_no;
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->lo_code = $request->lo_code;
        $record->lo_name = $request->lo_name;
        $record->lo_cnic = $request->lo_cnic;
        $record->lo_father_name = $request->lo_father_name;
        $record->code_no = $request->code_no;
        $record->createdBy =auth()->user()->id;

        $attachment = $request->file('attachment');
        if($attachment){
            $imageName5 = time(). '_' . uniqid() . '.' .  $attachment->getClientOriginalExtension();
            if( $attachment->move(public_path('assets/uploads'), $imageName5)){
                $record->attachment = $imageName5;
            }
        }

        if($request->approval_check == 1){
            $record->status = 1;
        }else{
            $record->status = 0;

        }




        $record->save();


        $lastid = $record->id;

        if($request->approval_check == 1){
            $count = 1;
            $Approval_setup_lines = Approval_setup_line::where('isDeleted', 0)->where('main', $request->approval_check)->get();
            foreach($Approval_setup_lines as $Approval_setup_line){
                $document_approval = new Document_approval();
                $document_approval->document_name = 'intimation_application';
                $document_approval->document_id = $lastid;
                $document_approval->priority = $count;
                $document_approval->approval_user_id = $Approval_setup_line->user;
                $document_approval->status = $Approval_setup_line->status;
                $document_approval->remarks = '';
                $document_approval->save();
                $count++;
            }

            return redirect()->route('intimation_application.index')
                ->with('success','The Intimation Application record sent for approval.');
        }else{
            return redirect()->route('intimation_application.index')
                ->with('success','Intimation Application has been created successfully.');
        }









//        return redirect()->route('intimation_application.index')
//            ->with('success', 'Intimation Application has been created successfully.');
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
    public function edit(Int_application $intimation_application)
    {
        if(auth()->user()->intimation_application_edit == 1){
            $data['seller_profile'] = Seller_profile::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            return view('pages.intimation.intimation_application.edit',compact('intimation_application'),$data);
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
            'file_no' => 'required',
            'doc_no' => 'required',
            'date' => 'required',
            'lo_code' => 'required',
            'lo_name' => 'required',
            'lo_cnic' => 'required',
            'lo_father_name' => 'required',
            'code_no' => 'required',
        ]);
        $record = Int_application::find($id);
        $record->file_no = $request->file_no;
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->lo_code = $request->lo_code;
        $record->lo_name = $request->lo_name;
        $record->lo_cnic = $request->lo_cnic;
        $record->lo_father_name = $request->lo_father_name;
        $record->code_no = $request->code_no;
        $attachment = $request->file('attachment');
        if($attachment){
            $imageName5 = time(). '_' . uniqid() . '.' .  $attachment->getClientOriginalExtension();
            if( $attachment->move(public_path('assets/uploads'), $imageName5)){
                $record->attachment = $imageName5;
            }
        }
        $record->save();
        return redirect()->route('intimation_application.index')
            ->with('success', 'Intimation Application has been Updated successfully.');
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

            $company = Int_application::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('intimation_application.index')
                ->with('success', 'Intimation Application Has Been Deleted successfully');
        } else {
            return redirect()->route('intimation_application.index')
                ->with('danger', 'Intimation Application Not Found');
        }
    }
}
