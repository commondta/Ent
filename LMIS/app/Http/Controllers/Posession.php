<?php

namespace App\Http\Controllers;

use App\Models\Exemption_rate;
use App\Models\Land_provider;
use App\Models\Possession_certificate;
use App\Models\Posession_of_land_rows;
use App\Models\Posession_of_land_lo_rows;
use App\Models\Purchase_of_land;
use App\Models\Land_form;
use App\Models\Purchase_of_land_rows;
use App\Models\Land_form_row;
use App\Models\Land_form_row_detail;
use App\Models\Seller_profile;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Http\Request;
use App\Models\Possession_attachments;

class Posession extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if (auth()->user()->possession_certificate_list == 1) {
            $data['record'] = Possession_certificate::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.purchasing_of_land.possession_certificate.show', $data);
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
        if (auth()->user()->possession_certificate_add == 1) {
            $data['doc_num']  = Possession_certificate::latest('id')->value('id') ?? 0;
            $data['land_provider'] = Land_provider::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            $data['land_owner'] = Seller_profile::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            $data['land_offer_form'] = Land_form::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.purchasing_of_land.possession_certificate.add', $data);
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
            'base_code_no' => 'required',
            'lp_name' => 'required',
            'lp_possession_jpo' => 'required',
            'picto_lo_name' => 'required',
            'picto_lp_name' => 'required',
            'picto_name_of_patwari' => 'required',
            // 'picto_area' => 'required',
            // 'picto_kanal' => 'required',
            // 'picto_marla' => 'required',
            'picto_possession_jco' => 'required',

        ]);
        $record = new Possession_certificate();
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->base_code_no = $request->base_code_no;
        $record->sq_feet = $request->sq_feet;
        $record->marla = $request->marla;
        $record->kanal = $request->kanal;
        $record->mouza = $request->mouza;
        $record->lp_name = $request->lp_name;
        $record->lp_contact_no = $request->lp_contact_no;
        $record->lp_rep_name = $request->lp_rep_name;
        $record->lp_possession_jpo = $request->lp_possession_jpo;
        $record->picto_lo_name = $request->picto_lo_name;
        $record->picto_lp_name = $request->picto_lp_name;

        $record->picto_name_of_patwari = $request->picto_name_of_patwari;
        $record->picto_possession_jco = $request->picto_possession_jco;


        // Add totals
        $record->total_land_kanal = $request->total_land_kanal ?? null;
        $record->total_land_marla = $request->total_land_marla ?? null;
        $record->total_land_sqft = $request->total_land_sqft ?? null;
        $record->total_land_acres = $request->total_land_acres ?? null;
        $record->total_poss_kanal = $request->total_poss_kanal ?? null;
        $record->total_poss_marla = $request->total_poss_marla ?? null;
        $record->total_poss_sqft = $request->total_poss_sqft ?? null;
        $record->total_poss_acres = $request->total_poss_acres ?? null;
        $record->total_unposs_kanal = $request->total_unposs_kanal ?? null;
        $record->total_unposs_marla = $request->total_unposs_marla ?? null;
        $record->total_unposs_sqft = $request->total_unposs_sqft ?? null;
        $record->total_unposs_acres = $request->total_unposs_acres ?? null;

        $record->createdBy = auth()->user()->id;
        //echo '<pre>';  print_r($record);exit;

        $attachment = $request->file('picto_picture');
        if ($attachment) {
            $imageName6 = time() . '_' . uniqid() . '.' .  $attachment->getClientOriginalExtension();
            if ($attachment->move(public_path('assets/uploads'), $imageName6)) {
                $record->picto_picture = $imageName6;
            }
        }



        $approval_check = Approval_setup_header::where('approval', 'Possession Certificate')->first();
        if ($approval_check) {
            $record->status = 1;
        } else {
            $record->status = 0;
        }
        $record->save();

        $lastid = $record->id;

        if ($lastid) {
            // Handle land_details (new table format)
            $land_details = $request->land_details;
            if ($land_details) {
                foreach ($land_details as $detail) {
                    if ($detail['khewat_no'] || $detail['khatooni_no']) {
                        $land_row = new Posession_of_land_rows();
                        $land_row->deed_id = $lastid;
                        $land_row->khewat_no = $detail['khewat_no'] ?? null;
                        $land_row->khatooni_no = $detail['khatooni_no'] ?? null;
                        // $land_row->block_no = $detail['block_no'] ?? null;
                        // $land_row->rectangle_no = $detail['rectangle_no'] ?? null;
                        $land_row->qatat = $detail['qatat'] ?? null;
                        $land_row->sector = $detail['sector'] ?? null;
                        //$land_row->khasra_no = $detail['khasra_no'] ?? null;
                        // $land_row->measuring_k = $detail['measuring_k'] ?? null;
                        // $land_row->measuring_m = $detail['measuring_m'] ?? null;
                        // $land_row->measuring_sqft = $detail['measuring_sqft'] ?? null;
                        // $land_row->transfer_share = $detail['transfer_share'] ?? null;
                        $land_row->land_measuring_k = $detail['land_measuring_k'] ?? null;
                        $land_row->land_measuring_m = $detail['land_measuring_m'] ?? null;
                        $land_row->land_measuring_sqft = $detail['land_measuring_sqft'] ?? null;
                        $land_row->land_category = $detail['land_category'] ?? null;
                        $land_row->possessed_k = $detail['possessed_k'] ?? null;
                        $land_row->possessed_m = $detail['possessed_m'] ?? null;
                        $land_row->possessed_sqft = $detail['possessed_sqft'] ?? null;
                        $land_row->unpossessed_k = $detail['unpossessed_k'] ?? null;
                        $land_row->unpossessed_m = $detail['unpossessed_m'] ?? null;
                        $land_row->unpossessed_sqft = $detail['unpossessed_sqft'] ?? null;
                        // echo '<pre>'; print_r($land_row);exit;
                        $land_row->save();
                    }
                }
            }

            // Handle LO (Land Owner) details from the fetched data
            $lo_names = $request->lo_name;
            $so_values = $request->so;
            $lo_cnics = $request->lo_cnic;
            $contact_nos = $request->contact_no;

            if ($lo_names) {
                // Convert to array if not already
                $lo_names = is_array($lo_names) ? $lo_names : [$lo_names];
                $so_values = is_array($so_values) ? $so_values : [$so_values];
                $lo_cnics = is_array($lo_cnics) ? $lo_cnics : [$lo_cnics];
                $contact_nos = is_array($contact_nos) ? $contact_nos : [$contact_nos];

                foreach ($lo_names as $index => $lo_name) {
                    if ($lo_name) { // Only save if lo_name exists
                        $lo_row = new Posession_of_land_lo_rows();
                        $lo_row->deed_id = $lastid;
                        $lo_row->lo_name = $lo_name;
                        $lo_row->so = $so_values[$index] ?? null;
                        $lo_row->lo_cnic = $lo_cnics[$index] ?? null;
                        $lo_row->contact_no = $contact_nos[$index] ?? null;
                        $lo_row->save();
                    }
                }
            }
        }
        $attachment_possession_certificate = $request->file('attachment_possession_certificate');
        if ($attachment_possession_certificate) {
            foreach ($attachment_possession_certificate as $attachment_possession_cert) {

                if ($attachment_possession_cert) {
                    $imageName4 = time() . '_' . uniqid() . '.' .  $attachment_possession_cert->getClientOriginalExtension();
                    if ($attachment_possession_cert->move(public_path('assets/uploads'), $imageName4)) {
                        $attachment = new possession_attachments();

                        $attachment->document = 'attachment_possession_certificate';
                        $attachment->attachment = $imageName4;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }
        $attachments = $request->file('attachment');
        if ($attachments) {
            foreach ($attachments as $attachment) {

                if ($attachment) {
                    $imageName5 = time() . '_' . uniqid() . '.' .  $attachment->getClientOriginalExtension();
                    if ($attachment->move(public_path('assets/uploads'), $imageName5)) {
                        $attachment = new possession_attachments();

                        $attachment->document = 'Other';
                        $attachment->attachment = $imageName5;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }

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

            return redirect()->route('possession_certificate.index')
                ->with('success', 'The Posession Certificate record sent for approval.');
        } else {
            return redirect()->route('possession_certificate.index')
                ->with('success', 'Posession Certificate has been created successfully.');
        }








        //        return redirect()->route('possession_certificate.index')
        //            ->with('success', 'Posession Certificate has been added successfully.');
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        if (auth()->user()->possession_certificate_print == 1) {
            $data['record'] = Possession_certificate::where('isDeleted', 0)->where('id', $id)->orderBy('id', 'desc')->first();
            // Check if record exists before proceeding
            if (!$data['record']) {
                return redirect()->route('possession_certificate.index')
                    ->with('error', 'Possession Certificate not found');
            }

            $data['land_p'] = Land_provider::where('isDeleted', 0)->where('lp_cod', $data['record']->lp_name)->first();
            // echo '<pre>';  print_r($data['land_p']);exit;
            if ($data['record']) {
                $base_doc_no = $data['record']->base_code_no;

                $id = $data['record']->id;
               
                $data['land_form'] = Land_form::where('isDeleted', 0)->where('doc_no',  $base_doc_no)->orderBy('id', 'desc')->first();

               // echo '<pre>';  print_r($data['land_form']);exit;

                if ($data['land_form']) {
                    $data['land_owners'] = Land_form_row::where('land_form_id', $data['land_form']->id)->orderBy('id', 'asc')->get();
                    //echo '<pre>';  print_r($data['land_owners']);exit;
                    $data['land_form_details'] = Land_form_row_detail::where('land_form_id', $data['land_form']->id)->orderBy('id', 'asc')->get();
                } else {
                    $data['land_owners'] = collect();
                    $data['land_form_details'] = collect();
                }
                //  echo '<pre>';  print_r($data['land_form_details']);exit;

                // Fetch the Possession Certificate's own land detail rows
                $data['land_details'] = Posession_of_land_rows::where('deed_id', $id)->get();

                // Fetch the Land Owner rows for this Possession Certificate
                //    $data['land_owners'] = Posession_of_land_lo_rows::where('deed_id', $id)->get();

                // Fetch attachments related to this Possession Certificate
                $data['attachments'] = Possession_attachments::where('parentId', $id)->get();
            }




            return view('pages.purchasing_of_land.possession_certificate.layout', $data);
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
    public function edit(Possession_certificate $possession_certificate)
    {
        if (auth()->user()->possession_certificate_edit == 1) {
            $id = $possession_certificate->id;
            $rows = Posession_of_land_rows::where('deed_id', $id)->get();
            $possession_certificate['rows'] =  $rows->toArray();


            // Fetch LO rows
            $loRows = Posession_of_land_lo_rows::where('deed_id', $id)->get();
            $possession_certificate['lo_rows'] = $loRows->toArray();
            $data['landDetails'] = $rows;
            // echo '<pre>'; print_r($rows);exit;
            $data['loDetails'] = $loRows;
            // echo '<pre>'; print_r($loRows);exit;


            $data['possession_certificate'] = Possession_certificate::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            $data['land_provider'] = Land_provider::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            $data['land_owner'] = Seller_profile::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            $data['purchase_of_land'] = Purchase_of_land::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            $data['land_offer_form'] = Land_form::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();

            $data['attachments'] = Possession_attachments::where('parentId', $possession_certificate->id)->get();
            return view('pages.purchasing_of_land.possession_certificate.edit', compact('possession_certificate'), $data);
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
            'lo_name' => 'required',
            'base_code_no' => 'required',

            'lp_name' => 'required',
            'lp_possession_jpo' => 'required',
            'picto_lo_name' => 'required',
            'picto_lp_name' => 'required',
            'picto_name_of_patwari' => 'required',
            // 'picto_area' => 'required',
            // 'picto_kanal' => 'required',
            // 'picto_marla' => 'required',
            'picto_possession_jco' => 'required',

        ]);
        $record = Possession_certificate::find($id);
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->base_code_no = $request->base_code_no;
        $record->sq_feet = $request->sq_feet;
        $record->marla = $request->marla;
        $record->kanal = $request->kanal;
        $record->mouza = $request->mouza;
        $record->lp_name = $request->lp_name;
        $record->lp_contact_no = $request->lp_contact_no;
        $record->lp_rep_name = $request->lp_rep_name;
        $record->lp_possession_jpo = $request->lp_possession_jpo;
        $record->picto_lo_name = $request->picto_lo_name;
        $record->picto_lp_name = $request->picto_lp_name;
        $record->picto_name_of_patwari = $request->picto_name_of_patwari;
        // $record->picto_area = $request->picto_area;
        // $record->picto_kanal = $request->picto_kanal;
        // $record->picto_marla = $request->picto_marla;
        $record->picto_possession_jco = $request->picto_possession_jco;

        // Add totals - only update if provided (prevent overwriting with NULL)
        if (!empty($request->total_land_kanal)) $record->total_land_kanal = $request->total_land_kanal;
        if (!empty($request->total_land_marla)) $record->total_land_marla = $request->total_land_marla;
        if (!empty($request->total_land_sqft)) $record->total_land_sqft = $request->total_land_sqft;
        if (!empty($request->total_land_acres)) $record->total_land_acres = $request->total_land_acres;
        if (!empty($request->total_poss_kanal)) $record->total_poss_kanal = $request->total_poss_kanal;
        if (!empty($request->total_poss_marla)) $record->total_poss_marla = $request->total_poss_marla;
        if (!empty($request->total_poss_sqft)) $record->total_poss_sqft = $request->total_poss_sqft;
        if (!empty($request->total_poss_acres)) $record->total_poss_acres = $request->total_poss_acres;
        if (!empty($request->total_unposs_kanal)) $record->total_unposs_kanal = $request->total_unposs_kanal;
        if (!empty($request->total_unposs_marla)) $record->total_unposs_marla = $request->total_unposs_marla;
        if (!empty($request->total_unposs_sqft)) $record->total_unposs_sqft = $request->total_unposs_sqft;
        if (!empty($request->total_unposs_acres)) $record->total_unposs_acres = $request->total_unposs_acres;

        // echo '<pre>';  print_r($record);exit;
        $attachment = $request->file('picto_picture');
        if ($attachment) {
            $imageName6 = time() . '_' . uniqid() . '.' .  $attachment->getClientOriginalExtension();
            if ($attachment->move(public_path('assets/uploads'), $imageName6)) {
                $record->picto_picture = $imageName6;
            }
        }

        $record->save();

        $lastid = $record->id;

        if ($lastid) {
            // Handle land_details (new table format)
            Posession_of_land_rows::where('deed_id', $lastid)->delete();
            $land_details = $request->land_details;
            if ($land_details) {
                foreach ($land_details as $detail) {
                    if ($detail['khewat_no'] || $detail['khatooni_no']) {
                        $land_row = new Posession_of_land_rows();
                        $land_row->deed_id = $lastid;
                        $land_row->khewat_no = $detail['khewat_no'] ?? null;
                        $land_row->khatooni_no = $detail['khatooni_no'] ?? null;
                        // $land_row->block_no = $detail['block_no'] ?? null;
                        // $land_row->rectangle_no = $detail['rectangle_no'] ?? null;
                        $land_row->qatat = $detail['qatat'] ?? null;
                        $land_row->sector = $detail['sector'] ?? null;
                        // $land_row->khasra_no = $detail['khasra_no'] ?? null;
                        // $land_row->measuring_k = $detail['measuring_k'] ?? null;
                        // $land_row->measuring_m = $detail['measuring_m'] ?? null;
                        // $land_row->measuring_sqft = $detail['measuring_sqft'] ?? null;
                        //$land_row->transfer_share = $detail['transfer_share'] ?? null;
                        $land_row->land_measuring_k = $detail['land_measuring_k'] ?? null;
                        $land_row->land_measuring_m = $detail['land_measuring_m'] ?? null;
                        $land_row->land_measuring_sqft = $detail['land_measuring_sqft'] ?? null;
                        $land_row->land_category = $detail['land_category'] ?? null;
                        $land_row->possessed_k = $detail['possessed_k'] ?? null;
                        $land_row->possessed_m = $detail['possessed_m'] ?? null;
                        $land_row->possessed_sqft = $detail['possessed_sqft'] ?? null;
                        $land_row->unpossessed_k = $detail['unpossessed_k'] ?? null;
                        $land_row->unpossessed_m = $detail['unpossessed_m'] ?? null;
                        $land_row->unpossessed_sqft = $detail['unpossessed_sqft'] ?? null;
                        // echo '<pre>'; print_r($land_row);exit;
                        $land_row->save();
                    }
                }
            }

            // Handle LO (Land Owner) details from the fetched data
            Posession_of_land_lo_rows::where('deed_id', $lastid)->delete();
            $lo_names = $request->lo_name;
            $so_values = $request->so;
            $lo_cnics = $request->lo_cnic;
            $contact_nos = $request->contact_no;

            if ($lo_names) {
                // Convert to array if not already
                $lo_names = is_array($lo_names) ? $lo_names : [$lo_names];
                $so_values = is_array($so_values) ? $so_values : [$so_values];
                $lo_cnics = is_array($lo_cnics) ? $lo_cnics : [$lo_cnics];
                $contact_nos = is_array($contact_nos) ? $contact_nos : [$contact_nos];

                foreach ($lo_names as $index => $lo_name) {
                    if ($lo_name) { // Only save if lo_name exists
                        $lo_row = new Posession_of_land_lo_rows();
                        $lo_row->deed_id = $lastid;
                        $lo_row->lo_name = $lo_name;
                        $lo_row->so = $so_values[$index] ?? null;
                        $lo_row->lo_cnic = $lo_cnics[$index] ?? null;
                        $lo_row->contact_no = $contact_nos[$index] ?? null;
                        $lo_row->save();
                    }
                }
            }
        }
        // echo '<pre>'; print_r($request->all());exit;


        $attachment_possession_certificate = $request->file('attachment_possession_certificate');
        if ($attachment_possession_certificate) {
            foreach ($attachment_possession_certificate as $attachment_possession_cert) {

                if ($attachment_possession_cert) {
                    $imageName4 = time() . '_' . uniqid() . '.' .  $attachment_possession_cert->getClientOriginalExtension();
                    if ($attachment_possession_cert->move(public_path('assets/uploads'), $imageName4)) {
                        $attachment = new possession_attachments();

                        $attachment->document = 'attachment_possession_certificate';
                        $attachment->attachment = $imageName4;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }
        $attachments = $request->file('attachment');
        if ($attachments) {
            foreach ($attachments as $attachment) {

                if ($attachment) {
                    $imageName5 = time() . '_' . uniqid() . '.' .  $attachment->getClientOriginalExtension();
                    if ($attachment->move(public_path('assets/uploads'), $imageName5)) {
                        $attachment = new possession_attachments();

                        $attachment->document = 'Other';
                        $attachment->attachment = $imageName5;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }
        return redirect()->route('possession_certificate.index')
            ->with('success', 'Possession Certificate has been Updated successfully.');
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

            $company = Possession_certificate::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('possession_certificate.index')
                ->with('success', 'Possession Certificate Has Been Deleted successfully');
        } else {
            return redirect()->route('possession_certificate.index')
                ->with('danger', 'Possession CertificateNot Found');
        }
    }
}
