<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\DB;


class Conveyance extends Model
{
    use HasFactory;
    protected $casts = [
    'fard_date' => 'date',
];
    public static function get_records($whereCondition = null){
//        print_r($whereCondition);exit;
        if($whereCondition){
            return DB::table('conveyances')
                ->leftJoin('conveyance_rows', 'conveyances.id', '=', 'conveyance_rows.deed_id')
                // ->leftJoin('conveyance_land_rows', 'conveyances.id', '=', 'conveyance_land_rows.deed_id')
                ->leftJoin('conveyance_land_fard_rows', 'conveyances.id', '=', 'conveyance_land_fard_rows.deed_id')
                ->select('conveyances.*', 'conveyance_rows.*', 'conveyance_land_fard_rows.*')
                ->where('conveyances.isDeleted', 0)->where($whereCondition)->first();
        }
        return DB::table('conveyances')->first();
    }
    
}
