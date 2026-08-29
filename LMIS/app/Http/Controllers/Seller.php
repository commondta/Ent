<?php

namespace App\Http\Controllers;

use Illuminate\Support\Str;
use App\Models\Land_provider;
use App\Models\Seller_profile;
use App\Models\Sellere_profile_land_row;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Http\Request;

class Seller extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if (auth()->user()->seller_profile_list == 1) {
            $data['record'] = Seller_profile::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            return view('pages.purchasing_of_land.seller_profile.show', $data);
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
        if (auth()->user()->seller_profile_add == 1) {
            $data['record'] = Land_provider::where('isDeleted', 0)->where('status', 0)->orderBy('id', 'desc')->get();
            $data['doc_num']  = (int) Seller_profile::latest('id')->value('id') ?? 0;
            $data['lo_code']  = (int) Seller_profile::latest('id')->value('id') ?? 0;
            return view('pages.purchasing_of_land.seller_profile.add', $data);
        } else {
            return view('pages.authrization.show');
        }
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        $line_items = $request->item_lines;

        $request->validate([
            'lo_cod' => 'required',
            'doc_no' => 'required|unique:seller_profiles,doc_no',
            
            'relationship_cnic' => 'required',
            'lo_name_as_per_cnic' => 'required',
            'father_name_cnic' => 'required',
           
            //            'rectangle' => 'required',
            //            'khasra' => 'required',
            //            'muraba' => 'required',
            //            'marla' => 'required',
            //            'kanal' => 'required',
            //            'sq_feet' => 'required',
            //          'lp_code' => 'required',
            'lo_cnic' => 'required',
            'contact_no' => 'required',
            'caste' => 'required',
            'address' => 'required',
            
        ]);
        $Seller_profiles = new Seller_profile();
        $Seller_profiles->lo_cod = $request->lo_cod;
        $Seller_profiles->doc_no = $request->doc_no;
        $Seller_profiles->lo_name = $request->lo_name;
        $Seller_profiles->relationship_revenue = $request->relationship_revenue;
        $Seller_profiles->lo_father_name = $request->lo_father_name;
        $Seller_profiles->lo_name_as_per_cnic = $request->lo_name_as_per_cnic;
        $Seller_profiles->relationship_cnic = $request->relationship_cnic;
        $Seller_profiles->father_name_cnic = $request->father_name_cnic;
        //        $Seller_profiles->rectangle = $request->rectangle;
        //        $Seller_profiles->khasra = $request->khasra;
        //        $Seller_profiles->muraba = $request->muraba;
        //        $Seller_profiles->marla = $request->marla;
        //        $Seller_profiles->kanal = $request->kanal;
        //        $Seller_profiles->sq_feet = $request->sq_feet;
        //        $Seller_profiles->lp_code = $request->lp_code;
        //        $Seller_profiles->lp_name = $request->lp_name;
        $Seller_profiles->lo_cnic = $request->lo_cnic;
        $Seller_profiles->contact_no = $request->contact_no;
        $Seller_profiles->caste = $request->caste;
        $Seller_profiles->createdBy = auth()->user()->id;

        //        $Seller_profiles->so = $request->so;
        $Seller_profiles->address = $request->address;
        $Seller_profiles->tem_address = $request->tem_address;
        if ($request->hasFile('attachments')) {
            $image = $request->file('attachments');
            $imageName = 'profile_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $Seller_profiles->attachment = $imageName;
        }
        
        if ($request->hasFile('cnic_front_attachments')) {
            $image = $request->file('cnic_front_attachments');
            $imageName = 'cnic_front_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $Seller_profiles->cnic_front_attachments = $imageName;
        }
        if ($request->hasFile('cnic_back_attachments')) {
            $image = $request->file('cnic_back_attachments');
            $imageName = 'cnic_back_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $Seller_profiles->cnic_back_attachments = $imageName;
        }

        $approval_check = Approval_setup_header::where('approval', 'Seller Profile')->first();

        if ($approval_check) {
            $Seller_profiles->status = 1;
        } else {
            $Seller_profiles->status = 0;
        }

        $Seller_profiles->save();

        $lastid = $Seller_profiles->id;

        // if ($lastid) {
        //     $line_items = $request->item_lines;
        //     if ($line_items) {
        //         foreach ($line_items as $line_item) {
        //             if ($line_item['khewat_no']) {

        //                 $Sellere_profile_land_row = new Sellere_profile_land_row();
        //                 $Sellere_profile_land_row->deed_id = $lastid;
        //                 $Sellere_profile_land_row->khewat_no = $line_item['khewat_no'];
        //                 $Sellere_profile_land_row->khatooni_no = $line_item['khatooni_no'];
        //                 $Sellere_profile_land_row->rectangle_no = $line_item['rectangle_no'];
        //                 $Sellere_profile_land_row->muraba_no = $line_item['muraba_no'];
        //                 $Sellere_profile_land_row->khasra_no = $line_item['khasra_no'];
        //                 $Sellere_profile_land_row->kanal = $line_item['kanal'];
        //                 $Sellere_profile_land_row->marla = $line_item['marla'];
        //                 $Sellere_profile_land_row->sq_feet = $line_item['sq_feet'];
        //                 $Sellere_profile_land_row->save();
        //             }
        //         }
        //     }
        // }


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



            return redirect()->route('seller_profile.index')
                ->with('success', 'The Seller Profile record sent for approval.');
        } else {
            return redirect()->route('seller_profile.index')
                ->with('success', 'Seller Profile has been created successfully.');
        }

        //        return redirect()->route('seller_profile.index')
        //            ->with('success', 'Seller Profile has been created successfully.');
    }

    /**
     * Display the specified resource.
     *
     * @param  int $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        //
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int $id
     * @return \Illuminate\Http\Response
     */
    public function edit(Seller_profile $Seller_profile)
    {
        if (auth()->user()->seller_profile_edit == 1) {

            $id = $Seller_profile->id;
            $rows = Sellere_profile_land_row::where('deed_id', $id)->get();
            $Seller_profile['rows'] =  $rows->toArray();
            $data['record'] = Land_provider::where('isDeleted', 0)->orderBy('id', 'desc')->get();
            return view('pages.purchasing_of_land.seller_profile.edit', compact('Seller_profile'), $data);
        } else {
            return view('pages.authrization.show');
        }
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request $request
     * @param  int $id
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, $id)
    {
        $request->validate([
            'lo_cod' => 'required',
            'doc_no' => 'required',
           
            'relationship_cnic' => 'required',
            'lo_name_as_per_cnic' => 'required',
            'father_name_cnic' => 'required',
            //  'lp_code' => 'required',
            'lo_cnic' => 'required',
            'contact_no' => 'required',
            'caste' => 'required',
            'address' => 'required',
        ]);
        $Seller_profiles = Seller_profile::find($id);
        $Seller_profiles->lo_cod = $request->lo_cod;
        $Seller_profiles->doc_no = $request->doc_no;
        $Seller_profiles->lo_name = $request->lo_name;
        $Seller_profiles->relationship_revenue = $request->relationship_revenue;
        $Seller_profiles->relationship_cnic = $request->relationship_cnic;
        $Seller_profiles->lo_name_as_per_cnic = $request->lo_name_as_per_cnic;
        $Seller_profiles->father_name_cnic = $request->father_name_cnic;
        $Seller_profiles->lo_father_name = $request->lo_father_name;
        //        $Seller_profiles->rectangle = $request->rectangle;
        //        $Seller_profiles->khasra = $request->khasra;
        //        $Seller_profiles->muraba = $request->muraba;
        //        $Seller_profiles->marla = $request->marla;
        //        $Seller_profiles->kanal = $rezzzzquest->kanal;
        //        $Seller_profiles->sq_feet = $request->sq_feet;
        //  $Seller_profiles->lp_code = $request->lp_code;
        //        $Seller_profiles->lp_name = $request->lp_name;
        $Seller_profiles->lo_cnic = $request->lo_cnic;
        $Seller_profiles->contact_no = $request->contact_no;
        $Seller_profiles->caste = $request->caste;
        $Seller_profiles->address = $request->address;
        $Seller_profiles->tem_address = $request->tem_address;

        if ($request->hasFile('attachments')) {
            $image = $request->file('attachments');
            $imageName = 'profile_' . Str::uuid() . '.' . $image->getClientOriginalExtension();
            $image->move(public_path('assets/uploads'), $imageName);
            $Seller_profiles->attachment = $imageName;
        }
        

        if ($request->hasFile('cnic_front_attachments')) {

            $image = $request->file('cnic_front_attachments');
            $imageName = 'cnic_front_' . Str::uuid() . '.' . $image->getClientOriginalExtension();

            $image->move(public_path('assets/uploads'), $imageName);

            $Seller_profiles->cnic_front_attachments = $imageName;
        }


        if ($request->hasFile('cnic_back_attachments')) {

            $image = $request->file('cnic_back_attachments');
            $imageName = 'cnic_back_' . Str::uuid() . '.' . $image->getClientOriginalExtension();

            $image->move(public_path('assets/uploads'), $imageName);

            $Seller_profiles->cnic_back_attachments = $imageName;
        }

        $Seller_profiles->save();



        // $line_items = $request->item_lines;
        // foreach ($line_items as $line_item) {
        //     if ($line_item['khewat_no']) {

        //         if (isset($line_item['id'])) {
        //             $Sellere_profile_land_row = Sellere_profile_land_row::find($line_item['id']);
        //         } else {
        //             $Sellere_profile_land_row = new Sellere_profile_land_row();
        //         }


        //         //                $Sellere_profile_land_row->deed_id = $id;
        //         $Sellere_profile_land_row->khewat_no = $line_item['khewat_no'];
        //         $Sellere_profile_land_row->khatooni_no = $line_item['khatooni_no'];
        //         $Sellere_profile_land_row->rectangle_no = $line_item['rectangle_no'];
        //         $Sellere_profile_land_row->muraba_no = $line_item['muraba_no'];
        //         $Sellere_profile_land_row->khasra_no = $line_item['khasra_no'];
        //         $Sellere_profile_land_row->kanal = $line_item['kanal'];
        //         $Sellere_profile_land_row->marla = $line_item['marla'];
        //         $Sellere_profile_land_row->sq_feet = $line_item['sq_feet'];
        //         $Sellere_profile_land_row->save();
        //     }
        // }

        return redirect()->route('seller_profile.index')
            ->with('success', 'Seller Profile has been Updated successfully.');
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int $id
     * @return \Illuminate\Http\Response
     */
    public function destroy($id)
    {
        if ($id) {

            $company = Seller_profile::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('seller_profile.index')
                ->with('success', 'Seller Profile Has Been Deleted successfully');
        } else {
            return redirect()->route('seller_profile.index')
                ->with('danger', 'Seller Profile Not Found');
        }
    }
}
