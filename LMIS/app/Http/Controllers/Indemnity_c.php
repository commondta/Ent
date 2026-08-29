<?php

namespace App\Http\Controllers;

use App\Models\Land_provider;
use App\Models\Conveyance;
use App\Models\Conveyance_row;
use App\Models\Conveyance_land_fard_row;
use App\Models\Purchase_of_land;
use App\Models\Purchase_of_land_rows;
use App\Models\Land_form;
use App\Models\Land_form_row;
use App\Models\Land_form_row_detail;
use App\Models\Indemnity_bond;
use Illuminate\Http\Request;

class Indemnity_c extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if (auth()->user()->indemnity_bond_list == 1) {
            $data['record'] = Indemnity_bond::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            return view('pages.registry.indemnity.show', $data);
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
        if (auth()->user()->indemnity_bond_add == 1) {
            $data['doc_no']  = Indemnity_bond::latest('id')->value('id') ?? 0;
            $data['conveyance'] = Conveyance::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.registry.indemnity.add', $data);
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
        ]);
        $record = new Indemnity_bond();
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->base_doc_no = $request->base_doc_no;
        $record->date_of_execution = $request->date_of_execution;
        $record->createdBy = auth()->user()->id;
        $record->save();
        return redirect()->route('indemnity_bond.index')
            ->with('success', 'Indemnity Bond has been created successfully.');
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
            $data['land_p'] = Land_provider::where('isDeleted', 0)->orderBy('id', 'desc')->first();
            $data['Indemnity_bond'] = Indemnity_bond::where('isDeleted', 0)->where('id', $id)->orderBy('id', 'desc')->first();

            if ($data['Indemnity_bond']) {
                $conveyance_id = $data['Indemnity_bond']->base_doc_no;
                $id = $data['Indemnity_bond']->id;

                // Get conveyance document using doc_no (Agreement->base_doc_no = Conveyance->doc_no)
                $data['record'] = Conveyance::where('isDeleted', 0)->where('doc_no', $conveyance_id)->orderBy('id', 'desc')->first();
                
                if ($data['record']) {
                    // Get Conveyance rows
                    $data['Conveyance_row'] = Conveyance_row::where('isDeleted', 0)->where('deed_id', $data['record']->doc_no)->orderBy('id', 'desc')->get();
                    $data['Conveyance_land_fard_row'] = Conveyance_land_fard_row::where('isDeleted', 0)->where('deed_id', $data['record']->doc_no)->orderBy('id', 'desc')->first();
                    
                    // Get Purchase of Land (Conveyance->base_doc_no = Purchase_of_land->File_No)
                    $data['purchase_doc'] = Purchase_of_land::where('isDeleted', 0)->where('File_No', $data['record']->base_doc_no)->orderBy('id', 'desc')->first();
                    
                    if ($data['purchase_doc']) {
                        // Get Purchase of Land rows
                        $data['purchase_land_row'] = Purchase_of_land_rows::where('isDeleted', 0)->where('deed_id', $data['purchase_doc']->id)->orderBy('id', 'desc')->get();
                        
                        // Get Land Form (Purchase_of_land->land_form_no = Land_form->doc_no)
                        $data['land_form'] = Land_form::where('isDeleted', 0)->where('doc_no', $data['purchase_doc']->land_form_no)->orderBy('id', 'desc')->first();
                        
                        if ($data['land_form']) {
                            // Get Land Owners and Land Form Details
                            $data['land_owners'] = Land_form_row::where('land_form_id', $data['land_form']->id)->orderBy('lo_cod', 'asc')->get();
                            $data['land_form_details'] = Land_form_row_detail::where('land_form_id', $data['land_form']->id)->orderBy('lo_cod', 'asc')->get();
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

                return view('pages.registry.indemnity.layout', $data);
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
    public function edit(Indemnity_bond $indemnity_bond)
    {
        if (auth()->user()->indemnity_bond_edit == 1) {
            $data['conveyance'] = Conveyance::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.registry.indemnity.edit', compact('indemnity_bond'), $data);
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
        ]);
        $record = Indemnity_bond::find($id);
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->base_doc_no = $request->base_doc_no;
        $record->date_of_execution = $request->date_of_execution;
        $record->save();
        return redirect()->route('indemnity_bond.index')
            ->with('success', 'Indemnity Bond has been Updated successfully.');
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

            $company = Indemnity_bond::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('indemnity_bond.index')
                ->with('success', 'Indemnity Bond Has Been Deleted successfully');
        } else {
            return redirect()->route('indemnity_bond.index')
                ->with('danger', 'Indemnity Bond Not Found');
        }
    }
}
