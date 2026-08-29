<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\DB;


class Int_application extends Model
{
    use HasFactory;
    public static function get_record($whereCondition = null){
//        print_r($whereCondition);exit;
        if($whereCondition){
            return DB::table('int_applications')
                ->leftJoin('seller_profiles', 'int_applications.lo_code', '=', 'seller_profiles.lo_cod')
                ->select('int_applications.*', 'seller_profiles.address as lo_address')
                ->where('int_applications.isDeleted', 0)->where($whereCondition)->first();
        }
        return DB::table('int_applications')->first();
    }
}
