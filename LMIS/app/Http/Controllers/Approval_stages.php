<?php

namespace App\Http\Controllers;

use App\Models\Approval_setup_header;
use Illuminate\Http\Request;
use App\Models\Approval_stage;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;

class Approval_stages extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if(auth()->user()->is_admin == 1){
            $data['record'] = Approval_stage::where('isDeleted', 0)->orderBy('id','desc')->get();
            return view('pages.approvals.stages.show',$data);
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
        if(auth()->user()->is_admin == 1){
            return view('pages.approvals.stages.add');

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
            'name' => 'required',
            'no_of_approvals' => 'required',
        ]);
        $record = new Approval_stage();
        $record->name = $request->name;
        $record->no_of_approvals = $request->no_of_approvals;

        $record->save();
        return redirect()->route('approval_stage.index')
            ->with('success', 'Stage has been created successfully.');
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
    public function edit(Approval_stage $approval_stage)
    {
        if(auth()->user()->is_admin == 1){

            return view('pages.approvals.stages.edit', compact('approval_stage'));
        }else{
            return view('pages.authrization.show');
        }

    }
    public function approval_stage_delete($id){
        print_r($id);exit;
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
            'no_of_approvals' => 'required',
        ]);
        $record = Approval_stage::find($id);
        $record->name = $request->name;
        $record->no_of_approvals = $request->no_of_approvals;

        $record->save();
        return redirect()->route('approval_stage.index')
            ->with('success', 'Approval Stage has been updated successfully.');
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy($id)
    {
//        print_r($id);exit;
        if ($id) {

            $company = Approval_stage::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('approval_stage.index')
                ->with('success', 'Approval Stage Has Been Deleted successfully');
        } else {
            return redirect()->route('approval_stage.index')
                ->with('danger', 'Approval Stage Not Found');
        }
    }
}
