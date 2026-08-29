<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Seller_profile extends Model
{
    use HasFactory;

    public function landForms()
    {
        // Assuming there's a foreign key 'seller_id' in the Land_form table
        return $this->hasMany(Land_form::class, 'lo_cod');
    }
    public function possessionCertificates()
    {
        return $this->hasMany(Possession_certificate::class, 'lo_name');
    }
    public static function get_recordss($whereCondition = null){
//        print_r($whereCondition);exit;
        if($whereCondition){
            return DB::table('seller_profiles')
                ->leftJoin('sellere_profile_land_rows', 'seller_profiles.id', '=', 'sellere_profile_land_rows.deed_id')
                ->select('*')
                ->where('seller_profiles.isDeleted', 0)->where($whereCondition)->get();
        }
        return DB::table('seller_profiles')->get();
    }
}
