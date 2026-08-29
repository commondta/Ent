<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\DB; // Correct import for DB facade

class Challan_form_header extends Model
{
    use HasFactory;
    public static function get_records($whereCondition = null){
//        print_r($whereCondition);exit;
        if($whereCondition){
            return DB::table('challan_form_headers')
                ->leftJoin('challan_form_footers', 'challan_form_headers.id', '=', 'challan_form_footers.challan_header_id')
                ->select('challan_form_headers.*', 'challan_form_headers.id', 'challan_form_footers.*')
                ->where('challan_form_headers.isDeleted', 0)->where($whereCondition)->first();
        }
        return DB::table('challan_form_headers')->first();
    }
    public static function get_recordss($whereCondition = null){
//        print_r($whereCondition);exit;
        if($whereCondition){
            return DB::table('challan_form_headers h')
                ->leftJoin('challan_form_footers f', 'h.id', '=', 'f.challan_header_id')
                ->select('h.*', 'f.*')
                ->where('h.isDeleted', 0)->where($whereCondition)->get();
        }
        return DB::table('challan_form_headers')->get();
    }
}
