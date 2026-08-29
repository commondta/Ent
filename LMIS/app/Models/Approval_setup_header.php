<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;

class Approval_setup_header extends Model
{
    use HasFactory;


    public static function get_records($whereCondition = null){
        if($whereCondition){
            return DB::table('approval_setup_headers')
                ->leftJoin('approval_setup_lines', 'approval_setup_headers.id', '=', 'approval_setup_lines.main')
                ->select('approval_setup_headers.id','approval_setup_headers.*', 'approval_setup_lines.*')
                ->where('approval_setup_headers.isDeleted', 0)->where($whereCondition)->first();
        }
        return DB::table('approval_setup_headers')->first();
    }
    public static function get_recordss($whereCondition = null){
//        print_r($whereCondition);exit;
        if($whereCondition){
            return DB::table('approval_setup_headers h')
                ->leftJoin('approval_setup_lines f', 'h.id', '=', 'f.challan_header_id')
                ->select('h.*', 'f.*')
                ->where('h.isDeleted', 0)->where($whereCondition)->get();
        }
        return DB::table('approval_setup_headers')->get();
    }
}
