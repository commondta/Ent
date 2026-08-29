<?php

namespace App\Http\Controllers;

use App\Models\Challan_form_header;
use App\Models\Challan_form_footer;
use App\Models\Seller_profile;
use App\Models\Challan_fee;
use App\Models\Approval_setup_header;
use App\Models\Approval_setup_line;
use App\Models\Document_approval;
use Illuminate\Http\Request;
use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;
class Challan_form extends MY_Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if(auth()->user()->challan_form_list == 1){
            $data['record'] = Challan_form_header::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            return view('pages.purchasing_of_land.challan_form.show', $data);
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
        if(auth()->user()->challan_form_add == 1){
            $data['challan_no']  = Challan_form_header::latest('id')->value('id');
            $data['seller_profiles'] = Seller_profile::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
            $data['challan_fee'] = Challan_fee::where('isDeleted',0)->where('status',0)->orderBy('id','desc')->get();
//            print_r($data['seller_profiles']);exit;

            return view('pages.purchasing_of_land.challan_form/add',$data);
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
            'challan_no' => 'required',
            'seller_name' => 'required',
            'amount' => 'required',
        ]);
        $record = new Challan_form_header();
        $record->date = $request->date;
        $record->challan_no = $request->challan_no;
        $record->seller_id = $request->seller_id;
        $record->seller_name = $request->seller_name;
        $record->seller_cnic = $request->seller_cnic;
        $record->amount = $request->amount;


        $approval_check = Approval_setup_header::where('approval', 'Challan Form')->first();

        if($approval_check){
            $record->status = 1;
        }else{
            $record->status = 0;

        }

        $record->save();
        $lastInsertedId = $record->id;
        if($lastInsertedId){
            $line_items = $request->challan_form_row;
            foreach ($line_items as $line_item) {
                if ($line_item['challan_type']) {

                    $Challan_form_footer = new Challan_form_footer();
                    $Challan_form_footer->challan_header_id = $lastInsertedId;
                    $Challan_form_footer->challan_type = $line_item['challan_type'];
                    $Challan_form_footer->amount = $line_item['amount'];
                    $Challan_form_footer->save();
                }
            }

        }

        if($approval_check){
            $count = 1;
            $Approval_setup_lines = Approval_setup_line::where('isDeleted', 0)->where('main', $approval_check->id)->get();
            foreach($Approval_setup_lines as $Approval_setup_line){
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

            return redirect()->route('challan_form.index')
                ->with('success','The Challan Form record sent for approval.');
        }else{
            return redirect()->route('challan_form.index')
                ->with('success','Challan Form has been created successfully.');
        }










//        return redirect()->route('challan_form.index')
//            ->with('success', 'Challan Form has been created successfully.');
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        if (auth()->user()->challan_form_print == 1) {
            $data['header'] = Challan_form_header::get_records(['challan_form_headers.id' => $id]);

            if ($data['header']->id) {
                $id = $data['header']->challan_header_id;
                $rows = Challan_form_footer::where('challan_header_id', $id)->get();

                $data['header']->rows = $rows->toArray();

                $data['seller_profiles'] = Seller_profile::where('isDeleted',0)->orderBy('id','desc')->get();
                $data['challan_fee'] = Challan_fee::where('isDeleted',0)->orderBy('id','desc')->get();


                return view('pages.purchasing_of_land.challan_form.print', $data);
            }
        }
        else {
            return view('pages.authrization.show');
        }
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit($id)
    {
        if (auth()->user()->challan_form_add == 1) {
            $data['header'] = Challan_form_header::get_records(['challan_form_headers.id' => $id]);
            if ($data['header']->id) {
                $id = $data['header']->challan_header_id;

                $rows = Challan_form_footer::where('challan_header_id', $id)->get();

                $data['header']->rows = $rows->toArray();

                $data['seller_profiles'] = Seller_profile::where('isDeleted',0)->orderBy('id','desc')->get();
                $data['challan_fee'] = Challan_fee::where('isDeleted',0)->orderBy('id','desc')->get();
                return view('pages.purchasing_of_land.challan_form.edit', $data);
            }
        }
        else {
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
            'challan_no' => 'required',
            'seller_name' => 'required',
            'amount' => 'required',
        ]);

        $record = Challan_form_header::find($id);
        $record->date = $request->date;
        $record->challan_no = $request->challan_no;
        $record->seller_id = $request->seller_id;
        $record->seller_name = $request->seller_name;
        $record->seller_cnic = $request->seller_cnic;
        $record->createdBy =auth()->user()->id;

        $record->amount = $request->amount;


        $record->save();

        if($id){

            $lastInsertedId = $record->id;
            if($lastInsertedId){
                $line_items = $request->challan_form_row;
                foreach ($line_items as $line_item) {
                    if ($line_item['challan_type']) {

                        if(isset($line_item['id'])){
                            $Challan_form_footer = Challan_form_footer::find($id);
//                            $Challan_form_footer->challan_header_id = $id;
                            $Challan_form_footer->challan_type = $line_item['challan_type'];
                            $Challan_form_footer->amount = $line_item['amount'];
                            $Challan_form_footer->save();
                        }else{
                            $Challan_form_footer = new Challan_form_footer();
                            $Challan_form_footer->challan_header_id = $id;
                            $Challan_form_footer->challan_type = $line_item['challan_type'];
                            $Challan_form_footer->amount = $line_item['amount'];
                            $Challan_form_footer->save();
                        }


                    }
                }

            }

            return redirect()->route('challan_form.index')
                ->with('success', 'Challan Form has been Updated successfully.');

        }


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

            $company = Challan_form_header::find($id);
            $company->isDeleted = 1;
            $company->save();
            return redirect()->route('challan_form.index')
                ->with('success', 'Challan Form Has Been Deleted successfully');
        } else {
            return redirect()->route('challan_form.index')
                ->with('danger', 'Challan Form Not Found');
        }
    }
}
