<?php

namespace App\Http\Controllers;

use App\Models\Exemption_rate;
use App\Models\Document_approval;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use Illuminate\Http\Request;

class Exption_r extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if(auth()->user()->exemption_rate_list == 1){

            $data['record'] = Exemption_rate::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            return view('pages.purchasing_of_land.exemption_rate.show', $data);

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
        $data['doc_num']  = Exemption_rate::latest('id')->value('id');
        return view('pages.purchasing_of_land.exemption_rate.add',$data);
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
            'mouza_code' => 'required',
            'mouza_name' => 'required',
            'exemption_rate' => 'required',
        ]);
        $Exemption_rate = new Exemption_rate();
        $Exemption_rate->mouza_code = $request->mouza_code;
        $Exemption_rate->mouza_name = $request->mouza_name;
        $Exemption_rate->exemption_rate = $request->exemption_rate;
        $Exemption_rate->createdBy =auth()->user()->id;

        $approval_check = Approval_setup_header::where('approval', 'Exemption Rate')->where('isDeleted', 0)->first();

        if($approval_check){
            $Exemption_rate->status = 1;
        }else{
            $Exemption_rate->status = 0;

        }



        $Exemption_rate->save();

        $lastid = $Exemption_rate->id;


        if($approval_check) {

            $Approval_setup_lines = Approval_setup_line::where('isDeleted', 0)->where('main', $approval_check->id)->get();
            $count = 1;
            foreach ($Approval_setup_lines as $Approval_setup_line) {
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

            return redirect()->route('exemption_rate.index')
                ->with('success','Exemption Rate record sent for approval.');
        }else{
            return redirect()->route('exemption_rate.index')
                ->with('success', 'Exemption Rate has been created successfully.');

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
        //
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit(Exemption_rate $exemption_rate)
    {
        return view('pages.purchasing_of_land.exemption_rate.edit', compact('exemption_rate'));

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
            'mouza_code' => 'required',
            'mouza_name' => 'required',
            'exemption_rate' => 'required',
        ]);
        $Exemption_rate = Exemption_rate::find($id);
        $Exemption_rate->mouza_code = $request->mouza_code;
        $Exemption_rate->mouza_name = $request->mouza_name;
        $Exemption_rate->exemption_rate = $request->exemption_rate;

        $Exemption_rate->save();
        return redirect()->route('exemption_rate.index')
            ->with('success', 'Exemption Rate has been Updated successfully.');
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

            $company = Exemption_rate::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('exemption_rate.index')
                ->with('success', 'Exemption Rate Has Been Deleted successfully');
        } else {
            return redirect()->route('exemption_rate.index')
                ->with('danger', 'Exemption Rate Not Found');
        }
    }
}
