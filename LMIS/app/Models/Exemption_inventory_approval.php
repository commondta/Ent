<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Exemption_inventory_approval extends Model
{
    use HasFactory;

    protected $table = 'exemption_inventory_approvals';

    protected $fillable = [
        'doc_no',
        'date',
        'land_offer_form_no',
        'total_registered_land',
        'total_possessed_land',
        'rate_per_acre',
        'total_cost_registered',
        'total_cost_possessed',
        'total_residential_files',
        'total_commercial_files',
        'total_marlas',
        'exemption_percent',
        'total_cost',
        'residential_percent',
        'commercial_percent',
        'cash',
        'inv_decimal',
        'remarks',
        'attachment',
        'status',
        'isDeleted',
        'createdBy',
    ];

    public function rows()
    {
        return $this->hasMany(Exemption_inventory_row::class, 'exemption_inventory_id');
    }
}
