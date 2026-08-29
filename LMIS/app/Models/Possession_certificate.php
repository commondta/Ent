<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\DB;

class Possession_certificate extends Model
{
    use HasFactory;
    protected $fillable = [
        'doc_no',
        'date',
        'base_code_no',
        'lo_name',
        'lo_father_name',
        'contact_no',
        'address',
        'cnic',
        
        'sq_feet',
        'marla',
        'kanal',
        'mouza',
        
        'possession_date',
        'possession_khewat_NO',
        'possession_mustatil_no',
        'possession_muraba_no',
        'possession_khasra_no',
        'possession_kanal',
        'possession_marla',
        'possession_sq_feet',
        'lp_name',
        
        'lp_contact_no',
        'lp_rep_name',
        'lp_possession_jpo',
        'picto_lo_name',
        'picto_lp_name',
        'picto_name_of_patwari',
        'picto_possession_jco',
        
        'picto_picture',
        'total_land_kanal',
        'total_land_marla',
        'total_land_sqft',
        'total_land_acres',
        'total_poss_kanal',
        'total_poss_marla',
        'total_poss_sqft',
        'total_poss_acres',
        'total_unposs_kanal',
        'total_unposs_marla',
        'total_unposs_sqft',
        'total_unposs_acres',
        'status',
        'isDeleted',
    ];
//    public function sellerProfile()
//    {
//
////        return   $results = DB::table('possession_certificates')
//            ->leftJoin('seller_profiles', 'possession_certificates.lo_name', '=', 'seller_profiles.lo_cod')
//            ->leftJoin('land_providers', 'possession_certificates.lp_name', '=', 'land_providers.lp_cod')
////            ->select('possession_certificate.*', 'seller_profile.lo_name as owner_Name', 'land_provider.lp_name as provider_Name')
////            ->get();
//        return $this->belongsTo(Seller_profile::class, 'lo_name');
//    }
    public static function get_records($whereCondition = null){
//        print_r($whereCondition);exit;
        if($whereCondition){
            return DB::table('possession_certificates')
                ->leftJoin('seller_profiles', 'possession_certificates.lo_name', '=', 'seller_profiles.lo_cod')
                ->leftJoin('land_providers', 'possession_certificates.lp_name', '=', 'land_providers.lp_cod')
                ->select('possession_certificates.*', 'seller_profiles.lo_name as owner_Name', 'land_providers.lp_name as provider_Name')
                ->where('possession_certificates.isDeleted', 0)->where($whereCondition)->first();
        }
        return DB::table('possession_certificates')->first();
    }
}
