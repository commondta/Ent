<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\DB;
class Document_approval_history extends Model
{
    use HasFactory;
    public static function get_recordss($whereCondition = null){
//        print_r($whereCondition);exit;
        if($whereCondition){
            return DB::table('document_approval_histories')
                ->leftJoin('users', 'document_approval_histories.approval_user_id', '=', 'users.id')
                ->select('document_approval_histories.*','users.name')
                ->where('document_approval_histories.isDeleted', 0)->where($whereCondition)->get();
        }
        return DB::table('document_approval_histories')->get();
    }
}
