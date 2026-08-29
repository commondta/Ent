<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Approval_setup_line extends Model
{
    use HasFactory;
    public static function get_recordss($whereCondition = null){
//        print_r($whereCondition);exit;
        if($whereCondition){
            return DB::table('approval_setup_lines')
                ->select('*')
               ->where($whereCondition)->get();
        }
        return DB::table('approval_setup_lines')->get();
    }
}
