<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\DB;


class Exemption_form extends Model
{
    use HasFactory;
    public static function get_record($whereCondition = null){
//        print_r($whereCondition);exit;
        if($whereCondition){
            return DB::table('exemption_forms')
                ->leftJoin('seller_profiles', 'exemption_forms.lo_code', '=', 'seller_profiles.lo_cod')
                ->leftJoin('land_providers', 'exemption_forms.lp_name', '=', 'land_providers.lp_name')
                ->select('exemption_forms.*', 'land_providers.lp_cnic', 'seller_profiles.lo_cnic', 'land_providers.address as lo_address')
                ->where('exemption_forms.isDeleted', 0)->where($whereCondition)->first();
        }
        return DB::table('exemption_forms')->first();
    }
}
