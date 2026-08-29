<?php

namespace App\Http\Controllers;

use App\Models\Affidavit_2;
use App\Models\Conveyance;
use App\Models\Conveyance_row;
use App\Models\Conveyance_land_fard_row;
use App\Models\Purchase_of_land;
use App\Models\Purchase_of_land_rows;
use App\Models\Land_form;
use App\Models\Land_form_row;
use App\Models\Land_form_row_detail;
use App\Models\Exemption_form;
use App\Models\Land_provider;
use Illuminate\Http\Request;

class Affidavit_2_c extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if (auth()->user()->affidavit_2_list == 1) {
            $data['record'] = Affidavit_2::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            return view('pages.exemption.affidavit_2.show', $data);
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
        if (auth()->user()->affidavit_2_add == 1) {
            $data['conveyance'] = Conveyance::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            $data['doc_no']  = Affidavit_2::latest('id')->value('id') ?? 0;
            $data['exemption_form'] = Exemption_form::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.exemption.affidavit_2.add', $data);
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
            'date' => 'required',
            'base_doc_no' => 'required',
        ]);
        $record = new Affidavit_2();
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->base_doc_no = $request->base_doc_no;
        $record->createdBy = auth()->user()->id;
        $record->save();

        return redirect()->route('affidavit_2.index')
            ->with('success', 'Affidavit has been created successfully.');
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        if (auth()->user()->affidavit_2_print == 1) {
            $data['land_p'] = Land_provider::where('isDeleted', 0)->orderBy('id', 'asc')->first();
            $data['affidavit'] = Affidavit_2::where('isDeleted', 0)->where('id', $id)->first();

            if ($data['affidavit']) {
                $conveyance_id = $data['affidavit']->base_doc_no;
                $id = $data['affidavit']->id;

                // Get conveyance document using doc_no (Agreement->base_doc_no = Conveyance->doc_no)
                $data['record'] = Conveyance::where('isDeleted', 0)->where('doc_no', $conveyance_id)->orderBy('id', 'asc')->first();

                if ($data['record']) {
                    // Get Conveyance rows
                    $data['Conveyance_row'] = Conveyance_row::where('isDeleted', 0)->where('deed_id', $data['record']->doc_no)->orderBy('id', 'asc')->get();
                    $data['Conveyance_land_fard_row'] = Conveyance_land_fard_row::where('isDeleted', 0)->where('deed_id', $data['record']->doc_no)->orderBy('id', 'asc')->first();

                    // Get Purchase of Land (Conveyance->base_doc_no = Purchase_of_land->File_No)
                    $data['purchase_doc'] = Purchase_of_land::where('isDeleted', 0)->where('File_No', $data['record']->base_doc_no)->orderBy('id', 'asc')->first();

                    if ($data['purchase_doc']) {
                        // Get Purchase of Land rows
                        $data['purchase_land_row'] = Purchase_of_land_rows::where('isDeleted', 0)->where('deed_id', $data['purchase_doc']->id)->orderBy('id', 'asc')->get();

                        // Get Land Form (Purchase_of_land->land_form_no = Land_form->doc_no)
                        $data['land_form'] = Land_form::where('isDeleted', 0)->where('doc_no', $data['purchase_doc']->land_form_no)->orderBy('id', 'asc')->first();

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

                return view('pages.exemption.affidavit_2.layout', $data);
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
    public function edit(Affidavit_2 $affidavit_2)
    {
        if (auth()->user()->affidavit_2_edit == 1) {
            $id = $affidavit_2->id;

            $data['record'] =  Affidavit_2::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            $data['conveyance'] = Conveyance::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.exemption.affidavit_2.edit', compact('affidavit_2'), $data);
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
            'date' => 'required',
            'base_doc_no' => 'required',
        ]);
        $record = Affidavit_2::find($id);
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->base_doc_no = $request->base_doc_no;
        $record->save();
        return redirect()->route('affidavit_2.index')
            ->with('success', 'Affidavit has been Updated successfully.');
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

            $company = Affidavit_2::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('affidavit_2.index')
                ->with('success', 'Affidavit Has Been Deleted successfully');
        } else {
            return redirect()->route('affidavit_2.index')
                ->with('danger', 'Affidavit Not Found');
        }
    }
}
