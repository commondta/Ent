<?php

namespace App\Http\Controllers;

use App\Models\Land_provider;
use App\Models\Purchase_of_land;
use App\Models\Purchase_of_land_rows;
use App\Models\Purchase_of_land_lo_rows;
use App\Models\Seller_profile;
use App\Models\Land_form;
use App\Models\Land_form_row;
use App\Models\Land_form_row_detail;
use App\Models\Exemption_rate;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use App\Models\Purchase_of_land_attachment;
use Illuminate\Http\Request;

class Purchs_L extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {

        if (auth()->user()->purchase_of_land_list == 1) {
            $data['record'] = Purchase_of_land::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            //echo '<pre>'; print_r($data['record']);exit;    
            // foreach ($data['record'] as $row) {
            //     $row->lp_name = is_string($row->lp_name) ? json_decode($row->lp_name, true) : $row->lp_name;
            // }

            return view('pages.purchasing_of_land.purchase_of_land.show', $data);
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
        if (auth()->user()->purchase_of_land_add == 1) {
            $data['record'] = Seller_profile::where('isDeleted', 0)->where('status', 0)->distinct()->orderBy('id', 'desc')->get();

            $data['file_num']  = Purchase_of_land::latest('File_No')->value('File_No') ?? 0;
            $data['land_provider'] = Land_provider::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            $data['land_owner'] = Land_form::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            //echo '<pre>'; print_r($data['land_owner']);exit;
            $data['exemption_rate'] = Exemption_rate::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();

            return view('pages.purchasing_of_land.purchase_of_land.add', $data);
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
            'File_No' => 'required|unique:purchase_of_lands,File_No',
            'doc_date' => 'required',
            'land_form_no' => 'required',
            'posting_date' => 'required',
            'acre' => 'required',
            'district_rate' => 'required',
            'district_amount' => 'required',
            'society_rate' => 'required',
            'society_amount' => 'required',
            'exemption_rate' => 'required',
            'mode_of_payment' => 'required',
            'fard_id' => 'required',
            'fard_date' => 'required',
        ]);
        $record = new Purchase_of_land();
        $record->File_No = $request->File_No;
        $record->doc_date = $request->doc_date;
        $record->land_form_no = $request->land_form_no;
        $record->posting_date = $request->posting_date;
       // $record->lp_name = json_encode($request->lp_name); // Convert array to JSON
       $record->lp_name = $request->lp_name;
        $record->mouza = $request->mouza;
        $record->acre = $request->acre;
        $record->district_rate = $request->district_rate;
        $record->district_amount = $request->district_amount;
        $record->society_rate = $request->society_rate;
        $record->society_amount = $request->society_amount;
        $record->exemption_rate = $request->exemption_rate;
        $record->mode_of_payment = $request->mode_of_payment;
        $record->fard_id = $request->fard_id;
        $record->fard_id2 = $request->fard_id2;
        $record->fard_date = $request->fard_date;
        $record->fard_date2 = $request->fard_date2;
        $record->total_kanal = $request->total_kanal;
        $record->total_marla = $request->total_marla;
        $record->total_sqft = $request->total_sqft;
        $record->total_acre = $request->total_acre;
        $record->createdBy = auth()->user()->id;
        $approval_check = Approval_setup_header::where('approval', 'Purchase of Land')->first();

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

                    $land_row = new Purchase_of_land_rows();
                    $land_row->deed_id = $lastid;
                    $land_row->khewat_no = $detail['khewat_no'] ?? null;
                    $land_row->khatooni_no = $detail['khatooni_no'] ?? null;
                    // $land_row->block_no = $detail['block_no'] ?? null;
                    // $land_row->rectangle_no = $detail['rectangle_no'] ?? null;
                    $land_row->qatat = $detail['qatat'] ?? null;
                    // $land_row->khasra_no = $detail['khasra_no'] ?? null;
                    $land_row->measuring_k = $detail['measuring_k'] ?? null;
                    $land_row->measuring_m = $detail['measuring_m'] ?? null;
                    $land_row->measuring_sqft = $detail['measuring_sqft'] ?? null;
                    $land_row->transfer_share = $detail['transfer_share'] ?? null;
                    $land_row->land_measuring_k = $detail['land_measuring_k'] ?? null;
                    $land_row->land_measuring_m = $detail['land_measuring_m'] ?? null;
                    $land_row->land_measuring_sqft = $detail['land_measuring_sqft'] ?? null;
                    $land_row->land_category = $detail['land_category'] ?? null;
                    $land_row->save();
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
                        $lo_row = new Purchase_of_land_lo_rows();
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

        $attachmentFiles = $request->file('attachment_nfc_sub_registrar');
        if ($attachmentFiles) {
            foreach ($attachmentFiles as $file) {
                $imageName = time() . '_' . uniqid() . '.' . $file->getClientOriginalExtension();
                if ($file->move(public_path('assets/uploads'), $imageName)) {
                    $attachment = new Purchase_of_land_attachment(); // Create a new instance for each file
                    $attachment->document = 'attachment_nfc_sub_registrar';
                    $attachment->attachment = $imageName;
                    $attachment->parentId = $lastid;
                    $attachment->save();
                }
            }
        }

        $attachment_massavi = $request->file('attachment_massavi');
        if ($attachment_massavi) {
            foreach ($attachment_massavi as $file) {
                $imageName1 = time() . '_' . uniqid() . '.' . $file->getClientOriginalExtension();
                if ($file->move(public_path('assets/uploads'), $imageName1)) {
                    $attachment = new Purchase_of_land_attachment(); // Create a new instance for each file
                    $attachment->document = 'attachment_massavi';
                    $attachment->attachment = $imageName1;
                    $attachment->parentId = $lastid;
                    $attachment->save();
                }
            }
        }

        $attachment_girdwaris = $request->file('attachment_girdwari');
        if ($attachment_girdwaris) {
            foreach ($attachment_girdwaris as $attachment_girdwari) {

                if ($attachment_girdwari) {
                    $imageName2 = time() . '_' . uniqid() . '.' .  $attachment_girdwari->getClientOriginalExtension();
                    if ($attachment_girdwari->move(public_path('assets/uploads'), $imageName2)) {
                        $attachment = new Purchase_of_land_attachment();

                        $attachment->document = 'attachment_girdwari';
                        $attachment->attachment = $imageName2;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }
        $attachment_fard_milkiyats = $request->file('attachment_fard_milkiyat');
        if ($attachment_fard_milkiyats) {
            foreach ($attachment_fard_milkiyats as $attachment_fard_milkiyat) {

                if ($attachment_fard_milkiyat) {
                    $imageName3 = time() . '_' . uniqid() . '.' . $attachment_fard_milkiyat->getClientOriginalExtension();
                    if ($attachment_fard_milkiyat->move(public_path('assets/uploads'), $imageName3)) {
                        $attachment = new Purchase_of_land_attachment();

                        $attachment->document = 'attachment_fard_milkiyat';
                        $attachment->attachment = $imageName3;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }
        $attachment_khata_of_lands = $request->file('attachment_khata_of_land');
        if ($attachment_khata_of_lands) {
            foreach ($attachment_khata_of_lands as $attachment_khata_of_land) {

                if ($attachment_khata_of_land) {
                    $imageName4 = time() . '_' . uniqid() . '.' .  $attachment_khata_of_land->getClientOriginalExtension();
                    if ($attachment_khata_of_land->move(public_path('assets/uploads'), $imageName4)) {
                        $attachment = new Purchase_of_land_attachment();

                        $attachment->document = 'attachment_khata_of_land';
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
                        $attachment = new Purchase_of_land_attachment();

                        $attachment->document = 'Other';
                        $attachment->attachment = $imageName5;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }
        $attachment_aks_shajra = $request->file('attachment_aks_shajra');
        if ($attachment_aks_shajra) {
            foreach ($attachment_aks_shajra as $file) {
                $imageName6 = time() . '_' . uniqid() . '.' . $file->getClientOriginalExtension();
                if ($file->move(public_path('assets/uploads'), $imageName6)) {
                    $attachment = new Purchase_of_land_attachment(); // Create a new instance for each file
                    $attachment->document = 'attachment_aks_shajra';
                    $attachment->attachment = $imageName6;
                    $attachment->parentId = $lastid;
                    $attachment->save();
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

            return redirect()->route('purchase_of_land.index')
                ->with('success', 'The Purchase of Land record sent for approval.');
        } else {
            return redirect()->route('purchase_of_land.index')
                ->with('success', 'Purchase of Land has been created successfully.');
        }
        //        return redirect()->route('purchase_of_land.index')
        //            ->with('success', 'Purchase of Land Form has been added successfully.');
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        if (auth()->user()->purchase_of_land_print == 1) {
            $data['land_p'] = Land_provider::where('isDeleted', 0)->orderBy('id', 'desc')->first();
            $data['record'] = Purchase_of_land::where('isDeleted', 0)->where('id', $id)->orderBy('id', 'desc')->first();
            // Get Land Form and Land Owners
            $data['land_form'] = Land_form::where('isDeleted', 0)->where('doc_no', $data['record']->land_form_no)->orderBy('id', 'desc')->first();
            //echo '<pre>';  print_r($data['land_form']);exit;

            if ($data['land_form']) {
                $data['land_owners'] = Land_form_row::where('land_form_id', $data['land_form']->id)->orderBy('lo_cod', 'asc')->get();
                $data['land_form_details'] = Land_form_row_detail::where('land_form_id', $data['land_form']->id)->orderBy('id', 'asc')->get();
            } else {
                $data['land_owners'] = collect();
                $data['land_form_details'] = collect();
            }
            //echo '<pre>';  print_r($data['land_form_details']);exit;
            $data['landRows'] = Purchase_of_land_rows::where('deed_id', $id)->get();
            $data['loRows'] = Purchase_of_land_lo_rows::where('deed_id', $id)->get();
            //echo '<pre>'; print_r($data['loRows']);exit;
            $data['exemption_rate'] = Exemption_rate::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            return view('pages.purchasing_of_land.purchase_of_land.layout', $data);
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
    public function edit(Purchase_of_land $purchase_of_land)
    {
        if (auth()->user()->purchase_of_land_edit == 1) {
            $data['record'] = Seller_profile::where('isDeleted', 0)->where('status', 0)->distinct()->orderBy('id', 'desc')->get();
            $id = $purchase_of_land->id;
            $rows = Purchase_of_land_rows::where('deed_id', $id)->get();
            $purchase_of_land['rows'] =  $rows->toArray();
            // Fetch LO rows
            $loRows = Purchase_of_land_lo_rows::where('deed_id', $id)->get();
            $purchase_of_land['lo_rows'] = $loRows->toArray();

            $data['land_provider'] = Land_provider::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            $data['land_owner'] = Land_form::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            $data['attachments'] = Purchase_of_land_attachment::where('parentId', $id)->get();
            $data['landDetails'] = $rows;
            $data['loDetails'] = $loRows;

            $data['exemption_rate'] = Exemption_rate::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            $data['mode_of_payment'] = collect([
                (object)['mode_of_payment' => 'Cash'],
                (object)['mode_of_payment' => 'Exemption File'],
                (object)['mode_of_payment' => 'Hybrid']
            ]);
            return view('pages.purchasing_of_land.purchase_of_land.edit', compact('purchase_of_land'), $data);
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
            'File_No' => 'required',
            'doc_date' => 'required',
            'land_form_no' => 'required',
            'posting_date' => 'required',
            'mouza' => 'required',
            'acre' => 'required',
            'district_rate' => 'required',
            'district_amount' => 'required',
            'society_rate' => 'required',
            'society_amount' => 'required',
            'exemption_rate' => 'required',
            'mode_of_payment' => 'required',
            'fard_id' => 'required',
            'fard_date' => 'required',
        ]);
        $record = Purchase_of_land::find($id);
        $record->File_No = $request->File_No;
        $record->doc_date = $request->doc_date;
        $record->land_form_no = $request->land_form_no;
        $record->posting_date = $request->posting_date;
       // $record->lp_name = json_encode($request->lp_name); // Convert array to JSON
       $record->lp_name = $request->lp_name;
        $record->mouza = $request->mouza;
        $record->acre = $request->acre;
        $record->district_rate = $request->district_rate;
        $record->district_amount = $request->district_amount;
        $record->society_rate = $request->society_rate;
        $record->society_amount = $request->society_amount;
        $record->exemption_rate = $request->exemption_rate;
        $record->mode_of_payment = $request->mode_of_payment;
        $record->fard_id = $request->fard_id;
        $record->fard_id2 = $request->fard_id2;
        $record->fard_date = $request->fard_date;
        $record->fard_date2 = $request->fard_date2;
        $record->total_kanal = $request->total_kanal;
        $record->total_marla = $request->total_marla;
        $record->total_sqft = $request->total_sqft;
        $record->total_acre = $request->total_acre;
        // echo '<pre>'; print_r($record);exit;


        $record->save();

        // Delete existing land detail rows before adding new ones
        Purchase_of_land_rows::where('deed_id', $id)->delete();

        // Handle land_details (new table format)
        $land_details = $request->land_details;
        if ($land_details) {
            foreach ($land_details as $detail) {

                $land_row = new Purchase_of_land_rows();
                $land_row->deed_id = $id;
                $land_row->khewat_no = $detail['khewat_no'] ?? null;
                $land_row->khatooni_no = $detail['khatooni_no'] ?? null;
                // $land_row->block_no = $detail['block_no'] ?? null;
                // $land_row->rectangle_no = $detail['rectangle_no'] ?? null;
                $land_row->qatat = $detail['qatat'] ?? null;
                // $land_row->khasra_no = $detail['khasra_no'] ?? null;
                $land_row->measuring_k = $detail['measuring_k'] ?? null;
                $land_row->measuring_m = $detail['measuring_m'] ?? null;
                $land_row->measuring_sqft = $detail['measuring_sqft'] ?? null;
                $land_row->transfer_share = $detail['transfer_share'] ?? null;
                $land_row->land_measuring_k = $detail['land_measuring_k'] ?? null;
                $land_row->land_measuring_m = $detail['land_measuring_m'] ?? null;
                $land_row->land_measuring_sqft = $detail['land_measuring_sqft'] ?? null;
                $land_row->land_category = $detail['land_category'] ?? null;
                $land_row->save();
            }
        }

        // Delete existing LO rows before adding new ones
        Purchase_of_land_lo_rows::where('deed_id', $id)->delete();

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
                    $lo_row = new Purchase_of_land_lo_rows();
                    $lo_row->deed_id = $id;
                    $lo_row->lo_name = $lo_name;
                    $lo_row->so = $so_values[$index] ?? null;
                    $lo_row->lo_cnic = $lo_cnics[$index] ?? null;
                    $lo_row->contact_no = $contact_nos[$index] ?? null;
                    $lo_row->save();
                }
            }
        }

        $lastid = $id;
        $attachmentFiles = $request->file('attachment_nfc_sub_registrar');
        if ($attachmentFiles) {
            foreach ($attachmentFiles as $file) {
                $imageName = time() . '_' . uniqid() . '.' . $file->getClientOriginalExtension();
                if ($file->move(public_path('assets/uploads'), $imageName)) {

                    $attachment = new Purchase_of_land_attachment(); // Create a new instance for each file
                    $attachment->document = 'attachment_nfc_sub_registrar';
                    $attachment->attachment = $imageName;
                    $attachment->parentId = $lastid;
                    $attachment->save();
                }
            }
        }

        $attachment_massavis = $request->file('attachment_massavi');
        if ($attachment_massavis) {
            foreach ($attachment_massavis as $attachment_massavi) {

                if ($attachment_massavi) {
                    $imageName1 = time() . '_' . uniqid() . '.' .  $attachment_massavi->getClientOriginalExtension();
                    if ($attachment_massavi->move(public_path('assets/uploads'), $imageName1)) {
                        $attachment = new Purchase_of_land_attachment();

                        $attachment->document = 'attachment_massavi';
                        $attachment->attachment = $imageName1;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }
        $attachment_girdwaris = $request->file('attachment_girdwari');
        if ($attachment_girdwaris) {
            foreach ($attachment_girdwaris as $attachment_girdwari) {

                if ($attachment_girdwari) {
                    $imageName2 = time() . '_' . uniqid() . '.' .  $attachment_girdwari->getClientOriginalExtension();
                    if ($attachment_girdwari->move(public_path('assets/uploads'), $imageName2)) {
                        $attachment = new Purchase_of_land_attachment();

                        $attachment->document = 'attachment_girdwari';
                        $attachment->attachment = $imageName2;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }

        $attachment_fard_milkiyats = $request->file('attachment_fard_milkiyat');
        if ($attachment_fard_milkiyats) {
            foreach ($attachment_fard_milkiyats as $attachment_fard_milkiyat) {

                if ($attachment_fard_milkiyat) {
                    $imageName3 = time() . '_' . uniqid() . '.' . $attachment_fard_milkiyat->getClientOriginalExtension();
                    if ($attachment_fard_milkiyat->move(public_path('assets/uploads'), $imageName3)) {
                        $attachment = new Purchase_of_land_attachment();

                        $attachment->document = 'attachment_fard_milkiyat';
                        $attachment->attachment = $imageName3;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }

        $attachment_khata_of_lands = $request->file('attachment_khata_of_land');
        if ($attachment_khata_of_lands) {
            foreach ($attachment_khata_of_lands as $attachment_khata_of_land) {

                if ($attachment_khata_of_land) {
                    $imageName4 = time() . '_' . uniqid() . '.' .  $attachment_khata_of_land->getClientOriginalExtension();
                    if ($attachment_khata_of_land->move(public_path('assets/uploads'), $imageName4)) {
                        $attachment = new Purchase_of_land_attachment();

                        $attachment->document = 'attachment_khata_of_land';
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
                        $attachment = new Purchase_of_land_attachment();

                        $attachment->document = 'Other';
                        $attachment->attachment = $imageName5;
                        $attachment->parentId = $lastid;
                        $attachment->save();
                    }
                }
            }
        }
        $attachment_aks_shajra = $request->file('attachment_aks_shajra');
        if ($attachment_aks_shajra) {
            foreach ($attachment_aks_shajra as $file) {
                $imageName6 = time() . '_' . uniqid() . '.' . $file->getClientOriginalExtension();
                if ($file->move(public_path('assets/uploads'), $imageName6)) {
                    $attachment = new Purchase_of_land_attachment(); // Create a new instance for each file
                    $attachment->document = 'attachment_aks_shajra';
                    $attachment->attachment = $imageName6;
                    $attachment->parentId = $lastid;
                    $attachment->save();
                }
            }
        }


        return redirect()->route('purchase_of_land.index')
            ->with('success', 'Purchase of Land Form has been Updated successfully.');
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

            $company = Purchase_of_land::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('purchase_of_land.index')
                ->with('success', 'Purchase of Land Has Been Deleted successfully');
        } else {
            return redirect()->route('purchase_of_land.index')
                ->with('danger', 'Purchase of Land Not Found');
        }
    }
}
