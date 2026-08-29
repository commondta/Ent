<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Land_form_row extends Model
{
    use HasFactory;

    protected $table = 'land_form_rows';

    protected $fillable = [
        'land_form_id',
        'lo_cod',
        'lo_name',
        'relationship_revenue',
        'so',
        'lo_name_as_per_cnic',
        'relationship_cnic',
        'father_name_cnic',
        'lo_cnic',
        'caste',
        'contact_no',
        'address',
    ];

    public function land_form()
    {
        return $this->belongsTo(Land_form::class, 'land_form_id');
    }

    public function seller()
    {
        return $this->belongsTo(Seller_profile::class, 'lo_cod', 'lo_cod');
    }
}
