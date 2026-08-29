<?php

namespace App\Http\Controllers;

use App\Models\Exemption_inventory_approval;
use App\Models\Land_form;
use App\Models\Exemption_inventory_row;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Http\Request;

class Exemption_inventory_c extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if (auth()->user()->exemption_inventory_list == 1 || auth()->user()->is_admin == 1) {
            $data['record'] = Exemption_inventory_approval::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->paginate(35);

            foreach ($data['record'] as $row) {
                $row->inventory_rows = Exemption_inventory_row::where('exemption_inventory_id', $row->id)->get();
            }

            return view('pages.exemptions.exemption_inventory.show', $data);
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
        if (auth()->user()->exemption_inventory_add == 1 || auth()->user()->is_admin == 1) {
            $data['doc_no'] = Exemption_inventory_approval::latest('doc_no')->value('doc_no');
            $data['doc_num']  = Exemption_inventory_approval::latest('id')->value('id');
            $data['land_owner'] = Land_form::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            //echo '<pre>'; print_r($data['land_owner']);exit;
            return view('pages.exemptions.exemption_inventory.add', $data);
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
            'doc_no' => 'required|unique:exemption_inventory_approvals,doc_no',
            'date' => 'required',
            'land_offer_form_no' => 'required',
            'attachment' => 'nullable|file|mimes:pdf,doc,docx,xls,xlsx,jpg,jpeg,png|max:5120',
        ]);

        $record = new Exemption_inventory_approval();
        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->land_offer_form_no = $request->land_offer_form_no;
        $record->total_registered_land = $request->total_registered_land;
        $record->total_possessed_land = $request->total_possessed_land;
        $record->rate_per_acre = $request->rate_per_acre;
        $record->total_cost_registered = $request->total_cost_registered;
        $record->total_cost_possessed = $request->total_cost_possessed;
        $record->total_residential_files = $request->total_residential_files;
        $record->total_commercial_files = $request->total_commercial_files;
        $record->total_marlas = $request->total_marlas;
        $record->exemption_percent = $request->exemption_percent;
        $record->total_cost = $request->total_cost;
        $record->residential_percent = $request->residential_percent;
        $record->commercial_percent = $request->commercial_percent;
        $record->cash = $request->cash;
        $record->inv_decimal = $request->inv_decimal;
        $record->remarks = $request->remarks;
        $record->createdBy = auth()->user()->id;

         $attachment = $request->file('attachment');
        if ($attachment) {
            $imageName6 = time() . '_' . uniqid() . '.' .  $attachment->getClientOriginalExtension();
            if ($attachment->move(public_path('assets/uploads'), $imageName6)) {
                $record->attachment = $imageName6;
            }
        }

        $approval_check = Approval_setup_header::where('approval', 'Exemption Inventory')->first();

        if ($approval_check) {
            $record->status = 1;
        } else {
            $record->status = 0;
        }
        $record->save();

        $lastId = $record->id;

        if ($lastId) {
            $inventory_lines = $request->inventory_lines;
            if ($inventory_lines && is_array($inventory_lines)) {
                foreach ($inventory_lines as $line) {
                    if (!empty($line['category'])) {
                        $inventory_row = new Exemption_inventory_row();
                        $inventory_row->exemption_inventory_id = $lastId;
                        $inventory_row->category = $line['category'] ?? null;
                        $inventory_row->inventory_type = $line['inventory_type'] ?? null;
                        $inventory_row->size_of_file = $line['size_of_file'] ?? null;
                        $inventory_row->no_of_files = $line['no_of_files'] ?? null;
                        $inventory_row->rate_file_plot = $line['rate_file_plot'] ?? null;
                        $inventory_row->total_cost = $line['total_cost'] ?? null;
                        $inventory_row->eighty_percent = $line['eighty_percent'] ?? null;
                        $inventory_row->twenty_percent = $line['twenty_percent'] ?? null;
                        $inventory_row->remark = $line['remark'] ?? null;
                        $inventory_row->save();
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
                $document_approval->document_id = $lastId;
                $document_approval->priority = $count;
                $document_approval->approval_user_id = $Approval_setup_line->user;
                $document_approval->status = $Approval_setup_line->status;
                $document_approval->remarks = '';
                $document_approval->save();
                $count++;
            }

            return redirect()->route('exemption_inventory.index')
                ->with('success', 'The Exemption Inventory record sent for approval.');
        } else {
            return redirect()->route('exemption_inventory.index')
                ->with('success', 'Exemption Inventory has been created successfully.');
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
    public function edit(Exemption_inventory_approval $exemption_inventory)
    {
        if (auth()->user()->exemption_inventory_edit == 1 || auth()->user()->is_admin == 1) {
            $record = $exemption_inventory;
            $record->rows = Exemption_inventory_row::where('exemption_inventory_id', $record->id)->get();
            $record->approvals = Document_approval::where('document_name', 'Exemption Inventory')
                ->where('document_id', $record->id)->get();
            $land_owner = Land_form::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();

            return view('pages.exemptions.exemption_inventory.edit', compact('record', 'land_owner'));
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
            'land_offer_form_no' => 'required',
            'attachment' => 'nullable|file|mimes:pdf,doc,docx,xls,xlsx,jpg,jpeg,png|max:5120',
        ]);

        $record = Exemption_inventory_approval::find($id);

        if (!$record) {
            return redirect()->back()->with('error', 'Record not found.');
        }

        $record->doc_no = $request->doc_no;
        $record->date = $request->date;
        $record->land_offer_form_no = $request->land_offer_form_no;
        $record->total_registered_land = $request->total_registered_land;
        $record->total_possessed_land = $request->total_possessed_land;
        $record->rate_per_acre = $request->rate_per_acre;
        $record->total_cost_registered = $request->total_cost_registered;
        $record->total_cost_possessed = $request->total_cost_possessed;
        $record->total_residential_files = $request->total_residential_files;
        $record->total_commercial_files = $request->total_commercial_files;
        $record->total_marlas = $request->total_commercial_files;
        $record->exemption_percent = $request->exemption_percent;
        $record->total_cost = $request->total_cost;
        $record->residential_percent = $request->residential_percent;
        $record->commercial_percent = $request->commercial_percent;
        $record->cash = $request->cash;
        $record->inv_decimal = $request->inv_decimal;
        $record->remarks = $request->remarks;

       
         $attachment = $request->file('attachment');
        if ($attachment) {
            $imageName6 = time() . '_' . uniqid() . '.' .  $attachment->getClientOriginalExtension();
            if ($attachment->move(public_path('assets/uploads'), $imageName6)) {
                $record->attachment = $imageName6;
            }
        }


        $record->save();

        if ($id) {
            Exemption_inventory_row::where('exemption_inventory_id', $id)->delete();
            $inventory_lines = $request->inventory_lines ?? [];

            foreach ($inventory_lines as $line) {
                if (!empty($line['category'])) {
                    Exemption_inventory_row::create([
                        'exemption_inventory_id' => $id,
                        'category' => $line['category'] ?? null,
                        'inventory_type' => $line['inventory_type'] ?? null,
                        'size_of_file' => $line['size_of_file'] ?? null,
                        'no_of_files' => $line['no_of_files'] ?? null,
                        'rate_file_plot' => $line['rate_file_plot'] ?? null,
                        'total_cost' => $line['total_cost'] ?? null,
                        'eighty_percent' => $line['eighty_percent'] ?? null,
                        'twenty_percent' => $line['twenty_percent'] ?? null,
                        'remark' => $line['remark'] ?? null,
                    ]);
                }
            }
        }

        return redirect()
            ->route('exemption_inventory.index')
            ->with('success', 'Exemption Inventory has been updated successfully.');
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy($id)
    {
        if (auth()->user()->exemption_inventory_delete == 1 || auth()->user()->is_admin == 1) {
            if ($id) {
                $record = Exemption_inventory_approval::find($id);
                $record->isDeleted = 1;
                $record->save();
                return redirect()->route('exemption_inventory.index')
                    ->with('success', 'Exemption Inventory Has Been Deleted successfully');
            } else {
                return redirect()->route('exemption_inventory.index')
                    ->with('danger', 'Exemption Inventory Not Found');
            }
        } else {
            return redirect()->route('exemption_inventory.index')
                ->with('danger', 'You are not authorized to delete this record');
        }
    }

    /**
     * Get land form details by doc_no (AJAX endpoint)
     */
    public function getLandFormDetails($doc_no)
    {
        // Initialize variables (IMPORTANT)
        $purchaseofland = null;
        $possessionCert = null;

        // Step 1: Get Land Form
        $landForm = Land_form::where('doc_no', $doc_no)
            ->where('isDeleted', 0)
            ->first();

        if (!$landForm) {
            return response()->json([
                'success' => false,
                'message' => 'Land form not found'
            ], 404);
        }

        // Step 2: Get Purchase of Land (Latest record)
        $purchaseofland = \DB::table('purchase_of_lands')
            ->where('isDeleted', 0)
            ->where('land_form_no', $doc_no)
            ->orderBy('id', 'desc')
            ->first();

        // Step 3: Get Possession Certificate - Try MULTIPLE lookup methods
        $possessionCert = null;
        
        if ($purchaseofland) {
            // Method 1: Direct match by File_No -> base_code_no
            $possessionCert = \DB::table('possession_certificates')
                ->where('isDeleted', 0)
                ->where('base_code_no', $purchaseofland->File_No)
                ->orderBy('id', 'desc')
                ->first();
            
            // Method 2: If not found, try matching via doc_no
            if (!$possessionCert) {
                $possessionCert = \DB::table('possession_certificates')
                    ->where('isDeleted', 0)
                    ->where('doc_no', $purchaseofland->File_No)
                    ->orderBy('id', 'desc')
                    ->first();
            }
            
            // Method 3: If still not found, get the LATEST possession certificate for this land form
            if (!$possessionCert) {
                $possessionCert = \DB::table('possession_certificates')
                    ->where('isDeleted', 0)
                    ->whereIn('base_code_no', function($query) use ($doc_no) {
                        $query->select('File_No')
                            ->from('purchase_of_lands')
                            ->where('isDeleted', 0)
                            ->where('land_form_no', $doc_no);
                    })
                    ->orderBy('id', 'desc')
                    ->first();
            }
        }

        // Step 4: Calculate Totals
        $totalRegisteredLand = 0;
        $totalPossessedLand = 0;

        if ($possessionCert) {
            $totalRegisteredLand = $possessionCert->total_land_acres ?? 0;
            $totalPossessedLand = $possessionCert->total_poss_acres ?? 0;
        } elseif ($purchaseofland) {
            // fallback if possession not found - use purchase land data
            $totalRegisteredLand = $purchaseofland->total_acre ?? 0;
            $totalPossessedLand = $purchaseofland->total_acre ?? 0;
        } else {
            // fallback from land form
            $totalRegisteredLand = $landForm->total_acre ?? 0;
            $totalPossessedLand = $landForm->total_acre ?? 0;
        }

        // Final Response
        return response()->json([
            'success' => true,
            'rate_per_acre' => $landForm->rate_per_acre ?? 0,
            'total_registered_land' => $totalRegisteredLand,
            'total_possessed_land' => $totalPossessedLand,
        ]);
    }
}
