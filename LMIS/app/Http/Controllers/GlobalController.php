<?php

namespace App\Http\Controllers;

use App\Models\Conveyance;
use App\Models\Exemption_form;
use App\Models\Int_application;
use App\Models\Land_form;
use App\Models\Land_provider;
use App\Models\Purchase_of_land;
use App\Models\Seller_profile;
use App\Models\Possession_certificate;
use Illuminate\Http\Request;

class GlobalController extends Controller
{
    public function get_seller_data(Request $request)
    {

        $id = $request->value;

        $data = Seller_profile::where('lo_cod',$id)->first();


        return response()->json($data);

    }
    public function get_land_form(Request $request)
    {
        $id = $request->value;

        $data = Land_form::where('doc_no', $id)->first();

        return response()->json($data);

    }
    public function get_purchase_of_land(Request $request)
    {
        $id = $request->value;

//        $data = Purchase_of_land::where('File_No',$id)->first();
        $data = Purchase_of_land::get_records(array('File_No' => $id));

        return response()->json($data);

    }
    public function getLandRate($doc_no)
{
    $land = \DB::table('land_forms')
        ->where('doc_no', $doc_no)
        ->select('rate_per_acre')
        ->first();

    if ($land) {
        return response()->json([
            'success' => true,
            'rate_per_acre' => $land->rate_per_acre
        ]);
    } else {
        return response()->json([
            'success' => false,
            'rate_per_acre' => 0
        ]);
    }
}

    public function get_conveyance_deed(Request $request)
    {
        $id = $request->value;

//        $data = Purchase_of_land::where('File_No',$id)->first();
        $data = Conveyance::get_records(array('conveyances.id' => $id));

        return response()->json($data);

    }public function get_land_provider(Request $request)
    {
        $id = $request->value;

        $data = Land_provider::where('lp_cod',$id)->first();


        return response()->json($data);

    }public function get_possession_record(Request $request)
    {
        $id = $request->value;

//        $data = Possession_certificate::where('doc_no',$id)->first();
        $data = Possession_certificate::get_records(array('possession_certificates.doc_no' => $id));
//        $data = Possession_certificate::with('sellerProfile')->where('doc_no', $id)->first();

        return response()->json($data);

    }
    public function get_intimation_application(Request $request)
    {
        $id = $request->value;

//        $data = Possession_certificate::where('doc_no',$id)->first();
        $data = Int_application::get_record(array('int_applications.doc_no' => $id));
//        $data = Possession_certificate::with('sellerProfile')->where('doc_no', $id)->first();

        return response()->json($data);

    }  public function get_exemption_form(Request $request)
    {
        $id = $request->value;

//        $data = Possession_certificate::where('doc_no',$id)->first();
        $data = Exemption_form::get_record(array('exemption_forms.doc_no' => $id));
//        $data = Possession_certificate::with('sellerProfile')->where('doc_no', $id)->first();

        return response()->json($data);

    }
    public function get_land_form_data(Request $request)
    {
        $id = $request->value;

        $data = Land_form::with('seller')->where('doc_no', $id)->first();

// Now you can access both the LandForm and its related Seller data
    if ($data) {
        // Access LandForm data
        $landFormData = $data->toArray();

//       echo '<pre>';  print_r($landFormData);exit;

        // Access Seller data related to the LandForm
        $sellerData = $data->seller->toArray();
    }

    return response()->json($data);
    }

    public function getLoDetails($doc_no)
    {
        $loRows = \DB::table('land_form_rows')
            ->leftJoin('land_forms', 'land_form_rows.land_form_id', '=', 'land_forms.id')
            ->where('land_forms.doc_no', $doc_no)
            ->select('land_form_rows.lo_name_as_per_cnic', 'land_form_rows.father_name_cnic', 'land_form_rows.lo_cnic', 'land_form_rows.contact_no')
            ->get();

        if ($loRows && count($loRows) > 0) {
            return response()->json([
                'success' => true,
                'data' => $loRows
            ]);
        } else {
            return response()->json([
                'success' => false,
                'data' => []
            ]);
        }
    }
    public function getpurchaseLoDetails($doc_no)
    {
        $loRows = \DB::table('purchase_of_land_lo_rows')
            ->leftJoin('purchase_of_lands', 'purchase_of_land_lo_rows.deed_id', '=', 'purchase_of_lands.id')
            ->where('purchase_of_lands.File_No', $doc_no)
            ->select('purchase_of_land_lo_rows.lo_name', 'purchase_of_land_lo_rows.so', 'purchase_of_land_lo_rows.lo_cnic', 'purchase_of_land_lo_rows.contact_no')
            ->get();

        if ($loRows && count($loRows) > 0) {
            return response()->json([
                'success' => true,
                'data' => $loRows
            ]);
        } else {
            return response()->json([
                'success' => false,
                'data' => []
            ]);
        }
    }

    public function getLandDetails($doc_no)
    {
        $landDetails = \DB::table('land_detail_rows')
            ->leftJoin('land_forms', 'land_detail_rows.land_form_id', '=', 'land_forms.id')
            ->where('land_forms.doc_no', $doc_no)
            ->select(
                'land_detail_rows.lo_cod',
                'land_detail_rows.khewat_no',
                'land_detail_rows.khatooni_no',
                'land_detail_rows.qatat',
                'land_detail_rows.measuring_k',
                'land_detail_rows.measuring_m',
                'land_detail_rows.measuring_sqft',
                'land_detail_rows.transfer_share',
                'land_detail_rows.land_measuring_k',
                'land_detail_rows.land_measuring_m',
                'land_detail_rows.land_measuring_sqft',
                'land_detail_rows.land_category'
            )
            ->get();

        if ($landDetails && count($landDetails) > 0) {
            return response()->json([
                'success' => true,
                'data' => $landDetails
            ]);
        } else {
            return response()->json([
                'success' => false,
                'data' => []
            ]);
        }
    }
    public function getpurchaseLandDetails($doc_no)
    {
        $landDetails = \DB::table('purchase_of_land_rows')
            ->leftJoin('purchase_of_lands', 'purchase_of_land_rows.deed_id', '=', 'purchase_of_lands.id')
            ->where('purchase_of_lands.File_No', $doc_no)
            ->select(
                'purchase_of_land_rows.khewat_no',
                'purchase_of_land_rows.khatooni_no',
                'purchase_of_land_rows.qatat',
                'purchase_of_land_rows.measuring_k',
                'purchase_of_land_rows.measuring_m',
                'purchase_of_land_rows.measuring_sqft',
                'purchase_of_land_rows.transfer_share',
                'purchase_of_land_rows.land_measuring_k',
                'purchase_of_land_rows.land_measuring_m',
                'purchase_of_land_rows.land_measuring_sqft',
                'purchase_of_land_rows.land_category'
            )
            ->get();

        if ($landDetails && count($landDetails) > 0) {
            return response()->json([
                'success' => true,
                'data' => $landDetails
            ]);
        } else {
            return response()->json([
                'success' => false,
                'data' => []
            ]);
        }
    }

    public function approval_inbox(){
        return view('pages.approvals.setup.edit');

    }


}
