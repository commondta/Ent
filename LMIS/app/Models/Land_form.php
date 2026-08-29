<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Land_form extends Model
{
    use HasFactory;

    protected $fillable = [
        'doc_date',
        'doc_no',
        'total_kanal',
        'total_marla',
        'total_sqft',
        'total_acre',
        'mouza',
        'sector',
        'tehsil',
        'district',
        'rate_per_acre',
        'poa_lo_code',
        'poa_name',
        'relationship',
        'poa_father_name',
        'poa_cnic',
        'poa_caste',
        'poa_current_address',
        'poa_permanent_address',
        'poa_remarks',
        'status',
        'createdBy',
        'isDeleted'
    ];

    public function seller()
    {
        // Assuming there's a foreign key 'seller_id' in the Land_form table
        return $this->belongsTo(Seller_profile::class, 'lo_cod');
    }

    public function rows()
    {
        return $this->hasMany(Land_form_row_detail::class, 'land_form_id');
    }

    public function lo_lines()
    {
        return $this->hasMany(Land_form_row::class, 'land_form_id');
    }

}
