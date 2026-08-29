<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\DB;


class Purchase_of_land extends Model
{
    use HasFactory;
    protected $casts = [
    ];
    public static function get_records($whereCondition = null)
    {
        $query = self::query()
           // ->leftJoin('seller_profiles', 'purchase_of_lands.lo_name', '=', 'seller_profiles.lo_name')
            ->select('purchase_of_lands.*')
            ->where('purchase_of_lands.isDeleted', 0);

        if ($whereCondition) {
            $query->where($whereCondition);
        }

        return $query->first();
    }
}
