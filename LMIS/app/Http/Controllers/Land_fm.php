<?php

namespace App\Http\Controllers;

use Illuminate\Support\Str;
use App\Models\Land_form;
use App\Models\Land_form_row;
use App\Models\Land_form_row_detail;
use App\Models\Seller_profile;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Http\Request;

class Land_fm extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if (auth()->user()->land_form_seller_list == 1) {
            $data['record'] = Land_form::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            // echo '<pre>'; print_r($data['record']); exit;
            foreach ($data['record'] as $row) {
                // Load Land Owner records for this Purchase of Land
                $row->loRows = Land_form_row::where('land_form_id', $row->id)->get();
            }

            return view('pages.purchasing_of_land.land_form.show', $data);
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
        if (auth()->user()->land_form_seller_add == 1) {
            $data['record'] = Seller_profile::where('isDeleted', 0)->where('status', 0)->distinct()->orderBy('id', 'desc')->get();
            //       echo '<pre>';      print_r($data['record']);exit;
            $data['doc_num']  = Land_form::latest('id')->value('id') ?? 0;
            return view('pages.purchasing_of_land.land_form.add', $data);
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

        //  echo '<pre>'; print_r($request);exit;
        $request->validate([
            'doc_date' => 'required',
            'doc_no' => 'required',
           
            'mouza' => 'required',
            'sector' => 'required',
            'tehsil' => 'required',
            'district' => 'required',
            'rate_per_acre' => 'required',
        ]);
        $Land_form = new Land_form();
        $Land_form->doc_date = $request->doc_date;
        $Land_form->doc_no = $request->doc_no;
        $Land_form->total_kanal = $request->total_kanal;
        $Land_form->total_marla = $request->total_marla;
        $Land_form->total_sqft = $request->total_sqft;
        $Land_form->total_acre = $request->total_acre;

    
        $Land_form->mouza = $request->mouza;
        $Land_form->sector = $request->sector;
        $Land_form->tehsil = $request->tehsil;
        $Land_form->district = $request->district;
        $Land_form->rate_per_acre = $request->rate_per_acre;
        $Land_form->poa_lo_code = is_array($request->poa_lo_code) ? implode(',', $request->poa_lo_code) : $request->poa_lo_code;
        $Land_form->poa_name = $request->poa_name;
        $Land_form->relationship = $request->relationship;
        $Land_form->poa_father_name = $request->poa_father_name;
        $Land_form->poa_cnic = $request->poa_cnic;
        $Land_form->poa_caste = $request->poa_caste;
        $Land_form->poa_current_address = $request->poa_current_address;
        $Land_form->poa_permanent_address = $request->poa_permanent_address;
        $Land_form->poa_remarks = $request->poa_remarks;
        $Land_form->createdBy = auth()->user()->id;

        if ($request->hasFile('attachments')) {
            $image = $request->file('attachments');
            $imageName = 'profile_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $Land_form->attachments = $imageName;
        }

        if ($request->hasFile('cnic_front_attachments')) {
            $image = $request->file('cnic_front_attachments');
            $imageName = 'cnic_front_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $Land_form->cnic_front_attachments = $imageName;
        }
        if ($request->hasFile('cnic_back_attachments')) {
            $image = $request->file('cnic_back_attachments');
            $imageName = 'cnic_back_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $Land_form->cnic_back_attachments = $imageName;
        }

        $approval_check = Approval_setup_header::where('approval', 'Land Form Seller')->first();

        if ($approval_check) {
            $Land_form->status = 1;
        } else {
            $Land_form->status = 0;
        }
        //echo '<pre>'; print_r($Land_form);exit;
        $Land_form->save();
        $lastid = $Land_form->id;
        if ($lastid) {
            // Handle item_lines (new table format)
            $lo_lines = $request->lo_lines;

            if ($lo_lines && is_array($lo_lines)) {
                foreach ($lo_lines as $row) {
                    if (!empty($row['lo_cod'])) {

                        $land_form_row = new Land_form_row();
                        $land_form_row->land_form_id = $lastid;
                        $land_form_row->lo_cod = $row['lo_cod'] ?? null;
                        $land_form_row->lo_name = $row['lo_name'] ?? null;
                        $land_form_row->relationship_revenue = $row['relationship_revenue'] ?? null;
                        $land_form_row->so = $row['so'] ?? null;
                        $land_form_row->lo_name_as_per_cnic = $row['lo_name_as_per_cnic'] ?? null;
                        $land_form_row->relationship_cnic = $row['relationship_cnic'] ?? null;
                        $land_form_row->father_name_cnic = $row['father_name_cnic'] ?? null;
                        $land_form_row->lo_cnic = $row['lo_cnic'] ?? null;
                        $land_form_row->caste = $row['caste'] ?? null;
                        $land_form_row->contact_no = $row['contact_no'] ?? null;
                        $land_form_row->address = $row['address'] ?? null;

                        $land_form_row->save();
                    }
                }
            }

            // echo '<pre>'; print_r($item_lines);exit;
        }

        if ($lastid) {
            $item_lines = $request->item_lines;

            if ($item_lines && is_array($item_lines)) {
                foreach ($item_lines as $row) {
                    if (
                        !empty($row['lo_cod']) ||
                        !empty($row['khewat_no']) ||
                        !empty($row['khatooni_no']) ||
                        !empty($row['qatat'])
                    ) {
                        $land_row = new Land_form_row_detail();
                        $land_row->land_form_id = $lastid;

                        $land_row->lo_cod = $row['lo_cod'] ?? null;
                        $land_row->khewat_no = $row['khewat_no'] ?? null;
                        $land_row->khatooni_no = $row['khatooni_no'] ?? null;
                        $land_row->qatat = $row['qatat'] ?? null;

                        $land_row->measuring_k = $row['measuring_k'] ?? null;
                        $land_row->measuring_m = $row['measuring_m'] ?? null;
                        $land_row->measuring_sqft = $row['measuring_sqft'] ?? null;

                        $land_row->transfer_share = $row['transfer_share'] ?? null;

                        $land_row->land_measuring_k = $row['land_measuring_k'] ?? null;
                        $land_row->land_measuring_m = $row['land_measuring_m'] ?? null;
                        $land_row->land_measuring_sqft = $row['land_measuring_sqft'] ?? null;

                        $land_row->land_category = $row['land_category'] ?? null;

                        $land_row->save();
                    }
                }
            }
        }
        //echo '<pre>'; print_r($request->all());exit;



        if ($approval_check) {
            $count = 1;
            $Approval_setup_lines = Approval_setup_line::where('isDeleted', 0)->where('main', $approval_check->id)->get();
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

            return redirect()->route('land_form.index')
                ->with('success', 'The Land Form record sent for approval.');
        } else {
            return redirect()->route('land_form.index')
                ->with('success', 'Land Form has been created successfully.');
        }






        //        return redirect()->route('land_form.index')
        //            ->with('success', 'Land Form has been created successfully.');
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
       
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit(Land_form $Land_form)
    {
        if (auth()->user()->land_form_seller_edit == 1) {

            // Load relationships properly
            $Land_form->load(['rows', 'lo_lines']);
            // echo '<pre>'; print_r($Land_form->toArray());exit;

            $record = Seller_profile::where('isDeleted', 0)
                ->orderBy('id', 'desc')
                ->get();

            return view(
                'pages.purchasing_of_land.land_form.edit',
                compact('Land_form', 'record')
            );
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
            'doc_date' => 'required',
            'doc_no' => 'required',
          
            'mouza' => 'required',
            'sector' => 'required',
            'tehsil' => 'required',
            'district' => 'required',
            'rate_per_acre' => 'required',
        ]);

        $Land_form = Land_form::findOrFail($id);

        // ---------- UPDATE MASTER ----------
        $Land_form->update([
            'doc_date' => $request->doc_date,
            'doc_no' => $request->doc_no,
            'total_kanal' => $request->total_kanal,
            'total_marla' => $request->total_marla,
            'total_sqft' => $request->total_sqft,
            'total_acre' => $request->total_acre,
            'mouza' => $request->mouza,
            'sector' => $request->sector,
            'tehsil' => $request->tehsil,
            'district' => $request->district,
            'rate_per_acre' => $request->rate_per_acre,
            'poa_lo_code' => is_array($request->poa_lo_code) ? implode(',', $request->poa_lo_code) : $request->poa_lo_code,
            'poa_name' => $request->poa_name,
            'relationship' => $request->relationship,
            'poa_father_name' => $request->poa_father_name,
            'poa_cnic' => $request->poa_cnic,
            'poa_caste' => $request->poa_caste,
            'poa_current_address' => $request->poa_current_address,
            'poa_permanent_address' => $request->poa_permanent_address,
            'poa_remarks' => $request->poa_remarks,

        ]);
        if ($request->hasFile('attachments')) {
            $image = $request->file('attachments');
            $imageName = 'profile_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $Land_form->attachments = $imageName;
        }


        if ($request->hasFile('cnic_front_attachments')) {

            $image = $request->file('cnic_front_attachments');
            $imageName = 'cnic_front_' . Str::uuid() . '.' . $image->getClientOriginalExtension();

            $image->move(public_path('assets/uploads'), $imageName);

            $Land_form->cnic_front_attachments = $imageName;
        }


        if ($request->hasFile('cnic_back_attachments')) {

            $image = $request->file('cnic_back_attachments');
            $imageName = 'cnic_back_' . Str::uuid() . '.' . $image->getClientOriginalExtension();

            $image->move(public_path('assets/uploads'), $imageName);

            $Land_form->cnic_back_attachments = $imageName;
        }

        $Land_form->save();

        // ---------- DELETE OLD LO RECORDS ----------
        Land_form_row::where('land_form_id', $id)->delete();

        // ---------- INSERT LO RECORDS ----------
        if ($request->filled('lo_lines')) {
            foreach ($request->lo_lines as $lo) {
                if (!empty($lo['lo_cod'])) {
                    Land_form_row::create([
                        'land_form_id' => $id,
                        'lo_cod' => $lo['lo_cod'],
                        'lo_name' => $lo['lo_name'] ?? null,
                        'relationship_revenue' => $lo['relationship_revenue'] ?? null,
                        'so' => $lo['so'] ?? null,
                        'lo_name_as_per_cnic' => $lo['lo_name_as_per_cnic'] ?? null,
                        'relationship_cnic' => $lo['relationship_cnic'] ?? null,
                        'father_name_cnic' => $lo['father_name_cnic'] ?? null,
                        'caste' => $lo['caste'] ?? null,
                        'lo_cnic' => $lo['lo_cnic'] ?? null,
                        'contact_no' => $lo['contact_no'] ?? null,
                        'address' => $lo['address'] ?? null,
                    ]);
                }
            }
        }

        // ---------- DELETE OLD LAND ROWS ----------
        Land_form_row_detail::where('land_form_id', $id)->delete();

        // ---------- INSERT LAND ROWS ----------
        if ($request->filled('item_lines')) {
            foreach ($request->item_lines as $row) {
                if (
                    !empty($row['lo_cod']) ||
                    !empty($row['khewat_no']) ||
                    !empty($row['khatooni_no']) ||
                    !empty($row['qatat'])
                ) {
                    Land_form_row_detail::create([
                        'land_form_id' => $id,
                        'lo_cod' => $row['lo_cod'] ?? null,
                        'khewat_no' => $row['khewat_no'] ?? null,
                        'khatooni_no' => $row['khatooni_no'] ?? null,
                        'qatat' => $row['qatat'] ?? null,
                        'measuring_k' => $row['measuring_k'] ?? null,
                        'measuring_m' => $row['measuring_m'] ?? null,
                        'measuring_sqft' => $row['measuring_sqft'] ?? null,
                        'transfer_share' => $row['transfer_share'] ?? null,
                        'land_measuring_k' => $row['land_measuring_k'] ?? null,
                        'land_measuring_m' => $row['land_measuring_m'] ?? null,
                        'land_measuring_sqft' => $row['land_measuring_sqft'] ?? null,
                        'land_category' => $row['land_category'] ?? null,
                    ]);
                }
            }
        }
        // echo '<pre>'; print_r($request->all());exit;

        return redirect()
            ->route('land_form.index')
            ->with('success', 'Land Form has been updated successfully.');
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

            $company = Land_form::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('land_form.index')
                ->with('success', 'Land Form Has Been Deleted successfully');
        } else {
            return redirect()->route('land_form.index')
                ->with('danger', 'Land Form Not Found');
        }
    }
}
