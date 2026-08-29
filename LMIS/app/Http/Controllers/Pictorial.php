<?php

namespace App\Http\Controllers;

use App\Models\Pictorial_view;
use App\Models\Possession_certificate;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Http\Request;

class Pictorial extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if(auth()->user()->pictorial_view_list == 1){
            $data['record'] = Pictorial_view::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            return view('pages.purchasing_of_land.pictorial.show', $data);
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
        if(auth()->user()->pictorial_view_add == 1){
            $data['doc_num']  = Pictorial_view::latest('doc_no')->value('doc_no');
            $data['possession_certificate'] = Possession_certificate::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.purchasing_of_land.pictorial.add', $data);
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
            'pc_no' => 'required',
            'lo_name' => 'required',
            'chak' => 'required',
            'lp_name' => 'required',
            'name_of_patwari' => 'required',
            'kanal' => 'required',
            'possession_jco' => 'required',
            'marla' => 'required',
            'signature1' => 'required',
            'signature2' => 'required',
        ]);
        $record = new Pictorial_view();
        $record->doc_no = $request->doc_no;
        $record->pc_no = $request->pc_no;
        $record->lo_name = $request->lo_name;
        $record->chak = $request->chak;
        $record->lp_name = $request->lp_name;
        $record->area = $request->area;
        $record->name_of_patwari = $request->name_of_patwari;
        $record->kanal = $request->kanal;
        $record->possession_jco = $request->possession_jco;
        $record->marla = $request->marla;
        $record->signature1 = $request->signature1;
        $record->signature2 = $request->signature2;
        $record->createdBy =auth()->user()->id;

        $attachment = $request->file('picture');
        if($attachment){
            $imageName5 = time(). '_' . uniqid() . '.' .  $attachment->getClientOriginalExtension();
            if( $attachment->move(public_path('assets/uploads'), $imageName5)){
                $record->picture = $imageName5;
            }
        }

        $approval_check = Approval_setup_header::where('approval', 'Pictorial View')->first();

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

            return redirect()->route('pictorial_view.index')
                ->with('success','The Pictorial View record sent for approval.');
        }else{
            return redirect()->route('pictorial_view.index')
                ->with('success','Pictorial View has been created successfully.');
        }


//        return redirect()->route('pictorial_view.index')
//            ->with('success', 'Pictorial View has been created successfully.');
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
    public function edit(Pictorial_view $pictorial_view)
    {
        if(auth()->user()->pictorial_view_edit == 1){
            $data['possession_certificate'] = Possession_certificate::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            return view('pages.purchasing_of_land.pictorial.edit',compact('pictorial_view'),$data);
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
            'pc_no' => 'required',
            'lo_name' => 'required',
            'chak' => 'required',
            'lp_name' => 'required',
            'name_of_patwari' => 'required',
            'kanal' => 'required',
            'possession_jco' => 'required',
            'marla' => 'required',
            'signature1' => 'required',
            'signature2' => 'required',
        ]);
        $record = Pictorial_view::find($id);
        $record->doc_no = $request->doc_no;
        $record->pc_no = $request->pc_no;
        $record->lo_name = $request->lo_name;
        $record->chak = $request->chak;
        $record->lp_name = $request->lp_name;
        $record->area = $request->area;
        $record->name_of_patwari = $request->name_of_patwari;
        $record->kanal = $request->kanal;
        $record->possession_jco = $request->possession_jco;
        $record->marla = $request->marla;
        $record->signature1 = $request->signature1;
        $record->signature2 = $request->signature2;
        $attachment = $request->file('picture');
        if($attachment){
            $imageName5 = time(). '_' . uniqid() . '.' .  $attachment->getClientOriginalExtension();
            if( $attachment->move(public_path('assets/uploads'), $imageName5)){
                $record->picture = $imageName5;
            }
        }
        $record->save();
        return redirect()->route('pictorial_view.index')
            ->with('success', 'Pictorial View has been Updated successfully.');
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

            $company = Pictorial_view::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('pictorial_view.index')
                ->with('success', 'Pictorial View Has Been Deleted successfully');
        } else {
            return redirect()->route('pictorial_view.index')
                ->with('danger', 'Pictorial View Not Found');
        }
    }
}
