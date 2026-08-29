<?php

namespace App\Http\Controllers;

use App\Models\Land_provider;
use App\Models\Agreement;
use App\Models\Conveyance;
use App\Models\Conveyance_row;
use App\Models\Conveyance_land_fard_row;
use App\Models\Purchase_of_land;
use App\Models\Purchase_of_land_rows;
use App\Models\Land_form;
use App\Models\Land_form_row;
use App\Models\Land_form_row_detail;
use Illuminate\Http\Request;

class Agreement_c extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if (auth()->user()->agreement_list == 1) {
            $data['record'] = Agreement::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            //echo '<pre>';  print_r($data['record']);exit;
            return view('pages.registry.agreement.show', $data);
        } else {
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

        if (auth()->user()->agreement_add == 1) {
            $data['doc_no']  = Agreement::latest('id')->value('id') ?? 0;
            $data['conveyance'] = Conveyance::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.registry.agreement.add', $data);
        } else {
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
            'witness1_rank' => 'required',
            'witness1_appointment' => 'required',
            'witness1_name' => 'required',
            'witness2_rank' => 'required',
            'witness2_appointment' => 'required',
            'witness2_name' => 'required'
        ]);

        $record = new Agreement();
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->base_doc_no = $request->base_doc_no;
        $record->agreement_date = $request->agreement_date;
        $record->witness1_rank = $request->witness1_rank;
        $record->witness1_appointment = $request->witness1_appointment;
        $record->witness1_name = $request->witness1_name;
        $record->witness2_rank = $request->witness2_rank;
        $record->witness2_appointment = $request->witness2_appointment;
        $record->witness2_name = $request->witness2_name;
        $record->is_land_provider = $request->is_land_provider ?? 0;

        $record->createdBy = auth()->user()->id;
        // echo '<pre>';  print_r($record);exit;

        $record->save();
        return redirect()->route('agreement.index')
            ->with('success', 'Agreement has been created successfully.');
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        if (auth()->user()->agreement_print == 1) {
            $data['land_p'] = Land_provider::where('isDeleted', 0)->orderBy('id', 'asc')->first();
            $data['agreement'] = Agreement::where('isDeleted', 0)->where('id', $id)->orderBy('id', 'asc')->first();

            if ($data['agreement']) {
                $conveyance_id = $data['agreement']->base_doc_no;
                $id = $data['agreement']->id;
                // Get conveyance document using doc_no (Agreement->base_doc_no = Conveyance->doc_no)
                $data['record'] = Conveyance::where('isDeleted', 0)->where('doc_no', $conveyance_id)->orderBy('id', 'asc')->first();

                if ($data['record']) {
                    // Get Conveyance rows
                    $data['Conveyance_row'] = Conveyance_row::where('isDeleted', 0)->where('deed_id', $data['record']->doc_no)->orderBy('id', 'asc')->get();
                    $data['Conveyance_land_fard_row'] = Conveyance_land_fard_row::where('isDeleted', 0)->where('deed_id', $data['record']->doc_no)->orderBy('id', 'asc')->first();

                    // Get Purchase of Land (Conveyance->base_doc_no = Purchase_of_land->File_No)
                    $data['purchase_doc'] = Purchase_of_land::where('isDeleted', 0)->where('File_No', $data['record']->base_doc_no)->orderBy('id', 'asc')->first();
                    $data['land_p'] = Land_provider::where('isDeleted', 0)->where('lp_cod', $data['purchase_doc']->lp_name)->first();
                    //echo '<pre>'; print_r($data['land_p']);exit;


                    if ($data['purchase_doc']) {
                        // Get Purchase of Land rows
                        $data['purchase_land_row'] = Purchase_of_land_rows::where('isDeleted', 0)->where('deed_id', $data['purchase_doc']->id)->orderBy('id', 'asc')->get();

                        // Get Land Form (Purchase_of_land->land_form_no = Land_form->doc_no)
                        $data['land_form'] = Land_form::where('isDeleted', 0)->where('doc_no', $data['purchase_doc']->land_form_no)->orderBy('id', 'asc')->first();

                        if ($data['land_form']) {
                            // Get Land Owners and Land Form Details
                            $data['land_owners'] = Land_form_row::where('land_form_id', $data['land_form']->id)->orderBy('id', 'asc')->get();
                            $data['land_form_details'] = Land_form_row_detail::where('land_form_id', $data['land_form']->id)->orderBy('id', 'asc')->get();
                        } else {
                            $data['land_owners'] = collect();
                            $data['land_form_details'] = collect();
                        }
                    } else {
                        $data['purchase_land_row'] = collect();
                        $data['land_owners'] = collect();
                        $data['land_form_details'] = collect();
                        $data['Conveyance_land_fard_row'] = null;
                        $data['land_form'] = null;
                    }
                } else {
                    $data['Conveyance_row'] = collect();
                    $data['Conveyance_land_fard_row'] = collect();
                    $data['purchase_land_row'] = collect();
                    $data['land_owners'] = collect();
                    $data['land_form_details'] = collect();
                    $data['land_form'] = null;
                }

                return view('pages.registry.agreement.layout', $data);
            }
        } else {
            return view('pages.authrization.show');
        }
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit(Agreement $agreement)
    {
        if (auth()->user()->agreement_edit == 1) {

            $data['conveyance'] = Conveyance::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.registry.agreement.edit', compact('agreement'), $data);
        } else {
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
            'witness1_rank' => 'required',
            'witness1_appointment' => 'required',
            'witness1_name' => 'required',
            'witness2_rank' => 'required',
            'witness2_appointment' => 'required',
            'witness2_name' => 'required'
        ]);

        // Fetch the existing record
        $record = Agreement::findOrFail($id);

        // Update basic fields
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->base_doc_no = $request->base_doc_no;
        $record->agreement_date = $request->agreement_date;
        $record->witness1_rank = $request->witness1_rank;
        $record->witness1_appointment = $request->witness1_appointment;
        $record->witness1_name = $request->witness1_name;
        $record->witness2_rank = $request->witness2_rank;
        $record->witness2_appointment = $request->witness2_appointment;
        $record->witness2_name = $request->witness2_name;
        $record->is_land_provider = $request->is_land_provider ?? 0;
        $record->save();
        return redirect()->route('agreement.index')
            ->with('success', 'Agreement has been updated successfully.');
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

            $company = Agreement::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('agreement.index')
                ->with('success', 'Agreement Has Been Deleted successfully');
        } else {
            return redirect()->route('agreement.index')
                ->with('danger', 'Agreement Not Found');
        }
    }
}
