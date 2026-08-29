<?php

namespace App\Http\Controllers;

use App\Models\Conveyance;
use App\Models\Conveyance_row;
use App\Models\Conveyance_land_fard_row;
use App\Models\Conveyance_land_row;
use App\Models\Land_provider;
use App\Models\Purchase_of_land;
use App\Models\Purchase_of_land_rows;
use App\Models\Land_form;
use App\Models\Land_form_row;
use App\Models\Land_form_row_detail;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use App\Services\ConveyanceReportsService;
use Illuminate\Http\Request;
use Barryvdh\DomPDF\Facade\Pdf;

class Conveyance_d extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if (auth()->user()->conveyance_deed_list == 1) {
            $data['record'] = Conveyance::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.registry.conveyance.show', $data);
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
        if (auth()->user()->conveyance_deed_add == 1) {
            $data['doc_no']  = Conveyance::latest('id')->value('id') ?? 0;
            $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.registry.conveyance.add', $data);
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
            'doc_no' => 'required|unique:conveyances,doc_no',
            'date' => 'required',
            'base_doc_no' => 'required',
            'date_of_creation' => 'required',
            'deed_executed_by_lo_name' => 'required',
            'vendee_witness_name' => 'required',
            'vendor_relationship' => 'required',
            'vendee_relationship' => 'required',
            'vendee_witness_father_name' => 'required',
            'vendee_witness_cnic' => 'required',
            'vendee_witness_caste' => 'required',
            'vendee_witness_address' => 'required',
        ]);
        $record = new Conveyance();
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->base_doc_no = $request->base_doc_no;
        $record->date_of_creation = $request->date_of_creation;
        $record->district = $request->district;
        $record->tehsil = $request->tehsil;
        $record->scheme = $request->scheme;
        $record->fixed_deed_rs = $request->fixed_deed_rs;
        $record->stamp_paper_value = $request->stamp_paper_value;
        $record->schedule_year = $request->schedule_year;
        $record->record_of_rights_year = $request->record_of_rights_year;
        $record->deed_executed_by_lo_name = $request->deed_executed_by_lo_name;
        $record->deed_executed_by_lo_father_name = $request->deed_executed_by_lo_father_name;
        $record->deed_executed_by_cnic = $request->deed_executed_by_cnic;
        $record->deed_executed_by_caste = $request->deed_executed_by_caste;
        $record->deed_executed_by_address = $request->deed_executed_by_address;
        $record->deed_in_favor_of_name = $request->deed_in_favor_of_name;

        $record->rep_cnic = $request->rep_cnic;
        $record->vendor_relationship = $request->vendor_relationship;
        $record->vendee_relationship = $request->vendee_relationship;
        $record->vendee_witness_name = $request->vendee_witness_name;
        $record->vendee_witness_father_name = $request->vendee_witness_father_name;
        $record->vendee_witness_cnic = $request->vendee_witness_cnic;
        $record->vendee_witness_caste = $request->vendee_witness_caste;
        $record->vendee_witness_address = $request->vendee_witness_address;
        $record->createdBy = auth()->user()->id;






        $approval_check = Approval_setup_header::where('approval', 'Conveyance Deed')->first();

        if ($approval_check) {
            $record->status = 1;
        } else {
            $record->status = 0;
        }
        $record->save();
        $lastInsertedId = $record->id;
        if ($lastInsertedId) {
            $line_items = $request->land_details;
            foreach ($line_items as $line_item) {
                $conveyance_row = new Conveyance_row();
                $conveyance_row->deed_id = $lastInsertedId;
                $conveyance_row->block_no = $line_item['block_no'];
                $conveyance_row->rectangle_no = $line_item['rectangle_no'];
                $conveyance_row->khasra_no = $line_item['khasra_no'];

                $conveyance_row->east_by = $line_item['east_by'];
                $conveyance_row->west_by = $line_item['west_by'];
                $conveyance_row->north_by = $line_item['north_by'];
                $conveyance_row->south_by = $line_item['south_by'];
                $conveyance_row->save();
            }

            $fard_item_lines = $request->fard_item_lines;
            foreach ($fard_item_lines as $line_item) {


                $Conveyance_land_fard_row = new Conveyance_land_fard_row();
                $Conveyance_land_fard_row->deed_id = $lastInsertedId;
                $Conveyance_land_fard_row->vide_fad_id_no = $line_item['vide_fad_id_no'];
                $Conveyance_land_fard_row->vide_fad_id_no_2 = $line_item['vide_fad_id_no_2'];
                $Conveyance_land_fard_row->date = $line_item['date'];
                $Conveyance_land_fard_row->date_2 = $line_item['date_2'];
                $Conveyance_land_fard_row->save();
            }
        }



        if ($approval_check) {
            $count = 1;
            $Approval_setup_lines = Approval_setup_line::where('isDeleted', 0)->where('main', $approval_check->id)->get();
            foreach ($Approval_setup_lines as $Approval_setup_line) {
                $document_approval = new Document_approval();
                $document_approval->document_name = $approval_check->approval;
                $document_approval->document_id = $lastInsertedId;
                $document_approval->priority = $count;
                $document_approval->approval_user_id = $Approval_setup_line->user;
                $document_approval->status = $Approval_setup_line->status;
                $document_approval->remarks = '';
                $document_approval->save();
                $count++;
            }

            return redirect()->route('conveyance.index')
                ->with('success', 'The Conveyance record sent for approval.');
        } else {
            return redirect()->route('conveyance.index')
                ->with('success', 'Conveyance has been created successfully.');
        }




        //        return redirect()->route('conveyance.index')
        //            ->with('success', 'Conveyance Deed has been created successfully.');
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        if (auth()->user()->conveyance_deed_print == 1) {
            $data['record'] = Conveyance::where('isDeleted', 0)->where('id', $id)->orderBy('id', 'asc')->first();

            if ($data['record']) {
                $base_doc_no = $data['record']->base_doc_no;
                $id = $data['record']->id;
                $data['purchase_doc'] = Purchase_of_land::where('isDeleted', 0)->where('File_No', $base_doc_no)->where('status', 0)->orderBy('id', 'asc')->first();
                $data['land_p'] = Land_provider::where('isDeleted', 0)->where('lp_cod', $data['purchase_doc']->lp_name)->first();
               // echo '<pre>'; print_r($data['land_p']);exit;

                // echo '<pre>';  print_r($data['purchase_doc']);exit;

                if ($data['purchase_doc']) {
                    $data['purchase_land_row'] = Purchase_of_land_rows::where('isDeleted', 0)->where('deed_id', $data['purchase_doc']->id)->orderBy('id', 'asc')->get();
                } else {
                    $data['purchase_land_row'] = collect();
                }

                // Get Land Form and Land Owners
                $data['land_form'] = Land_form::where('isDeleted', 0)->where('doc_no', $data['purchase_doc']->land_form_no)->orderBy('id', 'asc')->first();
                //echo '<pre>';  print_r($data['land_form']);exit;

                if ($data['land_form']) {
                    $data['land_owners'] = Land_form_row::where('land_form_id', $data['land_form']->id)->orderBy('id', 'asc')->get();
                    $data['land_form_details'] = Land_form_row_detail::where('land_form_id', $data['land_form']->id)->orderBy('id', 'asc')->get();
                    // echo '<pre>';  print_r($data['land_owners']);exit;
                } else {
                    $data['land_owners'] = collect();
                    $data['land_form_details'] = collect();
                }
                //  echo '<pre>';  print_r($data['land_form_details']);exit;

                $data['block'] = Conveyance_row::where('isDeleted', 0)->where('deed_id', $id)->orderBy('id', 'asc')->get();
                $data['fard_row'] = Conveyance_land_fard_row::where('isDeleted', 0)->where('deed_id', $id)->orderBy('id', 'asc')->first();
                $data['land_row'] = Conveyance_row::where('isDeleted', 0)->where('deed_id', $id)->orderBy('id', 'asc')->get();
            }
            return view('pages.registry.conveyance.layout', $data);
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
    public function edit(Conveyance $conveyance)
    {

        if (auth()->user()->conveyance_deed_edit == 1) {

            if ($conveyance->id) {
                $id = $conveyance->id;
                $rows = Conveyance_row::where('deed_id', $id)->get();
                $data['landDetails'] = $rows;

                $fard = Conveyance_land_fard_row::where('deed_id', $id)->get();
                $land = Conveyance_row::where('deed_id', $id)->get();
                // $land = Conveyance_land_row::where('deed_id', $id)->get();
                $conveyance['rows'] =  $rows->toArray();
                $conveyance['land'] =  $land->toArray();
                $conveyance['fard'] =  $fard->toArray();
            }

            $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.registry.conveyance.edit', compact('conveyance'), $data);
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
            'date_of_creation' => 'required',
            'deed_executed_by_lo_name' => 'required',
            'vendor_relationship' => 'required',
            'vendee_relationship' => 'required',

            'vendee_witness_name' => 'required',
            'vendee_witness_father_name' => 'required',
            'vendee_witness_cnic' => 'required',
            'vendee_witness_caste' => 'required',
            'vendee_witness_address' => 'required',
        ]);

        $record = Conveyance::find($id);
        if (!$record) {
            return redirect()->back()->with('error', 'Record not found.');
        }

        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->base_doc_no = $request->base_doc_no;
        $record->date_of_creation = $request->date_of_creation;
        $record->district = $request->district;
        $record->tehsil = $request->tehsil;

        if ($request->scheme) {
            $record->scheme = $request->scheme;
        }

        $record->fixed_deed_rs = $request->fixed_deed_rs;
        $record->stamp_paper_value = $request->stamp_paper_value;
        $record->schedule_year = $request->schedule_year;
        $record->record_of_rights_year = $request->record_of_rights_year;
        $record->deed_executed_by_lo_name = $request->deed_executed_by_lo_name;
        $record->deed_executed_by_lo_father_name = $request->deed_executed_by_lo_father_name;
        $record->deed_executed_by_cnic = $request->deed_executed_by_cnic;
        $record->deed_executed_by_caste = $request->deed_executed_by_caste;
        $record->deed_executed_by_address = $request->deed_executed_by_address;
        $record->deed_in_favor_of_name = $request->deed_in_favor_of_name;
        $record->vendor_relationship = $request->vendor_relationship;
        $record->vendee_relationship = $request->vendee_relationship;

        $record->rep_cnic = $request->rep_cnic;
        $record->vendee_witness_name = $request->vendee_witness_name;
        $record->vendee_witness_father_name = $request->vendee_witness_father_name;
        $record->vendee_witness_cnic = $request->vendee_witness_cnic;
        $record->vendee_witness_caste = $request->vendee_witness_caste;
        $record->vendee_witness_address = $request->vendee_witness_address;
        $record->save();

        if ($id) {

            // ================= LAND DETAILS =================
            Conveyance_row::where('deed_id', $id)->delete();
            $line_items = $request->land_details ?? [];

            foreach ($line_items as $line_item) {
                $conveyance_row = null;
                if (isset($line_item['id'])) {
                    $conveyance_row = Conveyance_row::find($line_item['id']);
                }

                if (!$conveyance_row) {
                    $conveyance_row = new Conveyance_row();
                }

                $conveyance_row->deed_id = $id;
                $conveyance_row->block_no = $line_item['block_no'] ?? null;
                $conveyance_row->rectangle_no = $line_item['rectangle_no'] ?? null;
                $conveyance_row->khasra_no = $line_item['khasra_no'] ?? null;

                $conveyance_row->east_by = $line_item['east_by'] ?? null;
                $conveyance_row->west_by = $line_item['west_by'] ?? null;
                $conveyance_row->north_by = $line_item['north_by'] ?? null;
                $conveyance_row->south_by = $line_item['south_by'] ?? null;
                $conveyance_row->save();
            }

            // ================= FARD DETAILS =================
            Conveyance_land_fard_row::where('deed_id', $id)->delete();
            $fard_item_lines = $request->fard_item_lines ?? [];

            foreach ($fard_item_lines as $line_item) {


                $Conveyance_land_fard_row = null;

                if (isset($line_item['id'])) {
                    $Conveyance_land_fard_row = Conveyance_land_fard_row::find($line_item['id']);
                }

                if (!$Conveyance_land_fard_row) {
                    $Conveyance_land_fard_row = new Conveyance_land_fard_row();
                }

                $Conveyance_land_fard_row->deed_id = $id;
                $Conveyance_land_fard_row->vide_fad_id_no = $line_item['vide_fad_id_no'] ?? null;
                $Conveyance_land_fard_row->vide_fad_id_no_2 = $line_item['vide_fad_id_no_2'] ?? null;
                $Conveyance_land_fard_row->date = $line_item['date'] ?? null;
                $Conveyance_land_fard_row->date_2 = $line_item['date_2'] ?? null;
                $Conveyance_land_fard_row->save();
            }
        }

        return redirect()->route('conveyance.index')
            ->with('success', 'Conveyance Deed has been updated successfully.');
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

            $company = Conveyance::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('conveyance.index')
                ->with('success', 'Conveyance Deed Has Been Deleted successfully');
        } else {
            return redirect()->route('conveyance.index')
                ->with('danger', 'Conveyance Deed Not Found');
        }
    }

    /**
     * Download Conveyance Deed Report (PDF)
     * 
     * @param int $id - Conveyance Deed ID
     * @return PDF|Redirect
     */
    public function downloadConveyance($id)
    {
        $reportsService = new ConveyanceReportsService();
        $data = $reportsService->getConveyanceData($id);

        if (!$data) {
            return redirect()->back()->with('danger', 'Conveyance Deed not found');
        }

        $fileName = $reportsService->getFileName($data['conveyance'], 'conveyance_deed');

        $pdf = Pdf::loadView('reports.conveyance', $data);
        return $pdf->download($fileName);
    }

    /**
     * Download Sale Agreement Report (PDF)
     * Auto-generated from Conveyance Deed data
     * 
     * @param int $id - Conveyance Deed ID
     * @return PDF|Redirect
     */
    public function downloadSaleAgreement($id)
    {
        $reportsService = new ConveyanceReportsService();
        $data = $reportsService->getSaleAgreementData($id);

        if (!$data) {
            return redirect()->back()->with('danger', 'Could not generate Sale Agreement');
        }

        $fileName = $reportsService->getFileName($data['conveyance'], 'sale_agreement');

        $pdf = Pdf::loadView('reports.sale-agreement', $data);
        return $pdf->download($fileName);
    }

    /**
     * Download Affidavit Report for a specific Land Owner (PDF)
     * 
     * @param int $id - Conveyance Deed ID
     * @param string|null $loCode - Land Owner Code
     * @return PDF|Redirect
     */
    public function downloadAffidavit($id, $loCode = null)
    {
        $reportsService = new ConveyanceReportsService();
        $data = $reportsService->getAffidavitData($id, $loCode);

        if (!$data) {
            return redirect()->back()->with('danger', 'Could not generate Affidavit');
        }

        $fileName = $reportsService->getFileName($data['conveyance'], 'affidavit', $loCode);

        $pdf = Pdf::loadView('reports.affidavit', $data);
        return $pdf->download($fileName);
    }

    /**
     * Download Undertaking Report for a specific Land Owner (PDF)
     * Conditional - only if the land provider is NOT the organisation itself
     * 
     * @param int $id - Conveyance Deed ID
     * @param string|null $loCode - Land Owner Code
     * @return PDF|Redirect
     */
    public function downloadUndertaking($id, $loCode = null)
    {
        $reportsService = new ConveyanceReportsService();

        if (!$reportsService->isUndertakingRequired($id)) {
            return redirect()->back()->with('warning', 'Undertaking not required for ' . config('app.org_label') . ' Land Provider');
        }

        $data = $reportsService->getUndertakingData($id, $loCode);

        if (!$data) {
            return redirect()->back()->with('danger', 'Could not generate Undertaking');
        }

        $fileName = $reportsService->getFileName($data['conveyance'], 'undertaking', $loCode);

        $pdf = Pdf::loadView('reports.undertaking', $data);
        return $pdf->download($fileName);
    }

    /**
     * Download Indemnity Bond Report for a specific Land Owner (PDF)
     * 
     * @param int $id - Conveyance Deed ID
     * @param string|null $loCode - Land Owner Code
     * @return PDF|Redirect
     */
    public function downloadIndemnityBond($id, $loCode = null)
    {
        $reportsService = new ConveyanceReportsService();
        $data = $reportsService->getIndemnityBondData($id, $loCode);

        if (!$data) {
            return redirect()->back()->with('danger', 'Could not generate Indemnity Bond');
        }

        $fileName = $reportsService->getFileName($data['conveyance'], 'indemnity_bond', $loCode);

        $pdf = Pdf::loadView('reports.indemnity-bond', $data);
        return $pdf->download($fileName);
    }

    /**
     * View document bundle page showing all available reports
     * Displays list of all Land Owners with download options
     * 
     * @param int $id - Conveyance Deed ID
     * @return View
     */
    public function viewDocumentBundle($id)
    {
        $reportsService = new ConveyanceReportsService();
        $bundle = $reportsService->getDocumentBundle($id);

        if (isset($bundle['error'])) {
            return redirect()->back()->with('danger', $bundle['error']);
        }

        return view('reports.bundle', compact('bundle'));
    }
}
